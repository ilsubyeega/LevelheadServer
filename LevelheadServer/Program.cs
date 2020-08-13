using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LevelheadServer
{
	public class Program
	{
		public static void Main(string[] args)
		{
			string value = Environment.GetEnvironmentVariable("HYPIXEL_API_KEY");
			if (value == null)
			{
				Console.WriteLine("HYPIXEL_API_KEY not found in environment variable.");
				return;
			}
			LevelHead.API_KEY = value;
			LevelHead.setObj();
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
