namespace Web_API_demo.Models
{
    // used as a model to update a contact in the database
    public class UpdateContactRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public long Phone { get; set; }
    }
}
