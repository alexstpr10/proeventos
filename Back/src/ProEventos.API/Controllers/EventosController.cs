using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Models;
using ProEventos.Repository.Context;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly IEventoService _eventoService;

        public EventosController(IEventoService eventoService)
        {            
            this._eventoService = eventoService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var eventos = await _eventoService.GetAllEventosAsync(true);
                if (eventos == null) return NoContent();

                return Ok(eventos);
            }
            catch (System.Exception ex)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError, 
                $"Erro ao tentar recuperar os eventos: Erro {ex.Message}");
            }            
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var evento = await _eventoService.GetEventoByIdAsync(id, true);
                if (evento == null) return NoContent();

                return Ok(evento);
            }
            catch (System.Exception ex)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError, 
                $"Erro ao tentar recuperar os eventos: Erro {ex.Message}");
            }             
        }

        [HttpGet("{tema}/tema")]
        public async Task<IActionResult> GetByTema(string tema)
        {
            try
            {
                var eventos = await _eventoService.GetAllEventosByTemaAsync(tema, true);
                if (eventos == null) return NoContent();

                return Ok(eventos);
            }
            catch (System.Exception ex)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError, 
                $"Erro ao tentar recuperar os eventos: Erro {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(EventoDto evento)
        {
            try
            {
                var eventos = await _eventoService.AddEvento(evento);
                if (eventos == null) return NoContent();

                return Ok(eventos);
            }
            catch (System.Exception ex)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError, 
                $"Erro ao tentar adicionar os eventos: Erro {ex.Message}");
            }            
        }

        [HttpPut]
        public async Task<IActionResult> Put(int eventoId, EventoDto evento)
        {
            try
            {
                var eventos = await _eventoService.UpdateEvento(eventoId, evento);
                if (eventos == null) return NoContent();

                return Ok(eventos);
            }
            catch (System.Exception ex)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError, 
                $"Erro ao tentar alterar os eventos: Erro {ex.Message}");
            }            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if(await _eventoService.DeleteEvento(id))
                    return Ok("Deletado");
                else 
                    return BadRequest("Erro ao tentar excluir o evento");
            }
            catch (System.Exception ex)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError, 
                $"Erro ao tentar deletar os eventos: Erro {ex.Message}");
            }            
        }
    }
}
