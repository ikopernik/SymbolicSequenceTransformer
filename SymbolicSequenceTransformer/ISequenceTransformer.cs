using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicSequenceTransformer
{
    // Common interface for sequence transformers
    public interface ISequenceTransformer
    {
        // Preprocess method should be called before any other method
        void Preprocess(int iterations);
        // Generate the final sequence based on the preprocessed data and write it to the provided action
        void GenerateFinalSequence(Action<string>? writeToString);
        // Calculate the count of a specific pattern "ΘΔ" in the final sequence
        BigInteger CalculatePatternCount();

    }
}
