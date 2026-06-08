using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using EldenRingSaveCopy.Saves.Model;

namespace EldenRingSaveCopy
{
    public partial class MainForm : Form
    {
        // Selected slot indices (-1 = none)
        private int _fromSlot = -1;
        private int _toSlot = -1;

        // Slots read from each file (index aligned to the save's 0..9 character slots)
        private List<CharacterSlot> _sourceSlots = new List<CharacterSlot>();
        private List<CharacterSlot> _destSlots = new List<CharacterSlot>();

        public MainForm()
        {
            InitializeComponent();
            UpdateState();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            DarkMica.Apply(this); // Windows 11 Mica backdrop + dark title bar
        }

        // ===================================================================
        //  STEP 1 — file pickers
        // ===================================================================
        private void btnBrowseSource_Click(object sender, EventArgs e)
        {
            string path = PickSaveFile();
            if (path == null) return;
            try { _sourceSlots = ReadCharacterSlots(path); }
            catch (Exception ex) { SetBanner("Could not read the source file: " + ex.Message, Banner.Kind.Critical); return; }
            srcField.Value = path;
            FillCombo(cmbFrom, _sourceSlots, sourceOnly: true);
            _fromSlot = -1;
            fromSummary.Visible = false;
            UpdateState();
        }

        private void btnBrowseDest_Click(object sender, EventArgs e)
        {
            string path = PickSaveFile();
            if (path == null) return;
            try { _destSlots = ReadCharacterSlots(path); }
            catch (Exception ex) { SetBanner("Could not read the destination file: " + ex.Message, Banner.Kind.Critical); return; }
            dstField.Value = path;
            FillCombo(cmbTo, _destSlots, sourceOnly: false);
            _toSlot = -1;
            toSummary.Visible = false;
            UpdateState();
        }

