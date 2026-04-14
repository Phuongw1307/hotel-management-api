using Hotel_Management_API.DTOs;
using Hotel_Management_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomTypesController : ControllerBase
    {
        private readonly IRoomTypeService _roomTypeService;

        public RoomTypesController(IRoomTypeService roomTypeService)
        {
            _roomTypeService = roomTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _roomTypeService.GetAllAsync();
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _roomTypeService.GetByIdAsync(id);

            if (!result.Success)
                return NotFound(new { message = result.Message });

            return Ok(result.Data);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateRoomTypeRequest request)
        {
            var result = await _roomTypeService.CreateAsync(request);

            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRoomTypeRequest request)
        {
            var result = await _roomTypeService.UpdateAsync(id, request);

            if (!result.Success)
                return NotFound(new { message = result.Message });

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _roomTypeService.DeleteAsync(id);

            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                    return NotFound(new { message = result.Message });

                return BadRequest(new { message = result.Message });
            }

            return NoContent();
        }
    }
}
