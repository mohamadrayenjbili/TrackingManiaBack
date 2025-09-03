using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackMania.Data;
using TrackMania.Models;

namespace TrackMania.Services
{
    public class SocieteService : ISocieteService
    {
        private readonly AppDbContext _context;

        public SocieteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Societe>> GetAllSocietesAsync()
        {
            return await _context.Societes
                .Include(s => s.Factures)
                .Include(s => s.Interventions)
                .ToListAsync();
        }

        public async Task<Societe?> GetSocieteByIdAsync(int id)
        {
            return await _context.Societes
                .Include(s => s.Factures)
                .Include(s => s.Interventions)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Societe> CreateSocieteAsync(Societe societe)
        {
            _context.Societes.Add(societe);
            await _context.SaveChangesAsync();
            return societe;
        }

        public async Task<Societe?> UpdateSocieteAsync(int id, Societe societe)
        {
            var existingSociete = await _context.Societes.FindAsync(id);
            if (existingSociete == null) return null;

            existingSociete.Nom = societe.Nom;
            existingSociete.Photo = societe.Photo;
            existingSociete.Localisation = societe.Localisation;
            existingSociete.Secteur = societe.Secteur;

            _context.Societes.Update(existingSociete);
            await _context.SaveChangesAsync();
            return existingSociete;
        }

        public async Task<bool> DeleteSocieteAsync(int id)
        {
            var societe = await _context.Societes.FindAsync(id);
            if (societe == null) return false;

            _context.Societes.Remove(societe);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
