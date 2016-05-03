using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShortenMe.API.Controllers;
using ShortenMe.DataAccess;
using ShortenMe.Services;
using ShortenMe.DataAccess.Contracts.Models;
using System.Web.Http;
using System.Net.Http;
using ShortenMe.DataAccess.DataAccessModels;

namespace ShortenMe.API.IntegrationTesting
{
    [TestClass]
    public class LinkInfoControllerTest
    {
        private LinkInfoController controller;
        private LinkInfoDA da;
        private LinkInfoService service;

        [TestInitialize]
        public void TestInit()
        {
            da = new LinkInfoDA();
            service = new LinkInfoService(da);
            controller = new LinkInfoController(service);
        }

        [TestMethod]
        public void PostMethod_with_input_already_exist_in_database_should_return_shorten_link_in_database()
        {
            string existingFullLink = "http://www.google.com";
            LinkInfo expectedLinkInfo = da.GetUniqueByFullLink(existingFullLink);

            System.Web.Http.Results.OkNegotiatedContentResult<string> postResult = 
                (System.Web.Http.Results.OkNegotiatedContentResult<string>) controller.Post(new Models.LinkInfoPostModel { FullLink = existingFullLink });

            Assert.AreEqual(expectedLinkInfo.ShortenedLink, postResult.Content);
        }

        [TestMethod]
        public void PostMethod_with_input_not_exist_in_database_should_create_new_object_and_return_the_shorten_link()
        {
            string linkName = "http://www.yahoo.com";
            LinkInfo linkInfo = da.GetUniqueByFullLink(linkName);

            Assert.IsNull(linkInfo);

            System.Web.Http.Results.OkNegotiatedContentResult<string> postResult = 
                (System.Web.Http.Results.OkNegotiatedContentResult<string>)controller.Post(new Models.LinkInfoPostModel { FullLink = linkName });

            linkInfo = da.GetUniqueByFullLink(linkName);

            Assert.AreEqual(linkName, linkInfo.FullLink);
            Assert.AreEqual(linkInfo.ShortenedLink, postResult.Content);

            da.Delete(linkInfo);
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessValidationException))]
        public void PostMethod_with_input_not_exist_in_database_whose_FullName_is_invalid_should_throw_DataAccessValidationException()
        {
            string linkName = "yahoo";

            System.Web.Http.Results.OkNegotiatedContentResult<string> postResult =
                (System.Web.Http.Results.OkNegotiatedContentResult<string>)controller.Post(new Models.LinkInfoPostModel { FullLink = linkName });
        }
    }
}
