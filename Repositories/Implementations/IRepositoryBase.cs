using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace HomeBanking.Repositories.Implementations
{
    public interface IRepositoryBase<T>
    {
        //Definimos la logica a implementar para el acceso a la base de datos
        //IQueryable retorna como una colección o lista de objetos, entre otros tipos de estructuras de datos
        //Utilizamos un Generic (T) para que se pueda remplazar por la clase necesaria dependiendo los casos de uso
        IQueryable<T> FindAll(); //Trae una lista de todos los objetos de la tabla
        IQueryable<T> FindAll(Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null); //Trae una lista con todos los objetos que cumplan una condición y permite un inclusion posterior de otra colección que se asocie a la misma
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void SaveChanges();
    }
}
