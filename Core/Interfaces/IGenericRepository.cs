using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T: BaseEntity
    {

        // read methods
        Task<T> GetByIdAsync(int id);

        Task<IReadOnlyList<T>> ListAllAsync();

        Task<T> GetEntityWithSpec(ISpecifications<T> spec);
         
        Task<IReadOnlyList<T>> ListAsync(ISpecifications<T> spec);


        // total num of returned results
        Task<int> CountAsync(ISpecifications<T> spec);

        // add  not async  we dont persis in the database
        void Add(T entity);


        //Update
        void Update(T entity);


        //Delete
        void Delete(T entity);
    }
}