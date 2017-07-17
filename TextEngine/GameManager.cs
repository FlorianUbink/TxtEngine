using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Reflection;

namespace TextEngine
{

    // Enumeration of all possible commands
    public enum Command
    {
        Print,
        Choice,
        Roll,
        Link,
        Empty
    }

    public class GameManager
    {
        #region ClassInstances
        StreamProcessor streamProcessor;
        Library library;
        Player player;
        #endregion

        Random dice;
        int DiceRoll = -1;
        int maxDice = 0;
        Timer holdTimer;

        #region properties
        public bool Active = true;
        bool BlockActive = true;

        bool execute = false;
        #endregion




        public GameManager()
        {
            #region InitiateClasses
            streamProcessor = new StreamProcessor(this);
            library = new Library();
            #endregion

            dice = new Random();
            holdTimer = new Timer();
            player = new Player();
        }

        public void Update()
        {
            while (execute)
            {
                ExecuteCommand();
            }

        }

        public void TEST()
        {
            LinkTo(500);
            execute = true;
            player.Name = "Florian";
        }




        // called by StreamProcessor when it processed a new commandblock

        public void ExecuteCommand()
        {
            List<string> commandBlock = streamProcessor.NextCommand();

            string commandLine = commandBlock[0];

            switch (commandLine)
            {
                case "#PRINT":
                    commandBlock.Remove(commandLine);
                    foreach (string line in commandBlock)
                    {
                        bool a = interTextCommands(line);
                        if (!a)
                        {
                            Console.WriteLine(line);
                        }
                        
                    }
                    break;

                case "#CHOICE":
                    commandBlock.Remove(commandLine);

                    int c = 0;

                    // Reads inputline from player, if input is not valid then 0 else 1
                    do
                    {
                        string condition = Console.ReadLine();
                        c = ExecuteBranch(commandBlock, condition);

                    } while (c != 1);

                    break;
                case "#ROLL":
                    // roll dice
                    string cond2 = "" + dice.Next(0, 2);
                    // execute branch
                    ExecuteBranch(commandBlock, cond2);
                    break;

                case "#BRANCH":
                    commandBlock.Remove(commandLine);
                    AdvBranch(commandBlock, dice);
                    break;

                default:
                    break;
            }
        }


        private int ExecuteBranch(List<string> branchData, string input)
        {

            #region LoadBranch
            Dictionary<string, string> BranchDic = new Dictionary<string, string>();

            for (int i = 0; i < branchData.Count - 1;)
            {
                BranchDic.Add(branchData[i], branchData[i + 1]);
                i += 2;
            }
            #endregion

            #region FindMatch
            if (BranchDic.ContainsKey(input))
            {
                // Finds Match
                interTextCommands(BranchDic[input]);
                return 1;
            }
            else
            {
                // no match: not correct input
                Console.WriteLine("ERROR: INPUT DOESNT MATCH!");
                return 0;
            }
            #endregion
        }

