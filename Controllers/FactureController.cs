using Microsoft.AspNetCore.Mvc;
using TrackMania.Interfaces;
using TrackMania.Models;

namespace TrackMania.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FactureController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public FactureController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET: api/Facture
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Facture>>> GetFactures()
    {
        var factures = await _unitOfWork.Factures.GetAllAsync();
        return Ok(factures);
    }

    // GET: api/Facture/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Facture>> GetFacture(int id)
    {
        var facture = await _unitOfWork.Factures.GetByIdAsync(id);
        
        if (facture == null)
        {
            return NotFound();
        }

        return Ok(facture);
    }

    // POST: api/Facture
    [HttpPost]
    public async Task<ActionResult<Facture>> CreateFacture(Facture facture)
    {
        if (string.IsNullOrWhiteSpace(facture.Source))
        {
            return BadRequest("La source de la facture est requise");
        }

        if (facture.Montant <= 0)
        {
            return BadRequest("Le montant doit être positif");
        }

        // Vérifier que la société existe
        var societe = await _unitOfWork.Societes.GetByIdAsync(facture.SocieteId);
        if (societe == null)
        {
            return BadRequest("La société spécifiée n'existe pas");
        }

        // Vérifier que l'ingénieur existe
        var ingenieur = await _unitOfWork.Ingenieurs.GetByIdAsync(facture.IngenieurId);
        if (ingenieur == null)
        {
            return BadRequest("L'ingénieur spécifié n'existe pas");
        }

        await _unitOfWork.Factures.AddAsync(facture);
        await _unitOfWork.SaveChangesAsync();

        return CreatedAtAction(nameof(GetFacture), new { id = facture.Id }, facture);
    }

    // PUT: api/Facture/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFacture(int id, Facture facture)
    {
        if (string.IsNullOrWhiteSpace(facture.Source))
        {
            return BadRequest("La source de la facture est requise");
        }

        if (facture.Montant <= 0)
        {
            return BadRequest("Le montant doit être positif");
        }

        var existingFacture = await _unitOfWork.Factures.GetByIdAsync(id);
        if (existingFacture == null)
        {
            return NotFound();
        }

        // Vérifier que la société existe
        var societe = await _unitOfWork.Societes.GetByIdAsync(facture.SocieteId);
        if (societe == null)
        {
            return BadRequest("La société spécifiée n'existe pas");
        }

        // Vérifier que l'ingénieur existe
        var ingenieur = await _unitOfWork.Ingenieurs.GetByIdAsync(facture.IngenieurId);
        if (ingenieur == null)
        {
            return BadRequest("L'ingénieur spécifié n'existe pas");
        }

        existingFacture.Source = facture.Source;
        existingFacture.Montant = facture.Montant;
        existingFacture.DateEmission = facture.DateEmission;
        existingFacture.SocieteId = facture.SocieteId;
        existingFacture.IngenieurId = facture.IngenieurId;
        
        await _unitOfWork.Factures.UpdateAsync(existingFacture);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Facture/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFacture(int id)
    {
        var facture = await _unitOfWork.Factures.GetByIdAsync(id);
        if (facture == null)
        {
            return NotFound();
        }

        await _unitOfWork.Factures.DeleteAsync(facture.Id);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/Facture/societe/5
    [HttpGet("societe/{societeId}")]
    public async Task<ActionResult<IEnumerable<Facture>>> GetFacturesBySociete(int societeId)
    {
        var societe = await _unitOfWork.Societes.GetByIdAsync(societeId);
        if (societe == null)
        {
            return NotFound();
        }

        var factures = await _unitOfWork.Factures.FindAsync(f => f.SocieteId == societeId);
        return Ok(factures);
    }

    // GET: api/Facture/ingenieur/5
    [HttpGet("ingenieur/{ingenieurId}")]
    public async Task<ActionResult<IEnumerable<Facture>>> GetFacturesByIngenieur(int ingenieurId)
    {
        var ingenieur = await _unitOfWork.Ingenieurs.GetByIdAsync(ingenieurId);
        if (ingenieur == null)
        {
            return NotFound();
        }

        var factures = await _unitOfWork.Factures.FindAsync(f => f.IngenieurId == ingenieurId);
        return Ok(factures);
    }
}
