using App;

namespace Tests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestRomanNumber1Parse()
		{
			Assert.AreEqual(               // ������� ����� - ����������� (������)
				1,                         // ��������� ��������
				RomanNumber                //
				.Parse("I")                // �������� ������� ���������� ������
				.Value,                    // 
				"1 == I"                   // ��������� � ������ ������� �����
				);                         // ���� ��������� ��� �� ����������� ����� 
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