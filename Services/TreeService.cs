/*
# -------------------------------------------------------------------------------
# Author:      Cody Martin <cody.martin@blacklanternsecurity.com>
#
# Created:     05-07-2021
# Copyright:   (c) BLS OPS LLC. 2021
# Licence:     GPL
# -------------------------------------------------------------------------------
*/

using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Enter_The_Matrix.Models;

namespace Enter_The_Matrix.Services
{
    public class TreeService
    {
        private readonly IMongoCollection<ThreatTree> _threatTrees;

        public TreeService(IETMDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _threatTrees = database.GetCollection<ThreatTree>(settings.TreesCollectionName);
        }

        public async Task<List<ThreatTree>> GetAllAsync()
        {
            return await _threatTrees.Find(s => true).ToListAsync();
        }

        public async Task<ThreatTree> GetByIdAsync(string id)
        {
            return await _threatTrees.Find<ThreatTree>(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ThreatTree> CreateAsync(ThreatTree tree)
        {
            await _threatTrees.InsertOneAsync(tree);
            return tree;
        }

        public async Task UpdateAsync(string id, ThreatTree tree)
        {
            await _threatTrees.ReplaceOneAsync(s => s.Id == id, tree);
        }

        public async Task DeleteAsync(string id)
        {
            await _threatTrees.DeleteOneAsync(s => s.Id == id);
        }

    }
}
