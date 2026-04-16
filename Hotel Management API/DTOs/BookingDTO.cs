namespace Hotel_Management_API.DTOs
{
    public class CreateBookingRequest
    {
        public int HotelId { get; set; }
        public int RoomId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public DateTime CheckInDate { get; set; } 
        public DateTime CheckOutDate { get; set; }
        public string? Note { get; set; }
    }

    public class CreateBookingResponse
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public string HotelName { get; set; } = string.Empty;
        public int RoomId { get; set; }
        public int RoomNumber { get; set; } 

        public int AppUserId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        public decimal PricePerNight { get; set; }
        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime? ActualCheckInAt { get; set; }
        public DateTime? ActualCheckOutAt { get; set; }

        public string? Note { get; set; }
    }
}
