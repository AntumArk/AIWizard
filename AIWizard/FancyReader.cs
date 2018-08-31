using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AIWizard
{
    public static class FancyReader
    {
        public static bool AwaitConfirmation()
        {
            while (Console.KeyAvailable)
                Console.ReadKey(true);
            var okStrings = new List<string>()
            {
                "Yes",
                "yes",
                "y",
                "Y",
                "Aha",
                "aha",
                "Ok",
                "OK",
                "taip"
            };
            var noStrings = new List<string>()
            {
                "No",
                "no",
                "NO",
                "Ne",
                "Never"
            };
            var receivedResult = false;
            var resultValue = false;
            var firstRead = true;

            while (!receivedResult)
            {
                if (!firstRead)
                    FancyWriter.WriteSlow("I did not get that... Please repeat that.");
                var inString = Console.ReadLine();
                if (inString != null)
                {
                    var words = inString.Split(" ");
                    foreach (var word in words)
                    {
                        if (okStrings.Contains(word))

                        {
                            receivedResult = true;
                            resultValue = true;
                            FancyWriter.WriteSlow("Ok is ok. Proceding...");
                            break;
                        }

                        if (noStrings.Contains(word))
                        {
                            receivedResult = true;
                            FancyWriter.WriteSlow("It's treason then!");
                        }
                    }
                }

                firstRead = false;
            }

            return resultValue;
        }

        public static bool AwaitConfirmation(string prompt)
        {
            while (Console.KeyAvailable)
                Console.ReadKey(true);
            FancyWriter.WriteSlow(prompt);
            var okStrings = new List<string>()
            {
                "Yes",
                "yes",
                "y",
                "Y",
                "Aha",
                "aha",
                "Ok",
                "OK",
                "taip"
            };
            var noStrings = new List<string>()
            {
                "No",
                "no",
                "NO",
                "Ne",
                "n",
                "N",
                "Never"
            };
            var receivedResult = false;
            var resultValue = false;
            var firstRead = true;

            while (!receivedResult)
            {
                if (!firstRead)
                    FancyWriter.WriteSlow("I did not get that... Please repeat that.");
                var inString = Console.ReadLine();
                if (inString != null)
                {
                    var words = inString.Split(" ");
                    foreach (var word in words)
                    {
                        if (okStrings.Contains(word))

                        {
                            receivedResult = true;
                            resultValue = true;
                            FancyWriter.WriteSlow("Ok is ok. Procceding...");
                            break;
                        }

                        if (noStrings.Contains(word))
                        {
                            receivedResult = true;
                            FancyWriter.WriteSlow("It's treason then!");
                        }
                    }
                }

                firstRead = false;
            }

            return resultValue;
        }

        /// <summary>
        /// Awaits the uint.
        /// </summary>
        /// <returns></returns>
        public static int AwaitInt()
        {
            while (Console.KeyAvailable)
                Console.ReadKey(true);
            var answer = Console.ReadLine();
            var result = 0;
            while (!int.TryParse(answer, out result))
            {
                FancyWriter.WriteSlow("Invalid argument. Your answer is not an integer. Please enter valid answer.");
                answer = Console.ReadLine();
            }

            return result;
        }

        public static uint AwaitPosUInt()
        {
            while (Console.KeyAvailable)
                Console.ReadKey(true);
            var answer = Console.ReadLine();
            uint result = 0;
            while (!uint.TryParse(answer, out result))
            {
                FancyWriter.WriteSlow(
                    "Invalid argument. Your answer is not a positive integer. Please enter valid answer.");
                answer = Console.ReadLine();
            }

            return result;
        }

        public static uint AwaitPosUInt(string prompt)
        {
            while (Console.KeyAvailable)
                Console.ReadKey(true);
            FancyWriter.WriteSlow(prompt);
            var answer = Console.ReadLine();
            uint result = 0;
            while (!uint.TryParse(answer, out result))
            {
                FancyWriter.WriteSlow(
                    "Invalid argument. Your answer is not a positive integer. Please enter valid answer.");
                answer = Console.ReadLine();
            }

            return result;
        }

        public static int AwaitNegInt()
        {
            while (Console.KeyAvailable)
                Console.ReadKey(true);
            var answer = Console.ReadLine();
            var result = 0;
            while (int.TryParse(answer, out result) && result >= 0)
            {
                FancyWriter.WriteSlow(
                    "Invalid argument. Your answer is not negative integer. Please enter valid answer.");
                answer = Console.ReadLine();
            }

            return result;
        }

        public static double AwaitDouble()
        {
            while (Console.KeyAvailable)
                Console.ReadKey(true);
            var answer = Console.ReadLine();
            double result = 0;
            while (!double.TryParse(answer, out result) && result >= 0)
            {
                FancyWriter.WriteSlow(
                    "Invalid argument. Your answer is not a double. Please enter valid answer.");
                answer = Console.ReadLine();
            }

            return result;
        }

        public static double AwaitPosDouble()
        {
            while (Console.KeyAvailable)
                Console.ReadKey(true);
            var answer = Console.ReadLine();
            double result = -1;
            while (!double.TryParse(answer, out result) && result < 0)
            {
                FancyWriter.WriteSlow(
                    "Invalid argument. Your answer is not a positive double. Please enter valid answer.");
                answer = Console.ReadLine();
            }

            return result;
        }

        public static double AwaitNegDouble()
        {
            while (Console.KeyAvailable)
                Console.ReadKey(true);
            var answer = Console.ReadLine();
            double result = 0;
            while (!double.TryParse(answer, out result) && result >= 0)
            {
                FancyWriter.WriteSlow(
                    "Invalid argument. Your answer is not a negative double. Please enter valid answer.");
                answer = Console.ReadLine();
            }

            return result;
        }

        public static string AwaitAnswer()
        {
            while (Console.KeyAvailable)
                Console.ReadKey(true);
            var answer = Console.ReadLine();
            return answer;
        }

        public static string AwaitDirectory(string prompt)
        {
            while (Console.KeyAvailable)
                Console.ReadKey(true);
            FancyWriter.WriteSlow(prompt);
            var answer = Console.ReadLine();
            while (!Directory.Exists(answer))
            {
                FancyWriter.WriteSlow(
                    "Invalid argument. Your answer is not a valid directory. Please enter valid answer.");
                answer = Console.ReadLine();
            }
            return answer;
        }

        public static string AwaitDirectory()
        {
            while (Console.KeyAvailable)
                Console.ReadKey(true);
            var answer = Console.ReadLine();
            while (!Directory.Exists(answer))
            {
                FancyWriter.WriteSlow(
                    "Invalid argument. Your answer is not a valid directory. Please enter valid answer.");
                answer = Console.ReadLine();
            }
            return answer;
        }

        public static string AwaitAnswer(string prompt)
        {
            while (Console.KeyAvailable)
                Console.ReadKey(true);
            FancyWriter.WriteSlow(prompt);
            var answer = Console.ReadLine();
            return answer;
        }
    }
}