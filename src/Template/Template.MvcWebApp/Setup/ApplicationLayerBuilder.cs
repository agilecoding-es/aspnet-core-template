namespace Template.MvcWebApp.Setup
{
    public class ApplicationLayerBuilder
    {
        private readonly IServiceCollection _services;
        private readonly IConfiguration _configuration;

        public ApplicationLayerBuilder(IServiceCollection services, IConfiguration configuration)
        {
            _services = services;
            _configuration = configuration;
        }

        public ApplicationLayerBuilder AddDependencies(Action<IServiceCollection, IConfiguration> builder)
        {
            _ = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.Invoke(_services, _configuration);

            return this;
        }

        public ApplicationLayerBuilder ConfigurePresentation(Action<IServiceCollection, IConfiguration> builder)
        {
            builder.Invoke(_services, _configuration);

            return this;
        }

        public ApplicationLayerBuilder ConfigureApplication(Action<IServiceCollection, IConfiguration> builder)
        {
            builder.Invoke(_services, _configuration);

            return this;
        }

        public ApplicationLayerBuilder ConfigurePersistence(Action<IServiceCollection, IConfiguration> builder)
        {
            builder.Invoke(_services, _configuration);

            return this;
        }

        public ApplicationLayerBuilder ConfigureConnectedServices(Action<IServiceCollection, IConfiguration> builder)
        {
            builder.Invoke(_services, _configuration);

            return this;
        }

        public ApplicationLayerBuilder ConfigureInfrastructure(Action<IServiceCollection, IConfiguration> builder)
        {
            builder.Invoke(_services, _configuration);

            return this;
        }
    }
}
