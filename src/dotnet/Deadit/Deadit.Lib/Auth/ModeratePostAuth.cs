using Deadit.Lib.Auth.AuthParms;
using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Other;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Service.Contracts;
using Microsoft.AspNetCore.Http;



namespace Deadit.Lib.Auth;

[AutoInject(AutoInjectionType.Scoped, InjectionProject.WebGui)]
public class ModeratePostAuth(IPostService postService, IHttpContextAccessor contextAccessor) : IAsyncPermissionsAuth<ModeratePostAuthData>
{
    private readonly IPostService _postService = postService;
    private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

    private HttpContext? Context => _contextAccessor.HttpContext?.Request.HttpContext;

    public async Task<ServiceResponse> HasPermissionAsync(ModeratePostAuthData data)
    {
        try
        {
            var post = await GetPostAsync(data);
            
            if (post.CommunityOwnerId != data.ClientId)
            {
                throw new ForbiddenHttpResponseException();
            }


            if (Context != null)
            {
                HttpRequestItems items = new(Context)
                {
                    Post = post,    
                };
            }



            return new();
        }
        catch(ServiceResponseException ex)
        {
            return ex;
        }

        
    }


    private async Task<ViewPost> GetPostAsync(ModeratePostAuthData data)
    {
        var getPost = await _postService.GetPostAsync(data.PostId);

        getPost.ThrowIfError();

        var post = NotFoundHttpResponseException.ThrowIfNot<ViewPost>(getPost.Data);

        return post;
    }
}
