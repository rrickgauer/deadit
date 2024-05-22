using Deadit.Lib.Domain.Dto;

namespace Deadit.Lib.Domain.ViewModel;

public class GetCommentsApiViewModel
{
    public required List<GetCommentDto> Comments { get; set; }
    public required bool IsLoggedIn { get; set; }
    public required bool PostIsDeleted { get; set; }
}
