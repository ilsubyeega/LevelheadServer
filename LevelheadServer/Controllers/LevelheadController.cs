using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LevelheadServer.Objects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LevelheadServer.Controllers
{
	[ApiController]
	public class LevelheadController : ControllerBase
	{
		[HttpGet("/getConfig")]
		public async Task<IActionResult> GetConfig()
		{
			return Ok(JsonConvert.SerializeObject(LevelHead.objlist));
		}
		[HttpGet("/getData")]
		public async Task<IActionResult> GetData()
		{
			return Ok(new LevelheadData());
		}
		[HttpGet("/getPurchases/{uuid}")]
		public async Task<IActionResult> GetPurchaseData(string uuid)
		{
			return Ok(new PurchaseData());
		}
		[HttpGet("/user/{uuid}/{type}")]
		public async Task<IActionResult> GetUser(string uuid, string type)
		{
			if (!LevelHead.objlist.ContainsKey(type))
			{
				ErrorResult rs = new ErrorResult();
				rs.success = false;
				rs.cause = $"Type {type} not exists";
				return StatusCode(500, rs);
			}

			HypixelAPI api = new HypixelAPI();
			await api.Fetch(uuid);
			if (api.isErr)
			{
				ErrorResult rs = new ErrorResult();
				rs.success = false;
				rs.cause = api.ErrMsg;
				return StatusCode(500, rs);
			}
			LevelHead.objlist.TryGetValue(type, out TypeObj value);
			if (value == null)
				return StatusCode(500, new ErrorResult($"Type {type} wasnt found on field."));
			string k = api.getValue(type, value);
			LevelResult ls = new LevelResult();
			ls.level = k;
			ls.strlevel = k;
			ls.success = true;
			Console.WriteLine($"/user/{uuid}/{type}");
			return StatusCode(200, ls);
		}
		
	}
}
