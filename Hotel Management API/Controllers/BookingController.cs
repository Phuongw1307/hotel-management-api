using Hotel_Management_API.DTOs;
using Hotel_Management_API.Models;
using Hotel_Management_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hotel_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        [Authorize(Roles = Roles.Staff + "," + Roles.Admin)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _bookingService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _bookingService.GetByIdAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("available-rooms")]
        [Authorize(Roles = Roles.User + "," + Roles.Staff + "," + Roles.Admin)]
        public async Task<IActionResult> GetAvailableRooms(
            [FromQuery] int hotelId,
            [FromQuery] DateTime checkInDate,
            [FromQuery] DateTime checkOutDate)
        {
            var result = await _bookingService.GetAvailableRoomsAsync(hotelId, checkInDate, checkOutDate);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = Roles.User + "," + Roles.Staff + "," + Roles.Admin)]
        public async Task<IActionResult> Create([FromBody] CreateBookingRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out var appUserId))
                return Unauthorized(new
                {
                    success = false,
                    message = "Invalid user claim.",
                    data = (object?)null
                });

            var result = await _bookingService.CreateAsync(appUserId, request);

            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
        }
    }
}
