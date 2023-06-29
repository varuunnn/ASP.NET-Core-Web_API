using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Web_API_demo.Controllers;
using Web_API_demo.Models;
using Web_API_demo.Services;
using Web_API_tests.TestServices;

namespace Web_API_tests.TestControllers
{
    public class TestContactsController: Controller
    {
        private readonly ContactsController _controller;
        private readonly IContactsService _service;
        private readonly ILogger<ContactsController> logger = new NullLogger<ContactsController>();


        public TestContactsController()
        {
            _service = new TestContactsServices();
            _controller = new ContactsController(_service, logger);
        }

        // Get all contacts
        [Fact]
        public async void GetContacts_WhenCalled_ReturnsOkResult() 
        {
            // Act
            var okResult = await _controller.GetAllContacts();
            // Assert
            Assert.IsType<OkObjectResult>(okResult);
        }
        [Fact]
        public async void GetContacts_WhenCalled_ReturnsAllContacts()
        {
            // Act
            var okResult = await _controller.GetAllContacts() as OkObjectResult;
            // Assert
            var contacts = Assert.IsType<List<Contact>>(okResult?.Value);
            Assert.Equal(3, contacts.Count);
        }

        // Get contact by id
        [Fact]
        public async void GetContact_UnknownGuidPassed_ReturnsNotFound()
        {
            //Act
            var notFoundResult = await _controller.GetContact(Guid.NewGuid());
            //Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async void GetContact_ExistingGuidPassed_ReturnsOkResult()
        {
            //Arrange
            var testGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            //Act
            var okResult = await _controller.GetContact(testGuid);
            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public async void ExistingGuidPassed_ReturnsRightContact()
        {
            //Arrange
            var testGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            //Act
            var okResult = await _controller.GetContact(testGuid) as OkObjectResult;
            //Assert
            var contact = Assert.IsType<Contact>(okResult?.Value);
            Assert.Equal(testGuid, (contact)?.Id);
        }

        // Add Contact
        //[Fact]
        //public async void Add_InvalidObjectPassed_ReturnsBadRequest()
        //{   
        //    //Arrange
        //    var nameMissingContact = new AddContactsRequest()
        //    {
        //        Email = "test4@email.com",
        //        Phone = 9087651234
        //    };
        //    _controller.ModelState.AddModelError("Name", "Required");

        //    //Act
        //    var badResponse = await _controller.AddContact(nameMissingContact);
        //    // Assert
        //    Assert.IsType<BadRequestObjectResult>(badResponse);
        //}

        [Fact]
        public async void Add_ValidObjectPassed_ReturnsOkResult()
        {
            //Arrange
            AddContactsRequest testContact = new AddContactsRequest()
            {
                Name = "Test4",
                Email = "test4@email.com",
                Phone = 9087651234
            };

            //Act
            var okResult = await _controller.AddContact(testContact);
            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public async void Add_ValidObjectPassed_ReturnedResponseHasCreatedContact()
        {
            //Arrange
            AddContactsRequest testContact = new AddContactsRequest()
            {
                Name = "Test4",
                Email = "test4@email.com",
                Phone = 9087651234
            };

            //Act
            var createdResponse = await _controller.AddContact(testContact) as OkObjectResult;
            //Assert
            var contact = Assert.IsType<Contact>(createdResponse?.Value);
            Assert.Equal("Test4", contact?.Name);
        }

        // Update Contact
        [Fact]
        public async void Update_UnknownGuidPassed_ReturnsNotFound()
        {
            //Act
            var notFoundResult = await _controller.GetContact(Guid.NewGuid());
            //Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async void Update_InvalidObjectPassed_ReturnsBadRequest()
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
            var badResponse = await _controller.UpdateContact(testId, nameMissingContact);
            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }

        [Fact]
        public async void Update_ValidObjectPassed_ReturnedOkResult()
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
            var updatedresponse = await _controller.UpdateContact(testId, testContact);
            //Assert
            Assert.IsType<OkResult>(updatedresponse);
        }

        // Delete contact
        [Fact]
        public async void Delete_NonExixtingGuidPassed_ReturnsNotFoundResponse()
        {
            //Arrange
            var nonExistingId = Guid.NewGuid();
            //Act
            var badResponse = await _controller.DeleteContact(nonExistingId);
            //Assert
            Assert.IsType<NotFoundResult>(badResponse);
        }

        [Fact]
        public async void Delete_ExistingIdPassed_RemovesOneContact()
        {
            //Arrange
            var existingId = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            //Act
            var okResponse = await _controller.DeleteContact(existingId);
            //Assert
            Assert.IsType<OkResult>(okResponse);
        }
    }

}
