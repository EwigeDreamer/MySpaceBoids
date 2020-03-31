using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Prefs;

public class TestPlayerPrefs : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
            Write();
        if (Input.GetKeyDown(KeyCode.Keypad2))
            Load();
        if (Input.GetKeyDown(KeyCode.Space))
            SavePrefs();
        if (Input.GetKeyDown(KeyCode.Keypad0))
            TypeTest();
    }


    void Write()
    {
        int dataInt = 5;
        MyPlayerPrefs.SetInt("SimpleInt", dataInt);
        float dataFloat = 0.3f;
        MyPlayerPrefs.SetFloat("SimpleFloat", dataFloat);
        string dataString = "Saved data atata";
        MyPlayerPrefs.SetString("SimpleString", dataString);
        Vector3 dataVector = new Vector3(0.3f, 1.3f, 2.3f);
        MyPlayerPrefs.SetVector3("SimpleVector", dataVector);
        Quaternion dataQuat = new Quaternion(1.3f, 2.3f, 3.3f, 4.3f);
        MyPlayerPrefs.SetQuaternion("SimpleQuat", dataQuat);
        Color dataColor = new Color(0.3f, 0.6f, 0.9f, 1f);
        MyPlayerPrefs.SetColor("SimpleColor", dataColor);
        Debug.LogWarning("WRITED");
    }

    void Load()
    {
        int dataInt = MyPlayerPrefs.GetInt("SimpleInt", 0);
        float dataFloat = MyPlayerPrefs.GetFloat("SimpleFloat", 0.8f);
        string dataString = MyPlayerPrefs.GetString("SimpleString", "wrong");
        Vector3 dataVector = MyPlayerPrefs.GetVector3("SimpleVector", Vector3.zero);
        Quaternion dataQuat = MyPlayerPrefs.GetQuaternion("SimpleQuat", new Quaternion(0f, 0f, 0f, 0f));
        Color dataColor = MyPlayerPrefs.GetColor("SimpleColor", new Color(0f, 0f, 0f, 0f));
        Debug.LogWarning(string.Format("{0}\n{1}\n{2}\n{3}\n{4}\n{5}", dataInt, dataFloat, dataString, dataVector, dataQuat, dataColor));
    }

    void SavePrefs()
    {
        MyPlayerPrefs.Save();
        Debug.LogWarning("SAVED?");
    }

    void TypeTest()
    {
        Debug.LogWarning(typeof(Vector3).FullName);
    }

    [ContextMenu("Test byte")]
    void TestByte()
    {
        byte bt = 130;
        Debug.LogWarning(bt);
        bt += 128;
        Debug.LogWarning(bt);
        bt -= 128;
        Debug.LogWarning(bt);
    }
}
