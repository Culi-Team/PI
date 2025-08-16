using EQX.Core.Common;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly UserStore _userStore;

        public LoginViewModel(UserStore userStore)
        {
            _userStore = userStore;
        }
    }
}
