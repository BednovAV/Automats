using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automats.WithEpsilon
{
	public class Automat
	{
		public int AlphabetCount { get; private set; }

		public Dictionary<string, Dictionary<string, HashSet<string>>> Matrix { get; } = new();

		private HashSet<string> _endStates;

		public Automat(string configurationPath)
		{
			ReadConfig(configurationPath);
		}

		public bool GoTransform(InputModel inputData)
		{
			_endStates = new HashSet<string>();
			Transform(inputData.Input, 0, inputData.StartState);
			return _endStates.Contains(inputData.EndState);
		}

		public void Transform(int[] input, int inputIndex, string currState)
		{
			if (inputIndex == input.Length)
			{
				_endStates.Add(currState);
			}

			int currentInput;
			if (inputIndex != input.Length)
			{
				currentInput = input[inputIndex++];
			}
			else
			{
				currentInput = -99;
			}

			if (currentInput >= AlphabetCount)
				throw new ArgumentException("Wrong value in inputData sequence");
			if (!Matrix.ContainsKey(currState))
				return;

			if (currentInput != -99)
			{
				foreach (var i in Matrix[currState][currentInput.ToString()])
				{
					Transform(input, inputIndex, i);
				}
			}

			foreach (var i in Matrix[currState]["e"])
			{
				Transform(input, inputIndex, i);
			}

		}

		private void ReadConfig(string configurationPath)
		{
			using (var reader = new StreamReader(configurationPath))
			{
				while (!reader.EndOfStream)
				{
					ParseLine(reader.ReadLine());
				}
			}

			AlphabetCount = Matrix.FirstOrDefault().Value.Count;
			if (Matrix.Values.Any(v => v.Count != AlphabetCount))
			{
				throw new ArgumentException("Parse error!");
			}
		}

		private void ParseLine(string line)
		{
			var columns = line.Split(' ');

			var state = columns.First();

			Matrix.TryAdd(state, new Dictionary<string, HashSet<string>>());

			for (int i = 1; i < columns.Length - 1; i++)
			{
				var transitions = new HashSet<string>(columns[i].Split('/'));
				Matrix[state].Add((i - 1).ToString(), transitions);
			}
			var epsilonColumn = columns.Last();
			Matrix[state].Add("e", new HashSet<string>(epsilonColumn.Split('/')));
		}
	}
}
