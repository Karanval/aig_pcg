using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NoiseAnimator))]
public class NoiseAnimatorEditor : Editor
{
    NoiseAnimator noiseAnimator;
    Editor editor;

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            {
                noiseAnimator.OnSettingsUpdated();
            }
        }

        DrawSettingsEditor(noiseAnimator.settings, noiseAnimator.OnSettingsUpdated, ref noiseAnimator.settingsFoldout);
    }

    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout)
    {
        if (settings != null)
        {
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
            using (var check = new EditorGUI.ChangeCheckScope())
            {

                if (foldout)
                {
                    CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();

                    if (check.changed)
                    {
                        if (onSettingsUpdated != null)
                        {
                            onSettingsUpdated();
                        }
                    }
                }
            }
        }
    }

    private void OnEnable()
    {
        noiseAnimator = (NoiseAnimator)target;
    }
}
