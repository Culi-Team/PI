using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EQX.Core.Common;
using EQX.Core.Common.Navigation;

namespace PIFilmAutoDetachCleanMC.Services.User
{
    public interface IUserAccountService
    {
        IEnumerable<UserAccount> GetAccounts();
        UserAccount? Authenticate(string userName, string password);
    }
}

