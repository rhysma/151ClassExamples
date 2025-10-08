using System;
using System.Collections.Generic;

namespace MegaTravelAPI.Data
{
    public partial class User
    {
        public User()
        {
            Trips = new HashSet<Trip>();
        }

        public int UserId { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Street1 { get; set; }
        public string? Street2 { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required int ZipCode { get; set; }
        public required string Phone { get; set; }

        public virtual Login LoginInfo { get; set; } = null!;
        public virtual ICollection<Trip> Trips { get; set; }
    }
}
