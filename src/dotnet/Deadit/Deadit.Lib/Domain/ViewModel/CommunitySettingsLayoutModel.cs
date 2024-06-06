using Deadit.Lib.Domain.TableView;

namespace Deadit.Lib.Domain.ViewModel;

public class CommunitySettingsLayoutModel
{
    public required string PageTitle { get; set; }
}

public class CommunitySettingsLayoutModel<T> : CommunitySettingsLayoutModel
{
    public required T PageModel { get; set; }
}

public class GeneralCommunitySettingsPageModel
{
    public required ViewCommunity Community { get; set; }
}


public class ContentCommunitySettingsPageModel
{
    public required ViewCommunity Community { get; set; }
}

public class MembersCommunitySettingsPageModel
{

}
