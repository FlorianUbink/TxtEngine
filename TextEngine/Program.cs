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

            gameHandle.TEST();

            gameHandle.Update();
            Console.ReadLine();

        }
    }
}
