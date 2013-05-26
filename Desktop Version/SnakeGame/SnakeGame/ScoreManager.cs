using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeGame
{
    public class ScoreManager
    {
        public const int maxn = 5;
        public List<int> sc;

        FileStream theFileWriter, theFileReader;
        StreamWriter theScoreWriter;
        StreamReader theScoreReader;

        public ScoreManager()
        {
            sc = new List<int>();

            theFileWriter = new FileStream("score.info", FileMode.Create, FileAccess.Write);
            theScoreWriter = new StreamWriter(theFileWriter);

            for (int i = 0; i < maxn; i++)
            {
                sc.Add(1000 - i * 100);
                theScoreWriter.WriteLine(sc[i]);
            }

            theScoreWriter.Close();
            theFileWriter.Close();
        }

        public void readFile()
        {
            theFileReader = new FileStream("score.info", FileMode.OpenOrCreate, FileAccess.Read);
            theScoreReader = new StreamReader(theFileReader);

            for (int i = 0; i < maxn; i++)
            {
                sc[i] = Convert.ToInt32(theScoreReader.ReadLine());
            }

            theScoreReader.Close();
            theFileReader.Close();
        }

        public void writeFile()
        {
            lSort();

            theFileWriter = new FileStream("score.info", FileMode.Create, FileAccess.Write);
            theScoreWriter = new StreamWriter(theFileWriter);

            for (int i = 0; i < maxn; i++)
            {
                theScoreWriter.WriteLine(sc[i]);
            }

            theScoreWriter.Close();
            theFileWriter.Close();
        }

        public void lSort()
        {
            int temp;
            for (int i = 0; i < sc.Count; i++)
                for (int j = i + 1; j < sc.Count; j++) if (sc[i] < sc[j])
                    {
                        temp = sc[i];
                        sc[i] = sc[j];
                        sc[j] = temp;
                    }
        }

        public void addScore(int val)
        {
            sc.Add(val);
        }

        public int getScore(int index)
        {
            return sc[index];
        }

        public int getLimit()
        {
            return maxn;
        }
    }
}
