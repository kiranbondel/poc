using System;

namespace Cfmg.Cafe.Manager.Domain.DomainModels.Dto
{
    public class CafeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Logo { get; set; }
        public int NoOfEmployees { get; set; }
        public CafeDto() { }

    }
}
