using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SecurityLectures
{
    class Program
    {
        static void Main(string[] args)
        {
            string crypt1 = Encrypt("Sobaka shifruetsa", 1);
            Console.WriteLine(crypt1);
            Console.WriteLine(Decrypt(crypt1, 1));

            string crypt2 = VCEncrypt("Sob", "klu");
            Console.WriteLine(crypt2);
            Console.WriteLine(VCDecrypt(crypt2, "klu"));

            Console.WriteLine("-------------------------");

            Console.WriteLine(Xor(true, false).ToString());
            Console.WriteLine(Xor(true, true).ToString());
            Console.WriteLine(Xor(false, false).ToString());

            Console.WriteLine("-------------------------");

            List<string> urls = new List<string> { @"texts\technical.txt", @"texts\art.txt", @"texts\prayer.txt" };
            int i = 1;
            foreach (string path in urls) {
                string value = (File.ReadAllText(path, Encoding.GetEncoding(1251))).ToLower();
                value = Frequency(value);
                File.WriteAllText(@"texts\res"+(i++)+".txt", value);
            }

            Console.ReadKey();
        }

        public static bool Xor(bool a, bool b)
        {
            return (!a & b) | (a & !b);
        }

        public static int Xor(int a, int b)
        {
            return (~a & b) | (a & ~b);
        }

        // Шифр Цезаря
        private static List<char> alphabet_en = new List<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        public static string Encrypt(string value, int key)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char t in value)
            {
                if (alphabet_en.Contains(Char.ToLower(t)))
                {
                    bool charIsBig = (t != Char.ToLower(t));
                    int index = alphabet_en.FindIndex(x => x == Char.ToLower(t));
                    index = (index + key) % alphabet_en.Count;
                    sb.Append((charIsBig) ? Char.ToUpper(alphabet_en[index]) : alphabet_en[index]);
                }
                else
                {
                    sb.Append(t);
                }
            }
            return sb.ToString();
        }

        public static string Decrypt(string value, int key)
        {
            return Encrypt(value, -key);
        }

        // Одноразовый блокнот
        public static string VCEncrypt(string value, string key)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < value.Length; i++)
            {
                sb.Append((char)(Xor(value[i], key[(i % key.Length)])));
            }

            return sb.ToString();

        }
        public static string VCDecrypt(string value, string key)
        {
            return VCEncrypt(key, value);
        }

        // Частота
        public static string Frequency(string value)
        {
            Dictionary<char, int> chars = new Dictionary<char, int>();
            foreach (char t in value)
            {
                if (char.IsLetter(t))
                {
                    if (chars.ContainsKey(t))
                    {
                        chars[t] += 1;
                    }
                    else
                    {
                        chars.Add(t, 1);
                    }
                }
            }
            StringBuilder sb = new StringBuilder();
            foreach (var pair in chars.OrderByDescending(x => x.Value))
            {
                sb.AppendFormat("{0} — {1}", pair.Key, pair.Value);
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
