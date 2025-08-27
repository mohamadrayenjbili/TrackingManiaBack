using Microsoft.AspNetCore.Mvc;
using TrackMania.Models;
using TrackMania.Services;

namespace TrackMania.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InterventionController : ControllerBase
{
    private readonly IInterventionService _interventionService;

    public InterventionController(IInterventionService interventionService)
    {
        _interventionService = interventionService;
    }

    // GET: api/Intervention
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Intervention>>> GetInterventions()
    {
        try
        {
            var interventions = await _interventionService.GetAllInterventionsAsync();
            return Ok(interventions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erreur interne: {ex.Message}");
        }
    }

    // GET: api/Intervention/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Intervention>> GetIntervention(int id)
    {
        try
        {
            var intervention = await _interventionService.GetInterventionByIdAsync(id);
            if (intervention == null)
                return NotFound($"Intervention avec l'ID {id} non trouvée");

            return Ok(intervention);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erreur interne: {ex.Message}");
        }
    }

    // POST: api/Intervention
    [HttpPost]
    public async Task<ActionResult<Intervention>> CreateIntervention(Intervention intervention)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdIntervention = await _interventionService.CreateInterventionAsync(intervention);
            return CreatedAtAction(nameof(GetIntervention), new { id = createdIntervention.Id }, createdIntervention);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erreur interne: {ex.Message}");
        }
    }

    // PUT: api/Intervention/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIntervention(int id, Intervention intervention)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Utiliser l'ID de l'URL comme source de vérité
            intervention.Id = id;
            await _interventionService.UpdateInterventionAsync(intervention);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erreur interne: {ex.Message}");
        }
    }

    // DELETE: api/Intervention/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIntervention(int id)
    {
        try
        {
            await _interventionService.DeleteInterventionAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erreur interne: {ex.Message}");
        }
    }

    // GET: api/Intervention/ingenieur/5
    [HttpGet("ingenieur/{ingenieurId}")]
    public async Task<ActionResult<IEnumerable<Intervention>>> GetInterventionsByIngenieur(int ingenieurId)
    {
        try
        {
            var interventions = await _interventionService.GetInterventionsByIngenieurAsync(ingenieurId);
            return Ok(interventions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erreur interne: {ex.Message}");
        }
    }

    // GET: api/Intervention/societe/5
    [HttpGet("societe/{societeId}")]
    public async Task<ActionResult<IEnumerable<Intervention>>> GetInterventionsBySociete(int societeId)
    {
        try
        {
            var interventions = await _interventionService.GetInterventionsBySocieteAsync(societeId);
            return Ok(interventions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erreur interne: {ex.Message}");
        }
    }

    // GET: api/Intervention/statistics
    [HttpGet("statistics")]
    public async Task<ActionResult<object>> GetStatistics()
    {
        try
        {
            var totalHeuresTravaillees = await _interventionService.GetTotalHeuresTravailleesAsync();
            var totalHeuresPayees = await _interventionService.GetTotalHeuresPayeesAsync();
            var totalHeuresNonPayees = await _interventionService.GetTotalHeuresNonPayeesAsync();

            return Ok(new
            {
                TotalHeuresTravaillees = totalHeuresTravaillees,
                TotalHeuresPayees = totalHeuresPayees,
                TotalHeuresNonPayees = totalHeuresNonPayees,
                TauxPayement = totalHeuresTravaillees > 0 ? (totalHeuresPayees / totalHeuresTravaillees) * 100 : 0
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erreur interne: {ex.Message}");
        }
    }
}
