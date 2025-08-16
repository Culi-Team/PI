using EQX.Core.InOut;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public class Outputs
    {
        private readonly IDOutputDevice _dOutputDevice;

        public Outputs([FromKeyedServices("OutputDevice#1")] IDOutputDevice dOutputDevice)
        {
            _dOutputDevice = dOutputDevice;
        }

        public bool Initialize()
        {
            return _dOutputDevice.Initialize();
        }

        public bool Connect()
        {
            return _dOutputDevice.Connect();
        }

        public bool Disconnect()
        {
            return _dOutputDevice.Disconnect();
        }

        public List<IDOutput> All => _dOutputDevice.Outputs;

        public IDOutput StartLed => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput1.OUT_SW_START_MAIN);
        public IDOutput StopLed => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput1.OUT_SW_STOP);
    }
}
