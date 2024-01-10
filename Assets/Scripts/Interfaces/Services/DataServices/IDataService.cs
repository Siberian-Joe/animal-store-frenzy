namespace Interfaces.Services.DataServices
{
    public interface IDataService
    {
        void Save<T>(T data, string key = null, string id = null);
        T Load<T>(string key = null, T defaultValue = default, string id = null);
    }
}