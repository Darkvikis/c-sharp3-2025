var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/test", () => "This is a test endpoint.");
app.MapGet("/fizzbuzz/{number:int}", (int number) =>
{
    if (number % 3 == 0 && number % 5 == 0)
        return "FizzBuzz";
    if (number % 3 == 0)
        return "Fizz";
    if (number % 5 == 0)
        return "Buzz";
    return number.ToString();
});

app.Run();

