using System;
using System.Collections.Generic;
using System.Text;

namespace CJ {
    public abstract class RandomizerBase {
        #region Protected Fields
        protected System.Random _rand;
        #endregion
        #region Constructors
        protected RandomizerBase() {
            _rand = new System.Random(DateTime.Now.Minute * DateTime.Now.Millisecond);
        }
        #endregion
        #region Protected Methods
        protected int _NextInt() { return _rand.Next(); }
        //In both _Next(int maxValue) and _Next(int minValue, int maxValue) , maxValue is inclusive
        protected int _NextInt(int maxValue) { return _rand.Next(maxValue + 1); }
        protected int _NextInt(int minValue, int maxValue) { return _rand.Next(minValue, maxValue + 1); }

        protected double _NextDouble() { return _rand.NextDouble(); }
        //To keep consistency, maxValue is inclusive in both next methods. However, the distribution will slightly differ from a uniform ditribution.
        protected double _NextDouble(double maxValue) { double ret = (maxValue + 0.1) * _rand.NextDouble(); return ret > maxValue ? maxValue : ret; }
        protected double _NextDouble(double minValue, double maxValue) {
            double ret = minValue + (_rand.NextDouble() * (maxValue - minValue + 0.1));
            return ret > maxValue ? maxValue : ret;
        }
        #endregion
    }
    public class NumeralRandomizer : RandomizerBase {
        #region Constructors
        public NumeralRandomizer() : base() { }
        #endregion
        #region Public Single Number Generation Methods
        public int NextInt() { return _NextInt(); }
        public int NextInt(int maxValueInclusive) { return _NextInt(maxValueInclusive); }
        public int NextInt(int minValueInclusive, int maxValueInclusive) { return _NextInt(minValueInclusive, maxValueInclusive); }
        public double NextDouble() { return _NextDouble(0, double.MaxValue / 2); }
        public double NextDouble(double maxValueInclusive) { return _NextDouble(maxValueInclusive); }
        public double NextDouble(double minValueInclusive, double maxValueInclusive) { return _NextDouble(minValueInclusive, maxValueInclusive); }
        public long NextLong() { return (long)(Math.Ceiling(_NextDouble() * long.MaxValue)); }
        public long NextLong(long maxValueInclusive) { return (long)(Math.Ceiling(_NextDouble() * maxValueInclusive)); }
        public long NextLong(long minValueInclusive, long maxValueInclusive) { return (long)(Math.Ceiling(_NextDouble(minValueInclusive, maxValueInclusive))); }
        #endregion
        #region Public Numbers List Generation Methods
        public List<int> NextInts(int count) {
            List<int> res = new List<int>(count);
            for (int i = 0; i < count; i++) { res.Add(_NextInt()); }
            return res;
        }
        public List<int> NextInts(int count, int maxValueInclusive) {
            List<int> res = new List<int>(count);
            for (int i = 0; i < count; i++) { res.Add(_NextInt(maxValueInclusive)); }
            return res;
        }
        public List<int> NextInts(int count, int minValueInclusive, int maxValueInclusive) {
            List<int> res = new List<int>(count);
            for (int i = 0; i < count; i++) { res.Add(_NextInt(minValueInclusive, maxValueInclusive)); }
            return res;
        }

        public List<double> NextDoubles(int count) {
            List<double> res = new List<double>(count);
            for (int i = 0; i < count; i++) { res.Add(_NextDouble(0, double.MaxValue / 2)); }
            return res;
        }
        public List<double> NextDoubles(int count, double maxValueInclusive) {
            List<double> res = new List<double>(count);
            for (int i = 0; i < count; i++) { res.Add(_NextDouble(maxValueInclusive)); }
            return res;
        }
        public List<double> NextDoubles(int count, double minValueInclusive, double maxValueInclusive) {
            List<double> res = new List<double>(count);
            for (int i = 0; i < count; i++) { res.Add(_NextDouble(minValueInclusive, maxValueInclusive)); }
            return res;
        }

        public List<long> NextLongs(int count) {
            List<long> res = new List<long>(count);
            for (int i = 0; i < count; i++) { res.Add(NextLong()); }
            return res;
        }
        public List<long> NextLongs(int count, long maxValueInclusive) {
            List<long> res = new List<long>(count);
            for (int i = 0; i < count; i++) { res.Add(NextLong(maxValueInclusive)); }
            return res;
        }
        public List<long> NextLongs(int count, long minValueInclusive, long maxValueInclusive) {
            List<long> res = new List<long>(count);
            for (int i = 0; i < count; i++) { res.Add(NextLong(minValueInclusive, maxValueInclusive)); }
            return res;
        }
        #endregion
    }
    public class TextRandomizer : RandomizerBase {
        #region Constructor
        public TextRandomizer() : base() { }
        #endregion
        #region Private Static Fields (standard character categories)
        //The following strings are used to generate standard text types random strings
        private static readonly string _alphaSmall = "abcdefghijklmnopqrstuvwxyz";
        private static readonly string _alphaCapital = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static readonly string _digits = "0123456789";
        private static readonly string _numeralSigns = "+-";
        private static readonly string _numeralOperator = "+-*/><=";
        private static readonly string _punctuation = ".!?,;'\":/&()[]{}<>";
        private static readonly string _special = "<,>.?/\"':;[{]}!@#$%^&*()_+-=~`";
        private static readonly string _white = " \t\r\n\f";
        private static readonly string _printable = _alphaCapital + _alphaSmall + _digits + _special + _white;
        private static readonly string _hexDigits = "0123456789ABCDEF";
        private static readonly string _octDigits = "01234567";
        private static readonly string _binDigits = "01";
        private static readonly string _alpha = _alphaCapital + _alphaSmall;
        private static readonly string _alphaNumeric = _alpha + _digits;
        private static readonly string _identifier = _alphaNumeric + "_";

