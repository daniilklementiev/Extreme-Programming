using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public class RomanNumber
    {
		private const char   ZERO_DIGIT = 'N';
		private const string MINUS_SIGN = "-";
		private const string INVALID_DIGIT_MESSAGE = "Parse error. Invalid digits:";
		private const string EMPTY_INPUT_MESSAGE = "Empty or NULL input";
		private const string INVALID_STRUCTURE_MESSAGE = "Invalid roman number structure";
		private const string INVALID_DIGIT_FORMAT = "{0} '{1}'";
		private const string INVALID_DIGITS_FORMAT = "Pase error '{0}'. Ivalid digit(s): '{1}'";

		public int Value { get; set; } 
		
		public RomanNumber(int value = 0)
        {
			Value = value;
        }

		public override string ToString()
		{
			if (Value == 0)
			{
				return ZERO_DIGIT.ToString();
			}

			Dictionary<int, String> ranges = new()
			{
				{ 1000, "M"  },
				{ 900,  "CM" },
				{ 500,  "D"  },
				{ 400,  "CD" },
				{ 100,  "C"  },
				{ 90,   "XC" },
				{ 50,   "L"  },
				{ 40,   "XL" },
				{ 10,   "X"  },
				{ 9,    "IX" },
				{ 5,    "V"  },
				{ 4,    "IV" },
				{ 1,    "I"  },
			};
			StringBuilder result = new();
			int value = Math.Abs(Value);
			
		    foreach (var range in ranges)
			{
				while (value >= range.Key)
				{
					result.Append(range.Value);
					value -= range.Key;
				}
			}
		    return Value < 0 ? $"{MINUS_SIGN}{result}" : result.ToString();
		}
		

		private static int DigitValue(char digit)
		{
			return digit switch               
			{                                  
				'I' => 1,                      
				'V' => 5,                      
				'L' => 50,                     
				'X' => 10,                     
				'C' => 100,                    
				'D' => 500,                    
				'M' => 1000,                   
				ZERO_DIGIT => 0,                      
				_ => throw new ArgumentException(   // $"{INVALID_DIGIT_MESSAGE} '{digit}' "
				    String.Format(
						INVALID_DIGIT_FORMAT, 
						INVALID_DIGIT_MESSAGE, 
						digit)
					)
			};
		}


		private static void CheckValidityOrThrow(String input)
		{
			#region Проверка числа на недопустимые символы

			if (String.IsNullOrEmpty(input))
			{
				throw new ArgumentException(EMPTY_INPUT_MESSAGE);
			}
			int firstDigitIndex = input.StartsWith(MINUS_SIGN) ? 1 : 0;
			List<char> invalidChars = new();
			for (int i = input.Length - 1; i >= firstDigitIndex; i--)
			{
				try 
				{ 
					DigitValue(input[i]); 
				}
				catch
				{ 
					invalidChars.Add(input[i]);
				}
			}
			if (invalidChars.Count > 0)
			{
				throw new ArgumentException(
					// $"'{input}' {INVALID_DIGIT_MESSAGE} '{String.Join(", ", invalidChars.Select(c => $"'{c}'"))}' "
					String.Format(
						INVALID_DIGITS_FORMAT, 
						input, 
						String.Join(", ", invalidChars.Select(c => $"'{c}'"))
					)
					);
			}

			#endregion
		}

		private static void CheckCompositionOrThrow(String input)
		{
			#region Проверка "легальности" - правильности композиции числа
			int maxDigit = 0;
			bool flag = false;
			int firstDigitIndex = input.StartsWith(MINUS_SIGN) ? 1 : 0;
			for (int i = input.Length - 1; i >= firstDigitIndex; i--)
			{
				int current = DigitValue(input[i]);                  
				if (current > maxDigit)
				{
					maxDigit = current;
				}
				if (current < maxDigit)
				{
					if (flag)
					{
						throw new ArgumentException(INVALID_STRUCTURE_MESSAGE);
					}
					flag = true;
				}
				else
				{
					flag = false;
				}
			}
			#endregion
		}

		public static RomanNumber Parse(String input)
		{
			// предварительная обработка - удаление артефактов ввода
			input = input?.Trim();
			CheckValidityOrThrow(input);
			CheckCompositionOrThrow(input);

			int firstDigitIndex = input.StartsWith(MINUS_SIGN) ? 1 : 0;
			int result = 0;
			int prev = 0;
			for (int i = input.Length - 1; i >= firstDigitIndex; i--)
			{
				int current = DigitValue(input[i]);
				result += (current < prev) ? -current : current;
				prev = current;
			}

			return new()
			{
				Value = firstDigitIndex == 0 ? result : -result
				/*Value = result * (1 - (firstDigitIndex << 1))*/
			};
		}
	}
}
