using System.Threading.Tasks;
using ProEventos.Application.Dtos;
using ProEventos.Repository.Models;

namespace ProEventos.Application.Interfaces
{
    public interface IEventoService
    {
        Task<EventoDto> AddEvento(int userId, EventoDto model);
        Task<EventoDto> UpdateEvento(int userId, int eventoId, EventoDto model);
        Task<bool> DeleteEvento(int userId, int eventoId);
        Task<PageList<EventoDto>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes);
        Task<EventoDto> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes);
    }
}