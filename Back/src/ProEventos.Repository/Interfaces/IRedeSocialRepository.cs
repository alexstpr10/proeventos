using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Models;


namespace ProEventos.Repository.Interfaces
{
    public interface IRedeSocialRepository: IGeralRepository
    {
        Task<RedeSocial> GetRedeSocialEventoByIdsAsync(int eventoId, int id);
        Task<RedeSocial> GetRedeSocialPalestranteByIdsAsync(int palestranteId, int id);
        Task<RedeSocial[]> GetAllByEventoIdsAsync(int eventoId);
        Task<RedeSocial[]> GetAllByPalestranteIdsAsync(int palestranteId);        
    }
}