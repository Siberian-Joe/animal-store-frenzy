using R3;

namespace NewCore.Services
{
    public static class Result
    {
        public static Result<Unit> Ok() => Result<Unit>.Ok(Unit.Default);
        public static Result<Unit> Fail(DataError error) => Result<Unit>.Fail(error);
    }

    public readonly struct Result<T>
    {
        public bool IsSuccess { get; }
        public T Value { get; }
        public DataError Error { get; }

        private Result(bool isSuccess, T value, DataError error) =>
            (IsSuccess, Value, Error) = (isSuccess, value, error);

        public static Result<T> Ok(T value) => new(true, value, default);
        public static Result<T> Fail(DataError error) => new(false, default, error);
    }
}