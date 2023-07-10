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
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Models;
using ProEventos.Repository.Context;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class EventosController : ControllerBase
    {
        private readonly IEventoService _eventoService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public EventosController(IEventoService eventoService, IWebHostEnvironment hostEnvironment)
        {            
            this._eventoService = eventoService;
            this._hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var eventos = await _eventoService.GetAllEventosAsync(User.GetUserId(), true);
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

        [HttpGet("{tema}/tema")]
        public async Task<IActionResult> GetByTema(string tema)
        {
            try
            {
                var eventos = await _eventoService.GetAllEventosByTemaAsync(User.GetUserId(), tema, true);
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
                    DeleteImage(evento.ImagemURL);
                    evento.ImagemURL = await SaveImage(file);
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
                    DeleteImage(evento.ImagemURL);                    
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

        [NonAction]
        public void DeleteImage(string imageName)
        {
            if(!string.IsNullOrEmpty(imageName))
            {
                var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources\Images", imageName);
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
            }
        }

        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName)
                                            .Take(10)
                                            .ToArray()
                                         ).Replace(' ', '-');

            imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imageFile.FileName)}";

            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources\Images", imageName);

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return imageName;

        }
    }
}
