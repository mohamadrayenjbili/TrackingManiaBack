namespace TrackMania.DTOs;

public class IngenieurCreateDto
{
    public string Nom { get; set; } = string.Empty;
    public string Photo { get; set; } = string.Empty;
    public int DepartementId { get; set; }
    public bool IsAdmin { get; set; }
}
