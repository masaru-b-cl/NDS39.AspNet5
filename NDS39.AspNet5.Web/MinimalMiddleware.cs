using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using System.Threading.Tasks;
using System.Diagnostics;

namespace NDS39.AspNet5.Web
{
	public class MinimalMiddleware
	{
		private readonly RequestDelegate next;

		public MinimalMiddleware(RequestDelegate next)
		{
			this.next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			Debug.WriteLine("@@@ start middleware @@@");
			await next(context);
			Debug.WriteLine("@@@ end middleware @@@");
		}
	}
}