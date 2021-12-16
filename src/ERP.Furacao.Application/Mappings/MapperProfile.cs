using AutoMapper;
using ERP.Furacao.Application.DTOs.Conta;
using ERP.Furacao.Application.DTOs.Log;
using ERP.Furacao.Domain.Models;

namespace ERP.Furacao.Application.Mappings
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<EmpresaModel, DadoDaEmpresaResponse>();
            CreateMap<EmpresaModel, DadoDaContaResponse>();
            CreateMap<LogModel, logDadosResponse>();

        }
    }
}
