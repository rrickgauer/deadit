﻿
using Deadit.Lib.Domain.TableView;

namespace Deadit.Lib.Domain.ViewModel;

public class CommunityPageViewModel
{
    public required ViewCommunity Community { get; set; }
    public bool IsMember { get; set; } = false;
    public bool IsLoggedIn { get; set; } = false;
}
