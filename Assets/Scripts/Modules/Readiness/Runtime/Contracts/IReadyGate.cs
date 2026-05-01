namespace Modules.Readiness.Runtime.Contracts
{
    public interface IReadyGate : IReadiness
    {
        void Open();
    }
}