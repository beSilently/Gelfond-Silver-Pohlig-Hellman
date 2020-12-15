using System;
using System.Numerics;

namespace GSPH
{
    public class Point
    {
        public BigInteger X { get; private set; }
        public BigInteger Y { get; private set; }

        public Point(BigInteger x, BigInteger y)
        {
            X = x;
            Y = y;
        }

        public Point(int x, int y) : this(new BigInteger(x), new BigInteger(y)) { }

        public override string ToString() => $"({X}, {Y})";

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Point);
        }

        public bool Equals(Point P)
        {
            if (Object.ReferenceEquals(P, null))
            {
                return false;
            }

            if (Object.ReferenceEquals(this, P))
            {
                return true;
            }

            if (this.GetType() != P.GetType())
            {
                return false;
            }

            return (X == P.X) && (Y == P.Y);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode();
        }

        public static bool operator ==(Point lhs, Point rhs)
        {
            // Check for null on left side.
            if (Object.ReferenceEquals(lhs, null))
            {
                if (Object.ReferenceEquals(rhs, null))
                {
                    // null == null = true.
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Point lhs, Point rhs)
        {
            return !(lhs == rhs);
        }
    }
}
