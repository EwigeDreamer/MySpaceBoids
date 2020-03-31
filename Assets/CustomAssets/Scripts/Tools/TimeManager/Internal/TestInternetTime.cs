using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyTools.Helpers;
using MyTools.Internal;

public class TestInternetTime : MonoValidate
{
    [SerializeField] TMPro.TMP_Text m_Text;

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateGetComponentInChildren(ref m_Text);
    }

    private void Start()
    {
        if (m_Text == null) return;
        Test();
    }

    async void Test()
    {
        m_Text.text = "getting internet time...";
        var time = await InternetTime.GetUtcNowAsync();
        if (this == null || m_Text == null) return;
        m_Text.text = time.ToString();
    }
}
