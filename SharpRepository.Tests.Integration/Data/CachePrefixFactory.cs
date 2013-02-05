namespace SharpRepository.Tests.Integration.Data
{
    public class CachePrefixFactory
    {
        private static int _num = 1;

        public static string Build()
        {
            _num++; // since it goes through and calls this for each test before running them, we need a different database for each test or else the auto increment goes to 2 on the second test with an add and it fails
                    // surprisingly the timing isn;'t too bad for this, the first test takes about 7 secs for EF to model the DB and create it, then each other test is super quick in creating the DB file


            return _num.ToString();
        }
    }
}
