using CommunityToolkit.Mvvm.ComponentModel;
using PIFilmAutoDetachCleanMC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines.Devices
{
    public class DieHardK180Plasma : ObservableObject
    {
        #region Privates
        private readonly Inputs _inputs;
        private readonly Outputs _outputs;
        private readonly AnalogInputs _analogInputs;
        #endregion

        #region Constructor
        public DieHardK180Plasma(Inputs inputs,
            Outputs outputs,
            AnalogInputs analogInputs)
        {
            _inputs = inputs;
            _outputs = outputs;
            _analogInputs = analogInputs;

            System.Timers.Timer plasmaStatusUpdateTimer = new System.Timers.Timer(500);
            plasmaStatusUpdateTimer.Elapsed += PlasmaStatusUpdateTimer_Elapsed;
            plasmaStatusUpdateTimer.AutoReset = true;
            plasmaStatusUpdateTimer.Enabled = true;
        }
        #endregion

        #region Properties
        //KV
        public double Voltage => AnalogConverter.Convert(_analogInputs.PlasmaVoltage.Volt, 0.0, 5.0, 0, 15);
        //KW
        public double Power => AnalogConverter.Convert(_analogInputs.PlasmaPower.Volt, 0.0, 5.0, 0, 2.5);
        //LPM
        public double N2FlowRate => AnalogConverter.Convert(_analogInputs.PlasmaN2FlowRate.Volt, 1.0, 5.0, 0, 1000);
        //LPM
        public double CDAFlowRate => AnalogConverter.Convert(_analogInputs.PlasmaCDAFlowRate.Volt, 1.0, 5.0, 0, 10);
        //C
        public double Temperature => AnalogConverter.Convert(_analogInputs.PlasmaTemperature.Volt, 1.0, 5.0, 0, 100);
        #endregion

        #region Public Methods
        public void EnableRemote()
        {
            _outputs.PlasmaRemoteEnable.Value = true;
        }

        public void AirOpenClose(bool bOpen)
        {
            _outputs.PlasmaN2SolOpen.Value = bOpen;
            _outputs.PlasmaCDASolOpen.Value = bOpen;
        }

        public void PlasmaOnOff(bool bOn)
        {
            _outputs.PlasmaRun.Value = bOn;
        }

        public void IdleMode()
        {
            _outputs.PlasmaRun.Value = false;
            _outputs.PlasmaN2SolOpen.Value = false;
            _outputs.PlasmaCDASolOpen.Value = false;

            _outputs.PlasmaIdleMode.Value = true;
        }

        public void Reset()
        {
            _outputs.PlasmaPowerReset.Value = true;
            Thread.Sleep(500);
            _outputs.PlasmaPowerReset.Value = false;
        }
        #endregion

        #region Private Methods
        private void PlasmaStatusUpdateTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            OnPropertyChanged(nameof(Voltage));
            OnPropertyChanged(nameof(Power));
            OnPropertyChanged(nameof(N2FlowRate));
            OnPropertyChanged(nameof(CDAFlowRate));
            OnPropertyChanged(nameof(Temperature));
        }
        #endregion
    }
}
