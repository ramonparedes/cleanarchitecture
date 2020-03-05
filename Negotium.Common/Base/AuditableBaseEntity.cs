using System;

namespace Negotium.Common
{
    public class AuditableBaseEntity
    {
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
    }
}
