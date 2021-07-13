/*
# -------------------------------------------------------------------------------
# Author:      Cody Martin <cody.martin@blacklanternsecurity.com>
#
# Created:     10-15-2020
# Copyright:   (c) BLS OPS LLC. 2020
# Licence:     GPL
# -------------------------------------------------------------------------------
*/

using DocumentFormat.OpenXml.Spreadsheet;
using Enter_The_Matrix.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enter_The_Matrix.Services
{
    public class UsersService
    {
        private readonly IMongoCollection<User> _users;
        private readonly string adminName;
        private readonly string adminPassword;

        public UsersService(IETMDatabaseSettings settings, ILocalAuthSettings authSettings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _users = database.GetCollection<User>(settings.UsersCollectionName);

            adminName = authSettings.AdminName;
            adminPassword = authSettings.AdminPassword;
        }

        public User AdminLogin(string username, string password)
        {
            User user = new User();

            if (username == adminName && password == adminPassword)
            {
                user.DisplayName = "admin";
                user.GivenName = "admin";
                user.UserName = "admin";
                user.PasswordHash = null;

                return user;
            }
            else { return user = null; }
        }

        public async Task<List<User>> GetUsers()
        {
            List<User> users = await _users.Find(u => true).ToListAsync();
            foreach (var user in users)
            {
                user.PasswordHash = null;
            }

            return users;
        }

        public async Task<User> AuthUser(string username, string password)
        {

            bool authenticated = false;
            
            // Grab the user from the DB
            User theUser = await _users.Find<User>(u => u.UserName == username).FirstOrDefaultAsync();
            
            // Check if the user exists
            // return null if does not exist
            if (theUser == null) { return null; }

            // Test if password provided matches
            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(theUser, theUser.PasswordHash, password);

            if (result == PasswordVerificationResult.Success) { authenticated = true; }
            else if (result == PasswordVerificationResult.SuccessRehashNeeded) { authenticated = true; }
            else if (result == PasswordVerificationResult.Failed) { authenticated = false; }

            // If authenticated return the valid user without the hash
            if (authenticated)
            {
                return new User
                {
                    DisplayName = theUser.DisplayName,
                    GivenName = theUser.GivenName,
                    UserName = theUser.UserName,
                    PasswordHash = null
                };
            }
            // return null if password mismatch
            else { return null; }
        }

        public async Task<User> AddUser(string username, string password, string givenName, string displayName)
        {
            User check = await _users.Find<User>(u => u.UserName == username).FirstOrDefaultAsync();
            
            // Check if user already exists
            if (check != null) { return null; }
            
            // Set up the user
            User user = new User();
            user.GivenName = givenName;
            user.DisplayName = username;
            user.UserName = username;

            // Hash the password provided for storing
            var passwordHasher = new PasswordHasher<User>();
            string hash = passwordHasher.HashPassword(user, password);
            user.PasswordHash = hash;

            // Insert into DB
            await _users.InsertOneAsync(user);

            return user;
        }

        public async Task DeleteUser(string username)
        {
            // Deletes user account
            await _users.DeleteOneAsync(u => u.UserName == username);

        }

        public async Task UpdateUser(string username, string password, string displayName, string givenName)
        {
            // Get the user ID
            string id = _users.Find<User>(u => u.UserName == username).FirstOrDefaultAsync().Result.Id;

            // Set up the user
            User user = new User
            {
                UserName = username,
                DisplayName = displayName,
                GivenName = givenName,
                Id = id
            };

            // Hash the password before updating the DB
            var passwordHasher = new PasswordHasher<User>();
            string hash = passwordHasher.HashPassword(user, password);
            user.PasswordHash = hash;

            // Update DB with new information
            await _users.ReplaceOneAsync(u => u.UserName == username, user);
        }
    }
}
