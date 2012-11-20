
namespace SharpRepository.Repository.Configuration
{
    public interface IRepositoryElement
    {
        // methods like LoadRepository or something that returns IRepository
        string Name { get; set; }

        IRepository<T, TKey> GetInstance<T, TKey>() where T : class, new();
    }

//    public class RepositoryElement : ConfigurationElement, IRepositoryElement
//    {
//
////        /// <summary>
////        /// Gets or sets the type of the repository.
////        /// </summary>
////        [ConfigurationProperty("type", IsRequired = true), TypeConverter(typeof(TypeNameConverter))]
////        public Type Type
////        {
////            get { return (Type)base["type"]; }
////            set
////            {
////                //ConfigurationHelper.CheckForInterface(value, typeof(T));
////                base["type"] = value;
////            }
////        }
//
////        [ConfigurationProperty("name", IsRequired = true)]
////        public string Name
////        {
////            get { return (string) base["name"]; }
////            set { base["name"] = value; }
////        }
//    }
}
