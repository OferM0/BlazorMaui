using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data.SqlClient;

namespace ChartJS.Data;

    public class SuperHeroService : ISuperHeroService
{
    private readonly IConfiguration _config;

    public SuperHeroService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<List<SuperHero>> GetAllSuperHeroes()
    {
        using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        return (await connection.QueryAsync<SuperHero>("select * from superheroes")).ToList();
    }

    public async Task<SuperHero> GetSuperHero(int heroId)
    {
        using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        return await connection.QueryFirstOrDefaultAsync<SuperHero>("select * from superheroes where id = @Id",
                new { Id = heroId });
    }

    public async Task<SuperHero> AddSuperHero(SuperHero hero)
    {
        using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await connection.ExecuteAsync("insert into superheroes (name, firstname, lastname, place) values (@Name, @FirstName, @LastName, @Place)", hero);
        return await GetSuperHero(hero.Id);
    }

    public async Task<SuperHero> UpdateSuperHero(SuperHero hero)
    {
        using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await connection.ExecuteAsync("update superheroes set name = @Name, firstname = @FirstName, lastname = @LastName, place = @Place where id = @Id", hero);
        return await GetSuperHero(hero.Id);
    }

    public async Task DeleteSuperHero(int heroId)
    {
        using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await connection.ExecuteAsync("delete from superheroes where id = @Id", new { Id = heroId });
    }
}

