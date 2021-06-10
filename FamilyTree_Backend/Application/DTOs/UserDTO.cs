using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Core.Domain.Enums;
using System;

namespace FamilyTreeBackend.Core.Application.DTOs
{
    public class UserDTO
    {
        public UserDTO(ApplicationUser user)
        {
            if (user == null) return;
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            LoginProvider = user.LoginProvider;
            FirstName = user.FirstName;
            MidName = user.MidName;
            LastName = user.LastName;
            AvatarUrl = user.AvatarUrl;
            Address = user.Address;
            Gender = user.Gender;
            DateOfBirth = user.DateOfBirth;
            CreatedDate = user.CreatedDate;
            UpdatedDate = user.UpdatedDate;
            Phone = user.PhoneNumber;
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string LoginProvider { get; set; }
        public string FirstName { get; set; }
        public string MidName { get; set; }
        public string LastName { get; set; }
        public string AvatarUrl { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
    }
}
