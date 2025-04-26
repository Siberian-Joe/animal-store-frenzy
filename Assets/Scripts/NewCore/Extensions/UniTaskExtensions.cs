using System;
using Cysharp.Threading.Tasks;
using R3;

namespace NewCore.Extensions
{
    public static class UniTaskExtensions
    {
        public static async UniTask<T> AddTo<T>(this UniTask<T> task, CompositeDisposable composite)
            where T : IDisposable
        {
            var result = await task;
            composite.Add(result);
            return result;
        }
    }
}