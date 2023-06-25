using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Web_API_demo.Data;
using Web_API_demo.Models;

namespace Web_API_demo.Services
{
    public class ContactsService: IContactsService
    {
        private readonly ContactsAPIDbContext _dbContext;

        public ContactsService(ContactsAPIDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Contact>> GetAllContacts()
        {
            return await _dbContext.Contacts.ToListAsync();
        }

        public async Task<Contact> GetContact(Guid id)
        {
            var res = await _dbContext.Contacts.FindAsync(id);
            return res;
        }

        public async Task<Contact> AddContact(AddContactsRequest addContactRequest)
        {
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                Name = addContactRequest.Name,
                Email = addContactRequest.Email,
                Phone = addContactRequest.Phone
            };
            var res = await _dbContext.Contacts.AddAsync(contact);
            if(res != null)
            {
                await _dbContext.SaveChangesAsync();
                return contact;
            }
            return null;
        }

        public async Task<int> UpdateContact(Guid id, UpdateContactRequest updateContactRequest)
        {
            var contact = await _dbContext.Contacts.FindAsync(id);
            if(contact != null)
            {
                contact.Name = updateContactRequest.Name;
                contact.Email = updateContactRequest.Email;
                contact.Phone = updateContactRequest.Phone;

                await _dbContext.SaveChangesAsync();
                return 1;
            }

            return 0;
        }

        public async Task<int> DeleteContact(Guid id)
        {
            var contact = await _dbContext.Contacts.FindAsync(id);
            if (contact != null)
            {
                _dbContext.Remove(contact);
                await _dbContext.SaveChangesAsync();
                return 1;
            }
            return 0;
        }
    }
}
