using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_API_demo.Data;
using Web_API_demo.Models;
using Web_API_demo.Services;

namespace Web_API_demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        // private dbContext used to talk to the database
        // private readonly ContactsAPIDbContext dbContext;

        private readonly IContactsService _contactsService;

        private readonly ILogger<ContactsController> logger;
        
        // inject ContactsAPIDbContext into the controller
        public ContactsController(IContactsService contactsService, ILogger<ContactsController> _logger) 
        {
            _contactsService = contactsService;
            logger = _logger;
        }


        // get method to get all the contacts
        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            logger.LogInformation("Executing {Action}", nameof(GetAllContacts));
            var res = await _contactsService.GetAllContacts();
            if(res != null) 
            {
                logger.LogInformation("Retrievd all contacts");
                return Ok(res);
            }
            
            logger.LogError("No contacts available");
            return BadRequest("Error while retrieving data");
        }

        // get method to get a specific contact using 'id' as a parameter in route
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
            logger.LogInformation("Executing {Action} with parameters {id}", nameof(GetContact), id);
            var contact = await _contactsService.GetContact(id);
            if(contact != null)
            {
                logger.LogInformation("Contact found: {name}", contact.Name);
                return Ok(contact);
                
            }

            logger.LogError("Contact not found");
            return NotFound();
        }

        // add a new contact to the databse
        [HttpPost]
        public async Task<IActionResult> AddContact(AddContactsRequest addContactRequest)
        {
            logger.LogInformation("Executing {Action}", nameof(AddContact));
            var res = await _contactsService.AddContact(addContactRequest);
            if(res != null)
            {
                logger.LogInformation("Contact added: {name}", addContactRequest.Name);
                return Ok(res);
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

            if(updateContactRequest == null)
            {
                logger.LogError("Null input passed");
                return BadRequest("Null input passed");
            }
            if (!ModelState.IsValid)
            {
                logger.LogError("Object input format incorrect");
                return BadRequest("Object input format incorrect");
            }

            int res = await _contactsService.UpdateContact(id, updateContactRequest);
            if(res != 0)
            {
                logger.LogInformation("Contact updated: {name}", updateContactRequest.Name);
                return Ok();
            }

            logger.LogError("Contact not found");
            return NotFound("Contact not found");

        }

        // delete a contact from database
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            logger.LogInformation("Executing {Action}", nameof(DeleteContact));
            int res = await _contactsService.DeleteContact(id);

            if(res == 1)
            {
                logger.LogInformation("Contact deleted");
                return Ok();
            }

            logger.LogError("Contact not found");
            return NotFound();
        }
    }
}
