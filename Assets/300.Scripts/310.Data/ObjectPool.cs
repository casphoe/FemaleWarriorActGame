using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//오브젝트 풀링 관리 함수
public class ObjectPool : MonoBehaviour
{
    List<GameObject> damageList = new List<GameObject>();

    [SerializeField] GameObject damageTxtPrefab;

    int damagePoolSize = 70;

    public static ObjectPool instance;

    private void Awake()
    {
        instance = this;
        InitializeTxtPool();
    }

    #region 데미지 텍스트
    void InitializeTxtPool()
    {
        for (int i = 0; i < damagePoolSize; i++)
        {
            GameObject txt = Instantiate(damageTxtPrefab, transform);
            txt.transform.SetParent(this.transform.GetChild(0).transform);
            txt.SetActive(false);
            damageList.Add(txt);
        }
    }

    public void SetDamageText(Vector3 target, int num, float damage)
    {
        GameObject txt = GetDamgeText();
        if (txt != null)
        {
            txt.transform.position = target;
            txt.SetActive(true);

            DamgeTxt _damage = txt.GetComponent<DamgeTxt>();
            if (_damage != null)
            {
                _damage.damageTxtSetting(num, damage);
            }
        }
    }

    GameObject GetDamgeText()
    {
        foreach (GameObject damageText in damageList)
        {
            if (!damageText.activeInHierarchy)
            {
                damageText.SetActive(true);
                return damageText;
            }
        }

        GameObject _damageText = Instantiate(damageTxtPrefab);
        _damageText.SetActive(true);
        damageList.Add(_damageText);
        return _damageText;
    }
    #endregion
}
