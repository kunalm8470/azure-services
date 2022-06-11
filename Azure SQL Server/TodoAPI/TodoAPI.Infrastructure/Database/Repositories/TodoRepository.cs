using Dapper;
using System.Data;
using TodoAPI.Domain.Contracts;
using TodoAPI.Domain.Entities;

namespace TodoAPI.Infrastructure.Database.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly SqlServerConnectionFactory _connectionFactory;
        private int _commandTimeoutSecs;

        public TodoRepository(SqlServerConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _commandTimeoutSecs = 30;
        }

        public async Task AddAsync(Todo item, CancellationToken token = default)
        {
            string sql = @"INSERT INTO
                [Todo](Title, Description)
                VALUES(@title, @description);";

            DynamicParameters parameters = new();
            parameters.Add("@title", item.Title, DbType.String, ParameterDirection.Input, item.Title.Length);
            parameters.Add("@description", item.Description, DbType.String, ParameterDirection.Input, item.Description.Length);

            using IDbConnection conn = _connectionFactory.Connect();

            await conn.ExecuteAsync(sql, param: parameters, commandTimeout: _commandTimeoutSecs).ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id, CancellationToken token = default)
        {
            string sql = @"DELETE FROM [Todo]
            WHERE [Id] = @id";

            DynamicParameters parameters = new();
            parameters.Add("@id", id, DbType.Int32, ParameterDirection.Input);

            using IDbConnection conn = _connectionFactory.Connect();

            await conn.ExecuteAsync(sql, param: parameters, commandTimeout: _commandTimeoutSecs).ConfigureAwait(false);
        }

        public async Task<Todo> GetByIdAsync(int id, CancellationToken token = default)
        {
            string sql = @"SELECT * 
            FROM [Todo]
            WITH (NOLOCK)
            WHERE [Id] = @id";

            DynamicParameters parameters = new();
            parameters.Add("@id", id, DbType.Int32, ParameterDirection.Input);

            using IDbConnection conn = _connectionFactory.Connect();

            return await conn.QueryFirstAsync<Todo>(sql, param: parameters, commandTimeout: _commandTimeoutSecs).ConfigureAwait(false);
        }

        public async Task<(IEnumerable<Todo>, int)> ListAsync(int skip, int limit, CancellationToken token = default)
        {
            using IDbConnection conn = _connectionFactory.Connect();

            string countSql = @"SELECT COUNT(*) FROM [Todo];";
            int count = await conn.ExecuteScalarAsync<int>(countSql, commandTimeout: _commandTimeoutSecs).ConfigureAwait(false);

            string listSql = @"SELECT *
            FROM [Todo]
            ORDER BY [Id]
            OFFSET @skip ROWS 
            FETCH NEXT @limit ROWS ONLY;";

            DynamicParameters parameters = new();
            parameters.Add("@skip", skip, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@limit", limit, DbType.Int32, ParameterDirection.Input);

            IEnumerable<Todo> todos = await conn.QueryAsync<Todo>(listSql, param: parameters, commandTimeout: _commandTimeoutSecs).ConfigureAwait(false);

            return (todos, count);
        }

        public async Task UpdateAsync(Todo item, CancellationToken token = default)
        {
            string sql = @"UPDATE [Todo]
            SET [Title] = @title,
            [Description] = @description,
            [Completed] = @completed
            WHERE [Id] = @id;";

            DynamicParameters parameters = new();
            parameters.Add("@title", item.Title, DbType.String, ParameterDirection.Input, item.Title.Length);
            parameters.Add("@description", item.Description, DbType.String, ParameterDirection.Input, item.Description.Length);
            parameters.Add("@completed", item.Completed, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("@id", item.Id, DbType.Int32, ParameterDirection.Input);

            using IDbConnection conn = _connectionFactory.Connect();

            await conn.ExecuteAsync(sql, param: parameters, commandTimeout: _commandTimeoutSecs).ConfigureAwait(false);
        }
    }
}
