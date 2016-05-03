using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShortenMe.DataAccess.Contracts.Models;
using System.Linq;

namespace ShortenMe.DataAccess.UnitTesting
{
    [TestClass]
    public class DateAccessDATest
    {
        private DateAccessDA dateAccessDA;

        [TestInitialize]
        public void TestInit()
        {
            dateAccessDA = new DateAccessDA();
        }

        [TestMethod]
        public void InsertDeleteTest()
        {
            DateAccess insertingObj = new DateAccess
            {
                ID = Guid.NewGuid(),
                LinkInfoID = new Guid("0B757FCD-71CB-4295-AE28-7228820067B1"),
                DateCreated = new DateTime(2016, 01, 01)
            };

            dateAccessDA.Insert(insertingObj);

            try
            {
                DateAccess insertedObj = dateAccessDA.GetByID(insertingObj.ID);

                Assert.AreEqual(insertingObj.ID, insertedObj.ID);
                Assert.AreEqual(insertingObj.LinkInfoID, insertedObj.LinkInfoID);
                Assert.AreEqual(insertingObj.DateCreated, insertedObj.DateCreated);
            }
            finally
            {
                dateAccessDA.Delete(insertingObj);

                DateAccess insertedObj = dateAccessDA.GetByID(insertingObj.ID);

                Assert.IsNull(insertedObj);
            }
        }

        [TestMethod]
        public void GetByLinkInfoIDTest_success()
        {
            Guid linkInfoID = new Guid("0B757FCD-71CB-4295-AE28-7228820067B1");
            Guid dateAccessID1 = Guid.NewGuid();
            Guid dateAccessID2 = Guid.NewGuid();

            DateAccess[] expected = new DateAccess[] 
            {
                new DateAccess { ID = dateAccessID1, LinkInfoID = linkInfoID, DateCreated = new DateTime(2016, 01, 01) },
                new DateAccess { ID = dateAccessID2, LinkInfoID = linkInfoID, DateCreated = new DateTime(2016, 01, 02) }
            };

            DateAccess[] ret = null;

            dateAccessDA.Insert(expected[0]);
            dateAccessDA.Insert(expected[1]);

            try
            {
                ret = dateAccessDA.GetByLinkInfoID(linkInfoID);

                Assert.AreEqual(expected.Length, ret.Length);
                Assert.AreEqual(dateAccessID1, ret.Single(a => a.ID == dateAccessID1).ID);
                Assert.AreEqual(dateAccessID2, ret.Single(a => a.ID == dateAccessID2).ID);
            }
            finally
            {
                dateAccessDA.Delete(ret[0]);
                dateAccessDA.Delete(ret[1]);
            }
        }
    }
}
