using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

namespace TextFilesLab
{
    internal class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("Лабораторна робота: робота з текстовими та двійковими файлами\n");

            Task1Gmail.Run();
            Task2HexReplace.Run();
            Task3MaxSameChars.Run();
            Task4BinaryWords.Run();
            Task5FolderInfo.Run();

            Console.WriteLine("\nУсі завдання виконано.");
        }
    }

    // Завдання 1.9
    // У тексті можуть міститися електронні адреси gmail.
    class Task1Gmail
    {
        public static void Run()
        {
            Console.WriteLine("\n=== Завдання 1.9: Gmail адреси ===");

            string inputFile = "task1_input.txt";
            string foundFile = "task1_found_gmails.txt";
            string replacedFile = "task1_replaced.txt";
            string removedFile = "task1_removed.txt";

            // Створюємо тестовий файл
            File.WriteAllText(inputFile,
                "Напишіть мені на адресу student@gmail.com або admin.test@gmail.com. " +
                "Також є неправильна адреса test@mail.com. Ще одна gmail адреса: user123@gmail.com.",
                Encoding.UTF8);

            string text = File.ReadAllText(inputFile, Encoding.UTF8);

            Regex gmailRegex = new Regex(@"\b[a-zA-Z0-9._%+-]+@gmail\.com\b");

            MatchCollection matches = gmailRegex.Matches(text);

            Console.WriteLine("Знайдені Gmail адреси:");
            foreach (Match match in matches)
            {
                Console.WriteLine(match.Value);
            }

            Console.WriteLine("Кількість Gmail адрес: " + matches.Count);

            // Запис знайдених адрес у новий файл
            File.WriteAllLines(foundFile, matches.Select(m => m.Value), Encoding.UTF8);

            // Пошук конкретної адреси
            Console.Write("\nВведіть Gmail адресу для пошуку: ");
            string searchEmail = Console.ReadLine() ?? "";

            if (matches.Any(m => m.Value == searchEmail))
                Console.WriteLine("Таку адресу знайдено у файлі.");
            else
                Console.WriteLine("Такої адреси немає у файлі.");

            // Заміна адреси
            Console.Write("Введіть Gmail адресу, яку треба замінити: ");
            string oldEmail = Console.ReadLine() ?? "";

            Console.Write("Введіть нову Gmail адресу: ");
            string newEmail = Console.ReadLine() ?? "";

            string replacedText = text.Replace(oldEmail, newEmail);
            File.WriteAllText(replacedFile, replacedText, Encoding.UTF8);

            // Вилучення адреси
            Console.Write("Введіть Gmail адресу, яку треба вилучити: ");
            string removeEmail = Console.ReadLine() ?? "";

            string removedText = text.Replace(removeEmail, "");
            File.WriteAllText(removedFile, removedText, Encoding.UTF8);

            Console.WriteLine("Результати записані у файли:");
            Console.WriteLine(foundFile);
            Console.WriteLine(replacedFile);
            Console.WriteLine(removedFile);
        }
    }

    // Завдання 2.9
    // Замінити всі шістнадцяткові цифри ('0'-'9', 'a'-'f') на '+'
    class Task2HexReplace
    {
        public static void Run()
        {
            Console.WriteLine("\n=== Завдання 2.9: Заміна шістнадцяткових цифр ===");

            string inputFile = "task2_input.txt";
            string outputFile = "task2_output.txt";

            File.WriteAllText(inputFile,
                "abc xyz 123 45f hello 9a test g h",
                Encoding.UTF8);

            string text = File.ReadAllText(inputFile, Encoding.UTF8);

            Regex hexRegex = new Regex("[0-9a-f]");

            int count = hexRegex.Matches(text).Count;
            string result = hexRegex.Replace(text, "+");

            File.WriteAllText(outputFile, result, Encoding.UTF8);

            Console.WriteLine("Початковий текст:");
            Console.WriteLine(text);

            Console.WriteLine("Кількість знайдених шістнадцяткових символів: " + count);

            Console.WriteLine("Результат:");
            Console.WriteLine(result);

            Console.WriteLine("Результат записано у файл: " + outputFile);
        }
    }

    // Завдання 3.18
    // Знайти слово з найбільшою кількістю однакових символів
    class Task3MaxSameChars
    {
        public static void Run()
        {
            Console.WriteLine("\n=== Завдання 3.18: Слово з найбільшою кількістю однакових символів ===");

            string inputFile = "task3_input.txt";
            string outputFile = "task3_output.txt";

            File.WriteAllText(inputFile,
                "мама школа програмування комунікація тестування ананас",
                Encoding.UTF8);

            string text = File.ReadAllText(inputFile, Encoding.UTF8);

            Regex wordRegex = new Regex(@"[\p{L}\p{Nd}]+");
            MatchCollection matches = wordRegex.Matches(text);

            string bestWord = "";
            int bestCount = 0;

            foreach (Match match in matches)
            {
                string word = match.Value.ToLower();

                int maxSameChars = word
                    .GroupBy(ch => ch)
                    .Max(group => group.Count());

                if (maxSameChars > bestCount)
                {
                    bestCount = maxSameChars;
                    bestWord = match.Value;
                }
            }

            string result = $"Слово з найбільшою кількістю однакових символів: {bestWord}\n" +
                            $"Кількість однакових символів у ньому: {bestCount}";

            File.WriteAllText(outputFile, result, Encoding.UTF8);

            Console.WriteLine(result);
            Console.WriteLine("Результат записано у файл: " + outputFile);
        }
    }

    // Завдання 4.18
    // Двійковий файл зі словами.
    // Вивести слова, які починаються й закінчуються однією буквою.
    class Task4BinaryWords
    {
        public static void Run()
        {
            Console.WriteLine("\n=== Завдання 4.18: Двійковий файл зі словами ===");

            string binaryFile = "task4_words.bin";

            string[] words =
            {
                "level",
                "test",
                "anna",
                "school",
                "radar",
                "window",
                "refer",
                "apple"
            };

            // Записуємо слова у двійковий файл
            using (BinaryWriter writer = new BinaryWriter(File.Open(binaryFile, FileMode.Create), Encoding.UTF8))
            {
                writer.Write(words.Length);

                foreach (string word in words)
                {
                    writer.Write(word);
                }
            }

            Console.WriteLine("Слова, які починаються і закінчуються однією буквою:");

            // Читаємо слова з двійкового файлу
            using (BinaryReader reader = new BinaryReader(File.Open(binaryFile, FileMode.Open), Encoding.UTF8))
            {
                int count = reader.ReadInt32();

                for (int i = 0; i < count; i++)
                {
                    string word = reader.ReadString();

                    if (word.Length > 0 &&
                        char.ToLower(word[0]) == char.ToLower(word[word.Length - 1]))
                    {
                        Console.WriteLine(word);
                    }
                }
            }

            Console.WriteLine("Двійковий файл створено: " + binaryFile);
        }
    }

    // Завдання 5.8
    // Вивести повну інформацію про файли папки All.
    class Task5FolderInfo
    {
        public static void Run()
        {
            Console.WriteLine("\n=== Завдання 5.8: Інформація про файли папки All ===");

            string folderName = "All";

            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            // Створимо кілька файлів для прикладу
            File.WriteAllText(Path.Combine(folderName, "file1.txt"), "Перший файл", Encoding.UTF8);
            File.WriteAllText(Path.Combine(folderName, "file2.txt"), "Другий файл", Encoding.UTF8);
            File.WriteAllText(Path.Combine(folderName, "data.txt"), "Дані для перевірки", Encoding.UTF8);

            DirectoryInfo directory = new DirectoryInfo(folderName);
            FileInfo[] files = directory.GetFiles();

            Console.WriteLine($"Повна інформація про файли папки {folderName}:\n");

            foreach (FileInfo file in files)
            {
                Console.WriteLine("Назва файлу: " + file.Name);
                Console.WriteLine("Повний шлях: " + file.FullName);
                Console.WriteLine("Розширення: " + file.Extension);
                Console.WriteLine("Розмір у байтах: " + file.Length);
                Console.WriteLine("Дата створення: " + file.CreationTime);
                Console.WriteLine("Дата зміни: " + file.LastWriteTime);
                Console.WriteLine("Дата останнього доступу: " + file.LastAccessTime);
                Console.WriteLine("Атрибути: " + file.Attributes);
                Console.WriteLine("-----------------------------------");
            }
        }
    }
}