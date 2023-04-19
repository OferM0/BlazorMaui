namespace ResultViewer.Server.EndpointDefinitions.Interfaces
{
    public interface IEndpointDefinition
    {
        void DefineEndPoints(WebApplication app);

        void DefineServices(IServiceCollection services);
    }
}
