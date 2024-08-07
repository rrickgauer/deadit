﻿using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Constants;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.Model;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Service.Contracts;
using System.Text.RegularExpressions;

namespace Deadit.Lib.Service.Implementations;


[AutoInject<ICommunityService>(AutoInjectionType.Scoped, InjectionProject.Always)]
public class CommunityService(ICommunityRepository repo, ITableMapperService tableMapper, IBannedCommunityNameService banned) : ICommunityService
{
    

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

        try
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
        catch (RepositoryException ex)
        {
            return ex;
        }



    }

    /// <summary>
    /// Validate the new community info before saving it to the database
    /// </summary>
    /// <param name="form"></param>
    /// <returns></returns>
    private async Task<ServiceDataResponse<ViewCommunity>> ValidateNewCommunityAsync(CreateCommunityRequestForm form)
    {
        try
        {
            ServiceDataResponse<ViewCommunity> response = new();

            // ensure no invalid characters
            if (DoesCommunityNameContainInvalidCharacters(form.Name))
            {
                response.AddError(ErrorCode.CommunitySettingsInvalidNameCharacter);
            }

            // check if the name already exists
            var communityNameTaken = await DoesCommunityNameExist(form.Name);

            if (communityNameTaken)
            {
                response.AddError(ErrorCode.CommunitySettingsNameTaken);
            }

            // ensure the name isn't banned
            var communityNameIsBanned = (await _bannedCommunityService.IsBannedCommunityNameAsync(form.Name)).Data;

            if (communityNameIsBanned)
            {
                response.AddError(ErrorCode.CommunitySettingsNameBanned);
            }

            return response;
        }
        catch (RepositoryException ex)
        {
            return ex;
        }


    }

    /// <summary>
    /// Ensure the given community name does not contain any invalid characters
    /// </summary>
    /// <param name="communityName"></param>
    /// <returns></returns>
    private bool DoesCommunityNameContainInvalidCharacters(string communityName)
    {
        return !Regex.IsMatch(communityName, RegexPatterns.UrlCharactersOnly);
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
        try
        {
            ServiceDataResponse<ViewCommunity> response = new()
            {
                Data = await TryGetCommunityByNameAsync(communityName),
            };

            return response;
        }
        catch (RepositoryException ex)
        {
            return ex;
        }


    }

    /// <summary>
    /// Get the community by id
    /// </summary>
    /// <param name="communityId">Community id</param>
    /// <returns></returns>
    public async Task<ServiceDataResponse<ViewCommunity>> GetCommunityAsync(uint communityId)
    {
        try
        {
            ServiceDataResponse<ViewCommunity> response = new();

            var datarow = await _communityRepository.SelectCommunityAsync(communityId);

            if (datarow != null)
            {
                response.Data = _tableMapperService.ToModel<ViewCommunity>(datarow);
            }

            return response;
        }
        catch (RepositoryException ex)
        {
            return ex;
        }

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


    public async Task<ServiceDataResponse<List<ViewCommunity>>> GetCreatedCommunitiesAsync(uint userId)
    {
        
        try
        {
            var table = await _communityRepository.SelectCreatedCommunitiesAsync(userId);

            var models = _tableMapperService.ToModels<ViewCommunity>(table);

            return models;
        }
        catch(RepositoryException ex)
        {
            return ex;
        }
    }


    public async Task<ServiceDataResponse<ViewCommunity>> SaveCommunityAsync(Community community)
    {
        if (community.Id is not uint communityId)
        {
            throw new Exception($"{nameof(community.Id)} is required");
        }

        try
        {
            await _communityRepository.UpdateCommunityAsync(community);
        }
        catch(RepositoryException ex)
        {
            return ex;
        }

        return await GetCommunityAsync(communityId);
    }

}
