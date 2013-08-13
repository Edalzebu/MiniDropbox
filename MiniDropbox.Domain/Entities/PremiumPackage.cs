using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MiniDropbox.Domain
{
    public class PremiumPackage : IEntity
    {
        public virtual long Id { get; set; }
        public virtual bool IsArchived { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual int Price { get; set; }
        public virtual int Space { get; set; }
        public virtual int Days { get; set; }
        public virtual bool Available { get; set; }
    }
}
