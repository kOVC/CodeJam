using System;
using System.Collections.Generic;
using System.Text;

namespace CJ {
    public enum Sign { Positive, Negative };
    public class Integer {
        #region Private Static Methods
        private static char _signChar(Sign s) { return s == Sign.Positive ? '+' : '-'; }
        private static Sign _toSign(char s) { if (s == '+') { return Sign.Positive; } else if (s == '-') { return Sign.Negative; } else { throw new Exception("Character could not be parsed into a valid sign."); } }

        private static int _int(byte b) { return (int)b; }
        private static char _char(byte b) { return ((int)b).ToString()[0]; }
        private static string _string(byte b) { return ((int)b).ToString(); }
        private static byte _byte(char c) {
            byte res = 0;
            switch (c) {
                case '0': res = 0; break;
                case '1': res = 1; break;
                case '2': res = 2; break;
                case '3': res = 3; break;
                case '4': res = 4; break;
                case '5': res = 5; break;
                case '6': res = 6; break;
                case '7': res = 7; break;
                case '8': res = 8; break;
                case '9': res = 9; break;
                default: throw new Exception("Character is not a valid digit");
            }
            return res;
        }

        private static bool _valid(string s) {
            if (string.IsNullOrEmpty(s)) { return false; }
            bool res = true;
            string ss = s.StartsWith("+") || s.StartsWith("-") ? s.Substring(1) : s;
            if (string.IsNullOrEmpty(ss)) { return false; }
            for (int i = 0; i < ss.Length; i++) { if (!char.IsDigit(ss[i])) { res = false; break; } }
            return res;
        }
        private static bool _magIsZero(List<byte> n) { return n != null && n.Count == 1 && n[0] == 0; }
        private static bool _isZero(Integer n) { return n._digits != null && n._digits.Count == 1 && n._digits[0] == 0; }
        private static bool _magIsOne(List<byte> n) { return n != null && n.Count == 1 && n[0] == 1; }

