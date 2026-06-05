using System;
using System.Text;

namespace EldenRingSaveCopy.Saves.Model
{
    public class SaveGame
    {
        public const int SLOT_START_INDEX = 0x310;
        public const int SLOT_LENGTH = 0x280000;
        public const int SAVE_HEADERS_SECTION_START_INDEX = 0x19003B0;
        public const int SAVE_HEADERS_SECTION_LENGTH = 0x60000;
        public const int SAVE_HEADER_START_INDEX = 0x1901D0E;
        public const int SAVE_HEADER_LENGTH = 0x24C;
        public const int CHAR_ACTIVE_STATUS_START_INDEX = 0x1901D04;

        private const int CHAR_NAME_LENGTH = 0x22;
        private const int CHAR_LEVEL_LOCATION = 0x22;
        private const int CHAR_PLAYED_START_INDEX = 0x26;

        public SaveGame()
        {
            Active = false;
            CharacterName = string.Empty;
            SaveData = Array.Empty<byte>();
            HeaderData = Array.Empty<byte>();
            Id = Guid.NewGuid();
            Index = -1;
        }

        public int Index { get; set; }
        public bool Active { get; set; }
        public string CharacterName { get; set; }
        public byte[] SaveData { get; set; }
        public byte[] HeaderData { get; set; }
        public Guid Id { get; }
        public int CharacterLevel { get; set; }
        public int SecondsPlayed { get; set; }

        public static int SlotStartIndex(int slotIndex)
        {
            return SLOT_START_INDEX + (slotIndex * 0x10) + (slotIndex * SLOT_LENGTH);
        }

        public static int HeaderStartIndex(int slotIndex)
        {
            return SAVE_HEADER_START_INDEX + (slotIndex * SAVE_HEADER_LENGTH);
        }

        public bool LoadData(byte[] data, int slotIndex)
        {
            if (data == null || slotIndex < 0 || slotIndex > 9)
            {
                return false;
            }

            int activeStatusIndex = CHAR_ACTIVE_STATUS_START_INDEX + slotIndex;
            int headerStart = SAVE_HEADER_START_INDEX + (slotIndex * SAVE_HEADER_LENGTH);
            int slotStart = SLOT_START_INDEX + (slotIndex * 0x10) + (slotIndex * SLOT_LENGTH);

            if (data.Length < activeStatusIndex + 1 || data.Length < headerStart + SAVE_HEADER_LENGTH || data.Length < slotStart + SLOT_LENGTH)
            {
                return false;
            }

            Index = slotIndex;
            Active = data[activeStatusIndex] == 1;
            CharacterName = Encoding.Unicode.GetString(data, headerStart, CHAR_NAME_LENGTH).TrimEnd('\0');
            CharacterLevel = data[headerStart + CHAR_LEVEL_LOCATION];
            SecondsPlayed = BitConverter.ToInt32(data, headerStart + CHAR_PLAYED_START_INDEX);
            SaveData = new byte[SLOT_LENGTH];
            HeaderData = new byte[SAVE_HEADER_LENGTH];
            Array.Copy(data, slotStart, SaveData, 0, SLOT_LENGTH);
            Array.Copy(data, headerStart, HeaderData, 0, SAVE_HEADER_LENGTH);
            return true;
        }
    }
}
