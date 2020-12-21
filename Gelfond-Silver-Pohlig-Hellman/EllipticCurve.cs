using System;
using System.Numerics;
namespace GSPH
{
    public class EllipticCurve
    {
        public int p { get; private set; }
        public int N { get; private set; }

        public int a { get; private set; }
        public int b { get; private set; }

        public EllipticCurve(int aCoef, int bConst, int pField, int groupOrder)
        {
            a = aCoef;
            b = bConst;

            p = pField;
            N = groupOrder;
        }

        public bool IsPointOnCurve(Point P)
        {
            if (P is null)
            {
                return true;
            }

            return ((P.Y * P.Y) - (P.X * P.X * P.X) - (a * P.X) - b) % p == 0;

        }

        public Point AddPoints(Point P, Point Q)
        {
            if (!IsPointOnCurve(P)) throw new ArgumentException($"Point {P} is not on curve {this}");
            if (!IsPointOnCurve(Q)) throw new ArgumentException($"Point {Q} is not on curve {this}");

            // The point at infinity is represented as null.
            if (P is null) return Q;
            if (Q is null) return P;

            BigInteger m;

            if (P.X != Q.X)
            {
                m = ((P.Y - Q.Y) * (P.X - Q.X).ModInverse(p)) % p;
            }
            else
            {
                if (P.Y == 0 && Q.Y == 0)
                {
                    // This may only happen if p1 = p2 is a root of the elliptic
                    // curve, hence the line is vertical.
                    return null;
                }
                else if (P.Y == Q.Y)
                {
                    // The points are the same, but the line is not vertical.
                    m = (((3 * P.X * P.X) + a) * (2 * P.Y).ModInverse(p)) % p;
                }
                else
                {
                    // The points are the same and the line is vertical.
                    return null;
                }
            }

            BigInteger x = (m * m - P.X - Q.X) % p;
            BigInteger y = (P.Y + m * (x - P.X)) % p;

            y = -y < 0 ? -y + p : -y;

            var result = new Point(x, y);

            if (!IsPointOnCurve(result)) throw new ArgumentException($"Point {result} is not on curve {this}");

            return result;
        }

        public Point DoublePoint(Point P)
        {
            return AddPoints(P, P);
        }

        public Point GetNegativePoint(Point P)
        {
            if (P is null) return null;

            BigInteger y = -P.Y % p;
            y = y < 0 ? y + p : y;

            var result = new Point(P.X, y);

            if (!IsPointOnCurve(result)) throw new ArgumentException($"Point {result} is not on curve {this}");

            return result;
        }

        public Point MultiplyPoint(int n, Point P)
        {
            if ((n % N) == 0 || P is null)
            {
                return null;
            }

            if (n < 0)
            {
                return GetNegativePoint(MultiplyPoint(-n, P));
            }

            Point result = null;
            var addend = P;
            uint nUnsigned = (uint)n;

            while (nUnsigned != 0)
            {
                if((nUnsigned & 1) != 0)
                {
                    result = AddPoints(result, addend);
                }
                addend = DoublePoint(addend);
                nUnsigned >>= 1;
            }

            return result;
        }

        public int GetLog(Point P, Point Q)
        {
            if (!IsPointOnCurve(P)) throw new ArgumentException($"Point {P} is not on curve {this}");
            if (!IsPointOnCurve(Q)) throw new ArgumentException($"Point {Q} is not on curve {this}");

            //Random random = new Random();
            int logarithm;

            var start = 1; //random.Next(N);
            var R = MultiplyPoint(start, Q);

            for (int i = 1; i < N; i++)
            {
                if (P == R)
                {
                    logarithm = i % N;
                    return logarithm;
                }

                R = AddPoints(R, Q);
            }

            throw new Exception("Logarithm not found.");
        }

        public override string ToString()
        {
            var aAbs = Math.Abs(a);
            var bAbs = Math.Abs(b);

            string aSign = "+", bSign = "+";

            if (a < 0) aSign = "-";
            if (b < 0) bSign = "-";

            if(a == 1) return $"y^2 = x^3 {aSign} x {bSign} {bAbs}";

            return $"y^2 = x^3 {aSign} {aAbs}x {bSign} {bAbs}";
        }
    }
}
