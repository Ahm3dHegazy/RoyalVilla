using Microsoft.AspNetCore.Authorization;
using RoyalVilla_API.Interfaces;

namespace RoyalVilla_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VillaController(IVillaService villaService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<VillaDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<VillaDto>>>> GetVillas()
        => Ok(ApiResponse<IEnumerable<VillaDto>>.Success(await villaService.GetAllAsync()));

    [HttpGet("{id:int:min(1)}", Name = "GetVilla")]
    [ProducesResponseType(typeof(ApiResponse<VillaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<VillaDto>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<VillaDto>>> GetVillaById(int id)
    {
        var villa = await villaService.GetByIdAsync(id);
        if (villa is null)
            return NotFound(ApiResponse<VillaDto>.NotFound($"Villa with id {id} was not found."));

        return Ok(ApiResponse<VillaDto>.Success(villa));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ApiResponse<VillaDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<VillaDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<VillaDto>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<VillaDto>>> CreateVilla(CreateVillaDto? villaDto)
    {
        if (villaDto is null)
            return BadRequest(ApiResponse<VillaDto>.BadRequest("Villa data is required."));

        if (await villaService.NameExistsAsync(villaDto.Name!))
            return Conflict(ApiResponse<VillaDto>.Conflict($"Villa with name '{villaDto.Name}' already exists."));

        var villa = await villaService.CreateAsync(villaDto);
        var response = ApiResponse<VillaDto>.Created(villa);
        return CreatedAtRoute("GetVilla", new { id = villa!.Id }, response);
    }

    [HttpPut("{id:int:min(1)}")]
    [Authorize(Roles = "Admin")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<VillaDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<VillaDto>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateVilla([FromRoute] int id, [FromBody] UpdateVillaDto villaDto)
    {
        if (await villaService.GetByIdAsync(id) is null)
            return NotFound(ApiResponse<VillaDto>.NotFound($"Villa with id {id} was not found."));

        if (await villaService.NameExistsAsync(villaDto.Name!, id))
            return Conflict(ApiResponse<VillaDto>.Conflict($"A villa with name '{villaDto.Name}' already exists."));

        await villaService.UpdateAsync(id, villaDto);
        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<VillaDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await villaService.DeleteAsync(id))
            return NotFound(ApiResponse<VillaDto>.NotFound($"Villa with id {id} was not found."));

        return NoContent();
    }
}
