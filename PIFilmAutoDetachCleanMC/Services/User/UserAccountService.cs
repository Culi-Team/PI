using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using EQX.Core.Common;
using EQX.Core.Common.Navigation;
using Microsoft.Extensions.Configuration;

namespace PIFilmAutoDetachCleanMC.Services.User
{
    public class UserAccountService : IUserAccountService
    {
        private readonly Dictionary<string, UserAccount> _accounts;

        public UserAccountService()
        {
            _accounts = new Dictionary<string, UserAccount>(StringComparer.OrdinalIgnoreCase)
            {
                ["SuperUser"] = new UserAccount { UserName = "SuperUser", Password = "1", Permission = EPermission.SuperUser },
                ["Admin"] = new UserAccount { UserName = "Admin", Password = "2", Permission = EPermission.Admin },
                ["Operator"] = new UserAccount { UserName = "Operator", Password = "3", Permission = EPermission.Operator }
            };
        }

        public IEnumerable<UserAccount> GetAccounts()
        {
            return _accounts.Values
                .Select(account => account.CloneWithoutPassword())
                .OrderByDescending(account => account.Permission)
                .ThenBy(account => account.UserName)
                .ToList();
        }

        public UserAccount? Authenticate(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || password == null)
            {
                return null;
            }

            if (_accounts.TryGetValue(userName, out var account) && account.Password == password)
            {
                return account.CloneWithoutPassword();
            }

            return null;
        }
    }
}
