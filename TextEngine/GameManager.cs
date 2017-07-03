using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine
{

    // Enumeration of all possible commands
    public enum Command
    {
        Print,
        Choice,
        Roll,
        Empty
    }

    public class GameManager
    {
        // called by StreamProcessor when it processed a new commandblock
        public void ProcessCommand(Command command, List<string> commandData)
        {
            switch (command)
            {
                case Command.Print:
                    foreach (string line in commandData)
                    {
                        Console.WriteLine(line);
                    }
                    break;

                case Command.Choice:
                    int c = 0;

                    // Reads inputline from player, if input is not valid then 0 else 1
                    do
                    {
                        string condition = Console.ReadLine();
                        c = ExecuteBranch(commandData, condition);

                    } while (c != 1);

                    break;
                case Command.Roll:
                    // roll dice
                    // execute branch
                    break;
                default:
                    break;
            }
        }

        private int ExecuteBranch(List<string> branchData, string input)
        {
            int index = 0;
            bool match = false;

            while (!match)
            {
                // if the index is to high, input must be invalid
                if (index > branchData.Count - 1)
                {
                    Console.WriteLine("INPUT ERROR MATCH NOT FOUND: PLEASE REFRASE");
                    return 0;
                }

                // input is correct, end loop + result
                else if (input == branchData[index])
                {
                    index += 1;
                    match = true;
                    //choice made

                    Console.WriteLine(branchData[index]);
                }

                // input incorrect, adds 2 for next conditioncheck
                else
                {
                    index += 2;
                }

            }
            // input was vallid
            return 1;


        }
    }
}
