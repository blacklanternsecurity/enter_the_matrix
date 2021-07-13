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
    public class ScenariosService
    {
        private readonly IMongoCollection<Scenarios> _scenarios;

        public ScenariosService(IETMDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _scenarios = database.GetCollection<Scenarios>(settings.ScenariosCollectionName);
        }

        public async Task<List<Scenarios>> GetAllAsync()
        {
            return await _scenarios.Find(s => true).ToListAsync();
        }

        public async Task<Scenarios> GetByIdAsync(string id)
        {
            return await _scenarios.Find<Scenarios>(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Scenarios> CreateAsync(Scenarios scenario)
        {
            await _scenarios.InsertOneAsync(scenario);
            return scenario;
        }

        public async Task UpdateAsync(string id, Scenarios scenario)
        {
            await _scenarios.ReplaceOneAsync(s => s.Id == id, scenario);
        }

        public async Task DeleteAsync(string id)
        {
            await _scenarios.DeleteOneAsync(s => s.Id == id);
        }
    }
}
