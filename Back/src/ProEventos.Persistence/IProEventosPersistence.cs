using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Persistence
{
    public interface IProEventosPersistence
    {
        void Add<T>(T entity) where T: class;
        void UpDate<T>(T entity) where T: class;
        void Delete<T>(T entity) where T: class;
        void DeleteRange<T>(T[] entities) where T: class;
        Task<bool> SaveChangesAsunc();
        Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes);
        Task<Evento[]> GetAllEventosAsync(string tema, bool includePalestrantes);
        Task<Evento> GetEventoByIdAsync(int EventoId, bool includePalestrantes);
    }
}