        private static int _maxVarStringLength = 30;
        #endregion
        #region Public Properties (Standard Character Categories)
        public static string AlphabiticalSmall { get { return TextRandomizer._alphaSmall; } }
        public static string AlphabiticalCapital { get { return TextRandomizer._alphaCapital; } }
        public static string Digits { get { return TextRandomizer._digits; } }
        public static string NumeralSigns { get { return TextRandomizer._numeralSigns; } }
        public static string NumeralOperator { get { return TextRandomizer._numeralOperator; } }
        public static string Punctuation { get { return TextRandomizer._punctuation; } }
        public static string Special { get { return TextRandomizer._special; } }
        public static string White { get { return TextRandomizer._white; } }
        public static string Printable { get { return TextRandomizer._printable; } }
        public static string HexadecimalDigits { get { return TextRandomizer._hexDigits; } }
        public static string OctalDigits { get { return TextRandomizer._octDigits; } }
        public static string BinaryDigits { get { return TextRandomizer._binDigits; } }
        public static string Alphabitical { get { return TextRandomizer._alpha; } }
        public static string Alphanumeric { get { return TextRandomizer._alphaNumeric; } }
        public static string Identifier { get { return TextRandomizer._identifier; } }

        public static int MaximumVariableStringLength { get { return _maxVarStringLength; } set { _maxVarStringLength = value; } }
        #endregion
        #region Single Char Strings Generators
        public string NextSampleChar(string s) { return s[_NextInt(0, s.Length - 1)].ToString(); }
        public string NextSmall() { return _alphaSmall[_NextInt(0, _alphaSmall.Length - 1)].ToString(); }
        public string NextCapital() { return _alphaCapital[_NextInt(0, _alphaCapital.Length - 1)].ToString(); }
        public string NextDigit() { return _digits[_NextInt(0, _digits.Length - 1)].ToString(); }
        public string NextSpecial() { return _special[_NextInt(0, _special.Length - 1)].ToString(); }
        public string NextNumeralSign() { return _numeralSigns[_NextInt(0, _numeralSigns.Length - 1)].ToString(); }
        public string NextNumeralOperator() { return _numeralOperator[_NextInt(0, _numeralOperator.Length - 1)].ToString(); }
        public string NextPunctuation() { return _punctuation[_NextInt(0, _punctuation.Length - 1)].ToString(); }
        public string NextPrintable() { return _printable[_NextInt(0, _printable.Length - 1)].ToString(); }
        public string NextHexadecimal() { return _hexDigits[_NextInt(0, _hexDigits.Length - 1)].ToString(); }
        public string NextOctal() { return _octDigits[_NextInt(0, _octDigits.Length - 1)].ToString(); }
        public string NextBinary() { return _binDigits[_NextInt(0, _binDigits.Length - 1)].ToString(); }
        public string NextAlphabitical() { return _alpha[_NextInt(0, _alpha.Length - 1)].ToString(); }
        public string NextAlphanumeric() { return _alphaNumeric[_NextInt(0, _alphaNumeric.Length - 1)].ToString(); }
        public string NextIdentifier() { return _identifier[_NextInt(0, _identifier.Length - 1)].ToString(); }
        #endregion
        #region Single Strings Generators
        //Fixed Length Strings
        public string NextSampleString(string s, int length) {
            StringBuilder res = new StringBuilder(length);
            for (int i = 0; i < length; i++) { res.Append(NextSampleChar(s)); }
            return res.ToString();
        }
        public string NextSmallString(int length) {
            StringBuilder res = new StringBuilder(length);
            for (int i = 0; i < length; i++) { res.Append(NextSmall()); }
            return res.ToString();
        }
        public string NextCapitalString(int length) {
            StringBuilder res = new StringBuilder(length);
            for (int i = 0; i < length; i++) { res.Append(NextCapital()); }
            return res.ToString();
        }
        public string NextDigitString(int length) {
            StringBuilder res = new StringBuilder(length);
            for (int i = 0; i < length; i++) { res.Append(NextDigit()); }
            return res.ToString();
        }
        public string NextSpecialString(int length) {
            StringBuilder res = new StringBuilder(length);
            for (int i = 0; i < length; i++) { res.Append(NextSpecial()); }
            return res.ToString();
        }
        public string NextNumeralSignString(int length) {
            StringBuilder res = new StringBuilder(length);
            for (int i = 0; i < length; i++) { res.Append(NextNumeralSign()); }
            return res.ToString();
        }
        public string NextNumeralOperatorString(int length) {
            StringBuilder res = new StringBuilder(length);
            for (int i = 0; i < length; i++) { res.Append(NextNumeralOperator()); }
            return res.ToString();
        }
        public string NextPunctuationString(int length) {
            StringBuilder res = new StringBuilder(length);
            for (int i = 0; i < length; i++) { res.Append(NextPunctuation()); }
            return res.ToString();
        }
        public string NextPrintableString(int length) {
            StringBuilder res = new StringBuilder(length);
            for (int i = 0; i < length; i++) { res.Append(NextPrintable()); }
            return res.ToString();
        }
        public string NextHexadecimalString(int length) {
            StringBuilder res = new StringBuilder(length);
            for (int i = 0; i < length; i++) { res.Append(NextHexadecimal()); }
            return res.ToString();
        }
        public string NextOctalString(int length) {
            StringBuilder res = new StringBuilder(length);
            for (int i = 0; i < length; i++) { res.Append(NextOctal()); }
            return res.ToString();
        }
        public string NextBinaryString(int length) {
            StringBuilder res = new StringBuilder(length);
            for (int i = 0; i < length; i++) { res.Append(NextBinary()); }
            return res.ToString();
        }
        public string NextAlphabiticalString(int length) {
            StringBuilder res = new StringBuilder(length);
            for (int i = 0; i < length; i++) { res.Append(NextAlphabitical()); }
            return res.ToString();
        }
        public string NextAlphanumericString(int length) {
            StringBuilder res = new StringBuilder(length);
            for (int i = 0; i < length; i++) { res.Append(NextAlphanumeric()); }
            return res.ToString();
        }
        public string NextIdentifierString(int length) {
            StringBuilder res = new StringBuilder(length);
            for (int i = 0; i < length; i++) { res.Append(NextIdentifier()); }
            return res.ToString();
        }
        //Variable Length Strings
        public string NextSampleString(string s) {
            int length = _NextInt(1, _maxVarStringLength);
            StringBuilder res = new StringBuilder(length);
            for (int i = 0; i < length; i++) { res.Append(NextSampleChar(s)); }
            return res.ToString();
        }
        public string NextSmallString() {
            int length = _NextInt(1, _maxVarStringLength);
            StringBuilder res = new StringBuilder(length);
            for (int i = 0; i < length; i++) { res.Append(NextSmall()); }
            return res.ToString();
        }
        public string NextCapitalString() {
            int length = _NextInt(1, _maxVarStringLength);
            StringBuilder res = new StringBuilder(length);
            for (int i = 0; i < length; i++) { res.Append(NextCapital()); }
            return res.ToString();
        }
        public string NextDigitString() {
            int length = _NextInt(1, _maxVarStringLength);
            StringBuilder res = new StringBuilder(length);
            for (int i = 0; i < length; i++) { res.Append(NextDigit()); }
            return res.ToString();
        }
        public string NextSpecialString() {
            int length = _NextInt(1, _maxVarStringLength);
            StringBuilder res = new StringBuilder(length);
            for (int i = 0; i < length; i++) { res.Append(NextSpecial()); }
            return res.ToString();
        }
        public string NextNumeralSignString() {
            int length = _NextInt(1, _maxVarStringLength);
            StringBuilder res = new StringBuilder(length);
            for (int i = 0; i < length; i++) { res.Append(NextNumeralSign()); }
            return res.ToString();
        }
        public string NextNumeralOperatorString() {
            int length = _NextInt(1, _maxVarStringLength);
            StringBuilder res = new StringBuilder(length);
            for (int i = 0; i < length; i++) { res.Append(NextNumeralOperator()); }
            return res.ToString();
        }
        public string NextPunctuationString() {
            int length = _NextInt(1, _maxVarStringLength);
            StringBuilder res = new StringBuilder(length);
            for (int i = 0; i < length; i++) { res.Append(NextPunctuation()); }
            return res.ToString();
        }
        public string NextPrintableString() {
            int length = _NextInt(1, _maxVarStringLength);
            StringBuilder res = new StringBuilder(length);
            for (int i = 0; i < length; i++) { res.Append(NextPrintable()); }
            return res.ToString();
        }
        public string NextHexadecimalString() {
            int length = _NextInt(1, _maxVarStringLength);
            StringBuilder res = new StringBuilder(length);
            for (int i = 0; i < length; i++) { res.Append(NextHexadecimal()); }
            return res.ToString();
        }
        public string NextOctalString() {
            int length = _NextInt(1, _maxVarStringLength);
            StringBuilder res = new StringBuilder(length);
            for (int i = 0; i < length; i++) { res.Append(NextOctal()); }
            return res.ToString();
        }
        public string NextBinaryString() {
            int length = _NextInt(1, _maxVarStringLength);
            StringBuilder res = new StringBuilder(length);
            for (int i = 0; i < length; i++) { res.Append(NextBinary()); }
            return res.ToString();
        }
        public string NextAlphabiticalString() {
            int length = _NextInt(1, _maxVarStringLength);
            StringBuilder res = new StringBuilder(length);
            for (int i = 0; i < length; i++) { res.Append(NextAlphabitical()); }
            return res.ToString();
        }
        public string NextAlphanumericString() {
            int length = _NextInt(1, _maxVarStringLength);
            StringBuilder res = new StringBuilder(length);
            for (int i = 0; i < length; i++) { res.Append(NextAlphanumeric()); }
            return res.ToString();
        }
        public string NextIdentifierString() {
            int length = _NextInt(1, _maxVarStringLength);
            StringBuilder res = new StringBuilder(length);
            for (int i = 0; i < length; i++) { res.Append(NextIdentifier()); }
            return res.ToString();
        }
        #endregion
        #region Strings Lists Generators
        //Fixed Length Strings
        public List<string> NextSampleStrings(string s, int length, int count) {
            List<string> res = new List<string>(count);
            for (int i = 0; i < count; i++) { res.Add(NextSampleString(s, length)); }
            return res;
        }
        public List<string> NextSmallStrings(int length, int count) {
            List<string> res = new List<string>(count);
            for (int i = 0; i < count; i++) { res.Add(NextSmallString(length)); }
            return res;
        }
        public List<string> NextCapitalStrings(int length, int count) {
            List<string> res = new List<string>(count);
            for (int i = 0; i < count; i++) { res.Add(NextCapitalString(length)); }
            return res;
        }
        public List<string> NextDigitStrings(int length, int count) {
            List<string> res = new List<string>(count);
            for (int i = 0; i < count; i++) { res.Add(NextDigitString(length)); }
            return res;
        }
        public List<string> NextSpecialStrings(int length, int count) {
            List<string> res = new List<string>(count);
            for (int i = 0; i < count; i++) { res.Add(NextSpecialString(length)); }
            return res;
        }
        public List<string> NextNumeralSignStrings(int length, int count) {
            List<string> res = new List<string>(count);
            for (int i = 0; i < count; i++) { res.Add(NextNumeralSignString(length)); }
            return res;
        }
        public List<string> NextNumeralOperatorStrings(int length, int count) {
            List<string> res = new List<string>(count);
            for (int i = 0; i < count; i++) { res.Add(NextNumeralOperatorString(length)); }
            return res;
        }
        public List<string> NextPunctuationStrings(int length, int count) {
            List<string> res = new List<string>(count);
            for (int i = 0; i < count; i++) { res.Add(NextPunctuationString(length)); }
            return res;
        }
        public List<string> NextPrintableStrings(int length, int count) {
            List<string> res = new List<string>(count);
            for (int i = 0; i < count; i++) { res.Add(NextPrintableString(length)); }
            return res;
        }
        public List<string> NextHexadecimalStrings(int length, int count) {
            List<string> res = new List<string>(count);
            for (int i = 0; i < count; i++) { res.Add(NextHexadecimalString(length)); }
            return res;
        }
        public List<string> NextOctalStrings(int length, int count) {
            List<string> res = new List<string>(count);
            for (int i = 0; i < count; i++) { res.Add(NextOctalString(length)); }
            return res;
        }
        public List<string> NextBinaryStrings(int length, int count) {
            List<string> res = new List<string>(count);
            for (int i = 0; i < count; i++) { res.Add(NextBinaryString(length)); }
            return res;
        }
        public List<string> NextAlphabiticalStrings(int length, int count) {
            List<string> res = new List<string>(count);
            for (int i = 0; i < count; i++) { res.Add(NextAlphabiticalString(length)); }
            return res;
        }
        public List<string> NextAlphanumericStrings(int length, int count) {
            List<string> res = new List<string>(count);
            for (int i = 0; i < count; i++) { res.Add(NextAlphanumericString(length)); }
            return res;
        }
        public List<string> NextIdentifierStrings(int length, int count) {
            List<string> res = new List<string>(count);
            for (int i = 0; i < count; i++) { res.Add(NextIdentifierString(length)); }
            return res;
        }
        //Variable Length Strings
        public List<string> NextSampleStrings(string s, int count) {
            List<string> res = new List<string>(count);
            for (int i = 0; i < count; i++) { res.Add(NextSampleString(s)); }
            return res;
        }
        public List<string> NextSmallStrings(int count) {
            List<string> res = new List<string>(count);
            for (int i = 0; i < count; i++) { res.Add(NextSmallString()); }
            return res;
        }
        public List<string> NextCapitalStrings(int count) {
            List<string> res = new List<string>(count);
            for (int i = 0; i < count; i++) { res.Add(NextCapitalString()); }
            return res;
        }
        public List<string> NextDigitStrings(int count) {
            List<string> res = new List<string>(count);
            for (int i = 0; i < count; i++) { res.Add(NextDigitString()); }
            return res;
        }
        public List<string> NextSpecialStrings(int count) {
            List<string> res = new List<string>(count);
            for (int i = 0; i < count; i++) { res.Add(NextSpecialString()); }
            return res;
        }
        public List<string> NextNumeralSignStrings(int count) {
            List<string> res = new List<string>(count);
            for (int i = 0; i < count; i++) { res.Add(NextNumeralSignString()); }
            return res;
        }
        public List<string> NextNumeralOperatorStrings(int count) {
            List<string> res = new List<string>(count);
            for (int i = 0; i < count; i++) { res.Add(NextNumeralOperatorString()); }
            return res;
        }
        public List<string> NextPunctuationStrings(int count) {
            List<string> res = new List<string>(count);
            for (int i = 0; i < count; i++) { res.Add(NextPunctuationString()); }
            return res;
        }
        public List<string> NextPrintableStrings(int count) {
            List<string> res = new List<string>(count);
            for (int i = 0; i < count; i++) { res.Add(NextPrintableString()); }
            return res;
        }
        public List<string> NextHexadecimalStrings(int count) {
            List<string> res = new List<string>(count);
            for (int i = 0; i < count; i++) { res.Add(NextHexadecimalString()); }
            return res;
        }
        public List<string> NextOctalStrings(int count) {
            List<string> res = new List<string>(count);
            for (int i = 0; i < count; i++) { res.Add(NextOctalString()); }
            return res;
        }
        public List<string> NextBinaryStrings(int count) {
            List<string> res = new List<string>(count);
            for (int i = 0; i < count; i++) { res.Add(NextBinaryString()); }
            return res;
        }
        public List<string> NextAlphabiticalStrings(int count) {
            List<string> res = new List<string>(count);
            for (int i = 0; i < count; i++) { res.Add(NextAlphabiticalString()); }
            return res;
        }
        public List<string> NextAlphanumericStrings(int count) {
            List<string> res = new List<string>(count);
            for (int i = 0; i < count; i++) { res.Add(NextAlphanumericString()); }
            return res;
        }
        public List<string> NextIdentifierStrings(int count) {
            List<string> res = new List<string>(count);
            for (int i = 0; i < count; i++) { res.Add(NextBinaryString()); }
            return res;
        }
        #endregion
    }
    public class SampleRandomizer<SampleItemType> : RandomizerBase {
        #region Protected Fields
        protected List<SampleItemType> _sample;
        #endregion
        #region Constructors
        protected SampleRandomizer() : base() { _sample = new List<SampleItemType>(); }
        protected SampleRandomizer(int capacity) : base() { _sample = new List<SampleItemType>(capacity); }
        public SampleRandomizer(IEnumerable<SampleItemType> sample) : base() { _sample = new List<SampleItemType>(sample); }
        #endregion
        #region Generation Methods
        public SampleItemType Next() { return _sample[_NextInt(0, _sample.Count)]; }
        public List<SampleItemType> Next(int count) {
            List<SampleItemType> res = new List<SampleItemType>(count);
            for (int i = 0; i < count; i++) { res.Add(_sample[_NextInt(0, _sample.Count)]); }
            return res;
        }
        #endregion
    }
    public class DateTimeRandomizer : RandomizerBase {
        #region Constructor
        public DateTimeRandomizer() : base() { }
        #endregion
        #region Public Methods
        public DateTime Next() {
            int year, month, day, hour, minute, second, millisecond;
            year = _NextInt(1, 9999); month = _NextInt(1, 12);
            int maxDay = _lastDayOfMonth(month, year);
            day = _NextInt(1, maxDay); hour = _NextInt(0, 23); minute = _NextInt(0, 59); second = _NextInt(0, 59); millisecond = _NextInt(0, 999);
            return new DateTime(year, month, day, hour, minute, second, millisecond);
        }
        public DateTime Next(int maxYearInclusive) {
            int year, month, day, hour, minute, second, millisecond;
            year = _NextInt(1, maxYearInclusive); month = _NextInt(1, 12);
            int maxDay = _lastDayOfMonth(month, year);
            day = _NextInt(1, maxDay); hour = _NextInt(0, 23); minute = _NextInt(0, 59); second = _NextInt(0, 59); millisecond = _NextInt(0, 999);
            return new DateTime(year, month, day, hour, minute, second, millisecond);
        }
        public DateTime NextFromYear(int minYearInclusive) {
            int year, month, day, hour, minute, second, millisecond;
            year = _NextInt(minYearInclusive, 9999); month = _NextInt(1, 12);
            int maxDay = _lastDayOfMonth(month, year);
            day = _NextInt(1, maxDay); hour = _NextInt(0, 23); minute = _NextInt(0, 59); second = _NextInt(0, 59); millisecond = _NextInt(0, 999);
            return new DateTime(year, month, day, hour, minute, second, millisecond);
        }
        public DateTime Next(int minYearInclusive, int maxYearInclusive) {
            int year, month, day, hour, minute, second, millisecond;
            year = _NextInt(minYearInclusive, maxYearInclusive); month = _NextInt(1, 12);
            int maxDay = _lastDayOfMonth(month, year);
            day = _NextInt(1, maxDay); hour = _NextInt(0, 23); minute = _NextInt(0, 59); second = _NextInt(0, 59); millisecond = _NextInt(0, 999);
            return new DateTime(year, month, day, hour, minute, second, millisecond);
        }
        public DateTime Next(int minYearInclusive, int minMonthInclusive, int maxYearInclusive, int maxMonthInclusive) {
            int year, month, day, hour, minute, second, millisecond;
            year = _NextInt(minYearInclusive, maxYearInclusive);
            month = minYearInclusive == maxYearInclusive ? (_NextInt(minMonthInclusive, maxMonthInclusive)) : (minYearInclusive == year ? (_NextInt(minMonthInclusive, 12)) : (maxYearInclusive == year ? _NextInt(1, maxMonthInclusive) : _NextInt(1, 12)));
            int maxDay = _lastDayOfMonth(month, year);
            day = _NextInt(1, maxDay); hour = _NextInt(0, 23); minute = _NextInt(0, 59); second = _NextInt(0, 59); millisecond = _NextInt(0, 999);
            return new DateTime(year, month, day, hour, minute, second, millisecond);
        }
        public DateTime Next(int minYearInclusive, int minMonthInclusive, int minDayInclusive, int maxYearInclusive, int maxMonthInclusive, int maxDayInclusive) {
            int year, month, day, hour, minute, second, millisecond;
            year = _NextInt(minYearInclusive, maxYearInclusive);
            month = minYearInclusive == maxYearInclusive ? (_NextInt(minMonthInclusive, maxMonthInclusive)) : (minYearInclusive == year ? (_NextInt(minMonthInclusive, 12)) : (maxYearInclusive == year ? _NextInt(1, maxMonthInclusive) : _NextInt(1, 12)));
            day = (minYearInclusive == maxYearInclusive && minMonthInclusive == maxMonthInclusive ? _NextInt(minDayInclusive, maxDayInclusive) : (year == minYearInclusive && month == minMonthInclusive ? _NextInt(minDayInclusive, _lastDayOfMonth(month, year)) : (year == maxYearInclusive && month == maxMonthInclusive ? _NextInt(1, maxDayInclusive) : _NextInt(1, _lastDayOfMonth(month)))));
            int minDay = 1, maxDay = _lastDayOfMonth(month, year);
            if ((minYearInclusive == maxYearInclusive) && (maxMonthInclusive - minMonthInclusive <= 1)) {
                if (maxMonthInclusive == minMonthInclusive) { minDay = minDayInclusive; maxDay = maxDayInclusive; }
                else if (month == minMonthInclusive) { minDay = minDayInclusive; }
                else { maxDay = maxDayInclusive; }
            }
            day = _NextInt(minDay, maxDay);
            hour = _NextInt(0, 23); minute = _NextInt(0, 59); second = _NextInt(0, 59); millisecond = _NextInt(0, 999);
            return new DateTime(year, month, day, hour, minute, second, millisecond);
        }
        public DateTime Next(DateTime minDateTimeInclusive, DateTime maxDateTimeInclusive) {
            long min = minDateTimeInclusive.Ticks, max = maxDateTimeInclusive.Ticks;
            return new DateTime(long.Parse(Math.Ceiling(_NextDouble(min, max)).ToString()));
        }
        private int _lastDayOfMonth(int month) {
            if (month == 4 || month == 6 || month == 9 || month == 11) { return 30; }
            else if (month == 2) { return 28; }
            else { return 31; }
        }
        private int _lastDayOfMonth(int month, int year) {
            if (month == 4 || month == 6 || month == 9 || month == 11) { return 30; }
            else if (month == 2) { return (year % 4 == 0 ? 29 : 28); }
            else { return 31; }
        }
        #endregion
    }
    public static class Randomizer {
        #region Private Fields
        private static TextRandomizer _text = new TextRandomizer();
        private static NumeralRandomizer _num = new NumeralRandomizer();
        private static DateTimeRandomizer _dat = new DateTimeRandomizer();
        #endregion
        #region Public Properties
        public static TextRandomizer Text { get { return _text; } }
        public static NumeralRandomizer Numbers { get { return _num; } }
        public static DateTimeRandomizer Dates { get { return _dat; } }
        #endregion
        #region Format Methods
        /*
         * Format modifiers
         * Integers: {i}, {i:max}, {i:min:max}
         * Long: {l}, {l:max}, {l:min:max}
         * Double: {d}, {d:max}, {d:min:max}
         * DateTime: {dt}, {dt:max}, {dt:miny:maxy}, {dt:miny-minm:maxy-maxm}, 
         *           {dt:miny-minm-mind:maxy-maxm-maxd}, 
         *           {dt:miny-minm-mind-minh-minm-mins:maxy-maxm-maxd-maxh-maxm-maxs}
         * Characters: printable: {cp}, small: {cs}, capital: {cc}, digit: {cd}, special: {csp}, 
         *             numeral sign: {cns}, numeral op: {cno}, punctuation: {cpu}, hexadecimal: {cx}, 
         *             octal: {co}, binary: {cb}, alpha: {ca}, alphanumeric: {can}, identifier: {ci}
         * Variable length Strings: printable: {sp}, small: {ss}, capital: {sc}, digit: {sd}, 
         *                          special: {ssp}, numeral sign: {sns}, numeral op: {sno}, 
         *                          punctuation: {spu}, hexadecimal: {sx}, octal: {so}, binary: {sb}, 
         *                          alpha: {sa}, alphanumeric: {san}, identifier: {si}
         * Fixed length Strings: printable: {sp:length}, small: {ss:length}, capital: {sc:length}, 
         *                          digit: {sd:length}, special: {ssp:length}, numeral sign: {sns:length}, 
         *                          numeral op: {sno:length}, punctuation: {spu:length}, 
         *                          hexadecimal: {sx:length}, octal: {so:length}, binary: {sb:length}, 
         *                          alpha: {sa:length}, alphanumeric: {san:length}, identifier: {si:length}
         */
        public static string Format(string format) {
            StringBuilder res = new StringBuilder(format.Length);
            for (int p = 0; p < format.Length; p++) {
                if (format[p] != '{' || p == format.Length - 1) { res.Append(format[p]); }
                else if (format[p + 1] == ' ') { res.Append(format[p]); }
                else if (format[p + 1] == '{') { res.Append('{'); p++; }
                else {
                    int closing = format.IndexOf('}', p);
                    if (closing > -1) {
                        res.Append(_generate(format.Substring(p + 1, closing - p - 1)));
                        p = closing;
                    }
                    else { res.Append(format[p]); }
                }
            }
            return res.ToString();
        }
        public static List<string> Format(string format, int count) {
            List<string> res = new List<string>(count);
            for (int i = 0; i < count; i++) { res.Add(Format(format)); }
            return res;
        }
        private static string _generate(string cmd) {
            string res = "";
            switch (cmd) {
                case "i": res = _num.NextInt().ToString(); break;
                case "l": res = _num.NextLong().ToString(); break;
                case "d": res = _num.NextDouble().ToString(); break;
                case "dt": res = _dat.Next().ToString(); break;
                case "cp": res = _text.NextPrintable(); break;
                case "cs": res = _text.NextSmall(); break;
                case "cc": res = _text.NextCapital(); break;
                case "cd": res = _text.NextDigit(); break;
                case "csp": res = _text.NextSpecial(); break;
                case "cns": res = _text.NextNumeralSign(); break;
                case "cno": res = _text.NextNumeralOperator(); break;
                case "cpu": res = _text.NextPunctuation(); break;
                case "cx": res = _text.NextHexadecimal(); break;
                case "co": res = _text.NextOctal(); break;
                case "cb": res = _text.NextBinary(); break;
                case "ca": res = _text.NextAlphabitical(); break;
                case "can": res = _text.NextAlphanumeric(); break;
                case "ci": res = _text.NextIdentifier(); break;
                case "sp": res = _text.NextPrintableString(); break;
                case "ss": res = _text.NextSmallString(); break;
                case "sc": res = _text.NextCapitalString(); break;
                case "sd": res = _text.NextDigitString(); break;
                case "ssp": res = _text.NextSpecialString(); break;
                case "sns": res = _text.NextNumeralSignString(); break;
                case "sno": res = _text.NextNumeralOperatorString(); break;
                case "spu": res = _text.NextPunctuationString(); break;
                case "sx": res = _text.NextHexadecimalString(); break;
                case "so": res = _text.NextOctalString(); break;
                case "sb": res = _text.NextBinaryString(); break;
                case "sa": res = _text.NextAlphabiticalString(); break;
                case "san": res = _text.NextAlphanumericString(); break;
                case "si": res = _text.NextIdentifierString(); break;
                default: res = _handleOptioned(cmd); break;
            }
            return res;
        }
        private static string _handleOptioned(string cmd) {
            string res = "";
            int colon = cmd.IndexOf(':', 0);
            if (colon < 1 || colon == cmd.Length - 1) { return "{" + cmd + "}"; }
            string command = cmd.Substring(0, colon);
            string options = cmd.Substring(colon + 1);
            try {
                switch (command) {
                    case "sp": res = _text.NextPrintableString(int.Parse(options)); break;
                    case "ss": res = _text.NextSmallString(int.Parse(options)); break;
                    case "sc": res = _text.NextCapitalString(int.Parse(options)); break;
                    case "sd": res = _text.NextDigitString(int.Parse(options)); break;
                    case "ssp": res = _text.NextSpecialString(int.Parse(options)); break;
                    case "sns": res = _text.NextNumeralSignString(int.Parse(options)); break;
                    case "sno": res = _text.NextNumeralOperatorString(int.Parse(options)); break;
                    case "spu": res = _text.NextPunctuationString(int.Parse(options)); break;
                    case "sx": res = _text.NextHexadecimalString(int.Parse(options)); break;
                    case "so": res = _text.NextOctalString(int.Parse(options)); break;
                    case "sb": res = _text.NextBinaryString(int.Parse(options)); break;
                    case "sa": res = _text.NextAlphabiticalString(int.Parse(options)); break;
                    case "san": res = _text.NextAlphanumericString(int.Parse(options)); break;
                    case "si": res = _text.NextIdentifierString(int.Parse(options)); break;
                    case "i": res = options.IndexOf(':') < 0 ? _num.NextInt(int.Parse(options)).ToString() : _num.NextInt(int.Parse(options.Substring(0, options.IndexOf(':'))), int.Parse(options.Substring(options.IndexOf(':') + 1))).ToString(); break;
                    case "l": res = options.IndexOf(':') < 0 ? _num.NextLong(long.Parse(options)).ToString() : _num.NextLong(long.Parse(options.Substring(0, options.IndexOf(':'))), long.Parse(options.Substring(options.IndexOf(':') + 1))).ToString(); break;
                    case "d": res = options.IndexOf(':') < 0 ? _num.NextDouble(double.Parse(options)).ToString() : _num.NextDouble(double.Parse(options.Substring(0, options.IndexOf(':'))), double.Parse(options.Substring(options.IndexOf(':') + 1))).ToString(); break;
                    case "dt":
                        int colon2 = options.IndexOf(':');
                        if (colon2 < 0) { res = _dat.Next(int.Parse(options)).ToString(); }
                        else {
                            string opt1 = options.Substring(0, colon2);
                            string opt2 = options.Substring(colon2 + 1);
                            string[] opt1p = opt1.Split('-');
                            string[] opt2p = opt2.Split('-');
                            if (opt1p.Length != opt2p.Length || opt1p.Length > 7) { res = "{" + cmd + "}"; }
                            else {
                                switch (opt1p.Length) {
                                    case 1: res = _dat.Next(int.Parse(opt1p[0]), int.Parse(opt2p[0])).ToString(); break;
                                    case 2: res = _dat.Next(int.Parse(opt1p[0]), int.Parse(opt1p[1]), int.Parse(opt2p[0]), int.Parse(opt2p[1])).ToString(); break;
                                    case 3: res = _dat.Next(int.Parse(opt1p[0]), int.Parse(opt1p[1]), int.Parse(opt1p[2]), int.Parse(opt2p[0]), int.Parse(opt2p[1]), int.Parse(opt2p[2])).ToString(); break;
                                    case 4: res = _dat.Next(new DateTime(int.Parse(opt1p[0]), int.Parse(opt1p[1]), int.Parse(opt1p[2]), int.Parse(opt1p[3]), 0, 0), new DateTime(int.Parse(opt2p[0]), int.Parse(opt2p[1]), int.Parse(opt2p[2]), int.Parse(opt2p[3]), 59, 59)).ToString(); break;
                                    case 5: res = _dat.Next(new DateTime(int.Parse(opt1p[0]), int.Parse(opt1p[1]), int.Parse(opt1p[2]), int.Parse(opt1p[3]), int.Parse(opt1p[4]), 0), new DateTime(int.Parse(opt2p[0]), int.Parse(opt2p[1]), int.Parse(opt2p[2]), int.Parse(opt2p[3]), int.Parse(opt2p[4]), 59)).ToString(); break;
                                    case 6: res = _dat.Next(new DateTime(int.Parse(opt1p[0]), int.Parse(opt1p[1]), int.Parse(opt1p[2]), int.Parse(opt1p[3]), int.Parse(opt1p[4]), int.Parse(opt1p[5])), new DateTime(int.Parse(opt2p[0]), int.Parse(opt2p[1]), int.Parse(opt2p[2]), int.Parse(opt2p[3]), int.Parse(opt2p[4]), int.Parse(opt2p[5]))).ToString(); break;
                                    case 7: res = _dat.Next(new DateTime(int.Parse(opt1p[0]), int.Parse(opt1p[1]), int.Parse(opt1p[2]), int.Parse(opt1p[3]), int.Parse(opt1p[4]), int.Parse(opt1p[5]), int.Parse(opt1p[6])), new DateTime(int.Parse(opt2p[0]), int.Parse(opt2p[1]), int.Parse(opt2p[2]), int.Parse(opt2p[3]), int.Parse(opt2p[4]), int.Parse(opt2p[5]), int.Parse(opt1p[6]))).ToString(); break;
                                }
                            }
                        }
                        break;
                    default: res = "{" + cmd + "}"; break;
                }
            }
            catch { res = "{" + cmd + "}"; }
            return res;
        }
        #endregion
    }
}
