using SharpRepository.Samples.Core3Mvc.Models;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpRepository.Samples.Core3Mvc.CustomRepositories
{
    public class EmailRepository : ConfigurationBasedRepository<Contact, string>
    {
        public EmailRepository(ISharpRepositoryConfiguration configuration, string repositoryName = null) : base(configuration, repositoryName)
        {
        }

        public IEnumerable<string> GetMails()
        {
            return this.AsQueryable().Where(c => c.Emails != null & c.Emails.Any()).SelectMany(c => c.Emails).Select(m => m.EmailAddress)
                .Distinct();
        }
    }
}
