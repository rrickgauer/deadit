using Deadit.Lib.Domain.Constants;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Deadit.Lib.Auth;

public class SessionManager
{
    private ISession Session { get; }

    public uint? ClientId
    {
        get
        {
            var data = Session.GetString(GuiSessionKeys.AuthUserId);

            if (uint.TryParse(data, out var userId))
            {
                return userId;
            }

            return null;
        }

        set
        {
            if (value == null)
            {
                Session.Remove(GuiSessionKeys.AuthUserId);
            }
            else
            {
                Session.SetString(GuiSessionKeys.AuthUserId, $"{value}");
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
