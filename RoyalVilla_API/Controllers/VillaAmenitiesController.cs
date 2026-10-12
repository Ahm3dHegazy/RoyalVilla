using Microsoft.AspNetCore.Authorization;
using RoyalVilla_API.Interfaces;

namespace RoyalVilla_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VillaAmenitiesController(IVillaAmenitiesService amenitiesService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<VillaAmenitiesDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<VillaAmenitiesDto>>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<IEnumerable<VillaAmenitiesDto>>>> GetAll([FromQuery] int villaId)
    {
        var amenities = await amenitiesService.GetByVillaIdAsync(villaId);
        if (amenities is null)
            return NotFound(ApiResponse<IEnumerable<VillaAmenitiesDto>>
                .NotFound($"Villa with id {villaId} was not found."));

        return Ok(ApiResponse<IEnumerable<VillaAmenitiesDto>>.Success(amenities));
    }

    [HttpGet("{id:int:min(1)}", Name = "GetVillaAmenity")]
    [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDto>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<VillaAmenitiesDto>>> GetById(int id)
    {
        var amenity = await amenitiesService.GetByIdAsync(id);
        if (amenity is null)
            return NotFound(ApiResponse<VillaAmenitiesDto>.NotFound($"Villa amenity with id {id} was not found."));

        return Ok(ApiResponse<VillaAmenitiesDto>.Success(amenity));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDto>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<VillaAmenitiesDto>>> Create(CreateVillaAmenitiesDto? amenityDto)
    {
        if (amenityDto is null)
            return BadRequest(ApiResponse<VillaAmenitiesDto>.BadRequest("Villa amenity data is required."));

        var amenity = await amenitiesService.CreateAsync(amenityDto);
        if (amenity is null)
            return NotFound(ApiResponse<VillaAmenitiesDto>
                .NotFound($"Villa with id {amenityDto.VillaId} was not found."));

        var response = ApiResponse<VillaAmenitiesDto>.Created(amenity);
        return CreatedAtRoute("GetVillaAmenity", new { id = amenity.Id }, response);
    }

    [HttpPut("{id:int:min(1)}")]
    [Authorize(Roles = "Admin")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, UpdateVillaAmenitiesDto? amenityDto)
    {
        if (amenityDto is null)
            return BadRequest(ApiResponse<VillaAmenitiesDto>.BadRequest("Villa amenity data is required."));

        if (await amenitiesService.GetByIdAsync(id) is null)
            return NotFound(ApiResponse<VillaAmenitiesDto>.NotFound($"Villa amenity with id {id} was not found."));

        if (!await amenitiesService.VillaExistsAsync(amenityDto.VillaId))
            return NotFound(ApiResponse<VillaAmenitiesDto>
                .NotFound($"Villa with id {amenityDto.VillaId} was not found."));

        await amenitiesService.UpdateAsync(id, amenityDto);
        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await amenitiesService.DeleteAsync(id))
            return NotFound(ApiResponse<VillaAmenitiesDto>.NotFound($"Villa amenity with id {id} was not found."));

        return NoContent();
    }
}
