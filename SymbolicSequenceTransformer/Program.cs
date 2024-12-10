using SymbolicSequenceTransformer;

try
{
    Console.WriteLine("Enter the number of iterations (N):");
    if (!int.TryParse(Console.ReadLine(), out int iterations) || iterations < 0)
    {
        Console.WriteLine("Invalid input. Please enter a positive integer.");
        return;
    }

    // Uncomment next line if you want to see results from simple transformer.
    // Don't use it for number of iterations over 40. It uses very inefficient approach.

    //TestTransformer("Use simple transformer", iterations, new SimpleTransformer());

    // On my computer, the recursive transformer can handle up to 3000 iterations.
    TestTransformer("Use recursive transformer", iterations, new RecursiveTransformer());

    Console.ReadLine();
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    Console.ReadLine();
}

void TestTransformer(string title, int iterations, ISequenceTransformer transformer)
{
    Console.WriteLine(title);
    transformer.Preprocess(iterations);
    Console.WriteLine($"Pattern 'ΘΔ' Count: {transformer.CalculatePatternCount()}");

    // Uncomment the next two lines if you want to see only see the output sequence.
    // Console.Write("Sequence: ");
    // transformer.GenerateFinalSequence(x => Console.Write(x));
    Console.WriteLine();
}
