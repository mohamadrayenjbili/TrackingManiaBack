using TrackMania.Models;

namespace TrackMania.Interfaces;

public interface IUnitOfWork : IDisposable
{
    // Repository Properties
    IRepository<Ingenieur> Ingenieurs { get; }
    IRepository<Admin> Admins { get; }
    IRepository<Departement> Departements { get; }
    IRepository<Societe> Societes { get; }
    IRepository<Intervention> Interventions { get; }
    IRepository<Facture> Factures { get; }
    IRepository<KPI> KPIs { get; }
    IRepository<Equipe> Equipes { get; }
    
    // Transaction Management
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
