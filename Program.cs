#nullable enable

namespace Example
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;

	public class Program
	{
		static private void Main()
		{
			var values = new string[] { "First", "Second", "Third", "Fourth", "Fifth", "Sixth", "Seventh", "Eighth", "Ninth", "Tenth" };
			var skipThreshold = 6;
			var sampler = new Extensions.DistinctRandomSampler<string>(values, skipThreshold);
			const int iterations = 10_000_000;


			// Test repetition
			Console.WriteLine("Repetition test started...");
			var selected = new Queue<string?>();
			for (var i = 0; i < iterations; ++i)
			{
				var curr = sampler.Next();
				if (selected.Contains(curr))
				{
					Console.WriteLine("TEST FAILED");
					return;
				}
				// The sampler might alter the skip threshold, so we use the property instead of the local variable
				if (selected.Count >= sampler.SkipThreshold)
				{
					selected.Dequeue();
				}
				selected.Enqueue(curr);
			}
			Console.WriteLine("TEST PASSED\n");


			// Speed test
			Console.WriteLine("Speed test started...");
			var stopwatch = Stopwatch.StartNew();
			for (var i = 0; i < iterations; ++i)
			{
				_ = sampler.Next();
			}
			stopwatch.Stop();
			Console.WriteLine($"{iterations:N0} iterations done in {stopwatch.ElapsedMilliseconds} ms.");


			// Print some samples
			bool print = true;
			if (print)
			{
				const int printCount = 20;
				Console.WriteLine($"\n\nFirst {printCount} samples:\n");
				for (var i = 0; i < printCount; ++i)
				{
					Console.WriteLine(sampler.Next());
				}
			}
		}
	}
}