using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using PIFilmAutoDetachCleanMC.Language;
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
        
        public LanguageService LanguageService => _languageService;

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

        public IRelayCommand UserNavigate => new RelayCommand(() =>
        {
            _navigationService.NavigateTo<LoginViewModel>();
        });

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

        public ICommand ApplicationCloseCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _viewModelProvider.GetViewModel<HeaderViewModel>().ApplicationCloseCommand.Execute(null);
                });
            }
        }

        public IRelayCommand<SupportedLanguage> LanguageSwitchCommand => 
            new RelayCommand<SupportedLanguage>(language =>
            {
                _languageService.CurrentLanguage = language;
                IsLanguageMenuOpen = false;
            });

        public IRelayCommand ToggleLanguageMenuCommand => 
            new RelayCommand(() => IsLanguageMenuOpen = !IsLanguageMenuOpen);

        public bool IsLanguageMenuOpen
        {
            get => _isLanguageMenuOpen;
            set
            {
                if (_isLanguageMenuOpen != value)
                {
                    _isLanguageMenuOpen = value;
                    OnPropertyChanged(nameof(IsLanguageMenuOpen));
                }
            }
        }

        public string CurrentUserLabel
        {
            get => _currentUserLabel;
            private set => SetProperty(ref _currentUserLabel, value);
        }
        #endregion

        public NavigateMenuViewModel(INavigationService navigationService,
            UserStore userStore,
            ViewModelProvider viewModelProvider,
            LanguageService languageService)
        {
            _navigationService = navigationService;
            _userStore = userStore;
            _viewModelProvider = viewModelProvider;
            _languageService = languageService;

            // TODO: Remove this

            _userStore.UserChanged += _userStore_UserChanged;
            UpdateNavigationButtons();
            UpdateCurrentUserLabel();
        }

        private void UpdateNavigationButtons()
        {
            var buttons = new ObservableCollection<NavigationButton>
            {
                new NavigationButton { Label = "Auto", Command = AutoNavigate, ImageKey = "image_auto_selected", DisabledImageKey = "image_auto_normal_dark" }
            };

            if (_userStore.Permission != EPermission.Operator)
            {
                buttons.Add(new NavigationButton { Label = "Manual", Command = ManualNavigate, ImageKey = "image_manual_selected", DisabledImageKey = "image_manual_normal_dark" });
                buttons.Add(new NavigationButton { Label = "Data", Command = DataNavigate, ImageKey = "image_data_selected", DisabledImageKey = "image_data_normal" });
                buttons.Add(new NavigationButton { Label = "Teach", Command = TeachNavigate, ImageKey = "image_teach_selected", DisabledImageKey = "image_teach_normal" });
            }

            if (_userStore.Permission == EPermission.SuperUser)
            {
                buttons.Add(new NavigationButton { Label = "Dev", Command = DevNavigate, ImageKey = "square_auto_seleted", DisabledImageKey = "square_auto_normal" });
            }

            NavigationButtons = buttons;
            OnPropertyChanged(nameof(NavigationButtons));
        }

        private void UpdateCurrentUserLabel()
        {
            var permissionLabel = _userStore.Permission switch
            {
                EPermission.SuperUser => "SuperUser",
                EPermission.Operator => "Operator",
                _ => "Admin"
            };

            CurrentUserLabel = $"{permissionLabel}";
        }

        private void _userStore_UserChanged()
        {
            UpdateNavigationButtons();
            UpdateCurrentUserLabel();
        }

        public override void Dispose()
        {
            _userStore.UserChanged -= _userStore_UserChanged;
            base.Dispose();
        }

        #region Privates
        private readonly INavigationService _navigationService;
        private readonly UserStore _userStore;
        private readonly ViewModelProvider _viewModelProvider;
        private readonly LanguageService _languageService;
        private bool _isLanguageMenuOpen;
        private string _currentUserLabel = string.Empty;
        #endregion
    }
}
