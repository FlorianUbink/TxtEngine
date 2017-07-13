using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine
{
    public class Library
    {
        Dictionary<int, string> fileDir;
        List<int> keys;

        public Library()
        {
            fileDir = new Dictionary<int, string>();
            keys = new List<int>();
            LoadDir();
        }



        public string FindFile(int StoryBlock)
        {
            string filePath = "ERROR";
            
            // iterates over all keys 
            for (int i = 0; i <= keys.Count - 1; i++)
            {

                // checks whether requested key (StoryBlock) is between current index and next index

                if (StoryBlock >= keys[i] && i + 1 > keys.Count - 1)
                {
                    filePath = fileDir[keys[i]];
                    break;
                }

                else if (StoryBlock >= keys[i] && StoryBlock < keys[i + 1])
                {
                    filePath = fileDir[keys[i]];
                    break;
                }
            }

            return filePath;

        }


        private void LoadDir()
        {
            foreach (string file in Directory.GetFiles(@"..\..\StoryBlock\"))
            {


                string subString = file.Substring(17);

                int key = int.Parse(subString.Remove((subString.LastIndexOf("."))));

                // Adds file key + full filepath to dictionary & adds key also to seperate keylist for iteration
                keys.Add(key);
                fileDir.Add(key, file);
            }
        }
    }
}
