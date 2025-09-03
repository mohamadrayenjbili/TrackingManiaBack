using System.Collections.Generic;

namespace TrackMania.Models;

public class Equipe
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Objectif { get; set; } = string.Empty;
    
    public int DepartementId { get; set; }
    public Departement? Departement { get; set; }

    public int AdminId { get; set; }
    public Admin? Admin { get; set; }

    public ICollection<Ingenieur> Ingenieurs { get; set; } = new List<Ingenieur>();
}
