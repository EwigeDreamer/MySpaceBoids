using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MyTools.Helpers;

public class CellToggleUI : ImprovedBehaviour
{
#pragma warning disable 649
    [SerializeField] Image m_BackGround;
    [SerializeField] Image m_CheckMark;
    [SerializeField] Toggle m_Toggle;
    [SerializeField] TMP_Text m_Text;
#pragma warning restore 649

    public Image BackGround => m_BackGround;
    public Image CheckMark => m_CheckMark;
    public Toggle Toggle => m_Toggle;
    public TMP_Text Label => m_Text;
}
