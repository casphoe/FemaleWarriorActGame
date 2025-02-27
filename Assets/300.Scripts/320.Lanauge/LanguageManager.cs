using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json; // Add this using statement for Newtonsoft.Json

[System.Serializable]
public class LocalizedData
{
    public string key;
    public string value;
}

public class LanguageManager : MonoBehaviour
{
    private static LanguageManager instance;

    internal static LanguageManager Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<LanguageManager>();
            return instance;
        }
    }

    public TextAsset jsonFile;
    private Dictionary<string, string> localizedTexts;
    public bool isJsonLoaded = false; // Flag to indicate if JSON file has finished loading
    private List<LanguageText> textComponentsToUpdate = new List<LanguageText>(); // List to store text components to update

    private void Awake()
    {
        StartCoroutine(LoadLocalizedTexts());
    }

    // JSON file load and parse
    IEnumerator LoadLocalizedTexts()
    {
        yield return null;
        localizedTexts = new Dictionary<string, string>();
        if (jsonFile != null)
        {
            string json = jsonFile.text;
            Dictionary<string, Dictionary<string, string>> data =
                JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);

            foreach (var localizedTextKey in data.Keys)
            {
                // 각 언어에 대한 텍스트 값을 가져와 localizedTexts 딕셔너리에 추가
                string engText = data[localizedTextKey]["ENG"];
                string korText = data[localizedTextKey]["KOR"];
               

                // localizedTexts 딕셔너리에 텍스트 추가
                localizedTexts.Add($"{localizedTextKey}_ENG", engText);
                localizedTexts.Add($"{localizedTextKey}_KOR", korText);             
            }
        }
        else
        {
            //Debug.LogError("JSON file is null. Please assign a JSON file to the LocalizationManager.");
        }

        isJsonLoaded = true; // Set the flag to indicate JSON file has finished loading
        //Debug.Log("Localized text loaded.");

        // Call the UpdateUIText() function on all stored text components
        foreach (var textComponent in textComponentsToUpdate)
        {
            textComponent.UpdateUIText();
        }
        textComponentsToUpdate.Clear(); // Clear the list after updating text components
    }

    // Get localized text by key
    public string GetLocalizedText(string key)
    {
        string languageKey = GameManager.data.lanauge.ToString();
        if (localizedTexts.ContainsKey(key + "_" + languageKey))
        {
            return localizedTexts[key + "_" + languageKey];
        }
        else
        {
            //Debug.LogWarning("key: " + key + ", Key not found for language: " + languageKey);
            return key;
        }
    }

    // Register text component to update
    public void RegisterTextComponent(LanguageText textComponent)
    {
        if (isJsonLoaded)
        {
            // Call the UpdateUIText() function immediately if JSON file has already loaded
            textComponent.UpdateUIText();
        }
        else
        {
            // Add the text component to the list to update after JSON file has loaded
            textComponentsToUpdate.Add(textComponent);
        }
    }

    public List<LanguageText> FindActiveObjectsWithLocalizedText()
    {
        List<LanguageText> activeLocalizedTexts = new List<LanguageText>();

        // Find all objects with LocalizedText component in the scene
        LanguageText[] allLocalizedTexts = GameObject.FindObjectsOfType<LanguageText>();

        // Loop through all found LocalizedText components
        foreach (LanguageText localizedText in allLocalizedTexts)
        {
            // Check if the game object that the LocalizedText component is attached to is active in the hierarchy
            if (localizedText.gameObject.activeInHierarchy)
            {
                // Add the LocalizedText component to the list of active objects
                activeLocalizedTexts.Add(localizedText);
            }
        }

        return activeLocalizedTexts;
    }

    public void UpdateLocalizedTexts() //전체 텍스트를 업데이트 시켜벼림
    {
        // Call the FindActiveObjectsWithLocalizedText() function to get the activeLocalizedTexts list
        List<LanguageText> activeLocalizedTexts = FindActiveObjectsWithLocalizedText();

        // Access the activeLocalizedTexts list and perform actions on its elements
        foreach (LanguageText localizedText in activeLocalizedTexts)
        {
            // Call methods or modify properties of the localizedText objects
            localizedText.UpdateUIText();
        }       
    }
}
