using Microsoft.AspNetCore.Mvc;
using MegaTravelAPI.Data;
using MegaTravelAPI.IRepository;
using MegaTravelAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace MegaTravelAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Route("/")]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        // private readonly IUser repository;
        private readonly IUser repository;

        //context for the database connection
        private readonly MegaTravelContext context;

        //variable for holding the configuration data for login authentication
        private IConfiguration config;

        public UserController(IConfiguration Config)
        {
            config = Config;
            context = new MegaTravelContext(config);
            repository = new UserDAL(context, config);
            

        }



        [HttpGet("GetUsers", Name = "GetUsers")]
        [AllowAnonymous]
        public async Task<GetUsersResponseModel> GetAllUsers()
        {
            GetUsersResponseModel response = new GetUsersResponseModel();

            //set up a list to hold the incoming users we will get from the db
            List<User> userList = new List<User>();
            try
            {
                // Use Task.Run to ensure the method runs asynchronously
                userList = await Task.Run(() => repository.GetAllUsers());

                //check the list isn't empty
                if (userList.Count != 0)
                {
                    response.Status = true;
                    response.StatusCode = 200;
                    response.userList = userList;
                }
                else
                {
                    //there has been an error
                    response.Status = false;
                    response.Message = "Get Failed";
                    response.StatusCode = 0;
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Get Failed";
                response.StatusCode = 0;
                //there has been an error
                Console.WriteLine(ex.Message);
            }
            return response;
        }


        

        [HttpPost("RegisterUser", Name = "RegisterUser")]
        [AllowAnonymous]
        public async Task<CreateUserResponseModel> RegisterUser(RegistrationModel userData)
        {
            CreateUserResponseModel response = new CreateUserResponseModel();

            if (userData != null)
            {
                try
                {
                    //call the method that will save the user record
                    var user = await repository.SaveUserRecord(userData).ConfigureAwait(true);

                    if (user != null && user.Data != null && user.StatusCode == 200) //status success
                    {
                        //user has been added
                        response.Status = true;
                        response.Message = "Registration Successful";
                        response.StatusCode = 200;

                        // send back the user information we just added
                        response.Data = new UserData
                        {
                            UserId = user.Data.UserId,
                            FirstName = user.Data.FirstName,
                            LastName = user.Data.LastName,
                            Email = user.Data.Email,
                            Phone = user.Data.Phone,
                            Street1 = user.Data.Street1,
                            Street2 = user.Data.Street2 ?? string.Empty, // Fix for null
                            City = user.Data.City,
                            State = user.Data.State,
                            ZipCode = user.Data.ZipCode
                        };
                    }
                    else
                    {
                        //there has been an error
                        response.Status = false;
                        response.Message = "Registration Failed";
                        response.StatusCode = 0;
                    }
                }
                catch (Exception ex)
                {
                    //there has been an error
                    response.Status = false;
                    response.Message = "Registration Failed";
                    response.StatusCode = 0;
                    Console.WriteLine(ex.Message);
                }
            }

            return response;
        }

        [HttpPost("LoginUser", Name = "LoginUser")]
        [AllowAnonymous]
        public async Task<LoginResponse> LoginUser(LoginModel tokenData)
        {
            LoginResponse response = new LoginResponse();
            try
            {
                if (tokenData != null && !string.IsNullOrWhiteSpace(tokenData.Username))
                {
                    //call the method that will check the user credentials
                    var loginResult = await repository.LoginUser(tokenData).ConfigureAwait(true);

                    if (loginResult.StatusCode == 200)
                    {
                        //login check has succeeded

                        //query the database to get the information about our logged in user
                        var user = await repository.FindByName(tokenData.Username).ConfigureAwait(true);

                        if (user != null)
                        {
                            //generate the authentication token
                            var tokenString = GenerateJwtToken(tokenData);

                            response.StatusCode = 200;
                            response.Status = true;
                            response.Message = "Login Successful";
                            response.AccountType = tokenData.UserType;
                            response.Authtoken = tokenString;
                            response.Data = new UserData
                            {
                                UserId = user.UserId,
                                Email = user.Email,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                Street1 = user.Street1,
                                Street2 = user.Street2,
                                City = user.City,
                                State = user.State,
                                ZipCode = user.ZipCode,
                                Phone = user.Phone
                            };
                           
                            return response;
                        }
                    }
                    else
                    {
                        response.StatusCode = 500;
                        response.Status = false;
                        response.Message = "Login Failed";
                    }
                }
                else
                {
                    response.StatusCode = 400;
                    response.Status = false;
                    response.Message = "Invalid Username";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("LoginUser --- " + ex.Message);
                response.StatusCode = 500;
                response.Status = false;
                response.Message = "Login Failed";
            }

            return response;
        }

        /// <summary>
        /// generate the token for registration
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private string GenerateJwtToken(LoginModel userInfo)
        {
            // 1) Guard against null userInfo or missing Username
            if (userInfo == null)
                throw new ArgumentNullException(nameof(userInfo));
            var username = userInfo.Username
                           ?? throw new ArgumentException(
                                  "Username cannot be null",
                                  nameof(userInfo.Username));

            // 2) Pull config values into locals and fail fast if they’re missing
            var secretKey = config["TokenAuthentication:SecretKey"]
                            ?? throw new InvalidOperationException(
                                   "Configuration value 'TokenAuthentication:SecretKey' is missing.");
            var issuer = config["TokenAuthentication:Issuer"]
                         ?? throw new InvalidOperationException(
                                "Configuration value 'TokenAuthentication:Issuer' is missing.");

            // 3) Now both are non-null for GetBytes and Claim ctor
            var securityKey = new SymmetricSecurityKey(
                                  Encoding.UTF8.GetBytes(secretKey)
                              );
            var credentials = new SigningCredentials(
                                  securityKey,
                                  SecurityAlgorithms.HmacSha256
                              );

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, username),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var expires = DateTime.UtcNow.AddDays(15);
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: issuer,
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        // Endpoint method needs some things:
        // What type of method is this (HttpPost? HttpGet?) What decorators?

        [HttpPost("UpdateUserRecord", Name = "UpdateUserRecord")] //Info for Swagger
        [AllowAnonymous] // no user or login required; public and open endpoint for anyone to use
        public async Task<CreateUserResponseModel> UpdateUserRecord(UserData user) 
        {

            CreateUserResponseModel response = new CreateUserResponseModel();

            try
            {
                // call the method in the UserDAL that saves the user record
                // anytime you have an instruction that might take a bit, if you use an async method, you need to use an await to get any data that might be received, otherwise you can get null data even if it exists; timing is important
                var userResponse = await repository.UpdateUserRecord(user).ConfigureAwait(true);
               
                // gets a status
                if (userResponse.StatusCode == 200) {


                    response.Data = new UserData();
                    // user has been updated, send back the user info to the client
                    response.Status = true;
                    response.Message = "Update Successful";
                    response.StatusCode = 200;

                    response.Data.UserId = userResponse.Data.UserId;
                    response.Data.FirstName = userResponse.Data.FirstName;
                    response.Data.LastName = userResponse.Data.LastName;
                    response.Data.Email = userResponse.Data.Email;
                    response.Data.State = userResponse.Data.State;
                    response.Data.Street1 = userResponse.Data.Street1;
                    if(userResponse == null) {
                        userResponse.Data.Street2 = string.Empty;
                    }
                    response.Data.Street2 = userResponse.Data.Street2;
                    response.Data.ZipCode = userResponse.Data.ZipCode;
                    response.Data.City = userResponse.Data.City;
                    response.Data.Phone = userResponse.Data.Phone;
                }

                else {

                    // There has been an error
                    response.Status = false;
                    response.Message = "Update Failed";
                    response.StatusCode = 500;
                }


                
            }
            catch (Exception ex)
            {

                // There has been an error
                response.Status = false;
                response.Message = "Update Failed";
                response.StatusCode = 500;

            }

            return response;
        }


    } // End of Class (EoC)
} // End of File (EoF)