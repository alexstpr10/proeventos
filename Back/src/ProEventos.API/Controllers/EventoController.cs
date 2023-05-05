using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.API.Models;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventoController : ControllerBase
    {
        private IEnumerable<Evento> _eventos = new Evento[]{
            new Evento(){
                EventoId = 1,
                Tema = "Angular",
                Local = "SP",
                Lote = "1 lote",
                QtdPessoas = 250,
                DataEvento = DateTime.Now.AddDays(2).ToString(),
                ImagemURL = "teste.png"
            }, 
            new Evento(){
                EventoId = 2,
                Tema = "Angular Teste",
                Local = "SP",
                Lote = "2 lote",
                QtdPessoas = 250,
                DataEvento = DateTime.Now.AddDays(2).ToString(),
                ImagemURL = "teste.png"
            }, 
        };



        public EventoController()
        {            
        }

        [HttpGet]
        public IEnumerable<Evento> Get()
        {
            return _eventos;
        }
        [HttpGet("{id}")]
        public Evento Get(int id)
        {
            return _eventos.Where(x => x.EventoId == id).FirstOrDefault();
        }
    }
}
