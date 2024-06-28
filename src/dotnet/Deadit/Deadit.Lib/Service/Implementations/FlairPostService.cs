using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Constants;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Model;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Service.Contracts;
using System.Text.RegularExpressions;

namespace Deadit.Lib.Service.Implementations;

[AutoInject<IFlairPostService>(AutoInjectionType.Scoped, InjectionProject.Always)]
public class FlairPostService(IFlairPostRepository repo, ITableMapperService tableMapperService) : IFlairPostService
{
    private readonly IFlairPostRepository _repo = repo;
    private readonly ITableMapperService _tableMapperService = tableMapperService;

    public async Task<ServiceDataResponse<List<ViewFlairPost>>> GetFlairPostsAsync(uint communityId)
    {
        try
        {
            var table = await _repo.SelectCommunityPostFlairsAsync(communityId);

            var models = _tableMapperService.ToModels<ViewFlairPost>(table);

            return models;
        }
        catch (RepositoryException ex)
        {
            return ex;
        }

    }

    public async Task<ServiceDataResponse<List<ViewFlairPost>>> GetFlairPostsAsync(string communityName)
    {
        try
        {
            var table = await _repo.SelectCommunityPostFlairsAsync(communityName);

            var models = _tableMapperService.ToModels<ViewFlairPost>(table);

            return models;
        }
        catch (RepositoryException ex)
        {
            return ex;
        }
    }

    public async Task<ServiceDataResponse<ViewFlairPost>> GetFlairPostAsync(uint flairId)
    {
        try
        {
            var datarow = await _repo.SelectPostFlairAsync(flairId);

            if (datarow == null)
            {
                return new();
            }

            return _tableMapperService.ToModel<ViewFlairPost>(datarow);
        }
        catch (RepositoryException ex)
        {
            return ex;
        }
    }



    public async Task<ServiceDataResponse<ViewFlairPost>> CreateFlairPostAsync(FlairPost flair)
    {
        return await SaveFlairPostAsync(flair, false);
    }

    public async Task<ServiceDataResponse<ViewFlairPost>> UpdateFlairPostAsync(FlairPost flair)
    {
        return await SaveFlairPostAsync(flair, true);
    }

    private async Task<ServiceDataResponse<ViewFlairPost>> SaveFlairPostAsync(FlairPost flair, bool isUpdate)
    {
        try
        {
            var validationResult = await ValidateFlairSaveAsync(flair, isUpdate);

            if (!validationResult.Successful)
            {
                return new(validationResult.Errors);
            }

            return await UpsertFlairAsync(flair);

        }
        catch (ServiceResponseException ex)
        {
            return ex;
        }
    }


    private async Task<ServiceDataResponse<ViewFlairPost>> UpsertFlairAsync(FlairPost flair)
    {

        uint flairId;

        if (flair.Id.HasValue)
        {
            flairId = flair.Id.Value;

            await _repo.UpdateFlairAsync(flair);
        }
        else
        {
            var newFlairId = await _repo.InsertFlairAsync(flair);

            if (!newFlairId.HasValue)
            {
                throw new NotFoundHttpResponseException();
            }

            flairId = newFlairId.Value;
        }

        return await GetFlairPostAsync(flairId);
    }




    private async Task<ServiceResponse> ValidateFlairSaveAsync(FlairPost flair, bool isUpdate)
    {
        ServiceResponse result = new();

        // ensure name has valid characters
        if (DoesFlairNameContainInvalidCharacters(flair.Name!))
        {
            return new(ErrorCode.FlairPostNameContainsInvalidCharacter);
        }

        // check if community already has a flair with this name
        var isDuplicate = await IsDuplicateCommunityFlairName(flair, isUpdate);

        if (isDuplicate)
        {
            return new(ErrorCode.FlairPostDuplicateName);
        }

        if (IsFlairColorInvalid(flair.Color!))
        {
            return new(ErrorCode.FlairPostInvalidColor);
        }

        return result;
    }

    /// <summary>
    /// Ensure the given flair name does not contain any invalid characters
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private static bool DoesFlairNameContainInvalidCharacters(string name)
    {
        return !Regex.IsMatch(name, RegexPatterns.UrlCharactersOnly);
    }


    private async Task<bool> IsDuplicateCommunityFlairName(FlairPost flair, bool isUpdate)
    {
        if (flair.CommunityId is not uint communityId)
        {
            throw new ArgumentNullException(nameof(flair.CommunityId));
        }

        var getFlairs = await GetFlairPostsAsync(communityId);

        if (!getFlairs.Successful)
        {
            throw new ServiceResponseException(getFlairs);
        }


        var communityFlairs = getFlairs.Data ?? new();


        var existingFlair = communityFlairs.FirstOrDefault(f => f.FlairPostName!.Equals(flair.Name, StringComparison.OrdinalIgnoreCase));

        if (existingFlair is not ViewFlairPost existingFlairPost)
        {
            return false;
        }
        

        if (!isUpdate)
        {
            return true;
        }
        
        if (isUpdate && existingFlairPost.FlairPostId != flair.Id)
        {
            return true;
        }

        return false;
    }


    private static bool IsFlairColorInvalid(string color)
    {
        return !Regex.IsMatch(color, RegexPatterns.HexColor);
    }



    public async Task<ServiceResponse> DeleteFlairPostAsync(uint flairId)
    {
        try
        {
            var numrecords = await _repo.DeleteFlairAsync(flairId);
            return new();
        }
        catch(RepositoryException ex)
        {
            return ex;
        }
    }





}
