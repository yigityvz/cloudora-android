using System;
using System.IO;
using UnityEngine;

namespace Cloudora.Services
{
    public sealed class LocalJsonSaveService : ISaveService
    {
        private readonly string _path;
        private string BackupPath => _path + ".bak";
        private string TempPath => _path + ".tmp";

        public LocalJsonSaveService(string path = null)
        {
            _path = path ?? Path.Combine(Application.persistentDataPath, "cloudora-save.json");
        }

        public SaveData Load()
        {
            if (TryRead(_path, out SaveData data) || TryRead(BackupPath, out data))
                return Migrate(data);
            return SaveData.Defaults();
        }

        public void Save(SaveData data)
        {
            data.schemaVersion = SaveData.CurrentSchemaVersion;
            string directory = Path.GetDirectoryName(_path);
            if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);
            File.WriteAllText(TempPath, JsonUtility.ToJson(data, true));
            if (File.Exists(_path)) File.Copy(_path, BackupPath, true);
            File.Copy(TempPath, _path, true);
            File.Delete(TempPath);
        }

        private static bool TryRead(string path, out SaveData data)
        {
            data = null;
            if (!File.Exists(path)) return false;
            try
            {
                data = JsonUtility.FromJson<SaveData>(File.ReadAllText(path));
                return data != null && data.schemaVersion > 0 && data.schemaVersion <= SaveData.CurrentSchemaVersion;
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"Cloudora save recovery: {exception.Message}");
                return false;
            }
        }

        private static SaveData Migrate(SaveData data)
        {
            if (data.schemaVersion < 2)
            {
                if (data.tutorialFlags == null || data.tutorialFlags.Length != 15) data.tutorialFlags = new bool[15];
                if (data.undoCharges < 0) data.undoCharges = 0;
                if (data.extraCloudCharges < 0) data.extraCloudCharges = 0;
                if (data.safeShuffleCharges < 0) data.safeShuffleCharges = 0;
            }
            data.currentLevel = Math.Max(1, data.currentLevel);
            data.highestCompletedLevel = Math.Max(0, data.highestCompletedLevel);
            data.lives = Math.Max(0, Math.Min(Progression.LifeManager.MaxLives, data.lives));
            data.schemaVersion = SaveData.CurrentSchemaVersion;
            return data;
        }
    }
}
