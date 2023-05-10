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
    public class PalestranteRepository : GeralRepository, IPalestranteRepository
    {
        private readonly ProEventosContext _context;
        public PalestranteRepository(ProEventosContext context) : base(context)
        {
            this._context = context;            
            this._context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;       
        }
        public async Task<Palestrante[]> GetAllPalestrantesAsync(string nome, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(e => e.RedesSociais);

            if(includeEventos)
            {
                query = query.Include(e => e.PalestrantesEventos)
                .ThenInclude(pe => pe.Evento);
            }

            query = query.OrderBy(e => e.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(e => e.RedesSociais);

            if(includeEventos)
            {
                query = query.Include(e => e.PalestrantesEventos)
                .ThenInclude(pe => pe.Evento);
            }

            query = query.OrderBy(e => e.Id)
                         .Where(x => x.Nome.ToLower().Contains(nome.ToLower()));


            return await query.ToArrayAsync();
        }

        public async Task<Palestrante> GetPalestranteByIdAsync(int palestranteId, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
            .Include(e => e.RedesSociais);

            if(includeEventos)
            {
                query = query.Include(e => e.PalestrantesEventos)
                .ThenInclude(pe => pe.Evento);
            }

            query = query.OrderBy(e => e.Id)
                         .Where(x => x.Id == palestranteId);

            return await query.FirstOrDefaultAsync();
        }

    }
}