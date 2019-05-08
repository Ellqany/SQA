using System.Collections.Generic;

namespace SQA.Models.FacultyContext
{
    public class Department
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string FacultyID { get; set; }
    }
    public class GetDepartments
    {
        public List<Department> Departments { get; set; }
        public string Department { get; set; }
    }
}
