using Deadit.Lib.Domain.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadit.Lib.Auth;

public class SessionManager
{
    private ISession Session { get; }

    public string? ClientId
    {
        get => Session.GetString(SessionKeys.AuthUserId);

        set
        {
            if (value == null)
            {
                Session.Remove(SessionKeys.AuthUserId);
            }
            else
            {
                Session.SetString(SessionKeys.AuthUserId, value);
            }
        }
    }

    public SessionManager(ISession session)
    {
        Session = session;
    }

    public bool IsClientAuthorized()
    {
        if (string.IsNullOrEmpty(ClientId))
        {
            return false;
        }

        return true;
    }

}
