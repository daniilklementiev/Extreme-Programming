using App;

namespace Tests
{
	[TestClass]
	public class RomanNumberTest
	{
		[TestMethod]
		public void TestParse()
		{
			Dictionary<String, int> testCases = new()
			{
				{"II", 2},
				{"III", 3},
				{"IIII", 4},
				{"IV", 4},
				{"IX", 9},
				{"LXII", 62},
				{"LXIII", 63},
				{"LXIV", 64},
				{"LXV", 65},
				{"LXVI", 66},
				{"LXVII", 67},
				{"LXXXI", 81},
				{"LXXXII", 82},
				{"LXXXIII", 83},
				{"LXXXIV", 84},
				{"LXXXV", 85},
				{"LXXXVI", 86},
				{"V", 5},
				{"VI", 6},
				{"VII", 7},
				{"VIII", 8},
				{"VIIII", 9},
				{"X", 10},
				{"XI", 11},
				{"XII", 12},
				{"XIII", 13},
				{"XIIII", 14},
				{"XIIIII", 15},
				{"XIV", 14},
				{"XL", 40},
				{"XLI", 41},
				{"XLII", 42},
				{"XLIII", 43},
				{"XLIV", 44},
				{"XLV", 45},
				{"XV", 15},
				{"XVI", 16},
				{"XVII", 17},
				{"XVIII", 18},
				{"XX", 20},
				{"XXIIIII", 25},
				{"XXV", 25},
				{"XXX", 30},
				{"C", 100 },
				{"D", 500 },
				{"M", 1000 },
				{"IM", 999 },
				{"CM", 900 },
				{"XM", 990 },
				{"MCM", 1900 },
				{"N", 0 },
				{"-XLI", -41 },
				{"-CLI", -151 },
				{"-IL", -49 },
				{"-XLIX", -49 }

			};
			Assert.AreEqual(               // Элемент теста - утверждение (ассерт)
				1,                         // ожидаемое значение
				RomanNumber                //
				.Parse("I")                // значение которое фактически верное
				.Value,                    // 
				"1 == I"                   // сообщение в случае провала теста
				);                         // Тест проверяет все ли утверждения верны 
			foreach (var pair in testCases)
			{
				Assert.AreEqual(pair.Value, RomanNumber.Parse(pair.Key).Value, $"{pair.Value} == {pair.Key}");
			}
		}

		[TestMethod]
		public void TestParseException()
		{

			Assert.ThrowsException<ArgumentException>(
				() => RomanNumber.Parse(null!),
				"RomanNumber.Parse(null!) -> Exception"
			);
			Assert.ThrowsException<ArgumentException>(
				() => RomanNumber.Parse(""),
				"RomanNumber.Parse('') -> Exception"
			);
			var ex = 
				Assert.ThrowsException<ArgumentException>(
					() => RomanNumber.Parse("  "),
					"RomanNumber.Parse('  ') -> Exception"
				);
			
			Assert.IsFalse(String.IsNullOrEmpty(ex.Message),
				"RomanNumber.Parse('') -- ex.Message not empty");

			Dictionary<String, char> testCases = new()
			{
				{ "XA", 'A' }, 
                { "LB", 'B' },
				{ "vI", 'v' },
				{ "1X", '1' },
				{ "$M", '$' },
				{ "mX", 'm' },
				{ "iM", 'i' }
			};
			foreach (var pair in testCases)
			{
				Assert.IsTrue(
					 Assert.ThrowsException<ArgumentException>(
						 () => RomanNumber.Parse(pair.Key),
						 $"RomanNumber.Parse({pair.Key}) -> Exception"
					 ) 
					 .Message.Contains($"'{pair.Value}'"),
					 $"RomanNumber.Parse({pair.Key}): ex.Message contains '{pair.Value}'"
				 );
			}
			String num = "MAM";  
			ex = Assert.ThrowsException<ArgumentException>(
				() => RomanNumber.Parse(num));
			
			Assert.IsTrue(ex.Message.Contains("Invalid digit", StringComparison.OrdinalIgnoreCase), "ex.Message Contains 'Invalid digit' "
			);
			Assert.IsTrue(
				ex.Message.Contains($"'{num}'"),
				$"ex.Message contains '{num}'"
			);

		}

