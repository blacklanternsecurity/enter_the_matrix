/*
# -------------------------------------------------------------------------------
# Author:      Cody Martin <cody.martin@blacklanternsecurity.com>
#
# Created:     10-15-2020
# Copyright:   (c) BLS OPS LLC. 2020
# Licence:     GPL
# -------------------------------------------------------------------------------
*/

using Enter_The_Matrix.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Enter_The_Matrix.Services
{
    public class AssessmentsService
    {
        private readonly IMongoCollection<Assessments> _assessments;

        public AssessmentsService(IETMDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _assessments = database.GetCollection<Assessments>(settings.AssessmentsCollectionName);
        }

        public async Task<List<Assessments>> GetAllAsync()
        {
            return await _assessments.Find(a => true).ToListAsync();
        }

        public async Task<Assessments> GetByIdAsync(string id)
        {
            return await _assessments.Find<Assessments>(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Assessments> CreateAsync(Assessments assessment)
        {
            await _assessments.InsertOneAsync(assessment);
            return assessment;
        }

        public async Task UpdateAsync(string id, Assessments assessment)
        {
            await _assessments.ReplaceOneAsync(a => a.Id == id, assessment);
        }

        public async Task DeleteAsync(string id)
        {
            await _assessments.DeleteOneAsync(a => a.Id == id);
        }
    }
}
