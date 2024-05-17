using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Model;
using Deadit.Lib.Domain.Parms;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Service.Contracts;
using Deadit.Lib.Utility;

namespace Deadit.Lib.Service.Implementations;

[AutoInject<ICommentService>(AutoInjectionType.Scoped, InjectionProject.Always)]
public class CommentService(ITableMapperService tableMapperService, ICommentRepository repo) : ICommentService
{
    private readonly ITableMapperService _tableMapperService = tableMapperService;
    private readonly ICommentRepository _repo = repo;

    public async Task<ServiceDataResponse<List<ViewComment>>> GetCommentsNestedAsync(GetCommentsParms args)
    {
        // gather a flat list of comments
        var getComments = await GetCommentsAsync(args);

        if (!getComments.Successful)
        {
            return new(getComments);
        }

        // build a nested tree with replies from the flat list
        List<ViewComment> comments = getComments.Data ?? new();

        var commentsTree = comments.ToTree();

        return new(commentsTree);
    }

    public async Task<ServiceDataResponse<List<ViewComment>>> GetCommentsAsync(GetCommentsParms args)
    {
        try
        {
            var selectResult = await _repo.SelectAllPostCommentsAsync(args.PostId, args.SortOption);

            var models = _tableMapperService.ToModels<ViewComment>(selectResult);

            return new(models);
        }
        catch(RepositoryException ex)
        {
            return ex;
        }
    }

    public async Task<ServiceDataResponse<ViewComment>> SaveCommentAsync(Comment comment)
    {
        if (comment.Id is not Guid commentId)
        {
            throw new ArgumentNullException(nameof(comment.Id));
        }

        try
        {
            await _repo.SaveCommentAsync(comment);
        }
        catch(RepositoryException ex)
        {
            return ex;
        }
        
        return await GetCommentAsync(commentId);
    }


    public async Task<ServiceDataResponse<ViewComment>> GetCommentAsync(Guid commentId)
    {
        try
        {
            ServiceDataResponse<ViewComment> result = new();

            var datarow = await _repo.SelectCommentAsync(commentId);

            if (datarow != null)
            {
                result.Data = _tableMapperService.ToModel<ViewComment>(datarow);
            }

            return result;
        }
        catch(RepositoryException ex)
        {
            return ex;
        }
    }


    public async Task<ServiceResponse> DeleteCommentAsync(Guid commentId)
    {
        try
        {
            var numrecords = await _repo.DeleteCommentAsync(commentId);

            return new();
        }
        catch(RepositoryException ex)
        {
            return ex;
        }
    }


}