namespace FakeProductLibrary
{
    public class Calculator
    {
        public int Power(int baseNumber, int exponent)
        {
            int result = 1;
            while (exponent > 0)
            {
                if ((exponent & 1) != 0)
                    result *= baseNumber;
                exponent >>= 1;
                baseNumber *= baseNumber;
            }
            return result;
        }
    }
}
