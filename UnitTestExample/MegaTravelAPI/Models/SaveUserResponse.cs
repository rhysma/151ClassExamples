using MegaTravelAPI.Data;

namespace MegaTravelAPI.Models
{
    public class SaveUserResponse
    {
        public int StatusCode { get; set; }
        public bool Status { get; set; }
        public string? Message { get; set; } = null;
        public string? Token { get; set; } = null;
        public User? Data { get; set; } = null;
        public Error? Errors { get; set; } = null;
        public int UserId { get; set; }

    }

    public class Error
    {
        public string? Code { get; set; } = null;
        public string? Description { get; set; } = null;
    }
}
