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

    [SerializeField] GoddessStatuesMapIcon currentHoveredGoddessStatue = null;

    // 현재 캐릭터 마커가 가리키는 여신상 ID 저장(세이브/로드용)
    private string currentCharacterStatueId = string.Empty;


    private void Awake()
    {
        instance = this;
        MapOpenSet(false);

        mapArrowObjectList.Clear();
        for (int i = 0; i < ArrowParent.transform.childCount; i++)
            mapArrowObjectList.Add(ArrowParent.transform.GetChild(i).GetComponent<ArrowMapIcon>());
        foreach (var arrow in mapArrowObjectList) Utils.OnOff(arrow.gameObject, false);
    }

    private void Start()
    {
        // 씬에 배치해둔 아이콘을 allMaps에 보정 등록 (이름은 아이콘에 이미 세팅돼있다면 그 값을 사용)
        // 아이콘/화살표 수집만
        var allIcons = contentTransform.GetComponentsInChildren<GoddessStatuesMapIcon>(true);
        mapIcons.Clear();
        foreach (var icon in allIcons)
        {
            Utils.OnOff(icon.gameObject, false);
            mapIcons.Add(icon);
        }

        StartCoroutine(InitializeMapLoad());
    }

    // 저장값(없으면 기본값)으로 맵
    IEnumerator InitializeMapLoad()
    {

        yield return new WaitForSeconds(0.5f);

        var pd = PlayerManager.instance.player;

        bool isFresh = pd.visitedMapsList.Count == 0;

        Debug.Log(isFresh);

        if (isFresh)
        {
            // 완전 새 게임: 기본 맵/여신상 한 번만 등록
            AddMap("Vilage_0", "마을", "Vilage", MapType.Village, 1, 0);
            OnEnterNewMap("Vilage_0");
            MoveCharacterToStatue("Vilage_0");

            if (pd != null)
            {
                pd.currentMapNum = 1;
                pd.lastStatueId = "Vilage_0";             
            }
        }
        else
        {
            // 저장 불러오기: PlayerData 기반으로 복원
            LoadMapStateFrom(pd);
            pd.RefreshLastStatueFromList();
            if (!string.IsNullOrEmpty(pd.lastStatueId))
                MoveCharacterToStatue(pd.lastStatueId);
        }
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
                TeleportPlayerToStatue(currentHoveredGoddessStatue.statueID, currentHoveredGoddessStatue.currentMapNum);
                MapOpenSet(false);
                PlayerManager.instance.isState = false;
                Utils.OnOff(GameCanvas.instance.blessPanel, false);
                MoveCharacterToStatue(currentHoveredGoddessStatue.statueID);
                EnemyManager.instance.ActivateEnemies(currentHoveredGoddessStatue.currentMapNum);
            }
        }
    }
    #region 여신상 등록
    public void RegisterStatue(string id, string nameKor, string nameEng, int mapNum, int index)
    {
        if (!registeredStatues.Contains(id))
        {
            registeredStatues.Add(id);
            AddMap(id, nameKor, nameEng, MapType.GoddessStatue, mapNum, index);
            OnEnterNewMap(id);
        }
    }

    public bool IsStatueRegistered(string id) => registeredStatues.Contains(id);

    public List<string> GetRegisteredStatues() => registeredStatues.ToList();

    public bool IsStatueDiscovered(string id)
    {
        return registeredStatues.Contains(id);
    }
    #endregion

    #region 맵 Ui 켜졌다가 꺼졌다가 하는 기능
    public void MapOpenSet(bool open)
    {
        isMapOpen = open;
        //지도 맵 UI를 켜거나 끄는 기능
        Utils.OnOff(MapUi, isMapOpen);
        //맵 UI 켜졌을 때 화살표들을 깜빡이게 하는 코류틴 시작
        if (isMapOpen)
        {
            if (blinkRoutine == null)
                blinkRoutine = StartCoroutine(BlinkArrows());

            //커서 초기화 코류틴 실행 (커서를 중앙 위치 초기 위치로 옮기고 스크롤 뷰 위치도 갱신)
            StartCoroutine(InitCursorAfterLayout());
        }
        else //맵 UI 닫기
        {
            //깜빡임 코류틴 중지 및 화살표 알파값 원본으로 되돌림(1,1,1,1)
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
    //깜빡임을 중단하고 모든 화살표들의 투명도를 다시 불투명으로 되돌립니다.
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
    public void TeleportPlayerToStatue(string statueID, int num)
    {
        // "GoddesSatute_0" -> 숫자만 추출 (뒤에 인덱스)
        string indexStr = new string(statueID.Where(char.IsDigit).ToArray());

        //추출한 문자열을 숫자로 변환 성공하면 진행
        if (int.TryParse(indexStr, out int index))
        {
            if (index >= 0 && index < goddlesStatuePosition.Count)
            {
                Vector2 targetPosition = goddlesStatuePosition[index];

                Player.instance.transform.position = targetPosition;

                CM.instance.SetMap(num, snapToTarget: true);

                PM.playerData.SetPosition(Player.instance.transform.position);
                // 캐릭터 마커 이동 + 마지막 여신상 ID 갱신
                MoveCharacterToStatue(statueID);
                // 적 활성화
                EnemyManager.instance.ActivateEnemies(num);
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

    [Header("맵 Arrow 이미지의 관한 변수")]
    public List<ArrowMapIcon> mapArrowObjectList = new List<ArrowMapIcon>();

    [SerializeField] GameObject ArrowParent;

    public bool isMapOpen = false;

    [SerializeField] GameObject MapUi;
    [SerializeField] Image[] imgArrow;

    [SerializeField] float blinkSpeed = 1.5f; // 깜빡임 주기 (초당 몇 번)

    private Coroutine blinkRoutine;

    public void AddMap(string id, string nameKor,string nameEng, MapType type, int currentMapNum, int iconIndex)
    {
        if (allMaps.ContainsKey(id)) return;

        // MapData 등록
        allMaps[id] = new MapData(id, nameKor,nameEng, type);

        // 아이콘 찾아서 설정 및 활성화
        GoddessStatuesMapIcon icon = mapIcons[iconIndex];

        if (icon != null)
        {
            icon.Setup(id, nameKor,nameEng, type, currentMapNum); // 아이콘에 ID, 이름 설정
            bool isVisited = allMaps.TryGetValue(id, out var md) && md.isVisited;
            bool isRegistered = registeredStatues.Contains(id);

            // 방문이거나(밝힘) 여신상 등록이 된 경우 표시
            Utils.OnOff(icon.gameObject, isVisited || isRegistered);

            if (!mapIcons.Contains(icon)) mapIcons.Add(icon);
        }

        // 맵과 관련된 화살표 아이콘 켜주기
        // ArrowMapIcon 중 statueID가 같은 오브젝트 찾아 켜주기
        var matchingArrow = mapArrowObjectList.FirstOrDefault(arrow => arrow != null && arrow.statueID == id);
        if (matchingArrow != null)
        {
            Utils.OnOff(matchingArrow.gameObject, true);
        }
    }

    public void OnEnterNewMap(string mapID)
    {
        if (allMaps.ContainsKey(mapID))
        {
            var md = allMaps[mapID];
            md.isVisited = true;

            var pd = PlayerManager.instance?.player;
            if (pd != null)
            {
                UpsertVisit(pd.visitedMapsList, mapID, md.mapNameKor, md.mapNameEng, md.type, true);

                pd.lastStatueId = mapID; // “마지막 방문” 갱신 규칙
            }

            var icon = mapIcons.FirstOrDefault(x => x != null && x.statueID == mapID);
            if (icon != null)
                Utils.OnOff(icon.gameObject, true);
        }
    }
    #region 맵 Ui 커서 이동
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
        //전체 스크롤 콘텐츠의 가로/세로 크기를 가져옵니다.
        float contentWidth = contentTransform.rect.width;
        float contentHeight = contentTransform.rect.height;

        //커서의 절반 크기를 계산해서 커서 중심이 콘텐츠 경계를 벗어나지 않게 합니다.
        float cursorHalfW = cursorTransform.sizeDelta.x * 0.5f;
        float cursorHalfH = cursorTransform.sizeDelta.y * 0.5f;

        //RectTransform에서 Y축은 위쪽이 양수, 아래가 음수이므로 minY와 maxY는 음수 방향으로 계산됩니다.
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

        //커서가 뷰포트 중앙에 위치하도록 targetX 계산
        float viewWidth = scrollRect.viewport.rect.width;
        float contentWidth = contentTransform.rect.width;

        float targetX = currentCursorPos.x - (viewWidth * 0.5f);

        //croll 가능한 범위 내로 targetX 제한
        float minScrollX = 0f;
        float maxScrollX = contentWidth - viewWidth;

        targetX = Mathf.Clamp(targetX, minScrollX, maxScrollX);

        // 콘텐츠를 좌우로 스크롤합니다. (음수인 이유: 콘텐츠가 왼쪽으로 움직여야 커서가 보이게 되므로)
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

            //커서가 해당 아이콘 위에 있는지 검사
            bool isOver = IsCursorOver(iconRect);
           
            // mapData 가져오기
            allMaps.TryGetValue(icon.statueID, out var mapData);
            //해당 아이콘이 여신상인지 확인
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
    //커서가 특정 아이콘(RectTransform) 위에 있는지 판단하는 충돌 체크 함수
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
        //두 사각형이 겹치는지를 체크해서 true/false 반환
        return cursorRect.Overlaps(targetRect);
    }
    #endregion

    //캐릭터 UI 위치 이동
    public void MoveCharacterToStatue(string statueID)
    {
        //statueID에 해당하는 아이콘을 찾아서 캐릭터 UI를 해당 위치로 옮김
        var icon = mapIcons.FirstOrDefault(x => x.statueID == statueID);
        if (icon != null)
        {
            charcterUiTransform.localPosition = icon.iconTransform.localPosition;
            currentCharacterStatueId = statueID;
            PM.playerData.lastStatueId = statueID;
        }
    }

    #endregion

    #region 저장

    public void SaveMapStateTo(PlayerData pd)
    {
        if (pd == null) return;

        // 1) allMaps 상태를 리스트에 반영 (기존 엔트리는 유지하면서 값만 동기화)
        foreach (var kv in allMaps)
        {
            var id = kv.Key;
            var md = kv.Value;
            bool visited = md != null && md.isVisited;
            string kor = md?.mapNameKor ?? id;
            string eng = md?.mapNameEng ?? id;
            MapType type = md?.type ?? MapType.Unknown;

            UpsertVisit(pd.visitedMapsList, id, kor, eng, type, visited);
        }


        // 2) 마지막 방문 갱신 규칙: visited == true 중 마지막 인덱스
        pd.RefreshLastStatueFromList();

        // 3) MoveCharacterToStatue로 마커와 동기화(선택)
        if (!string.IsNullOrEmpty(pd.lastStatueId))
            MoveCharacterToStatue(pd.lastStatueId);
    }

    #endregion

    #region 불러오기

    public void LoadMapStateFrom(PlayerData pd)
    {
        if (pd == null) return;

        // 0) 아이콘 수집 후 allMaps에 최소 엔트리 보장 (누락 방지)
        foreach (var icon in mapIcons)
        {
            if (icon == null || string.IsNullOrEmpty(icon.statueID)) continue;
            if (!allMaps.ContainsKey(icon.statueID))
                allMaps[icon.statueID] = new MapData(icon.statueID, icon.korStr, icon.engStr, icon.mapType);
        }

        // 1) 리스트 → allMaps.isVisited 주입
        foreach (var v in pd.visitedMapsList)
        {
            if (string.IsNullOrEmpty(v.mapID)) continue;
            if (allMaps.TryGetValue(v.mapID, out var md))
                md.isVisited = v.isVisited;
        }

        // 2) 아이콘/화살표 가시성 갱신 (네 코드 그대로 사용)
        foreach (var icon in mapIcons)
        {
            if (icon == null) continue;

            bool visited = allMaps.TryGetValue(icon.statueID, out var md) && md.isVisited;
            bool registered = registeredStatues.Contains(icon.statueID);

            bool shouldOn;
            if (allMaps.TryGetValue(icon.statueID, out var md2))
            {
                // 여신상은 등록 OR 방문이면 켜기
                shouldOn = (md2.type == MapType.GoddessStatue) ? (registered || visited) : visited;
            }
            else
            {
                shouldOn = registered || visited;
            }
            Utils.OnOff(icon.gameObject, shouldOn);

            var arrow = mapArrowObjectList.FirstOrDefault(a => a && a.statueID == icon.statueID);
            if (arrow != null) Utils.OnOff(arrow.gameObject, shouldOn);
        }

        // 마지막 방문 보정
        pd.RefreshLastStatueFromList();
        if (!string.IsNullOrEmpty(pd.lastStatueId))
            MoveCharacterToStatue(pd.lastStatueId);
    }

    #endregion

    // 업서트 헬퍼: 리스트 순서를 유지(마지막 True = 최근 방문 규칙)
    static void UpsertVisit(List<VisitFlag> list, string id, string kor, string eng, MapType type, bool visited)
    {
        if (string.IsNullOrEmpty(id)) return;
        var v = list.Find(x => x.mapID == id);
        if (v == null)
        {
            list.Add(new VisitFlag
            {
                mapID = id,
                mapNameKor = kor,
                mapNameEng = eng,
                type = type,
                isVisited = visited
            });
        }
        else
        {
            v.mapNameKor = kor;
            v.mapNameEng = eng;
            v.type = type;
            v.isVisited = visited;
        }
    }
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