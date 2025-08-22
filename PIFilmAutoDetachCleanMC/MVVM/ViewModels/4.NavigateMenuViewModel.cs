using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class NavigateMenuViewModel : ViewModelBase
    {
        #region Command(s)
        public ObservableCollection<NavigationButton> NavigationButtons { get; set; }

        public IRelayCommand AutoNavigate
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _navigationService.NavigateTo<AutoViewModel>();
                });
            }
        }

        public IRelayCommand ManualNavigate
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _navigationService.NavigateTo<ManualViewModel>();
                });
            }
        }

        public IRelayCommand DataNavigate
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _navigationService.NavigateTo<DataViewModel>();
                });
            }
        }

        public IRelayCommand TeachNavigate
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _navigationService.NavigateTo<TeachViewModel>();
                });
            }
        }

        public IRelayCommand VisionNavigate
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _navigationService.NavigateTo<VisionViewModel>();
                });
            }
        }

        public IRelayCommand LogNavigate
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _navigationService.NavigateTo<LogViewModel>();
                });
            }
        }

        public IRelayCommand LogoutNavigate
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _navigationService.NavigateTo<LoginViewModel>();
                });
            }
        }

        public IRelayCommand DevNavigate
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _navigationService.NavigateTo<DevViewModel>();
                });
            }
        }
        #endregion

        public NavigateMenuViewModel(INavigationService navigationService,
            UserStore userStore)
        {
            _navigationService = navigationService;
            _userStore = userStore;

            // TODO: Remove this
            _userStore.Permission = EPermission.Admin;

            _userStore.UserChanged += _userStore_UserChanged;
            UpdateNavigationButtons();
        }

        private void UpdateNavigationButtons()
        {
            NavigationButtons = new ObservableCollection<NavigationButton>();

            NavigationButtons.Add(new NavigationButton { Label = "Auto", Command = AutoNavigate, ImageKey = "MainUI_SS_Auto_Hover", DisabledImageKey = "MainUI_SS_Auto_Default" });

            if (_userStore.Permission == EPermission.Operator)
            {
                return;
            }

            NavigationButtons.Add(new NavigationButton { Label = "Manual", Command = ManualNavigate, ImageKey = "MainUI_SS_Manual_Hover", DisabledImageKey = "MainUI_SS_Manual_Default" });
            NavigationButtons.Add(new NavigationButton { Label = "Data", Command = DataNavigate, ImageKey = "MainUI_SS_Data_Hover", DisabledImageKey = "MainUI_SS_Data_Default" });
            NavigationButtons.Add(new NavigationButton { Label = "Teach", Command = TeachNavigate, ImageKey = "MainUI_SS_Teach_Hover", DisabledImageKey = "MainUI_SS_Teach_Default" });

            if (_userStore.Permission == EPermission.Admin)
            {
                return;
            }
            if (_userStore.Permission == EPermission.SuperUser)
            {
                NavigationButtons.Add(new NavigationButton { Label = "Dev", Command = DevNavigate, ImageKey = "image_exit_normal_light", DisabledImageKey = "image_exit_normal_dark" });

                return;
            }
        }

        private void _userStore_UserChanged()
        {
            UpdateNavigationButtons();
        }

        #region Privates
        private readonly INavigationService _navigationService;
        private readonly UserStore _userStore;
        #endregion
    }
}
