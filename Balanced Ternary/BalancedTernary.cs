using System;
using System.Collections.Generic;
using System.Text;

namespace BalancedTernary
{
    class BalancedTernary
    {
        private enum BalancedTernaryDigit
        {
            Minus = -1,
            Zero = 0,
            Plus = 1
        }

        private BalancedTernaryDigit[] digits;

        private static BalancedTernaryDigit carryDigit = BalancedTernaryDigit.Zero;

        #region Constructors

        public BalancedTernary(int number)
        {
            List<BalancedTernaryDigit> digits = new List<BalancedTernaryDigit>();

            int sign = Math.Sign(number);
            number = Math.Abs(number);

            while (number != 0)
            {
                int remainder = number % 3;

                if (remainder == 2)
                {
                    number += 1;
                    remainder = -1;
                }

                digits.Add((BalancedTernaryDigit)remainder);
                number /= 3;
            }

            this.digits = digits.ToArray();

            if (sign < 0)
            {
                this.Invert();
            }
        }

        public BalancedTernary(string number)
        {
            this.digits = new BalancedTernaryDigit[number.Length];

            for (int i = 0; i < number.Length; i++)
            {
                switch (number[i])
                {
                    case '-':
                        this.digits[i] = BalancedTernaryDigit.Minus;
                        break;
                    case '0':
                        this.digits[i] = BalancedTernaryDigit.Zero;
                        break;
                    case '+':
                        this.digits[i] = BalancedTernaryDigit.Plus;
                        break;
                    default:
                        throw new ArgumentException("Unknown Digit: " + number[i]);
                }
            }

            Array.Reverse(this.digits);
        }

        private BalancedTernary()
        {
            this.digits = new BalancedTernaryDigit[0];
        }

        private BalancedTernary(BalancedTernaryDigit[] value)
        {
            int end = value.Length - 1;

            while (value[end] == BalancedTernaryDigit.Zero)
            {
                end--;
            }

            this.digits = new BalancedTernaryDigit[end + 1];
            
            Array.Copy(value, this.digits, end + 1);
        }

        #endregion

        #region Operations

        public static BalancedTernary operator +(BalancedTernary a, BalancedTernary b)
        {
            int maxLength = Math.Max(a.digits.Length, b.digits.Length);

            BalancedTernaryDigit[] result = new BalancedTernaryDigit[maxLength + 1];

            for (int i = 0; i < maxLength; i++)
            {
                if (i < a.digits.Length)
                {
                    result[i] = Add(result[i], a.digits[i]);
                    result[i + 1] = carryDigit;
                }
                else
                {
                    carryDigit = BalancedTernaryDigit.Zero;
                }

                if (i < b.digits.Length)
                {
                    result[i] = Add(result[i], b.digits[i]);
                    result[i + 1] = Add(result[i + 1], carryDigit);
                }
            }
            return new BalancedTernary(result);
        }

        private static BalancedTernaryDigit Add(BalancedTernaryDigit a, BalancedTernaryDigit b)
        {
            if (a != b)
            {
                carryDigit = BalancedTernaryDigit.Zero;
                return (BalancedTernaryDigit)((int)a + (int)b);
            }
            else
            {
                carryDigit = a;
                return (BalancedTernaryDigit)(-(int)b);
            }
        }

        public static BalancedTernary operator -(BalancedTernary a)
        {
            BalancedTernary result = BalancedTernary.Clone(a);

            result.Invert();

            return result;
        }

        public static BalancedTernary operator -(BalancedTernary a, BalancedTernary b)
        {
            return a + (-b);
        }

        public static BalancedTernary operator *(BalancedTernary a, BalancedTernary b)
        {
            BalancedTernaryDigit[] longValue = a.digits;
            BalancedTernaryDigit[] shortValue = b.digits;
            BalancedTernary result = new BalancedTernary();

            if (a.digits.Length < b.digits.Length)
            {
                longValue = b.digits;
                shortValue = a.digits;
            }

            for (int i = 0; i < shortValue.Length; i++)
            {
                if (shortValue[i] != BalancedTernaryDigit.Zero)
                {
                    BalancedTernaryDigit[] temp = new BalancedTernaryDigit[i + longValue.Length];

                    for (int j = 0; j < longValue.Length; j++)
                    {
                        temp[i + j] = (BalancedTernaryDigit)((int)shortValue[i] * (int)longValue[j]);
                    }

                    result += new BalancedTernary(temp);
                }
            }

            return result;
        }

        #endregion

        private static BalancedTernary Clone(BalancedTernary source)
        {
            BalancedTernary result = new BalancedTernary();

            result.digits = (BalancedTernaryDigit[])source.digits.Clone();

            return result;
        }

        private void Invert()
        {
            for (int i = 0; i < this.digits.Length; i++)
            {
                this.digits[i] = (BalancedTernaryDigit)(-(int)this.digits[i]);
            }
        }

        public int ToInt()
        {
            int number = 0;

            for (int i = 0; i < this.digits.Length; i++)
            {
                number += (int)this.digits[i] * (int)Math.Pow(3, i);
            }

            return number;
        }

        public override string ToString()
        {
            StringBuilder number = new StringBuilder();

            for (int i = this.digits.Length - 1; i >= 0; i--)
            {
                switch (this.digits[i])
                {
                    case BalancedTernaryDigit.Minus:
                        number.Append('-');
                        break;
                    case BalancedTernaryDigit.Zero:
                        number.Append('0');
                        break;
                    case BalancedTernaryDigit.Plus:
                        number.Append('+');
                        break;
                }
            }

            return number.ToString();
        }
    }
}