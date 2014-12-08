using System;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;

namespace NDS39.AspNet5.App
{
	public delegate bool AuthenticateDelegate(string user, string password);

	public static class AuthBasicExtensions
	{
		public static void UseAuthBasic(this IApplicationBuilder app, AuthenticateDelegate authenticate)
		{
			app.UseMiddleware<AuthBasicMiddleware>(authenticate);
		}
	}

	public class AuthBasicMiddleware
	{
		private readonly RequestDelegate next;
		private readonly AuthenticateDelegate authenticate;

		public AuthBasicMiddleware(RequestDelegate next)
		{
			this.next = next;
		}

		public AuthBasicMiddleware(RequestDelegate next, AuthenticateDelegate authenticate) : this(next)
		{
			this.authenticate = authenticate;
		}

		public async Task Invoke(HttpContext context)
		{
			var request = context.Request;

			var authorization = request.Headers["Authorization"];

			if (string.IsNullOrEmpty(authorization))
			{
				Unauthorized(context);
				return;
			}

			if (authorization.StartsWith("Basic") == false)
			{
				Unauthorized(context);
				return;
			}

			var base64Parameter = authorization.Replace("Basic ", "");
			var parameter = Encoding.UTF8.GetString(Convert.FromBase64String(base64Parameter));
			var groups = new Regex("(?<user>.*):(?<password>.*)").Match(parameter).Groups;
			var user = groups["user"].Value;
			var password = groups["password"].Value;

			if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
			{
				Unauthorized(context);
				return;
			}

			if (authenticate(user, password) == false)
			{
				Unauthorized(context);
				return;
			}

			context.User = new ClaimsPrincipal(
				new ClaimsIdentity(
					new[] { new Claim(ClaimTypes.Name, user) },
					"Basic"
				)
			);

			await next(context);
		}

		private static void Unauthorized(HttpContext context)
		{
			var response = context.Response;
			response.StatusCode = 401;
			response.Headers["WWW-Authenticate"] = @"Basic realm=""NDS39""";
		}
	}
}
