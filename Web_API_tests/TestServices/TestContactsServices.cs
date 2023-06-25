using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_API_demo.Models;
using Web_API_demo.Services;

namespace Web_API_tests.TestServices
{
    public class TestContactsServices: IContactsService
    {
        private readonly List<Contact> _contacts;

        public TestContactsServices()
        {
            _contacts = new List<Contact>()
            {
                new Contact() {
                    Id = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200"),
                    Name = "Test1",
                    Email = "test1@email.com",
                    Phone = 1234567890
                },
                new Contact() {
                    Id = new Guid("33704c4a-5b87-464c-bfb6-51971b4d18ad"),
                    Name = "Test2",
                    Email = "test2@email.com",
                    Phone = 6789012345
                },
                new Contact() {
                    Id = new Guid("815accac-fd5b-478a-a9d6-f171a2f6ae7f"),
                    Name = "Test3",
                    Email = "test3@email.com",
                    Phone = 5432167890
                }
            };
        }

        public Task<List<Contact>> GetAllContacts()
        {
            return Task.FromResult(_contacts);
        }

        public Task<Contact> GetContact(Guid id) 
        {
            return Task.FromResult(_contacts.FirstOrDefault(c => c.Id == id));
        }

        public Task<Contact> AddContact(AddContactsRequest addContactRequest) 
        {
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                Name = addContactRequest.Name,
                Email = addContactRequest.Email,
                Phone = addContactRequest.Phone
            };
            _contacts.Add(contact);
            return Task.FromResult(contact);
        }

        public Task<int> UpdateContact(Guid id, UpdateContactRequest updateContactRequest)
        {
            var contact = _contacts.FirstOrDefault(c => c.Id == id);
            if (contact != null)
            {
                contact.Name = updateContactRequest.Name;
                contact.Email = updateContactRequest.Email;
                contact.Phone = updateContactRequest.Phone;
                return Task.FromResult(1);
            }
            return Task.FromResult(0);
        }

        public Task<int> DeleteContact(Guid id)
        {
            var contact = _contacts.FirstOrDefault(c => c.Id == id);
            if(contact != null)
            {
                _contacts.Remove(contact);
                return Task.FromResult(1);
            }
            return Task.FromResult(0);
        }
    }
}
