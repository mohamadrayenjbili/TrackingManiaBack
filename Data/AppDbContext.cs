using Microsoft.EntityFrameworkCore;
using TrackMania.Models;

namespace TrackMania.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Ingenieur> Ingenieurs => Set<Ingenieur>();
    public DbSet<Admin> Admins => Set<Admin>();
    public DbSet<Departement> Departements => Set<Departement>();
    public DbSet<Societe> Societes => Set<Societe>();
    public DbSet<Intervention> Interventions => Set<Intervention>();
    public DbSet<Facture> Factures => Set<Facture>();
    public DbSet<KPI> KPIs => Set<KPI>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Ingenieur>()
            .HasDiscriminator<string>("Discriminator")
            .HasValue<Ingenieur>("Ingenieur")
            .HasValue<Admin>("Admin");

        modelBuilder.Entity<Departement>()
            .HasMany(d => d.Ingenieurs)
            .WithOne(i => i.Departement!)
            .HasForeignKey(i => i.DepartementId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Ingenieur>()
            .HasMany(i => i.Interventions)
            .WithOne(iv => iv.Ingenieur!)
            .HasForeignKey(iv => iv.IngenieurId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Admin>()
            .HasMany<KPI>()
            .WithOne(k => k.Admin!)
            .HasForeignKey(k => k.AdminId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Intervention>()
            .HasOne(iv => iv.Admin!)
            .WithMany()
            .HasForeignKey(iv => iv.AdminId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Societe>()
            .HasMany(s => s.Factures)
            .WithOne(f => f.Societe!)
            .HasForeignKey(f => f.SocieteId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Societe>()
            .HasMany(s => s.Interventions)
            .WithOne(iv => iv.Societe!)
            .HasForeignKey(iv => iv.SocieteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}


