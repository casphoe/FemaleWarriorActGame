using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
    [SerializeField] GameObject panel;

    [SerializeField] Text[] txt;

    public static ShopPanel instance;

    private void Awake()
    {
        instance = this;
        Utils.OnOff(panel, false);
    }

    private void OnEnable()
    {
        Utils.OnOff(panel, false);
    }

    public void ShopTextSetting(string tilte, string desc)
    {
        Utils.OnOff(panel, true);
        txt[0].text = tilte;
        txt[1].text = desc;
        StartCoroutine(ShopOff());
    }

    IEnumerator ShopOff()
    {
        yield return new WaitForSeconds(2);
        Utils.OnOff(panel, false);
    }
}
