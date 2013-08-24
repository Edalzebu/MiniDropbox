using System;
namespace MiniDropbox.Domain.Entities
{
    public class Token : IEntity
    {
        public virtual long Id { get; set; }
        public virtual bool IsArchived { get; set; }
        public virtual string Name { get; set; }
        public virtual long UserId { get; set; }
        public virtual DateTime ExpirationDate { get; set; }
    }
}
