namespace Cfmg.Cafe.Manager.Application.Models.Dto
{
    public class EmployeeRequestDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Cafe { get; set; } = string.Empty;
    }
}
