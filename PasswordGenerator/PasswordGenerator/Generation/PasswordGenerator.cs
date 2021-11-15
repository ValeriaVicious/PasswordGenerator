using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordGenerator.Generation
{
    public class StringGenerator
    {
        private static readonly char[] LowerCaseCharactersAlphabet;
        private static readonly char[] UpperCaseCharactersAlphabet;
        private static readonly char[] DigitsAlphabet;
        private static readonly char[] SpecialSymbolsAlphabet;

        private static readonly int TotalAlphabetsLength;



        private readonly StringBuilder _result;
        private readonly StringBuilder _alphabet;
        private readonly Random _randomGenerator;



        static StringGenerator()
        {
            LowerCaseCharactersAlphabet =
                GetAlphabet(new (char start, char end)[]
                {
                    ('a', 'z')
                }).ToArray();
            UpperCaseCharactersAlphabet =
                GetAlphabet(new (char start, char end)[]
                {
                    ('A', 'Z')
                }).ToArray();
            DigitsAlphabet =
                GetAlphabet(new (char start, char end)[]
                {
                    ('0', '9')
                }).ToArray();
            SpecialSymbolsAlphabet =
                new[]
                {
                    '?',
                    '!',
                    '@',
                    '#',
                    '$',
                    '%',
                    '^',
                    '&',
                    '*',
                };

            TotalAlphabetsLength = LowerCaseCharactersAlphabet.Length
                                   + UpperCaseCharactersAlphabet.Length
                                   + DigitsAlphabet.Length
                                   + SpecialSymbolsAlphabet.Length;
        }

        public StringGenerator(int maxLength)
        {
            _result = new StringBuilder(maxLength);
            _alphabet = new StringBuilder(TotalAlphabetsLength);
            _randomGenerator = new Random();
        }



        private static IEnumerable<char> GetAlphabet(
            int count, int startPosition = 0)
        {
            if (count <= 0)
                return Enumerable.Empty<char>();

            if (startPosition < 0)
                startPosition = 0;

            var result = new char[count];

            int i = startPosition;
            int foundedCount = 0;

            do
            {
                char ch = (char)i;

                if (!char.IsControl(ch)
                    && !char.IsWhiteSpace(ch))
                {
                    result[foundedCount] = ch;
                    ++foundedCount;
                }

                ++i;
            }
            while (foundedCount < count);

            return result;
        }
        private static IEnumerable<char> GetAlphabet(
            (char start, char end)[] intervals)
        {
            var result = new List<char>(100);

            if (intervals == null || intervals.Length == 0)
                return result;

            foreach (var (start, end) in intervals)
            {
                if (start > end)
                    continue;

                if (start == end)
                {
                    result.Add(start);
                    continue;
                }

                var range = new List<char>(end - start + 1);

                for (var ch = start; ch <= end; ++ch)
                {
                    range.Add(ch);
                }

                result.AddRange(range);
            }

            return result;
        }



        public string GeneratePassword(
            int length,
            PasswordGenerationAlphabet alphabets)
        {
            bool HasAlphabet(PasswordGenerationAlphabet alphabet)
            {
                return (alphabets & alphabet) == alphabet;
            }



            if (HasAlphabet(PasswordGenerationAlphabet.LowerCaseCharacters))
                _alphabet.Append(LowerCaseCharactersAlphabet);
            if (HasAlphabet(PasswordGenerationAlphabet.UpperCaseCharacters))
                _alphabet.Append(UpperCaseCharactersAlphabet);
            if (HasAlphabet(PasswordGenerationAlphabet.Digits))
                _alphabet.Append(DigitsAlphabet);
            if (HasAlphabet(PasswordGenerationAlphabet.SpecialCharacters))
                _alphabet.Append(SpecialSymbolsAlphabet);

            if (_alphabet.Length == 0)
                return string.Empty;

            for (int i = 0; i < length; ++i)
            {
                var alphabetCharIndex = _randomGenerator
                    .Next(_alphabet.Length);

                _result.Append(
                    _alphabet[alphabetCharIndex]);
            }

            var resultString = _result.ToString();

            _alphabet.Clear();
            _result.Clear();

            return resultString;
        }
    }
}
