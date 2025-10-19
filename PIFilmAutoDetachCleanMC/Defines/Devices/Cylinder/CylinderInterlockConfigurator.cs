using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIFilmAutoDetachCleanMC.Recipe;

namespace PIFilmAutoDetachCleanMC.Defines.Devices.Cylinder
{
    public static class CylinderInterlockConfigurator
    {
        public static void Configure(Devices devices, CSTLoadUnloadRecipe cstLoadUnloadRecipe)
        {
            ConfigureConveryorSupportInterlock(devices, cstLoadUnloadRecipe);
        }

        private static void ConfigureConveryorSupportInterlock(Devices devices, CSTLoadUnloadRecipe cstLoadUnloadRecipe)
        {
            var inWorkTAxis = devices.Motions.InCassetteTAxis;
            var outWorkTAxis = devices.Motions.OutCassetteTAxis;
            if (inWorkTAxis == null || outWorkTAxis == null)
            {
                return;
            }

            var statusNotifierinWork = inWorkTAxis.Status as INotifyPropertyChanged;
            var statusNotifieroutWork = outWorkTAxis.Status as INotifyPropertyChanged;
            


            devices.Cylinders.InWorkCV_SupportCyl1.ConfigureInterlock(
                key: "Cylinder.InWorkCV_SupportCyl1",
                condition: () => inWorkTAxis.Status.IsHomeDone || inWorkTAxis.IsOnPosition(cstLoadUnloadRecipe.InCstTAxisLoadPosition),
                statusNotifierinWork);

            devices.Cylinders.InWorkCV_SupportCyl2.ConfigureInterlock(
                key: "Cylinder.InWorkCV_SupportCyl2",
                condition: () => inWorkTAxis.IsOnPosition(cstLoadUnloadRecipe.InCstTAxisUnloadPosition),
                statusNotifierinWork);

            devices.Cylinders.OutWorkCV_SupportCyl1.ConfigureInterlock(
                key: "Cylinder.OutWorkCV_SupportCyl1",
                condition: () => outWorkTAxis.IsOnPosition(cstLoadUnloadRecipe.OutCstTAxisLoadPosition),
                statusNotifieroutWork);

            devices.Cylinders.OutWorkCV_SupportCyl2.ConfigureInterlock(
                key: "Cylinder.OutWorkCV_SupportCyl2",
                condition: () => outWorkTAxis.Status.IsHomeDone || outWorkTAxis.IsOnPosition(cstLoadUnloadRecipe.OutCstTAxisUnloadPosition),
                statusNotifieroutWork);

        }
    }
}
