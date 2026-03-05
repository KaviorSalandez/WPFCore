using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryGeo.Domain.Base.EntityAbstraction
{
    public interface IEntityAuditBase<TKey> : IEntityBase<TKey>, IAuditable
    {
    }
}