        private static List<byte> _magAdd(List<byte> n1, List<byte> n2) {
            if (_magIsZero(n1)) { return new List<byte>(n2); }
            if (_magIsZero(n2)) { return new List<byte>(n1); }
            List<byte> larger = n1.Count >= n2.Count ? n1 : n2;
            List<byte> smaller = n1.Count >= n2.Count ? n2 : n1;
            List<byte> res = new List<byte>(larger.Count);
            int carry = 0, dor = 0, dotr = 0;
            for (int i = 0; i < larger.Count; i++) {
                dotr = i < smaller.Count ? smaller[i] + larger[i] + carry : larger[i] + carry;
                dor = dotr % 10;
                carry = dotr / 10;
                res.Add((byte)dor);
            }
            if (carry != 0) { res.Add((byte)carry); }
            return res;
        }
        //n1 must be greater than or equl to n2 in magnitude
        private static List<byte> _magSub(List<byte> n1, List<byte> n2) {
            if (_magIsZero(n2)) { return new List<byte>(n1); }
            List<byte> first = new List<byte>(n1);
            List<byte> res = new List<byte>();
            for (int i = 0; i < n1.Count; i++) {
                if (i >= n2.Count) { res.Add(first[i]); }
                else if (first[i] >= n2[i]) { res.Add((byte)(first[i] - n2[i])); }
                else {
                    int j = i + 1;
                    while (first[j] == 0) { j++; }
                    first[j] = (byte)(first[j] - 1); j--;
                    while (j > i) { first[j] = 9; j--; }
                    res.Add((byte)((first[i] + 10) - n2[i]));
                }
            }
            while (res[res.Count - 1] == 0 && res.Count > 1) { res.RemoveAt(res.Count - 1); }
            return res;
        }
        private static List<byte> _magMul(List<byte> n1, List<byte> n2) {
            if (_magIsZero(n1) || _magIsZero(n2)) { return new List<byte>(new byte[] { 0 }); }
            else if (_magIsOne(n1)) { return new List<byte>(n2); }
            else if (_magIsOne(n2)) { return new List<byte>(n1); }
            else {
                List<byte> larger = n1.Count >= n2.Count ? n1 : n2;
                List<byte> smaller = n1.Count >= n2.Count ? n2 : n1;
                List<List<byte>> ress = new List<List<byte>>(smaller.Count);
                for (int i = 0; i < smaller.Count; i++) {
                    ress.Add(new List<byte>());
                    for (int x = 0; x < i; x++) { ress[i].Add(0); }
                    int carry = 0, ropt = 0, rop = 0;
                    for (int j = 0; j < larger.Count; j++) {
                        ropt = (smaller[i] * larger[j]) + carry;
                        carry = ropt / 10;
                        rop = ropt % 10;
                        ress[i].Add((byte)rop);
                    }
                    if (carry != 0) { ress[i].Add((byte)carry); }
                }
                List<byte> res = new List<byte>(ress[0]);
                for (int f = 1; f < ress.Count; f++) { res = _magAdd(res, ress[f]); }
                return res;
            }
        }
        private static List<byte> _magDiv(List<byte> n1, List<byte> n2) {
            if (_magIsZero(n2)) { throw new DivideByZeroException(); }
            if (_magIsZero(n1)) { return new List<byte>(new byte[] { 0 }); }
            if (_magIsOne(n2)) { return new List<byte>(n1); }
            if (_magCmpEQ(n1, n2)) { return new List<byte>(new byte[] { 1 }); }
            if (_magCmpLT(n1, n2)) { return new List<byte>(new byte[] { 0 }); }
            if (n1.Count == n2.Count) { return new List<byte>(new byte[] { _divHelper(n1, n2) }); }
            int i = n2.Count;
            byte[] tt = new byte[i];
            n1.CopyTo(n1.Count - i, tt, 0, i);
            List<byte> temp1 = new List<byte>(tt);
            int j = n1.Count - i;
            List<byte> res = new List<byte>();
            while (j >= 0) {
                j--;
                byte r = _divHelper(temp1, n2);
                res.Add(r);
                temp1 = _magSub(temp1, _magMul(r, n2));
                if (j >= 0) { temp1.Insert(0, n1[j]); }
            }
            res.Reverse();
            while (res[res.Count - 1] == 0) { res.RemoveAt(res.Count - 1); }
            return res;
        }
        private static List<byte> _magMod(List<byte> n1, List<byte> n2) {
            if (_magIsZero(n2)) { throw new DivideByZeroException(); }
            if (_magIsZero(n1)) { return new List<byte>(new byte[] { 0 }); }
            if (_magIsOne(n2)) { return new List<byte>(new byte[] { 0 }); }
            if (_magCmpEQ(n1, n2)) { return new List<byte>(new byte[] { 0 }); }
            if (_magCmpLT(n1, n2)) { return new List<byte>(n1); }
            List<byte> d = _magDiv(n1, n2);
            return _magSub(n1, _magMul(n2, d));
        }
        private static byte _divHelper(List<byte> n1, List<byte> n2) {
            if (_magIsZero(n1)) { return 0; }
            if (_magCmpEQ(n1, n2)) { return 1; }
            if (_magCmpLT(n1, n2)) { return 0; }
            int t = 0;
            if (n1.Count == n2.Count) { t = n1[n1.Count - 1] / n2[n1.Count - 1]; }
            else { t = (n1[n1.Count - 1] * 10 + n1[n1.Count - 2]) / n2[n2.Count - 1]; }
            while (_magCmpGT(_magMul(t, n2), n1)) { t--; }
            return (byte)t;
        }
        private static List<byte> _magMul(int digit, List<byte> n) {
            List<byte> res = new List<byte>(n.Count + 1);
            int carry = 0, rop = 0, ropt = 0;
            for (int i = 0; i < n.Count; i++) {
                ropt = (digit * n[i]) + carry;
                rop = ropt % 10;
                carry = ropt / 10;
                res.Add((byte)rop);
            }
            if (carry != 0) { res.Add((byte)carry); }
            return res;
        }

