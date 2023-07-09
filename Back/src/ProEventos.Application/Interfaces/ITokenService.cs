using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(UserUpdateDto userUpdateDto);        
    }
}