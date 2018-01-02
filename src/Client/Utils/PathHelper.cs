﻿using System.IO;

namespace TDL.Client.Utils
{
    public static class PathHelper
    {
        public static string RepositoryPath { get; }

        static PathHelper()
        {
            var exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var srcPath = FindParent(exePath, "bin");

            //TODO: Find a path in a more reliable way.
            RepositoryPath = new DirectoryInfo(srcPath).Parent.Parent.Parent.FullName;
        }

        private static string FindParent(string path, string parentName)
        {
            while (true)
            {
                var directory = new DirectoryInfo(path);

                if (directory.Parent == null)
                {
                    return null;
                }

                if (directory.Parent.Name == parentName)
                {
                    return directory.Parent.FullName;
                }

                path = directory.Parent.FullName;
            }
        }
    }
}
