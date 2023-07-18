using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Models;
using ProEventos.Repository.Models;

namespace ProEventos.Repository.Interfaces
{
    public interface IEventoRepository: IGeralRepository
    {        
        Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes);
        Task<Evento> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes);     
    }
}