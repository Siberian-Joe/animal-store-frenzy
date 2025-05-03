using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using NewCore.Domain;
using R3;
using UnityEngine;

namespace NewCore.Services.Storage
{
    public sealed class PlayerPrefsStorage : IStorage
    {
        public UniTask<Result<bool>> ExistsAsync(string key, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                return UniTask.FromCanceled<Result<bool>>(cancellationToken);

            try
            {
                var exists = PlayerPrefs.HasKey(key);
                return UniTask
                    .FromResult(Result<bool>.Ok(exists))
                    .AttachExternalCancellation(cancellationToken);
            }
            catch (Exception exception)
            {
                return UniTask
                    .FromResult(Result<bool>.Fail(new DataError(exception)))
                    .AttachExternalCancellation(cancellationToken);
            }
        }

        public UniTask<Result<TModel>> LoadAsync<TModel>(string key, CancellationToken cancellationToken = default)
            where TModel : IModel
        {
            if (cancellationToken.IsCancellationRequested)
                return UniTask.FromCanceled<Result<TModel>>(cancellationToken);

            try
            {
                var json = PlayerPrefs.GetString(key);
                var data = JsonUtility.FromJson<TModel>(json);
                if (data == null)
                    return UniTask.FromResult(
                            Result<TModel>.Fail(new DataError($"Deserialization failed for '{key}'")))
                        .AttachExternalCancellation(cancellationToken);

                return UniTask.FromResult(Result<TModel>.Ok(data))
                    .AttachExternalCancellation(cancellationToken);
            }
            catch (Exception exception)
            {
                return UniTask.FromResult(
                        Result<TModel>.Fail(new DataError(exception)))
                    .AttachExternalCancellation(cancellationToken);
            }
        }

        public UniTask<Result<Unit>> SaveAsync<T>(string key, T data, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                return UniTask.FromCanceled<Result<Unit>>(cancellationToken);

            try
            {
                var json = JsonUtility.ToJson(data, prettyPrint: true);
                PlayerPrefs.SetString(key, json);
                PlayerPrefs.Save();

                return UniTask.FromResult(Result.Ok()).AttachExternalCancellation(cancellationToken);
            }
            catch (Exception exception)
            {
                return UniTask.FromResult(Result.Fail(new DataError(exception)))
                    .AttachExternalCancellation(cancellationToken);
            }
        }
    }
}