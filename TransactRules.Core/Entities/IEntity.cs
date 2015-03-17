using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TransactRules.Core.Entities
{
    public interface IEntity
    {
        [Key]
        [ReadOnly(true)]
        int Id
        {
            get;
            set;
        }

        string CreatedBy
        {
            get;
            set;
        }

        DateTime CreatedOn
        {
            get;
            set;
        }

         DateTime? LastUpdatedOn
        {
            get;
            set;
        }
    }
}
