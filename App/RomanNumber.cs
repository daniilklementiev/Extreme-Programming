using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public class RomanNumber
    {
		private const char   ZERO_DIGIT = 'N';
		private const char   MINUS_SIGN = '-';
		private const char   PLUS_SIGN  = '+';
		private const int    OPERANDS_COUNT = 2;
		private const string INVALID_DIGIT_MESSAGE = "Parse error. Invalid digits:";
		private const string EMPTY_INPUT_MESSAGE = "Empty or NULL input";
		private const string INVALID_STRUCTURE_MESSAGE = "Invalid roman number structure";
		private const string INVALID_DIGIT_FORMAT = "{0} '{1}'";
		private const string INVALID_DIGITS_FORMAT = "Pase error '{0}'. Ivalid digit(s): '{1}'";
		private const string PLUS_NULL_ARGUMENT_MESSAGE = "Illegal Plus() invocation with wull argument";
		private const string MINUS_NULL_ARGUMENT_MESSAGE = "Illegal Minus() invocation with wull argument";
		private static String InvalidDigitsFormatMessage(List<char> invalidChars) => String.Join(",", invalidChars.Select(DigitDecorator));
		private static String DigitDecorator(char c) => $"'{c}'";

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
					String.Format(
						INVALID_DIGITS_FORMAT,
						input,
						// String.Join(", ", invalidChars.Select(DigitDecorator))
						InvalidDigitsFormatMessage(invalidChars)
					));
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

		public RomanNumber Plus(RomanNumber other)
		{
			if (other is null)
			{
				throw new ArgumentNullException(PLUS_NULL_ARGUMENT_MESSAGE);
			}
			return new(this.Value + other.Value);
		}

		public RomanNumber Minus(RomanNumber other)
		{
			if (other is null)
			{
				throw new ArgumentNullException(MINUS_NULL_ARGUMENT_MESSAGE);
			}
			return new(this.Value - other.Value);
		}

		public static RomanNumber Sum(params RomanNumber[] numbers)
		{
			if (numbers == null!)
			{
				return null!;
			}

			if (numbers.Length > 0)
			{
				int nullableNumbersCount = 0;
				foreach (var number in numbers)
				{
					if (number == null!)
					{
						nullableNumbersCount++;
					}
				}

				if (nullableNumbersCount == numbers.Length)
				{
					return null!;
				}
			}

			return new(numbers.Sum(number => number?.Value ?? 0));
		}



		// TODO: REFACTORING
		public static RomanNumber Eval(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				throw new ArgumentNullException(EMPTY_INPUT_MESSAGE);
			}

			string trimmedInput = input.Trim();
			if (trimmedInput.Length > 1)
			{
				if (trimmedInput.Contains(PLUS_SIGN))
				{
					string[] res = trimmedInput.Split(PLUS_SIGN, OPERANDS_COUNT);

					if (res.Length == 0) { return null!; }
					if (res.Length == 1)
					{
						return RomanNumber.Parse(res[0]);
					}
					int firstOp = RomanNumber.Parse(res[0]).Value;
					int secondOp = RomanNumber.Parse(res[1]).Value;
					return new(firstOp + secondOp);
				}
				else
				{
					if (trimmedInput.StartsWith(MINUS_SIGN))
					{
						int i2 = trimmedInput.IndexOf(MINUS_SIGN, 1);
						if (i2 != -1)
						{
							// 0 to i2 - 1st; i2 to end - 2nd
							string op1 = (string)trimmedInput.Substring(0, i2);
							string op2 = (string)trimmedInput.Substring(i2 + 1);
							return new(RomanNumber.Parse(op1).Value - RomanNumber.Parse(op2).Value);
						}
						else
						{
							return RomanNumber.Parse(trimmedInput);
						}
					}
					else
					{
						// X - -X = X -X
						string[] resStr = trimmedInput.Split(MINUS_SIGN, OPERANDS_COUNT);
						int firstOp = RomanNumber.Parse(resStr[0]).Value;
						int secondOp = RomanNumber.Parse(resStr[1]).Value;
						int result = firstOp - secondOp;
						return new RomanNumber(result);
					}
				}
			}
			else
			{
				return new RomanNumber(RomanNumber.Parse(trimmedInput).Value);
			}
		}


	}
}
