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
    public class LoteService : ILoteService
    {
        private readonly ILoteRepository _loteRepository;        
        private readonly IMapper _mapper;

        public LoteService(ILoteRepository loteRepository, IMapper mapper)
        {
            this._loteRepository = loteRepository;
            this._mapper = mapper;
        }
        public async Task<bool> DeleteLote(int eventoId, int loteId)
        {
            var lote = await _loteRepository.GetLoteByIdsAsync(eventoId, loteId);
            if(lote == null) throw new Exception("Lote não encontrado");

            _loteRepository.Delete<Lote>(lote);            
            return await _loteRepository.SaveChangesAsync();   
        }

        public async Task<LoteDto> GetLoteByIdsAsync(int eventoId, int loteId)
        {
            var lote = await _loteRepository.GetLoteByIdsAsync(eventoId, loteId);

            if(lote == null) return null;

            var resultado = _mapper.Map<LoteDto>(lote);

            return resultado;
        }

        public async Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId)
        {
            var lotes = await _loteRepository.GetLotesByEventoIdAsync(eventoId);

            if(lotes == null) return null;

            var resultado = _mapper.Map<LoteDto[]>(lotes);

            return resultado;
        }

        private async Task AddLote(int eventoId, LoteDto model)
        {
            var lote = _mapper.Map<Lote>(model);

            lote.EventoId = eventoId;

            _loteRepository.Add<Lote>(lote);

            await _loteRepository.SaveChangesAsync();
        }

        public async Task<LoteDto[]> SaveLotes(int eventoId, LoteDto[] models)
        {
            var lotes = await _loteRepository.GetLotesByEventoIdAsync(eventoId);
            if (lotes == null) return null;

            foreach (var model in models)
            {
                if(model.Id == 0)
                {
                    await AddLote(eventoId, model);
                }
                else
                {
                    var lote = lotes.FirstOrDefault(x => x.Id == model.Id);

                    model.EventoId = eventoId;

                    _mapper.Map(model, lote);

                    _loteRepository.UpDate<Lote>(lote);            
                    
                    await _loteRepository.SaveChangesAsync();
                }
                
            }

            var lotesRetorno = await _loteRepository.GetLotesByEventoIdAsync(eventoId);
            return _mapper.Map<LoteDto[]>(lotesRetorno);
        }
    }
}