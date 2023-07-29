using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Models;
using ProEventos.Repository.Models;

namespace ProEventos.Repository.Interfaces
{
    public interface IPalestranteRepository
    {       
        Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false);
        Task<Palestrante> GetPalestranteByIdAsync(int userId, bool includeEventos);
    }
}