        private static bool _magCmpGT(List<byte> n1, List<byte> n2) {
            if (n1.Count > n2.Count) { return true; }
            else if (n1.Count < n2.Count) { return false; }
            else {
                for (int i = n1.Count - 1; i > -1; i--) {
                    if (n1[i] > n2[i]) { return true; }
                    else if (n1[i] < n2[i]) { return false; }
                }
                return false;
            }
        }
        private static bool _magCmpLT(List<byte> n1, List<byte> n2) {
            if (n1.Count < n2.Count) { return true; }
            else if (n1.Count > n2.Count) { return false; }
            else {
                for (int i = n1.Count - 1; i > -1; i--) {
                    if (n1[i] < n2[i]) { return true; }
                    else if (n1[i] > n2[i]) { return false; }
                }
                return false;
            }
        }
        private static bool _magCmpEQ(List<byte> n1, List<byte> n2) {
            if (n1.Count != n2.Count) { return false; }
            else {
                for (int i = n1.Count - 1; i > -1; i--) {
                    if (n1[i] != n2[i]) { return false; }
                }
                return true;
            }
        }
        private static bool _magCmpGTE(List<byte> n1, List<byte> n2) {
            if (n1.Count > n2.Count) { return true; }
            else if (n1.Count < n2.Count) { return false; }
            else {
                for (int i = n1.Count - 1; i > -1; i--) {
                    if (n1[i] > n2[i]) { return true; }
                    else if (n1[i] < n2[i]) { return false; }
                }
                return true;
            }
        }
        private static bool _magCmpLTE(List<byte> n1, List<byte> n2) {
            if (n1.Count < n2.Count) { return true; }
            else if (n1.Count > n2.Count) { return false; }
            else {
                for (int i = n1.Count - 1; i > -1; i--) {
                    if (n1[i] < n2[i]) { return true; }
                    else if (n1[i] > n2[i]) { return false; }
                }
                return true;
            }
        }
        private static bool _magCmpNE(List<byte> n1, List<byte> n2) {
            if (n1.Count != n2.Count) { return true; }
            else {
                for (int i = 0; i < n1.Count; i++) { if (n1[i] != n2[i]) { return true; } }
                return false;
            }
        }
        #endregion
        #region Public Static Methods
        public static bool IsValidIntegerString(string s) { return _valid(s); }
        //Factory Methods
        public static Integer New(string s) {
            if (_valid(s)) {
                Integer n = new Integer();
                int i = 0;
                if (s.StartsWith("+") || s.StartsWith("-")) { n._sign = _toSign(s[0]); i = 1; }
                else { n._sign = Sign.Positive; }
                while (i < s.Length && s[i] == '0') { i++; }
                if (i == s.Length) { return n; }
                n._digits = new List<byte>(s.Length - i);
                for (int j = 0; j < s.Length - i; j++) { n._digits.Add(0); }
                for (int x = s.Length - 1; x >= i; x--) { n._digits[s.Length - 1 - x] = _byte(s[x]); }
                return n;
            }
            else { throw new Exception("String could not be parsed into a valid Integer!"); }
        }
        public static Integer New(long number) { return New(number.ToString()); }
        public static Integer New(decimal number) { string s = number.ToString(); int i = s.IndexOf('.'); return New(i > -1 ? s.Substring(0, i) : s); }
        public static Integer New(double number) { string s = number.ToString(); int i = s.IndexOf('.'); return New(i > -1 ? s.Substring(0, i) : s); }
        public static Integer New(Integer n) { Integer r = new Integer(); r._sign = n._sign; r._digits = new List<byte>(n._digits.ToArray()); return r; }
        //Mathematical Operators
        public static Integer Add(Integer n1, Integer n2) {
            if (n1._sign == n2._sign) { return new Integer(_magAdd(n1._digits, n2._digits), n1._sign); }
            else {
                if (_magCmpEQ(n1._digits, n2._digits)) { return new Integer(new List<byte>(new byte[] { 0 }), Sign.Positive); }
                List<byte> larger, smaller; Sign sign = Sign.Positive;
                if (_magCmpGT(n1._digits, n2._digits)) { larger = n1._digits; smaller = n2._digits; sign = n1._sign; }
                else { larger = n2._digits; smaller = n1._digits; sign = n2._sign; }
                return new Integer(_magSub(larger, smaller), sign);
            }
        }
        public static Integer Subtract(Integer n1, Integer n2) { return Add(n1, -n2); }
        public static Integer Multiply(Integer n1, Integer n2) { return new Integer(_magMul(n1._digits, n2._digits), n1._sign == n2._sign ? Sign.Positive : Sign.Negative); }
        public static Integer Divide(Integer n1, Integer n2) { return new Integer(_magDiv(n1._digits, n2._digits), n1._sign == n2._sign ? Sign.Positive : Sign.Negative); }
        public static Integer Modulo(Integer n1, Integer n2) { return new Integer(_magMod(n1._digits, n2._digits), n1._sign == n2._sign ? Sign.Positive : Sign.Negative); }
        #endregion
        #region Private Fields
        private Sign _sign;
        private List<byte> _digits;
        #endregion
        #region Constructors
        public Integer() { _sign = Sign.Positive; _digits = new List<byte>(new byte[] { 0 }); }
        public Integer(Integer copy) { _copyInit(New(copy)); }
        public Integer(string number) { _copyInit(New(number)); }
        public Integer(long number) { _copyInit(New(number)); }
        public Integer(decimal number) { _copyInit(New(number)); }
        public Integer(double number) { _copyInit(New(number)); }
        private Integer(List<byte> digits, Sign sign) { _digits = digits; _sign = sign; }
        #endregion
        #region Private Methods
        private void _copyInit(Integer n) { _sign = n._sign; _digits = n._digits; }
        #endregion
        #region Public Properties
        public long SumOfDigits {
            get {
                long res = 0;
                for (int i = 0; i < _digits.Count; i++) { res += _digits[i]; }
                return res;
            }
        }
        public long ProductOfDigits {
            get {
                long res = 1;
                for (int i = 0; i < _digits.Count; i++) { if (_digits[i] == 0) { return 0; } res *= _digits[i]; }
                return res;
            }
        }
        public int DigitsCount { get { return _digits.Count; } }

