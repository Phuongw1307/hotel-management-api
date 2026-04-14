namespace Hotel_Management_API.Models
{
    public class RoomType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }

        public List<Room>? Rooms { get; set; } 
    }
}
