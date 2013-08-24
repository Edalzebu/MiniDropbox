namespace MiniDropbox.Domain.Entities
{
    public class Ban : IEntity
    {
        public virtual long Id { get; set; }
        public virtual bool IsArchived { get; set; }
        public virtual string Email { get; set; }
        public virtual string Reason { get; set; }
        public virtual string Administrator { get; set; }
    }
    
}
