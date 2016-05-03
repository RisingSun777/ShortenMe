using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShortenMe.API.Controllers;
using ShortenMe.DataAccess;
using ShortenMe.Services;
using ShortenMe.DataAccess.Contracts.Models;
using System.Web.Http;
using System.Net.Http;
using ShortenMe.DataAccess.DataAccessModels;
using Moq;
using ShortenMe.Services.Contracts;
using System.Web.Http.Results;

namespace ShortenMe.API.IntegrationTesting
{
    [TestClass]
    public class LinkInfoControllerTest
    {
        private LinkInfoController controller;

        private LinkInfoDA linkInfoDA;
        private DateAccessDA dateAccessDA;
        private LinkInfoService service;
        private Mock<IDateTimeProvider> dateTimeProviderMock;

        [TestInitialize]
        public void TestInit()
        {
            dateTimeProviderMock = new Mock<IDateTimeProvider>(MockBehavior.Strict);
            linkInfoDA = new LinkInfoDA();
            dateAccessDA = new DateAccessDA();
            service = new LinkInfoService(linkInfoDA, dateAccessDA, dateTimeProviderMock.Object);

            controller = new LinkInfoController(service);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            dateTimeProviderMock.VerifyAll();
        }

        [TestMethod]
        public void PostMethod_with_input_already_exist_in_database_should_return_shorten_link_in_database()
        {
            string existingFullLink = "http://www.google.com";
            LinkInfo expectedLinkInfo = linkInfoDA.GetUniqueByFullLink(existingFullLink);

            Assert.AreEqual(existingFullLink, expectedLinkInfo.FullLink);
            
            System.Web.Http.Results.OkNegotiatedContentResult<string> postResult = 
                (System.Web.Http.Results.OkNegotiatedContentResult<string>) controller.Post(new Models.LinkInfoPostModel { FullLink = existingFullLink });

            Assert.AreEqual(expectedLinkInfo.ShortenedLink, postResult.Content);
        }

        [TestMethod]
        public void PostMethod_with_input_not_exist_in_database_should_create_new_object_and_return_the_shorten_link()
        {
            string linkName = "http://www.yahoo.com";
            LinkInfo linkInfo = linkInfoDA.GetUniqueByFullLink(linkName);

            Assert.IsNull(linkInfo);

            System.Web.Http.Results.OkNegotiatedContentResult<string> postResult = 
                (System.Web.Http.Results.OkNegotiatedContentResult<string>)controller.Post(new Models.LinkInfoPostModel { FullLink = linkName });

            linkInfo = linkInfoDA.GetUniqueByFullLink(linkName);

            Assert.AreEqual(linkName, linkInfo.FullLink);
            Assert.AreEqual(linkInfo.ShortenedLink, postResult.Content);

            linkInfoDA.Delete(linkInfo);
        }

        [TestMethod]
        public void GetMethod_with_input_not_exist_should_return_NotFound_result()
        {
            string notFoundShortenedLink = "someRandomText";

            LinkInfo linkInfo = linkInfoDA.GetUniqueByShortenedLink(notFoundShortenedLink);

            Assert.IsNull(linkInfo);
            
            IHttpActionResult ret = controller.Get(notFoundShortenedLink);

            Assert.IsInstanceOfType(ret, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetMethod_with_input_existing_should_return_correct_result()
        {
            string shortenedLink = "abCD3";
            DateTime expectedDateCreated = new DateTime(2016, 01, 01);
            DateAccess[] dateAccessLogged = null;

            dateTimeProviderMock
                .Setup(x => x.Now())
                .Returns(expectedDateCreated)
                .Verifiable();

            LinkInfo linkInfo = linkInfoDA.GetUniqueByShortenedLink(shortenedLink);

            Assert.AreEqual(shortenedLink, linkInfo.ShortenedLink);

            try
            {
                IHttpActionResult ret = controller.Get(shortenedLink);

                Assert.IsInstanceOfType(ret, typeof(OkNegotiatedContentResult<string>));
                Assert.AreEqual(linkInfo.FullLink, (ret as OkNegotiatedContentResult<string>).Content);

                dateAccessLogged = dateAccessDA.GetByLinkInfoID(linkInfo.ID);

                Assert.AreEqual(1, dateAccessLogged.Length);
                Assert.AreEqual(linkInfo.ID, dateAccessLogged[0].LinkInfoID);
                Assert.AreEqual(expectedDateCreated, dateAccessLogged[0].DateCreated);
            }
            finally
            {
                dateAccessDA.Delete(dateAccessLogged[0]);
            }
        }
    }
}
