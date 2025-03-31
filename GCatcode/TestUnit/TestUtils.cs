namespace TestUnit
{
    using GCatcode.Utils;

    [TestClass]
    public sealed class TestUtils
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            // Este método se llama una vez para el ensamblado de pruebas, antes de que se ejecuten las pruebas.
            
        }

        [TestMethod]
        public void TestTripleDESHelper()
        {
            var res =  TripleDESHelper.Encrypt("Hola Mundo");
            Assert.AreEqual("Hola Mundo", TripleDESHelper.Decrypt(res));
        }
    }
}
