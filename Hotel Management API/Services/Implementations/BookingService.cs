using Hotel_Management_API.DTOs;
using Hotel_Management_API.Models;
using Hotel_Management_API.Repositories.Interfaces;
using Hotel_Management_API.Services.Interfaces;

namespace Hotel_Management_API.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IRoomRepository _roomRepository;

        public BookingService(
            IBookingRepository bookingRepository,
            IHotelRepository hotelRepository,
            IRoomRepository roomRepository)
        {
            _bookingRepository = bookingRepository;
            _hotelRepository = hotelRepository;
            _roomRepository = roomRepository;
        }

        public async Task<ServiceResult<List<CreateBookingResponse>>> GetAllAsync()
        {
            var bookings = await _bookingRepository.GetAllAsync();

            var response = bookings.Select(MapToResponse).ToList();

            return ServiceResult<List<CreateBookingResponse>>.Ok(response);
        }

        public async Task<ServiceResult<CreateBookingResponse>> GetByIdAsync(int id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
                return ServiceResult<CreateBookingResponse>.Fail("Booking not found.");

            return ServiceResult<CreateBookingResponse>.Ok(MapToResponse(booking));
        }

        public async Task<ServiceResult<CreateBookingResponse>> CreateAsync(int appUserId, CreateBookingRequest request)
        {
            var hotel = await _hotelRepository.GetByIdAsync(request.HotelId);
            if (hotel == null)
                return ServiceResult<CreateBookingResponse>.Fail("Hotel not found.");

            var room = await _roomRepository.GetByIdAsync(request.RoomId);
            if (room == null)
                return ServiceResult<CreateBookingResponse>.Fail("Room not found.");

            if (room.HotelId != request.HotelId)
                return ServiceResult<CreateBookingResponse>.Fail("The selected room does not belong to the specified hotel.");

            if (room.Status != RoomStatus.Available)
                return ServiceResult<CreateBookingResponse>.Fail("Room is not active.");

            var hasOverlap = await _bookingRepository.HasOverlappingBookingAsync(
                request.RoomId,
                request.CheckInDate,
                request.CheckOutDate);

            if (hasOverlap)
                return ServiceResult<CreateBookingResponse>.Fail("Room is already booked for the selected date range.");

            var numberOfNights = (request.CheckOutDate.Date - request.CheckInDate.Date).Days;
            if (numberOfNights <= 0)
                return ServiceResult<CreateBookingResponse>.Fail("Invalid booking date range.");

            var booking = new Booking
            {
                HotelId = request.HotelId,
                RoomId = request.RoomId,
                AppUserId = appUserId,

                CustomerName = request.CustomerName,
                CustomerPhone = request.CustomerPhone,
                CustomerEmail = request.CustomerEmail,

                CheckInDate = request.CheckInDate.Date,
                CheckOutDate = request.CheckOutDate.Date,

                PricePerNight = room.PricePerNight,
                TotalAmount = numberOfNights * room.PricePerNight,

                Status = BookingStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                Note = request.Note
            };

            await _bookingRepository.AddAsync(booking);
            await _bookingRepository.SaveChangesAsync();

            var createdBooking = await _bookingRepository.GetByIdAsync(booking.Id);
            if (createdBooking == null)
                return ServiceResult<CreateBookingResponse>.Fail("Booking created but failed to retrieve data.");

            return ServiceResult<CreateBookingResponse>.Ok(
                MapToResponse(createdBooking),
                "Booking created successfully.");
        }

        public async Task<ServiceResult<List<AvailableRoomResponse>>> GetAvailableRoomsAsync(
            int hotelId,
            DateTime checkInDate,
            DateTime checkOutDate)
        {
            var hotel = await _hotelRepository.GetByIdAsync(hotelId);
            if (hotel == null)
                return ServiceResult<List<AvailableRoomResponse>>.Fail("Hotel not found.");

            if (checkOutDate.Date <= checkInDate.Date)
                return ServiceResult<List<AvailableRoomResponse>>.Fail("Check-out date must be greater than check-in date.");

            var rooms = await _roomRepository.GetAllAsync();

            var hotelRooms = rooms
                .Where(r => r.HotelId == hotelId && r.Status == RoomStatus.Available)
                .ToList();

            var availableRooms = new List<AvailableRoomResponse>();

            foreach (var room in hotelRooms)
            {
                var hasOverlap = await _bookingRepository.HasOverlappingBookingAsync(
                    room.Id,
                    checkInDate.Date,
                    checkOutDate.Date);

                if (!hasOverlap)
                {
                    availableRooms.Add(new AvailableRoomResponse
                    {
                        RoomId = room.Id,
                        RoomNumber = room.RoomNumber,
                        HotelId = room.HotelId,
                        RoomTypeId = room.RoomTypeId,
                        RoomTypeName = room.RoomType?.Name ?? string.Empty,
                        PricePerNight = room.PricePerNight
                    });
                }
            }

            return ServiceResult<List<AvailableRoomResponse>>.Ok(availableRooms);
        }

        private static CreateBookingResponse MapToResponse(Booking booking)
        {
            return new CreateBookingResponse
            {
                Id = booking.Id,
                HotelId = booking.HotelId,
                HotelName = booking.Hotel?.Name ?? string.Empty,

                RoomId = booking.RoomId,
                RoomNumber = booking.Room.RoomNumber,

                AppUserId = booking.AppUserId,

                CustomerName = booking.CustomerName,
                CustomerPhone = booking.CustomerPhone,
                CustomerEmail = booking.CustomerEmail,

                CheckInDate = booking.CheckInDate,
                CheckOutDate = booking.CheckOutDate,

                PricePerNight = booking.PricePerNight,
                TotalAmount = booking.TotalAmount,

                Status = booking.Status,
                CreatedAt = booking.CreatedAt,
                ActualCheckInAt = booking.ActualCheckInAt,
                ActualCheckOutAt = booking.ActualCheckOutAt,

                Note = booking.Note
            };
        }
    }
}
