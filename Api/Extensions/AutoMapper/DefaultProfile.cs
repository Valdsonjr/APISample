using Api.v0.Models;
using AutoMapper;
using Domain.Types;

namespace Api.Extensions.AutoMapper
{
    /// <summary>
    /// Perfil padrão do automapper
    /// </summary>
    public class DefaultProfile : Profile
    {
        /// <summary>
        /// Construtor
        /// </summary>
        public DefaultProfile()
        {
            CreateMap<ItemTO, Item>().ReverseMap();
        }
    }
}
