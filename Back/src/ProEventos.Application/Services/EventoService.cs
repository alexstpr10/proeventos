using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Models;
using ProEventos.Repository.Interfaces;

namespace ProEventos.Application.Services
{
    public class EventoService : IEventoService
    {
        private readonly IEventoRepository _eventoRepository;        
        private readonly IGeralRepository _geralRepository;
        public EventoService(IEventoRepository eventoRepository, IGeralRepository geralRepository)
        {
            this._geralRepository = geralRepository;
            this._eventoRepository = eventoRepository;            
        }
        public async Task<Evento> AddEvento(Evento model)
        {
            _geralRepository.Add<Evento>(model);            
            var reponse = await _geralRepository.SaveChangesAsync();
            return model;
        }

        public async Task<bool> DeleteEvento(int eventoId)
        {
            var evento = await _eventoRepository.GetEventoByIdAsync(eventoId, false);
            if(evento == null) throw new Exception("Evento não encontrado");

            _geralRepository.Delete<Evento>(evento);            
            return await _geralRepository.SaveChangesAsync();            
        }        
        public async Task<Evento> UpdateEvento(Evento model)
        {
            _geralRepository.UpDate<Evento>(model);            
            var reponse = await _geralRepository.SaveChangesAsync();
            return model;
        }
        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes)
        {
            return await _eventoRepository.GetAllEventosAsync(includePalestrantes);
        }
        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes)
        {
            return await _eventoRepository.GetAllEventosByTemaAsync(tema, includePalestrantes);
        }
        public async Task<Evento> GetEventoByIdAsync(int eventoId, bool includePalestrantes)
        {
            return await _eventoRepository.GetEventoByIdAsync(eventoId, includePalestrantes);
        }
    }
}