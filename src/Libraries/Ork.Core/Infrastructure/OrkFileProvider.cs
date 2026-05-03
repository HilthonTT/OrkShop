using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System.Runtime.Versioning;
using System.Security.AccessControl;
using System.Text;

namespace Ork.Core.Infrastructure;

public sealed class OrkFileProvider : PhysicalFileProvider, IOrkFileProvider
{
    public OrkFileProvider(IWebHostEnvironment webHostEnvironment)
        : base(ResolveContentRootPath(webHostEnvironment.ContentRootPath))
    {
        WebRootPath = ResolveWebRootPath(webHostEnvironment.WebRootPath);
    }

    public string WebRootPath { get; set; }

    public string Combine(params string[] paths)
    {
        string path = Path.Combine(paths.SelectMany(p => IsUncPath(p) ? [p] : p.Split('\\', '/')).ToArray());

        if (Environment.OSVersion.Platform == PlatformID.Unix && !IsUncPath(path))
        {
            //add leading slash to correctly form path in the UNIX system
            path = "/" + path;
        }

        return path;
    }

    public void CreateDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public void CreateFile(string path)
    {
        if (File.Exists(path))
        {
            return;
        }

        var fileInfo = new FileInfo(path);

        if (!string.IsNullOrWhiteSpace(fileInfo.DirectoryName))
        {
            CreateDirectory(fileInfo.DirectoryName);
        }

        // we use 'using' to close the file after it's created
        using (File.Create(path))
        {
        }
    }

    public async Task DeleteDirectoryAsync(string path)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(path, nameof(path));

        // find more info about directory deletion
        // and why we use this approach at https://stackoverflow.com/questions/329355/cannot-delete-directory-with-directory-deletepath-true
        foreach (var directory in Directory.GetDirectories(path))
        {
            await DeleteDirectoryAsync(directory);
        }

