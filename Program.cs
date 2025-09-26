using System;
using System.Collections.Generic;
using System.IO;

namespace edu_simple
{

    public class Student
    {
        public string Id;                 
        public string FullName;             
        public Dictionary<string, int> Marks = new Dictionary<string, int>(); 

        public Student(string id, string fullName) { Id = id; FullName = fullName; }

        public bool IsExcellent()
        {
            if (Marks.Count == 0) return false;
            foreach (var kv in Marks) if (kv.Value != 5) return false;
            return true;
        }

        public override string ToString() => $"{FullName} (ID:{Id}), отличник: {IsExcellent()}";
    }

    public class Group
    {
        public string Name;                 
        public List<Student> Students = new List<Student>(); 
        public Group(string name) { Name = name; }
        public override string ToString() => $"Группа {Name}, студентов: {Students.Count}";
    }

    public class Course
    {
        public int Number;                  
        public List<Group> Groups = new List<Group>(); 
        public Course(int number) { Number = number; }
        public override string ToString() => $"Курс {Number}, групп: {Groups.Count}";
    }

    public class Institute
    {
        public string Name;                 
        public List<string> Subjects = new List<string>(); 
        public List<Course> Courses = new List<Course>(); 
        public Institute(string name) { Name = name; }

        public override string ToString() => $"Институт: {Name}, курсов: {Courses.Count}, предметов: {Subjects.Count}";
    }

    class Program
    {
        static List<Institute> institutes = new List<Institute>(); 
        static int autoId = 1; 

        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Seed(); 

            // Главный цикл программы
            while (true)
            {
                Menu(); 
                Console.Write("Выбор: ");
                var cmd = Console.ReadLine();
                Console.WriteLine();

                if (cmd == "0") break; 
                switch (cmd)
                {
                    case "1": AddInstitute(); break;        
                    case "2": RenameInstitute(); break;       
                    case "3": DeleteInstitute(); break;       
                    case "4": AddSubject(); break;            
                    case "5": AddCourseAndGroup(); break;      
                    case "6": AddStudent(); break;            
                    case "7": PutMark(); break;               
                    case "8": ListAll(); break;               
                    case "9": QueryNoBadMarks(); break;        
                    default: Console.WriteLine("Нет такого пункта.\n"); break;
                }
            }
        }
        static void Menu()
        {
            Console.WriteLine("=== МЕНЮ ===");
            Console.WriteLine("1) Добавить институт");
            Console.WriteLine("2) Переименовать институт");
            Console.WriteLine("3) Удалить институт");
            Console.WriteLine("4) Добавить предмет в институт");
            Console.WriteLine("5) Добавить курс и/или группу");
            Console.WriteLine("6) Добавить студента в группу");
            Console.WriteLine("7) Поставить/изменить оценку студенту");
            Console.WriteLine("8) Показать все данные");
            Console.WriteLine("9) Запрос: фамилии студентов, у которых нет троек и двоек ");
            Console.WriteLine("0) Выход\n");
        }
        
        // Выбор института из списка
        static Institute PickInstitute()
        {
            if (institutes.Count == 0) { Console.WriteLine("Институтов нет.\n"); return null; }
            for (int i = 0; i < institutes.Count; i++) Console.WriteLine($"{i + 1}. {institutes[i].Name}");
            Console.Write("Номер института: ");
            if (int.TryParse(Console.ReadLine(), out int iidx) && iidx >= 1 && iidx <= institutes.Count) return institutes[iidx - 1];
            Console.WriteLine("Неверно.\n"); return null;
        }

        // Выбор курса из института
        static Course PickCourse(Institute inst)
        {
            if (inst.Courses.Count == 0) { Console.WriteLine("Курсов нет.\n"); return null; }
            for (int i = 0; i < inst.Courses.Count; i++) Console.WriteLine($"{i + 1}. Курс {inst.Courses[i].Number}");
            Console.Write("Номер курса: ");
            if (int.TryParse(Console.ReadLine(), out int iidx) && iidx >= 1 && iidx <= inst.Courses.Count) return inst.Courses[iidx - 1];
            Console.WriteLine("Неверно.\n"); return null;
        }

