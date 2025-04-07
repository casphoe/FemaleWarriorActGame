using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//오브젝트 풀링 관리 함수
public class ObjectPool : MonoBehaviour
{
    List<GameObject> damageList = new List<GameObject>();
    List<GameObject> ConfusionList = new List<GameObject>();
    List<GameObject> SlasheList = new List<GameObject>();

    [SerializeField] GameObject damageTxtPrefab;
    [SerializeField] GameObject damageConfusionPrefab;
    [SerializeField] GameObject slasehrefab;

    int damagePoolSize = 70;
    int confusionPoolSize = 40;
    int slashPoolSize = 40;

    public static ObjectPool instance;

    private void Awake()
    {
        instance = this;
        InitializeTxtPool();
        InitializeConfusionPool();
        InitializeSlashPool();
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

    #region 혼란을 알려주는 오브젝트
    void InitializeConfusionPool()
    {
        for (int i = 0; i < confusionPoolSize; i++)
        {
            GameObject confusion = Instantiate(damageConfusionPrefab, transform);
            confusion.transform.SetParent(this.transform.GetChild(1).transform);
            confusion.SetActive(false);
            ConfusionList.Add(confusion);
        }
    }

    public void SetConfusion(Vector3 target, float duration)
    {
        GameObject confusion = GetConfusion();
        if(confusion != null)
        {
            confusion.transform.position = target;
            confusion.SetActive(true);

            ConfusionQuestionMark mark = confusion.GetComponent<ConfusionQuestionMark>();
            if(mark != null)
            {
                mark.ActivateForDuration(duration);
            }
        }
    }

    GameObject GetConfusion()
    {
        foreach (GameObject damageConfusion in ConfusionList)
        {
            if (!damageConfusion.activeInHierarchy)
            {
                damageConfusion.SetActive(true);
                return damageConfusion;
            }
        }

        GameObject _damageConfusion = Instantiate(damageConfusionPrefab);
        _damageConfusion.SetActive(true);
        ConfusionList.Add(_damageConfusion);
        return _damageConfusion;
    }
    #endregion

    #region 검기
    void InitializeSlashPool()
    {
        for (int i = 0; i < slashPoolSize; i++)
        {
            GameObject slash = Instantiate(slasehrefab, transform);
            slash.transform.SetParent(this.transform.GetChild(2).transform);
            slash.SetActive(false);
            SlasheList.Add(slash);
        }
    }

    public void SetSlash(Vector3 target,float damage,float cirticleRate,float cirticleDamage,float moveSpeed, float duration, Vector2 moveDir, float expandSpeed, SlashState state)
    {
        GameObject slash = GetSlash();
        if(slash != null)
        {
            slash.transform.position = target;
            slash.SetActive(true);

            Slash _slash = slash.GetComponent<Slash>();
            if(_slash != null)
            {
                _slash.Init(damage, cirticleRate, cirticleDamage, moveSpeed, duration, moveDir, expandSpeed, state);
            }
        }
    }

    GameObject GetSlash()
    {
        foreach (GameObject slashObject in SlasheList)
        {
            if (!slashObject.activeInHierarchy)
            {
                slashObject.SetActive(true);
                return slashObject;
            }
        }

        GameObject _slashObject = Instantiate(slasehrefab);
        _slashObject.SetActive(true);
        SlasheList.Add(_slashObject);
        return _slashObject;
    }
    #endregion
}
