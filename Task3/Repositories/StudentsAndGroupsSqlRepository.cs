using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using UniversitySql.Models;

namespace UniversitySql.Repositories
{
    class StudentsAndGroupsSqlRepository : IStudentsAndGroupsSqlRepository
    {
        private readonly string _connectionString;

        public StudentsAndGroupsSqlRepository( string connectionString )
        {
            _connectionString = connectionString;
        }

        public void AddStudentIntoGroup( int studentId, int groupId )
        {
            using ( var connection = new SqlConnection( _connectionString ) )
            {
                connection.Open();
                using ( SqlCommand сommand = connection.CreateCommand() )
                {
                    сommand.CommandText =
                        @"insert into [StudentsAndGroups]
                        ([StudentId], [StudyGroupId])
                    values
                        (@studentId, @groupId)";

                    сommand.Parameters.Add( "@studentId", SqlDbType.NVarChar ).Value = studentId;
                    сommand.Parameters.Add( "@groupId", SqlDbType.NVarChar ).Value = groupId;

                    сommand.ExecuteNonQuery();
                }
            }
        }

        public List<Student> GetStudentsBuGroupId( int groupId )
        {
            var result = new List<Student>();

            using ( var connection = new SqlConnection( _connectionString ) )
            {
                connection.Open();
                using ( SqlCommand сommand = connection.CreateCommand() )
                {
                    сommand.CommandText =
                        @"select [FirstName], [LastName] from StudentsAndGroups as studentsAndGroups
                            join Student as s on s.Id = studentsAndGroups.StudentId
                            where studentsAndGroups.StudyGroupId = @groupId";

                    сommand.Parameters.Add( "@groupId", SqlDbType.NVarChar ).Value = groupId;

                    using ( SqlDataReader reader = сommand.ExecuteReader() )
                    {
                        while ( reader.Read() )
                        {
                            result.Add( new Student()
                            {
                                FirstName = Convert.ToString( reader[ "FirstName" ] ),
                                LastName = Convert.ToString( reader[ "LastName" ] )
                            } );
                        }
                    }
                }
            }
            return result;
        }

        public int GetStudentsCountInGroup( int groupId )
        {
            int res = 0;

            using ( var connection = new SqlConnection( _connectionString ) )
            {
                connection.Open();
                using ( SqlCommand сommand = connection.CreateCommand() )
                {
                    сommand.CommandText =
                        @"select count(*) from StudentsAndGroups as studentsAndGroups
                            join Student as s on s.Id = studentsAndGroups.StudentId
                            where studentsAndGroups.StudyGroupId = @groupId";

                    сommand.Parameters.Add( "@groupId", SqlDbType.NVarChar ).Value = groupId;

                    res = Convert.ToInt32( сommand.ExecuteScalar() );
                }
            }
            return res;
        }
    }
}

