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
    public class PalestranteRepository : GeralRepository, IPalestranteRepository
    {
        private readonly ProEventosContext _context;
        public PalestranteRepository(ProEventosContext context) : base(context)
        {
            this._context = context;            
            this._context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;       
        }
        public async Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(p => p.User)
                .Include(e => e.RedesSociais);

            if(includeEventos)
            {
                query = query
                    .Include(e => e.PalestrantesEventos)
                    .ThenInclude(pe => pe.Evento);
            }

            query = query.AsNoTracking()
                         .Where(x => (x.MiniCurriculo.ToLower().Contains(pageParams.Term.ToLower()) ||
                                      x.User.PrimeiroNome.ToLower().Contains(pageParams.Term.ToLower()) ||
                                      x.User.UltimoNome.ToLower().Contains(pageParams.Term.ToLower())) &&
                                      x.User.Funcao == Domain.Enums.Funcao.Palestrante)
                         .OrderBy(e => e.Id);

            return await PageList<Palestrante>.CreateASync(query, pageParams.PageNumber, pageParams.pageSize);
        }
       

        public async Task<Palestrante> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
            .Include(p => p.User)
            .Include(e => e.RedesSociais);

            if(includeEventos)
            {
                query = query.Include(e => e.PalestrantesEventos)
                .ThenInclude(pe => pe.Evento);
            }

            query = query.OrderBy(e => e.Id)
                         .Where(x => x.UserId == userId);

            return await query.FirstOrDefaultAsync();
        }

    }
}