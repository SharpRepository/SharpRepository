namespace SharpRepository.Tests.Integration.TestObjects
{
    public class ConventionTestItem1
    {
        public int ConventionTestItem1Key { get; set; }
        public string Name { get; set; }
    }

    public class ConventionTestItem2
    {
        public int Key { get; set; }
        public string Name { get; set; }
    }

    public class ConventionTestItem3
    {
        public int SomeRandomPrimaryKeyProperty { get; set; }
        public string Name { get; set; }
    }
}
