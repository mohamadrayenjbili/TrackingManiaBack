using Microsoft.AspNetCore.Mvc;
using TrackMania.Interfaces;
using TrackMania.Models;

namespace TrackMania.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartementController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public DepartementController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET: api/Departement
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Departement>>> GetDepartements()
    {
        var departements = await _unitOfWork.Departements.GetAllAsync();
        return Ok(departements);
    }

    // GET: api/Departement/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Departement>> GetDepartement(int id)
    {
        var departement = await _unitOfWork.Departements.GetByIdAsync(id);
        
        if (departement == null)
        {
            return NotFound();
        }

        return Ok(departement);
    }

    // POST: api/Departement
    [HttpPost]
    public async Task<ActionResult<Departement>> CreateDepartement(Departement departement)
    {
        if (string.IsNullOrWhiteSpace(departement.Nom))
        {
            return BadRequest("Le nom du département est requis");
        }

        await _unitOfWork.Departements.AddAsync(departement);
        await _unitOfWork.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDepartement), new { id = departement.Id }, departement);
    }

    // PUT: api/Departement/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDepartement(int id, Departement departement)
    {
        if (string.IsNullOrWhiteSpace(departement.Nom))
        {
            return BadRequest("Le nom du département est requis");
        }

        var existingDepartement = await _unitOfWork.Departements.GetByIdAsync(id);
        if (existingDepartement == null)
        {
            return NotFound();
        }

        existingDepartement.Nom = departement.Nom;
        
        await _unitOfWork.Departements.UpdateAsync(existingDepartement);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Departement/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDepartement(int id)
    {
        var departement = await _unitOfWork.Departements.GetByIdAsync(id);
        if (departement == null)
        {
            return NotFound();
        }

        // Vérifier s'il y a des ingénieurs ou admins associés
        var ingenieurs = await _unitOfWork.Ingenieurs.FindAsync(i => i.DepartementId == id);
        var admins = await _unitOfWork.Admins.FindAsync(a => a.DepartementId == id);

        if (ingenieurs.Any() || admins.Any())
        {
            return BadRequest("Impossible de supprimer ce département car il est associé à des ingénieurs ou des administrateurs");
        }

        await _unitOfWork.Departements.DeleteAsync(departement.Id);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/Departement/5/ingenieurs
    [HttpGet("{id}/ingenieurs")]
    public async Task<ActionResult<IEnumerable<Ingenieur>>> GetIngenieursByDepartement(int id)
    {
        var departement = await _unitOfWork.Departements.GetByIdAsync(id);
        if (departement == null)
        {
            return NotFound();
        }

        var ingenieurs = await _unitOfWork.Ingenieurs.FindAsync(i => i.DepartementId == id);
        return Ok(ingenieurs);
    }

    // GET: api/Departement/5/admins
    [HttpGet("{id}/admins")]
    public async Task<ActionResult<IEnumerable<Admin>>> GetAdminsByDepartement(int id)
    {
        var departement = await _unitOfWork.Departements.GetByIdAsync(id);
        if (departement == null)
        {
            return NotFound();
        }

        var admins = await _unitOfWork.Admins.FindAsync(a => a.DepartementId == id);
        return Ok(admins);
    }
}
