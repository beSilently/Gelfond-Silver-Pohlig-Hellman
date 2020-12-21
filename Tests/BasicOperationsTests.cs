using System;
using System.Numerics;
using GSPH;
using NUnit.Framework;

namespace Tests
{
    public class BasicOperatonsTest
    {
        [Test]
        public void IsOnCurve_PointOnCurve_True()
        {
            EllipticCurve ECC = new EllipticCurve(1, 9, pField: 97, groupOrder: 90);
            Point P = new Point(19, 0);

            Assert.IsTrue(ECC.IsPointOnCurve(P));
        }

        [Test]
        public void IsOnCurve_PointNotOnCurve_False()
        {
            EllipticCurve ECC = new EllipticCurve(1, 9, pField: 97, groupOrder: 90);
            Point P = new Point(19, 5);

            Assert.IsFalse(ECC.IsPointOnCurve(P));
        }

        [Test]
        public void AddPoints_PointsOnCurve_CorrectResult()
        {
            EllipticCurve ECC = new EllipticCurve(1, 9, pField: 97, groupOrder: 90);
            Point P = new Point(19, 0);
            Point Q = new Point(22, 3);
            Point expected = new Point(57, 59);

            Point actual = ECC.AddPoints(P, Q);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AddPoints_SamePointOnCurve_CorrectResult()
        {
            EllipticCurve ECC = new EllipticCurve(1, 9, pField: 97, groupOrder: 90);
            Point P = new Point(22, 3);

            Point expected = new Point(10, 90);

            Point actual = ECC.AddPoints(P, P);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AddPoints_SamePointOnCurve_InfResult()
        {
            EllipticCurve ECC = new EllipticCurve(1, 9, pField: 97, groupOrder: 90);
            Point P = new Point(19, 0);

            Assert.IsNull(ECC.AddPoints(P, P));
        }

        [Test]
        public void MultiplyPoint_PointOnCurve_CorrectResult()
        {
            EllipticCurve ECC = new EllipticCurve(1, 9, pField: 97, groupOrder: 90);
            Point P = new Point(34, 16);
            Point expected = new Point(19, 0);

            Point actual = ECC.MultiplyPoint(P, 45);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void NegativePoint_PointOnCurve_CorrectResult()
        {
            EllipticCurve ECC = new EllipticCurve(1, 9, pField: 97, groupOrder: 90);
            Point P = new Point(10, 7);

            Point expected = new Point(10, 90);

            Point actual = ECC.GetNegativePoint(P);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ModInverse_CorrectResult()
        {
            BigInteger expected = 32;

            BigInteger actual = new BigInteger(-3).ModInverse(97);

            Assert.AreEqual(expected, actual);
        }
    }
}
