using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LevelheadServer.Objects
{
	public class LevelheadData
	{
		public Dictionary<string, object> extra_displays { get; set; } = new Dictionary<string, object>();
		public Dictionary<string, object> stats { get; set; } = new Dictionary<string, object>();
	}
}
