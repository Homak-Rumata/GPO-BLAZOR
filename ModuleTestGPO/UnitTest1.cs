using MigraDoc.DocumentObjectModel.Internals;
using System.Net.Http;
using System.Net.Http.Json;


namespace ModuleTestGPO
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            int calc = 1;
            for (int i = 0; i < 100000; i++)
                calc *= i;
            calc = 0;

            for (int i = 0; i < 1000; i++)
                calc *= i;
            Assert.AreNotEqual(calc, 100);
        }



        [TestMethod]
        public async Task TestMethod3()
        {
            int calc = 1;
            for (int i = 0; i < 5000; i++)
                calc *= i;
            Assert.AreEqual("sucsefull", "sucsefull");
        }
    }
}