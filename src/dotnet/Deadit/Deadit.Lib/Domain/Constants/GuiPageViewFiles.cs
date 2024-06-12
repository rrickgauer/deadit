namespace Deadit.Lib.Domain.Constants;

public sealed class GuiPageViewFiles
{
    private const string ParentDirectory = @"Views/Pages";

    public static string HomePage => $"{ParentDirectory}/Home/HomePage.cshtml";
    
    public static string LoginPage => $"{ParentDirectory}/Login/LoginPage.cshtml";

    public static string CommunityPage => $"{ParentDirectory}/Community/CommunityPage/CommunityPage.cshtml";
    public static string CreatePostPage => $"{ParentDirectory}/Community/CreatePost/CreatePostPage.cshtml";
    public static string PostPage => $"{ParentDirectory}/Community/Post/PostPage.cshtml";

    public static string CreateCommunitiesPage => $"{ParentDirectory}/CreateCommunity/CreateCommunityPage.cshtml";
    public static string CommunitiesPage       => $"{ParentDirectory}/Communities/CommunitiesPage.cshtml";
    public static string JoinedCommunitiesPage => $"{ParentDirectory}/Communities/JoinedCommunitiesPage.cshtml";
    public static string CreatedCommunitiesPage => $"{ParentDirectory}/Communities/CreatedCommunitiesPage.cshtml";

    public static string CommunitySettingsLayout => $"{ParentDirectory}/Community/Settings/_CommunitySettingsLayout.cshtml";
    public static string CommunitySettingsGeneralPage => $"{ParentDirectory}/Community/Settings/General/CommunitySettingsGeneralPage.cshtml";
    public static string CommunitySettingsMembersPage => $"{ParentDirectory}/Community/Settings/Members/CommunitySettingsMembersPage.cshtml";
    public static string CommunitySettingsContentPage => $"{ParentDirectory}/Community/Settings/Content/CommunitySettingsContentPage.cshtml";
}
