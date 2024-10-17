namespace Services
{
    public interface IObjectRegistrationService
    {
        void Register(IInteractiveObject interactiveObject);
        void Unregister(IInteractiveObject interactiveObject);
    }

    public interface IObjectQueryService
    {
        bool Has<T>() where T : IInteractiveObject;
        bool TryFindNearest<T>(IInteractiveObject from, out T nearest) where T : IInteractiveObject;
    }
}