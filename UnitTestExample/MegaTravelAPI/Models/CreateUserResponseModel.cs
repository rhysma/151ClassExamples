using MegaTravelAPI.Data;

namespace MegaTravelAPI.Models
{
    public class CreateUserResponseModel
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string? Message { get; set; } = null;
        public UserData? Data { get; set; } = null;
    }
}
