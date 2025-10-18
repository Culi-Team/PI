using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines.Devices.Cylinder
{
    public static class CylinderInterlockConfigurator
    {
        public static void Configure(Devices devices)
        {
            //ConfigureTransferFixtureInterlock(devices);
            ConfigureDetachInterlock(devices);
            //ConfigureVinylCleanInterlock(devices);
        }

        //private static void ConfigureTransferFixtureInterlock(Devices devices)
        //{
        //    var fixtureAxis = devices.Motions.FixtureTransferYAxis;
        //    if (fixtureAxis == null)
        //    {
        //        return;
        //    }

        //    var statusNotifier = fixtureAxis.Status as INotifyPropertyChanged;

        //    devices.Cylinders.TransferFixture_UpDownCyl.ConfigureInterlock(
        //        key: "Cylinder.TransferFixtureUpDown",
        //        condition: () => fixtureAxis.Status.IsHomeDone
        //                         && devices.Cylinders.TransferFixture_ClampCyl1.IsBackward
        //                         && devices.Cylinders.TransferFixture_ClampCyl2.IsBackward
        //                         && devices.Cylinders.TransferFixture_ClampCyl3.IsBackward
        //                         && devices.Cylinders.TransferFixture_ClampCyl4.IsBackward,
        //        failMessage: "Transfer fixture clamps must be backward and Fixture Transfer Y axis must be homed before moving the TransferFixtureUpDown cylinder.",
        //        statusNotifier,
        //        devices.Cylinders.TransferFixture_ClampCyl1,
        //        devices.Cylinders.TransferFixture_ClampCyl2,
        //        devices.Cylinders.TransferFixture_ClampCyl3,
        //        devices.Cylinders.TransferFixture_ClampCyl4);
        //}

        private static void ConfigureDetachInterlock(Devices devices)
        {
            var detachAxis = devices.Motions.DetachGlassZAxis;
            if (detachAxis == null)
            {
                return;
            }

            var statusNotifier = detachAxis.Status as INotifyPropertyChanged;

            devices.Cylinders.Detach_UpDownCyl1.ConfigureInterlock(
                key: "Cylinder.DetachCyl1UpDown",
                condition: () =>  devices.Cylinders.Detach_ClampCyl3.IsBackward
                                 && devices.Cylinders.Detach_ClampCyl4.IsBackward,
                statusNotifier,
                devices.Cylinders.Detach_ClampCyl1,
                devices.Cylinders.Detach_ClampCyl2);

            devices.Cylinders.Detach_UpDownCyl2.ConfigureInterlock(
                key: "Cylinder.DetachCyl2UpDown",
                condition: () =>  devices.Cylinders.Detach_ClampCyl3.IsBackward
                                 && devices.Cylinders.Detach_ClampCyl4.IsBackward,
                statusNotifier,
                devices.Cylinders.Detach_ClampCyl3,
                devices.Cylinders.Detach_ClampCyl4);
        }

        //private static void ConfigureVinylCleanInterlock(Devices devices)
        //{
        //    devices.Cylinders.VinylCleanPusherRollerUpDown.ConfigureInterlock(
        //        key: "Cylinder.VinylCleanPusherRollerUpDown",
        //        condition: () => devices.Cylinders.VinylCleanRollerFwBw.IsBackward
        //                         && devices.Cylinders.VinylCleanFixture1ClampUnclamp.IsBackward
        //                         && devices.Cylinders.VinylCleanFixture2ClampUnclamp.IsBackward,
        //        failMessage: "Ensure the vinyl clean roller is backward and both fixtures are unclamped before using the VinylCleanPusherRollerUpDown cylinder.",
        //        devices.Cylinders.VinylCleanRollerFwBw,
        //        devices.Cylinders.VinylCleanFixture1ClampUnclamp,
        //        devices.Cylinders.VinylCleanFixture2ClampUnclamp);
        //}
    }
}
