namespace TrackMania.DTOs;

public class IngenieurCreateDto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Photo { get; set; } = string.Empty;
    public int DepartementId { get; set; }
    public bool IsAdmin { get; set; }
}
