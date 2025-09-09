using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackMania.Data;
using TrackMania.Models;
using TrackMania.Interfaces;

namespace TrackMania.Services
{
    public class DepartementService : IDepartementService
    {
        private readonly AppDbContext _context;

        public DepartementService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Departement>> GetAllDepartementsAsync()
        {
            return await _context.Departements
                .Include(d => d.Ingenieurs)
                .ToListAsync();
        }

        public async Task<Departement?> GetDepartementByIdAsync(int id)
        {
            return await _context.Departements
                .Include(d => d.Ingenieurs)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Departement> CreateDepartementAsync(Departement departement)
        {
            _context.Departements.Add(departement);
            await _context.SaveChangesAsync();
            return departement;
        }

        public async Task<Departement?> UpdateDepartementAsync(int id, Departement departement)
        {
            var existingDepartement = await _context.Departements.FindAsync(id);
            if (existingDepartement == null)
                return null;

            existingDepartement.Nom = departement.Nom;

            _context.Departements.Update(existingDepartement);
            await _context.SaveChangesAsync();
            return existingDepartement;
        }

        public async Task<bool> DeleteDepartementAsync(int id)
        {
            var departement = await _context.Departements.FindAsync(id);
            if (departement == null)
                return false;

            _context.Departements.Remove(departement);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
