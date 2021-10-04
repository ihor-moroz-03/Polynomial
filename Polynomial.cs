using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Polynomial
{
    class Polynomial : ICloneable
    {
        double[] _coefs;

        public Polynomial(string s)
        {
            //first group is a minus sign, second is a value and third is a power
            var coefs = Regex.Matches(s, @"(-)? ?(\d+(?:.\d+)?)(?:\*x\^(\d+))?");
            _coefs = new double[int.Parse(coefs[^1].Groups[3].Value) + 1];

            _coefs[coefs[0].Groups[3].Success ? int.Parse(coefs[0].Groups[3].Value) : 0] =
                double.Parse(coefs[0].Groups[1].Value + coefs[0].Groups[2].Value);

            for (int i = 1; i < coefs.Count; ++i)
            {
                _coefs[int.Parse(coefs[i].Groups[3].Value)] =
                    double.Parse(coefs[i].Groups[1].Value + coefs[i].Groups[2].Value);
            }
        }

        public Polynomial(params double[] coefficients)
        {
            _coefs = coefficients.Clone() as double[];
        }

        public double this[int index]
        {
            get => _coefs[index];
            set => _coefs[index] = value;
        }

        public static Polynomial Parse(string s) => new Polynomial(s);

        public static bool TryParse(string s, out Polynomial poly)
        {
            try
            {
                poly = Parse(s);
                return true;
            }
            catch (Exception)
            {
                poly = new Polynomial();
                return false;
            }
        }

        public static Polynomial operator +(Polynomial p1, Polynomial p2) =>
            ((Polynomial)p1.Clone()).Add(p2);

        public static Polynomial operator -(Polynomial p1, Polynomial p2) =>
            ((Polynomial)p1.Clone()).Subtract(p2);

        public static Polynomial operator *(Polynomial p1, Polynomial p2) =>
            ((Polynomial)p1.Clone()).Multiply(p2);

        public static Polynomial operator *(Polynomial p1, double num) =>
            ((Polynomial)p1.Clone()).Multiply(num);

        public override string ToString()
        {
            var result = new List<string>(_coefs.Length);
            if (_coefs[0] != 0) result.Add(_coefs[0].ToString());
            for (int i = 1; i < _coefs.Length; ++i)
            {
                if (_coefs[i] != 0) result.Add($"{_coefs[i]}*x^{i}");
            }
            return string.Join<string>(" + ", result);
        }

        public object Clone() => new Polynomial(_coefs);

        public Polynomial Add(Polynomial other)
        {
            for (int i = 0; i < Math.Min(_coefs.Length, other._coefs.Length); ++i)
            {
                _coefs[i] += other[i];
            }
            return this;
        }

        public Polynomial Subtract(Polynomial other) => Multiply(-1).Add(other);

        public Polynomial Multiply(Polynomial other)
        {
            var newCoefs = new double[_coefs.Length + other._coefs.Length - 1];
            for (int i = 0; i < _coefs.Length; ++i)
            {
                for (int j = 0; j < other._coefs.Length; ++j)
                {
                    newCoefs[i + j] = _coefs[i] * other[j];
                }
            }
            _coefs = newCoefs;
            return this;
        }

        public Polynomial Multiply(double num)
        {
            for (int i = 0; i < _coefs.Length; ++i)
            {
                _coefs[i] *= num;
            }
            return this;
        }
    }
}
