using System;
using System.IO;
using Newtonsoft.Json;

namespace Automats.WithoutEpsilon
{
	class Program
	{
		private const int COLUMN_WIDTH = 8;
		private const string CONFIG_PATH = "conf.txt";
		private const string INPUT_PATH = "input.json";

		static void Main(string[] args)
		{
			var dfa = new Automat(CONFIG_PATH);
			var input = ReadInput();
			var result = dfa.GoTransform(input);
			Console.WriteLine($"Result: {result}");
		}

		private static InputModel ReadInput()
		{
			Console.WriteLine();
			var inputText = File.ReadAllText(INPUT_PATH);
			var input = JsonConvert.DeserializeObject<InputModel>(inputText);

			Console.WriteLine("Params:");
			Console.WriteLine("First state: " + input.StartState);
			Console.WriteLine("End state: " + input.EndState);
			Console.Write("Input sequence: " + string.Join(' ', input.Input));
			Console.WriteLine(Environment.NewLine);

			return input;
		}
	}
}
