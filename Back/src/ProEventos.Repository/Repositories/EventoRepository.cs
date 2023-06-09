using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;
using ProEventos.Repository.Context;
using ProEventos.Repository.Interfaces;

namespace ProEventos.Repository.Repositories
{
    public class EventoRepository : GeralRepository, IEventoRepository
    {
        private readonly ProEventosContext _context;
        public EventoRepository(ProEventosContext context) : base(context)
        {
            this._context = context;     
            this._context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;       
        }        

        public async Task<Evento[]> GetAllEventosAsync(int userId, bool includePalestrantes= false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(e => e.Lotes)
                .Include(e => e.RedesSociais)
                .ThenInclude(pe => pe.Palestrante);

            if(includePalestrantes)
            {
                query = query.Include(e => e.PalestrantesEventos)
                .ThenInclude(pe => pe.Palestrante);
            }

            query = query
                    .Where(e => e.UserId == userId)
                    .OrderBy(e => e.Id);

            return await query.ToArrayAsync();

        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(e => e.Lotes)
                .Include(e => e.RedesSociais);

            if(includePalestrantes)
            {
                query = query.Include(e => e.PalestrantesEventos)
                .ThenInclude(pe => pe.Palestrante);
            }

            query = query.OrderBy(e => e.Id)
                         .Where(x => x.Tema.ToLower().Contains(tema.ToLower()) && x.UserId == userId);

            return await query.ToArrayAsync();
        }

        public async Task<Evento> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(e => e.Lotes)
                .Include(e => e.RedesSociais);

            if(includePalestrantes)
            {
                query = query.Include(e => e.PalestrantesEventos)
                .ThenInclude(pe => pe.Palestrante);
            }

            query = query.OrderBy(e => e.Id)
                         .Where(x => x.Id == eventoId && x.UserId == userId);

            return await query.FirstOrDefaultAsync();
        }        

    }
}