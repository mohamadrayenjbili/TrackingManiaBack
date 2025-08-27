using TrackMania.Models;

namespace TrackMania.Services;

public interface IInterventionService
{
    // CRUD Operations
    Task<IEnumerable<Intervention>> GetAllInterventionsAsync();
    Task<Intervention?> GetInterventionByIdAsync(int id);
    Task<Intervention> CreateInterventionAsync(Intervention intervention);
    Task UpdateInterventionAsync(Intervention intervention);
    Task DeleteInterventionAsync(int id);
    
    // Business Logic
    Task<double> CalculerHeuresPayeesAsync(int interventionId);
    Task<double> CalculerHeuresNonPayeesAsync(int interventionId);
    Task<IEnumerable<Intervention>> GetInterventionsByIngenieurAsync(int ingenieurId);
    Task<IEnumerable<Intervention>> GetInterventionsBySocieteAsync(int societeId);
    Task<IEnumerable<Intervention>> GetInterventionsByDateRangeAsync(DateTime startDate, DateTime endDate);
    
    // Statistics
    Task<double> GetTotalHeuresTravailleesAsync();
    Task<double> GetTotalHeuresPayeesAsync();
    Task<double> GetTotalHeuresNonPayeesAsync();
}
