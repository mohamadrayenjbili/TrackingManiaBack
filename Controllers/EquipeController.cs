using Microsoft.AspNetCore.Mvc;
using TrackMania.Interfaces;
using TrackMania.Models;

namespace TrackMania.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EquipeController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public EquipeController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET: api/Equipe
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Equipe>>> GetEquipes()
    {
        var equipes = await _unitOfWork.Equipes.GetAllAsync();
        return Ok(equipes);
    }

    // GET: api/Equipe/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Equipe>> GetEquipe(int id)
    {
        var equipe = await _unitOfWork.Equipes.GetByIdAsync(id);
        
        if (equipe == null)
        {
            return NotFound();
        }

        return Ok(equipe);
    }

    // POST: api/Equipe
    [HttpPost]
    public async Task<ActionResult<Equipe>> CreateEquipe(Equipe equipe)
    {
        if (string.IsNullOrWhiteSpace(equipe.Nom))
        {
            return BadRequest("Le nom de l'équipe est requis");
        }

        // Vérifier que le département existe
        var departement = await _unitOfWork.Departements.GetByIdAsync(equipe.DepartementId);
        if (departement == null)
        {
            return BadRequest("Le département spécifié n'existe pas");
        }

        // Vérifier que l'admin existe
        var admin = await _unitOfWork.Admins.GetByIdAsync(equipe.AdminId);
        if (admin == null)
        {
            return BadRequest("L'administrateur spécifié n'existe pas");
        }

        await _unitOfWork.Equipes.AddAsync(equipe);
        await _unitOfWork.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEquipe), new { id = equipe.Id }, equipe);
    }

    // PUT: api/Equipe/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEquipe(int id, Equipe equipe)
    {
        if (string.IsNullOrWhiteSpace(equipe.Nom))
        {
            return BadRequest("Le nom de l'équipe est requis");
        }

        var existingEquipe = await _unitOfWork.Equipes.GetByIdAsync(id);
        if (existingEquipe == null)
        {
            return NotFound();
        }

        // Vérifier que le département existe
        var departement = await _unitOfWork.Departements.GetByIdAsync(equipe.DepartementId);
        if (departement == null)
        {
            return BadRequest("Le département spécifié n'existe pas");
        }

        // Vérifier que l'admin existe
        var admin = await _unitOfWork.Admins.GetByIdAsync(equipe.AdminId);
        if (admin == null)
        {
            return BadRequest("L'administrateur spécifié n'existe pas");
        }

        existingEquipe.Nom = equipe.Nom;
        existingEquipe.Objectif = equipe.Objectif;
        existingEquipe.DepartementId = equipe.DepartementId;
        existingEquipe.AdminId = equipe.AdminId;
        
        await _unitOfWork.Equipes.UpdateAsync(existingEquipe);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Equipe/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEquipe(int id)
    {
        var equipe = await _unitOfWork.Equipes.GetByIdAsync(id);
        if (equipe == null)
        {
            return NotFound();
        }

        await _unitOfWork.Equipes.DeleteAsync(equipe.Id);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/Equipe/5/ingenieurs
    [HttpGet("{id}/ingenieurs")]
    public async Task<ActionResult<IEnumerable<Ingenieur>>> GetIngenieursByEquipe(int id)
    {
        var equipe = await _unitOfWork.Equipes.GetByIdAsync(id);
        if (equipe == null)
        {
            return NotFound();
        }

        var ingenieurs = await _unitOfWork.Ingenieurs.FindAsync(i => i.Equipes.Any(e => e.Id == id));
        return Ok(ingenieurs);
    }

    // POST: api/Equipe/5/ingenieurs
    [HttpPost("{id}/ingenieurs")]
    public async Task<IActionResult> AddIngenieurToEquipe(int id, int ingenieurId)
    {
        var equipe = await _unitOfWork.Equipes.GetByIdAsync(id);
        if (equipe == null)
        {
            return NotFound("Équipe non trouvée");
        }

        var ingenieur = await _unitOfWork.Ingenieurs.GetByIdAsync(ingenieurId);
        if (ingenieur == null)
        {
            return NotFound("Ingénieur non trouvé");
        }

        // Vérifier que l'ingénieur n'est pas déjà dans l'équipe
        if (ingenieur.Equipes.Any(e => e.Id == id))
        {
            return BadRequest("L'ingénieur est déjà membre de cette équipe");
        }

        // Ajouter l'ingénieur à l'équipe
        ingenieur.Equipes.Add(equipe);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Equipe/5/ingenieurs/{ingenieurId}
    [HttpDelete("{id}/ingenieurs/{ingenieurId}")]
    public async Task<IActionResult> RemoveIngenieurFromEquipe(int id, int ingenieurId)
    {
        var equipe = await _unitOfWork.Equipes.GetByIdAsync(id);
        if (equipe == null)
        {
            return NotFound("Équipe non trouvée");
        }

        var ingenieur = await _unitOfWork.Ingenieurs.GetByIdAsync(ingenieurId);
        if (ingenieur == null)
        {
            return NotFound("Ingénieur non trouvé");
        }

        // Vérifier que l'ingénieur est bien dans l'équipe
        if (!ingenieur.Equipes.Any(e => e.Id == id))
        {
            return BadRequest("L'ingénieur n'est pas membre de cette équipe");
        }

        // Retirer l'ingénieur de l'équipe
        ingenieur.Equipes.Remove(equipe);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/Equipe/departement/5
    [HttpGet("departement/{departementId}")]
    public async Task<ActionResult<IEnumerable<Equipe>>> GetEquipesByDepartement(int departementId)
    {
        var departement = await _unitOfWork.Departements.GetByIdAsync(departementId);
        if (departement == null)
        {
            return NotFound();
        }

        var equipes = await _unitOfWork.Equipes.FindAsync(e => e.DepartementId == departementId);
        return Ok(equipes);
    }

    // GET: api/Equipe/admin/5
    [HttpGet("admin/{adminId}")]
    public async Task<ActionResult<IEnumerable<Equipe>>> GetEquipesByAdmin(int adminId)
    {
        var admin = await _unitOfWork.Admins.GetByIdAsync(adminId);
        if (admin == null)
        {
            return NotFound();
        }

        var equipes = await _unitOfWork.Equipes.FindAsync(e => e.AdminId == adminId);
        return Ok(equipes);
    }
}
