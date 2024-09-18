namespace DevSchool.Entities
{
    public class Student
    {
        public int Id { get; private set; }
        public string FullName { get; private set; }
        public DateTime Birthdate { get; private set; }
        public string SchoolClass { get; private set; }
        public bool IsActive { get; private set; }

        public Student() { }

        public Student(string fullName, DateTime birthdate, string schoolClass) 
        {
            FullName = fullName;
            Birthdate = birthdate;
            SchoolClass = schoolClass;
            IsActive = true;
        }
    }
}
