using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Cfmg.Cafe.Manager.Common.Library.SeedWork;

namespace Cfmg.Cafe.Manager.Domain.DomainModels
{
    [Index(nameof(Id), nameof(IsActive))]
    public class CafeEntity : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        public string? Logo { get; set; }

        [Required]
        [MaxLength(255)]
        public string Location { get; set; }

        public ICollection<EmployeeEntity> Employees { get; set; } = new List<EmployeeEntity>();

        public CafeEntity() { }
        public CafeEntity(Guid id, string name, string description, string? logo, string location)
        {
            Id = id;
            Name = name;
            Description = description;
            Logo = logo;
            Location = location;
        }        
    }
}
