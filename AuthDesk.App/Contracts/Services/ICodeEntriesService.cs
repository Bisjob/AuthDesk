using AuthDesk.Core.Models;

namespace AuthDesk.Contracts.Services
{
    public interface ICodeEntriesService
    {
        CodeEntries Entries { get; }

        void SaveData();
    }
}