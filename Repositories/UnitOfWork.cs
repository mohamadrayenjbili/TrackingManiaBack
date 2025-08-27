using Microsoft.EntityFrameworkCore.Storage;
using TrackMania.Data;
using TrackMania.Interfaces;

namespace TrackMania.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;
    
    // Repositories
    private IRepository<Ingenieur>? _ingenieurs;
    private IRepository<Admin>? _admins;
    private IRepository<Departement>? _departements;
    private IRepository<Societe>? _societes;
    private IRepository<Intervention>? _interventions;
    private IRepository<Facture>? _factures;
    private IRepository<KPI>? _kpis;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    // Repository Properties
    public IRepository<Ingenieur> Ingenieurs => 
        _ingenieurs ??= new Repository<Ingenieur>(_context);
    
    public IRepository<Admin> Admins => 
        _admins ??= new Repository<Admin>(_context);
    
    public IRepository<Departement> Departements => 
        _departements ??= new Repository<Departement>(_context);
    
    public IRepository<Societe> Societes => 
        _societes ??= new Repository<Societe>(_context);
    
    public IRepository<Intervention> Interventions => 
        _interventions ??= new Repository<Intervention>(_context);
    
    public IRepository<Facture> Factures => 
        _factures ??= new Repository<Facture>(_context);
    
    public IRepository<KPI> KPIs => 
        _kpis ??= new Repository<KPI>(_context);

    // Transaction Management
    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
