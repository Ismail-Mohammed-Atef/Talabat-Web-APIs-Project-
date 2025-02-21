using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using Talabat.APIS.Dtos;
using Talabat.Core.Entites.Identity;

namespace Talabat.APIS.Extensions
{
	public static class UserManagerExtensions
	{
		public async static Task<AppUser?> FindUserByEmailWithAddress(this UserManager<AppUser> userManager ,ClaimsPrincipal User)
		{
			var userEmail = User.FindFirstValue(ClaimTypes.Email);
			var user = await userManager.Users.Include(u=>u.Address).FirstOrDefaultAsync(u => u.Email == userEmail);

			return user;
		}
	}
}
