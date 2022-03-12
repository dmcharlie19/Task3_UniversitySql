using System.Collections.Generic;

namespace UniversitySql.Repositories
{
    public interface ICommonOperationsSqlRepository<T> where T : new()
    {
        void Add( T obj );
        void DeleteById( int id );
        List<T> GetAll();
        T GetById( int id );
    }
}