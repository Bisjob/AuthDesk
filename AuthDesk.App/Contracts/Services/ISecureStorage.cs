namespace AuthDesk.Contracts.Services
{
    public interface ISecureStorage
    {
        void Save<T>(T data, string filename);
        T Read<T>(string filename);
    }
}