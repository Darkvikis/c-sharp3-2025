namespace FizzBuzz;

public class FizzBuzz
{

    public static void OnCount(int lastNumber)
    {
        for (var i = 1; i <= lastNumber; i++)
        {
            var text = string.Empty;

            if (i % 3 == 0)
            {
                text += "Fizz";
            }

            if (i % 5 == 0)
            {
                text += "Buzz";
            }

            if (string.IsNullOrEmpty(text))
            {
                text = i.ToString();
            }

            Console.WriteLine(text);
        }
    }
}
