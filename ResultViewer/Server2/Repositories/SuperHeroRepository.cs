using AutoMapper;
using Dapper;
using ResultViewer.Server.Context;
using ResultViewer.Server.Dtos;
using ResultViewer.Server.Dtos.ResultDto;
using ResultViewer.Server.Dtos.SuperHeroDto;
using ResultViewer.Server.Models;
using ResultViewer.Server.Repositories.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace ResultViewer.Server.Repositories
{
    public class SuperHeroRepository : ISuperHeroRepository
    {
        private readonly DbContext _context;

        public SuperHeroRepository(DbContext context) => _context = context;

        public async Task<SuperHero> CreateSuperHero(SuperHeroForCreationDto hero)
        {
            var query = "INSERT INTO Results (Name, FirstName, LastName, Place) Values (@Name, @FirstName, @LastName, @Place)" +
                        "SELECT CAST(SCOPE_IDENTITY() AS int)";
            var parameters = new DynamicParameters();

            parameters.Add("Name", hero.Name, DbType.String);
            parameters.Add("FirstName", hero.FirstName, DbType.String);
            parameters.Add("LastName", hero.LastName, DbType.String);
            parameters.Add("Place", hero.Place, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, parameters);
                var createdHero = new SuperHero
                {
                    Id = id,
                    Name= hero.Name,
                    FirstName = hero.FirstName,
                    LastName= hero.LastName,
                    Place = hero.Place,
                };

                return createdHero;
            }
        }

        public async Task<SuperHero> GetSuperHeroById(int heroId)
        {
            var query = "SELECT * FROM SuperHeroes WHERE Id = @ParamId";
            using (var connection = _context.CreateConnection())
            {
                var hero = await connection.QueryFirstAsync<SuperHero>(query, new { ParamId = heroId });
                return hero;
            }
        }

        public async Task<List<SuperHero>> GetAllSuperHeroes()
        {
            var query = "SELECT * FROM SuperHeroes";
            using (var connection = _context.CreateConnection())
            {
                var heroes = await connection.QueryAsync<SuperHero>(query);
                return heroes.ToList();
            }
        }

        public async Task UpdateSuperHero(int id, SuperHeroForUpdateDto hero)
        {
            var query = "UPDATE SuperHeroes SET Name = @Name, FirstName = @FirstName, LastName = @LastName, Place = @Place where Id = @Id";

            var parameters = new DynamicParameters();

            parameters.Add("Id", id, DbType.Int32);
            parameters.Add("Name", hero.Name, DbType.String);
            parameters.Add("FirstName", hero.FirstName, DbType.String);
            parameters.Add("LastName", hero.LastName, DbType.String);
            parameters.Add("Place", hero.Place , DbType.String);


            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task DeleteSuperHero(int id)
        {
            var query = "DELETE FROM SuperHeroes WHERE Id = @ParamId";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { ParamId = id });
            }
        }

        //Querying Multiple Results With Dapper

        public async Task<MultipleQueryResult> GetSuperHeroAndVillainByPlace(string place)
        {
            string sql = @"
            SELECT * FROM SuperHeroes WHERE Place = @Place;
            SELECT * FROM Villains WHERE Place = @Place;";

            using (var connection = _context.CreateConnection())
            {
                var multiQueryResult = await connection.QueryMultipleAsync(sql, new { Place = place });

                var hero = await multiQueryResult.ReadAsync<SuperHero>();
                var villains = await multiQueryResult.ReadFirstOrDefaultAsync<Villain>();

                return new MultipleQueryResult(hero.ToList(), villains);
            }
        }

        //Executing Stored Procedures With Dapper
        public async Task<SuperHero> GetSuperHeroByPlace(string place)
        {
            var parameters = new DynamicParameters();
            parameters.Add("place", place);

            using (var connection = _context.CreateConnection())
            {
                var hero = await connection.QueryFirstOrDefaultAsync<SuperHero>("GetSuperHeroByPlace", parameters, commandType: CommandType.StoredProcedure);

                return hero;
            }
        }
    }
}
