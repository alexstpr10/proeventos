using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.Persistence;
using ProEventos.Domain.Models;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly ProEventosContext _context;

        public EventosController(ProEventosContext context)
        {            
            this._context = context;
        }

        [HttpGet]
        public IEnumerable<Evento> Get()
        {
            return _context.Eventos;
        }
        [HttpGet("{id}")]
        public Evento Get(int id)
        {
            return _context.Eventos.FirstOrDefault(x => x.Id == id);
        }
    }
}
