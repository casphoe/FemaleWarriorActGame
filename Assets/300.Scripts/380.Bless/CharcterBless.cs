using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ChacterBlessDb
{
    Gold, Exp, AttackUp, DefenceUp
}

public class CharcterBless : MonoBehaviour
{
    public ChacterBlessDb bless;

    Image image;
    float duration;


    private void Awake()
    {
        duration = 1800f;
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        if(image != null)
        {
            image.fillAmount = 1f;
            StartCoroutine(BlessCorute());
        }
    }

    IEnumerator BlessCorute()
    {
        float time = 0;
        float initialFillAmount = image.fillAmount;

        while(time < duration)
        {
            time += Time.deltaTime;
            switch(bless)
            {
                case ChacterBlessDb.Gold:
                    PlayerManager.instance.player.buffRemainTime[0] -= Time.deltaTime;
                    break;
                case ChacterBlessDb.Exp:
                    PlayerManager.instance.player.buffRemainTime[1] -= Time.deltaTime;
                    break;
                case ChacterBlessDb.AttackUp:
                    PlayerManager.instance.player.buffRemainTime[2] -= Time.deltaTime;
                    break;
                case ChacterBlessDb.DefenceUp:
                    PlayerManager.instance.player.buffRemainTime[3] -= Time.deltaTime;
                    break;
            }
            image.fillAmount = Mathf.Lerp(1, 0, time / duration);
            yield return null;
        }

        image.fillAmount = 0f;
        Utils.OnOff(gameObject, false);
    }
}