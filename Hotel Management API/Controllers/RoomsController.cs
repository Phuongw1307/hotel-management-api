using Hotel_Management_API.DTOs;
using Hotel_Management_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _roomService.GetAllAsync();
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _roomService.GetByIdAsync(id);

            if (!result.Success)
                return NotFound(new { message = result.Message });

            return Ok(result.Data);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateRoomRequest request)
        {
            var result = await _roomService.CreateAsync(request);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRoomRequest request)
        {
            var result = await _roomService.UpdateAsync(id, request);

            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                    return NotFound(new { message = result.Message });

                return BadRequest(new { message = result.Message });
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _roomService.DeleteAsync(id);

            if (!result.Success)
                return NotFound(new { message = result.Message });

            return NoContent();
        }
    }
}
