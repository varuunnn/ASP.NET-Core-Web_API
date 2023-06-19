using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_API_demo.Data;
using Web_API_demo.Models;

namespace Web_API_demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        // private dbContext used to talk to the database
        private readonly ContactsAPIDbContext dbContext;
        
        // inject ContactsAPIDbContext into the controller
        public ContactsController(ContactsAPIDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        // get method to get all the contacts
        [HttpGet]
        public async Task<IActionResult> GetContact()
        {
            return Ok( await dbContext.Contacts.ToListAsync());
        }

        // get method to get a specific contact using 'id' as a parameter in route
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);
            if(contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }

        // add a new contact to the databse
        [HttpPost]
        public async Task<IActionResult> AddContact(AddContactsRequest addContactRequest)
        {
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                Name = addContactRequest.Name,
                Email = addContactRequest.Email,
                Phone = addContactRequest.Phone
            };

            await dbContext.Contacts.AddAsync(contact);
            await dbContext.SaveChangesAsync();
            return Ok(contact);
        }

        // update a particular contact in the database
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateContactRequest updateContactRequest)
        {
            var contact = await dbContext.Contacts.FindAsync(id);

            if(contact != null)
            {
                contact.Name = updateContactRequest.Name;   
                contact.Email = updateContactRequest.Email;
                contact.Phone = updateContactRequest.Phone;

                await dbContext.SaveChangesAsync();
                return Ok(contact);
            }
            return NotFound();
        }

        // delete a contact from database
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var contact = dbContext.Contacts.Find(id);
            if(contact != null)
            {
                dbContext.Remove(contact);
                await dbContext.SaveChangesAsync();
                return Ok(contact);
            }
            return NotFound();
        }
    }
}
