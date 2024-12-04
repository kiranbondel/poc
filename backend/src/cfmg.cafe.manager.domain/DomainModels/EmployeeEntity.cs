using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Cfmg.Cafe.Manager.Common.Library.SeedWork;

namespace Cfmg.Cafe.Manager.Domain.DomainModels
{
    [Index(nameof(Id), nameof(IsActive))]
    public class EmployeeEntity : BaseEntity
    {
        [Key]
        [MaxLength(9)]
        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string EmailAddress { get; set; }

        [MaxLength(16)]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(6)]
        public string Gender { get; set; }

        public DateTime? StartDate { get; set; }
        public Guid? CafeId { get; set; }
        public CafeEntity Cafe { get; set; }

        public EmployeeEntity() { }

        public EmployeeEntity(string id, string name, string emailAddress, string phoneNumber, string gender, DateTime? startDate, Guid? cafeId)
        {
            Id = id;
            Name = name;
            EmailAddress = emailAddress;
            PhoneNumber = phoneNumber;
            Gender = gender;
            StartDate = startDate;
            CafeId = cafeId;
        }
    }
}
