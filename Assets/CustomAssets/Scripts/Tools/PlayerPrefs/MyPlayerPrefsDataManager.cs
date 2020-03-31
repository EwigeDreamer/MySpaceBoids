using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.IO;

namespace MyTools.Prefs.Helpers
{
    public class MyPlayerPrefsDataManager//<T> : IDataManager<T>
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
#if UNITY_EDITOR
        string folder = Application.dataPath.Substring(0, Application.dataPath.Length - "/Assets".Length);
#else
        string folder = Application.persistentDataPath;
#endif
        string path;

        //MyDataStorageManager m_Storage = null;

        //Dictionary<string, T> m_DataDict = null;
        //bool IsInited { get { return m_DataDict != null; } }

        private MyPlayerPrefsDataManager() { }
        public MyPlayerPrefsDataManager(string fileName) : this()
        {
            this.path = Path.Combine(folder, fileName);
            Load();
            //m_Storage = new MyDataStorageManager(fileName, fileExt);
        }

        void Load()
        {
            if (!File.Exists(this.path)) return;
            try
            {
                var json = File.ReadAllText(this.path);
                var handler = JsonUtility.FromJson<DataHandler>(json);
                var data = this.data;
                foreach (var entry in handler.data)
                    data.Add(entry.key, entry.value);
            }
            catch (Exception e) 
            {
                Debug.LogError($"File read error! [{this.path}]:\n{e.ToString()}");
            }
        }

        public void Save()
        {
            var handler = new DataHandler();
            var list = handler.data;
            list.Clear();
            foreach (var pair in data)
                list.Add(new DataEntry { key = pair.Key, value = pair.Value });
            var json = JsonUtility.ToJson(handler, Application.isEditor);
            try
            {
                File.WriteAllText(this.path, json);
            }
            catch (Exception e)
            {
                Debug.LogError($"File write error! [{this.path}]:\n{e.ToString()}");
            }
        }

        [System.Serializable]
        class DataHandler
        {
            public List<DataEntry> data = new List<DataEntry>();
        }
        [System.Serializable]
        struct DataEntry
        {
            public string key;
            public string value;
        }

        public bool TryGetEntry(string key, out string entry)
        {
            return this.data.TryGetValue(key, out entry);
        }

        public void SetEntry(string key, string value)
        {
            this.data[key] = value;
        }

        public bool ContainsKey(string key)
        {
            return this.data.ContainsKey(key);
        }

        public bool DeleteKey(string key)
        {
            return this.data.Remove(key);
        }

        public void DeleteAll()
        {
            this.data.Clear();
        }

        //bool IDataManager<T>.IsInited { get { return m_DataDict != null && m_Storage != null; } }

        //bool IDataManager<T>.TryGetEntry(string key, out T entry)
        //{
        //    #region IF_IS_NOT_INITED
        //    if (!IsInited)
        //    {
        //        MyLogger.ObjectErrorFormat<MyPlayerPrefsDataManager<T>>("{0} is not inited!", typeof(MyPlayerPrefsDataManager<T>));
        //        entry = default;
        //        return false;
        //    }
        //    #endregion
        //    return m_DataDict.TryGetValue(key, out entry);
        //}

        //void IDataManager<T>.SetEntry(string key, T entry)
        //{
        //    #region IF_IS_NOT_INITED
        //    if (!IsInited)
        //    {
        //        MyLogger.ObjectErrorFormat<MyPlayerPrefsDataManager<T>>("{0} is not inited!", typeof(MyPlayerPrefsDataManager<T>));
        //        return;
        //    }
        //    #endregion
        //    if (m_DataDict.ContainsKey(key))
        //        m_DataDict[key] = entry;
        //    else
        //        m_DataDict.Add(key, entry);
        //}

        //bool IDataManager<T>.ContainsKey(string key)
        //{
        //    #region IF_IS_NOT_INITED
        //    if (!IsInited)
        //    {
        //        MyLogger.ObjectErrorFormat<MyPlayerPrefsDataManager<T>>("{0} is not inited!", typeof(MyPlayerPrefsDataManager<T>));
        //        return false;
        //    }
        //    #endregion
        //    return m_DataDict.ContainsKey(key);
        //}

        //bool IDataManager<T>.DeleteKey(string key)
        //{
        //    #region IF_IS_NOT_INITED
        //    if (!IsInited)
        //    {
        //        MyLogger.ObjectErrorFormat<MyPlayerPrefsDataManager<T>>("{0} is not inited!", typeof(MyPlayerPrefsDataManager<T>));
        //        return false;
        //    }
        //    #endregion
        //    return m_DataDict.Remove(key);
        //}

        //void IDataManager<T>.DeleteAll()
        //{
        //    #region IF_IS_NOT_INITED
        //    if (!IsInited)
        //    {
        //        MyLogger.ObjectErrorFormat<MyPlayerPrefsDataManager<T>>("{0} is not inited!", typeof(MyPlayerPrefsDataManager<T>));
        //        return;
        //    }
        //    #endregion
        //    m_DataDict.Clear();
        //}

        //void IDataManager<T>.Save()
        //{
        //    #region IF_IS_NOT_INITED
        //    if (!IsInited)
        //    {
        //        MyLogger.ObjectErrorFormat<MyPlayerPrefsDataManager<T>>("{0} is not inited!", typeof(MyPlayerPrefsDataManager<T>));
        //        return;
        //    }
        //    #endregion
        //    var keys = new string[m_DataDict.Count];
        //    m_DataDict.Keys.CopyTo(keys, 0);
        //    var entries = new T[m_DataDict.Count];
        //    m_DataDict.Values.CopyTo(entries, 0);
        //    var data = new object[] { keys, entries };
        //    m_Storage.SaveData(data);
        //}

        //void Load()
        //{
        //    object loadedData = m_Storage.LoadData();
        //    if (loadedData != null
        //        && loadedData is object[] data
        //        && data.Length == 2
        //        && data[0] != null
        //        && data[0] is string[] keys
        //        && data[1] != null
        //        && data[1] is T[] entries
        //        && keys.Length == entries.Length)
        //    {
        //        int count = keys.Length;
        //        m_DataDict = new Dictionary<string, T>(count);
        //        for (int i = 0; i < count; ++i)
        //            m_DataDict.Add(keys[i], entries[i]);
        //        return;
        //    }
        //    m_DataDict = new Dictionary<string, T>();
        //}
    }
}
