using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Application.Interfaces
{
    public interface IPalestranteService
    {
        Task<Palestrante> AddPalestranteAsync(Palestrante model);
        Task<Palestrante> UpdatePalestranteAsync(Palestrante model);
        Task<bool> DeletePalestranteAsync(int palestranteId);
        Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos);
        Task<Palestrante[]> GetAllPalestrantesAsync(string nome, bool includeEventos);
        Task<Palestrante> GetEventoByIdAsync(int palestranteId, bool includeEventos);
        
    }
}