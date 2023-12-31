﻿namespace Deadit.Lib.Domain.Constants;

public sealed class GuiPageViewFiles
{
    private const string ParentDirectory = @"Views/Pages";

    public static string HomePage => $"{ParentDirectory}/Home/HomePage.cshtml";
    
    public static string LoginPage => $"{ParentDirectory}/Login/LoginPage.cshtml";

    public static string CommunityPage => $"{ParentDirectory}/Community/CommunityPage.cshtml";

    public static string CreateCommunitiesPage => $"{ParentDirectory}/CreateCommunity/CreateCommunityPage.cshtml";
    public static string CommunitiesPage       => $"{ParentDirectory}/Communities/CommunitiesPage.cshtml";
}
