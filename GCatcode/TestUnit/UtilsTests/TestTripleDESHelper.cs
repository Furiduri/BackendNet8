namespace TestUnit.UtilsTests
{
    using GCatcode.Utils;

    [TestClass]
    public sealed class TestTripleDESHelper
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            
        }

        [TestMethod]
        public void EncryptDecryptTest()
        {
            var res = TripleDESHelper.Encrypt("Hola Mundo");
            Assert.AreEqual("Hola Mundo", TripleDESHelper.Decrypt(res));
        }
    }
}