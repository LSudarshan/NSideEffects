using NSideEffects;
using NUnit.Framework;

namespace NSideEffectsTest
{
    [TestFixture]
    public class SampleProjectTest
    {
        [Test]
        public void Temp()
        {
            new ProjectParser().Parse(@"..\..\..\Sample\bin\debug\sample.dll");
        }
    }
}