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
    [Header("맵 아이콘 및 변수")]
    public List<GoddessStatuesMapIcon> mapIcons = new List<GoddessStatuesMapIcon>();
    //텔레포트 좌표(여신상)
    public List<Vector2> goddlesStatuePosition = new List<Vector2>();

    public bool isMapOpen = false;

    [SerializeField] GameObject MapUi;
    [SerializeField] Image[] imgArrow;

    [SerializeField] float blinkSpeed = 1.5f; // 깜빡임 주기 (초당 몇 번)

    [SerializeField] GoddessStatuesMapIcon currentHoveredGoddessStatue = null;

    private Coroutine blinkRoutine;

    private void Awake()
    {
        instance = this;
        MapOpenSet(false);
        // 미리 contentTransform 자식에서 아이콘 찾아 등록
        var allIcons = contentTransform.GetComponentsInChildren<GoddessStatuesMapIcon>(true);
        foreach (var icon in allIcons)
        {
            Utils.OnOff(icon.gameObject, false);
            mapIcons.Add(icon);
        }

        AddMap("Vilage_0", "마을", "Vilage", MapType.Village);
        AddMap("GoddesSatute_0", "마을여신상", "VilageGoddessSatute", MapType.GoddessStatue);
        MoveCharacterToStatue("Vilage_0");
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

        if(currentHoveredGoddessStatue != null)
        {
            if(PlayerManager.GetCustomKeyDown(CustomKeyCode.Attack))
            {
                TeleportPlayerToStatue(currentHoveredGoddessStatue.statueID);
                MapOpenSet(false);
                PlayerManager.instance.isState = false;
                Utils.OnOff(GameCanvas.instance.blessPanel, false);
                MoveCharacterToStatue(currentHoveredGoddessStatue.statueID);
            }
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
    #region 맵 Ui 켜졌다가 꺼졌다가 하는 기능
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
    #endregion

    #region 여신상 화살표 깜빡이게 하는 기능
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
    #endregion

    #region 텔레포트 기능
    public void TeleportPlayerToStatue(string statueID)
    {
        // "GoddesSatute_0" -> 숫자만 추출 (뒤에 인덱스)
        string indexStr = new string(statueID.Where(char.IsDigit).ToArray());

        if (int.TryParse(indexStr, out int index))
        {
            if (index >= 0 && index < goddlesStatuePosition.Count)
            {
                Vector2 targetPosition = goddlesStatuePosition[index];

                Player.instance.transform.position = targetPosition;

                CM.instance.SnapToTarget();
            }
        }
    }
    #endregion

    #region 맵
    [Header("Map")]
    [SerializeField] private RectTransform cursorTransform; // 커서 오브젝트 위치

    [SerializeField] private RectTransform charcterUiTransform; // 캐릭터 위치 이미지

    public Dictionary<string, MapData> allMaps = new Dictionary<string, MapData>();

    [SerializeField] ScrollRect scrollRect;          // Scroll View

    [SerializeField] RectTransform contentTransform;          // Content

    private Vector2 currentCursorPos = Vector2.zero;

    [Header("속도 설정")]
    [SerializeField] private float moveSpeed = 500f; // 커서 이동 속도

    public void AddMap(string id, string nameKor,string nameEng, MapType type)
    {
        if (allMaps.ContainsKey(id)) return;

        // MapData 등록
        allMaps[id] = new MapData(id, nameKor,nameEng, type);

        // 아이콘 찾아서 설정 및 활성화
        GoddessStatuesMapIcon icon = mapIcons.FirstOrDefault(x => string.IsNullOrEmpty(x.statueID));

        if (icon != null)
        {
            icon.Setup(id, nameKor,nameEng, type); // 아이콘에 ID, 이름 설정
            Utils.OnOff(icon.gameObject, true); // 아이콘 켜주기
            //리스트에 없으면 추가
            if (!mapIcons.Contains(icon))
            {
                mapIcons.Add(icon);
            }
        }
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

        //커서가 아이콘 위에 있는지 확인
        CheckCursorOverIcons();
    }

    private void CheckCursorOverIcons()
    {
        currentHoveredGoddessStatue = null; // 기본은 null로 시작

        foreach (var icon in mapIcons)
        {
            if (icon == null) continue;

            RectTransform iconRect = icon.GetComponent<RectTransform>();
            if (iconRect == null) continue;

            bool isOver = IsCursorOver(iconRect);
           
            // mapData 가져오기
            allMaps.TryGetValue(icon.statueID, out var mapData);
            bool isGoddess = (mapData != null && mapData.type == MapType.GoddessStatue);

            //커서가 여신상 위에 있을 경우에만 저장
            if (isOver && isGoddess)
            {
                currentHoveredGoddessStatue = icon;
            }

            // 기본 텍스트 처리 (모든 아이콘)
            var tmp = icon.GetComponentInChildren<TMPro.TextMeshProUGUI>(true);
            if (tmp != null)
                Utils.OnOff(tmp.gameObject, isOver);

            // 여신상일 경우 1번째 자식도 같이 처리
            if (isGoddess && icon.transform.childCount > 1)
            {
                Transform secondChild = icon.transform.GetChild(1);
                Utils.OnOff(secondChild.gameObject, isOver);
            }
            else if (!isOver && isGoddess && icon.transform.childCount > 1)
            {
                // 여신상인데 커서가 벗어난 경우 -> 텍스트 + 1번째 자식 꺼줌
                Transform secondChild = icon.transform.GetChild(1);
                Utils.OnOff(secondChild.gameObject, false);
            }
        }
    }

    private bool IsCursorOver(RectTransform target)
    {
        Rect cursorRect = new Rect(
            cursorTransform.localPosition.x - cursorTransform.rect.width * 0.5f,
            cursorTransform.localPosition.y - cursorTransform.rect.height * 0.5f,
            cursorTransform.rect.width,
            cursorTransform.rect.height
        );

        Rect targetRect = new Rect(
            target.localPosition.x - target.rect.width * 0.5f,
            target.localPosition.y - target.rect.height * 0.5f,
            target.rect.width,
            target.rect.height
        );

        return cursorRect.Overlaps(targetRect);
    }
    //캐릭터 UI 위치 이동
    public void MoveCharacterToStatue(string statueID)
    {
        var icon = mapIcons.FirstOrDefault(x => x.statueID == statueID);
        if (icon != null)
        {
            charcterUiTransform.localPosition = icon.iconTransform.localPosition;
        }
    }

    #endregion
}
#region 맵 데이터
public enum MapType
{
    Village,
    Cave,
    GoddessStatue,
    Forest,
    Dungeon,
    Road,
    Unknown
}

[System.Serializable]
public class MapData
{
    public string mapID;
    public string mapNameKor;
    public string mapNameEng;
    public MapType type;
    public bool isVisited;

    public MapData(string id, string nameKor, string nameEng, MapType type)
    {
        this.mapID = id;
        this.mapNameKor = nameKor;
        this.mapNameEng = nameEng;
        this.type = type;
        this.isVisited = false;
    }
}

#endregion