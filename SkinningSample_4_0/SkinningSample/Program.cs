using System;

namespace SmellOfRevenge2011
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SmellOfRevenge2011Win game = new SmellOfRevenge2011Win())
            {
                game.Run();
            }
        }
    }
#endif
}

