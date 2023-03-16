namespace Glossary_Library
{
    public class Word
    {
        public Word(params string[] translations)
        {
            Translations = translations;
        }

        public Word(int fromLanguage, int toLanguage, params string[] translations)
        {
            FromLanguage = fromLanguage;
            ToLanguage = toLanguage;
            Translations = translations;
        }

        public string[] Translations { get; }
        public int FromLanguage { get; }
        public int ToLanguage { get; }
    }
}
