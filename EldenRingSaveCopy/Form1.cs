using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using EldenRingSaveCopy.Saves.Model;

namespace EldenRingSaveCopy
{
    public partial class Form1 : Form
    {
        private FileManager _fileManager;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        private const int MESSAGE_ERROR = 0;
        private const int MESSAGE_INFO = 1;
        private const int MESSAGE_SUCCESS = 2;

        private BindingList<SaveGame> sourceSaveGames = new BindingList<SaveGame>();
        private BindingList<SaveGame> targetSaveGames = new BindingList<SaveGame>();

        private SaveGame selectedSourceSave;
        private SaveGame selectedTargetSave;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                 int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Form1_MouseDown(object sender,
        System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        public Form1()
        {
            InitializeComponent();
            showAdditionalInfoMessage(MESSAGE_INFO, "Select Source and Destination files and characters");
        }

        private string GetDefaultEldenRingSaveDirectory()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string eldenRingPath = System.IO.Path.Combine(appData, "EldenRing");

            return System.IO.Directory.Exists(eldenRingPath) ? eldenRingPath : appData;
        }

        private void sourceFileBrowse(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = GetDefaultEldenRingSaveDirectory();
                openFileDialog.Filter = "Elden Ring Save File |ER0000.sl2|Elden Ring Coop Save File |ER0000.co2";

                if (openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                sourceSaveGames.Clear();
                selectedSourceSave = null;
                _fileManager.SourceFile = Array.Empty<byte>();
                _fileManager.SourcePath = openFileDialog.FileName;

                try
                {
                    _fileManager.SourceFile = File.ReadAllBytes(_fileManager.SourcePath);
                    sourceFilePath.Text = _fileManager.SourcePath;

                    for (int i = 0; i < 10; i++)
                    {
                        var newSave = new SaveGame();
                        if (newSave.LoadData(_fileManager.SourceFile, i) && newSave.Active)
                        {
                            sourceSaveGames.Add(newSave);
                        }
                    }

                    if (sourceSaveGames.Count > 0)
                    {
                        fromSaveSlot.SelectedIndex = 0;
                        selectedSourceSave = (SaveGame)fromSaveSlot.SelectedItem;
                        showAdditionalInfoMessage(MESSAGE_INFO, "Source savegame file loaded correctly.");
                    }
                    else
                    {
                        showAdditionalInfoMessage(MESSAGE_ERROR, "No active characters were found in the selected source file.");
                    }
                }
                catch (Exception ex)
                {
                    sourceFilePath.Text = "Failed to load";
                    _fileManager.SourceFile = Array.Empty<byte>();
                    showAdditionalInfoMessage(MESSAGE_ERROR, $"Source savegame file failed to load: {ex.Message}");
                }
            }

            CheckButtonState();
        }

        private void targetButtonBrowse(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = GetDefaultEldenRingSaveDirectory();
                openFileDialog.Filter = "Elden Ring Save File |ER0000.sl2|Elden Ring Coop Save File |ER0000.co2";

                if (openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                targetSaveGames.Clear();
                selectedTargetSave = null;
                _fileManager.TargetFile = Array.Empty<byte>();
                _fileManager.TargetPath = openFileDialog.FileName;

                try
                {
                    _fileManager.TargetFile = File.ReadAllBytes(_fileManager.TargetPath);
                    targetFilePath.Text = _fileManager.TargetPath;

                    for (int i = 0; i < 10; i++)
                    {
                        var newSave = new SaveGame();
                        if (!newSave.LoadData(_fileManager.TargetFile, i))
                        {
                            continue;
                        }

                        if (!newSave.Active)
                        {
                            newSave.CharacterName = $"Slot {i + 1}";
                        }

                        targetSaveGames.Add(newSave);
                    }

                    if (targetSaveGames.Count > 0)
                    {
                        toSaveSlot.SelectedIndex = 0;
                        selectedTargetSave = (SaveGame)toSaveSlot.SelectedItem;
                        showAdditionalInfoMessage(MESSAGE_INFO, "Destination savegame file loaded correctly.");
                    }
                    else
                    {
                        showAdditionalInfoMessage(MESSAGE_ERROR, "No valid destination slots were found in the selected file.");
                    }
                }
                catch (Exception ex)
                {
                    targetFilePath.Text = "Failed to load";
                    _fileManager.TargetFile = Array.Empty<byte>();
                    showAdditionalInfoMessage(MESSAGE_ERROR, $"Destination savegame file failed to load: {ex.Message}");
                }
            }

            CheckButtonState();
        }

