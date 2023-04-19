using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ResultViewer.Server.Dtos;
using ResultViewer.Server.EndpointDefinitions.Interfaces;
using ResultViewer.Server.Models;
using ResultViewer.Server.Repositories;
using ResultViewer.Server.Repositories.Interfaces;
using System.Collections.Generic;

namespace ResultViewer.Server.EndpointDefinitions
{
    public class ExampleEndpointDefinition : IEndpointDefinition
    {
        public void DefineEndPoints(WebApplication app)
        {
            app.MapGet("examples", GetAllExamples).CacheOutput(x => x.Tag("examples"));
            app.MapGet("/examples/{id}", GetExampleById);
            app.MapPost("/examples", CreateExample);
            app.MapPut("/examples/{id}", UpdateExample);
            app.MapDelete("/examples/{id}", DeleteExampleById);
        }
        
        internal IResult GetAllExamples(IExampleRepository repo)
        {
            var examples = repo.mapper.Map<List<Example>, List<ExampleToReturnDto>>(repo.GetAll());
            return examples is not null ? Results.Ok(examples) : Results.Ok(new List<ExampleToReturnDto>());
        }

        internal IResult GetExampleById(Guid id, IExampleRepository repo)
        {
            var example = repo.GetById(id);
            if (example is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(repo.mapper.Map<ExampleToReturnDto>(example));
        }

        internal IResult CreateExample(Example example, IExampleRepository repo, IOutputCacheStore cache, CancellationToken ct)
        {
            repo.Create(example);
            cache.EvictByTagAsync("examples", ct);
            return Results.Created($"/examples/{example.Id}", example);
        }

        internal IResult UpdateExample(Guid id, Example updatedExample, IExampleRepository repo, IOutputCacheStore cache, CancellationToken ct)
        {
            var example = repo.GetById(id);
            if(example is null)
            {
                return Results.NotFound();
            }
            repo.Update(updatedExample);
            cache.EvictByTagAsync("examples", ct);
            return Results.Ok(repo.mapper.Map<ExampleToReturnDto>(updatedExample));
        }

        internal IResult DeleteExampleById(Guid id, IExampleRepository repo, IOutputCacheStore cache, CancellationToken ct)
        {
            var example = repo.GetById(id);
            if (example is null)
            {
                return Results.NotFound();
            }
            repo.Delete(id);
            cache.EvictByTagAsync("examples", ct);
            return Results.NoContent();
        }

        public void DefineServices(IServiceCollection services)
        {
            services.AddSingleton<IExampleRepository, ExampleRepository>();
        }
    }
}
