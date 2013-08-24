
namespace MiniDropbox.Domain.Entities
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
