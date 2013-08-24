using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniDropbox.Domain.Entities
{
    public class Directories : IEntity
    {
        private readonly IList<Directories> _subfolder = new List<Directories>();

        public virtual IEnumerable<Directories> SubFolder
        {
            get { return _subfolder; }
        }

        public virtual void AddSubFolder(Directories dir)
        {
            if (!_subfolder.Contains(dir))
            {
                _subfolder.Add(dir);
            }
        }
        public virtual long Id { get; set; }
        public virtual bool IsArchived { get; set; }
        public virtual string Name { get; set; } 
        public virtual string FileType { get; set; }
        public virtual string ModifiedDate { get; set; }    
    }
}
