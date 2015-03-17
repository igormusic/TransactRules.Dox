using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Dynamic;

namespace TransactRules.Core.Entities
{
    public abstract class Entity:IEntity
    {
        public Entity()
        {
            CreatedOn = DateTime.Now;
            CreatedBy = System.Threading.Thread.CurrentPrincipal.Identity.Name;
        }

        [Key]
        [ReadOnly(true)]
        public virtual int Id
        {
            get;
            set;
        }

        public virtual string CreatedBy
        {
            get;
            set;
        }

        public virtual DateTime CreatedOn
        {
            get;
            set;
        }

        public virtual DateTime? LastUpdatedOn
        {
            get;
            set;
        }

        public virtual int? Version { get; set; }

       
    }
}
