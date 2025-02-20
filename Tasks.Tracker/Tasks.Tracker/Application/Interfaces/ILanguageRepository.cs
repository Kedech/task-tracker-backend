using Tasks.Tracker.Domain.Entities;

namespace Tasks.Tracker.Application.Interfaces
{
    public interface ILanguageRepository
    {
        Task<IEnumerable<Language>> GetLiterals();
    }
}
