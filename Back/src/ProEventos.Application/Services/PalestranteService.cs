using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Models;
using ProEventos.Repository.Interfaces;
using ProEventos.Repository.Models;

namespace ProEventos.Application.Services
{
    public class PalestranteService : IPalestranteService
    {
        private readonly IPalestranteRepository _palestranteRepository;
        private readonly IMapper _mapper;

        public PalestranteService(IPalestranteRepository palestranteRepository, IMapper mapper)
        {
            this._palestranteRepository = palestranteRepository;
            this._mapper = mapper;
        }

        public async Task<PalestranteDto> AddPalestranteAsync(int userId, PalestranteAddDto model)
        {
            try
            {
                var palestrante = _mapper.Map<Palestrante>(model);
                palestrante.UserId = userId;

                _palestranteRepository.Add<Palestrante>(palestrante);

                if( await _palestranteRepository.SaveChangesAsync())
                {
                    var palestranteRetorno = await _palestranteRepository.GetPalestranteByUserIdAsync(userId, false);

                    return _mapper.Map<PalestranteDto>(palestranteRetorno);
                }

                return null;

            }
            catch (System.Exception ex)
            {                
                throw new Exception(ex.Message);
            }
            
        }
        public async Task<PalestranteDto> UpdatePalestranteAsync(int userId, PalestranteUpdateDto model)
        {
            try
            {
                var palestrante = await _palestranteRepository.GetPalestranteByUserIdAsync(userId, false);
                if (palestrante == null) return null;

                model.Id = palestrante.Id;
                model.UserId = userId;

                _mapper.Map(model, palestrante);

                _palestranteRepository.UpDate<Palestrante>(palestrante);            
                var reponse = await _palestranteRepository.SaveChangesAsync();
                
                if(reponse)
                {
                    var PalestranteRetorno = await _palestranteRepository.GetPalestranteByUserIdAsync(userId, false);
                    return _mapper.Map<PalestranteDto>(PalestranteRetorno);
                }
                
                return null;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task<PageList<PalestranteDto>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false)
        {

            try
            {
                var palestrantes = await _palestranteRepository.GetAllPalestrantesAsync(pageParams, includeEventos);

                if(palestrantes == null) return null;

                var resultado = _mapper.Map<PageList<PalestranteDto>>(palestrantes);

                resultado.CurrentPage = palestrantes.CurrentPage;
                resultado.PageSize = palestrantes.PageSize;
                resultado.TotalPages = palestrantes.TotalPages;
                resultado.TotalCount = palestrantes.TotalCount;

                return resultado;                
            }
            catch (System.Exception ex)
            {                
                throw new Exception(ex.Message);
            }

        }

        public async Task<PalestranteDto> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false)
        {
            try
            {
                var palestrante = await _palestranteRepository.GetPalestranteByUserIdAsync(userId, includeEventos);
                if(palestrante == null) return null;

                var palestranteDto = _mapper.Map<PalestranteDto>(palestrante);

                return palestranteDto;                
            }
            catch (System.Exception ex)
            {                
                throw new Exception(ex.Message);
            }

        }

    }
}