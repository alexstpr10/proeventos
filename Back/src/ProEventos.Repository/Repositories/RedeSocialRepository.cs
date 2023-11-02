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
    public class RedeSocialRepository : GeralRepository, IRedeSocialRepository
    {
        private readonly ProEventosContext _context;

        public RedeSocialRepository(ProEventosContext context) : base(context)
        {
            this._context = context;
        }        

        public async Task<RedeSocial> GetRedeSocialEventoByIdsAsync(int eventoId, int id)
        {
            IQueryable<RedeSocial> query = _context.RedesSocials;

            query = query.AsNoTracking()
            .Where(rs => rs.EventoId == eventoId && rs.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<RedeSocial> GetRedeSocialPalestranteByIdsAsync(int palestranteId, int id)
        {
            IQueryable<RedeSocial> query = _context.RedesSocials;

            query = query.AsNoTracking()
            .Where(rs => rs.PalestranteId == palestranteId && rs.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<RedeSocial[]> GetAllByEventoIdsAsync(int eventoId)
        {
            IQueryable<RedeSocial> query = _context.RedesSocials;

             query = query.AsNoTracking()
            .Where(rs => rs.EventoId == eventoId );

            return await query.ToArrayAsync();
        }

        public async Task<RedeSocial[]> GetAllByPalestranteIdsAsync(int palestranteId)
        {
            IQueryable<RedeSocial> query = _context.RedesSocials;

            query = query.AsNoTracking()
            .Where(p => p.PalestranteId == palestranteId);

            return await query.ToArrayAsync();
        }

    }
}