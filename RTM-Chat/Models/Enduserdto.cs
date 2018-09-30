using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RTM_Chat.Models
{
    public class Enduserdto
    {
        public long EndUserId { get; set; }
        public string Name { get; set; }
        public string ProfileImageUrl { get; set; }
        public long OrganisationId { get; set; }
        public string OrganisationEmail { get; set; }
    }
}