		[TestMethod]
		public void TestParseInvalid()
		{
			Dictionary<String, char> testCases = new()
			{
				{ "X C", ' ' },
				{ "X\tC", '\t' },
				{ "X\nC", '\n' },
			};
			foreach (var pair in testCases)
			{
				Assert.IsTrue(
					 Assert.ThrowsException<ArgumentException>(
						 () => RomanNumber.Parse(pair.Key),
						 $"RomanNumber.Parse({pair.Key}) -> Exception"
					 ) 
					 .Message.Contains($"'{pair.Value}'"),
					 $"RomanNumber.Parse({pair.Key}): ex.Message contains '{pair.Value}'"
				 );
			}

			Dictionary<String, char[]> testCases2 = new()
			{
				{ "12XC",  new[] { '1', '2' } },
				{ "XC12",  new[] { '1', '2' } },
				{ "123XC", new[] { '1', '2', '3' } },
				{ "321X",  new[] { '3', '2', '1' } },
				{ "3V2C1X",  new[] { '3', '2', '1' } },
			};
			foreach (var pair in testCases2)
			{
				var ex =
					Assert.ThrowsException<ArgumentException>(
						() => RomanNumber.Parse(pair.Key),
						$"Roman number parse {pair.Key} --> Exception"
					);
				foreach (char c in pair.Value)
				{
					Assert.IsTrue(
						ex.Message.Contains($"'{c}'"),
						$"Roman number parse ({pair.Key}): ex.Message contains '{c}'"
					);
				}
			}
		}

		[TestMethod]
		public void TestParseDubious()
		{
			String[] dubious = { " XC", "XC", "XC\n", "\tXC", " XC " };
			foreach (var str in dubious)
			{
				Assert.IsNotNull(RomanNumber.Parse(str), $"Dubious '{str}' cause NULL");
			}
			int value = 90; // RomanNumber.Parse(dubious[0]).Value;
			foreach (var str in dubious)
			{
				Assert.AreEqual(value, RomanNumber.Parse(str).Value, $"Dubious equality '{str}' --> '{value}'");
			}

			
			String[] dubious2 = { "IIX", "VVX" };
			foreach (var str in dubious2)
			{
				// 'IIX', 'VVX' - правильные
				/*
				Assert.IsNotNull(RomanNumber.Parse(str), $"Dubious '{str}' cause NULL");*/

				// изменяются правила - 'IIX', 'VVX' - не правильные
				Assert.ThrowsException<ArgumentException>(() => RomanNumber.Parse(str), $"Dubious '{str}' cause Exception");

			}

			// '12XC' XC12' - неправильные. Цифр не должно быть. Пример:
			// Enter: 12XC
			// '12XC' Parse error: Invalid digit '1', '2'
		}

		[TestMethod]
		public void TestToString()
		{
			Dictionary<int, String> testCases = new()
			{
				{ 0,   "N"        },
				{ 1,   "I"        },
				{ 2,   "II"       },
				{ 4,   "IV"       },
				{ 9,   "IX"       },
				{ 19,  "XIX"      },
				{ 99,  "XCIX"     },
				{ 499, "CDXCIX"   },
				{ 999, "CMXCIX"   },
				{ -45, "-XLV"     },
				{-95,  "-XCV"     },
				{-285, "-CCLXXXV" },
				{ 1000, "M"       },
				{ 500, "D"        },
				{ 100, "C"        },
				{ 50, "L"         },
				{ 10, "X"         },
				{ 5, "V"          },
				{ 90, "XC"        },
				{ 40, "XL"        },
			    {2000, "MM"},
			    {1050, "ML"},
			    {1115, "MCXV"},
			    {1400, "MCD"},
			    {1935, "MCMXXXV"},
			    {2023, "MMXXIII"}
			};

			foreach (var testCase in testCases)
			{
				Assert.AreEqual(
					testCase.Value,
					new RomanNumber(testCase.Key).ToString(),
					$"{testCase.Key}.ToString() --> {testCase.Value}");
			}
		}

		[TestMethod]
		public void CrossTestParseToString()
		{
			for (int i = 0; i < 697; i++)
			{
				int rnd = new Random().Next(-5000, 5000);
				RomanNumber r = new(rnd);
				Assert.IsNotNull(r);
				Assert.AreEqual(
					rnd,
					RomanNumber.Parse(r.ToString()).Value,
					$"Parse('{r}'.ToString()) --> '{rnd}'");
			}
		}
	}
}