I implemented two classes.

First class is SimpleTransformer.
It's a simple transformer that uses logic from the task and iterates in nested loops to find the result.
It's not the most efficient way to solve the task, but it's the simplest one. It's used to produce resuts,
that are used to test the second class.

Second class is RecursiveTransformer.
To implement it, first I generated sequences for several iterations using SimpleTransformer.
Then I tried to find patterns in the sequences. I found that logic can be transformed into state machine.
Every state represents sequence of symbols.
Every state can be transformed into another state by applying simple translation.
So we don't need to analyze any previous or next symbols.
I created a recursive function that generates the sequence for the given iteration.
Sequence is splitted into chunks for ever state. And this chunk can be passed for further processing using provided delegate.
In my example, I wrote text to Console. But it can be also written into file or any other output.
Similar logic with recursion is used to calculate the number of ΘΔ.
For further improvement, I used memorization technique to store already calculated results.
If the result is already calculated, it's returned from the memory. If it's not, it's calculated and stored in the memory.

RecursiveTransformer can work with the iteration up to 3000.
I crated Test project. It tests both classes with the same input and compares the results.
The number of iterations are from -1 to 35. I don't use more than 35, because of the SimpleTransformer's limit.

In the Main method of the project, I wrote logic to show results for both classes.
I commented code for SimpleTransformer, but you can uncomment it to see the results.
Also I commented code to show the sequence for RecursiveTransformer, for big iterations it takes a lot of space.
You can uncomment it to see the sequence.

Rigth now only the number of ΘΔ is shown for the RecursiveTransformer.