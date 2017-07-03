using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine
{
    public class StreamProcessor
    {
        GameManager gameHandle;
        StreamReader currentStream;
        List<string> currentBlock;
        int lineIndex;

        public StreamProcessor(GameManager gameHandler)
        {
            gameHandle = gameHandler;
        }


        // Loads new file
        public void LoadFile(string fileName)
        {
            currentStream = new StreamReader(@"..\..\StoryBlock\" + fileName);  //TODO: TEST WITH BUILD
        }

        // Get New StoryBlock
        public void GetBlock(string blockIndex)
        {
            currentBlock = new List<string>();
            string line = currentStream.ReadLine();
            // Finds block starting point in filestream
            while (line != blockIndex)
            {
                line = currentStream.ReadLine();
            }

            line = currentStream.ReadLine();
            // adds following lines to currentblock till ENDBLOCK
            while (line != "#ENDBLOCK")
            {
                currentBlock.Add(line);
                line = currentStream.ReadLine();
            }
            lineIndex = -1;

        }

        // TODO: ABLE TO Process NEXT COMMANDBLOCK
        public void ProcessNext()
        {
            lineIndex += 1;
            Command currentCommand = Command.Empty;
            List<string> commandStrings = new List<string>();

            // 1 Read Current Line
            string line = currentBlock[lineIndex];

            // 2 Switch between comamnds

            switch (line)
            {
                case "#PRINT":
                    currentCommand = Command.Print;
                    break;

                case "#BRANCH":
                    lineIndex += 1;
                    line = currentBlock[lineIndex];

                    if (line == "CHOICE")
                    {
                        currentCommand = Command.Choice;
                        
                    }

                    else if (line == "ROLL")
                    {
                        currentCommand = Command.Roll;
                    }
                    break;

                default:
                    break;
            }

            lineIndex += 1;
            line = currentBlock[lineIndex];
            //3 Iterate current Command

            while (line != "#END")
            {
                commandStrings.Add(line);

                lineIndex += 1;
                line = currentBlock[lineIndex];

            }

            // 4 sent CurrentCommand to GameManger
            gameHandle.ProcessCommand(currentCommand, commandStrings);
        }
    }
}
