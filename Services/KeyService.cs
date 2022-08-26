/*
# -------------------------------------------------------------------------------
# Author:      Cody Martin <cody.martin@blacklanternsecurity.com>
#
# Created:     08-23-2022
# Copyright:   (c) BLS OPS LLC. 2022
# Licence:     GPL
# -------------------------------------------------------------------------------
*/

using Enter_The_Matrix.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;


namespace Enter_The_Matrix.Services
{
    public class KeyService
    {
        private readonly IMongoCollection<Key> _keys;
        
        public KeyService(IETMDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _keys = database.GetCollection<Key>(settings.KeysCollectionName);
        }

        public async Task<List<Key>> GetKeys(bool safe = true)
        {
            List<Key> keys = await _keys.Find(k => true).ToListAsync();
            if (safe)
            {
                foreach (var key in keys)
                {
                    key.Hash = null;
                }
            }

            return keys;
        }

        public async Task<bool> Validate(string keyPlain, string operation, string service)
        {

            bool authenticated = false;
            bool authorized = false;
            string privilege = "";

            switch (operation)
            {
                case "POST":
                    privilege = "C";
                    break;
                case "GET":
                    privilege = "R";
                    break;
                case "PUT":
                    privilege = "U";
                    break;
                case "DELETE":
                    privilege = "D";
                    break;
                default:
                    privilege = "X";
                    break;
            }

            // Hash the passed in key
            var passwordHasher = new PasswordHasher<Key>();
            Key retKey = null;
            foreach (Key key in await GetKeys(false)){
                var result = passwordHasher.VerifyHashedPassword(key, key.Hash, keyPlain);
                if (result == PasswordVerificationResult.Success) { authenticated = true; retKey = key; break; }
                else if (result == PasswordVerificationResult.SuccessRehashNeeded) { authenticated = true; retKey = key; break; }
            }

            if (authenticated)
            {
                switch (service)
                {
                    case "Assessments":
                        authorized = retKey.AssessmentPrivileges.Contains(privilege);
                        break;
                    case "Scenarios":
                        authorized = retKey.ScenarioPrivileges.Contains(privilege);
                        break;
                    case "Events":
                        authorized = retKey.EventPrivileges.Contains(privilege);
                        break;
                    case "Templates":
                        authorized = retKey.TemplatePrivileges.Contains(privilege);
                        break;
                    case "Metrics":
                        authorized = retKey.MetricsPrivileges.Contains(privilege);
                        break;
                    default:
                        authorized = false;
                        break;
                }
            }

            return authorized;
        }

        public async Task<string> AddKey(string name, 
            List<string> assessmentPrivileges, 
            List<string> scenarioPrivileges,
            List<string> eventPrivileges,
            List<string> templatePrivileges,
            List<string> metricsPrivileges)
        {
            Key check = await _keys.Find<Key>(k => k.Name == name).FirstOrDefaultAsync();
            
            // Check if key already exists
            if (check != null) { return null; }
            
            // Set up the new key
            Key key = new Key();
            key.Name = name;
            key.AssessmentPrivileges = assessmentPrivileges;
            key.ScenarioPrivileges = scenarioPrivileges;
            key.EventPrivileges = eventPrivileges;
            key.TemplatePrivileges = templatePrivileges;
            key.MetricsPrivileges = metricsPrivileges;

            // Using MSFT Built-In GUID generation
            string keyVal = Guid.NewGuid().ToString();

            // Hashing the GUID to be stored in DB
            var passwordHasher = new PasswordHasher<Key>();
            string hash = passwordHasher.HashPassword(key, keyVal);
            key.Hash = hash;

            // Insert into DB
            await _keys.InsertOneAsync(key);

            return keyVal;
        }

        public async Task DeleteKey(string name)
        {
            // Deletes user account
            await _keys.DeleteOneAsync(k => k.Name == name);
            return;
        }
    }
}
