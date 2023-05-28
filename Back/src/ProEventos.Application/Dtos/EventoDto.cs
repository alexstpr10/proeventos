using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProEventos.Application.Dtos
{
    public class EventoDto
    {
        public int Id { get; set; }
        public string Local { get; set; }
        public string DataEvento { get; set; }
        [Required(ErrorMessage = "o campo {0} é obrigatório"),
            StringLength(50, MinimumLength = 3, ErrorMessage = "O campo {0} deve conter no minimo 4 caracteres e no máximo 50.")]
        public string Tema { get; set; }

        [Range(1, 120000, ErrorMessage = "{0} deve estar dentro do intervalo de 1 até 120.000"),Display(Name ="Qtd Pessoas")]
        public int QtdPessoas { get; set; }

        [RegularExpression(@".*\.(gif|jpe?g|bmp|png)$", ErrorMessage = "Não é uma imagem vãlida. (gif, jpg, jpeg, bmp ou png")]
        public string ImagemURL { get; set; }

        [Required, Phone(ErrorMessage = "O campo {0} está com o número inválido")]
        public string Telefone { get; set; }

        [ Required(ErrorMessage = "o campo {0} é obrigatório."),
          Display(Name = "e-mail"),
          EmailAddress(ErrorMessage = "o campo {0} deve ser um e-mail válido")]
        public string Email { get; set; }
        public IEnumerable<LoteDto> Lotes { get; set; }
        public IEnumerable<RedeSocialDto> RedesSociais { get; set; }
        public IEnumerable<PalestranteDto> Palestrantes { get; set; }
        
    }
}