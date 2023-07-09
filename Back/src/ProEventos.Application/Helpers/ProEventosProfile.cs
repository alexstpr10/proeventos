using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Identity;
using ProEventos.Domain.Models;

namespace ProEventos.Application.Helpers
{
    public class ProEventosProfile : Profile
    {
        public ProEventosProfile()
        {
            CreateMap<Evento, EventoDto>().ReverseMap();        
            //CreateMap<Evento[], EventoDto[]>().ReverseMap();        
            CreateMap<Lote, LoteDto>().ReverseMap();            
            CreateMap<Palestrante, PalestranteDto>().ReverseMap();            
            CreateMap<RedeSocial, RedeSocialDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();
            CreateMap<User, UserLoginDto>().ReverseMap();
        }        
    }
}