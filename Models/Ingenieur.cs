using System.Collections.Generic;

namespace TrackMania.Models;

public class Ingenieur : User
{
    public string Photo { get; set; } = string.Empty;
    public ICollection<Intervention> Interventions { get; set; } = new List<Intervention>();
    public int DepartementId { get; set; }
    public Departement? Departement { get; set; }
    public ICollection<Equipe> Equipes { get; set; } = new List<Equipe>();
}

public class Admin : Ingenieur
{
}


