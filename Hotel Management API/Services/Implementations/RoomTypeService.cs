using Hotel_Management_API.DTOs;
using Hotel_Management_API.Models;
using Hotel_Management_API.Repositories.Interfaces;
using Hotel_Management_API.Services.Interfaces;

namespace Hotel_Management_API.Services.Implementations
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IRoomTypeRepository _roomTypeRepository;

        public RoomTypeService(IRoomTypeRepository roomTypeRepository)
        {
            _roomTypeRepository = roomTypeRepository;
        }

        public async Task<ServiceResult<List<RoomTypeResponse>>> GetAllAsync()
        {
            var roomTypes = await _roomTypeRepository.GetAllAsync();

            var response = roomTypes.Select(rt => new RoomTypeResponse
            {
                Id = rt.Id,
                Name = rt.Name,
                Description = rt.Description,
                BasePrice = rt.BasePrice
            }).ToList();

            return ServiceResult<List<RoomTypeResponse>>.Ok(response);
        }

        public async Task<ServiceResult<RoomTypeResponse>> GetByIdAsync(int id)
        {
            var roomType = await _roomTypeRepository.GetByIdAsync(id);
            if (roomType == null)
                return ServiceResult<RoomTypeResponse>.Fail("Room type not found.");

            var response = new RoomTypeResponse
            {
                Id = roomType.Id,
                Name = roomType.Name,
                Description = roomType.Description,
                BasePrice = roomType.BasePrice
            };

            return ServiceResult<RoomTypeResponse>.Ok(response);
        }

        public async Task<ServiceResult<RoomTypeResponse>> CreateAsync(CreateRoomTypeRequest request)
        {
            var roomType = new RoomType
            {
                Name = request.Name,
                Description = request.Description,
                BasePrice = request.BasePrice
            };

            await _roomTypeRepository.AddAsync(roomType);
            await _roomTypeRepository.SaveChangesAsync();

            var response = new RoomTypeResponse
            {
                Id = roomType.Id,
                Name = roomType.Name,
                Description = roomType.Description,
                BasePrice = roomType.BasePrice
            };

            return ServiceResult<RoomTypeResponse>.Ok(response, "Room type created successfully.");
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateRoomTypeRequest request)
        {
            var existing = await _roomTypeRepository.GetByIdAsync(id);
            if (existing == null)
                return ServiceResult.Fail("Room type not found.");

            existing.Name = request.Name;
            existing.Description = request.Description;
            existing.BasePrice = request.BasePrice;

            _roomTypeRepository.Update(existing);
            await _roomTypeRepository.SaveChangesAsync();

            return ServiceResult.Ok("Room type updated successfully.");
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var existing = await _roomTypeRepository.GetByIdAsync(id);
            if (existing == null)
                return ServiceResult.Fail("Room type not found.");

            var hasRooms = await _roomTypeRepository.HasRoomsAsync(id);
            if (hasRooms)
                return ServiceResult.Fail("Cannot delete room type because it is in use.");

            _roomTypeRepository.Delete(existing);
            await _roomTypeRepository.SaveChangesAsync();

            return ServiceResult.Ok("Room type deleted successfully.");
        }
    }
}
