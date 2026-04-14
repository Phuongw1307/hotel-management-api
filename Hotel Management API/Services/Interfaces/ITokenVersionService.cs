namespace Hotel_Management_API.Services.Interfaces
{
    public interface ITokenVersionService
    {
        Task<bool> IsTokenVersionValidAsync(int userId, int tokenVersion);
    }
}
