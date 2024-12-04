using AutoMapper;
using TinkoffInvest.Core.Models;
using TinkoffInvest.Data.Entities;

namespace TinkoffInvest.Data.MapperProfiles;

public class DataMapperProfile :Profile
{
    public DataMapperProfile()
    {
        CreateMap<SecurityEntity, Security>();
    }
}