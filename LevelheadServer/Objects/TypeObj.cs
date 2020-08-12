using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LevelheadServer.Objects
{
	public class TypeObj
	{
		public TypeObj() { }
		public TypeObj(string name, int min, int range, string field)
		{
			this.name = name;
			this.min = min;
			this.range = range;
			this.field = field;
		}
		public string name { get; set; }
		public int min { get; set; }
		public int range { get; set; }
		public string field { get; set; }
	}
}
