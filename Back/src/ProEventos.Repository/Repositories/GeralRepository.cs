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
    public class GeralRepository : IGeralRepository
    {
        private readonly ProEventosContext _context;
        public GeralRepository(ProEventosContext context)
        {
            this._context = context;            
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public void DeleteRange<T>(T[] entities) where T : class
        {
            _context.RemoveRange(entities);
        }
        public void UpDate<T>(T entity) where T : class
        {
            _context.Update(entity);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }        

    }
}