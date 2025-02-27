using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Utils 
{
    internal static void OnOff(GameObject obj, bool isActive)
    {
        obj.SetActive(isActive);
    }

    internal static void ImageColorChange(Image img, Color color)
    {
        img.color = color;
    }

    internal static void TextColorChange(Text txt,Color color)
    {
        txt.color = color;
    }

    internal static void ImageSpriteChange(Image img, Sprite sprite)
    {
        img.sprite = sprite;
    }

    internal static string GetThousandCommaText(int data)
    {
        if (data == 0)
        {
            return "0";
        }
        else
        {
            return string.Format("{0:#,###}", data);
        }
    }

    internal static string GetThousandCommaText(float data)
    {
        if (data == 0)
        {
            return "0"; // 0�� ���� �״�� 0���� ǥ��
        }
        else
        {
            return string.Format("{0:#,0.##}", data); //�Ҽ��� ������ ����
        }
    }

    internal static int GetStringToInt(string str)
    {
        string _str = str.Replace(",", "");

        int intvalue = int.Parse(_str);

        return intvalue;
    }

}
