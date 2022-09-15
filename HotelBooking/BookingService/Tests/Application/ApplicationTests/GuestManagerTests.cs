using Application;
using Application.Guest.DTO;
using Application.Guest.Requests;
using Application.Responses;
using Domain.Entities;
using Domain.Ports;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace ApplicationTests
{
    ////mock na mão do reposítporio
    //public class FakeRepo : IGuestRepository
    //{
    //    public Task<int> Create(Guest guest)
    //    {
    //        return Task.FromResult(111);
    //    }

    //    public Task<Guest> Get(int Id)
    //    {
    //        throw new System.NotImplementedException();
    //    }
    //}

    public class Tests
    {
        GuestManager guestManager;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task HappyPath()
        {
            var guestDto = new GuestDTO
            {
                Name = "Fulano",
                Surname = "Ciclano",
                Email = "abc@gmail.com",
                IdNumber = "abcd",
                IdTypeCode = 1,
            };

            var request = new CreateGuestRequest()
            {
                Data = guestDto,
            };

            var expectedId = 222;

            var fakeRepo = new Mock<IGuestRepository>();
            fakeRepo.Setup(x => x.Create(It.IsAny<Guest>())).Returns(Task.FromResult(expectedId));
            guestManager = new GuestManager(fakeRepo.Object);

            var res = await guestManager.CreateGuest(request);
            Assert.IsNotNull(res);
            Assert.True(res.Success);
            Assert.AreEqual(res.Data.Id, expectedId);
            Assert.AreEqual(res.Data.Name, guestDto.Name);
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("a")]
        [TestCase("ab")]
        [TestCase("abc")]
        public async Task Should_Return_InvalidPersonDocumentIdException_WhenDocAreInvalid(string docNumber)
        {
            var guestDto = new GuestDTO
            {
                Name = "Fulano",
                Surname = "Ciclano",
                Email = "abc@gmail.com",
                IdNumber = docNumber,
                IdTypeCode = 1,
            };

            var request = new CreateGuestRequest()
            {
                Data = guestDto,
            };
           
            var fakeRepo = new Mock<IGuestRepository>();
            fakeRepo.Setup(x => x.Create(It.IsAny<Guest>())).Returns(Task.FromResult(222));
            guestManager = new GuestManager(fakeRepo.Object);

            var res = await guestManager.CreateGuest(request);

            Assert.IsNotNull(res);
            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, ErrorCodes.INVALID_PERSON_ID);
            Assert.AreEqual(res.Message, "The ID passed is not valid");
        }

        [TestCase("","surname","asdf@gmail.com")]
        [TestCase(null,"surname","asdf@gmail.com")]
        [TestCase("Fulano","","asdf@gmail.com")]
        [TestCase("Fulano",null,"asdf@gmail.com")]
        [TestCase("Fulano","surname","")]
        [TestCase("Fulano","surname",null)]        
        public async Task Should_Return_MissingRequiredInformation_WhenNameOrSurnameOrEmailIsEmpty(string name, string surname, string email)
        {
            var guestDto = new GuestDTO
            {
                Name = name,
                Surname = surname,
                Email = email,
                IdNumber = "abcd",
                IdTypeCode = 1,
            };

            var request = new CreateGuestRequest()
            {
                Data = guestDto,
            };

            var fakeRepo = new Mock<IGuestRepository>();
            fakeRepo.Setup(x => x.Create(It.IsAny<Guest>())).Returns(Task.FromResult(222));
            guestManager = new GuestManager(fakeRepo.Object);

            var res = await guestManager.CreateGuest(request);

            Assert.IsNotNull(res);
            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, ErrorCodes.MISSING_REQUIRED_INFORMATION);
            Assert.AreEqual(res.Message, "Missing Required Information");
        }

        [TestCase("b@b.com")]
        public async Task Should_Return_InvalidEmail_WhenEmailEqualB(string email)
        {
            var guestDto = new GuestDTO
            {
                Name = "Fulano",
                Surname = "surname",
                Email = email,
                IdNumber = "abcd",
                IdTypeCode = 1,
            };

            var request = new CreateGuestRequest()
            {
                Data = guestDto,
            };

            var fakeRepo = new Mock<IGuestRepository>();
            fakeRepo.Setup(x => x.Create(It.IsAny<Guest>())).Returns(Task.FromResult(222));
            guestManager = new GuestManager(fakeRepo.Object);

            var res = await guestManager.CreateGuest(request);

            Assert.IsNotNull(res);
            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, ErrorCodes.INVALID_EMAIL);
            Assert.AreEqual(res.Message, "The given email is not valid");
        }
    }
}