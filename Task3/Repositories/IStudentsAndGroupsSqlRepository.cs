using System.Collections.Generic;
using UniversitySql.Models;

namespace UniversitySql.Repositories
{
    interface IStudentsAndGroupsSqlRepository
    {
        void AddStudentIntoGroup(int studentId, int groupId);
        List<Student> GetStudentsBuGroupId(int groupId);
        int GetStudentsCountInGroup(int groupId);
    }
}