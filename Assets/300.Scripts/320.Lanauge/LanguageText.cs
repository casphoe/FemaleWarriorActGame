using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageText : MonoBehaviour
{
    public Text txt;
    public string localizedKey;

    // Start is called before the first frame update
    void Start()
    {
        txt = GetComponent<Text>();
        localizedKey = txt.text;
        LanguageManager.Instance.RegisterTextComponent(this);
    }

    private void OnEnable()
    {
        if (txt != null)
        {
            UpdateUIText();
        }
    }

    public void UpdateUIText()
    {
        if (txt != null && LanguageManager.Instance.isJsonLoaded == true)
        {
            txt.text = LanguageManager.Instance.GetLocalizedText(localizedKey);
        }
    }
}
