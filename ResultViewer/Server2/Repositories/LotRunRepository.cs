using Dapper;
using ResultViewer.Server.Context;
using ResultViewer.Server.Models;
using ResultViewer.Server.Repositories.Interfaces;

namespace ResultViewer.Server.Repositories
{
    class LotRunRepository : ILotRunRepository
    {
        private readonly DbContext _context;

        public LotRunRepository(DbContext context) => _context = context;

        public async Task<List<LotRun>> GetAllLotRuns()
        {
            var query = "SELECT * FROM dbo.LOT_RUN";
            using (var connection = _context.CreateConnection())
            {
                var results = await connection.QueryAsync<LotRun>(query);
                return results.ToList();
            }
        }
    }
}
