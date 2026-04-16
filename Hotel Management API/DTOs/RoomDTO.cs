using Hotel_Management_API.Models;

namespace Hotel_Management_API.DTOs
{
    public class CreateRoomRequest
    {
        public int HotelId { get; set; }
        public int RoomTypeId { get; set; }
        public int RoomNumber { get; set; }
        public int FloorNumber { get; set; }
        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
        public RoomStatus Status { get; set; }
    }

    public class UpdateRoomRequest
    {
        public int HotelId { get; set; }
        public int RoomTypeId { get; set; }
        public int RoomNumber { get; set; }
        public int FloorNumber { get; set; }
        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
        public RoomStatus Status { get; set; }
    }

    public class RoomResponse
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public string HotelName { get; set; } = string.Empty;

        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; } = string.Empty;

        public int RoomNumber { get; set; }
        public int FloorNumber { get; set; }
        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
        public RoomStatus Status { get; set; }
    }

    public class AvailableRoomResponse
    {
        public int RoomId { get; set; }
        public int RoomNumber { get; set; }

        public int HotelId { get; set; }
        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; } = string.Empty;

        public decimal PricePerNight { get; set; }
    }
}
