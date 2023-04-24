using ResultViewer.Server.EndpointDefinitions.Interfaces;
using System.Runtime.CompilerServices;

namespace ResultViewer.Server.EndpointDefinitions
{
    public static class EndpointDefinitionExtensions
    {
        public static void AddEndpointDefinitions(this IServiceCollection services, params Type[] types)
        {
            var endpointDefinitions = new List<IEndpointDefinition>();

            foreach (var type in types)
            {
                endpointDefinitions.AddRange(type.Assembly.ExportedTypes
                    .Where(x => typeof(IEndpointDefinition).IsAssignableFrom(x) && !x.IsInterface)
                    .Select(Activator.CreateInstance).Cast<IEndpointDefinition>());
            }

            foreach (var endpointDefinition in endpointDefinitions)
            {
                endpointDefinition.DefineServices(services);
            }

            services.AddSingleton(endpointDefinitions as IReadOnlyCollection<IEndpointDefinition>);
        }

        public static void UseEndpointDefinitions(this WebApplication app)
        {
            var definitions = app.Services.GetRequiredService<IReadOnlyCollection<IEndpointDefinition>>();

            foreach (var endpointDefinition in definitions)
            {
                endpointDefinition.DefineEndPoints(app);
            }
        }
    }
}
