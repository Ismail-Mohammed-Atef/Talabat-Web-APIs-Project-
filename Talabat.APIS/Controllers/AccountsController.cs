using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIS.Dtos;
using Talabat.APIS.Errors;
using Talabat.APIS.Extensions;
using Talabat.Core.Entites.Identity;
using Talabat.Core.Services.Interfaces;

namespace Talabat.APIS.Controllers
{

	public class AccountsController : BaseApiController
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly ITokenService _token;
		private readonly IMapper _mapper;

		public AccountsController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService token,IMapper mapper)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_token = token;
			_mapper = mapper;
		}

		[HttpPost("register")]
		public async Task<ActionResult<UserDto>> Register(RegisterDto model)
		{
			if(EmailExist(model.Email).Result.Value) 
			{
				return BadRequest(new ApiResponse(400, "Duplicated Email Detected"));
			}
			AppUser user = new AppUser()
			{
				DisplayName = model.DisplayName,
				Email = model.Email,
				PhoneNumber = model.PhoneNumber,
				UserName = model.Email.Split("@")[0]
			};

			var operation = await _userManager.CreateAsync(user, model.Password);

			if (!operation.Succeeded)
				return BadRequest(new ApiResponse(400));

			UserDto userDto = new UserDto()
			{
				Email = model.Email,
				Name = user.UserName,
				Token = await _token.CreatTokenAsync(user, _userManager)
			};

			return Ok(userDto);
		}

		[HttpPost("Login")]
		public async Task<ActionResult<UserDto>> Login(LoginDto model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user is null)
				return Unauthorized(new ApiResponse(401));
			var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

			if (!result.Succeeded)
				return Unauthorized(new ApiResponse(401));

			return Ok(new UserDto()
			{
				Email = model.Email,
				Name = user.UserName,
				Token = await _token.CreatTokenAsync(user, _userManager)
			});
		}


		[HttpGet("GetCurrentUser")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<ActionResult<UserDto>> GetCurrentUser()
		{
			var userEmail = User.FindFirstValue(ClaimTypes.Email);

			var result = await _userManager.FindByEmailAsync(userEmail);

			UserDto user = new UserDto()
			{
				Email = result.Email,
				Name = result.UserName,
				Token = await _token.CreatTokenAsync(result, _userManager)
			};

			return Ok(user);
		}

		[HttpGet("GetCurrentUserAddress")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
		{

			var result = await _userManager.FindUserByEmailWithAddress(User);

			var address = _mapper.Map<Address, AddressDto>(result.Address);

			return Ok(address);
		}


		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpPut]
		public async Task<ActionResult<AddressDto>> UpdateCurrentAddress(AddressDto model)
		{
			var user = await _userManager.FindUserByEmailWithAddress(User);

			user.Address = _mapper.Map<AddressDto, Address>(model);

			var result = await  _userManager.UpdateAsync(user);

			if(!result.Succeeded)
			{
				return BadRequest(new ApiResponse(400));
			}

			var address = _mapper.Map<Address, AddressDto>(user.Address);

			return Ok(address);

		}

		[HttpGet("emailExist")]
		public async Task<ActionResult<bool>> EmailExist(string email)
		{
			return await _userManager.FindByEmailAsync(email) is not null;
		}
	}
}
