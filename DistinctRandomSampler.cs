#nullable enable

namespace Extensions
{
	using System;
	using System.Runtime.InteropServices;

	/// <summary>
	/// Provides random selection of elements from a collection without repeating the last N elements.
	/// <br>Does not store any additional collection, and has O(1) complexity for sampling.</br>
	/// </summary>
	/// <typeparam name="T">Type of the elements in the collection.</typeparam>
	public class DistinctRandomSampler<T>
	{
		private readonly T?[] m_Values;
		private readonly int m_SkipThreshold;
		private readonly Random m_Random;
		private int m_ThresholdIndex;
		private int m_SkipIndex;

		// Exposed for testing
		public int SkipThreshold
		{
			get => m_SkipThreshold;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DistinctRandomSampler{T}"/> class.
		/// </summary>
		/// <param name="values">Array of elements to sample from.</param>
		/// <param name="skipThreshold">Number of elements to avoid repeating.</param>
		/// <param name="random">Optional random number generator instance.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public DistinctRandomSampler(T?[] values, int skipThreshold, [Optional] Random? random)
		{
			if (values == null)
			{
				throw new ArgumentNullException(nameof(values));
			}

			// Ensure skip threshold is between 0 and size - 1
			if (skipThreshold < 0)
			{
				skipThreshold = 0;
			}
			else if (skipThreshold >= values.Length)
			{
				skipThreshold = Math.Max(values.Length - 1, 0);
			}

			m_Values = values;
			m_SkipThreshold = skipThreshold;
			m_ThresholdIndex = 0;
			m_SkipIndex = 0;
#if NET6_0_OR_GREATER
			m_Random ??= Random.Shared;
#else
			m_Random ??= new();
#endif
		}

		/// <summary>
		/// Returns he next random element from the collection, ensuring it is not one of the last N elements.
		/// </summary>
		/// <returns>The next random element.</returns>
		public T? Next()
		{
			// Since the initial value of threshold index is 0, all elements have an equal chance of being selected.
			var index = m_Random.Next(m_ThresholdIndex, m_Values.Length);
			var ret = m_Values[index];

			m_Values[index] = m_Values[m_SkipIndex];
			m_Values[m_SkipIndex] = ret;

			// Update the threshold index until it reaches the skip threshold
			if (m_ThresholdIndex < m_SkipThreshold)
			{
				++m_ThresholdIndex;
			}

			// Increment and reset the skip index as necessary
			if (++m_SkipIndex >= m_SkipThreshold)
			{
				m_SkipIndex = 0;
			}
			return ret;
		}
	}
}