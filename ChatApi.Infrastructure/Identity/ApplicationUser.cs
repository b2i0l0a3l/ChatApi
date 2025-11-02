using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.Entities;
using ChatApi.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ChatApi.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser,IEntity<string>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string ProfileImage { get; set; } = string.Empty;

        public ICollection<Participant > Participants { get; set; } = new List<Participant>();
    }
}