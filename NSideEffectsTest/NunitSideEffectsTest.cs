using NSideEffects;
using NUnit.Framework;

namespace NSideEffectsTest
{
    [TestFixture]
    public class NunitSideEffectsTest
    {
        [Test]
        public void Temp()
        {
            new ProjectParser().Parse(@"..\..\..\lib\nunit.framework.dll");
        }
    }
}