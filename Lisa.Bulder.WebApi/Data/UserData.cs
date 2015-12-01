using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lisa.Bulder.WebApi
{
    partial class Database
    {
        public async Task<IEnumerable<UserEntity>> FetchUsers()
        {
            var query = new TableQuery<UserEntity>();
            var segment = await _users.ExecuteQuerySegmentedAsync(query, null);
            return segment;
        }

        public async Task<UserEntity> CreateUser(UserEntity user)
        {
            user.PartitionKey = user.PartitionKey;
            user.RowKey = Guid.NewGuid().ToString();
            var operation = TableOperation.Insert(user);
            var result = await _users.ExecuteAsync(operation);
            return (UserEntity) result.Result;
        }

        private CloudTable _users;
    }
}
