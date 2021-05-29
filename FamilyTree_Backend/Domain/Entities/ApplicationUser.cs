using FamilyTreeBackend.Core.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyTreeBackend.Core.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string FirstName { get; set; }
        public string MidName { get; set; }
        public string LastName { get; set; }
        public string AvatarUrl { get; set; }
        public string Address { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public byte Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public ICollection<FamilyTree> EditorOfFamilyTrees { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}
