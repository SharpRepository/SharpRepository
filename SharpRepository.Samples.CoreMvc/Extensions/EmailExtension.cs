using SharpRepository.CoreMvc.Models;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using SharpRepository.Repository.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpRepository.Samples.CoreMvc.Extensions
{
    public static class EmailRepositoryExtension
    {

        public static IEnumerable<string> GetMails(this IRepository<Email, int> repo)
        {
            return repo.GetAll(m => m.EmailAddress, new DistinctPagingOptions<Email>(1, 2, "EmailAddress"));
        }
    }
}
