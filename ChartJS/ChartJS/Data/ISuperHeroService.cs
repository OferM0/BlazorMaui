namespace ChartJS.Data;

public interface ISuperHeroService
{
    Task<List<SuperHero>> GetAllSuperHeroes();
    Task<SuperHero> GetSuperHero(int heroId);
    Task<SuperHero> AddSuperHero(SuperHero hero);
    Task<SuperHero> UpdateSuperHero(SuperHero hero);
    Task DeleteSuperHero(int heroId);
}


