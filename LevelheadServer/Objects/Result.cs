using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LevelheadServer.Objects
{
	public class Result
	{
		public bool success { get; set; }
	}
	public class ErrorResult : Result
	{
		public ErrorResult() { }
		public ErrorResult(string cause)
		{
			this.cause = cause;
			this.success = false;
		}
		public string cause { get; set; }
	}
	public class LevelResult : Result
	{
		public string level { get; set; }
		public string strlevel { get; set; }
		public int generated { get; set; } = 0;
	}
}
