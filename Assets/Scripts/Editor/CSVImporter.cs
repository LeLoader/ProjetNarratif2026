using SFB;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class CSVImporter : EditorWindow
{
    private Button selectFilePathButton;
    private string lastSelectedFolder;

    public void OnEnable()
    {
        if (lastSelectedFolder == "") lastSelectedFolder = Application.persistentDataPath;
    }

    [MenuItem("Window/Projet Narratif 2026/Dilemas Importer")]
    public static void ShowMyEditor()
    {
        // This method is called when the user selects the menu item in the Editor.
        EditorWindow wnd = GetWindow<CSVImporter>();
        GUIContent header = EditorGUIUtility.IconContent("Download-Available");
        header.text = "Dilemas Importer";
        wnd.titleContent = header;

        // Limit size of the window.
        wnd.minSize = new Vector2(450, 200);
        wnd.maxSize = new Vector2(1920, 720);
    }

    public void CreateGUI()
    {
        Box box = new();
        box.style.flexDirection = FlexDirection.Row;
        rootVisualElement.Add(box);

        Label label = new();
        label.text = "Currently selected csv file: ";
        box.Add(label);

        Label validLabel = new();
        validLabel.text = "Waiting for a file to be selected";

        Label invalidLabel = new(); 
        invalidLabel.text = "";

        selectFilePathButton = new Button(() =>
        {
            string[] output = StandaloneFileBrowser.OpenFilePanel("Import Dilema", lastSelectedFolder, "csv", false);
            if (output.Length > 0)
            {
                lastSelectedFolder = output[0];
                selectFilePathButton.text = output[0];
                if (File.Exists(selectFilePathButton.text))
                {
                    string[] allLines = File.ReadAllLines(selectFilePathButton.text);
                    int validLineCount = 0;
                    foreach (string line in allLines)
                    {
                        string[] splitData = line.Split(',');
                        if (splitData[0] != "")
                        {
                            validLineCount++;
                        }
                    }
                    validLabel.text = $"{validLineCount} valid(s) lines";
                    validLabel.style.color = new StyleColor(Color.green);

                    invalidLabel.text = $"{allLines.Count() - validLineCount} invalid(s) lines";
                    invalidLabel.style.color = new StyleColor(Color.red);
                }
            }
        });
        selectFilePathButton.text = "None";
        box.Add(selectFilePathButton);

        box.Add(validLabel);
        box.Add(invalidLabel);

        Button importDataEraseButton = new Button(() =>
        {
            ImportDilemas(true);
        });
        importDataEraseButton.text = "Import data (DANGEROUS)";
        rootVisualElement.Add(importDataEraseButton);

        Button importDataKeepButton = new Button(() =>
        {
            ImportDilemas(false);
        });
        importDataKeepButton.text = "Import data (SAFE)";
        rootVisualElement.Add(importDataKeepButton);
    }

    private void ImportDilemas(bool eraseExistingAssets)
    {
        if (File.Exists(selectFilePathButton.text))
        {
            string[] allLines = File.ReadAllLines(selectFilePathButton.text);
            RemoveInvalidLines(ref allLines, eraseExistingAssets);

            PreImportDilemas(allLines);
            PostImportDilemas(allLines, eraseExistingAssets);
            EditorUtility.FocusProjectWindow();
        }
    }

    private void RemoveInvalidLines(ref string[] lines, bool eraseExistingAssets)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            if (line.Split(',')[0] == "")
            {
                Debug.LogWarning($"Empty key found at line {i}");
                lines[i] = "";
                // listLines.RemoveAt(i);
                continue;
            }

            if (!eraseExistingAssets && UnityEditor.AssetDatabase.AssetPathExists($"Assets/Data/Dilemas/DIL_{line.Split(',')[0]}.asset"))
            {
                lines[i] = "";
                // listLines.RemoveAt(i);
                continue;
            }
        }

        List<string> listLines = lines.ToList();
        listLines.RemoveAll((a) => a == "");
        lines = listLines.ToArray();
    }

    private void PreImportDilemas(string[] linesToImport)
    {
        for (int i = 0; i < linesToImport.Length; i++)
        {
            string line = linesToImport[i];

            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            string[] splitData = CSVParser.Split(line);

            // 0 INDEX
            // 1 REPEATABLE
            // 2 CONDITIONS
            // 3 NEW DILEMAS
            // 4 NPC TO SPAWN

            // 1st CHOICE
            // 5 NEW DILEMAS
            // 6 ACTIONS

            // 7 + ENDOCTRINATED
            // 8 / NEUTRAL
            // 9 - FREEWILL
            // 10 + VIOLENCE
            // 11 / NEUTRAL
            // 12 - PEACE

            // 2nd CHOICE
            // 13 NEW DILEMAS
            // 14 ACTIONS

            // 15 + ENDOCTRINATED
            // 16 / NEUTRAL
            // 17 - FREEWILL
            // 18 + VIOLENCE
            // 19 / NEUTRAL
            // 20 - PEACE

            SODilema dilema = CreateInstance<SODilema>();
            dilema.key = splitData[0];

            bool bRepeatable;
            if (bool.TryParse(splitData[1], out bRepeatable)) dilema.bRepeatable = bRepeatable;

            dilema.question = new LocalizedString("DilemasTable", $"QUE_{dilema.key}");
            // dilema.appearanceConditions = [splitData[2]];

            int npcToSpawn;
            if (int.TryParse(splitData[4], out npcToSpawn)) dilema.npcToSpawn = npcToSpawn;

            // First choice
            dilema.firstChoice.label = new LocalizedString("DilemasTable", $"ANS_{dilema.key}_1");
            // dilema.firstChoice.actions. ;
            dilema.firstChoice.consequences = new();

            int tempInt = 0;
            if (int.TryParse(splitData[7], out tempInt)) if (tempInt != 0) dilema.firstChoice.consequences.Add(new(EMetricType.INDOCTRINATED, EMetricState.NEGATIVE, tempInt));
            if (int.TryParse(splitData[8], out tempInt)) if (tempInt != 0) dilema.firstChoice.consequences.Add(new(EMetricType.INDOCTRINATED, EMetricState.NEUTRAL, tempInt)); 
            if (int.TryParse(splitData[9], out tempInt)) if (tempInt != 0) dilema.firstChoice.consequences.Add(new(EMetricType.INDOCTRINATED, EMetricState.POSITIVE, tempInt));
            if (int.TryParse(splitData[10], out tempInt)) if (tempInt != 0) dilema.firstChoice.consequences.Add(new(EMetricType.VIOLENCE, EMetricState.NEGATIVE, tempInt));
            if (int.TryParse(splitData[11], out tempInt)) if (tempInt != 0) dilema.firstChoice.consequences.Add(new(EMetricType.VIOLENCE, EMetricState.NEUTRAL, tempInt));
            if (int.TryParse(splitData[12], out tempInt)) if (tempInt != 0) dilema.firstChoice.consequences.Add(new(EMetricType.VIOLENCE, EMetricState.POSITIVE, tempInt));
            
            // Second choice
            dilema.secondChoice.label = new LocalizedString("DilemasTable", $"ANS_{dilema.key}_2");
            // dilema.secondChoice.actions. ;
            dilema.secondChoice.consequences = new();

            if (int.TryParse(splitData[15], out tempInt)) if (tempInt != 0) dilema.secondChoice.consequences.Add(new(EMetricType.INDOCTRINATED, EMetricState.NEGATIVE, tempInt));
            if (int.TryParse(splitData[16], out tempInt)) if (tempInt != 0) dilema.secondChoice.consequences.Add(new(EMetricType.INDOCTRINATED, EMetricState.NEUTRAL, tempInt));
            if (int.TryParse(splitData[17], out tempInt)) if (tempInt != 0) dilema.secondChoice.consequences.Add(new(EMetricType.INDOCTRINATED, EMetricState.POSITIVE, tempInt));
            if (int.TryParse(splitData[18], out tempInt)) if (tempInt != 0) dilema.secondChoice.consequences.Add(new(EMetricType.VIOLENCE, EMetricState.NEGATIVE, tempInt));
            if (int.TryParse(splitData[19], out tempInt)) if (tempInt != 0) dilema.secondChoice.consequences.Add(new(EMetricType.VIOLENCE, EMetricState.NEUTRAL, tempInt));
            if (int.TryParse(splitData[20], out tempInt)) if (tempInt != 0) dilema.secondChoice.consequences.Add(new(EMetricType.VIOLENCE, EMetricState.POSITIVE, tempInt));

            DilemaManager.dilemaDatabase.AddDilema(dilema);
        }
    }

    private void PostImportDilemas(string[] linesToImport, bool eraseExistingAssets)
    {
        for (int i = 0; i < linesToImport.Length; i++)
        {
            string line = linesToImport[i];

            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            string[] splitData = CSVParser.Split(line);

            SODilema dilema = DilemaManager.GetDilema(splitData[0]);

            // dilema.appearanceConditions = [splitData[2]];

            dilema.newDilemas = new();
            foreach (string key in splitData[3].Split(','))
            {
                string cleanKey = Regex.Replace(key, "[^a-zA-Z0-9-_]", "");
                SODilema foundDilema = DilemaManager.GetDilema(cleanKey);
                if (foundDilema)
                {
                    dilema.newDilemas.Add(foundDilema);
                }
            }

            dilema.firstChoice.newDilemas = new();
            foreach (string key in splitData[5].Split(','))
            {
                string cleanKey = Regex.Replace(key, "[^a-zA-Z0-9-_]", "");
                SODilema foundDilema = DilemaManager.GetDilema(cleanKey);
                if (foundDilema)
                {
                    dilema.firstChoice.newDilemas.Add(foundDilema);
                } 
            }

            // dilema.firstChoice.actions. ;

            dilema.secondChoice.newDilemas = new();
            foreach (string key in splitData[13].Split(','))
            {
                string cleanKey = Regex.Replace(key, "[^a-zA-Z0-9-_]", "");
                SODilema foundDilema = DilemaManager.GetDilema(cleanKey);
                if (foundDilema)
                {
                    dilema.secondChoice.newDilemas.Add(foundDilema);
                }
            }

            // dilema.secondChoice.actions. ;

            string path = "";
            if (eraseExistingAssets)
            {
                path = $"Assets/Data/Dilemas/DIL_{dilema.key}.asset";
            }
            else
            {
                path = UnityEditor.AssetDatabase.GenerateUniqueAssetPath($"Assets/Data/Dilemas/DIL_{dilema.key}.asset");
            }

            AssetDatabase.CreateAsset(dilema, path);
            AssetDatabase.SaveAssets();
            Selection.activeObject = dilema;
        }
    }
}