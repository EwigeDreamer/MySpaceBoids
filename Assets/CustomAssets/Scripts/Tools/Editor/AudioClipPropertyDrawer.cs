using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

[CustomPropertyDrawer(typeof(AudioClip))]
public class AudioClipPropertyDrawer : PropertyDrawer
{
    private Dictionary<ButtonState, Action<SerializedProperty, AudioClip>> _audioButtonStates = new Dictionary<ButtonState, Action<SerializedProperty, AudioClip>>
    {
        { ButtonState.Play, Play },
        { ButtonState.Stop, Stop },
    };

    private enum ButtonState
    {
        Play,
        Stop
    }

    private static string CurrentClip;


    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, prop);

        if (prop.objectReferenceValue != null)
        {
            float totalWidth = position.width;
            position.width = totalWidth - (totalWidth / 4);
            EditorGUI.PropertyField(position, prop, true);

            position.x += position.width;
            position.width = totalWidth / 4;
            DrawButton(position, prop);
        }
        else
        {
            EditorGUI.PropertyField(position, prop, true);
        }

        EditorGUI.EndProperty();
    }

    static GUIStyle timeLabelStyle;
    static GUIStyle timeOutlineStyle;

    static AudioClipPropertyDrawer()
    {
        timeLabelStyle = new GUIStyle(GUI.skin.label);
        timeLabelStyle.alignment = TextAnchor.MiddleCenter;
        timeLabelStyle.normal.textColor = Color.white;
        //timeLabelStyle.fontStyle = FontStyle.Bold;
        timeOutlineStyle = new GUIStyle(timeLabelStyle);
        timeOutlineStyle.normal.textColor = Color.black;
    }

    private void DrawLabelOutline(Rect rect, string label)
    {
        var rectTmp1 = rect;
        var rectTmp2 = rect;
        rectTmp1.x -= 1f;
        GUI.Label(rectTmp1, label, timeOutlineStyle);
        rectTmp1.x += 2f;
        GUI.Label(rectTmp1, label, timeOutlineStyle);
        rectTmp2.y -= 1f;
        GUI.Label(rectTmp2, label, timeOutlineStyle);
        rectTmp2.y += 2f;
        GUI.Label(rectTmp2, label, timeOutlineStyle);
        GUI.Label(rect, label, timeLabelStyle);
    }

    private void DrawButton(Rect position, SerializedProperty prop)
    {
        var obj = prop.objectReferenceValue;
        if (obj == null) return;

        position.x += 4;
        position.width -= 5;

        AudioClip clip = obj as AudioClip;

        Rect buttonRect = new Rect(position);
        buttonRect.width = 20;

        Rect waveformRect = new Rect(position);
        waveformRect.x += 22;
        waveformRect.width -= 22;
        GUI.DrawTexture(waveformRect, AssetPreview.GetAssetPreview(obj));

        bool isPlaying = AudioUtility.IsClipPlaying(clip) && (CurrentClip == prop.propertyPath);
        string buttonText = "";
        Action<SerializedProperty, AudioClip> buttonAction;
        if (isPlaying)
        {
            EditorUtility.SetDirty(prop.serializedObject.targetObject);
            //EditorApplication.QueuePlayerLoopUpdate();
            buttonAction = GetStateInfo(ButtonState.Stop, out buttonText);

            Rect progressRect = new Rect(waveformRect);
            float time = AudioUtility.GetClipPosition(clip);
            float length = clip.length;
            float progress = time / length;
            float width = progressRect.width * progress;
            progressRect.width = Mathf.Clamp(width, 3, width);
            GUI.Box(progressRect, "", "SelectionRect");
            DrawLabelOutline(waveformRect, TimeSpan.FromSeconds(length - time).ToString(@"mm\:ss\:fff"));
        }
        else
        {
            buttonAction = GetStateInfo(ButtonState.Play, out buttonText);
            float length = clip.length;
            DrawLabelOutline(waveformRect, TimeSpan.FromSeconds(length).ToString(@"mm\:ss\:fff"));
        }

        if (GUI.Button(buttonRect, buttonText))
        {
            AudioUtility.StopAllClips();
            buttonAction(prop, clip);
        }

    }

    private static void Play(SerializedProperty prop, AudioClip clip)
    {
        CurrentClip = prop.propertyPath;
        AudioUtility.PlayClip(clip);
    }

    private static void Stop(SerializedProperty prop, AudioClip clip)
    {
        CurrentClip = "";
        AudioUtility.StopClip(clip);
    }

    private Action<SerializedProperty, AudioClip> GetStateInfo(ButtonState state, out string buttonText)
    {
        buttonText = state == ButtonState.Play ? "►" : "■";
        return _audioButtonStates[state];
    }

}
