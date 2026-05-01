using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.Startup.Contracts;
using Debug = UnityEngine.Debug;

namespace Modules.Startup.Runtime
{
    public sealed class StartupPipeline<TTask>
        where TTask : IStartupTask
    {
        private readonly IReadOnlyList<StartupTaskDescriptor> _plan;

        public StartupPipeline(
            IEnumerable<TTask> tasks,
            IStartupTaskDescriptorResolver descriptorResolver)
        {
            if (tasks == null)
                throw new ArgumentNullException(nameof(tasks));

            if (descriptorResolver == null)
                throw new ArgumentNullException(nameof(descriptorResolver));

            _plan = tasks
                .Select(task => descriptorResolver.Resolve(task))
                .OrderBy(descriptor => descriptor.Phase)
                .ThenBy(descriptor => descriptor.Order)
                .ThenBy(descriptor => descriptor.Name, StringComparer.Ordinal)
                .ThenBy(descriptor => descriptor.TaskType.FullName ?? string.Empty, StringComparer.Ordinal)
                .ToArray();
        }

        public async UniTask<StartupRunReport> RunAsync(CancellationToken token)
        {
            var results = new List<StartupTaskExecutionResult>(_plan.Count);
            var stop = false;

            foreach (var bucket in _plan.GroupBy(x => (x.Phase, x.Order)))
            {
                token.ThrowIfCancellationRequested();

                var entries = bucket.ToArray();

                if (stop)
                {
                    AppendSkipped(entries, results);
                    continue;
                }

                var bucketResults = await RunBucketAsync(entries, token);
                results.AddRange(bucketResults);

                if (bucketResults.Any(IsCriticalFailure))
                    stop = true;
            }

            return new StartupRunReport(results);
        }

        private static async UniTask<IReadOnlyList<StartupTaskExecutionResult>> RunBucketAsync(
            IReadOnlyList<StartupTaskDescriptor> entries,
            CancellationToken token)
        {
            if (entries == null || entries.Count == 0)
                return Array.Empty<StartupTaskExecutionResult>();

            ValidateEntries(entries);

            var executionMode = entries[0].ExecutionMode;

            if (executionMode == StartupExecutionMode.Sequential)
                return await RunSequentialAsync(entries, token);

            return await RunParallelAsync(entries, token);
        }

        private static async UniTask<IReadOnlyList<StartupTaskExecutionResult>> RunSequentialAsync(
            IReadOnlyList<StartupTaskDescriptor> entries,
            CancellationToken token)
        {
            await UniTask.SwitchToMainThread(token);

            var results = new List<StartupTaskExecutionResult>(entries.Count);

            for (var index = 0; index < entries.Count; index++)
            {
                var result = await RunEntryAsync(entries[index], token);
                results.Add(result);

                LogFailure(result);

                if (IsCriticalFailure(result) == false)
                    continue;

                AppendSkipped(entries, results, index + 1);
                break;
            }

            return results;
        }

        private static async UniTask<IReadOnlyList<StartupTaskExecutionResult>> RunParallelAsync(
            IReadOnlyList<StartupTaskDescriptor> entries,
            CancellationToken token)
        {
            var tasks = entries
                .Select(entry => RunEntryAsync(entry, token))
                .ToArray();

            var results = await UniTask.WhenAll(tasks);

            await UniTask.SwitchToMainThread(token);

            foreach (var result in results)
                LogFailure(result);

            return results;
        }

        private static async UniTask<StartupTaskExecutionResult> RunEntryAsync(
            StartupTaskDescriptor entry,
            CancellationToken token)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                if (entry.ExecutionMode == StartupExecutionMode.ParallelBackground)
                {
                    await UniTask.SwitchToThreadPool();
                    token.ThrowIfCancellationRequested();
                }

                await entry.Task.ExecuteAsync(token);

                stopwatch.Stop();

                return new StartupTaskExecutionResult(
                    entry.Name,
                    entry.TaskType,
                    entry.Phase,
                    entry.Order,
                    entry.FailurePolicy,
                    StartupTaskExecutionStatus.Succeeded,
                    stopwatch.Elapsed);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception exception)
            {
                stopwatch.Stop();

                return new StartupTaskExecutionResult(
                    entry.Name,
                    entry.TaskType,
                    entry.Phase,
                    entry.Order,
                    entry.FailurePolicy,
                    StartupTaskExecutionStatus.Failed,
                    stopwatch.Elapsed,
                    exception);
            }
        }

        private static void ValidateEntries(IReadOnlyList<StartupTaskDescriptor> entries)
        {
            var expectedMode = entries[0].ExecutionMode;
            var phase = entries[0].Phase;
            var order = entries[0].Order;

            foreach (var entry in entries)
            {
                if (entry.ExecutionMode != expectedMode)
                {
                    throw new InvalidOperationException(
                        $"Startup bucket '{phase}:{order}' mixes execution modes. " +
                        "All tasks inside the same Phase/Order bucket must use the same execution mode.");
                }

                if (entry.ExecutionMode == StartupExecutionMode.ParallelBackground &&
                    entry.Phase != StartupPhase.Preparation)
                {
                    throw new InvalidOperationException(
                        $"Startup task '{entry.TaskType.FullName}' uses {nameof(StartupExecutionMode.ParallelBackground)} " +
                        $"outside of {nameof(StartupPhase.Preparation)}. " +
                        "Parallel background execution is allowed only during preparation.");
                }
            }
        }

        private static void AppendSkipped(
            IReadOnlyList<StartupTaskDescriptor> entries,
            List<StartupTaskExecutionResult> results,
            int startIndex = 0)
        {
            for (var index = startIndex; index < entries.Count; index++)
            {
                var entry = entries[index];

                results.Add(new StartupTaskExecutionResult(
                    entry.Name,
                    entry.TaskType,
                    entry.Phase,
                    entry.Order,
                    entry.FailurePolicy,
                    StartupTaskExecutionStatus.Skipped,
                    TimeSpan.Zero));
            }
        }

        private static void LogFailure(StartupTaskExecutionResult result)
        {
            if (result.Status != StartupTaskExecutionStatus.Failed || result.Exception == null)
                return;

            Debug.LogException(result.Exception);
        }

        private static bool IsCriticalFailure(StartupTaskExecutionResult result) =>
            result is
            {
                Status: StartupTaskExecutionStatus.Failed,
                FailurePolicy: StartupFailurePolicy.Critical
            };
    }
}