using Microsoft.AspNetCore.Mvc;
using TrackMania.Interfaces;
using TrackMania.Models;
using TrackMania.DTOs;

namespace TrackMania.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngenieurController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public IngenieurController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET: api/Ingenieur
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Ingenieur>>> GetAll()
    {
        var list = await _unitOfWork.Ingenieurs.GetAllAsync();
        return Ok(list);
    }

    // GET: api/Ingenieur/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Ingenieur>> GetById(int id)
    {
        var item = await _unitOfWork.Ingenieurs.GetByIdAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    // POST: api/Ingenieur
    [HttpPost]
    public async Task<ActionResult<object>> Create([FromBody] IngenieurCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (dto.IsAdmin)
        {
            var admin = new Admin
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password
            };
            await _unitOfWork.Admins.AddAsync(admin);
            await _unitOfWork.SaveChangesAsync();
            return Created(string.Empty, admin);
        }
        else
        {
            var ingenieur = new Ingenieur
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password,
                Photo = dto.Photo,
                DepartementId = dto.DepartementId
            };
            await _unitOfWork.Ingenieurs.AddAsync(ingenieur);
            await _unitOfWork.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = ingenieur.Id }, ingenieur);
        }
    }

    // PUT: api/Ingenieur/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Ingenieur ingenieur)
    {
        if (id != ingenieur.Id) return BadRequest("L'id ne correspond pas");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var existing = await _unitOfWork.Ingenieurs.GetByIdAsync(id);
        if (existing == null) return NotFound();

        // Mettre à jour les champs autorisés
        existing.Username = ingenieur.Username;
        existing.Photo = ingenieur.Photo;
        existing.DepartementId = ingenieur.DepartementId;

        await _unitOfWork.Ingenieurs.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/Ingenieur/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _unitOfWork.Ingenieurs.GetByIdAsync(id);
        if (existing == null) return NotFound();

        await _unitOfWork.Ingenieurs.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }
}
