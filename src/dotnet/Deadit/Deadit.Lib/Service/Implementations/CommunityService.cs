using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Service.Contracts;
using System.Text.RegularExpressions;

namespace Deadit.Lib.Service.Implementations;


[AutoInject<ICommunityService>(AutoInjectionType.Scoped, InjectionProject.Always)]
public class CommunityService(ICommunityRepository repo, ITableMapperService tableMapper, IBannedCommunityNameService banned) : ICommunityService
{
    private const string NewCommunityNameRegexPattern = @"^[a-zA-Z0-9_]*$";

    private readonly ICommunityRepository _communityRepository = repo;
    private readonly ITableMapperService _tableMapperService = tableMapper;
    private readonly IBannedCommunityNameService _bannedCommunityService = banned;

    /// <summary>
    /// Create a new community
    /// </summary>
    /// <param name="form"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<ServiceDataResponse<ViewCommunity>> CreateCommunityAsync(CreateCommunityRequestForm form, uint userId)
    {
        var validateFormResponse = await ValidateNewCommunityAsync(form);

        if (!validateFormResponse.Successful)
        {
            return validateFormResponse;
        }

        ServiceDataResponse<ViewCommunity> response = new();

        var newCommunityId = await InsertNewCommunityAsync(form, userId);

        if (newCommunityId.HasValue)
        {
            response.Data = (await GetCommunityAsync(newCommunityId.Value)).Data;
        }

        return response;
    }

    /// <summary>
    /// Validate the new community info before saving it to the database
    /// </summary>
    /// <param name="form"></param>
    /// <returns></returns>
    private async Task<ServiceDataResponse<ViewCommunity>> ValidateNewCommunityAsync(CreateCommunityRequestForm form)
    {
        ServiceDataResponse<ViewCommunity> response = new();

        // ensure no invalid characters
        if (DoesCommunityNameContainInvalidCharacters(form.Name))
        {
            response.AddError(ErrorCode.CreateCommunityInvalidNameCharacter);
        }

        // check if the name already exists
        var communityNameTaken = await DoesCommunityNameExist(form.Name);

        if (communityNameTaken)
        {
            response.AddError(ErrorCode.CreateCommunityNameTaken);
        }

        // ensure the name isn't banned
        var communityNameIsBanned = (await _bannedCommunityService.IsBannedCommunityNameAsync(form.Name)).Data;

        if (communityNameIsBanned)
        {
            response.AddError(ErrorCode.CreateCommunityNameBanned);
        }

        return response;
    }

    /// <summary>
    /// Ensure the given community name does not contain any invalid characters
    /// </summary>
    /// <param name="communityName"></param>
    /// <returns></returns>
    private bool DoesCommunityNameContainInvalidCharacters(string communityName)
    {
        return !Regex.IsMatch(communityName, NewCommunityNameRegexPattern);
    }

    /// <summary>
    /// Checks if the given community name exists
    /// </summary>
    /// <param name="communityName"></param>
    /// <returns></returns>
    private async Task<bool> DoesCommunityNameExist(string communityName)
    {
        // check if the name already exists
        var existingCommunity = (await GetCommunityAsync(communityName))?.Data;

        if (existingCommunity != null)
        {
            return true;
        }

        return false;
    }
    
    /// <summary>
    /// Save the new community to the database
    /// </summary>
    /// <param name="form"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    private async Task<uint?> InsertNewCommunityAsync(CreateCommunityRequestForm form, uint userId)
    {
        return await _communityRepository.InsertCommunityAsync(form, userId);
    }

    /// <summary>
    /// Get the community by name
    /// </summary>
    /// <param name="communityName">Community Name</param>
    /// <returns></returns>
    public async Task<ServiceDataResponse<ViewCommunity>> GetCommunityAsync(string communityName)
    {
        ServiceDataResponse<ViewCommunity> response = new()
        {
            Data = await TryGetCommunityByNameAsync(communityName),
        };

        return response;
    }

    /// <summary>
    /// Get the community by id
    /// </summary>
    /// <param name="communityId">Community id</param>
    /// <returns></returns>
    public async Task<ServiceDataResponse<ViewCommunity>> GetCommunityAsync(uint communityId)
    {
        ServiceDataResponse<ViewCommunity> response = new();

        var datarow = await _communityRepository.SelectCommunityAsync(communityId);

        if (datarow != null)
        {
            response.Data = _tableMapperService.ToModel<ViewCommunity>(datarow);
        }

        return response;
    }


    private async Task<ViewCommunity?> TryGetCommunityByNameAsync(string communityName)
    {
        var datarow = await _communityRepository.SelectCommunityAsync(communityName);
        
        if (datarow == null)
        {
            return null;
        }

        var community = _tableMapperService.ToModel<ViewCommunity>(datarow);

        return community;
    }

}
