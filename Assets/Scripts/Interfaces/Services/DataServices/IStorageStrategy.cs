namespace Interfaces.Services.DataServices
{
    public interface IStorageStrategy
    {
        void Save(string key, string data);
        string Load(string key, string defaultValue);
    }
}