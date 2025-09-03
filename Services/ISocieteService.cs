using System.Collections.Generic;
using System.Threading.Tasks;
using TrackMania.Models;

namespace TrackMania.Services
{
    public interface ISocieteService
    {
        Task<IEnumerable<Societe>> GetAllSocietesAsync();
        Task<Societe?> GetSocieteByIdAsync(int id);
        Task<Societe> CreateSocieteAsync(Societe societe);
        Task<Societe?> UpdateSocieteAsync(int id, Societe societe);
        Task<bool> DeleteSocieteAsync(int id);
    }
}
