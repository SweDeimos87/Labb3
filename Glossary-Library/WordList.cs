using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Glossary_Library
{
    public class WordList
    {
        public List<Word> WordsList { get; } = new List<Word>();
        public string Name { get; }
        public string[] Languages { get; }

        public WordList(string name, params string[] languages)
        {
            Name = name;
            Languages = languages;
        }

        public static string[] GetLists()
        {
            var applicationDataPath = GetApplicationDataPath();
            var files = Directory.GetFiles(applicationDataPath, "*.dat")
                                 .Select(Path.GetFileNameWithoutExtension)
                                 .ToArray();
            return files;
        }

        private static string GetApplicationDataPath()
        {
            var applicationName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var applicationDataPath = Path.Combine(basePath, applicationName);

            if (!Directory.Exists(applicationDataPath))
            {
                Directory.CreateDirectory(applicationDataPath);
            }

            return applicationDataPath;
        }

        public static WordList LoadList(string name)
        {
            var applicationDataPath = GetApplicationDataPath();
            var dictionaryPath = Path.Combine(applicationDataPath, name + ".dat");
            if (!File.Exists(dictionaryPath))
            {
                return null;
            }

            var fileData = File.ReadAllLines(dictionaryPath);
            var languages = fileData.First().Split(';');
            var wordList = new WordList(name, languages);
            foreach (var line in fileData.Skip(1))
            {
                var translations = line.Split(';');
                wordList.Add(translations);
            }

            return wordList;
        }

        public void Save()
        {
            var data = new List<string> { string.Join(";", Languages) };
            data.AddRange(WordsList.Select(word => string.Join(";", word.Translations)));

            var applicationDataPath = GetApplicationDataPath();
            var dictionaryPath = Path.Combine(applicationDataPath, Name + ".dat");

            File.WriteAllLines(dictionaryPath, data);
        }

        public void Add(params string[] translations)
        {
            var word = new Word(translations);
            WordsList.Add(word);
        }

        public bool Remove(int translation, string word)
        {
            foreach (var item in WordsList)
            {
                if (item.Translations[translation] == word)
                {
                    item.Translations[translation] = "";
                    return true;
                }
            }

            return false;
        }

        public int Count()
        {
            return WordsList.Count;
        }

        public void List(int sortByTranslation, Action<string[]> showTranslations)
        {
            var dictionary = WordsList
                .Where(word => word.Translations.Length > sortByTranslation)
                .ToLookup(word => word.Translations[sortByTranslation],
                          word => word.Translations);
            var lst = dictionary.Select(group => group.Key).OrderBy(key => key).ToArray();
            showTranslations(lst);
        }

        public Word GetWordToPractice()
        {
            var random = new Random();
            var number = random.Next(0, WordsList.Count);
            return WordsList[number];
        }
    }
}