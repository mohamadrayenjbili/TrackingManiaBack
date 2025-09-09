using System.Collections.Generic;
using System.Threading.Tasks;
using TrackMania.Models;

namespace TrackMania.Services
{
    public interface IIngenieurService
    {
        Task<IEnumerable<Ingenieur>> GetAllIngenieursAsync();
        Task<Ingenieur?> GetIngenieurByIdAsync(int id);
        Task<Ingenieur> CreateIngenieurAsync(Ingenieur ingenieur);
        Task<Ingenieur?> UpdateIngenieurAsync(int id, Ingenieur ingenieur);
        Task<bool> DeleteIngenieurAsync(int id);
        Task<IEnumerable<Ingenieur>> GetIngenieursByDepartementAsync(int departementId);
    }
}