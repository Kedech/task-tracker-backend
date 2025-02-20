using System.Text.Json;
using Tasks.Tracker.Application.Interfaces;
using Tasks.Tracker.Domain.Entities;

namespace Tasks.Tracker.Infrastructure.Persistence
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly static string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "literals.json");

        List<Language> _literals;
        public LanguageRepository()
        {
            _literals = LoadLiteralsFromFile();
        }

        private static List<Language> LoadLiteralsFromFile()
        {
            if (!File.Exists(_filePath))
            {
                return [];
            }
            string json = File.ReadAllText(_filePath);

            return JsonSerializer.Deserialize<List<Language>>(json) ?? [];
        }

        public async Task<IEnumerable<Language>> GetLiterals()
        {
            return await Task.FromResult(_literals);
        }
    }
}
