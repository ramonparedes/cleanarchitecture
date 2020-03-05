using System;
using System.ComponentModel.DataAnnotations.Schema;
using Negotium.Common;

namespace Negotium.Data
{
    public class User : AuditableBaseEntity
    {
       public long Id { get; set; }
       public string UserName { get; set; }
       public string Email { get; set; }
       public string Password { get; set; }
    }
}