        private void CheckButtonState()
        {
            bool validSelection = _fileManager.HasValidSource && _fileManager.HasValidTarget
                && !string.IsNullOrWhiteSpace(_fileManager.SourcePath)
                && !string.IsNullOrWhiteSpace(_fileManager.TargetPath)
                && !string.Equals(
                    System.IO.Path.GetFullPath(_fileManager.SourcePath).TrimEnd(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar),
                    System.IO.Path.GetFullPath(_fileManager.TargetPath).TrimEnd(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar),
                    StringComparison.InvariantCultureIgnoreCase)
                && IsValidSourceSelection(selectedSourceSave)
                && IsValidTargetSelection(selectedTargetSave);

            if (validSelection)
            {
                copyButton.Enabled = true;
                copyButton.BackColor = Color.Goldenrod;
                copyButton.Text =
                    "Copy source character "
                    + GetDisplayName(selectedSourceSave, "source")
                    + " over destination character "
                    + GetDisplayName(selectedTargetSave, "destination");
            }
            else
            {
                copyButton.Enabled = false;
                copyButton.BackColor = Color.DarkOrange;
                copyButton.Text = "Select Source and Destination file and characters";
            }
        }

        private string GetDisplayName(SaveGame save, string defaultPrefix)
        {
            if (save == null)
            {
                return defaultPrefix;
            }

            if (string.IsNullOrWhiteSpace(save.CharacterName) || save.CharacterName.Contains("Slot "))
            {
                return save.CharacterName;
            }

            return save.CharacterName.Split('\0')[0];
        }

        private bool IsValidSourceSelection(SaveGame save)
        {
            return save != null
                && save.Id != Guid.Empty
                && save.Index >= 0
                && save.Index < 10
                && save.Active
                && save.SaveData.Length == SaveGame.SLOT_LENGTH
                && save.HeaderData.Length == SaveGame.SAVE_HEADER_LENGTH;
        }

        private bool IsValidTargetSelection(SaveGame save)
        {
            return save != null
                && save.Id != Guid.Empty
                && save.Index >= 0
                && save.Index < 10
                && save.SaveData.Length == SaveGame.SLOT_LENGTH
                && save.HeaderData.Length == SaveGame.SAVE_HEADER_LENGTH;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            titleBar.MouseDown += Form1_MouseDown;
            _fileManager = new FileManager();

            fromSaveSlot.DisplayMember = "CharacterName";
            fromSaveSlot.DataSource = new BindingSource() { DataSource = this.sourceSaveGames }.DataSource;
            toSaveSlot.DisplayMember = "CharacterName";
            toSaveSlot.DataSource = new BindingSource() { DataSource = this.targetSaveGames }.DataSource;
        }

        private void fromSaveSlot_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = (ComboBox)sender;
            selectedSourceSave = comboBox.SelectedItem as SaveGame;
            CheckButtonState();
        }

