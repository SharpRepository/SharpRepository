﻿using SharpRepository.CoreMvc.Models;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpRepository.Samples.CoreMvc.CustomRepositories
{
    public class EmailRepository : ConfigurationBasedRepository<Contact, string>
    {
        public EmailRepository(ISharpRepositoryConfiguration configuration, string repositoryName = null) : base(configuration, repositoryName)
        {
        }

        public IEnumerable<string> GetMails()
        {
            return this.GetAll().SelectMany(c => c.Emails)
                .Distinct();
        }
    }
}