        public bool IsEven { get { return _digits[0] % 2 == 0; } }
        public bool IsOdd { get { return _digits[0] % 2 != 0; } }
        #endregion
        #region Public Methods
        //digitPosition is a zero based position where 0 is the rightmost (lsd, ones) digit, 1 the second digit (tens) .. etc
        public int GetDigit(int digitPosition) {
            return (int)(_digits[digitPosition]);
        }
        #endregion
        #region Object Overrides
        public override string ToString() {
            if (_digits.Count == 1 && _digits[0] == 0) { return "0"; }
            StringBuilder s = new StringBuilder(_digits.Count + 1);
            if (_sign == Sign.Negative) { s.Append("-"); }
            for (int i = _digits.Count - 1; i >= 0; i--) { s.Append(_char(_digits[i])); }
            return s.ToString();
        }
        public override int GetHashCode() { return base.GetHashCode(); }
        public bool Equals(Integer i) {
            if (i._sign != _sign) { return false; }
            return _magCmpEQ(_digits, i._digits);
        }
        public override bool Equals(object obj) {
            if (obj is Integer) { return Equals((Integer)obj); }
            else {
                Integer i = new Integer();
                try { i = new Integer(obj.ToString()); }
                catch { return false; }
                return Equals(i);
            }
        }
        #endregion
        #region Operator Oevrloading
        #region Comparison Operators
        public static bool operator ==(Integer i1, Integer i2) { return (i1._sign == i2._sign) && (_magCmpEQ(i1._digits, i2._digits)); }
        public static bool operator !=(Integer i1, Integer i2) { return (i1._sign != i2._sign) || (_magCmpNE(i1._digits, i2._digits)); }
        public static bool operator >(Integer i1, Integer i2) { return i1._sign == i2._sign ? (i1._sign == Sign.Positive ? _magCmpGT(i1._digits, i2._digits) : _magCmpGT(i2._digits, i1._digits)) : (i1._sign == Sign.Positive); }
        public static bool operator <(Integer i1, Integer i2) { return i1._sign == i2._sign ? (i1._sign == Sign.Positive ? _magCmpLT(i1._digits, i2._digits) : _magCmpLT(i2._digits, i1._digits)) : (i1._sign == Sign.Negative); }
        public static bool operator >=(Integer i1, Integer i2) { return i1._sign == i2._sign ? (i1._sign == Sign.Positive ? _magCmpGTE(i1._digits, i2._digits) : _magCmpGTE(i2._digits, i1._digits)) : (i1._sign == Sign.Positive); }
        public static bool operator <=(Integer i1, Integer i2) { return i1._sign == i2._sign ? (i1._sign == Sign.Positive ? _magCmpLTE(i1._digits, i2._digits) : _magCmpLTE(i2._digits, i1._digits)) : (i1._sign == Sign.Negative); }

