using AutoMapper;
using Dapper;
using ResultViewer.Server.Context;
using ResultViewer.Server.Dtos.ResultDto;
using ResultViewer.Server.Models;
using ResultViewer.Server.Repositories.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace ResultViewer.Server.Repositories
{
    class ResultRepository: IResultRepository
    {
        private readonly DbContext _context;

        public ResultRepository(DbContext context) => _context = context;

        public async Task<Result> CreateResult(ResultForCreationDto result)
        {
            var query = "INSERT INTO Results (ResultName, Status, Date) Values (@ResultName, @Status, @Date)" +
                        "SELECT CAST(SCOPE_IDENTITY() AS int)";
            var parameters = new DynamicParameters();

            parameters.Add("ResultName", result.ResultName, DbType.String);
            parameters.Add("Status", result.Status ? 1 : 0, DbType.Int32);
            parameters.Add("Date", result.Date.ToString("yyyy-DD-mm"), DbType.Date);

            using (var connection = _context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, parameters);
                var createdResult = new Result
                {
                    Id = id,
                    Status = result.Status,
                    Date = result.Date,
                    ResultName = result.ResultName
                };
                
                return createdResult;
            }
        }

        public async Task<Result> GetResultById(int resultId)
        {
            var query = "SELECT * FROM Results WHERE Id = @ParamId";
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryFirstAsync<Result>(query, new { ParamId = resultId });
                return result;
            }
        }

        public async Task<List<Result>> GetAllResults()
        {
            var query = "SELECT * FROM Results";
            using (var connection = _context.CreateConnection())
            {
                var results = await connection.QueryAsync<Result>(query);
                return results.ToList();
            }
        }

        public async Task UpdateResult(int id, ResultForUpdateDto result)
        {
            var query = "UPDATE Results SET ResultName = @ResultName, Status = @Status, Date = @Date WHERE Id = @Id";

            var parameters = new DynamicParameters();

            parameters.Add("Id", id, DbType.Int32);
            parameters.Add("ResultName", result.ResultName, DbType.String);
            parameters.Add("Status", result.Status ? 1 : 0, DbType.Int32);
            parameters.Add("Date", result.Date.ToString("yyyy-MM-dd"), DbType.Date);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task DeleteResult(int id)
        {
            var query = "DELETE FROM Results WHERE Id = @ParamId";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { ParamId = id });
            }
        }
    }
}
