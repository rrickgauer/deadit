using Deadit.Lib.Domain.Constants;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Deadit.Lib.Domain.Other;

public class SessionManager(ISession session)
{
    private ISession Session { get; } = session;

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

    public bool IsClientAuthorized()
    {
        return ClientId.HasValue;
    }

}
