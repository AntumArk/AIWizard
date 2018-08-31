using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AIWizard
{
    public class FancyWriter
    {
        public static void WriteSlow(string sentence)
        {
            foreach (var letter in sentence)
            {
                Console.Write(letter);
                Thread.Sleep(7);
            }
            Thread.Sleep(500);
            Console.WriteLine('\n');
        }

        public static void WriteSlow(string sentence, int duration)
        {
            foreach (var letter in sentence)
            {
                Console.Write(letter);
                Thread.Sleep(duration / sentence.Length);
            }
            Thread.Sleep(500);
            Console.WriteLine('\n');
        }

        public static void WriteHeader(string sentence)
        {
            Console.WriteLine("############################################");
            Console.WriteLine("##############" + sentence + "##############");
            Console.WriteLine("############################################");
            Console.WriteLine('\n');
        }

        public static double UpdateStatus(string infoText, ushort current, ushort total, bool returnPercents = false)
        {
            double percents = (((double)current / total) * 100);

            if (current < (total))
            {
                if (returnPercents)
                {
                    Console.Write(infoText + " " + current + "/" + total + " " + percents.ToString("0.00") + "%                            \r");
                }
                else
                {
                    Console.Write(infoText + " " + current + "/" + total + "                                      \r");
                }
            }
            else
            {
                if (returnPercents)
                {
                    Console.Write(infoText + " " + current + "/" + total + " " + percents.ToString("0.00") + "%                          \r\n");
                }
                else
                {
                    Console.Write(infoText + " " + current + "/" + total + "                      \r\n");
                }
                Console.WriteLine("Finished");
            }

            if (returnPercents)
            {
                return percents;
            }

            return 0;
        }
    }
}