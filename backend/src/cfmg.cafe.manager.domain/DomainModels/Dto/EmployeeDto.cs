namespace Cfmg.Cafe.Manager.Domain.DomainModels.Dto
{
    public class EmployeeDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public int DaysWorked { get; set; }
        public string Cafe { get; set; }
        public EmployeeDto() { }
        public EmployeeDto(string id, string name, string emailAddress, string gender, string phoneNumber, int daysWorked, string cafe)
        {
            Id = id;
            Name = name;
            EmailAddress = emailAddress;
            Gender = gender;
            PhoneNumber = phoneNumber;
            DaysWorked = daysWorked;
            Cafe = cafe;
        }

    }

}