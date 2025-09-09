using System.Collections.Generic;
using System.Threading.Tasks;
using TrackMania.Models;

namespace TrackMania.Services
{
    public interface IInterventionService
    {
        Task<IEnumerable<Intervention>> GetAllInterventionsAsync();
        Task<Intervention?> GetInterventionByIdAsync(int id);
        Task<Intervention> CreateInterventionAsync(Intervention intervention);
        Task<Intervention?> UpdateInterventionAsync(int id, Intervention intervention);
        Task<bool> DeleteInterventionAsync(int id);
    }
}
