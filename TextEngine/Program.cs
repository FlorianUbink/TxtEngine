using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine
{
    class Program
    {


        static void Main(string[] args)
        {

            GameManager gameHandle = new GameManager();
            StreamProcessor procesHandle = new StreamProcessor(gameHandle);


            procesHandle.LoadFile("TestFile.txt");
            procesHandle.GetBlock("420");

            procesHandle.ProcessNext();

            procesHandle.ProcessNext();
            Console.ReadLine();




        }
    }
}
