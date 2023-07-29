using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Application.Dtos;


namespace ProEventos.Application.Interfaces
{
    public interface IRedeSocialService
    {
        Task<RedeSocialDto[]> SaveByEventoAsync(int eventoId, RedeSocialDto[] models);
        Task<bool> DeleteByEventoAsync(int eventoId, int redeSocialId);
        Task<RedeSocialDto[]> SaveByPalestranteAsync(int palestranteId, RedeSocialDto[] models);
        Task<bool> DeleteByPalestranteAsync(int palestranteId, int redeSocialId);
        Task<RedeSocialDto[]> GetAllByEventoIdAsync(int eventoId);
        Task<RedeSocialDto[]> GetAllByPalestranteIdAsync(int palestranteId);
        Task<RedeSocialDto> GetRedeSocialEventoByIdsAsync(int eventoId, int redeSocialId);
        Task<RedeSocialDto> GetRedeSocialPalestranteByIdsAsync(int palestranteId, int redeSocialId);

    }
}