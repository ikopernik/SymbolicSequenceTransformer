using System;
using System.Collections.Generic;
using System.Numerics;

namespace SymbolicSequenceTransformer
{
    // Simple transformer is created using a dictionary of transformation rules.
    // Its code very clear and main goal is to test the RecursiveTransformer
    // results for small number of iterations.
    public class SimpleTransformer: ISequenceTransformer
    {
        // Define the transformation rules
        private readonly Dictionary<char, Func<List<char>, int, List<char>>> _transformationRules;
        // Final sequence
        private string _finalSequence = "";
        // Count of "ΘΔ" pattern in the final sequence
        private BigInteger _patternCount = 0;
        
        public SimpleTransformer()
        {
            // Initialize transformation rules
            _transformationRules = new Dictionary<char, Func<List<char>, int, List<char>>>
            {
                { 'Δ', (sequence, index) => ApplyRule1(sequence, index) },
                { 'Θ', (sequence, index) => ApplyRule2(sequence, index) },
                { 'Ξ', (sequence, index) => ApplyRule3(sequence, index) }
            };
        }

        // Generate sequence for N iterations
        public void Preprocess(int iterations)
        {
            if (iterations < 0)
                throw new ArgumentException("Iterations must be a positive integer.");

            List<char> sequence = new List<char> { 'Δ' };

            for (int i = 0; i < iterations; i++)
            {
                List<char> newSequence = new List<char>();
                for (int j = 0; j < sequence.Count; j++)
                {
                    char currentSymbol = sequence[j];
                    if (_transformationRules.ContainsKey(currentSymbol))
                    {
                        List<char> transformed = _transformationRules[currentSymbol](sequence, j);
                        newSequence.AddRange(transformed);
                    }
                }
                sequence = newSequence;
            }

            // Convert the final sequence to a string
            string finalSequence = new string(sequence.ToArray());

            // Count "ΘΔ" in the final sequence
            int patternCount = CountPattern(finalSequence, "ΘΔ");

            _finalSequence = finalSequence;
            _patternCount = patternCount;
        }

        public void GenerateFinalSequence(Action<string>? writeToString)
        {
            writeToString?.Invoke(_finalSequence);
        }

        public BigInteger CalculatePatternCount()
        {
            return _patternCount;
        }

        // Transformation Rule 1
        private List<char> ApplyRule1(List<char> sequence, int index)
        {
            int thetaCount = 0;
            for (int i = index - 1; i >= 0 && sequence[i] == 'Θ'; i--)
            {
                thetaCount++;
            }
            return (thetaCount % 2 == 0) ? new List<char> { 'Θ', 'Ξ' } : new List<char> { 'Δ' };
        }

        // Transformation Rule 2
        private List<char> ApplyRule2(List<char> sequence, int index)
        {
            if (index < sequence.Count - 1 && sequence[index + 1] == 'Ξ')
            {
                return new List<char> { 'Δ', 'Θ' };
            }
            return new List<char> { 'Θ' };
        }

        // Transformation Rule 3
        private List<char> ApplyRule3(List<char> sequence, int index)
        {
            if ((index == 0 || sequence[index - 1] != 'Ξ') &&
                (index == sequence.Count - 1 || sequence[index + 1] != 'Ξ'))
            {
                return new List<char> { 'Θ', 'Δ', 'Ξ' };
            }
            return new List<char> { 'Ξ' };
        }

        // Helper to count a pattern in a string
        private int CountPattern(string input, string pattern)
        {
            int count = 0;
            for (int i = 0; i <= input.Length - pattern.Length; i++)
            {
                if (input.Substring(i, pattern.Length) == pattern)
                {
                    count++;
                }
            }
            return count;
        }
    }
}