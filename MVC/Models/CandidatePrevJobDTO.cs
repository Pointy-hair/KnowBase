namespace MVC.Models
{
    public class CandidatePrevJobDTO
    {
        public int Id { get; set; }

        public string CompanyName { get; set; }

        public ContactsDTO Contact { get; set; }

        public string Position { get; set; }
    }
}