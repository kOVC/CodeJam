using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CJ {
    public class InputFile {
        #region Protected Fields
        protected string _filePath;
        protected FileStream _stream;
        protected StreamReader _reader;
        #endregion
        #region Constructors
        public InputFile() { _initialize("a.in"); }
        public InputFile(string path) { _initialize(path); }
        private void _initialize(string path) {
            _filePath = path;
            _stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
            _reader = new StreamReader(_stream);
        }
        #endregion
        #region Public Properties
        public bool EOF { get { return _reader.EndOfStream; } }
        public StreamReader Reader { get { return _reader; } }
        #endregion
        #region Reader Methods
        public string ReadLine() { return _reader.ReadLine(); }
        public string ReadToEnd() { return _reader.ReadToEnd(); }
        public int Read() { return _reader.Read(); }
        public int Read(char[] buffer, int index, int count) { return _reader.Read(buffer, index, count); }
        public int ReadBlock(char[] buffer, int index, int count) { return _reader.ReadBlock(buffer, index, count); }
        public int Peek() { return _reader.Peek(); }
        public void Close() { _reader.Close(); _stream.Close(); _reader.Dispose(); _stream.Dispose(); }
        #endregion
        #region CJ Methods
        public int ReadIntLine() { return int.Parse(_reader.ReadLine()); }
        public List<int> ReadIntList() {
            string[] nums = _reader.ReadLine().Split(' ');
            List<int> res = new List<int>(nums.Length);
            for (int i = 0; i < nums.Length; i++) {
                res.Add(int.Parse(nums[i]));
            }
            return res;
        }
        public int[] ReadIntArray() {
            string[] nums = _reader.ReadLine().Split(' ');
            int[] res = new int[nums.Length];
            for (int i = 0; i < nums.Length; i++) {
                res[i] = int.Parse(nums[i]);
            }
            return res;
        }

        public List<string> ReadStringList() { return new List<string>(_reader.ReadLine().Split(' ')); }
        public string[] ReadStringArray() { return _reader.ReadLine().Split(' '); }

        public long ReadLongLine() { return long.Parse(_reader.ReadLine()); }
        public List<long> ReadLongList() {
            string[] nums = _reader.ReadLine().Split(' ');
            List<long> res = new List<long>(nums.Length);
            for (int i = 0; i < nums.Length; i++) {
                res.Add(long.Parse(nums[i]));
            }
            return res;
        }
        public long[] ReadLongArray() {
            string[] nums = _reader.ReadLine().Split(' ');
            long[] res = new long[nums.Length];
            for (int i = 0; i < nums.Length; i++) {
                res[i] = long.Parse(nums[i]);
            }
            return res;
        }

        public double ReadDoubleLine() { return double.Parse(_reader.ReadLine()); }
        public List<double> ReadDoubleList() {
            string[] nums = _reader.ReadLine().Split(' ');
            List<double> res = new List<double>(nums.Length);
            for (int i = 0; i < nums.Length; i++) {
                res.Add(double.Parse(nums[i]));
            }
            return res;
        }
        public double[] ReadDoubleArray() {
            string[] nums = _reader.ReadLine().Split(' ');
            double[] res = new double[nums.Length];
            for (int i = 0; i < nums.Length; i++) {
                res[i] = double.Parse(nums[i]);
            }
            return res;
        }

        public decimal ReadDecimalLine() { return decimal.Parse(_reader.ReadLine()); }
        public List<decimal> ReadDecimalList() {
            string[] nums = _reader.ReadLine().Split(' ');
            List<decimal> res = new List<decimal>(nums.Length);
            for (int i = 0; i < nums.Length; i++) {
                res.Add(decimal.Parse(nums[i]));
            }
            return res;
        }
        public decimal[] ReadDecimalArray() {
            string[] nums = _reader.ReadLine().Split(' ');
            decimal[] res = new decimal[nums.Length];
            for (int i = 0; i < nums.Length; i++) {
                res[i] = decimal.Parse(nums[i]);
            }
            return res;
        }

        public DateTime ReadDateTimeLine() { return DateTime.Parse(_reader.ReadLine()); }
        public List<DateTime> ReadDateTimeList() {
            string[] nums = _reader.ReadLine().Split(' ');
            List<DateTime> res = new List<DateTime>(nums.Length);
            for (int i = 0; i < nums.Length; i++) {
                res.Add(DateTime.Parse(nums[i]));
            }
            return res;
        }
        public DateTime[] ReadDateTimeArray() {
            string[] nums = _reader.ReadLine().Split(' ');
            DateTime[] res = new DateTime[nums.Length];
            for (int i = 0; i < nums.Length; i++) {
                res[i] = DateTime.Parse(nums[i]);
            }
            return res;
        }
        #endregion
    }
    public class OutputFile {
        #region Protected Fields
        protected string _filePath;
        protected FileStream _stream;
        protected StreamWriter _writer;
        protected int _lastCase;
        #endregion
        #region Public Properties
        public StreamWriter Writer { get { return _writer; } }
        public int CasesCount { get { return _lastCase; } }
        #endregion
        #region Constructors
        public OutputFile() { _initialize("a.out"); }
        public OutputFile(string filename) { _initialize(filename); }
        private void _initialize(string filename) {
            _filePath = filename;
            _stream = new FileStream(_filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            _writer = new StreamWriter(_stream);
            _lastCase = 0;
        }
        #endregion
        #region Writer Methods
        public void Flush() { _writer.Flush(); }
        public void Close() { _stream.Flush(); _writer.Flush(); _writer.Close(); _writer.Dispose(); _stream.Close(); _stream.Dispose(); }
        #endregion
        #region Case Outputting Methods
        public void WriteCase(string s) { _lastCase++; _writer.WriteLine("Case #{0}: {1}", _lastCase, s); }
        public void WriteCase(int caseNumber, string s) { _writer.WriteLine("Case #{0}: {1}", caseNumber, s); }
        #endregion
    }
}
