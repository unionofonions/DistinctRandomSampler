# DistinctRandomSampler

DistinctRandomSampler is a highly efficient algorithm for selecting random elements from a collection without repeating the last N selected elements.
This implementation ensures O(1) time complexity for each sampling operation.


Features

•   O(1) Sampling Complexity: Each sampling operation is performed in constant time, ensuring minimal overhead.
•   Non-Repeating Selection: Guarantees that the last N selected elements are not repeated.
•   Easy Integration: Simple and intuitive API for seamless integration into your projects.
•   Memory Efficient: Uses minimal additional memory beyond the input collection.


Usage

var values = new string[] { "First", "Second", "Third", "Fourth", "Fifth" };
var skipThreshold = 3;
var sampler = new DistinctRandomSampler<string>(values, skipThreshold);

for (int i = 0; i < 10; i++)
{
    var selected = sampler.Next();
    Console.WriteLine(selected);
}


Contributing

Contributions are welcome! Feel free to open issues or submit pull requests.