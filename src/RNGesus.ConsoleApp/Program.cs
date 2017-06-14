using System;
using System.Text;

namespace RNGesus.ConsoleApp
{
    class Program
    {
        static void Main()
        {
            using (var rngesus = new Luke.RNG.RNGesus())
            {
                Console.WriteLine("Start\n");

                Console.WriteLine("GenerateBool(): " + rngesus.GenerateBool() + "\n");

                Console.WriteLine("GenerateInt(): " + rngesus.GenerateInt());
                Console.WriteLine("GenerateInt() <= 999: " + rngesus.GenerateInt(999));
                Console.WriteLine("GenerateInt() >= 9999 and <= 999: " + rngesus.GenerateInt(999, 9999) + "\n");

                Console.WriteLine("GenerateLong(): " + rngesus.GenerateLong());
                Console.WriteLine("GenerateLong() <= 999999999: " + rngesus.GenerateLong(999999999));
                Console.WriteLine("GenerateLong() >= 999999999 and <= 999999999999: " + rngesus.GenerateLong(999999999, 999999999999) + "\n");

                Console.WriteLine("GenerateFloat(): " + rngesus.GenerateFloat() + "\n");

                Console.WriteLine("GenerateDouble(): " + rngesus.GenerateDouble() + "\n");

                Console.WriteLine("GenerateString() == 99 characters: " + rngesus.GenerateString(99));
                Console.WriteLine("GenerateString() == 99 characters, abc: " + rngesus.GenerateString(99, "abc") + "\n");

                Console.WriteLine("GenerateByteArray() == 99 characters (ASCII): " + Encoding.ASCII.GetString(rngesus.GenerateByteArray(99)) + "\n");

                Console.WriteLine("End");
            }

            Console.Read();
        }
    }
}
