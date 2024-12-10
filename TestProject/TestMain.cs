using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Numerics;
using System.Text;

namespace SymbolicSequenceTransformer.Tests
{
    [TestClass]
    public class TransformerTests
    {
        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]
        [DataRow(6)]
        [DataRow(7)]
        [DataRow(8)]
        [DataRow(9)]
        [DataRow(10)]
        [DataRow(11)]
        [DataRow(12)]
        [DataRow(13)]
        [DataRow(14)]
        [DataRow(15)]
        [DataRow(16)]
        [DataRow(17)]
        [DataRow(18)]
        [DataRow(19)]
        [DataRow(20)]
        [DataRow(21)]
        [DataRow(22)]
        [DataRow(23)]
        [DataRow(24)]
        [DataRow(25)]
        [DataRow(26)]
        [DataRow(27)]
        [DataRow(28)]
        [DataRow(29)]
        [DataRow(30)]
        [DataRow(31)]
        [DataRow(32)]
        [DataRow(33)]
        [DataRow(34)]
        [DataRow(35)]
        public void TestTransformers(int iterations)
        {
            var recursiveTransformer = new RecursiveTransformer();
            var simpleTransformer = new SimpleTransformer();

            var (recursivePatternCount, recursiveSequence) = GetTransformerResults(recursiveTransformer, iterations);
            var (simplePatternCount, simpleSequence) = GetTransformerResults(simpleTransformer, iterations);

            Assert.AreEqual(simplePatternCount, recursivePatternCount, $"Pattern count mismatch for iterations = {iterations}");
            Assert.AreEqual(simpleSequence, recursiveSequence, $"Sequence mismatch for iterations = {iterations}");
        }   

        (BigInteger, string) GetTransformerResults(ISequenceTransformer transformer, int iterations)
        {
            BigInteger patternCount = 0;
            string sequence = string.Empty;
            BigInteger recursivePatternCount = 0;
            try
            {
                transformer.Preprocess(iterations);
                recursivePatternCount = transformer.CalculatePatternCount();
                var sb = new StringBuilder();
                transformer.GenerateFinalSequence(x => sb.Append(x));
                sequence = sb.ToString();
            }
            catch (Exception ex)
            {
                if (iterations < 0)
                {
                    Assert.AreEqual("Iterations must be a positive integer.", ex.Message);
                }
                else
                {
                    Assert.Fail($"Unexpected exception: {ex.Message}");
                }
            }
            return (patternCount, sequence);
        }
    }
}
