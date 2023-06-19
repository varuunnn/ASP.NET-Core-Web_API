namespace Web_API_demo.Models
{
    // used as a model to add a new contact in the database
    public class AddContactsRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public long Phone { get; set; }
    }
}
