using System;

namespace EldenRingSaveCopy
{
    public class FileManager
    {
        private const int ID_LOCATION = 0x19003B4;
        private const int ID_LENGTH = 8;

        private byte[] sourceFile = Array.Empty<byte>();
        private byte[] targetFile = Array.Empty<byte>();
        private readonly byte[] sourceID = new byte[ID_LENGTH];
        private readonly byte[] targetID = new byte[ID_LENGTH];
        private string sourcePath = string.Empty;
        private string targetPath = string.Empty;

        public byte[] SourceFile
        {
            get => sourceFile;
            set
            {
                sourceFile = value ?? Array.Empty<byte>();
                ExtractId(sourceFile, sourceID);
            }
        }

        public byte[] TargetFile
        {
            get => targetFile;
            set
            {
                targetFile = value ?? Array.Empty<byte>();
                ExtractId(targetFile, targetID);
            }
        }

        public string TargetPath
        {
            get => targetPath;
            set => targetPath = value ?? string.Empty;
        }

        public string SourcePath
        {
            get => sourcePath;
            set => sourcePath = value ?? string.Empty;
        }

        public byte[] TargetID => targetID;
        public byte[] SourceID => sourceID;
        public bool HasValidSource => sourceFile.Length >= ID_LOCATION + ID_LENGTH;
        public bool HasValidTarget => targetFile.Length >= ID_LOCATION + ID_LENGTH;

        private static void ExtractId(byte[] file, byte[] destination)
        {
            if (file != null && file.Length >= ID_LOCATION + ID_LENGTH)
            {
                Array.Copy(file, ID_LOCATION, destination, 0, ID_LENGTH);
            }
            else
            {
                Array.Clear(destination, 0, destination.Length);
            }
        }
    }
}
