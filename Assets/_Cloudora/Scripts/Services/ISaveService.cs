namespace Cloudora.Services
{
    public interface ISaveService
    {
        SaveData Load();
        void Save(SaveData data);
    }
}
