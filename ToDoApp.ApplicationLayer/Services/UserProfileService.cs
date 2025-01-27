using FluentValidation;
using ToDoApp.ApplicationLayer.Services.Contracts;
using ToDoApp.Shared.Models;
using ToDoApp.Shared.Repositories;

namespace ToDoApp.ApplicationLayer.Services;

public class UserProfileService : IUserProfileService
{
	private readonly IUserProfileRepository _userProfileRepository;
	private readonly IValidator<UserProfileModel> _userProfileModelValidator;

	public UserProfileService(IUserProfileRepository userProfileRepository, IValidator<UserProfileModel> userProfileModelValidator)
	{
		_userProfileRepository = userProfileRepository;
		_userProfileModelValidator = userProfileModelValidator;
	}

	public async Task<UserProfileModel?> GetById(int id)
	{
		return await _userProfileRepository.GetById(id);
	}

	public async Task<UserProfileModel?> GetCurrentUserProfile(int id)
	{
		return await _userProfileRepository.GetById(id);
	}

	public async Task<UserProfileModel?> Save(UserProfileModel userProfile)
	{
		var validationResult = await _userProfileModelValidator.ValidateAsync(userProfile);

		if (!validationResult.IsValid)
		{
			return null;
		}

		return await _userProfileRepository.Save(userProfile);
	}
}