using LevelheadServer.Calculator;
using LevelheadServer.Objects;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Runtime.Caching;
using System.Net.Http;

namespace LevelheadServer
{
	public class HypixelAPI : IDisposable
	{
		public static Regex regex = new Regex("^[a-zA-Z0-9 ]*$");

		private string BASE_URL = "https://api.hypixel.net";
		public JObject Value = null;
		public bool isErr = false;
		public string ErrMsg = null;
		private bool disposedValue;

		public async Task Fetch(string uuid)
		{
			
			ObjectCache cache = MemoryCache.Default;
			JObject content = cache[uuid] as JObject;
			if (content != null)
			{
				Value = content;
				Value.TryGetValue("player", out JToken token);
				if (!token.HasValues)
				{
					Console.WriteLine($"Player {uuid} not found.");
					isErr = true;
					ErrMsg = "Player not found";
					cache.Remove(uuid);
				} else
				{
					return;
				}
				
			}
			Task.Delay(50);
			if (LevelHead.FROZEN == true)
			{
				ErrMsg = "Hypixel API Server Rate Limiteed";
				isErr = true;
				Value = null;
				return;
			}
				
			using (HttpClient client = new HttpClient())
			{
				try
				{
					uuid = uuid.Replace("-", "");
					if (!regex.IsMatch(uuid))
					{
						isErr = true;
						ErrMsg = "UUID not match the regex.";
						return;
					}
					
					while (true)
					{
						HttpResponseMessage response = await client.GetAsync($"{BASE_URL}/player?key={LevelHead.API_KEY}&uuid={uuid}");
						if (response.Headers.Contains("retry-after")){
							string wait = response.Headers.GetValues("retry-after").First();
							LevelHead.FROZEN = true;
							Console.WriteLine("API FROZEN! TIME: " + wait);
							await Task.Delay(int.Parse(wait) * 1000);
							Console.WriteLine("API FROZEN FINISHED");
							LevelHead.FROZEN = false;
						} else
						{
							Value = JObject.Parse(await response.Content.ReadAsStringAsync());
							break;
						}
						
					}
					
					
					Value.TryGetValue("player", out JToken token);
					if (!token.HasValues)
					{
						Console.WriteLine($"Player {uuid} not found.");
						isErr = true;
						ErrMsg = "Player not found";
					}
					cache[uuid] = Value;
				} catch (Exception ex)
				{
					Console.WriteLine("An Error occoured: " + ex.Message);
					ErrMsg = ex.Message;
					isErr = true;
					Value = null;
				}
			}
		}

		public string getValue(string name, TypeObj typeObj)
		{
			if (typeObj == null || typeObj.field == null || typeObj.field == "" || typeObj.field.Length == 0)
			{
				return getValueByName(name);
			}
			JToken t = Value.SelectToken(typeObj.field);
			if (t == null)
				return null;
			return t.ToString();
		}
		private string getValueByKey(string key)
		{
			JToken t = Value.SelectToken(key);
			return t?.ToString();
		}
		public string getValueByName(string name)
		{
			switch (name)
			{
				case "LEVEL":
					string a = getValueByKey("player.networkExp");
					if (a == null)
						return "0";
					return LevelCalculator.GetExactStringLevel(
						double.Parse(
							a
							)
						);
				default:
					if (name.Contains("+"))
					{
						string[] namelist = name.Split("+");
						if (namelist.Length < 2)
							return null;
						StringBuilder builder = new StringBuilder();
						string[] colorlist = new string[6] { "6", "a", "c", "d", "e", "b" };
						for (int i = 0; i < namelist.Length; i++)
						{
							if (LevelHead.objlist.ContainsKey(namelist[i]))
							{
								LevelHead.objlist.TryGetValue(namelist[i], out TypeObj value);
								string b = getValue(namelist[i], value);
								if (b != null)
								{
									builder.Append(" &!");
									builder.Append(colorlist[i % 6]);
									builder.Append(b);
								} else
								{
									Console.WriteLine($"Warning: getValue({namelist[i]}) not found");
								}
							}
						}
						return builder.ToString();
					} else
					{
						return null;
					}
			}
			return null;
		}
		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: 관리형 상태(관리형 개체)를 삭제합니다.
				}

				// TODO: 비관리형 리소스(비관리형 개체)를 해제하고 종료자를 재정의합니다.
				// TODO: 큰 필드를 null로 설정합니다.
				disposedValue = true;
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
