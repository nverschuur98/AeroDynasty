namespace AeroDynasty.Models.Core
{
    public class Registration
    {
        private string _countryID {  get; set; }
        public string Number { get; private set; }
        private string _format { get; set; }
        public Country Country { get; private set; }
        public string Value {  get; private set; }

        public Registration()
        {
            _countryID = "PH";
            _number = "";
            _format = "AAAA";

            string test = "ZZZ";
            int num = LettersToNumber(test);
            test = NumberToLetters(num, 3);
            
        }

        private string returnNextRegistration()
        {
            string temp = _format;
            bool letterFormat = false;
            int length;
            int count = 100;

            if (_format.Contains('A'))
            {
                letterFormat = true;
                temp = _format.Replace('A', '0');
            }

            length = temp.Length;

            count = Convert.ToInt32(temp);
            count++;

            if (letterFormat)
            {
                temp = NumberToLetters(count, length);
            }
            else
            {
                temp = count.ToString();

                for (int i = temp.Length; i < length; ++i)
                {
                    temp = '0' + temp;
                }

            }
            return temp;
        }

        /// <summary>
        /// Convert a number representation to a letter representation
        /// </summary>
        /// <param name="number">the value</param>
        /// <param name="length">the pattern length</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private string NumberToLetters(int number, int length)
        {
            if (number < 1 || number > Math.Pow(26, length))
            {
                throw new ArgumentOutOfRangeException(nameof(number), $"Input must be between 1 and {Math.Pow(26, length)}.");
            }

            number--; // Adjust to make 1 = 'A'... and not 'B'...

            char[] letters = new char[length];

            for (int i = length - 1; i >= 0; i--)
            {
                letters[i] = (char)('A' + number % 26);
                number /= 26;
            }

            return new string(letters);
        }

        /// <summary>
        /// Convert a letter representation (e.g., "AAA") back to a number (e.g., 1 for AAA).
        /// </summary>
        /// <param name="letters">The string of letters</param>
        /// <returns>The corresponding number</returns>
        public int LettersToNumber(string letters)
        {
            int result = 0;
            int length = letters.Length;

            // Process each character in the string from left to right
            for (int i = 0; i < length; i++)
            {
                char c = letters[i];

                // Ensure the character is between 'A' and 'Z'
                if (c < 'A' || c > 'Z')
                {
                    throw new ArgumentOutOfRangeException(nameof(letters), "All characters must be between 'A' and 'Z'.");
                }

                // Convert the character to its corresponding value (A=0, B=1, ..., Z=25)
                result = result * 26 + (c - 'A');
            }

            // Add 1 to make "AAA" = 1 instead of 0 (because it's zero-indexed)
            return result + 1;
        }



        private string returnValue()
        {
            return $"{_countryID}-{_number}";
        }
    }
}
