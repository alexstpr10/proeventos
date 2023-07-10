using System.Threading.Tasks;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Interfaces
{
    public interface IEventoService
    {
        Task<EventoDto> AddEvento(int userId, EventoDto model);
        Task<EventoDto> UpdateEvento(int userId, int eventoId, EventoDto model);
        Task<bool> DeleteEvento(int userId, int eventoId);
        Task<EventoDto[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes);
        Task<EventoDto[]> GetAllEventosAsync(int userId, bool includePalestrantes);
        Task<EventoDto> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes);
    }
}