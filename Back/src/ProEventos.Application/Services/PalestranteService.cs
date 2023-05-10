using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Models;
using ProEventos.Repository.Interfaces;

namespace ProEventos.Application.Services
{
    public class PalestranteService : IPalestranteService
    {
        private readonly IPalestranteRepository _palestranteRepository;
        private readonly IGeralRepository _geralRepository;
        public PalestranteService(IPalestranteRepository palestranteRepository, IGeralRepository geralRepository)
        {
            this._geralRepository = geralRepository;
            this._palestranteRepository = palestranteRepository;            
        }

        public async Task<Palestrante> AddPalestranteAsync(Palestrante model)
        {
            _geralRepository.Add<Palestrante>(model);
            if(await _geralRepository.SaveChangesAsync())
            {
                return model;
            }
            return null;
        }
        public async Task<Palestrante> UpdatePalestranteAsync(Palestrante model)
        {
            _geralRepository.UpDate<Palestrante>(model);
            if(await _geralRepository.SaveChangesAsync())
            {
                return model;
            }
            return null;
        }

        public async Task<bool> DeletePalestranteAsync(int palestranteId)
        {
            var palestrante = await _palestranteRepository.GetPalestranteByIdAsync(palestranteId, false);

            if(palestrante == null) throw new Exception("Palestrante não encontrado");

            _geralRepository.Delete<Palestrante>(palestrante);

            return await _geralRepository.SaveChangesAsync();
        }

        public async Task<Palestrante[]> GetAllPalestrantesAsync(string nome, bool includeEventos)        
            => await _palestranteRepository.GetAllPalestrantesAsync(nome, includeEventos);        

        public async Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos) 
            => await _palestranteRepository.GetAllPalestrantesByNomeAsync(nome, includeEventos);        

        public async Task<Palestrante> GetEventoByIdAsync(int palestranteId, bool includeEventos)
            => await _palestranteRepository.GetPalestranteByIdAsync(palestranteId, includeEventos);

    }
}