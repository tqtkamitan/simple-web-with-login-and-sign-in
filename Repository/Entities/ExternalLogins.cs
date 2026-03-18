using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class ExternalLogins : BaseEntity
    {
        public int UserId { get; set; }
        public string? Provider { get; set; }      // "Google"
        public string? ProviderId { get; set; }    // Google unique ID

        public User User { get; set; }
    }
}
