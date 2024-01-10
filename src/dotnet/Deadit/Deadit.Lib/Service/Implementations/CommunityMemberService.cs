using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Dto;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Model;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Service.Contracts;
using System.Reflection.Metadata.Ecma335;

namespace Deadit.Lib.Service.Implementations;

[AutoInject(AutoInjectionType.Scoped, InjectionProject.Always, InterfaceType = typeof(ICommunityMemberService))]
public class CommunityMemberService : ICommunityMemberService
{
    private readonly ICommunityMembershipRepository _repo;
    private readonly ITableMapperService _tableMapperService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="repo"></param>
    /// <param name="tableMapperService"></param>
    public CommunityMemberService(ICommunityMembershipRepository repo, ITableMapperService tableMapperService)
    {
        _repo = repo;
        _tableMapperService = tableMapperService;
    }

    /// <summary>
    /// Get the user's joined communities
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<ServiceDataResponse<IEnumerable<ViewCommunityMembership>>> GetJoinedCommunitiesAsync(uint userId)
    {
        var datatable = await _repo.SelectUserJoinedCommunitiesAsync(userId);
        var models = _tableMapperService.ToModels<ViewCommunityMembership>(datatable);
        return new(models);   
    }

    /// <summary>
    /// Checks if the user is a member of the specified community
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="communityId"></param>
    /// <returns></returns>
    public async Task<ServiceDataResponse<bool>> IsMemberAsync(uint userId, uint communityId)
    {
        var getMembershipResponse = (await GetMembershipAsync(userId, communityId)).Data;

        if (getMembershipResponse == null)
        {
            return new(false);
        }

        return new(true);
    }

    /// <summary>
    /// Get the specified community membership
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="communityId"></param>
    /// <returns></returns>
    public async Task<ServiceDataResponse<ViewCommunityMembership>> GetMembershipAsync(uint userId, uint communityId)
    {
        ServiceDataResponse<ViewCommunityMembership> response = new();

        var datarow = await _repo.SelectCommunityMembershipAsync(userId, communityId);
        
        if (datarow != null)
        {
            response.Data = _tableMapperService.ToModel<ViewCommunityMembership>(datarow);
        }

        return response;
    }

    /// <summary>
    /// Get the specified membership
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="communityName"></param>
    /// <returns></returns>
    public async Task<ServiceDataResponse<ViewCommunityMembership>> GetMembershipAsync(uint userId, string communityName)
    {
        ServiceDataResponse<ViewCommunityMembership> response = new();

        var datarow = await _repo.SelectCommunityMembershipAsync(userId, communityName);

        if (datarow != null)
        {
            response.Data = _tableMapperService.ToModel<ViewCommunityMembership>(datarow);
        }

        return response;
    }


    /// <summary>
    /// Leave the community
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="communityName"></param>
    /// <returns></returns>
    public async Task<ServiceDataResponse<int>> LeaveCommunityAsync(uint userId, string communityName)
    {
        var numRecords = await _repo.DeleteMembershipAsync(userId, communityName);

        return new(numRecords);
    }

    /// <summary>
    /// Join the community
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="communityName"></param>
    /// <returns></returns>
    public async Task<ServiceDataResponse<GetJoinedCommunity>> JoinCommunityAsync(uint userId, string communityName)
    {
        ServiceDataResponse<GetJoinedCommunity> response = new();

        var insertResult = await SaveCommunityMembershipAsync(userId, communityName);

        if (!insertResult.Successful)
        {
            response.Errors = insertResult.Errors;
            return response;
        }

        var newMembershipResponse = await GetMembershipAsync(userId, communityName);

        if (newMembershipResponse.Data != null)
        {
            response.Data = (GetJoinedCommunity)newMembershipResponse.Data;
        }
        return response;
    }



    /// <summary>
    /// Save the community membership to the database
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="communityName"></param>
    /// <returns></returns>
    private async Task<ServiceDataResponse<int>> SaveCommunityMembershipAsync(uint userId, string communityName)
    {
        ServiceDataResponse<int> response = new();

        var canJoinCommunity = await CanJoinCommunityAsync(userId, communityName);

        if (!canJoinCommunity.Successful)
        {
            response.Errors = canJoinCommunity.Errors;
            return response;
        }

        response.Data = await _repo.InsertMembershipAsync(userId, communityName);

        return response;
    }

    /// <summary>
    /// Validates a user joining the community
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="communityId"></param>
    /// <returns></returns>
    private async Task<ServiceResponse> CanJoinCommunityAsync(uint userId, string communityId)
    {
        ServiceResponse response = new();

        return response;
    }

}
