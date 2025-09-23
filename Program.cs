using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LAB_3
{
    internal class Program
    {
        class Student
        {
            public string LastName;
            public List<int> Grades;
            public string Group;
            public int Course;
            public string Institute;
            public Student(string lastName, List<int> grades, string group, int course, string institute)
            {
                LastName = lastName;
                Grades = grades;
                Group = group;
                Course = course;
                Institute = institute;
            }
            public bool HasNoThreesOrTwos()
            {
                return Grades.Count > 0 && Grades.All(grade => grade == 4 || grade == 5);
            }
        }
        static List<Student> students = new List<Student>();
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("1. Добавить студента");
                Console.WriteLine("2. Показать всех студентов");
                Console.WriteLine("3. Сохранить в файл");
                Console.WriteLine("4. Загрузить из файла");
                Console.WriteLine("5. Найти студентов без троек и двоек");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите: ");
                string choice = Console.ReadLine();
                if (choice == "1") AddStudent();
                else if (choice == "2") ShowStudents();
                else if (choice == "3") SaveToFile();
                else if (choice == "4") LoadFromFile();
                else if (choice == "5") FindStudentsWithNoThreesOrTwos();
                else if (choice == "0") break;
            }
        }
        static void AddStudent()
        {
            Console.Write("Фамилия: ");
            string lastName = Console.ReadLine();
            Console.Write("Оценки через запятую: ");
            List<int> grades = Console.ReadLine().Split(',').Select(int.Parse).ToList();
            Console.Write("Группа: ");
            string group = Console.ReadLine();
            Console.Write("Курс: ");
            int course = int.Parse(Console.ReadLine());
            Console.Write("Институт: ");
            string institute = Console.ReadLine();
            students.Add(new Student(lastName, grades, group, course, institute));
        }
        static void ShowStudents()
        {
            foreach (Student student in students)
            {
                Console.WriteLine($"{student.LastName} | {student.Group} | {student.Course} | {student.Institute}");
            }
        }
        static void SaveToFile()
        {
            Console.Write("Имя файла: ");
            string filename = Console.ReadLine();
            using (StreamWriter writer = new StreamWriter(filename))
            {
                foreach (Student student in students)
                {
                    writer.WriteLine($"{student.LastName};{string.Join(",", student.Grades)};{student.Group};{student.Course};{student.Institute}");
                }
            }
        }
        static void LoadFromFile()
        {
            Console.Write("Имя файла: ");
            string filename = Console.ReadLine();
            students.Clear();
            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(';');
                    List<int> grades = parts[1].Split(',').Select(int.Parse).ToList();
                    students.Add(new Student(parts[0], grades, parts[2], int.Parse(parts[3]), parts[4]));
                }
            }
        }

        // Метод: поиск студентов без троек и двоек
        static void FindStudentsWithNoThreesOrTwos()
        {
            List<Student> goodStudents = new List<Student>();
            foreach (Student student in students)
            {
                if (student.HasNoThreesOrTwos())
                {
                    goodStudents.Add(student);
                }
            }

            if (goodStudents.Count == 0)
            {
                Console.WriteLine("Нет студентов без троек и двоек");
                return;
            }

            Console.WriteLine("=== Студенты без троек и двоек ===");
            foreach (Student student in goodStudents)
            {
                Console.WriteLine($"{student.LastName} | {student.Institute} | {student.Course} курс | {student.Group} | Оценки: {string.Join(", ", student.Grades)}");
            }

            Console.WriteLine($"\nВсего найдено: {goodStudents.Count} студент(ов)");

            // Сохранение в файл
            using (StreamWriter writer = new StreamWriter("students_no_threes_twos.txt"))
            {
                writer.WriteLine("=== Студенты без троек и двоек ===");
                foreach (Student student in goodStudents)
                {
                    writer.WriteLine($"{student.LastName} | {student.Institute} | {student.Course} курс | {student.Group} | Оценки: {string.Join(", ", student.Grades)}");
                }
                writer.WriteLine($"\nВсего найдено: {goodStudents.Count} студент(ов)");
            }
            Console.WriteLine("Результат сохранен в файл: students_no_threes_twos.txt");
        }
    }
}
