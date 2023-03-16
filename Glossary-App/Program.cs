using Glossary_Library;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Glossaries.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Check if there are any arguments passed
                if (args == null || args.Length == 0)
                {
                    PrintError();
                    return;
                }

                // Get the first argument
                var command = args[0].ToLower();

                // Handle different commands
                switch (command)
                {
                    case "-lists":
                        // Get all word lists and print them
                        var lists = WordList.GetLists();
                        Console.WriteLine("Following lists found:");
                        foreach (var list in lists)
                        {
                            Console.WriteLine(list);
                        }
                        break;

                    case "-new":
                        // Create a new word list
                        if (args.Length < 3)
                        {
                            Console.WriteLine("Invalid arguments.");
                            PrintError();
                            return;
                        }

                        var newListName = args[1];
                        var newListLanguages = args.Skip(2).ToArray();
                        var newWordList = new WordList(newListName, newListLanguages);
                        newWordList.Save();

                        Console.WriteLine($"New list created with name {newListName}");
                        WordInput(newListName);
                        break;

                    case "-add":
                        // Add words to a word list
                        if (args.Length < 2)
                        {
                            PrintError();
                            return;
                        }

                        WordInput(args[1]);
                        break;

                    case "-remove":
                        // Remove words from a word list
                        if (args.Length < 4)
                        {
                            PrintError();
                            return;
                        }

                        var removeListName = args[1];
                        var removeLanguage = args[2];
                        var wordsToRemove = args.Skip(3);

                        var removeWordList = WordList.LoadList(removeListName);
                        if (removeWordList == null)
                        {
                            throw new Exception($"Could not find a word list with name {removeListName}");
                        }

                        var languageIndex = removeWordList.Languages.ToList().IndexOf(removeLanguage);
                        if (languageIndex < 0)
                        {
                            Console.WriteLine($"Language {removeLanguage} not found in list {removeListName}");
                            return;
                        }

                        foreach (var word in wordsToRemove)
                        {
                            removeWordList.Remove(languageIndex, word);
                            Console.WriteLine($"Word {word} removed from {removeLanguage}");
                        }

                        removeWordList.Save();
                        break;

                    case "-words":
                        // List all the words in a word list
                        if (args.Length < 2)
                        {
                            PrintError();
                            return;
                        }

                        var listName = args[1];
                        var wordList = WordList.LoadList(listName);
                        if (wordList == null)
                        {
                            throw new Exception($"Could not find a word list with name {listName}");
                        }

                        var languageIndexToSortBy = args.Length > 2 ? wordList.Languages.ToList().IndexOf(args[2]) : 0;

                        wordList.List(languageIndexToSortBy, (translations) =>
                        {
                            foreach (var translation in translations)
                            {
                                Console.WriteLine(translation);
                            }
                        });

                        break;

                    case "-count":
                        // Count the number of words in a word list
                        if (args.Length < 2)
                        {
                            PrintError();
                            return;
                        }

                        var countListName = args[1];
                        var countWordList = WordList.LoadList(countListName);
                        if (countWordList == null)
                        {
                            throw new Exception($"Could not find a word list with name {countListName}");
                        }

                        Console.WriteLine($"There are {countWordList.Count()} words in {countListName}");
                        break;

                    case "-practice":
                        // Practice words in a word list
                        if (args.Length < 2)
                        {
                            PrintError();
                            return;
                        }

                        var practiceListName = args[1];
                        var practiceWordList = WordList.LoadList(practiceListName);
                        if (practiceWordList == null)
                        {
                            throw new Exception($"Could not find a word list with name {practiceListName}");
                        }

                        var practiceResults = new List<PracticeResults>();
                        var random = new Random();

                        while (true)
                        {
                            var wordToPractice = practiceWordList.GetWordToPractice();
                            if (wordToPractice == null)
                            {
                                Console.WriteLine("List is empty, no word found.");
                                break;
                            }

                            var indexFrom = random.Next(0, wordToPractice.Translations.Count() - 1);
                            var indexTo = random.Next(0, wordToPractice.Translations.Count() - 1);

                            while (indexTo == indexFrom)
                            {
                                indexTo = random.Next(0, wordToPractice.Translations.Count() - 1);
                            }

                            var translationFrom = practiceWordList.Languages[indexFrom];
                            var translationTo = practiceWordList.Languages[indexTo];

                            // Check if word is removed
                            if (string.IsNullOrWhiteSpace(wordToPractice.Translations[indexFrom]) ||
                                string.IsNullOrWhiteSpace(wordToPractice.Translations[indexTo]))
                            {
                                continue;
                            }

                            Console.WriteLine($"Please translate {wordToPractice.Translations[indexFrom]} from {translationFrom} to {translationTo}");

                            var input = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(input))
                            {
                                break;
                            }

                            var expectedAnswer = wordToPractice.Translations[indexTo];

                            if (input.ToLower() == expectedAnswer.ToLower())
                            {
                                Console.WriteLine("Correct. Let's do the next one!");
                            }
                            else
                            {
                                Console.WriteLine($"Sorry, the correct translation is {expectedAnswer}");
                            }

                            practiceResults.Add(new PracticeResults()
                            {
                                Answer = input,
                                ExpectedAnswer = expectedAnswer,
                                TranslationFrom = translationFrom,
                                TranslationTo = translationTo,
                                Word = wordToPractice.Translations[indexFrom],
                            });
                        }

                        Console.WriteLine("Practice Summary");
                        Console.WriteLine($"Practice Words: {practiceResults.Count}");
                        Console.WriteLine($"Correct: {practiceResults.Count(a => a.IsCorrect)}");
                        Console.WriteLine($"Wrong: {practiceResults.Count(a => !a.IsCorrect)}");
                        Console.WriteLine("Details");
                        Console.WriteLine("====================================");

                        foreach (var result in practiceResults)
                        {
                            Console.WriteLine($"Word: {result.Word}");
                            Console.WriteLine($"Translation From: {result.TranslationFrom}");
                            Console.WriteLine($"Translation To: {result.TranslationTo}");
                            Console.WriteLine($"Answer: {result.Answer}");
                            Console.WriteLine($"Expected Answer: {result.ExpectedAnswer}");
                            Console.WriteLine($"Is Correct: {(result.IsCorrect ? "Yes" : "No")}");
                            Console.WriteLine("====================================");
                        }

                        break;

                    default:
                        PrintError();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                PrintError();
            }

            Console.WriteLine("");
            Console.WriteLine("Press any key to exit!");
            Console.ReadLine();
        }

        private static void WordInput(string listName)
        {
            Console.WriteLine($"We are writing words in list {listName}");

            var wordList = WordList.LoadList(listName);
            while (true)
            {
                var translations = new List<string>();

                foreach (var language in wordList.Languages)
                {
                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLine($"Please write a word for language: {language}");

                    var word = "";
                    while (string.IsNullOrEmpty(word))
                    {
                        word = Console.ReadLine();
                    }

                    translations.Add(word);
                }

                wordList.Add(translations.ToArray());

                Console.WriteLine("Do you want to enter more words? Press Y to continue. Enter to exit.");
                var input = Console.ReadKey();
                if (input.Key == ConsoleKey.Y)
                {
                    continue;
                }
                else
                {
                    wordList.Save();
                    break;
                }
            }
        }

        private static void PrintError()
        {
            Console.WriteLine("Use any of the following parameters:");
            Console.WriteLine("-lists");
            Console.WriteLine("-new <list name> <language 1> <language 2> .. <language n>");
            Console.WriteLine("-add <list name>");
            Console.WriteLine("-remove <list name> <language> <word 1> <word 2> .. <word n>");
            Console.WriteLine("-words <listname> <sortByLanguage>");
            Console.WriteLine("-count <listname>");
            Console.WriteLine("-practice <listname>");
            Console.ReadLine();
        }
    }

    public class PracticeResults
    {
        public string Word { get; set; }
        public string TranslationFrom { get; set; }
        public string TranslationTo { get; set; }
        public string ExpectedAnswer { get; set; }
        public string Answer { get; set; }

        public bool IsCorrect => ExpectedAnswer.ToLower() == Answer.ToLower();
    }
}

