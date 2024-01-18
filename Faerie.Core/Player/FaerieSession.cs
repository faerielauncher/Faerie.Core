using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.Sessions;
using Microsoft.Extensions.Logging;
using XboxAuthNet.Game.Accounts;

namespace Faerie.Core.Player
{
    public class FaerieSession (JELoginHandler loginHandler)
    {
        public List<JEGameAccount> GetAccountCollection()
        {
            var accounts = loginHandler.AccountManager.GetAccounts();

            foreach (var account in accounts)
            {
                if (account is not JEGameAccount gameAccount)
                    continue;

                logger.LogInformation("Found: {} with UUID: {}", gameAccount.Profile?.Username, gameAccount.Profile?.UUID);
            }

            return accounts.OfType<JEGameAccount>().ToList();
        }

        public void PurgeAllSessions()
        {
            var accounts = loginHandler.AccountManager.GetAccounts();
            foreach (var account in accounts)
            {
                loginHandler.Signout(account);
            }
        }
    }
}
