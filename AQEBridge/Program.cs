namespace AQEBridge
{
    internal class Program
    {

       

        static void Main(string[] args)
        {
            Client c;
			UI ui = new();

			Console.WriteLine("Hello, World!");

            ui.Update();
			c = new(54321,ui);
            c.Start().Wait();
            Console.ReadKey();
        }
    }
}
