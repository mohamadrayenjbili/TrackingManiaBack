using System.Collections.Generic;

namespace TrackMania.Models;

public class Societe
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Photo { get; set; } = string.Empty;
    public string Localisation { get; set; } = string.Empty;
    public string Secteur { get; set; } = string.Empty;
    public ICollection<Facture> Factures { get; set; } = new List<Facture>();
    public ICollection<Intervention> Interventions { get; set; } = new List<Intervention>();
}


