using Dapper.Fluent.Mapping;
using Dapper.Fluent.ORM.Contracts;
using Dapper.Fluent.Repository.FluentMapper;

namespace Dapper.Fluent.API.Util
{
    public class MapperConfiguration : IMapperConfiguration
    {
        private readonly IRepositorySettings _repository;
        private readonly IRequestInfo _info;

        public MapperConfiguration(IRepositorySettings repository, IRequestInfo info)
        {
            this._repository = repository;
            this._info = info;
        }

        public void ConfigureMappers()
        {
            FluentMapping.AddMap(new LogEntityMap(_info.Schema));
            FluentMapping.AddMap(new PublicSchemaEntityMap(_repository.DefaultSchema));
        }
    }
}
