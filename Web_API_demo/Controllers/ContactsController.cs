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

        private ILogger<ContactsController> logger;
        
        // inject ContactsAPIDbContext into the controller
        public ContactsController(ContactsAPIDbContext dbContext, ILogger<ContactsController> logger) 
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        // get method to get all the contacts
        [HttpGet]
        public async Task<IActionResult> GetContact()
        {
            logger.LogInformation("Executing {Action}", nameof(GetContact));
            return Ok( await dbContext.Contacts.ToListAsync());
        }

        // get method to get a specific contact using 'id' as a parameter in route
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
            logger.LogInformation("Executing {Action} with parameters {id}", nameof(GetContact), id);
            var contact = await dbContext.Contacts.FindAsync(id);
            if(contact == null)
            {
                logger.LogError("Contact not found");
                return NotFound();
            }

            logger.LogInformation("Contact found: {name}", contact.Name);
            return Ok(contact);
        }

        // add a new contact to the databse
        [HttpPost]
        public async Task<IActionResult> AddContact(AddContactsRequest addContactRequest)
        {
            logger.LogInformation("Executing {Action}", nameof(AddContact));
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                Name = addContactRequest.Name,
                Email = addContactRequest.Email,
                Phone = addContactRequest.Phone
            };

            var res = await dbContext.Contacts.AddAsync(contact);
            if(res != null)
            {
                await dbContext.SaveChangesAsync();
                logger.LogInformation("Contact added: {name}", addContactRequest.Name);
                return Ok(contact);
            }

            logger.LogError("Error while inserting contact");
            return BadRequest("Error while inserting contact");
            
        }

        // update a particular contact in the database
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateContactRequest updateContactRequest)
        {
            logger.LogInformation("Executing {Action}", nameof(UpdateContact));
            var contact = await dbContext.Contacts.FindAsync(id);

            if(contact != null)
            {
                contact.Name = updateContactRequest.Name;   
                contact.Email = updateContactRequest.Email;
                contact.Phone = updateContactRequest.Phone;

                await dbContext.SaveChangesAsync();
                logger.LogInformation("Contact updated: {name}", updateContactRequest.Name);
                return Ok(contact);
            }
            logger.LogError("Contact not found");
            return NotFound();
        }

        // delete a contact from database
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            logger.LogInformation("Executing {Action}", nameof(DeleteContact));
            var contact = dbContext.Contacts.Find(id);

            if(contact != null)
            {
                dbContext.Remove(contact);
                await dbContext.SaveChangesAsync();
                logger.LogInformation("Contact deleted: {name}", contact.Name);
                return Ok(contact);
            }

            logger.LogError("Contact not found");
            return NotFound();
        }
    }
}
