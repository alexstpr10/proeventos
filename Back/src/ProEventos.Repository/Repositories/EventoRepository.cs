using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;
using ProEventos.Repository.Context;
using ProEventos.Repository.Interfaces;
using ProEventos.Repository.Models;

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

        public async Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes= false)
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
                    .Where(x => x.Tema.ToLower().Contains(pageParams.Term.ToLower()) && x.UserId == userId)
                    .OrderBy(e => e.Id);

            return await PageList<Evento>.CreateASync(query, pageParams.PageNumber, pageParams.pageSize);

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