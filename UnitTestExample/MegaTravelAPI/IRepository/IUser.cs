﻿using MegaTravelAPI.Models;
using MegaTravelAPI.Data;


namespace MegaTravelAPI.IRepository
{
    public interface IUser
    {
        /// <summary>
        /// Returns a list of all registered users
        /// </summary>
        /// <returns></returns>
        List<User> GetAllUsers();

        /// <summary>
        /// Saves a new user who has registered
        /// </summary>
        /// <param name="usermodel"></param>
        /// <returns></returns>
        Task<SaveUserResponse> SaveUserRecord(RegistrationModel usermodel);


        /// <summary>
        /// Logs in an already registered user
        /// </summary>
        /// <param name="tokenData"></param>
        /// <returns></returns>
        Task<LoginResponse> LoginUser(LoginModel tokenData);

        /// <summary>
        /// Finds the user data for a user based on username
        /// </summary>
        /// <param name="tokenData"></param>
        /// <returns></returns>
        Task<UserData> FindByName(string username);


        /// <summary>
        /// Updates an existing user record
        /// </summary>
        /// <param name="updatedUser"></param>
        /// <returns></returns>
        Task<SaveUserResponse> UpdateUserRecord(UserData updatedUser);
    }
}