        public static bool operator ==(Integer i1, string i2) { return i1 == New(i2); }
        public static bool operator ==(string i1, Integer i2) { return New(i1) == i2; }
        public static bool operator ==(Integer i1, long i2) { return i1 == New(i2); }
        public static bool operator ==(long i1, Integer i2) { return New(i1) == i2; }
        public static bool operator ==(Integer i1, decimal i2) { return i1 == New(i2); }
        public static bool operator ==(decimal i1, Integer i2) { return New(i1) == i2; }
        public static bool operator ==(Integer i1, double i2) { return i1 == New(i2); }
        public static bool operator ==(double i1, Integer i2) { return New(i1) == i2; }

        public static bool operator !=(Integer i1, string i2) { return i1 != New(i2); }
        public static bool operator !=(string i1, Integer i2) { return New(i1) != i2; }
        public static bool operator !=(Integer i1, long i2) { return i1 != New(i2); }
        public static bool operator !=(long i1, Integer i2) { return New(i1) != i2; }
        public static bool operator !=(Integer i1, decimal i2) { return i1 != New(i2); }
        public static bool operator !=(decimal i1, Integer i2) { return New(i1) != i2; }
        public static bool operator !=(Integer i1, double i2) { return i1 != New(i2); }
        public static bool operator !=(double i1, Integer i2) { return New(i1) != i2; }

        public static bool operator >(Integer i1, string i2) { return i1 > New(i2); }
        public static bool operator >(string i1, Integer i2) { return New(i1) > i2; }
        public static bool operator >(Integer i1, long i2) { return i1 > New(i2); }
        public static bool operator >(long i1, Integer i2) { return New(i1) > i2; }
        public static bool operator >(Integer i1, decimal i2) { return i1 > New(i2); }
        public static bool operator >(decimal i1, Integer i2) { return New(i1) > i2; }
        public static bool operator >(Integer i1, double i2) { return i1 > New(i2); }
        public static bool operator >(double i1, Integer i2) { return New(i1) > i2; }

        public static bool operator >=(Integer i1, string i2) { return i1 >= New(i2); }
        public static bool operator >=(string i1, Integer i2) { return New(i1) >= i2; }
        public static bool operator >=(Integer i1, long i2) { return i1 >= New(i2); }
        public static bool operator >=(long i1, Integer i2) { return New(i1) >= i2; }
        public static bool operator >=(Integer i1, decimal i2) { return i1 >= New(i2); }
        public static bool operator >=(decimal i1, Integer i2) { return New(i1) >= i2; }
        public static bool operator >=(Integer i1, double i2) { return i1 >= New(i2); }
        public static bool operator >=(double i1, Integer i2) { return New(i1) >= i2; }

        public static bool operator <(Integer i1, string i2) { return i1 < New(i2); }
        public static bool operator <(string i1, Integer i2) { return New(i1) < i2; }
        public static bool operator <(Integer i1, long i2) { return i1 < New(i2); }
        public static bool operator <(long i1, Integer i2) { return New(i1) < i2; }
        public static bool operator <(Integer i1, decimal i2) { return i1 < New(i2); }
        public static bool operator <(decimal i1, Integer i2) { return New(i1) < i2; }
        public static bool operator <(Integer i1, double i2) { return i1 < New(i2); }
        public static bool operator <(double i1, Integer i2) { return New(i1) < i2; }

