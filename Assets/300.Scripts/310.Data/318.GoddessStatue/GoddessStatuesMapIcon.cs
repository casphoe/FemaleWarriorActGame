using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoddessStatuesMapIcon : MonoBehaviour
{
    public string statueID;
    public TextMeshProUGUI nameText;
    public RectTransform iconTransform;

    public void Setup(string id, string displayName, Vector2 anchoredPos)
    {
        statueID = id;
        nameText.text = displayName;
        iconTransform.anchoredPosition = anchoredPos;
    }
}
