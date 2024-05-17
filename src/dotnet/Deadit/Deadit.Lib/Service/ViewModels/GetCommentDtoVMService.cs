using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Dto;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Parms;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Service.Contracts;

namespace Deadit.Lib.Service.ViewModels;

[AutoInject(AutoInjectionType.Scoped, InjectionProject.WebGui)]
public class GetCommentDtoVMService(ICommentService commentService) : IVMService<GetCommentDtoVMServiceParms, GetCommentDto>
{
    private readonly ICommentService _commentService = commentService;

    public async Task<ServiceDataResponse<GetCommentDto>> GetViewModelAsync(GetCommentDtoVMServiceParms args)
    {
        var getComment = await _commentService.GetCommentAsync(args.CommentId);

        if (!getComment.Successful)
        {
            return new(getComment);
        }

        if (getComment.Data is not ViewComment comment)
        {
            throw new NotFoundHttpResponseException();
        }

        var result = (GetCommentDto)comment;
        result.SetIsAuthorRecursive(args.ClientId);

        return new(result);
    }
}
