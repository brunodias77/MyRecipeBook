using AutoMapper;
using MRB.Application.Services;
using MRB.CommonTest.IdEncrypter;

namespace MRB.CommonTest.Mapper;

public class MapperBuilder
{
    public static IMapper Build()
    {
        var idEncrypter = IdEncrypterBuilder.Build();

        return new MapperConfiguration(options => { options.AddProfile(new AutoMapping(idEncrypter)); }).CreateMapper();
    }
}