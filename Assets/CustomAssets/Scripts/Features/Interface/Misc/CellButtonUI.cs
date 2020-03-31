using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MyTools.Helpers;

public class CellButtonUI : ImprovedBehaviour
{
#pragma warning disable 649
    [SerializeField] Image m_Image;
    [SerializeField] Button m_Button;
    [SerializeField] TMP_Text m_Text;
#pragma warning restore 649

    public Image Image => m_Image;
    public Button Button => m_Button;
    public TMP_Text Label => m_Text;
}
