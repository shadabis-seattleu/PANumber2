// AUTHOR: Jake Ladera, Shokoufeh Shadabi
// FILENAME: EncryptWord.cs
// DATE: 4/14/2018
// REVISION HISTORY:
//  4/14/2018
//      - None
// References:
//      - None

/*
Description:
This class takes a string and encrypts it using an abstracted Caesar shift if the proffered
string is considered valid. 

A string is considered invalid if
- any of its characters are not a part of the English alphabet 
- if it is less than 4 characters in length.
- if it contains any whitespaces. 

This class allows the user to guess the value of the shift and also records guess 
statistics (high guesses, low guesses, average guess value, and number of queries). Also 
supports 'resetting', which is defined as the re-initialization of the 'originalWord', 
'encryptedWord', and 'state'private data fields to their original values defined in the 
constructor, however guess statistics are not.

Anticipated Use:
- Securing and protecting data from general public. 
- Sensitizing data that may be accessed by non-authorized individuals.

Input/Output:
            EncryptWord()
                Legal Input: none
                Illegal Input: all
                Output: none
            receiveWord(string)
                Legal Input: a string
                Illegal Input: non-strings
                Output: True if proffered string is valid. False otherwise
            returnEncryptedWord()
                Legal Input: none
                Illegal Input: all
                Output: The encryptedWord string
            recieveGuess(int)
                Legal Input: an integer
                Illegal Input: all non-integer arguments
                Output: True if the proffered integer matches the value of
                        the private data field 'shift'. False if otherwise.
        
Legal States:
-	State of the class is defined by the private data field 'state'. 
-	State is considered to be 'on' when 'state' == false.      




When state is on:
                - Word encryption is not possible.
                - Guessing is not possible
When state is off:
                - Word encryption is  possible
                - Guessing not possible
                
Notes: 
-	Guess statistics' state is defined by the private data field 'canRequestStats'. 
-	State is considered to be 'on' when 'canRequestStats' == false.
        
When canRequestStats is on:
                - Requests for guess statistics (high guess, low guess, 
                  average guess value, and number of queries) are approved (returned)
When canRequestStats is off:
                - Requests for guess statistics are denied.
            
 
Interface:
            All legal function calls and their state dependencies are listed below. 
            Therefore, function calls that do not align with the states listed below are 
            considered to be illegal.

            When 'state' is on:
                    - May call decode().
                    - May call receiveGuess(int).

            When 'state' is off:
                    - May call receiveWord(string)

            May always call for guess statistics:
                    - May call returnHighGuess().
                    - May call returnLowGuess().
                    - May call returnAverageGuess().
                    - May call returnNumQueries().
                    Proper guess statistics are assumed to be the programmer's
                    responsibility. If the highGuess/lowGuess/averageGuess
                    return -1, appication programmer must print an error
                    message.

            May call at any time:
                - isStateOn()
                - isCanRequestStatsOn()
       
        Constructor:
        private data fields conditions: 
            - 'shift' is initially set to 0.
            - 'originalWord' is initially set to an empty string.
            - 'encryptedWord' is initially set to an empty string.
            - 'state' is initially set to true.
            - 'canRequestStats' is intitially set to true.
            - 'highGuess' is initially set to -1.
            - 'lowGuess' is initially set to -1.
            - 'numGuesses' is initially set to 0.
            - 'sumGuesses' is initially set to 0.
           
        State Transitions:
        State transitions for private data field 'state':
                - From on -> off:
                    - constructor -> receiveWord(string)
                - From off -> off:
                    - receiveWord(string) -> receiveGuess(int)
                    - receiveWord(string) -> receiveGuess(int) -> (any return 
                      guess statistic)

                - From off -> on:
                    - receiveGuess(int)
                        - NOTE: if and only if proffered integer matches shift value. 
                        State transition occurs when private function reset() is called 
                        from within the public function receiveGuess(int).

            State transitions for private data field 'canRequestStats':
                - From on -> off:
                    - constructor -> receiveWord(string) -> receiveGuess(int)
                - NOTE: state variable for returning guess statistics
                  ('canRequestStats') can only go from on -> off because guess statistics 
                  are retained for the life of the class object.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA1
{
    public class EncryptWord
    {
        private int shift;              // Caesar shift value
        private string originalWord;    // The original word proffered. Changes as each word is passed in.
        private string encryptedWord;   // The original string shifted by the Caesar shift
        private bool state;             // State of encryption
        private bool canRequestStats;   // State of statistic requesting capability (except for numGuesses)
        private int highGuess;          // Highest guess value in the history of class object
        private int lowGuess;           // Lowest guess value in the history of class object
        private int numGuesses;         // Total number of guesses in the history of class object
        private int sumGuesses;         // Sum of all guess values in the history of class object
        
        /*
         * DESCRIPTION: Default constructor. Sets all private data fields to valid initial state.
         * PRECONDITIONS: None.
         * POSTCONDITIONS: state = true, canRequestStats = true.
         */
        public EncryptWord()
        {
            shift = 0;
            originalWord = "";
            encryptedWord = "";
            state = true;
            canRequestStats = true;
            highGuess = -1;
            lowGuess = -1;
            numGuesses = 0;
            sumGuesses = 0;
        }

        /*
         * DESCRIPTION: Checks if state variable is on. If on (state = false) returns true. 
         * PRECONDITIONS: None
         * POSTCONDITIONS: None
         */
         public bool isStateOn()
        {
            if (state)
                return false;
            else
                return true;
        }

        /*
         * DESCRIPTION:Takes a passed in string, and if valid assigns to originalWord.
         * PRECONDITIONS: state = true 
         * POSTCONDITIONS: state = false 
         */
        public bool receiveWord(string ow)
        {
            // Enters condition if the string is considered legal
            if(isValid(ow))
            {
                generateShift();
                originalWord = ow.ToLower();
                // Shifts each character over by the shift value
                foreach(char c in ow)
                {
                    encryptedWord = ew(encryptedWord, c);
                }
                // Turns state on
                state = false;
                return true;

            }
            return false;
        }

        /*
        * DESCRIPTION: Checks if string argument is valid for encryption. Returns true if valid, false otherwise.
        * PRECONDITIONS: state = true
        * POSTCONDITIONS: state = true
        */
        private bool isValid(string s)
        {
            foreach(char c in s)
            {
                if(!((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z')))
                {
                    return false;
                }
            }
            return true;
        }

        /*
        * DESCRIPTION: Generates a new random shift value from 1-25
        * PRECONDITIONS: None
        * POSTCONDITIONS: None
        */
        private void generateShift()
        {
            const int ALPHA_BEGIN = 1;
            const int ALPHA_END = 25;
            Random r = new Random();
            shift = r.Next(ALPHA_BEGIN, ALPHA_END);
        }

        /*
         *  DESCRIPTION: Shifts each character in originalWord by the shift amount.
         *  PRECONDITIONS: None
         *  POSTCONDITIONS: None
         */
        private string ew(string s, char c)
        {
            int ASCII;
            const int ONE_LESS_A = 96;    // Character right before 'a' in ASCII code
            const int Z = 122;          // Lowercase 'z' in ASCII code
            // Convers charcter to lowercase
            c = Char.ToLower(c);

            // Converts character to ASCII value and shifts by shift
            ASCII = (int)c + shift;

            // Loops back around to beginning of alphabet if out of alphabetical scope.
            if(ASCII > Z)
            {
                ASCII = (ASCII - Z) + ONE_LESS_A;
            }

            // Converts back to a character
            c = (char)ASCII;

            // Adds shifted character to encryptedWord string
            StringBuilder sb = new StringBuilder();
            sb.Append(s);
            sb.Append(c);
            s = sb.ToString();
            return s;
        }

        /*
        * DESCRIPTION: Returns value of encryptedWord
        * PRECONDITIONS: state = false
        * POSTCONDITIONS: state = false
        */
        public string returnEncrypted()
        {
            return encryptedWord;
        }

        /*
         * DESCRIPTION: Checks if canRequestStats variable is on. If on (canRequestStats = false) returns true.
         * PRECONDITIONS: None
         * POSTCONDITIONS: None
         */
        public bool isCanRequestStatsOn()
        {
            if (canRequestStats)
                return false;
            else
                return true;
        }

        /*
         * DESCRIPTION: Takes user guess of the current shift value
         * PRECONDITIONS: state = false
         * POSTCONDITIONS: state = false if the guess (g) does not match shift. Otherwise state = true.
         */
         public bool receiveGuess(int g)
        {
            // Increment numGuesses
            numGuesses++;
            sumGuesses += g;

            // Assigns to highGuess and/or low guess
            if (numGuesses == 1)    // first guess
            {
                highGuess = g;
                lowGuess = g;
                if (g != shift)
                    return false;
            }
            else
            {
                // Assigns to highGuess if highest guess
                if (g != shift)     // subsequent guesses
                {
                    if (g > highGuess)
                        highGuess = g;
                    else if (g < lowGuess)
                        lowGuess = g;
                    return false;
                }
            }

            // Checks to see if the guess matches the shift value
            if (g == shift)
                reset();
            return true;
        }

        /*
         * DESCRIPTION: Returns the value of originalWord as the method of decoding.
         * PRECONDITIONS: state = false
         * POSTCONDITIONS: state = false
         */
        public string decode()
        {
            return originalWord;
        }

        /*
         * DESCRIPTION: Returns the value of highest shift value guess in the lifetime of the class object.
         * PRECONDITIONS: canRequestStats = false
         * POSTCONDITIONS: canRequestStats = false
         */
        public int returnHighGuess()
        {
            return highGuess;
        }

        /*
         * DESCRIPTION: Returns the value of the lowest shift value guess in the lifetime of the class object.
         * PRECONDITIONS: canRequestStats = false
         * POSTCONDITIONS: canRequestStats = false
         */
         public int returnLowGuess()
        {
            return lowGuess;
        }

        /*
         * DESCRIPTION: Returns the average value of the user's guesses
         * PRECONDITIONS: canRequestStats = false
         * POSTCONDITIONS: canRequestStats = false
         */
         public int returnAverageGuess()
        {
            int avG = sumGuesses / numGuesses;
            return avG;
        }

        /*
         * DESCRIPTION: Returns the total number of times the user has guessed the shift value.
         * PRECONDITIONS: None
         * POSTCONDITIONS: None
         */
         public int returnNumQueries()
        {
            return numGuesses;
        }

        /*
         * DESCRIPTION: Resets originalWord, encryptedWord, and state to the values defined in the constructor.
         * PRECONDITIONS: state = false
         * POSTCONDITIONS: state = true
         */
        private void reset()
        {
            originalWord = "";
            encryptedWord = "";
            state = true;
        }
    }
}
