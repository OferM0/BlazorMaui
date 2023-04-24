using Microsoft.AspNetCore.OutputCaching;
using ResultViewer.Server.Dtos;
using ResultViewer.Server.Models;
using ResultViewer.Server.Repositories.Interfaces;
using ResultViewer.Server.Repositories;
using ResultViewer.Server.EndpointDefinitions.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ResultViewer.Server.Dtos.SuperHeroDto;
using ResultViewer.Server.Dtos.ResultDto;
using ResultViewer.Server.Context;

namespace ResultViewer.Server.EndpointDefinitions
{
    public class SuperHeroEndpointDefinition : IEndpointDefinition
    {
        public void DefineEndPoints(WebApplication app)
        {
            app.MapGet("/superheroes", GetAllSuperHeroes).CacheOutput(x => x.Tag("superheroes"));
            app.MapGet("/superheroes/{id}", GetSuperHeroById);
            app.MapPost("/superheroes", CreateSuperHero);
            app.MapPut("/superheroes/{id}", UpdateSuperHero);
            app.MapDelete("/superheroes/{id}", DeleteSuperHeroById);
            app.MapGet("/superheroesandvillains/{place}", GetSuperHeroAndVillainByPlace);
            app.MapGet("/superheroebyplace/{place}", GetSuperHeroByPlace);
        }

        internal async Task<IResult> GetAllSuperHeroes(ISuperHeroRepository repo)
        {
            return Results.Ok(await repo.GetAllSuperHeroes());
        }

        internal async Task<IResult> GetSuperHeroById(int id, ISuperHeroRepository repo)
        {
            var superhero = await repo.GetSuperHeroById(id);
            if (superhero is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(superhero);
        }

        internal async Task<IResult> CreateSuperHero(SuperHeroForCreationDto superhero, ISuperHeroRepository repo, IOutputCacheStore cache, CancellationToken ct)
        {
            var superheroResponse = await repo.CreateSuperHero(superhero);
            await cache.EvictByTagAsync("superheroes", ct);
            return Results.Created($"/superheroes/{superheroResponse.Id}", superheroResponse);
        }

        internal async Task<IResult> UpdateSuperHero(int id, SuperHeroForUpdateDto updatedSuperHero, ISuperHeroRepository repo, IOutputCacheStore cache, CancellationToken ct)
        {
            var superhero = repo.GetSuperHeroById(id);
            if (superhero is null)
            {
                return Results.NotFound();
            }
            await repo.UpdateSuperHero(id, updatedSuperHero);
            await cache.EvictByTagAsync("superheroes", ct);
            return Results.NoContent();
        }

        internal async Task<IResult> DeleteSuperHeroById(int id, ISuperHeroRepository repo, IOutputCacheStore cache, CancellationToken ct)
        {
            var superhero = repo.GetSuperHeroById(id);
            if (superhero is null)
            {
                return Results.NotFound();
            }
            await repo.DeleteSuperHero(id);
            await cache.EvictByTagAsync("superheroes", ct);
            return Results.NoContent();
        }

        internal async Task<IResult> GetSuperHeroAndVillainByPlace(string place, ISuperHeroRepository repo)
        {
            return Results.Ok(await repo.GetSuperHeroAndVillainByPlace(place));
        }

        internal async Task<IResult> GetSuperHeroByPlace(string place, ISuperHeroRepository repo)
        {
            var superhero = await repo.GetSuperHeroByPlace(place);
            if (superhero is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(superhero);
        }

        public void DefineServices(IServiceCollection services)
        {
            services.AddSingleton<ISuperHeroRepository, SuperHeroRepository>();
        }
    }
}
