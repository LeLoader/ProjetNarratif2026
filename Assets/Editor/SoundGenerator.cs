using System.Collections.Generic;
using System.IO;
using SFB;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundGenerator : EditorWindow
{
    string _lastSelectedPath;
    public SOSounds _soundDatabase;
    public void OnEnable()
    {
        if (_lastSelectedPath == "")
        {
            _lastSelectedPath = Application.dataPath;
        }

        _soundDatabase = Resources.Load<SOSounds>("Databases/SoundDatabase");
        if (_soundDatabase == null)
        {
            Debug.Log("uh");
        }
    }

    [MenuItem("Window/Projet Narratif 2026/Sound Generator")]
    public static void ShowMyEditor()
    {
        EditorWindow editorWindow = GetWindow<SoundGenerator>();
        GUIContent header = EditorGUIUtility.IconContent("AudioSource Gizmo");
        header.text = "Source Generator";
        editorWindow.titleContent = header;

        // Limit size of the window.
        editorWindow.minSize = new Vector2(450, 200);
        editorWindow.maxSize = new Vector2(1920, 720);
    }

    public void CreateGUI()
    {
        _soundDatabase = Resources.Load<SOSounds>("Databases/SoundDatabase");
        if (_soundDatabase == null)
        {
            Debug.Log("uh");
        }
        Button LoadButton = new Button(() =>
        {
            var allObjectGuids = AssetDatabase.FindAssets("t:AudioClip");
            _soundDatabase.Reset();
            foreach (var guid in allObjectGuids)
            {
                AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(AssetDatabase.GUIDToAssetPath(guid));
                string[] prefix = clip.name.Split("_");
                ESoundType currentType;
                switch (prefix[0])
                {
                    case "SFX":
                        currentType = ESoundType.SFX;
                        break;
                    case "Music":
                        currentType = ESoundType.Music;
                        break;
                    default:
                        currentType = ESoundType.None;
                        break;
                }
                if (currentType == ESoundType.None)
                {
                    Debug.LogError($"[SOUND GENERATOR] audio clip {clip.name} is not named correctly.");
                    continue;
                }
                Sound _sound = new Sound(clip, currentType);
                _soundDatabase.Add(_sound);
            }
            _soundDatabase.DebugText = "Working";
            EditorUtility.SetDirty(_soundDatabase);
            
        });
        LoadButton.text = "Load Sounds";
        rootVisualElement.Add(LoadButton);
        Button DebugDatabaseButton = new Button(() =>
        {
            _soundDatabase.ShowAllValues();
        });
        DebugDatabaseButton.text = "Show all sounds in database";
        rootVisualElement.Add(DebugDatabaseButton);
    }

}