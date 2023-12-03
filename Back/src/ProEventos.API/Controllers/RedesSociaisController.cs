using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RedesSociaisController : ControllerBase
    {
        private readonly IRedeSocialService _redeSocialService;
        private readonly IEventoService _eventoService;
        private readonly IPalestranteService _palestranteService;

        public RedesSociaisController(IRedeSocialService redeSocialService, IEventoService eventoService, IPalestranteService palestranteService)
        {
            _palestranteService = palestranteService;
            _eventoService = eventoService;
            _redeSocialService = redeSocialService;
        }

        [HttpGet("evento/{eventoId}")]
        public async Task<IActionResult> GetByEvento(int eventoId)
        {
            try
            {
                if(!(await AutorEvento(eventoId)))
                    return Unauthorized();

                var redeSocial = await _redeSocialService.GetAllByEventoIdAsync(eventoId);
                if (redeSocial == null) return NoContent();

                return Ok(redeSocial);
            }
            catch (System.Exception ex)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError, 
                $"Erro ao tentar recuperar as redes sociais do evento: Erro {ex.Message}");
            }            
        }

        [HttpGet("palestrante")]
        public async Task<IActionResult> GetBypalestrante()
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
                if(palestrante == null) return Unauthorized();
                
                var redeSocial = await _redeSocialService.GetAllByPalestranteIdAsync(palestrante.Id);
                if (redeSocial == null) return NoContent();

                return Ok(redeSocial);
            }
            catch (System.Exception ex)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError, 
                $"Erro ao tentar recuperar as redes sociais do palestrante: Erro {ex.Message}");
            }            
        }

        [HttpPut("evento/{eventoId}")]
        public async Task<IActionResult> SaveByEvento(int eventoId, RedeSocialDto[] models)
        {
            try
            {
                var redeSocial = await _redeSocialService.SaveByEventoAsync(eventoId, models);
                if (redeSocial == null) return NoContent();

                return Ok(redeSocial);
            }
            catch (System.Exception ex)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError, 
                $"Erro ao tentar salvar rede social por evento: Erro {ex.Message}");
            }            
        }

        [HttpPut("palestrante")]
        public async Task<IActionResult> SaveByPalestrante(RedeSocialDto[] models)
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
                if(palestrante == null) return Unauthorized();

                var redeSocial = await _redeSocialService.SaveByPalestranteAsync(palestrante.Id, models);
                if (redeSocial == null) return NoContent();

                return Ok(redeSocial);
            }
            catch (System.Exception ex)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError, 
                $"Erro ao tentar salvar rede social por palestrante: Erro {ex.Message}");
            }            
        }

        [HttpDelete("evento/{eventoId}/{redeSocialId}")]
        public async Task<IActionResult> Delete(int eventoId, int redeSocialId)
        {
            try
            {
                if(!(await AutorEvento(eventoId)))
                    return Unauthorized();

                var redeSocial = await _redeSocialService.GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);
                if(redeSocial == null) return NoContent();
                
                return await _redeSocialService.DeleteByEventoAsync(eventoId, redeSocialId)
                    ? Ok(new { message = "Rede Social Deletada" })
                    : throw new Exception("Ocorreu um erro não especifico ao tentar deletar a Rede Social por evento");               
                    
            }
            catch (System.Exception ex)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError, 
                $"Erro ao tentar deletar rede social por evento: Erro {ex.Message}");
            }            
        }
        [HttpDelete("palestrante/{redeSocialId}")]
        public async Task<IActionResult> DeleteByPalestrante(int redeSocialId)
        {
            try
            {

                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
                if(palestrante == null) return Unauthorized();

                var redeSocial = await _redeSocialService.GetRedeSocialPalestranteByIdsAsync(palestrante.Id, redeSocialId);
                if(redeSocial == null) return NoContent();
                
                return await _redeSocialService.DeleteByPalestranteAsync(palestrante.Id, redeSocialId)
                    ? Ok(new { message = "Rede Social Deletada" })
                    : throw new Exception("Ocorreu um erro não especifico ao tentar deletar a Rede Social por palestrante");               
                    
            }
            catch (System.Exception ex)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError, 
                $"Erro ao tentar deletar rede social por palestrante: Erro {ex.Message}");
            }            
        }
        [NonAction]
        private async Task<bool> AutorEvento(int eventoId)
        {
            var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), eventoId, false);
            return evento != null;
        }
        
    }
}