        public static bool operator <=(Integer i1, string i2) { return i1 <= New(i2); }
        public static bool operator <=(string i1, Integer i2) { return New(i1) <= i2; }
        public static bool operator <=(Integer i1, long i2) { return i1 <= New(i2); }
        public static bool operator <=(long i1, Integer i2) { return New(i1) <= i2; }
        public static bool operator <=(Integer i1, decimal i2) { return i1 <= New(i2); }
        public static bool operator <=(decimal i1, Integer i2) { return New(i1) <= i2; }
        public static bool operator <=(Integer i1, double i2) { return i1 <= New(i2); }
        public static bool operator <=(double i1, Integer i2) { return New(i1) <= i2; }
        #endregion
        #region Mathematical Operators
        //Plus and minus unary operators (return a copy of the integer with the same sign (+) or opposite sign (-))
        public static Integer operator +(Integer n) { return new Integer(n); }
        public static Integer operator -(Integer n) { Integer res = new Integer(n); res._sign = res._sign == Sign.Positive ? Sign.Negative : Sign.Positive; return res; }
        //Increment and Decrement Operators
        public static Integer operator ++(Integer n) { return Add(n, new Integer("1")); }
        public static Integer operator --(Integer n) { return Add(n, new Integer("-1")); }
        //Addition (binary +)
        public static Integer operator +(Integer n1, Integer n2) { return Add(n1, n2); }
        public static Integer operator +(Integer n1, string n2) { return Add(n1, New(n2)); }
        public static Integer operator +(string n1, Integer n2) { return Add(New(n1), n2); }
        public static Integer operator +(Integer n1, long n2) { return Add(n1, New(n2)); }
        public static Integer operator +(long n1, Integer n2) { return Add(New(n1), n2); }
        public static Integer operator +(Integer n1, double n2) { return Add(n1, New(n2)); }
        public static Integer operator +(double n1, Integer n2) { return Add(New(n1), n2); }
        public static Integer operator +(Integer n1, decimal n2) { return Add(n1, New(n2)); }
        public static Integer operator +(decimal n1, Integer n2) { return Add(New(n1), n2); }
        //Subtraction (binary -)
        public static Integer operator -(Integer n1, Integer n2) { return Add(n1, -n2); }
        public static Integer operator -(Integer n1, string n2) { return Add(n1, -New(n2)); }
        public static Integer operator -(string n1, Integer n2) { return Add(New(n1), -n2); }
        public static Integer operator -(Integer n1, long n2) { return Add(n1, -New(n2)); }
        public static Integer operator -(long n1, Integer n2) { return Add(New(n1), -n2); }
        public static Integer operator -(Integer n1, double n2) { return Add(n1, -New(n2)); }
        public static Integer operator -(double n1, Integer n2) { return Add(New(n1), -n2); }
        public static Integer operator -(Integer n1, decimal n2) { return Add(n1, -New(n2)); }
        public static Integer operator -(decimal n1, Integer n2) { return Add(New(n1), -n2); }
        //Multiplication
        public static Integer operator *(Integer n1, Integer n2) { return Multiply(n1, n2); }
        public static Integer operator *(Integer n1, string n2) { return Multiply(n1, New(n2)); }
        public static Integer operator *(string n1, Integer n2) { return Multiply(New(n1), n2); }
        public static Integer operator *(Integer n1, long n2) { return Multiply(n1, New(n2)); }
        public static Integer operator *(long n1, Integer n2) { return Multiply(New(n1), n2); }
        public static Integer operator *(Integer n1, double n2) { return Multiply(n1, New(n2)); }
        public static Integer operator *(double n1, Integer n2) { return Multiply(New(n1), n2); }
        public static Integer operator *(Integer n1, decimal n2) { return Multiply(n1, New(n2)); }
        public static Integer operator *(decimal n1, Integer n2) { return Multiply(New(n1), n2); }
        //Division
        public static Integer operator /(Integer n1, Integer n2) { return Divide(n1, n2); }
        public static Integer operator /(Integer n1, string n2) { return Divide(n1, New(n2)); }
        public static Integer operator /(string n1, Integer n2) { return Divide(New(n1), n2); }
        public static Integer operator /(Integer n1, long n2) { return Divide(n1, New(n2)); }
        public static Integer operator /(long n1, Integer n2) { return Divide(New(n1), n2); }
        public static Integer operator /(Integer n1, double n2) { return Divide(n1, New(n2)); }
        public static Integer operator /(double n1, Integer n2) { return Divide(New(n1), n2); }
        public static Integer operator /(Integer n1, decimal n2) { return Divide(n1, New(n2)); }
        public static Integer operator /(decimal n1, Integer n2) { return Divide(New(n1), n2); }
        //Modulo
        public static Integer operator %(Integer n1, Integer n2) { return Modulo(n1, n2); }
        public static Integer operator %(Integer n1, string n2) { return Modulo(n1, New(n2)); }
        public static Integer operator %(string n1, Integer n2) { return Modulo(New(n1), n2); }
        public static Integer operator %(Integer n1, long n2) { return Modulo(n1, New(n2)); }
        public static Integer operator %(long n1, Integer n2) { return Modulo(New(n1), n2); }
        public static Integer operator %(Integer n1, double n2) { return Modulo(n1, New(n2)); }
        public static Integer operator %(double n1, Integer n2) { return Modulo(New(n1), n2); }
        public static Integer operator %(Integer n1, decimal n2) { return Modulo(n1, New(n2)); }
        public static Integer operator %(decimal n1, Integer n2) { return Modulo(New(n1), n2); }
        #endregion
        #endregion
    }
}
