namespace TrackMania.Models;

public class Intervention
{
    public int Id { get; set; }
    public DateTime DateDebut { get; set; }
    public DateTime DateFin { get; set; }
    public double HeuresTravaillees { get; set; }
    public double HeuresPayees { get; set; }
    public double HeuresNonPayees { get; set; }
    public string Notes { get; set; } = string.Empty;

    public int IngenieurId { get; set; }
    public Ingenieur? Ingenieur { get; set; }

    public int? AdminId { get; set; }
    public Admin? Admin { get; set; }

    public int SocieteId { get; set; }
    public Societe? Societe { get; set; }
}


