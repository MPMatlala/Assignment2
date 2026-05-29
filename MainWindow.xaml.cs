using Assignment2;
using demAssignment2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Assignment2
{
    public partial class MainWindow : Window
    {
        // chatbot storage
        ArrayList reply = new ArrayList();
        ArrayList ignore = new ArrayList();
        user_name check_name = new user_name();
        // variables
        string username = string.Empty;
        int counting = 0;

        public MainWindow()
        {
            InitializeComponent();

            new respond(reply, ignore) { };
            voice_greeting greet = new voice_greeting();
            greet.greet();

            // sample ignored words
            ignore.Add("the");
            ignore.Add("and");
            ignore.Add("is");
            ignore.Add("a");
            ignore.Add("an");
            ignore.Add("to");
            ignore.Add("of");
            ignore.Add("in");

            // sample replies
            reply.Add("Passwords should be strong and unique.");
            reply.Add("Never share your OTP with anyone.");
            reply.Add("Phishing emails try to steal your information.");
            reply.Add("Use two-factor authentication for better security.");
            reply.Add("Cybersecurity helps protect your data online.");
        }

        // =========================================================
        // START BUTTON
        // =========================================================
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            home_grid.Visibility = Visibility.Hidden;
            username_grid.Visibility = Visibility.Visible;
        }

        // =========================================================
        // SUBMIT NAME BUTTON
        // =========================================================
        private void SubmitNameButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UsernameInput.Text))
            {
                UsernameErrorText.Text = "Please enter your name.";
                return;
            }

            username = UsernameInput.Text.Trim();

            HeaderUsernameText.Text = username;

            username_grid.Visibility = Visibility.Hidden;
            ChatGrid.Visibility = Visibility.Visible;

            error_method("ChatBot", "Hello " + username + "! Ask me anything about cybersecurity.");
        }

        // =========================================================
        // SEND BUTTON
        // =========================================================
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string rawQuestion = MessageInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(rawQuestion))
            {
                error_method("ChatBot", "Please enter a question.");
                return;
            }

            // ── EXIT COMMAND ──────────────────────────────────────
            string lowerRaw = rawQuestion.ToLower();
            if (lowerRaw == "exit" || lowerRaw == "quit" || lowerRaw == "bye")
            {
                error_method("ChatBot", "Goodbye " + username + "! Stay safe online.");
                MessageInput.Clear();
                // Short delay so the user sees the farewell message before the window closes
                System.Threading.Tasks.Task.Delay(1200).ContinueWith(_ =>
                {
                    Dispatcher.Invoke(() => Application.Current.Shutdown());
                });
                return;
            }

            // display user message
            error_method(username, rawQuestion);

            // clean question
            string questions = RemoveSpecialCharacters(rawQuestion);

            // process chatbot response (single message only)
            ai_check(questions);
        }

        // =========================================================
        // ENTER KEY EVENTS
        // =========================================================
        private void UsernameInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SubmitNameButton_Click(sender, e);
        }

        private void MessageInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SendButton_Click(sender, e);
        }

        // =========================================================
        // AI RESPONSE CHECKER
        // Returns exactly ONE message per question
        // =========================================================
        private void ai_check(string questions)
        {
            if (string.IsNullOrWhiteSpace(questions))
            {
                error_method("ChatBot", "Please enter a valid question.");
                MessageInput.Clear();
                return;
            }

            string[] words = questions.ToLower().Split(
                new char[] { ' ', ',', '.', '?', '!', ';', ':' },
                StringSplitOptions.RemoveEmptyEntries);

            // Track how many keywords each reply matches
            Dictionary<string, int> scoreMap = new Dictionary<string, int>();

            foreach (string word in words)
            {
                if (word.Length < 3 || ignore.Contains(word))
                    continue;

                foreach (string answer in reply)
                {
                    if (answer.ToLower().Contains(word))
                    {
                        if (scoreMap.ContainsKey(answer))
                            scoreMap[answer]++;
                        else
                            scoreMap[answer] = 1;
                    }
                }
            }

            if (scoreMap.Count > 0)
            {
                // Pick the single reply with the highest keyword score
                string bestAnswer = scoreMap.OrderByDescending(kv => kv.Value).First().Key;

                // Show the interest reminder every 3 questions, combined into one message
                if (counting >= 3)
                {
                    error_method("ChatBot", bestAnswer
                        + "\n\nRemember: stay safe online and never share your passwords.");
                    counting = 0;
                }
                else
                {
                    error_method("ChatBot", bestAnswer);
                    counting++;
                }
            }
            else
            {
                Random random = new Random();
                string[] fallback =
                {
                    "I don't understand that yet.",
                    "Can you ask something about cybersecurity?",
                    "Please rephrase your question.",
                    "I could not find an answer for that.",
                    "Try asking about passwords, phishing or scams."
                };

                error_method("ChatBot", fallback[random.Next(fallback.Length)]);
            }

            MessageInput.Clear();
        }

        // =========================================================
        // REMOVE SPECIAL CHARACTERS
        // =========================================================
        private string RemoveSpecialCharacters(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            StringBuilder sanitized = new StringBuilder();

            foreach (char c in input)
            {
                if (char.IsLetterOrDigit(c) ||
                    char.IsWhiteSpace(c) ||
                    c == '\'' ||
                    c == '-')
                {
                    sanitized.Append(c);
                }
                else
                {
                    sanitized.Append(' ');
                }
            }

            string result = sanitized.ToString();
            result = Regex.Replace(result, @"\s+", " ").Trim();

            return result;
        }

        // =========================================================
        // CHAT MESSAGE DISPLAY
        // =========================================================
        private void error_method(string name, string message)
        {
            Border messageBorder = new Border
            {
                Margin = new Thickness(0, 5, 0, 5),
                Padding = new Thickness(10),
                CornerRadius = new CornerRadius(8)
            };

            bool isBot = name.ToLower().Contains("chatbot");

            if (isBot)
            {
                messageBorder.Background =
                    new SolidColorBrush(Color.FromRgb(240, 248, 255));
                messageBorder.BorderBrush =
                    new SolidColorBrush(Color.FromRgb(173, 216, 230));
            }
            else
            {
                messageBorder.Background =
                    new SolidColorBrush(Color.FromRgb(245, 245, 245));
                messageBorder.BorderBrush =
                    new SolidColorBrush(Color.FromRgb(211, 211, 211));
            }

            messageBorder.BorderThickness = new Thickness(1);

            TextBlock messageText = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap
            };

            messageText.Inlines.Add(new Run
            {
                Text = name + ": ",
                FontWeight = FontWeights.Bold,
                Foreground = isBot ? Brushes.DarkBlue : Brushes.DarkGreen
            });

            messageText.Inlines.Add(new Run
            {
                Text = message,
                Foreground = Brushes.Black
            });

            messageBorder.Child = messageText;

            ChatListBox.Items.Add(messageBorder);

            ChatListBox.ScrollIntoView(
                ChatListBox.Items[ChatListBox.Items.Count - 1]);
        }//end of error_method
    }//end of class
}//end of namespace