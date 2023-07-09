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
    public class EventoService : IEventoService
    {
        private readonly IEventoRepository _eventoRepository;        
        private readonly IMapper _mapper;
        public EventoService(IEventoRepository eventoRepository, IMapper mapper)
        {
            this._eventoRepository = eventoRepository;  
            this._mapper = mapper;          
        }
        public async Task<EventoDto> AddEvento(EventoDto model)
        {
            var evento = _mapper.Map<Evento>(model);

            _eventoRepository.Add<Evento>(evento);            
            var retorno = await _eventoRepository.SaveChangesAsync();
            if(retorno)
            {
                var eventoRetorno = await _eventoRepository.GetEventoByIdAsync(evento.Id, false);
                return _mapper.Map<EventoDto>(eventoRetorno);
            }

            return null;
        }

        public async Task<bool> DeleteEvento(int eventoId)
        {
            var evento = await _eventoRepository.GetEventoByIdAsync(eventoId, false);
            if(evento == null) throw new Exception("Evento não encontrado");

            _eventoRepository.Delete<Evento>(evento);            
            return await _eventoRepository.SaveChangesAsync();            
        }        
        public async Task<EventoDto> UpdateEvento(int idEvento, EventoDto model)
        {

            var evento = await _eventoRepository.GetEventoByIdAsync(idEvento, false);
            if (evento == null) return null;

            model.Id = evento.Id;

             _mapper.Map(model, evento);

            _eventoRepository.UpDate<Evento>(evento);            
            var reponse = await _eventoRepository.SaveChangesAsync();
            if(reponse)
            {
                var eventoRetorno = await _eventoRepository.GetEventoByIdAsync(evento.Id, false);
                return _mapper.Map<EventoDto>(eventoRetorno);
            }
            return null;
        }
        public async Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes)
        {
            var eventos = await _eventoRepository.GetAllEventosAsync(includePalestrantes);

            if(eventos == null) return null;

            var eventosDtos = _mapper.Map<EventoDto[]>(eventos);

            return eventosDtos;
        }
        public async Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes)
        {
            var eventos = await _eventoRepository.GetAllEventosByTemaAsync(tema, includePalestrantes);

            if(eventos == null) return null;

            var eventosDtos = _mapper.Map<EventoDto[]>(eventos);

            return eventosDtos;
        }
        public async Task<EventoDto> GetEventoByIdAsync(int eventoId, bool includePalestrantes)
        {
            var evento = await _eventoRepository.GetEventoByIdAsync(eventoId, includePalestrantes);
            if(evento == null) return null;

            var eventoDto = _mapper.Map<EventoDto>(evento);

            return eventoDto;
        }
    }
}