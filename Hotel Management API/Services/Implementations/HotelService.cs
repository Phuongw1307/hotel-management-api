using Hotel_Management_API.DTOs;
using Hotel_Management_API.Models;
using Hotel_Management_API.Repositories.Interfaces;
using Hotel_Management_API.Services.Interfaces;

namespace Hotel_Management_API.Services.Implementations
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;

        public HotelService(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public async Task<ServiceResult<List<HotelResponse>>> GetAllAsync()
        {
            var hotels = await _hotelRepository.GetAllAsync();

            var response = hotels.Select(h => new HotelResponse
            {
                Id = h.Id,
                Name = h.Name,
                Address = h.Address,
                Phone = h.Phone,
                IsActive = h.IsActive
            }).ToList();

            return ServiceResult<List<HotelResponse>>.Ok(response);
        }

        public async Task<ServiceResult<HotelResponse>> GetByIdAsync(int id)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id);
            if (hotel == null)
                return ServiceResult<HotelResponse>.Fail("Hotel not found.");

            var response = new HotelResponse
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Address = hotel.Address,
                Phone = hotel.Phone,
                IsActive = hotel.IsActive
            };

            return ServiceResult<HotelResponse>.Ok(response);
        }

        public async Task<ServiceResult<HotelResponse>> CreateAsync(CreateHotelRequest request)
        {
            var hotel = new Hotel
            {
                Name = request.Name,
                Address = request.Address,
                Phone = request.Phone,
                IsActive = request.IsActive
            };

            await _hotelRepository.AddAsync(hotel);
            await _hotelRepository.SaveChangesAsync();

            var response = new HotelResponse
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Address = hotel.Address,
                Phone = hotel.Phone,
                IsActive = hotel.IsActive
            };

            return ServiceResult<HotelResponse>.Ok(response, "Hotel created successfully.");
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateHotelRequest request)
        {
            var existing = await _hotelRepository.GetByIdForUpdateAsync(id);
            if (existing == null)
                return ServiceResult.Fail("Hotel not found.");

            existing.Name = request.Name;
            existing.Address = request.Address;
            existing.Phone = request.Phone;
            existing.IsActive = request.IsActive;

            await _hotelRepository.SaveChangesAsync();

            return ServiceResult.Ok("Hotel updated successfully.");
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var existing = await _hotelRepository.GetByIdForUpdateAsync(id);
            if (existing == null)
                return ServiceResult.Fail("Hotel not found.");

            var hasRooms = await _hotelRepository.HasRoomsAsync(id);
            if (hasRooms)
                return ServiceResult.Fail("Cannot delete hotel because it has rooms.");

            _hotelRepository.Delete(existing);
            await _hotelRepository.SaveChangesAsync();

            return ServiceResult.Ok("Hotel deleted successfully.");
        }
    }