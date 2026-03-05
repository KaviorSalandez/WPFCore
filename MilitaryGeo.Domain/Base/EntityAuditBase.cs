using MilitaryGeo.Domain.Base.EntityAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryGeo.Domain.Base
{
    public abstract  class EntityAuditBase<TKey> : EntityBase<TKey>, IEntityAuditBase<TKey>
    {
        public Guid? CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
        public Guid? ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
