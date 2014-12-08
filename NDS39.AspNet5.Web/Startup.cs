using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using System.Diagnostics;

namespace NDS39.AspNet5.Web
{
	public class Startup
	{
		public void Configure(IApplicationBuilder app)
		{
			app.Use(async (context, next) =>
				{
					Debug.WriteLine("@@@ start middleware @@@");
					await next();
					Debug.WriteLine("@@@ end middleware @@@");
				});

			app.Run(async context =>
				{
					Debug.WriteLine("@@@ run core app @@@");

					var response = context.Response;
					response.StatusCode = 200;
					response.ContentType = "text/plain";
					await response.WriteAsync("Hello world");
				});
		}
	}
}
