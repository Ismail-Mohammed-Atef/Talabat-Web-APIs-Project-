using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.Identity;

namespace Talabat.Repository.Identity
{
	public static class AppIdentityDataSeeding
	{
		public async static Task AddDataSeedAsync(UserManager<AppUser> _userManager)
		{
			if (_userManager.Users.Count() == 0)
			{
				AppUser user = new AppUser()
				{
					DisplayName = "ismail atef",
					Email = "esmaeldude@gmail.com",
					UserName = "ismail",
					PhoneNumber = "1234567890",

				};

				await _userManager.CreateAsync(user, "Pas$w0rd");
			}
		}
	}
}
