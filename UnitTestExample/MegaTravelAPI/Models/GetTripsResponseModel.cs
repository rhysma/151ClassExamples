using MegaTravelAPI.Data;

namespace MegaTravelAPI.Models
{
    public class GetTripsResponseModel
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string? Message { get; set; } = null;
        public List<TripData>? tripList { get; set; } = null;
        public User? userData { get; set; } = null;
    }
}
