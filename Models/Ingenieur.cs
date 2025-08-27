using System.Collections.Generic;

namespace TrackMania.Models;

public class Ingenieur
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Photo { get; set; } = string.Empty;
    public ICollection<Intervention> Interventions { get; set; } = new List<Intervention>();
    public int DepartementId { get; set; }
    public Departement? Departement { get; set; }
}

public class Admin : Ingenieur
{
}


