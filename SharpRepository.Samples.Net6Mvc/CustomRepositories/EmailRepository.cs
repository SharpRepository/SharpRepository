using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using SharpRepository.Samples.Net6Mvc.Models;

namespace SharpRepository.Samples.Net6Mvc.CustomRepositories
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
