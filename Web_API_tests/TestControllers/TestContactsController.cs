using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Web_API_demo.Controllers;
using Web_API_demo.Models;
using Web_API_demo.Services;
using Web_API_tests.TestServices;

namespace Web_API_tests.TestControllers
{
    public class TestContactsController
    {
        private readonly ContactsController _controller;
        private readonly IContactsService _service;
        private readonly ILogger<ContactsController> logger;

        public TestContactsController(ILogger<ContactsController> _logger)
        {
            logger = _logger;
            _service = new TestContactsServices();
            _controller = new ContactsController(_service, logger);
        }

        // Get all contacts
        [Fact]
        public void GetContacts_WhenCalled_ReturnsOkResult() 
        {
            // Act
            var okResult = _controller.GetAllContacts();
            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result as OkObjectResult);
        }
        [Fact]
        public void GetContacts_WhenCalled_ReturnsAllContacts()
        {
            // Act
            var okResult = _controller.GetAllContacts();
            // Assert
            var contacts = Assert.IsType<List<Contact>>(okResult.Result);
            Assert.Equal(3, contacts.Count);
        }

        // Get contact by id
        [Fact]
        public void GetContact_UnknownGuidPassed_ReturnsNotFound()
        {
            //Act
            var notFoundResult = _controller.GetContact(Guid.NewGuid());
            //Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public void GetContact_ExistingGuidPassed_ReturnsOkResult()
        {
            //Arrange
            var testGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            //Act
            var okResult = _controller.GetContact(testGuid);
            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void ExistingGuidPassed_ReturnsRightContact()
        {
            //Arrange
            var testGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            //Act
            var okResult = _controller.GetContact(testGuid);
            //Assert
            Assert.IsType<Contact>(okResult.Result);
            Assert.Equal(testGuid, (okResult.Result as Contact)?.Id);
        }

        // Add Contact
        [Fact]
        public void Add_InvalidObjectPassed_ReturnsBadRequest()
        {   
            //Arrange
            var nameMissingContact = new AddContactsRequest()
            {
                Email = "test4@email.com",
                Phone = 9087651234
            };
            _controller.ModelState.AddModelError("Name", "Required");

            //Act
            var badresponse = _controller.AddContact(nameMissingContact);
            // Assert
            Assert.IsType<BadRequestObjectResult>(badresponse);
        }

        [Fact]
        public void Add_ValidObjectPassed_ReturnsCreatedRespone()
        {
            //Arrange
            AddContactsRequest testContact = new AddContactsRequest()
            {
                Name = "Test4",
                Email = "test4@email.com",
                Phone = 9087651234
            };

            //Act
            var createdResponse = _controller.AddContact(testContact);
            //Assert
            Assert.IsType<CreatedAtActionResult>(createdResponse);
        }

        [Fact]
        public void Add_ValidObjectPassed_ReturnedResponseHasCreatedContact()
        {
            //Arrange
            AddContactsRequest testContact = new AddContactsRequest()
            {
                Name = "Test4",
                Email = "test4@email.com",
                Phone = 9087651234
            };

            //Act
            var createdResponse = _controller.AddContact(testContact);
            var contact = createdResponse.Result as Contact;
            //Assert
            Assert.IsType<Contact>(contact);
            Assert.Equal("Test4", contact.Name);
        }

        // Update Contact
        [Fact]
        public void Update_UnknownGuidPassed_ReturnsNotFound()
        {
            //Act
            var notFoundResult = _controller.GetContact(Guid.NewGuid());
            //Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public void Update_InvalidObjectPassed_ReturnsBadRequest()
        {
            //Arrange
            var testId = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            var nameMissingContact = new UpdateContactRequest()
            {
                Email = "test4@email.com",
                Phone = 9087651234
            };
            _controller.ModelState.AddModelError("Name", "Required");

            //Act
            var badresponse = _controller.UpdateContact(testId, nameMissingContact);
            // Assert
            Assert.IsType<BadRequestObjectResult>(badresponse);
        }

        [Fact]
        public void Update_ValidObjectPassed_ReturnedResponseHasUpdatedContact()
        {
            // Arrange
            var testId = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            var testContact = new UpdateContactRequest()
            {
                Name = "Test0",
                Email = "test0@email.com",
                Phone = 9087651234
            };

            //Act
            var updatedresponse = _controller.UpdateContact(testId, testContact);
            var updatedContact = _controller.GetContact(testId).Result;
            //Assert
            Assert.Equal(testContact.Name, (updatedContact as Contact)?.Name);
        }

        // Delete contact
        [Fact]
        public void Delete_NonExixtingGuidPassed_ReturnsNotFoundResponse()
        {
            //Arrange
            var nonExistingId = Guid.NewGuid();
            //Act
            var badResponse = _controller.DeleteContact(nonExistingId);
            //Assert
            Assert.IsType<NotFoundObjectResult>(badResponse);
        }

        [Fact]
        public void Delete_ExistingIdPassed_RemovesOneContact()
        {
            //Arrange
            var existingId = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            //Act
            var okResponse = _controller.DeleteContact(existingId).Result;
            //Assert
            int val = Assert.IsType<int>(okResponse);
            Assert.Equal(1, val);
        }
    }

}
