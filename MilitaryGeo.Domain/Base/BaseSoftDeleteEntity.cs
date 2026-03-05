using MilitaryGeo.Domain.Base.EntityAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryGeo.Domain.Base
{
    public abstract class BaseSoftDeleteEntity<TKey> : EntityBase<TKey>, ISoftDelete
    {
        public bool IsDeleted { get; set; } = false;
        public DateTimeOffset? DeletedAt { get; set; }
        public Guid? DeleteBy { get; set; }
    }
}
