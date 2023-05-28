using System.Threading.Tasks;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Interfaces
{
    public interface IEventoService
    {
        Task<EventoDto> AddEvento(EventoDto model);
        Task<EventoDto> UpdateEvento(int eventoId, EventoDto model);
        Task<bool> DeleteEvento(int eventoId);
        Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes);
        Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes);
        Task<EventoDto> GetEventoByIdAsync(int eventoId, bool includePalestrantes);
    }
}