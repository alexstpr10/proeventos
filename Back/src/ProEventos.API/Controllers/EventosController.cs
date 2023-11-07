using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.API.Extensions;
using ProEventos.API.Helpers;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Models;
using ProEventos.Repository.Context;
using ProEventos.Repository.Models;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class EventosController : ControllerBase
    {
        private readonly IEventoService _eventoService;
        private readonly IUtil _util;
        private readonly string _destino = "Images";

        public EventosController(IEventoService eventoService, IUtil util)
        {            
            this._eventoService = eventoService;
            this._util = util;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]PageParams pageParams)
        {
            try
            {
                var eventos = await _eventoService.GetAllEventosAsync(User.GetUserId(), pageParams, true);
                if (eventos == null) return NoContent();

                Response.AddPagination(eventos.CurrentPage, eventos.PageSize, eventos.TotalCount, eventos.TotalPages);

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
                var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), id, true);
                if (evento == null) return NoContent();

                return Ok(evento);
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
                var eventos = await _eventoService.AddEvento(User.GetUserId(), evento);
                if (eventos == null) return NoContent();

                return Ok(eventos);
            }
            catch (System.Exception ex)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError, 
                $"Erro ao tentar adicionar os eventos: Erro {ex.Message}");
            }            
        }

        [HttpPost("upload-image/{eventoId}")]
        public async Task<IActionResult> UploadImage(int eventoId)
        {
            try
            {
                var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), eventoId, true);
                if (evento == null) return NoContent();

                var file = Request.Form.Files[0];

                if(file.Length > 0)
                {                    
                    _util.DeleteImage(evento.ImagemURL, _destino);
                    evento.ImagemURL = await _util.SaveImage(file, _destino);
                }

                var EventoRetorno = await _eventoService.UpdateEvento(User.GetUserId(), eventoId, evento);                
                return Ok(EventoRetorno);
            }
            catch (System.Exception ex)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError, 
                $"Erro ao tentar adicionar os eventos: Erro {ex.Message}");
            }            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EventoDto evento)
        {
            try
            {
                var eventos = await _eventoService.UpdateEvento(User.GetUserId(), id, evento);
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
                var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), id, true);
                if (evento == null) return NoContent();

                if(await _eventoService.DeleteEvento(User.GetUserId(), id))
                {
                    _util.DeleteImage(evento.ImagemURL, _destino);
                    return Ok(new { message = "Deletado" });
                }
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
