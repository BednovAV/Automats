using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automats.WithoutEpsilon
{
	public class InputModel
	{
		public string StartState { get; set; }
		public string EndState { get; set; }
		public int[] Input { get; set; }
	}
}
