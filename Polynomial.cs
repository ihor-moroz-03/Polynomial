using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Polynomial
{
    class Polynomial : ICloneable
    {
        double[] _coefs;

        public Polynomial(string s)
        {
            _coefs = Parse(s)._coefs;
        }

        public Polynomial(params double[] coefficients)
        {
            _coefs = coefficients.Clone() as double[];
        }

        public double this[int index]
        {
            get => _coefs[index];
            set
            {
                try
                {
                    _coefs[index] = value;
                }
                catch (ArgumentOutOfRangeException)
                {
                    if (value != 0)
                    {
                        EnsureSize(index + 1);
                        _coefs[index] = value;
                    }
                }
            }
        }

        //public static Polynomial Parse(string s) => new Polynomial(s);
        public static Polynomial Parse(string s)
        {
            //first group is a minus sign, second is a value and third is a power
            var matches = Regex.Matches(s, @"(-)? ?(\d+(?:.\d+)?)(?:\*?x\^(\d+))?");
            double[] coefs = new double[int.Parse(matches[^1].Groups[3].Value) + 1];

            coefs[matches[0].Groups[3].Success ? int.Parse(matches[0].Groups[3].Value) : 0] =
                double.Parse(matches[0].Groups[1].Value + matches[0].Groups[2].Value);

            for (int i = 1; i < matches.Count; ++i)
            {
                coefs[int.Parse(matches[i].Groups[3].Value)] =
                    double.Parse(matches[i].Groups[1].Value + matches[i].Groups[2].Value);
            }
            return new Polynomial(coefs);
        }

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

        public static Polynomial operator +(Polynomial p) => p;

        public static Polynomial operator -(Polynomial p) => p * -1;

        public static Polynomial operator +(Polynomial p1, Polynomial p2) =>
            (p1.Clone() as Polynomial).Add(p2);

        public static Polynomial operator -(Polynomial p1, Polynomial p2) =>
            (p1.Clone() as Polynomial).Subtract(p2);

        public static Polynomial operator *(Polynomial p1, Polynomial p2) =>
            (p1.Clone() as Polynomial).Multiply(p2);

        public static Polynomial operator *(Polynomial p1, double num) =>
            (p1.Clone() as Polynomial).Multiply(num);

        public static implicit operator Polynomial(double d) => new Polynomial(d);

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
            EnsureSize(other._coefs.Length);

            for (int i = 0; i < Math.Min(_coefs.Length, other._coefs.Length); ++i)
            {
                _coefs[i] += other[i];
            }

            return this;
        }

        public Polynomial Subtract(Polynomial other) => Add(-other);

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

        void EnsureSize(long size)
        {
            if (_coefs.Length < size)
            {
                double[] coefs = new double[size];
                for (int i = 0; i < _coefs.Length; ++i) coefs[i] = _coefs[i];
                _coefs = coefs;
            }
        }
    }
}
