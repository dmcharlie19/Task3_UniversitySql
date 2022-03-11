using System;
using UniversitySql.Models;
using UniversitySql.Repositories;

namespace UniversitySql
{
    class Program
    {

        private const string ConnectionString =
            @"Data Source=KM\SQLEXPRESS;Initial Catalog=University;Pooling=true;Integrated Security=SSPI";

        static bool GetAnsver()
        {
            while (true)
            {
                var a = Console.ReadLine();

                if (a == "y")
                    return true;
                if (a == "n")
                    return false;
            }
        }

        static int GetId(string objName = null)
        {
            Console.WriteLine($"Введите id {objName}");
            while (true)
            {
                var s = Console.ReadLine();
                try
                {
                    int id = Convert.ToInt32(s);
                    return id;
                }
                catch (Exception)
                {
                    Console.WriteLine("введите число!");
                }
            }
        }

        static void GetAllAndPrint<T>(ICommonOperationsSqlRepository<T> repos) where T: new()
        {
            var units = repos.GetAll();
            foreach (var unit in units)
            {
                Console.WriteLine(unit.ToString());
            }
        }

        static void Main(string[] args)
        {
            var studentRepository = new CommonOperationsSqlRepository<Student>(ConnectionString);
            var studyGroupRepository = new CommonOperationsSqlRepository<StudyGroup>(ConnectionString);
            var studentsAndGroupsRepository = new StudentsAndGroupsSqlRepository(ConnectionString);

            while (true)
            {
                Console.WriteLine("    1 - получить список всех студентов");
                Console.WriteLine("    2 - Добавить студента");
                Console.WriteLine("    3 - Удалить студента по номеру");
                Console.WriteLine("    4 - получить список всех групп");
                Console.WriteLine("    5 - Добавить группу");
                Console.WriteLine("    6 - Удалить группу по номеру");
                Console.WriteLine("    7 - Добавить студента в группу");
                Console.WriteLine("    8 - получить студентов по id группы");
                Console.WriteLine("    9 - получить отчет по количеству cтудентов в группах");
                Console.WriteLine("");
                Console.WriteLine("Введите номер команды");

                int command = 0;
                try
                {
                    command = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("введите число!");
                }

                switch (command)
                {
                    case 1:
                        GetAllAndPrint(studentRepository);
                        break;
                    case 2:
                        {
                            Console.WriteLine("Введите имя студента");
                            string fn = Console.ReadLine();

                            Console.WriteLine("Введите фамилию студента");
                            string ln = Console.ReadLine();

                            studentRepository.Add(new Student
                            {
                                FirstName = fn,
                                LastName = ln,
                            });
                            Console.WriteLine("Успешно добавлено");
                            break;
                        }
                    case 3:
                        { 
                            int id = GetId();

                            var s = studentRepository.GetById(id);
                            if (s is null)
                            {
                                Console.WriteLine("Объект не найден");
                                break;
                            }

                            Console.WriteLine($"Удалить студента {s.FirstName} {s.LastName} ? (y/n)");
                            if (GetAnsver())
                            {
                                studentRepository.DeleteById(id);
                                Console.WriteLine("Успешно удалено");
                            }
                            break;
                        }
                    case 4:
                        GetAllAndPrint(studyGroupRepository);
                        break;
                    case 5:
                        {
                            Console.WriteLine("Введите название группы");
                            string title = Console.ReadLine();

                            studyGroupRepository.Add(new StudyGroup
                            {
                                Title = title,
                            });
                            Console.WriteLine("Успешно добавлено");
                            break;
                        }
                    case 6:
                        {
                            int id = GetId();

                            var g = studyGroupRepository.GetById(id);
                            if (g is null)
                            {
                                Console.WriteLine("Объект не найден");
                                break;
                            }

                            Console.WriteLine($"Удалить группу {g.Title}  ? (y/n)");
                            if (GetAnsver())
                            {
                                studyGroupRepository.DeleteById(id);
                                Console.WriteLine("Успешно удалено");
                            }
                            break;
                        }
                    case 7:
                        {
                            int studentId = GetId("студента");
                            var s = studentRepository.GetById(studentId);
                            if (s is null)
                            {
                                Console.WriteLine("Студент не найден");
                                break;
                            }

                            int groupId = GetId("группы");
                            var g = studyGroupRepository.GetById(groupId);
                            if (g is null)
                            {
                                Console.WriteLine("Группа не найдена");
                                break;
                            }

                            studentsAndGroupsRepository.AddStudentIntoGroup(studentId, groupId);
                            break;
                        }
                    case 8:
                        {
                            int groupId = GetId("группы");
                            var g = studyGroupRepository.GetById(groupId);
                            if (g is null)
                            {
                                Console.WriteLine("Группа не найдена");
                                break;
                            }

                            var students = studentsAndGroupsRepository.GetStudentsBuGroupId(groupId);
                            foreach (var student in students)
                            {
                                Console.WriteLine($"{student.FirstName} {student.LastName}");
                            }
                            break;
                        }
                    case 9:
                        {
                            const int space = 10;
                            Console.WriteLine($"{"Id",space} | {"Название",space} | {"Количество",space}");
                            Console.WriteLine($"{"группы",space} | {"группы",space} | {"студентов",space}");
                            Console.WriteLine("----------------------------------------------------------------------");

                            var groups = studyGroupRepository.GetAll();
                            foreach (var g in groups)
                            {
                                int count = studentsAndGroupsRepository.GetStudentsCountInGroup(g.Id);
                                Console.WriteLine($"{g.Id, space} | {g.Title, space} | {count, space}");
                            }
                            break;
                        }
                    default:
                        break;
                    }
            }
        }
    }
}
