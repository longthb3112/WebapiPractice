using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TheCodeCamp.Controllers;
using TheCodeCamp.Data;
using TheCodeCamp.Models;

namespace UnitTestProject2
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod2()
        {
            //Arrange
            var mockRepo = new Mock<ICampRepository>();
            var expected = new List<CampModel>();
            var mockMapper = new Mock<IMapper>();
            List<Camp> list = new List<Camp>();
            Func<IEnumerable<Camp>, Camp[]> selector = str => new Camp[] { };
            
            mockRepo.SetupAsync(x => x.GetAllCampsAsync(It.IsAny<bool>())).Returns<IEnumerable<Camp>>(selector);
            mockMapper.Setup(x => x.Map<IEnumerable<Camp>, IEnumerable<CampModel>>(It.IsAny<IEnumerable<Camp>>()))
                  .Returns(expected);

            //Action
            var campController = new CampsController(mockRepo.Object, mockMapper.Object);
            var result = await campController.Get();

            //Assert
            Assert.IsNotNull(result);
        }
    }
}
