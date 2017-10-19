using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Assignment2Tests
{
    [TestClass]
    public class Assignment2Tests
    {
        [TestMethod]
        public void GenericTest_10points()
        {
            GenericList<string> stringList = new GenericList<string>();

            stringList.Add("pero");
            Assert.AreEqual("pero", stringList.GetElement(0));
            Assert.AreEqual(1, stringList.Count);
            Assert.AreEqual(true, stringList.Contains("pero"));

            stringList.Remove("pero");
            Assert.AreEqual(false, stringList.Contains("pero"));
            Assert.AreEqual(0, stringList.Count);

            GenericList<bool> boolList = new GenericList<bool>();

            boolList.Add(false);
            boolList.Add(true);
            boolList.Add(true);
            Assert.AreEqual(false, boolList.GetElement(0));
            Assert.AreEqual(true, boolList.GetElement(1));
            Assert.AreEqual(3, boolList.Count);

            boolList.Remove(false);
            Assert.AreEqual(false, boolList.Contains(false));
            Assert.AreEqual(2, boolList.Count);

        }
    }
}
