using Microsoft.AspNetCore.Mvc;
using TrackMania.Interfaces;
using TrackMania.Models;

namespace TrackMania.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SocieteController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public SocieteController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET: api/Societe
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Societe>>> GetAll()
    {
        var list = await _unitOfWork.Societes.GetAllAsync();
        return Ok(list);
    }

    // GET: api/Societe/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Societe>> GetById(int id)
    {
        var item = await _unitOfWork.Societes.GetByIdAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    // POST: api/Societe
    [HttpPost]
    public async Task<ActionResult<Societe>> Create([FromBody] Societe societe)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _unitOfWork.Societes.AddAsync(societe);
        await _unitOfWork.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = societe.Id }, societe);
    }

    // PUT: api/Societe/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Societe societe)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var existing = await _unitOfWork.Societes.GetByIdAsync(id);
        if (existing == null) return NotFound();

        existing.Nom = societe.Nom;
        existing.Photo = societe.Photo;
        existing.Localisation = societe.Localisation;
        existing.Secteur = societe.Secteur;

        await _unitOfWork.Societes.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/Societe/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _unitOfWork.Societes.GetByIdAsync(id);
        if (existing == null) return NotFound();

        await _unitOfWork.Societes.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }
}
