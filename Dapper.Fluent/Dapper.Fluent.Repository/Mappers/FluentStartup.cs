using Dapper.Fluent.Mapping;
using Dapper.Fluent.ORM.Contracts;
using Dapper.Fluent.Repository.FluentMapper;

namespace Dapper.Fluent.Repository.Mappers
{
    public class MapperConfiguration : IMapperConfiguration
    {
        private readonly IRepositorySettings _repository;

        public MapperConfiguration(IRepositorySettings repository)
        {
            this._repository = repository;
        }

        public void ConfigureMappers()
        {
            FluentMapping.AddMap(new CategoryMap(_repository.DefaultSchema));
            FluentMapping.AddMap(new PublicSchemaEntityMap(_repository.DefaultSchema));
            FluentMapping.AddMap(new LogEntityMap());
        }

        public void SetDynamicSchema(string schema) => FluentMapping.SetDynamicSchema(schema);
    }
}
