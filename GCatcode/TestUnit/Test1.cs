namespace TestUnit
{
    using GCatcode.SQLServerDatabase.Models;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using System.ComponentModel.DataAnnotations;

    [TestClass]
    public sealed class Test1
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            // Este método se llama una vez para el ensamblado de pruebas, antes de que se ejecuten las pruebas.
        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
