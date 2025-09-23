using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Process
{
    public class ProcessInitSelect : ObservableObject
    {
        private bool isInConveyorInit;

        public bool IsInConveyorInit
        {
            get { return isInConveyorInit; }
            set { isInConveyorInit = value; OnPropertyChanged(); }
        }

        private bool isInWorkConveyorInit;

        public bool IsInWorkConveyorInit
        {
            get { return isInWorkConveyorInit; }
            set { isInWorkConveyorInit = value; OnPropertyChanged(); }
        }

        private bool isBufferConveyorInit;

        public bool IsBufferConveyorInit
        {
            get { return isBufferConveyorInit; }
            set { isBufferConveyorInit = value; OnPropertyChanged(); }
        }

        private bool isOutWorkConveyorInit;

        public bool IsOutWorkConveyorInit
        {
            get { return isOutWorkConveyorInit; }
            set { isOutWorkConveyorInit = value; OnPropertyChanged(); }
        }

        private bool isOutConveyorInit;

        public bool IsOutConveyorInit
        {
            get { return isOutConveyorInit; }
            set { isOutConveyorInit = value; OnPropertyChanged(); }
        }

        private bool isTransferFixtureInit;

        public bool IsTransferFixtureInit
        {
            get { return isTransferFixtureInit; }
            set { isTransferFixtureInit = value; OnPropertyChanged(); }
        }

        private bool isRobotLoadInit;

        public bool IsRobotLoadInit
        {
            get { return isRobotLoadInit; }
            set { isRobotLoadInit = value; OnPropertyChanged(); }
        }

        private bool isVinylCleanInit;

        public bool IsVinylCleanInit
        {
            get { return isVinylCleanInit; }
            set { isVinylCleanInit = value; OnPropertyChanged(); }
        }

        private bool isRemoveFilmInit;

        public bool IsRemoveFilmInit
        {
            get { return isRemoveFilmInit; }
            set { isRemoveFilmInit = value; OnPropertyChanged(); }
        }

        private bool isDetachInit;

        public bool IsDetachInit
        {
            get { return isDetachInit; }
            set { isDetachInit = value; OnPropertyChanged(); }
        }

        private bool isFixtureAlignInit;

        public bool IsFixtureAlignInit
        {
            get { return isFixtureAlignInit; }
            set { isFixtureAlignInit = value; OnPropertyChanged(); }
        }

        private bool isGlassTransferInit;

        public bool IsGlassTransferInit
        {
            get { return isGlassTransferInit; }
            set { isGlassTransferInit = value; OnPropertyChanged(); }
        }

        private bool isGlassAlignLeftInit;

        public bool IsGlassAlignLeftInit
        {
            get { return isGlassAlignLeftInit; }
            set { isGlassAlignLeftInit = value; OnPropertyChanged(); }
        }

        private bool isGlassAlignRightInit;

        public bool IsGlassAlignRightInit
        {
            get { return isGlassAlignRightInit; }
            set { isGlassAlignRightInit = value; OnPropertyChanged(); }
        }

        private bool isTransferInShuttleLeftInit;

        public bool IsTransferInShuttleLeftInit
        {
            get { return isTransferInShuttleLeftInit; }
            set { isTransferInShuttleLeftInit = value; OnPropertyChanged(); }
        }

        private bool isTransferInShuttleRightInit;

        public bool IsTransferInShuttleRightInit
        {
            get { return isTransferInShuttleRightInit; }
            set { isTransferInShuttleRightInit = value; OnPropertyChanged(); }
        }

        private bool isWetCleanLeftInit;

        public bool IsWetCleanLeftInit
        {
            get { return isWetCleanLeftInit; }
            set { isWetCleanLeftInit = value; OnPropertyChanged(); }
        }

        private bool isWetCleanRightInit;

        public bool IsWetCleanRightInit
        {
            get { return isWetCleanRightInit; }
            set { isWetCleanRightInit = value; OnPropertyChanged(); }
        }

        private bool isTransferRotationLeftInit;

        public bool IsTransferRotationLeftInit
        {
            get { return isTransferRotationLeftInit; }
            set { isTransferRotationLeftInit = value; OnPropertyChanged(); }
        }

        private bool isTransferRotationRightInit;

        public bool IsTransferRotationRightInit
        {
            get { return isTransferRotationRightInit; }
            set { isTransferRotationRightInit = value; OnPropertyChanged(); }
        }

        private bool isAfCleanLeftInit;

        public bool IsAfCleanLeftInit
        {
            get { return isAfCleanLeftInit; }
            set { isAfCleanLeftInit = value; OnPropertyChanged(); }
        }

        private bool isAfCleanRightInit;

        public bool IsAfCleanRightInit
        {
            get { return isAfCleanRightInit; }
            set { isAfCleanRightInit = value; OnPropertyChanged(); }
        }

        private bool isUnloadTransferLeftInit;

        public bool IsUnloadTransferLeftInit
        {
            get { return isUnloadTransferLeftInit; }
            set { isUnloadTransferLeftInit = value; OnPropertyChanged(); }
        }

        private bool isUnloadTransferRightInit;

        public bool IsUnloadTransferRightInit
        {
            get { return isUnloadTransferRightInit; }
            set { isUnloadTransferRightInit = value; OnPropertyChanged(); }
        }

        private bool isUnloadAlignInit;

        public bool IsUnloadAlignInit
        {
            get { return isUnloadAlignInit; }
            set { isUnloadAlignInit = value; OnPropertyChanged(); }
        }

        private bool isUnloadRobotInit;

        public bool IsUnloadRobotInit
        {
            get { return isUnloadRobotInit; }
            set { isUnloadRobotInit = value; OnPropertyChanged(); }
        }

    }
}
