namespace Template.MvcWebApp.Setup
{
    public class ApplicationLayerBuilder
    {
        private readonly IServiceCollection _services;
        private readonly ConfigurationManager _configuration;

        public ApplicationLayerBuilder(IServiceCollection services, ConfigurationManager configuration)
        {
            _services = services;
            _configuration = configuration;
        }

        public ApplicationLayerBuilder AddDependencies(Action<IServiceCollection, ConfigurationManager> builder)
        {
            _ = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.Invoke(_services, _configuration);

            return this;
        }

        public ApplicationLayerBuilder ConfigurePresentation(Action<IServiceCollection, ConfigurationManager> builder)
        {
            builder.Invoke(_services, _configuration);

            return this;
        }

        public ApplicationLayerBuilder ConfigureApplication(Action<IServiceCollection, ConfigurationManager> builder)
        {
            builder.Invoke(_services, _configuration);

            return this;
        }

        public ApplicationLayerBuilder ConfigurePersistence(Action<IServiceCollection, ConfigurationManager> builder)
        {
            builder.Invoke(_services, _configuration);

            return this;
        }

        public ApplicationLayerBuilder ConfigureConnectedServices(Action<IServiceCollection, ConfigurationManager> builder)
        {
            builder.Invoke(_services, _configuration);

            return this;
        }

        public ApplicationLayerBuilder ConfigureInfrastructure(Action<IServiceCollection, ConfigurationManager> builder)
        {
            builder.Invoke(_services, _configuration);

            return this;
        }
    }
}
