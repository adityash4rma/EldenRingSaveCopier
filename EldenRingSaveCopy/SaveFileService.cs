using System;
using System.Security.Cryptography;
using EldenRingSaveCopy.Saves.Model;

namespace EldenRingSaveCopy
{
    public static class SaveFileService
    {
        public static byte[] CreateUpdatedSaveFile(SaveGame sourceSave, SaveGame targetSave, byte[] targetFile, byte[] sourceId, byte[] targetId)
        {
            if (sourceSave == null)
            {
                throw new ArgumentNullException(nameof(sourceSave));
            }

            if (targetSave == null)
            {
                throw new ArgumentNullException(nameof(targetSave));
            }

            if (targetFile == null)
            {
                throw new ArgumentNullException(nameof(targetFile));
            }

            if (sourceId == null)
            {
                throw new ArgumentNullException(nameof(sourceId));
            }

            if (targetId == null)
            {
                throw new ArgumentNullException(nameof(targetId));
            }

            var updatedFile = (byte[])targetFile.Clone();
            var workingSaveData = (byte[])sourceSave.SaveData.Clone();

            foreach (int index in workingSaveData.FindSubArrayIndices(sourceId))
            {
                Array.Copy(targetId, 0, workingSaveData, index, targetId.Length);
            }

            int destinationSlotStart = SaveGame.SlotStartIndex(targetSave.Index);
            int destinationHeaderStart = SaveGame.HeaderStartIndex(targetSave.Index);

            Array.Copy(workingSaveData, 0, updatedFile, destinationSlotStart, SaveGame.SLOT_LENGTH);
            Array.Copy(sourceSave.HeaderData, 0, updatedFile, destinationHeaderStart, SaveGame.SAVE_HEADER_LENGTH);
            updatedFile[SaveGame.CHAR_ACTIVE_STATUS_START_INDEX + targetSave.Index] = 0x01;

            using (var md5 = MD5.Create())
            {
                md5.ComputeHash(workingSaveData);
                Array.Copy(md5.Hash, 0, updatedFile, destinationSlotStart - 0x10, 0x10);
                md5.ComputeHash(updatedFile, SaveGame.SAVE_HEADERS_SECTION_START_INDEX, SaveGame.SAVE_HEADERS_SECTION_LENGTH);
                Array.Copy(md5.Hash, 0, updatedFile, SaveGame.SAVE_HEADERS_SECTION_START_INDEX - 0x10, 0x10);
            }

            return updatedFile;
        }
    }
}
