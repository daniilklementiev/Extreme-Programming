using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public class RomanNumber
    {
		public int Value { get; set; } 
		
		public RomanNumber(int value = 0)
        {
			Value = value;
        }

		public override string ToString()
		{
			if (Value == 0)
			{
				return "N";
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
		    return Value < 0 ? $"-{result}" : result.ToString();
		}

		private static Dictionary<char, int> romanValues = new Dictionary<char, int>
		{
			{ 'I', 1 },
			{ 'V', 5 },
			{ 'X', 10 },
			{ 'L', 50 },
			{ 'C', 100 },
			{ 'D', 500 },
			{ 'M', 1000 },
			{ 'N', 0 },
		};

       

		public static RomanNumber Parse(String input)
		{
			input = input?.Trim();
			if (String.IsNullOrEmpty(input))
			{
				throw new ArgumentException("Empty or NULL input");
			}
			int result = 0;
			int prev = 0;
			int firstDigitIndex = input.StartsWith("-") ? 1 : 0;
			for (int i = input.Length - 1; i >= firstDigitIndex; i--)
			{
				int current = input[i] switch
				{
					'I' => 1,
					'V' => 5,
					'L' => 50,
					'X' => 10,
					'C' => 100,
					'D' => 500,
					'M' => 1000,
					'N' => 0,
					_ => throw new ArgumentException($"'{input}' parse error: Invalid digit '{input[i]}' ")
				}; ;
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