        try
        {
            await DeleteDirectoryRecursiveAsync(path);
        }
        catch (IOException)
        {
            await DeleteDirectoryRecursiveAsync(path);
        }
        catch (UnauthorizedAccessException)
        {
            await DeleteDirectoryRecursiveAsync(path);
        }
    }

    public void DeleteFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return;
        }

        File.Delete(filePath);
    }

    public bool DirectoryExists(string path)
    {
        return Directory.Exists(path);
    }

    public void DirectoryMove(string sourceDirName, string destDirName)
    {
        Directory.Move(sourceDirName, destDirName);
    }

    public IEnumerable<string> EnumerateFiles(string directoryPath, string searchPattern, bool topDirectoryOnly = true)
    {
        return Directory.EnumerateFiles(directoryPath, searchPattern,
            topDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories);
    }

    public void FileCopy(string sourceFileName, string destFileName, bool overwrite = false)
    {
        File.Copy(sourceFileName, destFileName, overwrite);
    }

    public bool FileExists(string filePath)
    {
        return File.Exists(filePath);
    }

    public long FileLength(string path)
    {
        if (!File.Exists(path))
        {
            return -1;
        }

        return new FileInfo(path).Length;
    }

    public void FileMove(string sourceFileName, string destFileName)
    {
        File.Move(sourceFileName, destFileName);
    }

    public string GetAbsolutePath(params string[] paths)
    {
        var allPaths = new List<string>();

        if (paths.Length > 0 && !paths[0].Contains(WebRootPath, StringComparison.InvariantCulture))
        {
            allPaths.Add(WebRootPath);
        }

        allPaths.AddRange(paths);

        return Combine(allPaths.ToArray());
    }

    [SupportedOSPlatform("windows")]
    public DirectorySecurity GetAccessControl(string path)
    {
        return new DirectoryInfo(path).GetAccessControl();
    }

    public DateTime GetCreationTime(string path)
    {
        return File.GetCreationTime(path);
    }

    public string[] GetDirectories(string path, string searchPattern = "", bool topDirectoryOnly = true)
    {
        if (string.IsNullOrEmpty(searchPattern))
        {
            searchPattern = "*";
        }

        return Directory.GetDirectories(path, searchPattern,
            topDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories);
    }

    public string? GetDirectoryName(string path)
    {
        return Path.GetDirectoryName(path);
    }

    public string GetDirectoryNameOnly(string path)
    {
        return new DirectoryInfo(path).Name;
    }

    public string GetFileExtension(string filePath)
    {
        return Path.GetExtension(filePath);
    }

    public string GetFileName(string path)
    {
        return Path.GetFileName(path);
    }

    public string GetFileNameWithoutExtension(string filePath)
    {
        return Path.GetFileNameWithoutExtension(filePath);
    }

    public string[] GetFiles(string directoryPath, string searchPattern = "", bool topDirectoryOnly = true)
    {
        if (string.IsNullOrEmpty(searchPattern))
        {
            searchPattern = "*";
        }

        return Directory.GetFileSystemEntries(directoryPath, searchPattern,
            new EnumerationOptions
            {
                IgnoreInaccessible = true,
                MatchCasing = MatchCasing.CaseInsensitive,
                RecurseSubdirectories = !topDirectoryOnly,

            });
    }

    public DateTime GetLastAccessTime(string path)
    {
        return File.GetLastAccessTime(path);
    }

    public DateTime GetLastWriteTime(string path)
    {
        return File.GetLastWriteTime(path);
    }

    public DateTime GetLastWriteTimeUtc(string path)
    {
        return File.GetLastWriteTimeUtc(path);
    }

    public FileStream GetOrCreateFile(string path)
    {
        if (File.Exists(path))
        {
            return File.Open(path, FileMode.Open, FileAccess.ReadWrite);
        }

        var fileInfo = new FileInfo(path);

        if (string.IsNullOrWhiteSpace(fileInfo.DirectoryName))
        {
            CreateDirectory(path);
        }

        return File.Create(path);
    }

    public string? GetParentDirectory(string directoryPath)
    {
        return Directory.GetParent(directoryPath)?.FullName;
    }

    public string? GetVirtualPath(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return path;
        }

        if (!IsDirectory(path) && FileExists(path))
        {
            path = new FileInfo(path).DirectoryName;
        }

        path = path?.Replace(WebRootPath, string.Empty).Replace('\\', '/').Trim('/').TrimStart('~', '/');

        return $"~/{path ?? string.Empty}";
    }

    public bool IsDirectory(string path)
    {
        return DirectoryExists(path);
    }

    public bool IsPathRooted(string path)
    {
        return !string.IsNullOrEmpty(path) && Path.IsPathRooted(path);
    }

    public string MapPath(string path)
    {
        path = path.Replace("~/", string.Empty).TrimStart('/');

        //if virtual path has slash on the end, it should be after transform the virtual path to physical path too
        var pathEnd = path.EndsWith('/') ? Path.DirectorySeparatorChar.ToString() : string.Empty;

        return Combine(Root ?? string.Empty, path) + pathEnd;
    }

    public async Task<byte[]> ReadAllBytesAsync(string filePath)
    {
        return File.Exists(filePath) ? await File.ReadAllBytesAsync(filePath) : [];
    }

    public string ReadAllText(string path, Encoding encoding)
    {
        using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var streamReader = new StreamReader(fileStream, encoding);

        return streamReader.ReadToEnd();
    }

    public async Task<string> ReadAllTextAsync(string path, Encoding encoding)
    {
        await using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var streamReader = new StreamReader(fileStream, encoding);

        return await streamReader.ReadToEndAsync();
    }

    public async Task WriteAllBytesAsync(string filePath, byte[] bytes)
    {
        await File.WriteAllBytesAsync(filePath, bytes);
    }

    public void WriteAllText(string path, string contents, Encoding encoding)
    {
        File.WriteAllText(path, contents, encoding);
    }

    public async Task WriteAllTextAsync(string path, string contents, Encoding encoding)
    {
        await File.WriteAllTextAsync(path, contents, encoding);
    }

    private static string ResolveContentRootPath(string contentRootPath)
    {
        if (File.Exists(contentRootPath))
        {
            return Path.GetDirectoryName(contentRootPath)!;
        }

        return contentRootPath;
    }

    private static string ResolveWebRootPath(string webRootPath)
    {
        if (File.Exists(webRootPath))
        {
            return Path.GetDirectoryName(webRootPath) ?? webRootPath;
        }

        return webRootPath;
    }

    private static async Task DeleteDirectoryRecursiveAsync(string directoryPath)
    {
        Directory.Delete(directoryPath, true);

        const int maxInterationToWait = 10;
        int curIteration = 0;

        //according to the documentation(https://msdn.microsoft.com/ru-ru/library/windows/desktop/aa365488.aspx) 
        //System.IO.Directory.Delete method ultimately (after removing the files) calls native 
        //RemoveDirectory function which marks the directory as "deleted". That's why we wait until 
        //the directory is actually deleted. For more details see https://stackoverflow.com/a/4245121
        while (Directory.Exists(directoryPath))
        {
            curIteration += 1;

            if (curIteration > maxInterationToWait)
            {
                return;
            }

            await Task.Delay(100);
        }
    }

    /// <summary>
    /// Determines if the string is a valid Universal Naming Convention (UNC)
    /// for a server and share path.
    /// </summary>
    /// <param name="path">The path to be tested.</param>
    /// <returns><see langword="true"/> if the path is a valid UNC path; 
    /// otherwise, <see langword="false"/>.</returns>
    private static bool IsUncPath(string path)
    {
        return Uri.TryCreate(path, UriKind.Absolute, out var uri) && uri.IsUnc;
    }
}
