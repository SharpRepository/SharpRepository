using System.Collections.Generic;

namespace SharpRepository.Tests.TestObjects
{
    public class ContactType
    {
        public int ContactTypeId { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }

        public override bool Equals(object obj)
        {
            var cotactType = obj as ContactType;
            return GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            var hashCode = -591342771;
            hashCode = hashCode * -1521134295 + ContactTypeId.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Abbreviation);
            return hashCode;
        }
    }
}
