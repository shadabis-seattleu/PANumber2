using System;
namespace PA1
{
    public class Driver
    {
        EncryptWord ew = new EncryptWord();
        string[] possWords = { "hello", "hi", "&yuL", "!!!!", "    ", " helo", "mart ini" };
        const int MAX_GUESS = 30;
        const string ILLEGAL_STR1 = "I'm sorry, your string '";
        const string ILLEGAL_STR2 = "' is invalid. Please try another string.\n";
        const string GUESS_RIGHT = "Congratulations! You guessed the shift correctly!\n";
        const string GUESS_WRONG = "Shucks... You guessed the shift value incorrectly. Try again :)\n";
        const string CANNOT_GUESS_STAT = "Sorry, you cannot request guess statistics at this time.\n";
        const int CANT_GUESS_CONDITION = -1;
        const string CANNOT_GUESS = "Sorry, you must have a word encrypted before guessing the shift value\n";
        public Driver()
        {
            Console.WriteLine("Now beginning EncryptWord!\n\n");
        }

        public bool run()
        {
            Console.WriteLine("Requesting guess statistic before any guesses...\n");
            if (ew.returnHighGuess() == CANT_GUESS_CONDITION)
                Console.WriteLine(CANNOT_GUESS_STAT);

            foreach(string s in possWords)
            {
                // Encrypt word
                if(ew.receiveWord(s))
                {
                    Console.WriteLine("\tNow passing in: " + s + "\n");
                    Console.WriteLine("\tEncrypted: " + ew.returnEncrypted() + ", Decoded: " +
                                      ew.decode() + "\n");

                    // Test if you can encrypt more than one word at a time
                    Console.WriteLine("\tTesting if you can encrypt another word...");

                    if (ew.receiveWord(possWords[0]))
                    {
                        Console.WriteLine("Nope!\n");
                    }
                    else
                    {
                        Console.WriteLine("Yep!\n");
                    }

                    // Guess shift
                    for (int i = 0; i < MAX_GUESS; i++)
                    {
                        // Guess shift
                        Console.WriteLine("\t\tNow guessing shift '" + i + "'...");
                        if(ew.receiveGuess(i))
                        {
                            Console.WriteLine("\t\t" + GUESS_RIGHT);
                        }
                        else
                        {
                            if (ew.isStateOn())
                                Console.WriteLine("\t\t" + GUESS_WRONG);
                            else
                                Console.WriteLine("\t\t" + CANNOT_GUESS);    
                        }
                    }
                }
                else
                {
                    Console.WriteLine(ILLEGAL_STR1 + s + ILLEGAL_STR2);
                }
            }
            Console.WriteLine("Highest guess was: " + ew.returnHighGuess() + "\n");
            Console.WriteLine("Lowest guess was: " + ew.returnLowGuess() + "\n");
            Console.WriteLine("Average guess was: " + ew.returnAverageGuess() + "\n");
            Console.WriteLine("Number of guesses were: " + ew.returnNumQueries() + "\n");

            return true;
        }
    }
}
