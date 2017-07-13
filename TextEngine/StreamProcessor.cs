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

        public string currentFile { get; set; }
        public int currentStory { get; set; }

        public StreamProcessor(GameManager gameHandler)
        {
            gameHandle = gameHandler;
        }

        // Loads new file from full path
        public void LoadFile(string fileName)
        {
            currentFile = fileName;
            currentStream = new StreamReader(currentFile);
        }




        public bool GetCell(int cellNum)
        {
            bool success = false;

            if (currentStream != null)
            {
                currentBlock = new List<string>();
                string line = currentStream.ReadLine();

                while (line != "" + cellNum && !currentStream.EndOfStream)
                {
                    line = currentStream.ReadLine();
                }
                if (line == "" + cellNum)
                {
                    while (line != "#ENDCELL")
                    {
                        currentBlock.Add(line);
                        line = currentStream.ReadLine();
                    }
                    currentBlock.Remove("" + cellNum);
                    success = true;
                }
                else
                {
                    success = false;
                }
            }

            return success;
        }

        public List<string> NextCommand()
        {
            List<string> nextCommand = new List<string>();
            bool openGate = false;


            foreach (string line in currentBlock)
            {
                if (line.First<char>() == '#' && line != "#END")
                {
                    openGate = true;
                }

                else if (line == "#END")
                {
                    openGate = false;
                    break;
                }

                if (openGate)
                {
                    nextCommand.Add(line);
                }
            }

            currentBlock.RemoveRange(0, nextCommand.Count + 1);
            return nextCommand;
        }
    }
}
