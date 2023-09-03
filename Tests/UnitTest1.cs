using App;

namespace Tests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestRomanNumber1Parse()
		{
			Assert.AreEqual(               // Элемент теста - утверждение (ассерт)
				1,                         // ожидаемое значение
				RomanNumber                //
				.Parse("I")                // значение которое фактически верное
				.Value,                    // 
				"1 == I"                   // сообщение в случае провала теста
				);                         // Тест проверяет все ли утверждения верны 
			Assert.AreEqual(
				2,
				RomanNumber
				.Parse("II")
				.Value,
				"2 == II"
				);

		}

	}
}