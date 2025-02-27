using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    [SerializeField] Button[] btnGameOption;

    [SerializeField] Button[] btnLanaugeOption;

    [SerializeField] Button[] btnBgm;
    [SerializeField] Button[] btnEffect;

    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider effectSlider;

    public static OptionManager instance;


    private void Awake()
    {
        btnGameOption[0].onClick.AddListener(() => GameOptionSetting(0));
        btnGameOption[1].onClick.AddListener(() => GameOptionSetting(1));
        btnGameOption[2].onClick.AddListener(() => GameOptionSetting(2));

        btnLanaugeOption[0].onClick.AddListener(() => LanaugeSetting(0));
        btnLanaugeOption[1].onClick.AddListener(() => LanaugeSetting(1));

        btnBgm[0].onClick.AddListener(() => BgmSoundSetting(0));
        btnBgm[1].onClick.AddListener(() => BgmSoundSetting(1));

        btnEffect[0].onClick.AddListener(() => EffectSoundSetting(0));
        btnEffect[1].onClick.AddListener(() => EffectSoundSetting(1));

        instance = this;
        GameManager.LoadDataToIni();
        LanguageManager.Instance.UpdateLocalizedTexts();
    }


    public void GameOptionSetting(int num)
    {
        for(int i = 0; i < btnGameOption.Length; i++)
        {
            Utils.ImageColorChange(btnGameOption[i].image, Color.white);
            Utils.TextColorChange(btnGameOption[i].transform.GetChild(0).GetComponent<Text>(), Color.black);
        }
        Utils.ImageColorChange(btnGameOption[num].image, Color.red);
        Utils.TextColorChange(btnGameOption[num].transform.GetChild(0).GetComponent<Text>(), Color.white);
        switch (num)
        {
            case 0:
                GameManager.data.diffucity = Diffucity.Easy;
                break;
            case 1:
                GameManager.data.diffucity = Diffucity.Normal;
                break;
            case 2:
                GameManager.data.diffucity = Diffucity.Hard;
                break;
        }
        GameManager.SaveDataToIni();
    }

    public void LanaugeSetting(int num)
    {
        for (int i = 0; i < btnLanaugeOption.Length; i++)
        {
            Utils.ImageColorChange(btnLanaugeOption[i].image, Color.white);
            Utils.TextColorChange(btnLanaugeOption[i].transform.GetChild(0).GetComponent<Text>(), Color.black);
        }
        Utils.ImageColorChange(btnLanaugeOption[num].image, Color.red);
        Utils.TextColorChange(btnLanaugeOption[num].transform.GetChild(0).GetComponent<Text>(), Color.white);
        switch(num)
        {
            case 0:
                GameManager.data.lanauge = LANGUAGE.KOR;
                break;
            case 1:
                GameManager.data.lanauge = LANGUAGE.ENG;
                break;
        }
        LanguageManager.Instance.UpdateLocalizedTexts();
        GameManager.SaveDataToIni();
    }

    public void BgmSoundSetting(int num)
    {
        for (int i = 0; i < btnBgm.Length; i++)
        {
            Utils.ImageColorChange(btnBgm[i].image, Color.white);
            Utils.TextColorChange(btnBgm[i].transform.GetChild(0).GetComponent<Text>(), Color.black);
        }
        Utils.ImageColorChange(btnBgm[num].image, Color.red);
        Utils.TextColorChange(btnBgm[num].transform.GetChild(0).GetComponent<Text>(), Color.white);
        switch(num)
        {
            case 0:
                bgmSlider.value = 1;
                break;
            case 1:
                bgmSlider.value = 0;
                break;
        }
        GameManager.data.bgmSoundValue = bgmSlider.value;
        GameManager.SaveDataToIni();
    }

    public void EffectSoundSetting(int num)
    {
        for (int i = 0; i < btnEffect.Length; i++)
        {
            Utils.ImageColorChange(btnEffect[i].image, Color.white);
            Utils.TextColorChange(btnEffect[i].transform.GetChild(0).GetComponent<Text>(), Color.black);
        }
        Utils.ImageColorChange(btnEffect[num].image, Color.red);
        Utils.TextColorChange(btnEffect[num].transform.GetChild(0).GetComponent<Text>(), Color.white);
        switch(num)
        {
            case 0:
                effectSlider.value = 1;
                break;
            case 1:
                effectSlider.value = 0;
                break;
        }
        GameManager.data.effectSoundValue = effectSlider.value;
        GameManager.SaveDataToIni();
    }

    public void SliderChange(int num)
    {
        switch(num)
        {
            case 0:
                for (int i = 0; i < btnBgm.Length; i++)
                {
                    Utils.ImageColorChange(btnBgm[i].image, Color.white);
                    Utils.TextColorChange(btnBgm[i].transform.GetChild(0).GetComponent<Text>(), Color.black);
                }
                if (bgmSlider.value == 0)
                {
                    Utils.ImageColorChange(btnBgm[1].image, Color.red);
                    Utils.TextColorChange(btnBgm[1].transform.GetChild(0).GetComponent<Text>(), Color.white);
                }
                else
                {
                    Utils.ImageColorChange(btnBgm[0].image, Color.red);
                    Utils.TextColorChange(btnBgm[0].transform.GetChild(0).GetComponent<Text>(), Color.white);
                }
                GameManager.data.bgmSoundValue = bgmSlider.value;
                break;
            case 1:
                for (int i = 0; i < btnEffect.Length; i++)
                {
                    Utils.ImageColorChange(btnEffect[i].image, Color.white);
                    Utils.TextColorChange(btnEffect[i].transform.GetChild(0).GetComponent<Text>(), Color.black);
                }
                if (effectSlider.value == 0)
                {
                    Utils.ImageColorChange(btnEffect[1].image, Color.red);
                    Utils.TextColorChange(btnEffect[1].transform.GetChild(0).GetComponent<Text>(), Color.white);
                }
                else
                {
                    Utils.ImageColorChange(btnEffect[0].image, Color.red);
                    Utils.TextColorChange(btnEffect[0].transform.GetChild(0).GetComponent<Text>(), Color.white);
                }
                GameManager.data.effectSoundValue = effectSlider.value;
                break;
        }
        GameManager.SaveDataToIni();
    }
}
