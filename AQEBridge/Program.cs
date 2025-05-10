namespace AQEBridge
{
    internal class Program
    {

       

        static void Main(string[] args)
        {
            Client c;

            Console.WriteLine("Hello, World!");
            c = new(54321);
            c.Start();
            Console.ReadKey();
        }
    }
}