        private void toSaveSlot_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = (ComboBox)sender;
            selectedTargetSave = comboBox.SelectedItem as SaveGame;
            CheckButtonState();
        }

        private string CreateFileBackup(string path, byte[] file)
        {
            string directory = System.IO.Path.GetDirectoryName(path) ?? string.Empty;
            string fileName = System.IO.Path.GetFileNameWithoutExtension(path);
            string backupPath = System.IO.Path.Combine(directory, $"{fileName}.backup1");
            int backupCount = 2;

            while (System.IO.File.Exists(backupPath))
            {
                backupPath = System.IO.Path.Combine(directory, $"{fileName}.backup{backupCount}");
                backupCount++;
            }

            System.IO.File.WriteAllBytes(backupPath, file);
            return backupPath;
        }

        private void copyButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedSourceSave == null || selectedTargetSave == null)
                {
                    throw new InvalidOperationException("The selected source or destination slot is not available.");
                }

                string backupPath = CreateFileBackup(_fileManager.TargetPath, _fileManager.TargetFile);

                byte[] updatedTargetFile = SaveFileService.CreateUpdatedSaveFile(
                    selectedSourceSave,
                    selectedTargetSave,
                    _fileManager.TargetFile,
                    _fileManager.SourceID,
                    _fileManager.TargetID);

                File.WriteAllBytes(_fileManager.TargetPath, updatedTargetFile);
                DeleteLegacyBackupFile(_fileManager.TargetPath);

                _fileManager.TargetFile = updatedTargetFile;

                var updatedTargetSave = new SaveGame();
                if (!updatedTargetSave.LoadData(updatedTargetFile, selectedTargetSave.Index))
                {
                    throw new InvalidOperationException("The updated destination slot could not be reloaded.");
                }

                int targetListIndex = GetTargetSaveListIndex(selectedTargetSave.Index);
                if (targetListIndex < 0)
                {
                    throw new InvalidOperationException("The destination slot could not be found in the current list.");
                }

                targetSaveGames[targetListIndex] = updatedTargetSave;
                toSaveSlot.SelectedItem = updatedTargetSave;

                copyButton.Enabled = false;
                copyButton.Text = "Copy Successful!";
                copyButton.BackColor = Color.Gold;
                showAdditionalInfoMessage(MESSAGE_SUCCESS, $"Copy completed successfully. Backup saved as {System.IO.Path.GetFileName(backupPath)}. Remove any ER0000.bak file before launching the game.");
            }
            catch (Exception ex)
            {
                copyButton.Enabled = false;
                copyButton.Text = "Copy Failed!";
                copyButton.BackColor = Color.DarkOrange;

                try
                {
                    string errorPath = GetErrorLogPath(_fileManager.TargetPath);
                    File.WriteAllBytes(errorPath, Encoding.Default.GetBytes(ex.ToString()));
                }
                catch
                {
                    // Ignore secondary logging errors.
                }

                showAdditionalInfoMessage(MESSAGE_ERROR, $"Copy failed: {ex.Message}");
            }
        }

        private string GetErrorLogPath(string savePath)
        {
            string directory = System.IO.Path.GetDirectoryName(savePath);
            if (string.IsNullOrWhiteSpace(directory))
            {
                directory = AppDomain.CurrentDomain.BaseDirectory;
            }

            return System.IO.Path.Combine(directory, "Error.log");
        }

        private int GetTargetSaveListIndex(int slotIndex)
        {
            for (int i = 0; i < targetSaveGames.Count; i++)
            {
                if (targetSaveGames[i].Index == slotIndex)
                {
                    return i;
                }
            }

            return -1;
        }

        private void DeleteLegacyBackupFile(string path)
        {
            try
            {
                string directory = System.IO.Path.GetDirectoryName(path) ?? string.Empty;
                string fileName = System.IO.Path.GetFileNameWithoutExtension(path);

                string bakPath = System.IO.Path.Combine(directory, $"{fileName}.bak");
                if (File.Exists(bakPath))
                {
                    File.Delete(bakPath);
                }

                for (int backupIndex = 1; backupIndex <= 10; backupIndex++)
                {
                    string legacyBackupPath = System.IO.Path.Combine(directory, $"{fileName}.backup{backupIndex}");
                    if (File.Exists(legacyBackupPath))
                    {
                        File.Delete(legacyBackupPath);
                    }
                }
            }
            catch
            {
                // Ignore cleanup failures.
            }
        }

        private void showAdditionalInfoMessage(int type, string message)
        {
            additionalInfoLabel.Text = message;
            switch (type)
            {
                case MESSAGE_ERROR:
                    additionalInfoLabel.ForeColor = Color.DarkOrange;
                    break;
                case MESSAGE_INFO:
                    additionalInfoLabel.ForeColor = Color.White;
                    break;
                case MESSAGE_SUCCESS:
                    additionalInfoLabel.ForeColor = Color.Gold;
                    break;
                default:
                    additionalInfoLabel.ForeColor= Color.White;
                    break;

            }
        }

        private void exitButtonClick(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
