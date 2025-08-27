using Microsoft.EntityFrameworkCore;
using TrackMania.Interfaces;
using TrackMania.Models;

namespace TrackMania.Services;

public class InterventionService : IInterventionService
{
    private readonly IUnitOfWork _unitOfWork;

    public InterventionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // CRUD Operations
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
        // Validation métier
        if (intervention.DateDebut >= intervention.DateFin)
            throw new ArgumentException("La date de début doit être antérieure à la date de fin");

        if (intervention.HeuresTravaillees <= 0)
            throw new ArgumentException("Les heures travaillées doivent être positives");

        // Normaliser les DateTime en UTC pour PostgreSQL (timestamptz)
        intervention.DateDebut = NormalizeUtc(intervention.DateDebut);
        intervention.DateFin = NormalizeUtc(intervention.DateFin);

        await _unitOfWork.Interventions.AddAsync(intervention);
        await _unitOfWork.SaveChangesAsync();
        return intervention;
    }

    public async Task UpdateInterventionAsync(Intervention intervention)
    {
        var existing = await _unitOfWork.Interventions.GetByIdAsync(intervention.Id);
        if (existing == null)
            throw new ArgumentException("Intervention non trouvée");

        // Validation métier
        if (intervention.DateDebut >= intervention.DateFin)
            throw new ArgumentException("La date de début doit être antérieure à la date de fin");

        // Normaliser les DateTime en UTC pour PostgreSQL (timestamptz)
        intervention.DateDebut = NormalizeUtc(intervention.DateDebut);
        intervention.DateFin = NormalizeUtc(intervention.DateFin);

        // Mettre à jour l'instance suivie pour éviter les conflits de suivi EF Core
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
    }

    public async Task DeleteInterventionAsync(int id)
    {
        await _unitOfWork.Interventions.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    // Business Logic
    public async Task<double> CalculerHeuresPayeesAsync(int interventionId)
    {
        var intervention = await _unitOfWork.Interventions.GetByIdAsync(interventionId);
        if (intervention == null)
            return 0;

        // Logique métier : calcul des heures payées
        return Math.Min(intervention.HeuresTravaillees, intervention.HeuresPayees);
    }

    public async Task<double> CalculerHeuresNonPayeesAsync(int interventionId)
    {
        var intervention = await _unitOfWork.Interventions.GetByIdAsync(interventionId);
        if (intervention == null)
            return 0;

        // Logique métier : calcul des heures non payées
        return Math.Max(0, intervention.HeuresTravaillees - intervention.HeuresPayees);
    }

    public async Task<IEnumerable<Intervention>> GetInterventionsByIngenieurAsync(int ingenieurId)
    {
        return await _unitOfWork.Interventions.FindAsync(i => i.IngenieurId == ingenieurId);
    }

    public async Task<IEnumerable<Intervention>> GetInterventionsBySocieteAsync(int societeId)
    {
        return await _unitOfWork.Interventions.FindAsync(i => i.SocieteId == societeId);
    }

    public async Task<IEnumerable<Intervention>> GetInterventionsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var startUtc = NormalizeUtc(startDate);
        var endUtc = NormalizeUtc(endDate);
        return await _unitOfWork.Interventions.FindAsync(i => 
            i.DateDebut >= startUtc && i.DateFin <= endUtc);
    }

    // Statistics
    public async Task<double> GetTotalHeuresTravailleesAsync()
    {
        var interventions = await _unitOfWork.Interventions.GetAllAsync();
        return interventions.Sum(i => i.HeuresTravaillees);
    }

    public async Task<double> GetTotalHeuresPayeesAsync()
    {
        var interventions = await _unitOfWork.Interventions.GetAllAsync();
        return interventions.Sum(i => i.HeuresPayees);
    }

    public async Task<double> GetTotalHeuresNonPayeesAsync()
    {
        var interventions = await _unitOfWork.Interventions.GetAllAsync();
        return interventions.Sum(i => i.HeuresNonPayees);
    }

    // Helpers
    private static DateTime NormalizeUtc(DateTime dateTime)
    {
        if (dateTime.Kind == DateTimeKind.Utc)
            return dateTime;
        if (dateTime.Kind == DateTimeKind.Local)
            return dateTime.ToUniversalTime();
        return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
    }
}

