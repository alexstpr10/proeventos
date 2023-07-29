using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Models;
using ProEventos.Repository.Models;

namespace ProEventos.Application.Interfaces
{
    public interface IPalestranteService
    {
        Task<PalestranteDto> AddPalestranteAsync(int userId, PalestranteAddDto model);
        Task<PalestranteDto> UpdatePalestranteAsync(int userId, PalestranteUpdateDto model);
        Task<PageList<PalestranteDto>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false);
        Task<PalestranteDto> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false);        
    }
}