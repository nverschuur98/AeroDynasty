using AeroDynasty.Models.AirlinerModels;
using AeroDynasty.Services;
using System.Diagnostics.Metrics;

namespace AeroDynasty.Models.Core
{
    public class Registration
    {
        public int Number { get; private set; }
        private RegistrationTemplate _template {  get; set; }
        public Country Country { get {  return _template.Country; } }
        public string ReturnValue { get { return returnValue(); } }

        /// <summary>
        /// Build a registration for the airliner
        /// </summary>
        /// <param name="country">The country the airliner is based in</param>
        public Registration(Country country)
        {
            _template = GetTemplateForCountry(country);

            // Check if the GameData is ready before accessing
            if (GameData.Instance == null || GameData.Instance.Airliners == null)
            {
                throw new InvalidOperationException("GameData or Airliners is not initialized.");
            }

            Number = returnNextRegistrationNumber(Country);
        }

        /// <summary>
        /// Build a registration for the airliner
        /// </summary>
        /// <param name="country">The country the airliner is based in</param>
        /// <param name="number">The registration number</param>
        public Registration(Country country, int number)
        {
            _template = GetTemplateForCountry(country);
            Number = number;
        }

        /// <summary>
        /// Return the registration template based on the country
        /// </summary>
        /// <param name="country">The country of which to get the template from</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private RegistrationTemplate GetTemplateForCountry(Country country)
        {
            var templateEntry = GameData.Instance.RegistrationTemplates
                .FirstOrDefault(reg => reg.Country == country);

            if (templateEntry == null)
            {
                throw new Exception($"No registration template found for country: {country}");
            }

            return templateEntry;
        }

        /// <summary>
        /// Based on the country, return the next available registration number
        /// </summary>
        /// <param name="country">Country for which to give a registration number</param>
        /// <returns></returns>
        private int returnNextRegistrationNumber(Country country)
        {
            //Set a default for if no airliner exists yet
            int num = 1;

            //Check in the GameData for the specified country what the latest number is
            if (GameData.Instance.Airliners != null && GameData.Instance.Airliners.Any())
            {
                var airliners = GameData.Instance.Airliners.Where(air => air.Registration.Country == country).ToList();
                // Proceed with your logic using airliners

                int highestNumber = airliners.OrderByDescending(air => air.Registration.Number).FirstOrDefault().Registration.Number;

                //Add one to the number
                num = ++highestNumber;
            }

            //Return the Number value
            return num;
        }

        /// <summary>
        /// Convert a number representation to a letter representation
        /// </summary>
        /// <param name="number">the value</param>
        /// <param name="length">the pattern length</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private string NumberToLetters(int number)
        {
            if (number < 1 || number > Math.Pow(26, _template.Format.Length))
            {
                throw new ArgumentOutOfRangeException(nameof(number), $"Input must be between 1 and {Math.Pow(26, _template.Format.Length)}.");
            }

            number--; // Adjust to make 1 = 'A'... and not 'B'...

            char[] letters = new char[_template.Format.Length];

            for (int i = _template.Format.Length - 1; i >= 0; i--)
            {
                letters[i] = (char)('A' + number % 26);
                number /= 26;
            }

            return new string(letters);
        }

        /// <summary>
        /// Convert a number representation to a string representation
        /// </summary>
        /// <param name="number">the value</param>
        /// <returns></returns>
        private string NumberToString(int number)
        {
            string temp = number.ToString();

            for (int i = temp.Length; i < _template.Format.Length; ++i)
            {
                temp = "0" + temp;
            }

            return temp;
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

        /// <summary>
        /// Returns the registration
        /// </summary>
        /// <returns>String value of registration</returns>
        private string returnValue()
        {
            if (_template.IsLetterFormat)
            {
                return $"{_template.CountryID}-{NumberToLetters(Number)}";
            }
            else
            {
                return $"{_template.CountryID}-{NumberToString(Number)}";
            }
        }
    }

    public class RegistrationTemplate
    {
        private string _countryID { get; set; }
        private bool _separator { get; set; }
        private string _format { get; set; }
        private Country _country { get; set; }

        public RegistrationTemplate(string countryID, bool separator, string format, Country country)
        {
            _countryID = countryID;
            _separator = separator;
            _format = format;
            _country = country;
        }

        public Country Country  { get { return _country; } }
        public string CountryID { get { return _countryID; } }
        public bool Separator { get { return _separator; } }
        public string Format { get { return _format; } }
        public bool IsLetterFormat
        {
            get
            {
                if (_format.Contains('A'))
                {
                    return true;
                }
                return false;
            }
        }
    }
}
