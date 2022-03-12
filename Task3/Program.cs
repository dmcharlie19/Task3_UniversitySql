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
            while ( true )
            {
                var a = Console.ReadLine();

                if ( a == "y" )
                    return true;
                if ( a == "n" )
                    return false;
            }
        }

        static int GetId( string objName = null )
        {
            Console.WriteLine( $"Введите id {objName}" );
            while ( true )
            {
                var s = Console.ReadLine();
                try
                {
                    int id = Convert.ToInt32( s );
                    return id;
                }
                catch ( Exception )
                {
                    Console.WriteLine( "введите число!" );
                }
            }
        }

        static void Main( string[] args )
        {
            var studentRepository = new CommonOperationsSqlRepository<Student>( ConnectionString );
            var studyGroupRepository = new CommonOperationsSqlRepository<StudyGroup>( ConnectionString );
            var studentsAndGroupsRepository = new StudentsAndGroupsSqlRepository( ConnectionString );

            while ( true )
            {
                Console.WriteLine( "    1 - получить список всех студентов" );
                Console.WriteLine( "    2 - Добавить студента" );
                Console.WriteLine( "    3 - Удалить студента по номеру" );
                Console.WriteLine( "    4 - получить список всех групп" );
                Console.WriteLine( "    5 - Добавить группу" );
                Console.WriteLine( "    6 - Удалить группу по номеру" );
                Console.WriteLine( "    7 - Добавить студента в группу" );
                Console.WriteLine( "    8 - получить студентов по id группы" );
                Console.WriteLine( "    9 - получить отчет по количеству cтудентов в группах" );
                Console.WriteLine( "" );
                Console.WriteLine( "Введите номер команды" );

                int command = 0;
                try
                {
                    command = Convert.ToInt32( Console.ReadLine() );
                }
                catch ( Exception )
                {
                    Console.WriteLine( "введите число!" );
                }

                switch ( command )
                {
                    case 1:
                        {
                            var students = studentRepository.GetAll();
                            foreach ( var student in students )
                            {
                                Console.WriteLine( student.ToString() );
                            }
                            break;
                        }
                    case 2:
                        {
                            Console.WriteLine( "Введите имя студента" );
                            string firstName = Console.ReadLine();

                            Console.WriteLine( "Введите фамилию студента" );
                            string lastName = Console.ReadLine();

                            studentRepository.Add( new Student
                            {
                                FirstName = firstName,
                                LastName = lastName,
                            } );
                            Console.WriteLine( "Успешно добавлено" );
                            break;
                        }
                    case 3:
                        {
                            int id = GetId();

                            var student = studentRepository.GetById( id );
                            if ( student is null )
                            {
                                Console.WriteLine( "Объект не найден" );
                                break;
                            }

                            Console.WriteLine( $"Удалить студента {student.FirstName} {student.LastName} ? (y/n)" );
                            if ( GetAnsver() )
                            {
                                studentRepository.DeleteById( id );
                                Console.WriteLine( "Успешно удалено" );
                            }
                            break;
                        }
                    case 4:
                        {
                            var groups = studentRepository.GetAll();
                            foreach ( var group in groups )
                            {
                                Console.WriteLine( group.ToString() );
                            }
                            break;
                        }
                    case 5:
                        {
                            Console.WriteLine( "Введите название группы" );
                            string title = Console.ReadLine();

                            studyGroupRepository.Add( new StudyGroup
                            {
                                Title = title,
                            } );
                            Console.WriteLine( "Успешно добавлено" );
                            break;
                        }
                    case 6:
                        {
                            int id = GetId();

                            var group = studyGroupRepository.GetById( id );
                            if ( group is null )
                            {
                                Console.WriteLine( "Объект не найден" );
                                break;
                            }

                            Console.WriteLine( $"Удалить группу {group.Title}  ? (y/n)" );
                            if ( GetAnsver() )
                            {
                                studyGroupRepository.DeleteById( id );
                                Console.WriteLine( "Успешно удалено" );
                            }
                            break;
                        }
                    case 7:
                        {
                            int studentId = GetId( "студента" );
                            var student = studentRepository.GetById( studentId );
                            if ( student is null )
                            {
                                Console.WriteLine( "Студент не найден" );
                                break;
                            }

                            int groupId = GetId( "группы" );
                            var group = studyGroupRepository.GetById( groupId );
                            if ( group is null )
                            {
                                Console.WriteLine( "Группа не найдена" );
                                break;
                            }

                            studentsAndGroupsRepository.AddStudentIntoGroup( studentId, groupId );
                            break;
                        }
                    case 8:
                        {
                            int groupId = GetId( "группы" );
                            var group = studyGroupRepository.GetById( groupId );
                            if ( group is null )
                            {
                                Console.WriteLine( "Группа не найдена" );
                                break;
                            }

                            var students = studentsAndGroupsRepository.GetStudentsBuGroupId( groupId );
                            foreach ( var student in students )
                            {
                                Console.WriteLine( $"{student.FirstName} {student.LastName}" );
                            }
                            break;
                        }
                    case 9:
                        {
                            const int space = 10;
                            Console.WriteLine( $"{"Id",space} | {"Название",space} | {"Количество",space}" );
                            Console.WriteLine( $"{"группы",space} | {"группы",space} | {"студентов",space}" );
                            Console.WriteLine( "----------------------------------------------------------------------" );

                            var groups = studyGroupRepository.GetAll();
                            foreach ( var group in groups )
                            {
                                int count = studentsAndGroupsRepository.GetStudentsCountInGroup( group.Id );
                                Console.WriteLine( $"{group.Id,space} | {group.Title,space} | {count,space}" );
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
