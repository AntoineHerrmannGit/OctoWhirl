using OctoWhirl.Core.Tools.Technicals.Extensions;

namespace OctoWhirl.Core.Tools.Technicals.FileManagement
{
    public static class FileManager
    {
        private static List<string> IgnoredDirs = new List<string>
        {
            "bin", "obj", "src", ".vs", ".git"
        };

        public static string FindDirPath(string dirname, string? root = null)
        {
            if (dirname.IsNull())
                throw new ArgumentNullException(nameof(dirname));

            if (root.IsNull())
                root = GetSolutionRoot();

            var subDirectories = new List<string> { root };
            while (subDirectories.Any())
            {
                var newSubDirs = new List<string>();
                foreach (var dir in subDirectories)
                {
                    if (dir.EndsWith(dirname))
                        return dir;
                    newSubDirs.AddRange(Directory.GetDirectories(dir).Where(d => !IgnoredDirs.Contains(d.Split("\\").Last())));
                }
                subDirectories = newSubDirs;
            }

            throw new FileNotFoundException(nameof(dirname));
        }

        public static string FindFilePath(string filename, string? root = null)
        {
            if (filename.IsNull())
                throw new ArgumentNullException(nameof(filename));
            if (filename.Contains("/") || filename.Contains("\\"))
                throw new ArgumentException(nameof(filename));

            if (root.IsNull())
                root = GetSolutionRoot();

            var subDirectories = new List<string> { root };
            var subfiles = Directory.GetFiles(root);
            while (subDirectories.Any())
            {
                var newSubDirs = new List<string>();
                foreach (var dir in subDirectories)
                {
                    foreach (var file in Directory.GetFiles(dir))
                        if (file.EndsWith(filename))
                            return file;
                    newSubDirs.AddRange(Directory.GetDirectories(dir).Where(d => !IgnoredDirs.Contains(d.Split("\\").Last())));
                }

                subDirectories = newSubDirs;
            }

            throw new FileNotFoundException(nameof(filename));
        }

        public static string GetSolutionRoot()
        {
            var root = Directory.GetCurrentDirectory();
            while (!Directory.GetFiles(root).Any(file => file.EndsWith(".sln")))
                root = Directory.GetParent(root)?.FullName ?? throw new FileNotFoundException("Impossible to find the root of the solution");
            return root;
        }
    }
}
