using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;

namespace NDS39.AspNet5.Web
{
	public class Startup
	{
		public void Configure(IApplicationBuilder app)
		{
			app.Run(async context =>
				{
					var response = context.Response;
					response.StatusCode = 200;
					response.ContentType = "text/plain";
					await response.WriteAsync("Hello world");
				});
		}
	}
}
