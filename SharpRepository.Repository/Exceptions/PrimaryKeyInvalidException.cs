using System.Runtime.Serialization;

namespace SharpRepository.Repository.Exceptions
{
    // http://msdn.microsoft.com/en-us/library/ms229064.aspx
    // http://blogs.msdn.com/b/jaredpar/archive/2008/10/20/custom-exceptions-when-should-you-create-them.aspx
    public class PrimaryKeyInvalidException : System.Exception, ISerializable
    {
        public PrimaryKeyInvalidException()
        {
            
            // Add implementation.
        }
        public PrimaryKeyInvalidException(string message) : base (message)
        {
            
        }

        public PrimaryKeyInvalidException(string message, System.Exception inner) : base( message, inner)
        {
           
        }

        // This constructor is needed for serialization.
        protected PrimaryKeyInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
          
        }
    }
}
