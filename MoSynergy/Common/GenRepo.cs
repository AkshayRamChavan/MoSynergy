using Dapper;
using MoSynergy.Data;

namespace MoSynergy.Common
{
    public class GenRepo<T> : IGenRepo<T> where T : class
    {
        private readonly DapperDbContext _dbContext;


        public GenRepo(DapperDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task AddAsync(T entity)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var sql = GenericAdd();

                await connection.ExecuteAsync(sql, entity);
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var sql = $"DELETE FROM {typeof(T).Name} WHERE Id = @Id";

                await connection.ExecuteAsync(sql, new { Id = id });
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var sql = $"SELECT * FROM {typeof(T).Name}";

                return await connection.QueryAsync<T>(sql);
            }
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var sql = $"Select * from {typeof(T).Name} Where Id=@Id";

            using (var connection = this._dbContext.CreateConnection())
            {

                var data = await connection.QuerySingleAsync<T>(sql, new { Id = id });


                return data;


            }
        }

        public async Task UpdateAsync(T entity)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var sql = GenerateUpdate();
                await connection.ExecuteAsync(sql, entity);
            }
        }
        private string GenericAdd()
        {

            var properties = typeof(T).GetProperties().Where(p => p.Name != "Id");

            var columns = string.Join(", ", properties.Select(p => p.Name));

            var values = string.Join(", ", properties.Select(p => "@" + p.Name));


            return $"INSERT INTO {typeof(T).Name} ({columns}) VALUES ({values})";
        }
        private string GenerateUpdate()
        {
            var setClause = string.Join(", ", typeof(T).GetProperties()
                .Where(p => p.Name != "Id")
                .Select(p => $"{p.Name} = @{p.Name}"));

            var UpdatedData = $"UPDATE {typeof(T).Name} SET {setClause} WHERE Id = @Id";

            return UpdatedData;
        }
    }
}
