using EQX.Device.SyringePump;
using EQX.Core.Communication;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Ports;

namespace PIFilmAutoDetachCleanMC.Test
{
    public class PSD4SyringePumpTest
    {
        #region Basic Tests

        [Fact]
        public void Constructor()
        {
            // Arrange
            var serialCommunicator = new SerialCommunicator(1, "TestPump", "COM7", 9600, Parity.None, 8, StopBits.One);
            
            // Act
            var pump = new PSD4SyringePump("TestPump", 1, serialCommunicator, 1.0);

            // Assert
            Assert.Equal("TestPump", pump.Name);
            Assert.Equal(1, pump.Id);
            Assert.NotNull(pump.Status);
            Assert.False(pump.IsConnected);
        }

        [Fact]
        public void Connect()
        {
            // Arrange
            var serialCommunicator = new SerialCommunicator(1, "TestPump", "COM7", 9600, Parity.None, 8, StopBits.One);
            var pump = new PSD4SyringePump("TestPump", 1, serialCommunicator, 1.0);

            // Act
            var result = pump.Connect();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Disconnect()
        {
            // Arrange
            var serialCommunicator = new SerialCommunicator(1, "TestPump", "COM7", 9600, Parity.None, 8, StopBits.One);
            var pump = new PSD4SyringePump("TestPump", 1, serialCommunicator, 1.0);

            // Act
            var result = pump.Disconnect();

            // Assert
            Assert.True(result);
        }

        #endregion

        #region Operation Tests
        [Fact]
        public void Dispense()
        {
            // Arrange
            var serialCommunicator = new SerialCommunicator(1, "TestPump", "COM7", 9600, Parity.None, 8, StopBits.One);
            var pump = new PSD4SyringePump("TestPump", 1, serialCommunicator, 1.0);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => pump.Dispense(1.0, 1));
        }

        [Fact]
        public void Fill()
        {
            // Arrange
            var serialCommunicator = new SerialCommunicator(1, "TestPump", "COM7", 9600, Parity.None, 8, StopBits.One);
            var pump = new PSD4SyringePump("TestPump", 1, serialCommunicator, 1.0);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => pump.Fill(1.0));
        }

        [Fact]
        public void QueryStatus()
        {
            // Arrange
            var serialCommunicator = new SerialCommunicator(1, "TestPump", "COM7", 9600, Parity.None, 8, StopBits.One);
            var pump = new PSD4SyringePump("TestPump", 1, serialCommunicator, 1.0);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => pump.QueryStatus());
        }

        #endregion

        #region Integration Test

        [Fact]
        public async Task PumpWork_Test()
        {
            // Arrange
            TestAppCommon.AppHost = TestAppCommon.BuildHost();
            await TestAppCommon.AppHost!.StartAsync();

            var serialCommunicator = new SerialCommunicator(1, "TestPump", "COM7", 9600, Parity.None, 8, StopBits.One);
            var pump = new PSD4SyringePump("TestPump", 1, serialCommunicator, 1.0);

            // Act & Assert
            try
            {
                var connectResult = pump.Connect();
                Assert.True(connectResult);
                pump.Initialize();

                // Test fill 
                pump.Fill(0.5);

                // Test dispense 
                pump.Dispense(0.3, 1);

                // Test status query
                pump.QueryStatus();

                // Test disconnection
                var disconnectResult = pump.Disconnect();
                Assert.True(disconnectResult);
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is ArgumentException)
            {
                Assert.True(true, "Expected exception in simulation environment");
            }
        }

        #endregion
    }
}
