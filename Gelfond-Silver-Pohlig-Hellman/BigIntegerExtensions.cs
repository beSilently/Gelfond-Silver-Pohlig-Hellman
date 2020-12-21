using System.Numerics;

namespace GSPH
{
    public static class BigIntegerExtensions
    {

        public static BigInteger ModInverse(this BigInteger a, BigInteger m)
        {
            var aInverse = BigInteger.ModPow(a, m - 2, m);
            return aInverse < 0 ? aInverse + m : aInverse;
        }
    }
}
