using System.Collections.Generic;

namespace TrackMania.Models;

public class Departement
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public ICollection<Ingenieur> Ingenieurs { get; set; } = new List<Ingenieur>();
    public ICollection<Equipe> Equipes { get; set; } = new List<Equipe>();
    public ICollection<Admin> SupervisedAdmins { get; set; } = new List<Admin>();
}


