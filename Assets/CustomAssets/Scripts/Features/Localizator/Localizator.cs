using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections.ObjectModel;
using MyTools.Extensions.Common;
using MyTools.Extensions.String;
using MyTools.Prefs;

namespace MyTools.Localizator
{
    using LocalizationMap = Dictionary<string, Dictionary<LangCode, string>>;
    using LW = LumenWorks.Framework.IO.Csv;

    public enum LangCode
    {
        en,
        ru,
        //uk,
        //de,
        //it,
        //pt,
        //pl,
        //es,
        //fr,
        //zh,
        //ko,
        //ja
    }

    public static class LocData
    {
        static LangCode[] langs;
        static Dictionary<string, LangCode> parsedDict;
        static Dictionary<SystemLanguage, LangCode> langDict;
        static LocData()
        {
            langs = (LangCode[])System.Enum.GetValues(typeof(LangCode));
            Langs = new ReadOnlyCollection<LangCode>(langs);
            parsedDict = new Dictionary<string, LangCode>(langs.Length);
            foreach (var lang in langs) parsedDict.Add(lang.ToString().ToLower(), lang);
            langDict = new Dictionary<SystemLanguage, LangCode>
            {
                { SystemLanguage.English, LangCode.en },
                { SystemLanguage.Russian, LangCode.ru },
                //{ SystemLanguage.Ukrainian, LangCode.uk },
                //{ SystemLanguage.German, LangCode.de },
                //{ SystemLanguage.Italian, LangCode.it },
                //{ SystemLanguage.Portuguese, LangCode.pt },
                //{ SystemLanguage.Polish, LangCode.pl },
                //{ SystemLanguage.Spanish, LangCode.es },
                //{ SystemLanguage.French, LangCode.fr },
                //{ SystemLanguage.Chinese, LangCode.zh },
                //{ SystemLanguage.Korean, LangCode.ko },
                //{ SystemLanguage.Japanese, LangCode.ja },
            };
        }
        public static bool TryGetCode(SystemLanguage lang, out LangCode code) => langDict.TryGetValue(lang, out code);
        public static LangCode GetCode(SystemLanguage lang) => TryGetCode(lang, out var code) ? code : LangCode.en;
        public static bool TryParseCode(string str, out LangCode code) => parsedDict.TryGetValue(str, out code);
        public static LangCode ParseCode(string str) => TryParseCode(str, out var code) ? code : LangCode.en;
        public static ReadOnlyCollection<LangCode> Langs { get; }
    }

    public static class Localizator
    {
        public static event System.Action OnLangChanged = delegate { };

        const string path = "LocalizationTables";
        const string key = "app_localization";
        const string missing = "[NOT FOUND]";
        const char delimeter = '\t';

        static ReadOnlyCollection<LangCode> langs;
        static LangCode lang = LangCode.en;
        public static LangCode Lang => lang;

        static LocalizationMap locMap = new LocalizationMap();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            langs = LocData.Langs;
            LoadLocalizedText();
            LoadLang();
        }
        static void LoadLocalizedText()
        {
            var strs = Resources.LoadAll<TextAsset>(path);
            if (strs == null || strs.Length < 1) return;
            var map = locMap;
            foreach (var str in strs) ReadTable(str.text, map);
        }


        static void ReadTable(string tableStr, LocalizationMap map)
        {
            using (var csv = new LW.CsvReader(new StringReader(tableStr), true, delimeter))
            {
                int columnsCount = csv.FieldCount;
                var headers = csv.GetFieldHeaders();
                if (!headers.HasFieldNoBox("key", out int keyIndex)) return;

                Dictionary<int, LangCode> validKeys = new Dictionary<int, LangCode>(columnsCount);
                List<int> validKeysIds = new List<int>(columnsCount);
                for (int i = 0; i < columnsCount; ++i)
                {
                    if (i == keyIndex) continue;
                    var codeStr = headers[i].ToLower();
                    if (!LocData.TryParseCode(codeStr, out var code)) continue;
                    validKeys[i] = code;
                    validKeysIds.Add(i);
                }

                int keysCount = validKeysIds.Count;
                List<string> fields = new List<string>(columnsCount);
                while (csv.ReadNextRecord())
                {
                    fields.Clear();
                    for (int i = 0; i < columnsCount; ++i) fields.Add(csv[i]);
                    var trans = new Dictionary<LangCode, string>(keysCount);
                    for (int i = 0; i < keysCount; ++i)
                    {
                        int j = validKeysIds[i];
                        trans.Add(validKeys[j], fields[j]);
                    }
                    map.Add(fields[keyIndex], trans);
                }
            }
        }

        static void LoadLang()
        {
            LangCode lt;
            string l = MyPlayerPrefs.GetString(key, null);
            if (l == null)
            {
                var sl = Application.systemLanguage;
                if (!LocData.TryGetCode(sl, out lt)) lt = LangCode.en;
            }
            else
            {
                if (!LocData.TryParseCode(l, out lt)) lt = LangCode.en;
            }
            MyPlayerPrefs.SetString(key, lt.ToString());
            MyLogger.LogFormat("Localization: {0}", lt);
            lang = lt;
        }

        public static void ChangeLang(LangCode lang)
        {
            if (lang == Localizator.lang) return;
            MyPlayerPrefs.SetString(key, lang.ToString());
            Localizator.lang = lang;
            OnLangChanged();
            Debug.Log($"Change language: {lang}");
        }

        public static string GetLocalizedValue(string key)
        { return GetLocalizedValue(lang, key); }

        public static string GetLocalizedValue(LangCode lang, string key)
        {
            if (locMap.TryGetValue(key, out var entry))
                if (entry.TryGetValue(lang, out var str))
                    return str;
            return missing;
        }
    }
}
