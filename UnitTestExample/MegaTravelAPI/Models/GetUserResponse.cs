using MegaTravelAPI.Data;

namespace MegaTravelAPI.Models
{
    public class GetUserResponse
    {
        public int StatusCode { get; set; }
        public bool Status { get; set; }
        public string? Message { get; set; } = null;
        public string? Token { get; set; } = null;
        public List<User>? Data { get; set; } = null;
        public Error? Errors { get; set; } = null;

    }

}
