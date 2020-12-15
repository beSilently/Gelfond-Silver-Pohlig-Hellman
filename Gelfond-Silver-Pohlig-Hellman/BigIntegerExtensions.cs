using System.Numerics;

namespace GSPH
{
    public static class BigIntegerExtensions
    {
        public static BigInteger ModInverse(this BigInteger a, BigInteger m)
        {
            if (m == 1) return 0;
            var m0 = m;
            (BigInteger x, BigInteger y) = (1, 0);

            if (a < 0) return m - ModInverse(-a, m);

            while (a > 1)
            {
                var q = a / m;
                (a, m) = (m, a % m);
                (x, y) = (y, x - q * y);
            }
            return x < 0 ? x + m0 : x;
        }
    }
}
