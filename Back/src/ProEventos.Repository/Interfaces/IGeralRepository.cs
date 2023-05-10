using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Repository.Interfaces
{
    public interface IGeralRepository
    {
        void Add<T>(T entity) where T : class;        
        void Delete<T>(T entity) where T : class;       

        void DeleteRange<T>(T[] entities) where T : class;        
        void UpDate<T>(T entity) where T : class;        
        Task<bool> SaveChangesAsync();                
    }
}