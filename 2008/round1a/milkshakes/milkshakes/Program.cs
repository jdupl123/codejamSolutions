using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace milkshakes
{
    class Program
    {

        public void process_test_case(StreamReader input, StreamWriter outputStream, int NumFlav, int numCust, int curCase)
        {
            int[] numUnMaltChoises = new int[numCust];
            int[] maltPos = new int[numCust];
            for(int i=0; i<numCust; i++)
            {
                maltPos[i] = -1;
            }
            int[,] choiceTable = new int[numCust, NumFlav];
            for (int i=0; i < numCust; i++)
            {
                for (int j=0; j<NumFlav; j++)
                {
                    choiceTable[i, j] = -1;
                }
            }
            string line;
            string[] result;
            int[] conRes = new int[2500];
            int[] output = new int[NumFlav];
            for (int cust = 0; cust < numCust; cust++)
            {
                line = input.ReadLine();
                result = line.Split(' ');
                for (int i = 0; i < result.Length; i++)
                {
                    conRes[i] = int.Parse(result[i]);

                }
                for (int i = 1; i < conRes[0] * 2; i += 2)
                {
                    //Console.Write("{0}{1}\r\n", conRes[i], conRes[i + 1]);
                    choiceTable[cust, conRes[i]-1] = conRes[i + 1];
                    if (conRes[i + 1] == 0)
                    {
                        numUnMaltChoises[cust] += 1;
                    } else
                    {
                        maltPos[cust] = conRes[i]-1;
                    }
                }
            }

            bool stopped = true;
            while (stopped == true)
            {
                stopped = false;
                for (int i = 0; i < numCust; i++)
                {
                    if (numUnMaltChoises[i] == 0)
                    {
                        if (maltPos[i] == -1)
                        {
                            outputStream.WriteLine("Case #{0}: IMPOSSIBLE", curCase + 1);
                            outputStream.Flush();
                            return;
                        }
                        else
                        {
                            if (output[maltPos[i]] != 1)
                            {
                                output[maltPos[i]] = 1;
                                stopped = true;
                                for(int j=0; j < numCust; j++)
                                {
                                    if (choiceTable[ j, maltPos[i] ] == 0) //was one of cust j's unmalt choices 
                                    {
                                        numUnMaltChoises[j] -= 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            string strOut = string.Format("Case #{0}: ", curCase +1); 
            for(int i=0; i < NumFlav; i++)
            {
                strOut += string.Format("{0} ",output[i]);
            }
            strOut = strOut.Substring(0, strOut.Length - 1);
            outputStream.WriteLine(strOut);
            outputStream.Flush();
        }

        static int Main(string[] args)
        { 
            Program prog = new Program();
            int numTestCases;

            if (args.Length < 1) {
                Console.WriteLine("Usage: milkshakes.ext pathtotestfile");
                return 1;
            }
            string path = args[0];
            string outputPath = path.Substring(0, path.Length - 3);
            outputPath = outputPath + ".out";
            Console.WriteLine(outputPath);
            StreamReader inputStream = new StreamReader(path);
            StreamWriter outputStream = new StreamWriter(outputPath,false);
            string line = inputStream.ReadLine();
            numTestCases = Int32.Parse(line);
            for (int i = 0; i < numTestCases; i ++)
            {
                line = inputStream.ReadLine();
                int numFlav = int.Parse(line);
                line = inputStream.ReadLine();
                int numCust = int.Parse(line);
                prog.process_test_case(inputStream, outputStream, numFlav,numCust, i);
            }
            return 0;
        }
    }
}
