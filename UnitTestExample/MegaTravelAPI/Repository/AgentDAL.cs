using MegaTravelAPI.IRepository;
using MegaTravelAPI.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using System.Text;

namespace MegaTravelAPI.Data
{
    public class AgentDAL : IAgent
    {
        //context for the database connection
        private readonly MegaTravelContext context;

        public AgentDAL(MegaTravelContext Context)
        {
            context = Context;
        }

        public AgentDAL(MegaTravelContext Context, object value)
        {
            context = Context;
        }



        #region Login Agent Method
        public async Task<LoginResponse> LoginAgent(LoginModel tokenData)
        {
            LoginResponse res = new LoginResponse();
            try
            {
                if (tokenData != null)
                {
                    // Use await to make the query asynchronous
                    var query = await context.Agents
                        .Where(x => x.LoginInfo.UserName == tokenData.Username && x.LoginInfo.Password == tokenData.Password)
                        .FirstOrDefaultAsync();

                    // If query has a result then we have a match
                    if (query != null)
                    {
                        res.Status = true;
                        res.StatusCode = 200;
                        res.Message = "Login Success";
                        return res;
                    }
                    else
                    {
                        // The user wasn't found or wasn't a match
                        res.Status = false;
                        res.StatusCode = 500;
                        res.Message = "Login Failed";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("LoginAgent --- " + ex.Message);
                res.Status = false;
                res.StatusCode = 500;
            }

            return res;
        }


        public async Task<Agent> FindByName(string username)
        {
            Agent? agent = null;

            try
            {
                if (!string.IsNullOrEmpty(username))
                {
                    // Use await to make the query asynchronous
                    var query = await context.Agents
                        .Where(x => x.LoginInfo.UserName == username)
                        .FirstOrDefaultAsync();

                    if (query != null)
                    {
                        // Set up the object so we can return it
                        agent = new Agent
                        {
                            AgentId = query.AgentId,
                            FirstName = query.FirstName,
                            LastName = query.LastName,
                            OfficeLocation = query.OfficeLocation,
                            Phone = query.Phone
                        };
                    }
                }

                // Return the agent object, ensuring it is not null
                return agent ?? throw new InvalidOperationException("Agent not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("FindByName --- " + ex.Message);
                throw;
            }
        }
        public async Task<Agent> FindByID(int agentID)
        {
            Agent? agent = null;

            try
            {
                if (agentID > 0)
                {
                    //query the database to find the agent who has this username
                    var query = await context.Agents
                        .Where(x => x.LoginInfo.Id == agentID)
                        .FirstOrDefaultAsync();

                    if (query != null)
                    {
                        //set up the object so we can return it
                        agent = new Agent
                        {
                            AgentId = query.AgentId,
                            FirstName = query.FirstName,
                            LastName = query.LastName,
                            OfficeLocation = query.OfficeLocation,
                            Phone = query.Phone
                        };
                    }
                }

                if (agent == null)
                {
                    throw new InvalidOperationException("Agent not found.");
                }

                return agent;
            }
            catch (Exception ex)
            {
                Console.WriteLine("FindByID --- " + ex.Message);
                throw;
            }
        }
        #endregion

        #region Get All Trips Method
        /// <summary>
        /// Method to get all trips in the database
        /// </summary>
        /// <returns></returns>

        public List<TripData> GetAllTrips()
        {
            List<TripData> tripList = new List<TripData>();

            try
            {
                //query the database to get all of the trips
                var trips = context.Trips.ToList();

                foreach(Trip trip in trips)
                {
                    //get all of the object data to send back
                    tripList.Add(new TripData()
                    {
                        UserID = trip.UserId,
                        AgentID = trip.AgentId,
                        TripID = trip.TripId,
                        TripName = trip.TripName,
                        Location = trip.Location,
                        StartDate = trip.StartDate,
                        EndDate = trip.EndDate,
                        NumAdults = trip.NumAdults,
                        NumChildren = trip.NumChildren
                    });


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetAllTrips --- " + ex.Message);
                throw;
            }

            return tripList;
        }
        #endregion

        #region Get All Trips For Agent
        /// <summary>
        /// Method to get all trips in the database for a particular agent
        /// </summary>
        /// <returns></returns>

        public List<TripData> GetAllTripsForAgent(int agentID)
        {
            List<TripData> tripList = new List<TripData>();
            UserData selectedUser = null;

            try
            {
                var trips = context.Trips.Where(x => x.AgentId == agentID).ToList();

                foreach (Trip trip in trips)
                {

                    ////query the database to get the user by id
                    var query = context.Users.Where(x => x.UserId == trip.UserId).FirstOrDefault<User>();

                    if (query != null) {

                        selectedUser = new UserData
                        {

                            UserId = query.UserId,
                            FirstName = query.FirstName,
                            LastName = query.LastName,
                            Street1 = query.Street1,
                            Street2 = query.Street2,
                            City = query.City,
                            State = query.State,
                            ZipCode = query.ZipCode,
                            Phone = query.Phone,
                            

                        };

                    }


                    //get all of the object data to send back
                    tripList.Add(new TripData()
                    {

                        UserID = trip.UserId,
                        AgentID = trip.AgentId,
                        TripID = trip.TripId,
                        TripName = trip.TripName,
                        Location = trip.Location,
                        StartDate = trip.StartDate,
                        EndDate = trip.EndDate,
                        NumAdults = trip.NumAdults,
                        NumChildren = trip.NumChildren,
                        userInfo = selectedUser
                    });

                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetAllTripsForAgent --- " + ex.Message);
                throw;
            }

            return tripList;
        }
        #endregion

    }
}
