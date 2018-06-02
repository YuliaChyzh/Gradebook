using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PL.Validation
{
    public class Validate
    {
        private static readonly Lazy<String> groupName = new Lazy<String>(() => @"^([\u0410-\u044F\u0406\u0407\u0490\u0404\u0456\u0457\u0491\u0454a-zA-Z]{1,2})?-([0-9]{2})$");
        private static readonly Lazy<String> subjectName = new Lazy<String>(() => @"^[\u0410-\u044F\u0406\u0407\u0490\u0404\u0456\u0457\u0491\u0454a-zA-Z ]{1,40}$");
        private static readonly Lazy<String> studentName = new Lazy<String>(() => @"^[\u0410-\u044F\u0406\u0407\u0490\u0404\u0456\u0457\u0491\u0454a-zA-Z ]{1,40}$");
        private static readonly Lazy<String> subjectRes = new Lazy<String>(() => @"^[0-9]{1,3}$");
        
        public static String GroupName => groupName.Value;
        public static String SubjectName => subjectName.Value;
        public static String StudentName => studentName.Value;
        public static String SubjectRes => subjectRes.Value;

        public bool ValidationGroupName(string groupName)
        {
            return new Regex(GroupName).IsMatch(groupName);
        }

        public bool ValidationSubjectName(string subjectName)
        {
            return new Regex(SubjectName).IsMatch(subjectName);
        }

        public bool ValidationStudentName(string studentName)
        {
            return new Regex(StudentName).IsMatch(studentName);
        }

        public bool ValidationSubjectRes(int subjectRes)
        {
            return new Regex(SubjectRes).IsMatch(subjectRes.ToString());
        }

    }
}