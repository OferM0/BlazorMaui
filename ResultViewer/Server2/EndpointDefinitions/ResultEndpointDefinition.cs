using Microsoft.AspNetCore.OutputCaching;
using ResultViewer.Server.Dtos;
using ResultViewer.Server.Models;
using ResultViewer.Server.Repositories.Interfaces;
using ResultViewer.Server.Repositories;
using ResultViewer.Server.EndpointDefinitions.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ResultViewer.Server.Dtos.ResultDto;

namespace ResultViewer.Server.EndpointDefinitions
{
    public class ResultEndpointDefinition: IEndpointDefinition
    {
        public void DefineEndPoints(WebApplication app)
        {
            app.MapGet("/results", GetAllResults).CacheOutput(x => x.Tag("results"));
            app.MapGet("/results/{id}", GetResultById);
            app.MapPost("/results", CreateResult);
            app.MapPut("/results/{id}", UpdateResult);
            app.MapDelete("/results/{id}", DeleteResultById);
        }

        internal async Task<IResult> GetAllResults(IResultRepository repo)
        {
            return Results.Ok(await repo.GetAllResults());
        }

        internal async Task<IResult> GetResultById(int id, IResultRepository repo)
        {
            var result = await repo.GetResultById(id);
            if (result is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(result);
        }

        internal async Task<IResult> CreateResult(ResultForCreationDto result, IResultRepository repo, IOutputCacheStore cache, CancellationToken ct)
        {
            var resultResponse = await repo.CreateResult(result);
            await cache.EvictByTagAsync("results", ct);
            return Results.Created($"/results/{resultResponse.Id}", resultResponse);
        }

        internal async Task<IResult> UpdateResult(int id, ResultForUpdateDto updatedResult, IResultRepository repo, IOutputCacheStore cache, CancellationToken ct)
        {
            var result = repo.GetResultById(id);
            if (result is null)
            {
                return Results.NotFound();
            }
            await repo.UpdateResult(id, updatedResult);
            await cache.EvictByTagAsync("results", ct);
            return Results.NoContent();
        }

        internal async Task<IResult> DeleteResultById(int id, IResultRepository repo, IOutputCacheStore cache, CancellationToken ct)
        {
            var example = repo.GetResultById(id);
            if (example is null)
            {
                return Results.NotFound();
            }
            await repo.DeleteResult(id);
            await cache.EvictByTagAsync("results", ct);
            return Results.NoContent();
        }

        public void DefineServices(IServiceCollection services)
        {
            services.AddSingleton<IResultRepository, ResultRepository>();
        }
    }
}
