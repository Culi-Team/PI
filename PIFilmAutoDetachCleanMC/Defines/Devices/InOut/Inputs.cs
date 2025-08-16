using CommunityToolkit.Mvvm.ComponentModel;
using EQX.Core.InOut;
using Microsoft.Extensions.DependencyInjection;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public class Inputs : ObservableObject
    {
        private readonly IDInputDevice _dInputDevice;

        public Inputs([FromKeyedServices("InputDevice#1")]IDInputDevice dInputDevice)
        {
            _dInputDevice = dInputDevice;

            System.Timers.Timer inputUpdateTimer = new System.Timers.Timer(100);
            inputUpdateTimer.Elapsed += InputUpdateTimer_Elapsed;
            inputUpdateTimer.AutoReset = true;
            inputUpdateTimer.Enabled = true;
        }

        public bool Initialize()
        {
            return _dInputDevice.Initialize();
        }

        public bool Connect()
        {
            return _dInputDevice.Connect();
        }

        public bool Disconnect()
        {
            return _dInputDevice.Disconnect();
        }

        private void InputUpdateTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            foreach (var input in _dInputDevice.Inputs)
            {
                input.RaiseValueUpdated();
            }
        }

        public List<IDInput> All => _dInputDevice.Inputs;

        public IDInput StartSW => _dInputDevice.Inputs.First(i => i.Id == (int)EInput1.IN_SW_START_MAIN);
        public IDInput StopSW => _dInputDevice.Inputs.First(i => i.Id == (int)EInput1.IN_SW_STOP);
    }
}
