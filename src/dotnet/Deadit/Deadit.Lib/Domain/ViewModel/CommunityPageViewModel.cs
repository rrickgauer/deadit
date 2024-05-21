﻿using Deadit.Lib.Domain.Dto;
using Deadit.Lib.Domain.Other;
using Deadit.Lib.Domain.Paging;
using Deadit.Lib.Domain.TableView;
using static Deadit.Lib.Domain.ViewModel.ViewModelContracts;

namespace Deadit.Lib.Domain.ViewModel;

public class CommunityPageViewModel : ILoggedIn
{
    public required ViewCommunity Community { get; set; }
    public bool IsMember { get; set; } = false;
    public bool IsLoggedIn { get; set; } = false;
    public required List<GetPostUserVoteDto> PostDtos { get; set; } = new();
    public required PostSorting PostSort { get; set; }
    public required PaginationPosts Pagination { get; set; }
}

