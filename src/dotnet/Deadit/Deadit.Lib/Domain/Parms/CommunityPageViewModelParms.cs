﻿using Deadit.Lib.Domain.Contracts;
using Deadit.Lib.Domain.Other;
using Deadit.Lib.Domain.Paging;

namespace Deadit.Lib.Domain.Parms;

public class CommunityPageViewModelParms : IClientId, ICommunityName
{
    public required string CommunityName { get; set; }
    public required uint? ClientId { get; set; }
    public required PostSorting PostSorting { get; set; }
    public required PaginationPosts Pagination { get; set; }
    public required uint? FilterByFlairId { get; set; }
}
