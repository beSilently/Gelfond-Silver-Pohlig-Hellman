using System;
using System.Collections.Generic;
using GSPH;
using NUnit.Framework;

namespace Tests
{
    public class MainTest
    {
        [Test]
        public void GelfodMethod_MainTest()
        {
            EllipticCurve ECC = new EllipticCurve(1, 9, pField: 97, groupOrder: 90);
            Console.WriteLine($"{ECC}, N = {ECC.N}, p = {ECC.p}");
            Point Q = new Point(34, 16);
            Console.WriteLine($"Q = {Q}");
            Point P = new Point(69, 40);
            Console.WriteLine($"P = {P}");

            Dictionary<int, int> divisors = new Dictionary<int, int>
            {
                { 2, 1 },
                { 3, 2 },
                { 5, 1 }
            };

            int expected = 34;

            int actual = GelfondMethod.GetLog(ECC, Q, P, divisors);

            Console.WriteLine($"l = {actual}(mod {ECC.N})");

            Assert.AreEqual(expected, actual);
        }
    }
}
