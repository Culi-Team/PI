using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.UI.Controls;
using PIFilmAutoDetachCleanMC.Services.User;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly UserStore _userStore;
        private readonly INavigationService _navigationService;
        private ObservableCollection<string> accesses;

        public ObservableCollection<string> Accesses
        {
            get => accesses;
            set { accesses = value; OnPropertyChanged(nameof(Accesses)); }
        }

        public string AccessSelected { get; set; }

        public LoginViewModel(UserStore userStore, INavigationService navigationService)
        {
            _userStore = userStore;
            _navigationService = navigationService;

            _userStore.UserChanged += _userStore_UserChanged;

            Accesses = new ObservableCollection<string>(Enum.GetNames(typeof(EPermission)).ToList());
            if (Accesses != null) AccessSelected = Accesses[0];
        }

        private void _userStore_UserChanged()
        {
            _navigationService.NavigateTo<AutoViewModel>();
        }

        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand<string>((password) =>
                {
                    if (password == null) return;
                    if (AccessSelected == EPermission.Admin.ToString())
                    {
                        //if (Password == "123")
                        _userStore.Permission = EPermission.Admin;
                    }
                    else if (AccessSelected == EPermission.Operator.ToString())
                    {
                        //if (Password == "1")
                        _userStore.Permission = EPermission.Operator;
                    }
                    else if (AccessSelected == EPermission.SuperUser.ToString())
                    {
                        string currentPassword = DateTime.Now.ToString("HHdd"); 
                        if (password != currentPassword && password != "3141")
                        {
                            MessageBoxEx.ShowDialog((string)Application.Current.Resources["str_WrongPassword"]);
                            return;
                        }
                        _userStore.Permission = EPermission.SuperUser;
                    }
                });
            }
        }
    }
}
