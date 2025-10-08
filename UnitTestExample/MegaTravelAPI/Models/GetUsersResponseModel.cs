using MegaTravelAPI.Data;

namespace MegaTravelAPI.Models
{
    public class GetUsersResponseModel
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string? Message { get; set; } = null;
        public List<User>? userList { get; set; } = null;
    }
}
