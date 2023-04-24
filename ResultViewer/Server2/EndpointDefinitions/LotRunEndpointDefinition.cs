using Microsoft.AspNetCore.Mvc;
using ResultViewer.Server.EndpointDefinitions.Interfaces;
using ResultViewer.Server.Repositories;
using ResultViewer.Server.Repositories.Interfaces;

namespace ResultViewer.Server.EndpointDefinitions
{
    public class LotRunEndpointDefinition : IEndpointDefinition
    {
        public void DefineEndPoints(WebApplication app)
        {
            app.MapGet("/lotruns", GetAllLotRuns);
        }

        internal async Task<IResult> GetAllLotRuns(ILotRunRepository repo)
        {
            return Results.Ok(await repo.GetAllLotRuns());
        }

        public void DefineServices(IServiceCollection services)
        {
            services.AddSingleton<ILotRunRepository, LotRunRepository>();
        }
    }
}
