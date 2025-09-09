using System.Collections.Generic;
using System.Threading.Tasks;
using TrackMania.Models;

namespace TrackMania.Interfaces
{
    public interface IDepartementService
    {
        Task<IEnumerable<Departement>> GetAllDepartementsAsync();
        Task<Departement?> GetDepartementByIdAsync(int id);
        Task<Departement> CreateDepartementAsync(Departement departement);
        Task<Departement?> UpdateDepartementAsync(int id, Departement departement);
        Task<bool> DeleteDepartementAsync(int id);
    }
}
