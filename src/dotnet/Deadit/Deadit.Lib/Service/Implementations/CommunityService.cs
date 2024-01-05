using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Service.Contracts;
using System.Text.RegularExpressions;

namespace Deadit.Lib.Service.Implementations;

public class CommunityService : ICommunityService
{
    private const string NewCommunityNameRegexPattern = @"^[a-zA-Z0-9_]*$";

    private readonly ICommunityRepository _communityRepository;
    private readonly ITableMapperService _tableMapperService;
    private readonly IBannedCommunityNameService _bannedCommunityService;

    public CommunityService(ICommunityRepository communityRepository, ITableMapperService tableMapperService, IBannedCommunityNameService bannedCommunityService)
    {
        _communityRepository = communityRepository;
        _tableMapperService = tableMapperService;
        _bannedCommunityService = bannedCommunityService;
    }

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


    private async Task<ServiceDataResponse<ViewCommunity>> ValidateNewCommunityAsync(CreateCommunityRequestForm form)
    {
        ServiceDataResponse<ViewCommunity> response = new();

        // ensure no invalid characters
        if (DoesCommunityNameContainInvalidCharacters(form.Name))
        {
            response.Add(ErrorCode.CreateCommunityInvalidNameCharacter);
        }

        // check if the name already exists
        var communityNameTaken = await DoesCommunityNameExist(form.Name);

        if (communityNameTaken)
        {
            response.Add(ErrorCode.CreateCommunityNameTaken);
        }

        // ensure the name isn't banned
        var communityNameIsBanned = (await _bannedCommunityService.IsBannedCommunityNameAsync(form.Name)).Data;

        if (communityNameIsBanned)
        {
            response.Add(ErrorCode.CreateCommunityNameBanned);
        }

        return response;
    }

    private bool DoesCommunityNameContainInvalidCharacters(string communityName)
    {
        return !Regex.IsMatch(communityName, NewCommunityNameRegexPattern);
    }

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



    private async Task<uint?> InsertNewCommunityAsync(CreateCommunityRequestForm form, uint userId)
    {
        return await _communityRepository.InsertCommunityAsync(form, userId);
    }





    public async Task<ServiceDataResponse<ViewCommunity>> GetCommunityAsync(string communityName)
    {
        ServiceDataResponse<ViewCommunity> response = new();

        var datarow = await _communityRepository.SelectCommunityAsync(communityName);

        if (datarow != null)
        {
            response.Data = _tableMapperService.ToModel<ViewCommunity>(datarow);
        }

        return response;
    }

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

}
