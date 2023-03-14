namespace CODEInterpreter.Classes.ErrorHandling
{
    public static class ErrorHandler
    {
        public static object? ThrowError(int line, string message)
        {
            Console.WriteLine($"Error: Line {line}.");
            Console.WriteLine("Details: " + message);
            Environment.Exit(400);

            return null;
        }
    }
}
