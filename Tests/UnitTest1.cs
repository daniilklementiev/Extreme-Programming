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
			Assert.AreEqual(               
				1,                         
				RomanNumber                
				.Parse("I")                
				.Value,                    
				"1 == I"                   
				);                          
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
			
			Assert.IsTrue(
				ex.Message.Contains("Invalid digit", StringComparison.OrdinalIgnoreCase),
				"ex.Message Contains 'Invalid digit' "
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
			    {2023, "MMXXIII"},
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


		[TestMethod]
		public void TypesFeature()
		{
			RomanNumber r = new(10);
			Assert.AreEqual(10L, r.Value); // 10u - uint, r.Value - int -- fail

			Assert.AreEqual((short)10, r.Value);
		}

		[TestMethod]
		public void TestPlus()
		{
			RomanNumber r1 = new(10);
			RomanNumber r2 = new(20);
			var r3 = r1.Plus(r2);
			Assert.IsInstanceOfType(r1.Plus(r2), typeof(RomanNumber));
			Assert.AreNotSame(r1, r3);
			Assert.AreNotSame(r2, r3);

			Assert.AreEqual(30, r3.Value);
			Assert.AreEqual("XXX", r3.ToString());

			Assert.AreEqual(15, r1.Plus(new(5)).Value);
			Assert.AreEqual(1, r1.Plus(new(-9)).Value);
			Assert.AreEqual(-1, r1.Plus(new(-11)).Value);
			Assert.AreEqual(0, r1.Plus(new(-10)).Value);
			Assert.AreEqual(10, r1.Plus(new()).Value);

			Assert.AreEqual(14, RomanNumber.Parse("IV").Plus(new(10)).Value);
			Assert.AreEqual(-10, RomanNumber.Parse("-V").Plus(new(-5)).Value);

			Assert.AreEqual("N", new RomanNumber(41).Plus(new(-41)).ToString());
			Assert.AreEqual("-II", new RomanNumber(-32).Plus(new(30)).ToString());

			var ex = Assert.ThrowsException<ArgumentNullException>(() => r1.Plus(null!),
				"Plus(null!) -> ArgumentNullException"
			);

			String expectedFragment = "Illegal Plus() invocation with wull argument";
			Assert.IsTrue(ex.Message.Contains(expectedFragment,
					StringComparison.InvariantCultureIgnoreCase
				),
				$"Plus(null!): ex.Message ({ex.Message}) contains '{expectedFragment}'"
			);

		}

		[TestMethod]
		public void TestMinus()
		{
			RomanNumber r1 = new(10);
			RomanNumber r2 = new(20);
			var r3 = r1.Minus(r2);
			Assert.IsInstanceOfType(r1.Minus(r2), typeof(RomanNumber));
			Assert.AreNotSame(r1, r3);
			Assert.AreNotSame(r2, r3);

			Assert.AreEqual(-10, r3.Value);
			Assert.AreEqual("-X", r3.ToString());

			Assert.AreEqual(5, r1.Minus(new(5)).Value);
			Assert.AreEqual(19, r1.Minus(new(-9)).Value);
			Assert.AreEqual(21, r1.Minus(new(-11)).Value);
			Assert.AreEqual(20, r1.Minus(new(-10)).Value);
			Assert.AreEqual(10, r1.Minus(new()).Value);
			Assert.AreEqual(0, r1.Minus(new(10)).Value);

			Assert.AreEqual(4, RomanNumber.Parse("IX").Minus(new(5)).Value);
			Assert.AreEqual(6, RomanNumber.Parse("-V").Minus(new(-11)).Value);

			Assert.AreEqual("XL", new RomanNumber(20).Minus(new(-20)).ToString());
			Assert.AreEqual("N", new RomanNumber(20).Minus(new(20)).ToString());

			var ex = Assert.ThrowsException<ArgumentNullException>(() => r1.Minus(null!),
				"Minus(null!) -> ArgumentNullException"
			);

			String expectedFragment = "Illegal Minus() invocation with wull argument";
			Assert.IsTrue(ex.Message.Contains(expectedFragment,
					StringComparison.InvariantCultureIgnoreCase
				),
				$"Minus(null!): ex.Message ({ex.Message}) contains '{expectedFragment}'"
			);

		}

		[TestMethod]
		public void TestSum()
		{
			RomanNumber r1 = new(10);
			RomanNumber r2 = new(20);

			var r3 = RomanNumber.Sum(r1, r2);
			Assert.IsInstanceOfType(r3, typeof(RomanNumber));

			Assert.AreEqual(60, RomanNumber.Sum(r1, r2, r3).Value);

			Assert.AreEqual(0, RomanNumber.Sum().Value);

			Assert.IsNull(RomanNumber.Sum(null!));

			Assert.AreEqual(40, RomanNumber.Sum(r1, null!, r3).Value);

			var arr1 = Array.Empty<RomanNumber>();
			var arr2 = new RomanNumber[] { new(2), new(4), new(5) };
			Assert.AreEqual(0, RomanNumber.Sum(arr1).Value, "Empty arr --> Sum 0");
			Assert.AreEqual(11, RomanNumber.Sum(arr2).Value, "2-4-5 arr --> Sum 11");

			IEnumerable<RomanNumber> arr3 = new List<RomanNumber>() { new(2), new(4), new(5) };
			Assert.AreEqual(11, RomanNumber.Sum(arr3.ToArray()).Value, "2-4-5 list --> Sum 11");

			var arr4 = new RomanNumber[] { null!, null!, null! };
			Assert.AreEqual(null, RomanNumber.Sum(arr4), "null! + null! + null! = null");


			Random rnd = new();
			for (int i = 0; i < 200; i++)
			{
				int x = rnd.Next(-2000, 2000);
				int y = rnd.Next(-2000, 2000);
				Assert.AreEqual(x + y,RomanNumber.Sum(new(x), new(y)).Value,$"{x} + {y} random_t");
			}

			for (int i = 0; i < 200; i++)
			{
				RomanNumber rx = new(rnd.Next(-2000, 2000));
				RomanNumber ry = new(rnd.Next(-2000, 2000));
				Assert.AreEqual(rx.Plus(ry).Value, RomanNumber.Sum(rx, ry).Value, $"{rx} + {ry} random_t"
				);
			}
		}
		
		[TestMethod]
		public void TestEval()
		{
			RomanNumber input = RomanNumber.Eval("V+V"); // 10
			RomanNumber r1 = RomanNumber.Eval("-V-X");
			RomanNumber r2 = RomanNumber.Eval("-V - -X");
			RomanNumber r3 = RomanNumber.Eval("-V--X");
			RomanNumber r4 = RomanNumber.Eval("-V+-X");
			RomanNumber r5 = RomanNumber.Eval("V + -X");
			RomanNumber r6 = RomanNumber.Eval("N-I");
			RomanNumber r7 = RomanNumber.Eval("I-N");


			Assert.IsNotNull(RomanNumber.Eval("V+V")); // if not null
			Assert.IsNotNull(RomanNumber.Eval("V-V")); // if not null
			Assert.IsNotNull(RomanNumber.Eval("V")); // if not null
			Assert.IsInstanceOfType(RomanNumber.Eval("V+V"), typeof(RomanNumber));  // check type
			Assert.IsInstanceOfType(RomanNumber.Eval("V-V"), typeof(RomanNumber));  // check type
			Assert.AreEqual("X",   input.ToString(),            "Sum of input.ToString() === X"  ); // equal check
			Assert.AreEqual("-XV", r1.ToString(),    "Sum of input.ToString() === -XV"); // equal check
			Assert.AreEqual("V", r2.ToString(), "Sum of input.ToString() === -XV"); // equal check
			Assert.AreEqual("V", r3.ToString(),   "Sum of input.ToString() === -XV"); // equal check
			Assert.AreEqual("-I", r6.ToString(),   "Sum of input.ToString() === -XV"); // equal check
			Assert.AreEqual("I", r7.ToString(),  "Sum of input.ToString() === -XV"); // equal check
		    var ex = Assert.ThrowsException<ArgumentNullException>(() => RomanNumber.Eval(null!),"Evaluate `null!` throws ArgumentNullException");

			Random r = new();
			for (int i = 0; i < 100; i++)
			{
				RomanNumber o1 = new(r.Next(-5000, 5000));
				RomanNumber o2 = new(r.Next(-5000, 5000));
				RomanNumber res = RomanNumber.Eval($"{o1} + {o2}"); // evaluate string
				Assert.AreEqual(RomanNumber.Sum(o1, o2).Value, res.Value);
				Assert.AreEqual(o1.Plus(o2).Value, res.Value);
			}
		}
	}
}

/* Завдання до заліку
* Розробити метод обчислення виразів "Eval"
* Семантика:
*  RomanNumber res = RomanNumber.Eval("XL + II")
* Дозволені обмеження: 
*  операції дві - додавання та віднімання
*  аргументів - не більше двох, з одним аргументом - повертає
*   сам аргумент (у вигляді RomanNumber), з двома - результат
*   виразу
* Задачі:
* І.
*  - Складаємо тести, на першому етапі тільки для додаваня
*   = На коректні значення
*     - NotNull
*     - Type
*     - Algo
*      = реперні значення
*      = випадкове тестування
*     - Крос тестування
*      = з методом Plus
*      = з методом Sum
*  = На некоректні значення
*   - тип виключення
*   - повідомлення у виключенні
*   - винятки для порожних виразів
* ІІ. 
*  Реалізація
* ІІІ.
*  Те ж для віднімання
* IV.
*  Реалізація
* V.
*  Обговорення, перероблення, рефакторинг
*/