namespace TrackMania.Models;

public class KPI
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public double Valeur { get; set; }
    public string Type { get; set; } = string.Empty;
    public int? AdminId { get; set; }
    public Admin? Admin { get; set; }
}