        private string PickSaveFile()
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Title = "Select an Elden Ring save file";
                dlg.Filter = "Elden Ring save (*.sl2;*.co2;*.bak)|*.sl2;*.co2;*.bak|All files (*.*)|*.*";
                string roaming = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EldenRing");
                if (Directory.Exists(roaming)) dlg.InitialDirectory = roaming;
                return dlg.ShowDialog(this) == DialogResult.OK ? dlg.FileName : null;
            }
        }

        // ===================================================================
        //  STEP 2 — slot combos
        // ===================================================================
        private void FillCombo(FluentComboBox cmb, List<CharacterSlot> slots, bool sourceOnly)
        {
            cmb.BeginUpdate();
            cmb.Items.Clear();
            cmb.Tag = new List<int>(); // maps display index -> real slot index
            var map = (List<int>)cmb.Tag;

            for (int i = 0; i < slots.Count; i++)
            {
                var s = slots[i];
                if (sourceOnly && s.IsEmpty) continue; // can't copy FROM an empty slot
                string text = s.IsEmpty ? $"Slot {i + 1} · Empty" : SlotLabel(i, s);
                cmb.Items.Add(text);
                map.Add(i);
            }
            cmb.SelectedIndex = -1;
            cmb.Enabled = cmb.Items.Count > 0;
            cmb.PlaceholderText = cmb.Items.Count > 0 ? "Choose a slot…" : "Select a file first";
            cmb.EndUpdate();
            cmb.Invalidate();
        }

        private void Slot_Changed(object sender, EventArgs e)
        {
            _fromSlot = RealIndex(cmbFrom);
            _toSlot = RealIndex(cmbTo);
            RefreshSummaries();
            UpdateState();
        }

        private int RealIndex(ComboBox cmb)
        {
            if (cmb.SelectedIndex < 0 || !(cmb.Tag is List<int> map)) return -1;
            return map[cmb.SelectedIndex];
        }

        private void RefreshSummaries()
        {
            // FROM summary (source character)
            if (_fromSlot >= 0 && _fromSlot < _sourceSlots.Count && !_sourceSlots[_fromSlot].IsEmpty)
            {
                var s = _sourceSlots[_fromSlot];
                fromSummary.SetContent(SlotSummary.Variant.Neutral, Initial(s.Name), s.Name, SlotMeta(s), "");
            }
            else fromSummary.Visible = false;

            // TO summary (overwrite / conflict / safe)
            if (_toSlot >= 0 && _toSlot < _destSlots.Count)
            {
                var s = _destSlots[_toSlot];
                bool sameFile = string.Equals(srcField.Value, dstField.Value, StringComparison.OrdinalIgnoreCase);
                if (s.IsEmpty)
                    toSummary.SetContent(SlotSummary.Variant.Safe, "+", "Empty slot", "Safe to copy into", "Safe");
                else if (sameFile && _toSlot == _fromSlot)
                    toSummary.SetContent(SlotSummary.Variant.Warn, Initial(s.Name), s.Name, "Same as source slot", "Conflict");
                else
                    toSummary.SetContent(SlotSummary.Variant.Warn, Initial(s.Name), s.Name, "Will be overwritten — backup kept", "Overwrite");
            }
            else toSummary.Visible = false;
        }

        // ===================================================================
        //  Enable / disable + banner + step badges
        // ===================================================================
        private bool FilesReady => srcField.HasValue && dstField.HasValue;
        private bool SlotsReady => _fromSlot >= 0 && _toSlot >= 0;
        private bool SameSlot => string.Equals(srcField.Value, dstField.Value, StringComparison.OrdinalIgnoreCase)
                                && _fromSlot >= 0 && _fromSlot == _toSlot;
        private bool Ready => FilesReady && SlotsReady && !SameSlot;

        private void UpdateState()
        {
            badge1.State = FilesReady ? StepBadge.BadgeState.Done : StepBadge.BadgeState.Active;
            badge2.State = !FilesReady ? StepBadge.BadgeState.Pending
                          : (SlotsReady ? StepBadge.BadgeState.Done : StepBadge.BadgeState.Active);
            badge3.State = Ready ? StepBadge.BadgeState.Active : StepBadge.BadgeState.Pending;

            btnCopy.Enabled = Ready;

            if (SameSlot)
                SetBanner("Source and destination are the same slot. Choose a different destination slot to continue.", Banner.Kind.Critical);
            else if (!FilesReady)
                SetBanner("Select your source and destination save files to begin.", Banner.Kind.Info);
            else if (!SlotsReady)
                SetBanner("Choose a character slot to copy from and a slot to copy to.", Banner.Kind.Info);
            else
                SetBanner($"Ready to copy {_sourceSlots[_fromSlot].Name} into Slot {_toSlot + 1}. Review and press Copy.", Banner.Kind.Info);
        }

        private void SetBanner(string text, Banner.Kind kind)
        {
            banner.Set(kind, text);
        }

        // ===================================================================
        //  STEP 3 — copy (with automatic backup) on a background worker
        // ===================================================================
        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (SameSlot) { UpdateState(); return; }
            if (!Ready) return;

            SetControlsEnabled(false);
            btnCopy.Text = "Copying…";
            banner.Progress.Value = 0;
            banner.Progress.Visible = true;
            SetBanner("Copying character… please keep the app open.", Banner.Kind.Neutral);

            string sourcePath = srcField.Value;
            string destPath = dstField.Value;
            int fromSlot = _fromSlot, toSlot = _toSlot;

            var worker = new BackgroundWorker { WorkerReportsProgress = true };
            worker.DoWork += (s, ev) =>
            {
                // 1) Back up the destination before touching it.
                string backup = CreateBackup(destPath);
                worker.ReportProgress(40);

                // 2) === Hand off to the core save-copy logic. ===
                CopyCharacterSlot(sourcePath, fromSlot, destPath, toSlot);
                worker.ReportProgress(100);

                ev.Result = backup;
            };
            worker.ProgressChanged += (s, ev) => banner.Progress.Value = ev.ProgressPercentage;
            worker.RunWorkerCompleted += (s, ev) =>
            {
                banner.Progress.Visible = false;
                SetControlsEnabled(true);
                if (ev.Error != null)
                {
                    btnCopy.Text = "Copy character";
                    SetBanner("Copy failed: " + ev.Error.Message, Banner.Kind.Critical);
                }
                else
                {
                    // Reflect the freshly written destination so a follow-up copy sees real data.
                    try
                    {
                        _destSlots = ReadCharacterSlots(destPath);
                        FillCombo(cmbTo, _destSlots, sourceOnly: false);
                        _toSlot = -1;
                        toSummary.Visible = false;
                    }
                    catch { /* the copy already succeeded; a refresh failure is non-fatal. */ }

                    string backupName = Path.GetFileName((string)ev.Result);
                    btnCopy.Text = "Done — Copy again";
                    SetBanner($"Copy complete. Backup saved as {backupName}. Delete any ER0000.bak file before launching the game.", Banner.Kind.Success);
                    badge3.State = StepBadge.BadgeState.Done;
                }
            };
            worker.RunWorkerAsync();
        }

        private void SetControlsEnabled(bool on)
        {
            btnBrowseSource.Enabled = btnBrowseDest.Enabled = on;
            cmbFrom.Enabled = cmbFrom.Items.Count > 0 && on;
            cmbTo.Enabled = cmbTo.Items.Count > 0 && on;
            btnCopy.Enabled = on && Ready;
        }

        /// <summary>Copies the destination file to ER0000.backup1 (incrementing if it already exists).</summary>
        private string CreateBackup(string destPath)
        {
            string dir = Path.GetDirectoryName(destPath);
            string baseName = Path.GetFileNameWithoutExtension(destPath); // "ER0000"
            int n = 1;
            string target;
            do { target = Path.Combine(dir, $"{baseName}.backup{n}"); n++; }
            while (File.Exists(target));
            File.Copy(destPath, target, overwrite: false);
            return target;
        }

        // ---- display helpers (the parser exposes name/level/playtime; class is not decoded) ----
        private static string Initial(string name) =>
            string.IsNullOrWhiteSpace(name) ? "?" : name.Substring(0, 1).ToUpperInvariant();

        private static string SlotLabel(int slotIndex, CharacterSlot s)
        {
            string cls = string.IsNullOrEmpty(s.ClassName) ? "" : $" — {s.ClassName}";
            string lvl = s.Level > 0 ? $" · RL{s.Level}" : "";
            return $"Slot {slotIndex + 1} · {s.Name}{cls}{lvl}";
        }

        private static string SlotMeta(CharacterSlot s)
        {
            var parts = new List<string>();
            if (!string.IsNullOrEmpty(s.ClassName)) parts.Add(s.ClassName);
            if (s.Level > 0) parts.Add($"Rune Level {s.Level}");
            if (!string.IsNullOrEmpty(s.PlayTime) && s.PlayTime != "—") parts.Add(s.PlayTime);
            return string.Join(" · ", parts);
        }

        // ===================================================================
        //  CORE LOGIC HOOKS — wired to the existing byte-level save logic.
        // ===================================================================

        /// <summary>
        /// Reads the 10 character slots from an ER0000.sl2 file using the existing
        /// <see cref="SaveGame"/> parser. Inactive/unused slots are returned with IsEmpty = true.
        /// </summary>
        private List<CharacterSlot> ReadCharacterSlots(string savePath)
        {
            byte[] data = File.ReadAllBytes(savePath);
            var slots = new List<CharacterSlot>();

            for (int i = 0; i < 10; i++)
            {
                var save = new SaveGame();
                if (save.LoadData(data, i) && save.Active)
                {
                    slots.Add(new CharacterSlot
                    {
                        IsEmpty = false,
                        Name = SafeName(save.CharacterName, i),
                        ClassName = "", // the byte parser does not decode the starting class
                        Level = save.CharacterLevel,
                        PlayTime = FormatPlayTime(save.SecondsPlayed),
                    });
                }
                else
                {
                    slots.Add(new CharacterSlot { IsEmpty = true });
                }
            }

            return slots;
        }

        /// <summary>
        /// Copies a single character slot from one save to another using the existing
        /// <see cref="SaveFileService"/> byte-copy routine (ID rewrite + MD5 re-hash).
        /// The UI guarantees valid indices and a fresh backup before this runs.
        /// </summary>
        private void CopyCharacterSlot(string sourcePath, int fromSlot, string destPath, int toSlot)
        {
            byte[] sourceFile = File.ReadAllBytes(sourcePath);
            byte[] targetFile = File.ReadAllBytes(destPath);

            var fileManager = new FileManager
            {
                SourcePath = sourcePath,
                TargetPath = destPath,
                SourceFile = sourceFile, // setters extract the steam/profile IDs
                TargetFile = targetFile,
            };

            if (!fileManager.HasValidSource)
                throw new InvalidOperationException("The source file is not a valid Elden Ring save.");
            if (!fileManager.HasValidTarget)
                throw new InvalidOperationException("The destination file is not a valid Elden Ring save.");

            var sourceSave = new SaveGame();
            if (!sourceSave.LoadData(sourceFile, fromSlot) || !sourceSave.Active)
                throw new InvalidOperationException("The selected source character could not be read.");

            var targetSave = new SaveGame();
            if (!targetSave.LoadData(targetFile, toSlot))
                throw new InvalidOperationException("The destination slot could not be read.");

            byte[] updated = SaveFileService.CreateUpdatedSaveFile(
                sourceSave, targetSave, targetFile, fileManager.SourceID, fileManager.TargetID);

            File.WriteAllBytes(destPath, updated);
        }

        private static string SafeName(string name, int slotIndex)
        {
            if (string.IsNullOrWhiteSpace(name)) return $"Slot {slotIndex + 1}";
            return name.Split('\0')[0];
        }

        private static string FormatPlayTime(int seconds)
        {
            if (seconds <= 0) return "—";
            var t = TimeSpan.FromSeconds(seconds);
            return $"{(int)t.TotalHours}:{t.Minutes:00}";
        }
    }

    /// <summary>Lightweight view-model for one character save slot.</summary>
    public class CharacterSlot
    {
        public bool IsEmpty { get; set; }
        public string Name { get; set; } = "";
        public string ClassName { get; set; } = "";
        public int Level { get; set; }
        public string PlayTime { get; set; } = "";
    }
}
