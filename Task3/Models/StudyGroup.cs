namespace UniversitySql.Models
{
    class StudyGroup
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public override string ToString()
        {
            return $"{Id} {Title}";
        }
    }
}
