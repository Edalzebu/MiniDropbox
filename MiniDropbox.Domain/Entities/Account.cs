using System.Collections.Generic;

namespace MiniDropbox.Domain
{
    public class Account : IEntity
    {
        public virtual long Id { get; set; }
        public virtual bool IsArchived { get; set; }
        public virtual IList<File> Files { get; set; }
        public virtual IList<string> Referidos { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual bool Admin { get; set; }
        public virtual int Space { get; set; }
        public virtual long SpaceUsed { get; set; }
        public virtual bool Banned { get; set; }
    }
}