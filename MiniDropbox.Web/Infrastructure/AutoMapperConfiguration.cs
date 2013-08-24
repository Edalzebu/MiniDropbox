using System.Collections.Generic;
using AutoMapper;
using MiniDropbox.Domain;
using MiniDropbox.Domain.Entities;
using MiniDropbox.Web.Models;
using MiniDropbox.Web.Models.Admin;
using MiniDropbox.Web.Models.Premium;
using Ninject.Modules;

namespace MiniDropbox.Web.Infrastructure
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<AccountRegisterModel, Account>();
            Mapper.CreateMap<Account, AccountRegisterModel>();
            Mapper.CreateMap<PremiumPackageModel, PremiumPackage>();
            Mapper.CreateMap<PremiumPackage, PremiumPackageModel>();
            Mapper.CreateMap<BanUserModel, Ban>();
            Mapper.CreateMap<Ban, BanUserModel>();
            Mapper.CreateMap<DiskContentModel, Directories>().ForMember(x => x.FileType, o => o.MapFrom(y => y.Type));
            Mapper.CreateMap<Directories, DiskContentModel>().ForMember(x => x.Type, o => o.MapFrom(y => y.FileType));
            //Mapper.CreateMap<AccountRegisterModel, Account>()
            //  .ForMember(x => x.Email, o => o.MapFrom(y => y.Email));
        }
    }
}