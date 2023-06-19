namespace Web_API_demo.Models
{
    // used a model to store a tuple from the database
    public class Contact
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public long Phone { get; set; }
    }
}
