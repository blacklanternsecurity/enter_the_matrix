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
using MongoDB.Driver;
using Enter_The_Matrix.Models;

namespace Enter_The_Matrix.Services
{
    public class StepsService
    {
        private readonly IMongoCollection<Steps> _steps;

        public StepsService(IETMDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _steps = database.GetCollection<Steps>(settings.StepsCollectionName);
        }

        public async Task<List<Steps>> GetAllAsync()
        {
            return await _steps.Find(s => true).ToListAsync();
        }

        public async Task<Steps> GetByIdAsync(string id)
        {
            return await _steps.Find<Steps>(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Steps> CreateAsync(Steps step)
        {
            await _steps.InsertOneAsync(step);
            return step;
        }

        public async Task UpdateAsync(string id, Steps step)
        {
            await _steps.ReplaceOneAsync(s => s.Id == id, step);
        }

        public async Task DeleteAsync(string id)
        {
            await _steps.DeleteOneAsync(s => s.Id == id);
        }
    }
}
