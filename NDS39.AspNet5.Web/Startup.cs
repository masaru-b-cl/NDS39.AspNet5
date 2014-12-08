using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;

using NDS39.AspNet5.App;

namespace NDS39.AspNet5.Web
{
	public class Startup
	{
		public void Configure(IApplicationBuilder app)
		{
			app.UseAuthBasic((user, password) => user == password);

			app.Run(async context =>
				{
					var response = context.Response;
					response.StatusCode = 200;
					response.ContentType = "text/plain";
					await response.WriteAsync(string.Format("Hello {0}", context.User.Identity.Name));
				});
		}
	}
}
