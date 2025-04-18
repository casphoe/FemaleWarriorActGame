using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//여신상 관리
public class GoddessStatueManager : MonoBehaviour
{
    public static GoddessStatueManager instance;

    //등록된 여신상 ID 목록을 보관하는 HashSet
    /*
     * HashSet<T> : C# 에서 제공하는 집합(Set) 자료구조 
     * 1) 중복 불허 : 같은 값을 두번 추가 할 수 없음
     * 2) 빠른 탐색 속도 : 내부적으로 해시 테이블(Hashtable)로 동작해 탐색이 빠름
     * 3) 순서 없음 : 입력한 순서대로 저장되지 않음
     */
    public HashSet<string> registeredStatues = new HashSet<string>();

    public List<GoddessStatuesMapIcon> mapIcons = new List<GoddessStatuesMapIcon>();

    public bool isMapOpen = false;

    [SerializeField] GameObject MapUi;
    [SerializeField] Image[] imgArrow;

    [SerializeField] float blinkSpeed = 1.5f; // 깜빡임 주기 (초당 몇 번)
    private Coroutine blinkRoutine;

    private void Awake()
    {
        instance = this;
        MapOpenSet(false);
    }

    private void Update()
    {
        if (!isMapOpen) return;

        if (PlayerManager.GetCustomKeyDown(CustomKeyCode.Canel))
        {
            MapOpenSet(false);
        }

        Vector2 moveDir = Vector2.zero;

        if (PlayerManager.GetCustomKey(CustomKeyCode.Up)) moveDir += Vector2.down;
        if (PlayerManager.GetCustomKey(CustomKeyCode.Down)) moveDir += Vector2.up;
        if (PlayerManager.GetCustomKey(CustomKeyCode.Left)) moveDir += Vector2.left;
        if (PlayerManager.GetCustomKey(CustomKeyCode.Right)) moveDir += Vector2.right;


        if (moveDir != Vector2.zero)
        {
            currentCursorPos += new Vector2(moveDir.x, -moveDir.y) * moveSpeed * Time.deltaTime;
            ClampToContent();
            MoveCursor();
        }
    }

    public void RegisterStatue(string id)
    {
        if (!registeredStatues.Contains(id))
            registeredStatues.Add(id);
    }

    public bool IsStatueRegistered(string id) => registeredStatues.Contains(id);

    public List<string> GetRegisteredStatues() => registeredStatues.ToList();

    public bool IsStatueDiscovered(string id)
    {
        return registeredStatues.Contains(id);
    }

    public void MapOpenSet(bool open)
    {
        isMapOpen = open;
        Utils.OnOff(MapUi, isMapOpen);
        if (isMapOpen)
        {
            if (blinkRoutine == null)
                blinkRoutine = StartCoroutine(BlinkArrows());

            //커서 초기화 코류틴 실행
            StartCoroutine(InitCursorAfterLayout());
        }
        else
        {
            if (blinkRoutine != null)
            {
                StopCoroutine(blinkRoutine);
                blinkRoutine = null;
                ResetArrowsAlpha(); // 끄면 알파 복구
            }
        }
    }


    private IEnumerator BlinkArrows()
    {
        float t = 0f;

        while (true)
        {
            t += Time.deltaTime * blinkSpeed;
            float alpha = Mathf.Abs(Mathf.Sin(t)); // 0~1로 반복

            foreach (var arrow in imgArrow)
            {
                if (arrow != null)
                {
                    Color c = arrow.color;
                    c.a = alpha;
                    arrow.color = c;
                }
            }

            yield return null;
        }
    }

    private void ResetArrowsAlpha()
    {
        foreach (var arrow in imgArrow)
        {
            if (arrow != null)
            {
                Color c = arrow.color;
                c.a = 1f;
                arrow.color = c;
            }
        }
    }

    #region 맵

    [SerializeField] private RectTransform cursorTransform; // 커서 오브젝트

    public Dictionary<string, MapData> allMaps = new Dictionary<string, MapData>();

    [SerializeField] ScrollRect scrollRect;          // Scroll View

    [SerializeField] RectTransform contentTransform;          // Content

    private Vector2 currentCursorPos = Vector2.zero;

    [Header("속도 설정")]
    [SerializeField] private float moveSpeed = 500f; // 커서 이동 속도

    public void AddMap(string id, string name, MapType type, Vector2 uiPosition)
    {
        if (!allMaps.ContainsKey(id))
            allMaps[id] = new MapData(id, name, type, uiPosition);
    }

    public void OnEnterNewMap(string mapID)
    {
        if (allMaps.ContainsKey(mapID))
        {
            allMaps[mapID].isVisited = true;
        }
    }

    private IEnumerator InitCursorAfterLayout()
    {
        yield return null; // 한 프레임 대기해서 Layout 확정

        float centerX = contentTransform.rect.width * 0.5f;
        float centerY = -contentTransform.rect.height * 0.5f;

        currentCursorPos = new Vector2(centerX, centerY);
        ClampToContent(); // 위치 제한 걸기
        MoveCursor();     // 커서 + 스크롤 위치 적용
    }

    private void ClampToContent()
    {
        float contentWidth = contentTransform.rect.width;
        float contentHeight = contentTransform.rect.height;

        float cursorHalfW = cursorTransform.sizeDelta.x * 0.5f;
        float cursorHalfH = cursorTransform.sizeDelta.y * 0.5f;

        // Content의 좌상단 기준
        float minX = 0 + cursorHalfW;
        float maxX = contentWidth - cursorHalfW;

        float minY = -contentHeight + cursorHalfH;
        float maxY = 0 - cursorHalfH;

        currentCursorPos.x = Mathf.Clamp(currentCursorPos.x, minX, maxX);
        currentCursorPos.y = Mathf.Clamp(currentCursorPos.y, minY, maxY);
    }

    private void MoveCursor()
    {
        // localPosition으로 이동 (중요!)
        cursorTransform.localPosition = currentCursorPos;

        float viewWidth = scrollRect.viewport.rect.width;
        float contentWidth = contentTransform.rect.width;

        float targetX = currentCursorPos.x - (viewWidth * 0.5f);

        float minScrollX = 0f;
        float maxScrollX = contentWidth - viewWidth;

        targetX = Mathf.Clamp(targetX, minScrollX, maxScrollX);

        scrollRect.content.anchoredPosition = new Vector2(-targetX, scrollRect.content.anchoredPosition.y);
    }

    #endregion
}
#region 맵 데이터
public enum MapType
{
    Village,
    Cave,
    Forest,
    Dungeon,
    Unknown
}

[System.Serializable]
public class MapData
{
    public string mapID;
    public string mapName;
    public MapType type;
    public bool isVisited;
    public Vector2 mapPosition; // UI 좌표

    public MapData(string id, string name, MapType type, Vector2 pos)
    {
        this.mapID = id;
        this.mapName = name;
        this.type = type;
        this.mapPosition = pos;
        this.isVisited = false;
    }
}

#endregion