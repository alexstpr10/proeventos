using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Models;
using ProEventos.Repository.Interfaces;

namespace ProEventos.Application.Services
{
    public class RedeSocialService : IRedeSocialService
    {
        private readonly IRedeSocialRepository _redeSocialRepository;        
        private readonly IMapper _mapper;

        public RedeSocialService(IRedeSocialRepository redeSocialRepository, IMapper mapper)
        {
            this._redeSocialRepository = redeSocialRepository;
            this._mapper = mapper;
        }

        public async Task<RedeSocialDto[]> SaveByEventoAsync(int eventoId, RedeSocialDto[] models)
        {
            var redesSociais = await _redeSocialRepository.GetAllByEventoIdsAsync(eventoId);
            if (redesSociais == null) return null;

            foreach (var model in models)
            {
                if(model.Id == 0)
                {
                    await AddRedeSocial(model, eventoId, null);
                }
                else
                {
                    var RedeSocial = redesSociais.FirstOrDefault(x => x.Id == model.Id);

                    model.EventoId = eventoId;

                    _mapper.Map(model, RedeSocial);

                    _redeSocialRepository.UpDate<RedeSocial>(RedeSocial);            
                    
                    await _redeSocialRepository.SaveChangesAsync();
                }
                
            }

            var retorno = await _redeSocialRepository.GetAllByEventoIdsAsync(eventoId);
            return _mapper.Map<RedeSocialDto[]>(retorno);
        }

        public async Task<RedeSocialDto[]> SaveByPalestranteAsync(int palestranteId, RedeSocialDto[] models)
        {
            var redesSociais = await _redeSocialRepository.GetAllByPalestranteIdsAsync(palestranteId);
            if (redesSociais == null) return null;

            foreach (var model in models)
            {
                if(model.Id == 0)
                {
                    await AddRedeSocial(model, null, palestranteId);
                }
                else
                {
                    var RedeSocial = redesSociais.FirstOrDefault(x => x.Id == model.Id);

                    model.PalestranteId = palestranteId;

                    _mapper.Map(model, RedeSocial);

                    _redeSocialRepository.UpDate<RedeSocial>(RedeSocial);            
                    
                    await _redeSocialRepository.SaveChangesAsync();
                }
                
            }

            var retorno = await _redeSocialRepository.GetAllByPalestranteIdsAsync(palestranteId);
            return _mapper.Map<RedeSocialDto[]>(retorno);
        }

        private async Task AddRedeSocial(RedeSocialDto model, int? eventoId, int? palestranteId)
        {
            var redeSocial = _mapper.Map<RedeSocial>(model);

            redeSocial.EventoId = eventoId;
            redeSocial.PalestranteId = palestranteId;

            _redeSocialRepository.Add<RedeSocial>(redeSocial);

            await _redeSocialRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteByEventoAsync(int eventoId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialRepository.GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);
                if(redeSocial == null) throw new Exception("Rede Social não encontrado");

                _redeSocialRepository.Delete<RedeSocial>(redeSocial);            
                return await _redeSocialRepository.SaveChangesAsync(); 
            }
            catch (System.Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteByPalestranteAsync(int palestranteId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialRepository.GetRedeSocialPalestranteByIdsAsync(palestranteId, redeSocialId);
                if(redeSocial == null) throw new Exception("Rede Social não encontrado");

                _redeSocialRepository.Delete<RedeSocial>(redeSocial);            
                return await _redeSocialRepository.SaveChangesAsync(); 
            }
            catch (System.Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> GetAllByEventoIdAsync(int eventoId)
        {
            try
            {
                var redesSociais = await _redeSocialRepository.GetAllByEventoIdsAsync(eventoId);

                if(redesSociais == null) return null;

                var resultado = _mapper.Map<RedeSocialDto[]>(redesSociais);

                return resultado;
            }
            catch (System.Exception ex)
            {                
                throw new Exception(ex.Message);
            }

        }

        public async Task<RedeSocialDto[]> GetAllByPalestranteIdAsync(int palestranteId)
        {
            try
            {
                var redesSociais = await _redeSocialRepository.GetAllByPalestranteIdsAsync(palestranteId);

                if(redesSociais == null) return null;

                var resultado = _mapper.Map<RedeSocialDto[]>(redesSociais);

                return resultado;
            }
            catch (System.Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto> GetRedeSocialEventoByIdsAsync(int eventoId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialRepository.GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);

                if(redeSocial == null) return null;

                var resultado = _mapper.Map<RedeSocialDto>(redeSocial);

                return resultado;
            }
            catch (System.Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto> GetRedeSocialPalestranteByIdsAsync(int palestranteId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialRepository.GetRedeSocialPalestranteByIdsAsync(palestranteId, redeSocialId);

                if(redeSocial == null) return null;

                var resultado = _mapper.Map<RedeSocialDto>(redeSocial);

                return resultado;
            }
            catch (System.Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }
        
    }
}