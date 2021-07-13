/*
# -------------------------------------------------------------------------------
# Author:      Cody Martin <cody.martin@blacklanternsecurity.com>
#
# Created:     10-15-2020
# Copyright:   (c) BLS OPS LLC. 2020
# Licence:     GPL
# -------------------------------------------------------------------------------
*/

using System.Collections.Generic;
using System.Threading.Tasks;
using Enter_The_Matrix.Models;
using MongoDB.Driver;

namespace Enter_The_Matrix.Services
{
    public class SteplatesService
    {
        private readonly IMongoCollection<Steplates> _steplates;

        public SteplatesService(IETMDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _steplates = database.GetCollection<Steplates>(settings.SteplatesCollectionName);
        }

        public async Task<List<Steplates>> GetAllAsync()
        {
            return await _steplates.Find(s => true).ToListAsync();
        }

        public async Task<Steplates> GetByIdAsync(string id)
        {
            return await _steplates.Find<Steplates>(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Steplates> CreateAsync(Steplates steplate)
        {
            await _steplates.InsertOneAsync(steplate);
            return steplate;
        }

        public async Task UpdateAsync(string id, Steplates steplate)
        {
            await _steplates.ReplaceOneAsync(s => s.Id == id, steplate);
        }

        public async Task DeleteAsync(string id)
        {
            await _steplates.DeleteOneAsync(s => s.Id == id);
        }
    }
}
