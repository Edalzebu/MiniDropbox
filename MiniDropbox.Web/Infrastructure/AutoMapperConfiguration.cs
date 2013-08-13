using System.Collections.Generic;
using AutoMapper;
using MiniDropbox.Domain;
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
        }
    }
}