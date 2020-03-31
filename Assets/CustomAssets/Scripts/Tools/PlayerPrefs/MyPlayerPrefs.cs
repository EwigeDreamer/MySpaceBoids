using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Prefs.Helpers;
using MyTools.Helpers;
using MyTools.Singleton;
using System.Globalization;

namespace MyTools.Prefs
{
    public static class MyPlayerPrefs
    {
        public static event Action OnSave = delegate { };

        static CultureInfo culture = CultureInfo.InvariantCulture;
        static string format = "G32";

        const string fileName = @"player_prefs.bin";
        static MyPlayerPrefsDataManager dataManager;

        static MyPlayerPrefs()
        {
            dataManager = new MyPlayerPrefsDataManager(fileName);
            Application.focusChanged += Focus;
            Application.quitting += Quit;
        }

        static void Focus(bool focus) { if (!focus) Save(); }
        static void Quit() { Save(); }

        public static void SetFloat(string key, float value)
        {
            var entry = value.ToString(format, culture);
            dataManager.SetEntry(key, entry);
        }
        public static float GetFloat(string key, float defaultValue)
        {
            if (dataManager.TryGetEntry(key, out var entry)
                && float.TryParse(entry, NumberStyles.Any, culture, out var entryValue))
                return entryValue;
            return defaultValue;
        }

        public static void SetInt(string key, int value)
        {
            var entry = value.ToString();
            dataManager.SetEntry(key, entry);
        }
        public static int GetInt(string key, int defaultValue)
        {
            if (dataManager.TryGetEntry(key, out var entry)
                && int.TryParse(entry, out var entryValue))
                return entryValue;
            return defaultValue;
        }

        public static void SetDouble(string key, double value)
        {
            var entry = value.ToString(format, culture);
            dataManager.SetEntry(key, entry);
        }
        public static double GetDouble(string key, double defaultValue)
        {
            if (dataManager.TryGetEntry(key, out var entry)
                && double.TryParse(entry, NumberStyles.Any, culture, out var entryValue))
                return entryValue;
            return defaultValue;
        }

        public static void SetLong(string key, long value)
        {
            var entry = value.ToString();
            dataManager.SetEntry(key, entry);
        }
        public static long GetLong(string key, long defaultValue)
        {
            if (dataManager.TryGetEntry(key, out var entry)
                && long.TryParse(entry, out var entryValue))
                return entryValue;
            return defaultValue;
        }

        public static void SetString(string key, string value)
        {
            dataManager.SetEntry(key, value);
        }
        public static string GetString(string key, string defaultValue)
        {
            if (dataManager.TryGetEntry(key, out var entry))
                return entry;
            return defaultValue;
        }
        
        public static void SetVector2(string key, Vector2 value)
        {
            var entry = JsonUtility.ToJson(value);
            dataManager.SetEntry(key, entry);
        }
        public static Vector2 GetVector2(string key, Vector2 defaultValue)
        {
            if (dataManager.TryGetEntry(key, out var entry))
                try { return JsonUtility.FromJson<Vector2>(entry); }
                catch { }
            return defaultValue;
        }

        public static void SetVector2Int(string key, Vector2Int value)
        {
            var entry = JsonUtility.ToJson(value);
            dataManager.SetEntry(key, entry);
        }
        public static Vector2Int GetVector2Int(string key, Vector2Int defaultValue)
        {
            if (dataManager.TryGetEntry(key, out var entry))
                try { return JsonUtility.FromJson<Vector2Int>(entry); }
                catch { }
            return defaultValue;
        }

        public static void SetVector3(string key, Vector3 value)
        {
            var entry = JsonUtility.ToJson(value);
            dataManager.SetEntry(key, entry);
        }
        public static Vector3 GetVector3(string key, Vector3 defaultValue)
        {
            if (dataManager.TryGetEntry(key, out var entry))
                try { return JsonUtility.FromJson<Vector3>(entry); }
                catch { }
            return defaultValue;
        }

