using System;
using System.IO;

namespace SharpRepository.Tests
{
    //TODO: BG - This guts of the following code was found online. Find the source and reference.
    public class RelativeDirectory
    {
        private DirectoryInfo _dirInfo;

        public RelativeDirectory()
        {
            _dirInfo = new DirectoryInfo(AppContext.BaseDirectory);
        }

        public RelativeDirectory(string absoluteDir)
        {
            _dirInfo = new DirectoryInfo(absoluteDir);
        }

        public string Dir
        {
            get { return _dirInfo.Name; }
        }

        public string Path
        {
            get { return _dirInfo.FullName; }
            set
            {
                try
                {
                    var newDir = new DirectoryInfo(value);
                    _dirInfo = newDir;
                }
                catch
                {
                    // silent
                }
            }
        }

        public Boolean UpTo(string folderName)
        {
            do
            {
                if (_dirInfo.Name.Equals(folderName)) return true;
            } while (Up());

            return false;
        }

        public Boolean Up(int numLevels)
        {
            for (int i = 0; i < numLevels; i++)
            {
                DirectoryInfo tempDir = _dirInfo.Parent;
                if (tempDir != null)
                    _dirInfo = tempDir;
                else
                    return false;
            }
            return true;
        }

        public Boolean Up()
        {
            return Up(1);
        }
        public Boolean Down(string match)
        {
            DirectoryInfo[] dirs = _dirInfo.GetDirectories(match + '*');

            if (dirs.Length == 0) return false;

            _dirInfo = dirs[0];
            return true;
        }
    }
}
