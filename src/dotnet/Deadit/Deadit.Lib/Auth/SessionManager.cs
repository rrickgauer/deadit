using Deadit.Lib.Domain.Constants;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Deadit.Lib.Auth;

public class SessionManager
{
    private ISession Session { get; }

    public int? ClientId
    {
        get
        {
            var data = Session.GetString(SessionKeys.AuthUserId);

            if (int.TryParse(data, out var userId))
            {
                return userId;
            }

            return null;
        }

        set
        {
            if (value == null)
            {
                Session.Remove(SessionKeys.AuthUserId);
            }
            else
            {
                Session.SetString(SessionKeys.AuthUserId, $"{value}");
            }
        }
    }

    public SessionManager(ISession session)
    {
        Session = session;
    }

    public bool IsClientAuthorized()
    {
        return ClientId.HasValue;
    }

}
