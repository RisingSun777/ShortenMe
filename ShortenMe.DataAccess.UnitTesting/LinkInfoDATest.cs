using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShortenMe.DataAccess.Contracts.Models;

namespace ShortenMe.DataAccess.UnitTesting
{
    [TestClass]
    public class LinkInfoDATest
    {
        private LinkInfoDA linkInfoDA;

        [TestInitialize]
        public void TestInit()
        {
            linkInfoDA = new LinkInfoDA();
        }

        [TestMethod]
        public void GetAll_should_retrieve_all_LinkInfo_objects()
        {
            LinkInfo[] ret = linkInfoDA.GetAll();
        }

        [TestMethod]
        public void GetByFullLink_should_retrieve_LinkInfo_object_with_matching_FullLink()
        {
            LinkInfo ret = linkInfoDA.GetUniqueByFullLink("some random string");
            Assert.IsNull(ret);

            ret = linkInfoDA.GetUniqueByFullLink("http://www.google.com");
            Assert.AreEqual(new Guid("0B757FCD-71CB-4295-AE28-7228820067B1"), ret.ID);
            Assert.AreEqual("http://www.google.com", ret.FullLink);
            Assert.AreEqual("abCD3", ret.ShortenedLink);
        }

        [TestMethod]
        public void InsertDeleteTest()
        {
            LinkInfo insertingObj = new LinkInfo
            {
                ID = Guid.NewGuid(),
                FullLink = "http://www.google.com.vn",
                ShortenedLink = "aaAB3"
            };

            linkInfoDA.Insert(insertingObj);

            try
            {
                LinkInfo insertedObj = linkInfoDA.GetUniqueByFullLink(insertingObj.FullLink);

                Assert.AreEqual(insertingObj.ID, insertedObj.ID);
                Assert.AreEqual(insertingObj.FullLink, insertedObj.FullLink);
                Assert.AreEqual(insertingObj.ShortenedLink, insertedObj.ShortenedLink);
            }
            finally
            {
                linkInfoDA.Delete(insertingObj);

                LinkInfo insertedObj = linkInfoDA.GetUniqueByFullLink(insertingObj.FullLink);

                Assert.IsNull(insertedObj);
            }
        }
    }
}
