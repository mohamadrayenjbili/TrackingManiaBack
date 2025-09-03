namespace TrackMania.Models;

public class Facture
{
    public int Id { get; set; }
    public string Source { get; set; } = string.Empty;
    public double Montant { get; set; }
    public DateTime DateEmission { get; set; }
    public int SocieteId { get; set; }
    public Societe? Societe { get; set; }

    public int IngenieurId { get; set; }
    public Ingenieur? Ingenieur { get; set; }
}


