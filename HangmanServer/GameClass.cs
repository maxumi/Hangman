using System.Net.Sockets;
using System.Text;

namespace HangmanServer
{
    internal class GameClass
    {
        private readonly Socket handler;

        public GameClass(Socket handler)
        {
            this.handler = handler;
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
                StringBuilder stringBuilder = new StringBuilder();
                string msg = null;
                byte[] buffer = new byte[1024];
                // Print the current state of the game
                stringBuilder.Append($"\nWord: {GetWordToDisplay(wordToGuess, correctGuesses)}");
                stringBuilder.Append($"\nLives: {lives}");
                stringBuilder.Append($"\nIncorrect guesses: {string.Join(", ", incorrectGuesses)}");

                // Send the game state to the client
                byte[] messageBytes = Encoding.ASCII.GetBytes(stringBuilder.ToString());
                handler.Send(messageBytes);

                // Get clients answer and put it in msg
                while (msg == null)
                {
                    int received = handler.Receive(buffer);
                    msg += Encoding.ASCII.GetString(buffer, 0, received);
                }

                // Check if the guess is one c
                char input = msg[0];
                var guess = char.ToLower(input);

                // Check if the guess is correct or incorrect
                if (wordToGuess.Contains(guess))
                {
                    correctGuesses.Add(guess);
                    if (HasWon(wordToGuess, correctGuesses))
                    {
                        string win = "You win!";
                        byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(win);
                        handler.Send(byteArray);

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
                        string lose = "You lose!";
                        byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(lose);
                        handler.Send(byteArray);

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

