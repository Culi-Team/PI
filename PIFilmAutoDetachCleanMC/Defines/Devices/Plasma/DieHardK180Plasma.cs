using CommunityToolkit.Mvvm.ComponentModel;
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

            System.Timers.Timer plasmaStatusUpdateTimer = new System.Timers.Timer(10);
            plasmaStatusUpdateTimer.Elapsed += PlasmaStatusUpdateTimer_Elapsed;
            plasmaStatusUpdateTimer.AutoReset = true;
            plasmaStatusUpdateTimer.Enabled = true;
        }
        #endregion

        #region Properties
        //KV
        public double Voltage => ConvertAnalog(_analogInputs.PlasmaVoltage.Volt, 0.0, 5.0, 0, 15);
        //KW
        public double Power => ConvertAnalog(_analogInputs.PlasmaPower.Volt, 0.0, 5.0, 0, 2.5);
        //LPM
        public double N2FlowRate => ConvertAnalog(_analogInputs.PlasmaN2FlowRate.Volt, 1.0, 5.0, 0, 1000);
        //LPM
        public double CDAFlowRate => ConvertAnalog(_analogInputs.PlasmaCDAFlowRate.Volt, 1.0, 5.0, 0, 10);
        //C
        public double Temperature => ConvertAnalog(_analogInputs.PlasmaTemperature.Volt, 1.0, 5.0, 0, 100);
        #endregion

        #region Private Methods
        private double ConvertAnalog(double value, double vMin, double vMax, double unitMin, double unitMax)
        {
            if (value < vMin) value = vMin;
            if (value > vMax) value = vMax;

            return (value - vMin) * (unitMax - unitMin) / (vMax - vMin) + unitMin;
        }
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
