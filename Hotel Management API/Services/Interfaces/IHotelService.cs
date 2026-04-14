using Hotel_Management_API.Dtos;

namespace Hotel_Management_API.Services.Interfaces
{
    public interface IHotelService
    {
        Task<ServiceResult<List<HotelResponse>>> GetAllAsync();
        Task<ServiceResult<HotelResponse>> GetByIdAsync(int id);
        Task<ServiceResult<HotelResponse>> CreateAsync(CreateHotelRequest request);
        Task<ServiceResult> UpdateAsync(int id, UpdateHotelRequest request);
        Task<ServiceResult> DeleteAsync(int id);
    }
}
