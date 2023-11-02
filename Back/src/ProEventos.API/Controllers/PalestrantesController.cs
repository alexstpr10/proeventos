using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Repository.Models;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PalestrantesController : ControllerBase
    {
        private readonly IPalestranteService _palestranteService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IAccountService _accountService;

        public PalestrantesController(IPalestranteService palestranteService, IWebHostEnvironment hostEnvironment, IAccountService accountService)
        {
            _palestranteService = palestranteService;            
            _hostEnvironment = hostEnvironment;
            _accountService = accountService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery]PageParams pageParams)
        {
            try
            {
                var palestrantes = await _palestranteService.GetAllPalestrantesAsync( pageParams, true);
                if (palestrantes == null) return NoContent();

                Response.AddPagination(palestrantes.CurrentPage, palestrantes.PageSize, palestrantes.TotalCount, palestrantes.TotalPages);

                return Ok(palestrantes);
            }
            catch (System.Exception ex)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError, 
                $"Erro ao tentar recuperar os palestrantes: Erro {ex.Message}");
            }            
        }

        [HttpGet]
        public async Task<IActionResult> GetPalestrantes()
        {
            try
            {
                var palestrantre = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId(), true);
                if (palestrantre == null) return NoContent();

                return Ok(palestrantre);
            }
            catch (System.Exception ex)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError, 
                $"Erro ao tentar recuperar os palestrantres: Erro {ex.Message}");
            }             
        }    

        [HttpPost]
        public async Task<IActionResult> Post(PalestranteAddDto model)
        {
            try
            {               

                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId(), false);

                if (palestrante == null)
                    palestrante = await _palestranteService.AddPalestranteAsync(User.GetUserId(), model);
                
                if (palestrante == null) return NoContent();

                return Ok(palestrante);
            }
            catch (System.Exception ex)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError, 
                $"Erro ao tentar adicionar os palestrantes: Erro {ex.Message}");
            }            
        }

        [HttpPut]
        public async Task<IActionResult> Put(PalestranteUpdateDto palestrante)
        {
            try
            {
                var palestrantes = await _palestranteService.UpdatePalestranteAsync(User.GetUserId(), palestrante);
                if (palestrantes == null) return NoContent();

                return Ok(palestrantes);
            }
            catch (System.Exception ex)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError, 
                $"Erro ao tentar adicionar os palestrantes: Erro {ex.Message}");
            }            
        }     
    }
}