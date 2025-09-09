using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackMania.Data;
using TrackMania.Models;

namespace TrackMania.Services
{
    public class IngenieurService : IIngenieurService
    {
        private readonly AppDbContext _context;

        public IngenieurService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ingenieur>> GetAllIngenieursAsync()
        {
            return await _context.Ingenieurs
                .Include(i => i.Departement)
                .Include(i => i.Interventions)
                .ToListAsync();
        }

        public async Task<Ingenieur?> GetIngenieurByIdAsync(int id)
        {
            return await _context.Ingenieurs
                .Include(i => i.Departement)
                .Include(i => i.Interventions)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Ingenieur> CreateIngenieurAsync(Ingenieur ingenieur)
        {
            _context.Ingenieurs.Add(ingenieur);
            await _context.SaveChangesAsync();
            return ingenieur;
        }

        public async Task<Ingenieur?> UpdateIngenieurAsync(int id, Ingenieur ingenieur)
        {
            var existingIngenieur = await _context.Ingenieurs.FindAsync(id);
            if (existingIngenieur == null)
                return null;

            existingIngenieur.Nom = ingenieur.Nom;
            existingIngenieur.Photo = ingenieur.Photo;
            existingIngenieur.DepartementId = ingenieur.DepartementId;

            _context.Ingenieurs.Update(existingIngenieur);
            await _context.SaveChangesAsync();
            return existingIngenieur;
        }

        public async Task<bool> DeleteIngenieurAsync(int id)
        {
            var ingenieur = await _context.Ingenieurs.FindAsync(id);
            if (ingenieur == null)
                return false;

            _context.Ingenieurs.Remove(ingenieur);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Ingenieur>> GetIngenieursByDepartementAsync(int departementId)
        {
            return await _context.Ingenieurs
                .Include(i => i.Departement)
                .Where(i => i.DepartementId == departementId)
                .ToListAsync();
        }
    }
}