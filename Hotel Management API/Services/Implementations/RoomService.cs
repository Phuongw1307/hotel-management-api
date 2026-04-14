using Hotel_Management_API.DTOs;
using Hotel_Management_API.Models;
using Hotel_Management_API.Repositories.Interfaces;
using Hotel_Management_API.Services.Interfaces;

namespace Hotel_Management_API.Services.Implementations
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IRoomTypeRepository _roomTypeRepository;

        public RoomService(
            IRoomRepository roomRepository,
            IHotelRepository hotelRepository,
            IRoomTypeRepository roomTypeRepository)
        {
            _roomRepository = roomRepository;
            _hotelRepository = hotelRepository;
            _roomTypeRepository = roomTypeRepository;
        }

        public async Task<ServiceResult<List<RoomResponse>>> GetAllAsync()
        {
            var rooms = await _roomRepository.GetAllWithDetailsAsync();

            var response = rooms.Select(r => new RoomResponse
            {
                Id = r.Id,
                HotelId = r.HotelId,
                HotelName = r.Hotel?.Name ?? string.Empty,
                RoomTypeId = r.RoomTypeId,
                RoomTypeName = r.RoomType?.Name ?? string.Empty,
                RoomNumber = r.RoomNumber,
                FloorNumber = r.FloorNumber,
                Capacity = r.Capacity,
                PricePerNight = r.PricePerNight,
                Status = r.Status
            }).ToList();

            return ServiceResult<List<RoomResponse>>.Ok(response);
        }

        public async Task<ServiceResult<RoomResponse>> GetByIdAsync(int id)
        {
            var room = await _roomRepository.GetByIdWithDetailsAsync(id);
            if (room == null)
                return ServiceResult<RoomResponse>.Fail("Room not found.");

            var response = new RoomResponse
            {
                Id = room.Id,
                HotelId = room.HotelId,
                HotelName = room.Hotel?.Name ?? string.Empty,
                RoomTypeId = room.RoomTypeId,
                RoomTypeName = room.RoomType?.Name ?? string.Empty,
                RoomNumber = room.RoomNumber,
                FloorNumber = room.FloorNumber,
                Capacity = room.Capacity,
                PricePerNight = room.PricePerNight,
                Status = room.Status
            };

            return ServiceResult<RoomResponse>.Ok(response);
        }

        public async Task<ServiceResult<RoomResponse>> CreateAsync(CreateRoomRequest request)
        {
            var hotelExists = await _hotelRepository.ExistsAsync(request.HotelId);
            if (!hotelExists)
                return ServiceResult<RoomResponse>.Fail("Hotel does not exist.");

            var roomTypeExists = await _roomTypeRepository.ExistsAsync(request.RoomTypeId);
            if (!roomTypeExists)
                return ServiceResult<RoomResponse>.Fail("Room type does not exist.");

            var duplicateRoomNumber = await _roomRepository.RoomNumberExistsInHotelAsync(
                request.HotelId,
                request.RoomNumber);

            if (duplicateRoomNumber)
                return ServiceResult<RoomResponse>.Fail("Room number already exists in this hotel.");

            var room = new Room
            {
                HotelId = request.HotelId,
                RoomTypeId = request.RoomTypeId,
                RoomNumber = request.RoomNumber,
                FloorNumber = request.FloorNumber,
                Capacity = request.Capacity,
                PricePerNight = request.PricePerNight,
                Status = request.Status
            };

            await _roomRepository.AddAsync(room);
            await _roomRepository.SaveChangesAsync();

            var createdRoom = await _roomRepository.GetByIdWithDetailsAsync(room.Id);

            var response = new RoomResponse
            {
                Id = createdRoom!.Id,
                HotelId = createdRoom.HotelId,
                HotelName = createdRoom.Hotel?.Name ?? string.Empty,
                RoomTypeId = createdRoom.RoomTypeId,
                RoomTypeName = createdRoom.RoomType?.Name ?? string.Empty,
                RoomNumber = createdRoom.RoomNumber,
                FloorNumber = createdRoom.FloorNumber,
                Capacity = createdRoom.Capacity,
                PricePerNight = createdRoom.PricePerNight,
                Status = createdRoom.Status
            };

            return ServiceResult<RoomResponse>.Ok(response, "Room created successfully.");
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateRoomRequest request)
        {
            var existing = await _roomRepository.GetByIdAsync(id);
            if (existing == null)
                return ServiceResult.Fail("Room not found.");

            var hotelExists = await _hotelRepository.ExistsAsync(request.HotelId);
            if (!hotelExists)
                return ServiceResult.Fail("Hotel does not exist.");

            var roomTypeExists = await _roomTypeRepository.ExistsAsync(request.RoomTypeId);
            if (!roomTypeExists)
                return ServiceResult.Fail("Room type does not exist.");

            var duplicateRoomNumber = await _roomRepository.RoomNumberExistsInHotelAsync(
                request.HotelId,
                request.RoomNumber,
                id);

            if (duplicateRoomNumber)
                return ServiceResult.Fail("Room number already exists in this hotel.");

            existing.HotelId = request.HotelId;
            existing.RoomTypeId = request.RoomTypeId;
            existing.RoomNumber = request.RoomNumber;
            existing.FloorNumber = request.FloorNumber;
            existing.Capacity = request.Capacity;
            existing.PricePerNight = request.PricePerNight;
            existing.Status = request.Status;

            _roomRepository.Update(existing);
            await _roomRepository.SaveChangesAsync();

            return ServiceResult.Ok("Room updated successfully.");
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var existing = await _roomRepository.GetByIdAsync(id);
            if (existing == null)
                return ServiceResult.Fail("Room not found.");

            _roomRepository.Delete(existing);
            await _roomRepository.SaveChangesAsync();

            return ServiceResult.Ok("Room deleted successfully.");
        }
    }
}
