using SFB;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
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

            for (int i = 0; i < allLines.Length; i++)
            {
                string line = allLines[i];

                //string[] splitData = line.Split(',');
                Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                string[] splitData = CSVParser.Split(line);


                if (splitData[0] == "")
                {
                    Debug.LogWarning($"Empty key found at line {i}");
                    continue;
                }

                if (!eraseExistingAssets && UnityEditor.AssetDatabase.AssetPathExists($"Assets/Data/Dilemas/DIL_{splitData[0]}.asset"))
                {
                    continue;
                }

                SODilema dilema = CreateInstance<SODilema>();
                
                dilema.key = splitData[0];

                var e = LocalizationSettings.AssetDatabase.GetTableEntry("DilemasTable", $"QUE_{dilema.key}_1");

                if (bool.TryParse(splitData[1], out dilema.bRepeatable))
                {

                }
              
                dilema.question = new LocalizedString("DilemasTable", $"QUE_{dilema.key}");
                // dilema.appearanceConditions = [splitData[3]];
                foreach (string key in splitData[4].Split(','))
                {
                    dilema.newDilemas.Add(DilemaManager.GetDilema(key));
                }

                // First choice
                dilema.firstChoice.label = new LocalizedString("DilemasTable", $"ANS_{dilema.key}_1");
                // dilema.firstChoice.actions. ;
                // dilema.firstChoice.consequences. ;
                // dilema.firstChoice.newDilemas = DilemaManager.GetDilema([]);

                // Second choice
                dilema.secondChoice.label = new LocalizedString("DilemasTable", $"ANS_{dilema.key}_2");
                // dilema.secondChoice.actions. ;
                // dilema.secondChoice.consequences. ;
                // dilema.secondChoice.newDilemas. ;

                // Find a way to chose if you want to erase old assets, or keep them if they already exist
                // string path = UnityEditor.AssetDatabase.GenerateUniqueAssetPath($"Assets/Data/Dilemas/DIL_{dilema.key}.asset");

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
}
