using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Text.Json;



namespace FlashCard
{
    class Program
    {
        class Flashcard
        {
            public string Question { get; set; }
            public string Answer { get; set; }
            public string Subject { get; set; }

            public Flashcard(string question, string answer, string subject = "General")
            {
                Question = question;
                Answer = answer;
                Subject = subject;
            }


        }
        static List<Flashcard> flashcards = new List<Flashcard>();



        static void Main(string[] args)
        {
            LoadFlashcards();

            while (true)
            {
                Console.WriteLine("Flashcard");
                Console.WriteLine("1- Add Flashcard");
                Console.WriteLine("2- View All Flashcards");
                Console.WriteLine("3- Quiz Mode");
                Console.WriteLine("4- Remove flashcard");
                Console.WriteLine("5- Filter flashcard");
                Console.WriteLine("6- Random flashcard");
                Console.WriteLine("7- Choose subject flashcard");
                Console.WriteLine("8- Exit");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddFlashcard();
                        break;
                    case "2":
                        ViewFlashcard();
                        break;
                    case "3":
                        Quiz_Mode();
                        break;
                    case "4":
                        RemoveFlashcard();
                        break;
                    case "5":
                        FilterFlashcard();
                        break;
                    case "6":
                        RandomFlashcard();
                        break;
                    case "7":
                        subjectFlashcard();
                        break;
                    case "8":
                        SaveFlashCards();
                        Console.WriteLine("Exiting program...");
                        return;


                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }

        }

        private static void SaveFlashCards()
        {
            string Json = JsonSerializer.Serialize(flashcards);
            System.IO.File.WriteAllText("flashcards.json", Json);
            Console.WriteLine("Flashcards saved successfully.");
        }

        private static void LoadFlashcards()
        {
            if (System.IO.File.Exists("flashcards.json"))
            {
                string json = System.IO.File.ReadAllText("flashcards.json");
                flashcards = JsonSerializer.Deserialize<List<Flashcard>>(json) ?? new List<Flashcard>();
                Console.WriteLine("Flashcards loaded successfully.");
            }
            else
            {
                Console.WriteLine("No saved flashcards found.");
            }
        }


        private static void subjectFlashcard()
        {
            Console.WriteLine("Enter the subject to view flashcards:");
            string subject = Console.ReadLine();

            var subjectCards = flashcards.Where(card => card.Subject.Equals(subject, StringComparison.OrdinalIgnoreCase)).ToList();

            if (subjectCards.Count > 0)
            {
                Console.WriteLine($"Flashcards under subject '{subject}':");
                foreach (var card in subjectCards)
                {
                    Console.WriteLine($"Question: {card.Question}");
                    Console.WriteLine($"Answer: {card.Answer}");
                    Console.WriteLine("-----------------------------");
                }
            }
        }

        private static void RandomFlashcard()
        {
            if (flashcards.Count == 0)
            {
                Console.WriteLine("No flashcards available.");
                return;
            }

            Console.WriteLine("How many questions do you want to answer?");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int numQuestions) || numQuestions <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a positive number.");
                return;
            }

            if (numQuestions > flashcards.Count)
            {
                Console.WriteLine($"Only {flashcards.Count} flashcards available. Adjusting the number of questions to {flashcards.Count}.");
                numQuestions = flashcards.Count;
            }


            var random = new Random();
            var shuffledFlashcards = flashcards.OrderBy(_ => random.Next()).Take(numQuestions).ToList();

            for (int i = 0; i < numQuestions; i++)
            {
                var card = shuffledFlashcards[i];
                Console.WriteLine($"Question {i + 1}: {card.Question}");
                Console.WriteLine($"Answer: {card.Answer}");
                Console.WriteLine($"Subject: {card.Subject}");
                Console.WriteLine("-----------------------------");
            }
        }


        private static void FilterFlashcard()
        {
            if (flashcards.Count == 0)
            {
                Console.WriteLine("No flashcards available.");
                return;
            }

            Console.WriteLine("Enter the subject to filter by:");
            string filterSubject = Console.ReadLine();

            var filteredCards = flashcards.FindAll(card => card.Subject.Equals(filterSubject, StringComparison.OrdinalIgnoreCase));

            if (filteredCards.Count > 0)
            {
                Console.WriteLine($"Flashcards under subject '{filterSubject}':");
                foreach (var card in filteredCards)
                {
                    Console.WriteLine($"Question: {card.Question}");
                    Console.WriteLine($"Answer: {card.Answer}");
                    Console.WriteLine("-----------------------------");
                }
            }
            else
            {
                Console.WriteLine($"No flashcards found under subject '{filterSubject}'.");
            }
        }

        private static void RemoveFlashcard()
        {
            if (flashcards.Count == 0)
            {
                System.Console.WriteLine("No flashcard to remove: ");
                return;
            }

            System.Console.WriteLine("Enter the name of the flashcard to remove: ");
            ViewFlashcard();

            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= flashcards.Count)
            {
                flashcards.RemoveAt(index - 1);
                Console.WriteLine("Flashcard removed successfully.");
            }
            else
            {
                Console.WriteLine("Invalid input. No flashcard removed.");
            }
        }

        private static void Quiz_Mode()
        {
            if (flashcards.Count == 0)
            {
                System.Console.WriteLine("No flashcard available: ");
                return;
            }

            var random = new Random();
            int correctAnswers = 0;

            foreach (var card in flashcards.OrderBy(x => random.Next()))
            {
                Console.WriteLine($"Question: {card.Question}");
                Console.Write("Your Answer: ");
                string userAnswer = Console.ReadLine();

                if (userAnswer.Trim().Equals(card.Answer, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Correct!");
                    correctAnswers++;
                }
                else
                {
                    Console.WriteLine($"Wrong! The correct answer is: {card.Answer}");
                }
            }

            Console.WriteLine($"Quiz over! You got {correctAnswers}/{flashcards.Count} correct.\n\n");
        }

        private static void ViewFlashcard()
        {
            if (flashcards.Count == 0)
            {
                System.Console.WriteLine("No flashcard available: ");
                return;
            }

            Console.WriteLine("All Flashcards:");
            int index = 1;
            foreach (var card in flashcards)
            {
                Console.WriteLine($"Flashcard {index}:");
                Console.WriteLine($"Question: {card.Question}");
                Console.WriteLine($"Subject: {card.Subject}");
                Console.WriteLine("-----------------------------");
                index++;
            }

        }

        private static void AddFlashcard()
        {
            System.Console.WriteLine("Enter the question: ");
            string question = Console.ReadLine();

            Console.WriteLine("Enter the answer:");
            string answer = Console.ReadLine();

            Console.WriteLine("Enter the subject (optional):");
            string subject = Console.ReadLine();

            if (string.IsNullOrEmpty(subject))
            {
                subject = "General";
            }

            flashcards.Add(new Flashcard(question, answer, subject));
            Console.WriteLine("Flashcard added successfully!\n\n");
        }
    }

}