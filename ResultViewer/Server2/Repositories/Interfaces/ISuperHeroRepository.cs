using AutoMapper;
using ResultViewer.Server.Dtos;
using ResultViewer.Server.Dtos.SuperHeroDto;
using ResultViewer.Server.Models;

namespace ResultViewer.Server.Repositories.Interfaces
{
    interface ISuperHeroRepository
    {
        Task<List<SuperHero>> GetAllSuperHeroes();
        Task<SuperHero> GetSuperHeroById(int heroId);
        Task<SuperHero> CreateSuperHero(SuperHeroForCreationDto hero);
        Task UpdateSuperHero(int id, SuperHeroForUpdateDto hero);
        Task DeleteSuperHero(int heroId);
        Task<MultipleQueryResult> GetSuperHeroAndVillainByPlace(string place);
        Task<SuperHero> GetSuperHeroByPlace(string place);
    }
}
