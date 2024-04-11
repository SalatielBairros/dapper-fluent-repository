using Dapper.Fluent.Domain;

namespace Dapper.Fluent.Repository.Contracts
{
    public interface ICategoryRepository
    {
        void Insert(Category entity);
    }
}