using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SharpRepository.Repository;

namespace SharpRepository.Tests.TestObjects.PrimaryKeys
{
    public class TripleObjectKeys
    {
        public int Id { get; set; }
        
        [Key, Column(Order = 0)]
        [RepositoryPrimaryKey(Order = 0)]
        public int KeyInt1 { get; set; }

        [Key, Column(Order = 1)]
        [RepositoryPrimaryKey(Order = 1)]
        public int KeyInt2 { get; set; }

        [Key, Column(Order = 2)]
        [RepositoryPrimaryKey(Order = 2)]
        public int KeyInt3 { get; set; }
    }
}
