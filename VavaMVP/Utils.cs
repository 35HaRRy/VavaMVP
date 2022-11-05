
namespace VavaMVP
{
    public static class Utils
    {
        public static void ThrowControlledError(string message)
        {
            Console.WriteLine(message);
            throw new Exception()
            {
                Source = "Controlled"
            };
        }
    }
}
