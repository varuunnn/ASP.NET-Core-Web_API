using Web_API_demo.Models;

namespace Web_API_demo.Services
{
    public interface IContactsService
    {
        Task<List<Contact>> GetAllContacts();
        Task<Contact> GetContact(Guid id);
        Task<Contact> AddContact(AddContactsRequest addContactRequest);
        Task<int> UpdateContact(Guid id, UpdateContactRequest updateContactRequest);
        Task<int> DeleteContact(Guid id);
    }
}
