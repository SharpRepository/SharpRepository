using SharpRepository.Repository;
using System.ComponentModel.DataAnnotations;

namespace SharpRepository.Tests.TestObjects
{
    public class TripleCompoundKeyItemInts
    {
        [RepositoryPrimaryKey(Order = 1)]
        public int SomeId { get; set; }

        [RepositoryPrimaryKey(Order = 2)]
        public int AnotherId { get; set; }

        [RepositoryPrimaryKey(Order = 3)]
        public int LastId { get; set; }

        public string Title { get; set; }
    }
}
