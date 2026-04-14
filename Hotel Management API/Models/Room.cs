using System.ComponentModel.DataAnnotations;

namespace Hotel_Management_API.Models
{
    public enum RoomStatus
    {
        Available = 0,     
        Occupied = 1,
        Maintenance = 2,
        Reserved = 3    
    }
    public class Room
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public int RoomTypeId { get; set; }
        public int RoomNumber { get; set; }
        public int FloorNumber { get; set; }
        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
        public RoomStatus Status { get; set; }

        public Hotel? Hotel { get; set; }
        public RoomType? RoomType { get; set; }
    }
}
