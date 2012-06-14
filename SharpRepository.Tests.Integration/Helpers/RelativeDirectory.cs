using System;
using System.IO;

namespace SharpRepository.Tests.Integration.Helpers
{
    public class RelativeDirectory
    {
        protected DirectoryInfo DirectoryInfo;

        /// <summary>
        /// Sets the relative directory location based on a known absolute path
        /// </summary>
        /// <param name="path">A string specifying the path on which to create the DirectoryInfo.</param>
        public RelativeDirectory(string path)
        {
            DirectoryInfo = new DirectoryInfo(path);
        }

        /// <summary>
        /// Returns the name of the "current" directory 
        /// </summary>
        public string Name
        {
            get { return DirectoryInfo.Name; }
        }

        /// <summary>
        /// Returns the full path of the "current" directory 
        /// </summary>
        public string FullName
        {
            get { return DirectoryInfo.FullName; }
        }

        /// <summary>
        /// Navigates up the directory tree to the directory specified by name.
        /// </summary>
        /// <param name="directoryName">Name of directory to navigate</param>
        /// <returns>True if found, false if not found</returns>
        public Boolean MoveUpToDirectory(string directoryName)
        {
            do
            {
                if (DirectoryInfo.Name.Equals(directoryName))
                {
                    return true;
                }
            } while (MoveUpToDirectory());

            return false;
        }

        /// <summary>
        /// Navigates up the directory tree to the number of levels specified.
        /// </summary>
        /// <param name="numLevels">Number of levels to navigate</param>
        /// <returns>True if found, false if not found</returns>
        public Boolean MoveUpToDirectory(int numLevels = 1)
        {
            for (int i = 0; i < numLevels; i++)
            {
                DirectoryInfo directoryInfo = DirectoryInfo.Parent;
                if (directoryInfo != null)
                    DirectoryInfo = directoryInfo;
                else
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Navigates down the directory tree one level to the specified name
        /// </summary>
        /// <param name="match">Starts-with name of directory to navigate</param>
        /// <returns>True if found, false if not found</returns>
        public Boolean MoveDownToDirectory(string match)
        {
            DirectoryInfo[] dirs = DirectoryInfo.GetDirectories(match + '*');

            if (dirs.Length == 0) return false;

            DirectoryInfo = dirs[0];
            return true;
        }
    }
}