        public static void SetVector3Int(string key, Vector3Int value)
        {
            var entry = JsonUtility.ToJson(value);
            dataManager.SetEntry(key, entry);
        }
        public static Vector3Int GetVector3Int(string key, Vector3Int defaultValue)
        {
            if (dataManager.TryGetEntry(key, out var entry))
                try { return JsonUtility.FromJson<Vector3Int>(entry); }
                catch { }
            return defaultValue;
        }

        public static void SetVector4(string key, Vector4 value)
        {
            var entry = JsonUtility.ToJson(value);
            dataManager.SetEntry(key, entry);
        }
        public static Vector4 GetVector4(string key, Vector4 defaultValue)
        {
            if (dataManager.TryGetEntry(key, out var entry))
                try { return JsonUtility.FromJson<Vector4>(entry); }
                catch { }
            return defaultValue;
        }

        public static void SetColor(string key, Color value)
        {
            var entry = JsonUtility.ToJson(value);
            dataManager.SetEntry(key, entry);
        }
        public static Color GetColor(string key, Color defaultValue)
        {
            if (dataManager.TryGetEntry(key, out var entry))
                try { return JsonUtility.FromJson<Color>(entry); }
                catch { }
            return defaultValue;
        }

        public static void SetColor32(string key, Color32 value)
        {
            var entry = JsonUtility.ToJson(value);
            dataManager.SetEntry(key, entry);
        }
        public static Color32 GetColor32(string key, Color32 defaultValue)
        {
            if (dataManager.TryGetEntry(key, out var entry))
                try { return JsonUtility.FromJson<Color32>(entry); }
                catch { }
            return defaultValue;
        }

        public static void SetQuaternion(string key, Quaternion value)
        {
            var entry = JsonUtility.ToJson(value);
            dataManager.SetEntry(key, entry);
        }
        public static Quaternion GetQuaternion(string key, Quaternion defaultValue)
        {
            if (dataManager.TryGetEntry(key, out var entry))
                try { return JsonUtility.FromJson<Quaternion>(entry); }
                catch { }
            return defaultValue;
        }

        public static void SetTimeSpan(string key, TimeSpan value)
        {
            var entry = value.ToString();
            dataManager.SetEntry(key, entry);
        }
        public static TimeSpan GetTimeSpan(string key, TimeSpan defaultValue)
        {
            if (dataManager.TryGetEntry(key, out var entry)
                && TimeSpan.TryParse(entry, out var entryValue))
                return entryValue;
            return defaultValue;
        }

        public static void SetDateTime(string key, DateTime value)
        {
            var entry = value.ToString();
            dataManager.SetEntry(key, entry);
        }
        public static DateTime GetDateTime(string key, DateTime defaultValue)
        {
            if (dataManager.TryGetEntry(key, out var entry)
                && DateTime.TryParse(entry, out var entryValue))
                return entryValue;
            return defaultValue;
        }

        public static void SetBool(string key, bool value)
        {
            var entry = value.ToString();
            dataManager.SetEntry(key, entry);
        }
        public static bool GetBool(string key, bool defaultValue)
        {
            if (dataManager.TryGetEntry(key, out var entry)
                && bool.TryParse(entry, out bool entryValue))
                return entryValue;
            return defaultValue;
        }

        public static void SetObject<TObj>(string key, TObj value)
        {
            if (value == null) return;
            var entry = JsonUtility.ToJson(value);
            dataManager.SetEntry(key, entry);
        }
        public static TObj GetObject<TObj>(string key, TObj defaultValue)
        {
            if (!dataManager.TryGetEntry(key, out var entry)) return defaultValue;
            var obj = JsonUtility.FromJson<TObj>(entry);
            return obj;
        }

        public static bool ContainsKey(string key)
        {
            return dataManager.ContainsKey(key);
        }
        public static void DeleteKey(string key)
        {
            dataManager.DeleteKey(key);
        }
        public static void DeleteAll()
        {
            dataManager.DeleteAll();
        }
        public static void Save()
        {
            OnSave();
            dataManager.Save();
        }

    }
}
