using EQX.InOut;
using PIFilmAutoDetachCleanMC.Defines;

namespace PIFilmAutoDetachCleanMC.Test
{
    public class IODeviceTest
    {
        [Fact]
        public void InitialTest()
        {
            var ioDevice = new InputDeviceBase<EInput1>() { Id = 1, Name = "InDevice1", MaxPin = 32 };

            ioDevice.Initialize();

            Assert.Equal(1, ioDevice.Id);
            Assert.Equal("InDevice1", ioDevice.Name);
            Assert.Equal(32, ioDevice.MaxPin);

            for (int i = 0; i < ioDevice.MaxPin; i++)
            {
                Assert.Equal(i, ioDevice.Inputs[i].Id);
                Assert.Equal(((EInput1)i).ToString(), ioDevice.Inputs[i].Name);
            }
        }
    }
}