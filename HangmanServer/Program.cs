using HangmanServer;
using System.Text;

namespace Hangman
{
    class Program
    {
        static void Main(string[] args)
        {
            //var server = new HangmanServerClass();
            Console.WriteLine("Welcome to Hangman!");

            // Select a word
            var words = new List<string> { "computer", "programming", "language", "internet", "keyboard", "monitor" };
            var random = new Random();
            var wordToGuess = words[random.Next(words.Count)];

            // Initialize the game state
            var correctGuesses = new HashSet<char>();
            var incorrectGuesses = new HashSet<char>();
            var lives = 6;

            while (true)
            {
                // Print the current state of the game
                Console.WriteLine();
                Console.WriteLine($"Word: {GetWordToDisplay(wordToGuess, correctGuesses)}");
                Console.WriteLine($"Lives: {lives}");
                Console.WriteLine($"Incorrect guesses: {string.Join(", ", incorrectGuesses)}");

                // Get the user's guess
                Console.Write("Guess a letter: ");
                var input = Console.ReadLine();
                if (input.Length != 1 || !char.IsLetter(input[0]))
                {
                    Console.WriteLine("Invalid input. Please enter a single letter.");
                    continue;
                }
                var guess = char.ToLower(input[0]);

                // Check if the guess is correct or incorrect
                if (wordToGuess.Contains(guess))
                {
                    correctGuesses.Add(guess);
                    if (HasWon(wordToGuess, correctGuesses))
                    {
                        Console.WriteLine("You win!");
                        return;
                    }
                }
                else
                {
                    incorrectGuesses.Add(guess);
                    lives--;
                    if (lives == 0)
                    {
                        Console.WriteLine("You lose!");
                        return;
                    }
                }
            }
        }

        static string GetWordToDisplay(string wordToGuess, HashSet<char> correctGuesses)
        {
            var wordToDisplay = new StringBuilder();
            foreach (var letter in wordToGuess)
            {
                if (correctGuesses.Contains(letter))
                {
                    wordToDisplay.Append(letter);
                }
                else
                {
                    wordToDisplay.Append("_");
                }
                wordToDisplay.Append(" ");
            }
            return wordToDisplay.ToString();
        }

        static bool HasWon(string wordToGuess, HashSet<char> correctGuesses)
        {
            foreach (var letter in wordToGuess)
            {
                if (!correctGuesses.Contains(letter))
                {
                    return false;
                }
            }
            return true;
        }
    }
}