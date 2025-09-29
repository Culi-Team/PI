using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using PIFilmAutoDetachCleanMC.Services.User;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly UserStore _userStore;
        private readonly IUserAccountService _userAccountService;
        private readonly INavigationService _navigationService;

        private string _selectedUser = string.Empty;
        private string _loginPassword = string.Empty;
        private string _loginMessage = string.Empty;
        private string _currentUserDescription = string.Empty;

        public ObservableCollection<string> AvailableUsers { get; }

        public string SelectedUser
        {
            get => _selectedUser;
            set
            {
                if (SetProperty(ref _selectedUser, value))
                {
                    LoginCommand.NotifyCanExecuteChanged();
                }
            }
        }

        public string LoginPassword
        {
            get => _loginPassword;
            set
            {
                if (SetProperty(ref _loginPassword, value))
                {
                    LoginCommand.NotifyCanExecuteChanged();
                }
            }
        }

        public string LoginMessage
        {
            get => _loginMessage;
            set => SetProperty(ref _loginMessage, value);
        }

        public string CurrentUserDescription
        {
            get => _currentUserDescription;
            private set => SetProperty(ref _currentUserDescription, value);
        }

        public RelayCommand LoginCommand { get; }

        public RelayCommand LogoutCommand { get; }

        public LoginViewModel(UserStore userStore, IUserAccountService userAccountService, INavigationService navigationService)
        {
            _userStore = userStore;
            _userAccountService = userAccountService;
            _navigationService = navigationService;

            AvailableUsers = new ObservableCollection<string>(_userAccountService.GetAccounts().Select(account => account.UserName));

            if (!AvailableUsers.Any())
            {
                AvailableUsers.Add("Operator");
            }

            _selectedUser = _userStore.UserName;
            if (string.IsNullOrWhiteSpace(_selectedUser) || !AvailableUsers.Contains(_selectedUser))
            {
                _selectedUser = AvailableUsers.First();
            }

            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
            LogoutCommand = new RelayCommand(ExecuteLogout, CanExecuteLogout);

            _userStore.UserChanged += OnUserChanged;
            UpdateCurrentUserDescription();
        }

        private bool CanExecuteLogin() => !string.IsNullOrWhiteSpace(SelectedUser) && !string.IsNullOrEmpty(LoginPassword);

        private void ExecuteLogin()
        {
            LoginMessage = string.Empty;

            var account = _userAccountService.Authenticate(SelectedUser, LoginPassword);
            if (account == null)
            {
                LoginMessage = "Wrong Password";
                return;
            }

            _userStore.SetUser(account);
            LoginPassword = string.Empty;
            SelectedUser = account.UserName;
            LoginMessage = $"Access success {account.Permission}";

            LogoutCommand.NotifyCanExecuteChanged();
            _navigationService.NavigateTo<AutoViewModel>();
        }

        private bool CanExecuteLogout()
        {
            return _userStore.Permission != EPermission.Operator || !string.Equals(_userStore.UserName, "Operator", StringComparison.OrdinalIgnoreCase);
        }

        private void ExecuteLogout()
        {
            _userStore.Reset();
            LoginPassword = string.Empty;
            LoginMessage = "Have been logout and turn into Operator Right";
            SelectedUser = AvailableUsers.FirstOrDefault() ?? string.Empty;

            LogoutCommand.NotifyCanExecuteChanged();
            _navigationService.NavigateTo<AutoViewModel>();
        }

        private void OnUserChanged()
        {
            UpdateCurrentUserDescription();
            if (!AvailableUsers.Contains(_userStore.UserName))
            {
                AvailableUsers.Add(_userStore.UserName);
            }
            SelectedUser = _userStore.UserName;

            LogoutCommand.NotifyCanExecuteChanged();
        }

        private void UpdateCurrentUserDescription()
        {
            var permissionLabel = _userStore.Permission switch
            {
                EPermission.SuperUser => "SuperUser",
                EPermission.Operator => "Operator",
                _ => "Admin"
            };

            CurrentUserDescription = $"Current user: {_userStore.UserName} ({permissionLabel})";
        }

        public override void Dispose()
        {
            _userStore.UserChanged -= OnUserChanged;
            base.Dispose();
        }
    }
}
