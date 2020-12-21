using System;
using System.Collections.Generic;
using System.Linq;

namespace GSPH
{
    public class GelfondMethod
    {
        public static int GetLog(EllipticCurve ECC, Point Q, Point P, Dictionary<int, int> divisors)
        {
            Dictionary<int, int> ls = new Dictionary<int, int>();

            foreach (var divisor in divisors)
			{
				var q = divisor.Key;
				var a = divisor.Value;

				Point S = null;
                List<int> ks = new List<int>
                {
                    0
                };

                Point Q_ = ECC.MultiplyPoint(ECC.N / q, Q);
				for (int j = 1; j <= a; j++)
				{
					var kq = ks[j - 1] * Convert.ToInt32(Math.Pow(q, j - 2)); //потому что степень у q равна j-1, по алгоритму, а тут она у нас j+1 из-за работы с листом
					Point kqQ = ECC.MultiplyPoint(kq, Q);
					S = ECC.AddPoints(kqQ, S);

					Point P_S = ECC.AddPoints(P, ECC.GetNegativePoint(S));
					Point P_ = ECC.MultiplyPoint(ECC.N / Convert.ToInt32(Math.Pow(q, j)), P_S);

					int newK = ECC.GetLog(P_, Q_);
					if (newK == -1)
					{
						throw new Exception("Could not fing log.");
					}
					ks.Add(newK);
				}

				int l = 0;

				for (int i = 1; i < ks.Count; i++)
				{
					l += ks[i] * Convert.ToInt32(Math.Pow(q, i - 1));
				}
				ls.Add(Convert.ToInt32(Math.Pow(q, a)), l); // key -> modulus, value -> li
			}

			int result = solveCDP(ls);
            return result;
		}

        /// <summary>
        /// Находит решение системы сравнений с помощью китайской теоремы об остатках
        /// </summary>
        /// <param name="data">словарь сравнений, в котором ключ - модуль, а значение - b</param>
        /// <returns>Решение системы сравнений</returns>
        public static int solveCDP(Dictionary<int, int> data)
        {
            int[] modules = data.Keys.ToArray();

            //Проверяем, разрешима ли система сравнений
            if (gcdForSetNumbers(modules) == 1)
            {
                int M0 = 1;
                int[] M = new int[modules.Length];

                foreach (int module in modules)
                {
                    M0 *= module;
                }

                for (int i = 0; i < M.Length; i++)
                {
                    M[i] = M0 / modules[i];
                }

                int[] y = new int[M.Length];
                int[] b = data.Values.ToArray();
                int solution = 0;

                for (int i = 0; i < M.Length; i++)
                {
                    y[i] = getComparisonSolution(M[i], b[i], modules[i]);
                    solution += (y[i] * M[i]);
                }

                return solution % M0;

            }
            else
            {
                throw new Exception("Модули не взаимно просты");
            }
        }

        /// <summary>
        /// Находит наибольший общий делитель набора чисел
        /// </summary>
        /// <param name="numbers">массив чисел, НОД которых необходимо найти</param>
        /// <returns>НОД набора чисел</returns>
        public static int gcdForSetNumbers(int[] numbers)
        {
            var result = numbers.First();

            for (var i = 1; i < numbers.Length && result != 1; i++)
            {
                result = gcd(result, numbers[i]);
            }

            return result;
        }


        /// <summary>
        /// Находит наибольший общий делитель двух чисел
        /// </summary>
        /// <param name="a">первое число</param>
        /// <param name="b">второе число</param>
        /// <returns>НОД двух чисел</returns>
        public static int gcd(int a, int b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                {
                    a %= b;
                }
                else
                {
                    b %= a;
                }
            }

            var result = a + b;

            return result;
        }

        /// <summary>
        /// Нахождение решения сравнения
        /// </summary>
        /// <param name="a">аргумент при x</param>
        /// <param name="b"></param>
        /// <param name="m">модуль</param>
        /// <returns>Решение сравнения</returns>
        public static int getComparisonSolution(int a, int b, int m)
        {
            int module = m;

            if (a > m)
            {
                a = a % m;
            }

            //Проверяем, разрешима ли система
            if ((b / gcd(a, m)) % 1 == 0)
            {
                List<int> continuedFractionElements = new List<int>();

                //Разложение в цепную дробь
                while (m != 0 && a != 0)
                {
                    if (m > a)
                    {
                        continuedFractionElements.Add(m / a);
                        m = m - a * (m / a);
                    }
                    else
                    {
                        continuedFractionElements.Add(a / m);
                        a = a - m * (a / m);
                    }
                }

                int[] P = new int[continuedFractionElements.Count + 1];
                P[0] = 1;
                P[1] = continuedFractionElements[0];

                //нахождение числителей подходящих дробей по рекуррентной формуле
                for (int i = 2; i < P.Length; i++)
                {
                    P[i] = continuedFractionElements[i - 1] * P[i - 1] + P[i - 2];
                }

                return ((int)Math.Pow(-1, continuedFractionElements.Count - 1) * P[continuedFractionElements.Count - 1] * b) % module;
            }
            else
            {
                throw new Exception("Сравнение " + a.ToString() + "x ≡ " + b.ToString() + " (mod " + m.ToString() + ")");
            }
        }
    }
}
