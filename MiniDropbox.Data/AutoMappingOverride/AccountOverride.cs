using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Conventions.Inspections;
using MiniDropbox.Domain;
using MiniDropbox.Domain.Entities;
using Prefix = FluentNHibernate.Mapping.Prefix;

namespace MiniDropbox.Data.AutoMappingOverride
{
    internal class AccountOverride : IAutoMappingOverride<Account>
    {
        public void Override(AutoMapping<Account> mapping)
        {
            /*mapping.HasMany(x => x.Referrals)
                 .Inverse()
                 .Access.CamelCaseField(Prefix.Underscore);*/
            //mapping.HasMany(x => x.Roles).Inverse().Access.CamelCaseField(Prefix.Underscore);

        }
        
    }

    public class DirectoriesOverride : IAutoMappingOverride<Directories>
    {
        public void Override(AutoMapping<Directories> mapping)
        {
            mapping.HasMany(x => x.SubFolder).Inverse().Access.CamelCaseField(Prefix.Underscore);
        }
    }

}
