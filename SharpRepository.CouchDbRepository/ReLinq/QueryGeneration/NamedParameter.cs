
namespace SharpRepository.CouchDbRepository.ReLinq.QueryGeneration
{
    public class NamedParameter
    {
        public NamedParameter(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public object Value { get; set; }
    }
}
