using System;
using System.Collections.Generic;
using System.Numerics;

namespace SymbolicSequenceTransformer
{
    public class RecursiveTransformer: ISequenceTransformer
    {
        // I analyzed results of several iterations and found repeating patterns.
        // Every pattern has a unique string representation and a state for state machine.
        // Define the state to string mapping
        private readonly Dictionary<byte, string> stateToString = new Dictionary<byte, string>
        {
            { 0, "Δ" },
            { 1, "ΔΘΘΔΞ" },
            { 2, "ΘΘ" },
            { 4, "ΘΞ" },
            { 5, "ΘΞΘΔΞ" },
            { 6, "ΘΔ" },
            { 7, "Ξ" }
        };

        // Every state is easily transformed to another state based on the rules.
        // Define the state transitions based on the provided rules
        private readonly Dictionary<byte, List<byte>> stateTransitions = new Dictionary<byte, List<byte>>
        {
            { 0, new List<byte> { 4 } },                     // Δ -> ΘΞ
            { 4, new List<byte> { 1 } },                     // ΘΞ -> ΔΘΘΔΞ
            { 1, new List<byte> { 4, 2, 5 } },               // ΔΘΘΔΞ -> ΘΞ + ΘΘ + ΘΞΘΔΞ
            { 2, new List<byte> { 2 } },                     // ΘΘ -> ΘΘ
            { 5, new List<byte> { 1, 6, 6, 7 } },            // ΘΞΘΔΞ -> ΔΘΘΔΞ + ΘΔ + ΘΔ + Ξ
            { 6, new List<byte> { 6 } },                     // ΘΔ -> ΘΔ
            { 7, new List<byte> { 6, 7 } }                   // Ξ -> ΘΔ + Ξ
        };

        // Memorization dictionary to store the results of recursive calls
        private readonly Dictionary<(byte, int), BigInteger> _memo = new Dictionary<(byte, int), BigInteger>();
        // Total number of iterations
        private int _totalIterations = 0;
        // Action to write the sequence
        private Action<string>? _writeToString;

        public void Preprocess(int iterations)
        {
            _totalIterations = iterations;
        }

        // Public method to write the sequence to the console
        public void GenerateFinalSequence(Action<string>? writeToString)
        {
            if (_totalIterations < 0)
                throw new ArgumentException("Iterations must be a positive integer.");

            _writeToString = writeToString;
            // Initial sequence
            List<byte> initialSequence = new List<byte> { 0 };

            // Write the sequence using recursion
            foreach (byte symbol in initialSequence)
            {
                WriteSequenceRecursive(symbol, _totalIterations);
            }
        }

        // New method to calculate the total number of "ΘΔ" using a recursive approach with memorization
        public BigInteger CalculatePatternCount()
        {
            if (_totalIterations < 0)
                throw new ArgumentException("Iterations must be a positive integer.");

            // Initial sequence
            List<byte> initialSequence = new List<byte> { 0 };

            // Calculate the total number of "ΘΔ" using recursion with memorization
            BigInteger totalThetaDelta = 0;
            foreach (byte symbol in initialSequence)
            {
                totalThetaDelta += CountThetaDeltaRecursive(symbol, _totalIterations);
            }

            return totalThetaDelta;
        }

        // Recursive method to write the sequence to the console
        private void WriteSequenceRecursive(byte symbol, int iterations)
        {
            if (iterations == 0)
            {
                _writeToString?.Invoke(stateToString[symbol]);
            }
            else
            {
                // Recursive case: apply state transitions and recurse
                if (stateTransitions.ContainsKey(symbol))
                {
                    foreach (byte nextSymbol in stateTransitions[symbol])
                    {
                        WriteSequenceRecursive(nextSymbol, iterations - 1);
                    }
                }
            }
        }

        // Recursive method to count the number of "ΘΔ" patterns with memorization
        private BigInteger CountThetaDeltaRecursive(byte symbol, int iterations)
        {
            if (iterations == 0)
            {
                // Base case: final iteration, count "ΘΔ" patterns
                return (symbol == 1 || symbol == 5 || symbol == 6 || (symbol == 2 && _totalIterations % 2 == 0)) ? 1 : 0;
            }
            else
            {
                // Check if the result is already memorized
                if (_memo.TryGetValue((symbol, iterations), out BigInteger memorizedResult))
                {
                    return memorizedResult;
                }

                // Recursive case: apply state transitions and recurse
                BigInteger count = 0;
                if (stateTransitions.ContainsKey(symbol))
                {
                    foreach (byte nextSymbol in stateTransitions[symbol])
                    {
                        count += CountThetaDeltaRecursive(nextSymbol, iterations - 1);
                    }
                }

                // Memorize the result before returning
                _memo[(symbol, iterations)] = count;
                return count;
            }
        }
    }
}