        private bool interTextCommands(string line)
        {

            if (line.Contains('|'))
            {
                string subline = line.Substring(0, line.IndexOf("|"));
                switch (subline)
                {
                    case "CLEAR":
                        Console.Clear();
                        break;

                    case "LINK":
                        int key = int.Parse(line.Substring(subline.Length + 1));
                        LinkTo(key);
                        break;
                    case "BREAK":
                        string fullType = line.Substring(subline.Length + 1);
                        string type = fullType.Substring(0, 5);

                        switch (type)
                        {
                            case "INPUT":
                                Console.ReadLine();
                                break;
                            case "TIMER":
                                // wait
                                break;
                            default:
                                break;
                        }
                        break;

                    case "PLAYER":
                        string fullCommand = line.Substring(subline.Length + 1);
                        string getorset = fullCommand.Remove(4);
                        getorset = getorset.Trim();


                        switch (getorset)
                        {
                            case "get":
                                string property = fullCommand.Substring(4);
                                property = property.Trim();
                                PropertyInfo info = player.GetType().GetProperty(property);
                                string a = (string)info.GetValue(player);
                                Console.Write(a);
                                break;

                            case "set":
                                string subProp = fullCommand.Substring(4);
                                property = subProp.Substring(0, subProp.IndexOf('='));
                                property = property.Trim();
                                string value = subProp.Substring(subProp.IndexOf('=') + 1);
                                value = value.Trim();

                                info = player.GetType().GetProperty(property);
                                info.SetValue(player, value);
                                break;

                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
                return true;
            }

            else
            {
                return false;
            }
        }


        private void AdvBranch(List<string> cBlock, Random dice)
        {
            Dictionary<string, string> choices = new Dictionary<string, string>();
            List<string> keyRoll = new List<string>();
            string inputKey = "";
            bool input = false;
            bool sucessfull = false;

            foreach (string line in cBlock)
            {
                if (line.Contains("DICE"))
                {
                    string subline = line.Substring(6);
                    subline.Trim();
                    maxDice = int.Parse(subline);
                }

                else if (line.Contains("CHOICE"))
                {
                    int index = cBlock.IndexOf(line) + 1;

                    for (int i = index; i < cBlock.Count - 1;)
                    {
                        choices.Add(cBlock[i], cBlock[i + 1]);
                        i += 2;
                    }

                    DiceRoll = dice.Next(0, maxDice);

                    foreach (string rawcondition in choices.Keys)
                    {
                        if (rawcondition.Contains("DICE|"))
                        {
                            string RollCondition = rawcondition.Substring(rawcondition.IndexOf("DICE|") + 5);
                            RollCondition.Trim();
                            int cIn = RollCondition.IndexOfAny(new Char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0'});
                            int oIn = RollCondition.IndexOfAny(new Char[] { '>', '<', '=', '!' });
                            string Oper = RollCondition.Substring(oIn, (cIn - oIn));
                            int cond = int.Parse(RollCondition.Substring(cIn));

                            bool result = DiceOperation(DiceRoll, cond, Oper);

                            if (result)
                            {
                                keyRoll.Add(rawcondition);
                            }
                        }
                    }

                    do
                    {
                        string rawInput = Console.ReadLine();

                        foreach (string rawCondition in choices.Keys)
                        {
                            if (rawCondition.Contains("INPUT|"))
                            {
                                string condition = rawCondition.Substring(rawCondition.IndexOf("INPUT|") + 6);
                                if (condition.Contains(','))
                                {
                                    condition = condition.Substring(0, condition.IndexOf(','));
                                    condition.Trim();
                                }

                                if (condition == rawInput)
                                {
                                    input = true;
                                    inputKey = rawCondition;
                                    break;
                                }
                            }
                        }

                        if (!input)
                        {
                            Console.WriteLine("ERROR INPUT IS FALSE");
                        }


                    } while (!input);

                    if (keyRoll.Count > 0)
                    {
                        foreach (string rollKey in keyRoll)
                        {
                            if (rollKey == inputKey)
                            {
                                sucessfull = true;
                                break;
                            }

                        }
                    }
                    if (sucessfull)
                    {
                        string[] operations = choices[inputKey].Split(',');

                        foreach (string operation in operations)
                        {
                            string c = operation.Trim();
                            interTextCommands(c);
                        }


                    }
                    break;
                }
            }


        }


        private bool DiceOperation(int roll, int condition, string boolOp)
        {
            switch (boolOp)
            {
                case ">": return roll > condition;
                case ">=": return roll >= condition;
                case "<": return roll < condition;
                case "<=": return roll <= condition;
                case "==": return roll == condition;
                case "!=": return roll != condition;

                default: throw new Exception("Invalid Logic Operation");
            }
        }




        private void LinkTo(int cellNum)
        {
            bool validFile = streamProcessor.GetCell(cellNum);

            if (!validFile)
            {
                string filePath = library.FindFile(cellNum);

                // throw error if there is no filepath
                if (filePath == "ERROR")
                {
                    throw new Exception("ERROR: StoryBlock not Found.");
                }

                else
                {
                    streamProcessor.LoadFile(filePath);
                    streamProcessor.GetCell(cellNum);
                }
            }
        }

    }
}