        // Выбор группы из курса
        static Group PickGroup(Course course)
        {
            if (course.Groups.Count == 0) { Console.WriteLine("Групп нет.\n"); return null; }
            for (int i = 0; i < course.Groups.Count; i++) Console.WriteLine($"{i + 1}. {course.Groups[i].Name}");
            Console.Write("Номер группы: ");
            if (int.TryParse(Console.ReadLine(), out int iidx) && iidx >= 1 && iidx <= course.Groups.Count) return course.Groups[iidx - 1];
            Console.WriteLine("Неверно.\n"); return null;
        }

        // Выбор студента из группы
        static Student PickStudent(Group group)
        {
            if (group.Students.Count == 0) { Console.WriteLine("Студентов нет.\n"); return null; }
            for (int i = 0; i < group.Students.Count; i++) Console.WriteLine($"{i + 1}. {group.Students[i]}");
            Console.Write("Номер студента: ");
            if (int.TryParse(Console.ReadLine(), out int iidx) && iidx >= 1 && iidx <= group.Students.Count) return group.Students[iidx - 1];
            Console.WriteLine("Неверно.\n"); return null;
        }
        
        // Добавление нового института
        static void AddInstitute()
        {
            Console.Write("Название: ");
            var name = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(name)) { Console.WriteLine("Пусто.\n"); return; }
            institutes.Add(new Institute(name));
            Console.WriteLine("Добавлено.\n");
        }

        // Переименование  института
        static void RenameInstitute()
        {
            var inst = PickInstitute(); if (inst == null) return;
            Console.Write("Новое название: ");
            var name = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(name)) { Console.WriteLine("Пусто.\n"); return; }
            inst.Name = name; Console.WriteLine("Ок.\n");
        }

        // Удаление института
        static void DeleteInstitute()
        {
            var inst = PickInstitute(); if (inst == null) return;
            institutes.Remove(inst); Console.WriteLine("Удалено.\n");
        }

        // Добавление учебного предмета в институт
        static void AddSubject()
        {
            var inst = PickInstitute(); if (inst == null) return;
            Console.Write("Название предмета: ");
            var s = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(s)) { Console.WriteLine("Пусто.\n"); return; }
            if (!inst.Subjects.Contains(s)) inst.Subjects.Add(s);
            Console.WriteLine("Добавлено.\n");
        }

        // Добавление курса и или группы в институт
        static void AddCourseAndGroup()
        {
            var inst = PickInstitute(); if (inst == null) return;

            Console.Write("Номер курса (1..6): ");
            if (!int.TryParse(Console.ReadLine(), out int num) || num < 1 || num > 6) { Console.WriteLine("Неверно.\n"); return; }

            // Поиск или создание курса
            var course = inst.Courses.Find(c => c.Number == num);
            if (course == null) { course = new Course(num); inst.Courses.Add(course); Console.WriteLine("Курс создан."); }

            Console.Write("Название группы: ");
            var gname = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(gname)) { Console.WriteLine("Пусто.\n"); return; }
            if (course.Groups.Exists(g => string.Equals(g.Name, gname, StringComparison.OrdinalIgnoreCase)))
            { Console.WriteLine("Уже есть.\n"); return; }

            course.Groups.Add(new Group(gname));
            Console.WriteLine("Группа добавлена.\n");
        }

        // Добавление студента в группу
        static void AddStudent()
        {
            var inst = PickInstitute(); if (inst == null) return;
            var course = PickCourse(inst); if (course == null) return;
            var group = PickGroup(course); if (group == null) return;

            var id = $"S{autoId++:000}"; 
            Console.Write("ФИО: ");
            var fio = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(fio)) { Console.WriteLine("Пусто.\n"); return; }

            group.Students.Add(new Student(id, fio));
            Console.WriteLine($"Добавлен студент {fio} с ID {id}.\n");
        }

        // Постановка или изменение оценки студенту
        static void PutMark()
        {
            var inst = PickInstitute(); if (inst == null) return;
            if (inst.Subjects.Count == 0) { Console.WriteLine("Нет предметов.\n"); return; }
            var course = PickCourse(inst); if (course == null) return;
            var group = PickGroup(course); if (group == null) return;
            var st = PickStudent(group); if (st == null) return;

           
            for (int i = 0; i < inst.Subjects.Count; i++) Console.WriteLine($"{i + 1}. {inst.Subjects[i]}");
            Console.Write("Номер предмета: ");
            if (!int.TryParse(Console.ReadLine(), out int sidx) || sidx < 1 || sidx > inst.Subjects.Count) { Console.WriteLine("Неверно.\n"); return; }
            var subj = inst.Subjects[sidx - 1];

        
            Console.Write("Оценка (2..5): ");
            if (!int.TryParse(Console.ReadLine(), out int m) || m < 2 || m > 5) { Console.WriteLine("Неверно.\n"); return; }

            st.Marks[subj] = m; 
            Console.WriteLine("Сохранено.\n");
        }

     
        
        // Вывод всей структуры данных
        static void ListAll()
        {
            if (institutes.Count == 0) { Console.WriteLine("Данных нет.\n"); return; }

            foreach (var inst in institutes)
            {
                Console.WriteLine(inst);
                if (inst.Subjects.Count > 0)
                {
                    Console.Write("  Предметы: ");
                    for (int i = 0; i < inst.Subjects.Count; i++)
                    {
                        Console.Write(inst.Subjects[i]);
                        if (i < inst.Subjects.Count - 1) Console.Write(", ");
                    }
                    Console.WriteLine();
                }
                foreach (var c in inst.Courses)
                {
                    Console.WriteLine($"  {c}");
                    foreach (var g in c.Groups)
                    {
                        Console.WriteLine($"    {g}");
                        foreach (var s in g.Students)
                        {
                            Console.WriteLine($"      {s}");
                            if (s.Marks.Count > 0)
                            {
                                Console.Write("        Оценки: ");
                                int k = 0; foreach (var kv in s.Marks)
                                {
                                    Console.Write($"{kv.Key}:{kv.Value}");
                                    if (++k < s.Marks.Count) Console.Write(", ");
                                }
                                Console.WriteLine();
                            }
                        }
                    }
                }
                Console.WriteLine();
            }
        }
        
        // Поиск студентов без двоек и троек с сохранением в файл
        static void QueryNoBadMarks()
        {
            if (institutes.Count == 0) { Console.WriteLine("Нет данных.\n"); return; }

            List<string> lines = new List<string>();
            lines.Add("Студенты, у которых нет двоек и троек:");

            // Поиск по всем студентам
            foreach (var inst in institutes)
                foreach (var c in inst.Courses)
                    foreach (var g in c.Groups)
                        foreach (var s in g.Students)
                        {
                            // Проверка что нет оценок 2 и 3
                            if (s.Marks.Count > 0 && !s.Marks.ContainsValue(2) && !s.Marks.ContainsValue(3))
                            {
                                string info = $"{s.FullName} (Институт: {inst.Name}, Курс: {c.Number}, Группа: {g.Name})";
                                Console.WriteLine(info);
                                lines.Add(info);
                            }
                        }

            Console.WriteLine();

         
            try
            {
                File.WriteAllLines("result.txt", lines);
                Console.WriteLine("Сохранено в result.txt\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка записи файла: " + ex.Message + "\n");
            }
        }

     
        static void Seed()
        {
            // Создание первого института с данными
            var i1 = new Institute("Гос-институт");
            i1.Subjects.AddRange(new[] { "Программирование", "Математика" });
            var c1 = new Course(1);
            var g1 = new Group("П2");
            
           
            var s1 = new Student("S001", "Дрон");
            s1.Marks["Программирование"] = 5;
            s1.Marks["Математика"] = 5;
            
            var s2 = new Student("S002", "Головач Лена");
            s2.Marks["Программирование"] = 4;
            s2.Marks["Математика"] = 5;
            
            var s3 = new Student("S003", "Надя Куховарка");
            s3.Marks["Программирование"] = 3;
            s3.Marks["Математика"] = 4;
            
            g1.Students.Add(s1); g1.Students.Add(s2); g1.Students.Add(s3);
            c1.Groups.Add(g1); i1.Courses.Add(c1);
            institutes.Add(i1);

       
            var i2 = new Institute("Какой-то институт");
            i2.Subjects.Add("Физика");
            var c2 = new Course(1);
            var g2 = new Group("И1");
            var s4 = new Student("S004", "Нег Рот");
            s4.Marks["Физика"] = 5;
            g2.Students.Add(s4); c2.Groups.Add(g2); i2.Courses.Add(c2);
            institutes.Add(i2);

            autoId = 5; // Установка начального значения для ID
        }
    }
}
