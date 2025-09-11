using Microsoft.EntityFrameworkCore;
using TrackMania.Interfaces;
using TrackMania.Models;

namespace TrackMania.Services
{
    public class InterventionService : IInterventionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InterventionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Intervention>> GetAllInterventionsAsync()
        {
            return await _unitOfWork.Interventions.GetAllAsync();
        }

        public async Task<Intervention?> GetInterventionByIdAsync(int id)
        {
            return await _unitOfWork.Interventions.GetByIdAsync(id);
        }

        public async Task<Intervention> CreateInterventionAsync(Intervention intervention)
        {
            await _unitOfWork.Interventions.AddAsync(intervention);
            await _unitOfWork.SaveChangesAsync();
            return intervention;
        }

        public async Task<Intervention?> UpdateInterventionAsync(int id, Intervention intervention)
        {
            var existing = await _unitOfWork.Interventions.GetByIdAsync(id);
            if (existing == null)
                return null;

            existing.DateDebut = intervention.DateDebut;
            existing.DateFin = intervention.DateFin;
            existing.HeuresTravaillees = intervention.HeuresTravaillees;
            existing.HeuresPayees = intervention.HeuresPayees;
            existing.HeuresNonPayees = intervention.HeuresNonPayees;
            existing.Notes = intervention.Notes;
            existing.IngenieurId = intervention.IngenieurId;
            existing.AdminId = intervention.AdminId;
            existing.SocieteId = intervention.SocieteId;

            await _unitOfWork.Interventions.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteInterventionAsync(int id)
        {
            var existing = await _unitOfWork.Interventions.GetByIdAsync(id);
            if (existing == null)
                return false;
            
            await _unitOfWork.Interventions.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
