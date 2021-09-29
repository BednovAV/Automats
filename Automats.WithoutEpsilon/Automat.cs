using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automats.WithoutEpsilon
{
	public class Automat
	{
		public int AlphabetCount { get; private set; }

		public Dictionary<string, List<HashSet<string>>> Matrix { get; } = new();

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
			else
			{
				var currentInput = input[inputIndex++];

				if (currentInput >= AlphabetCount)
					throw new ArgumentException("Wrong value in inputData sequence");
				if (!Matrix.ContainsKey(currState))
					return;

				foreach (var i in Matrix[currState][currentInput])
				{
					Transform(input, inputIndex, i);
				}
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

			Matrix.TryAdd(state, new List<HashSet<string>>());

			foreach (var col in columns.Skip(1))
			{
				var transitions = new HashSet<string>(col.Split('/'));
				Matrix[state].Add(transitions);
			}
		}
	}
}
