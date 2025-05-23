﻿using System.Collections;
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

        AddMap("Vilage_0", "마을", "Vilage", MapType.Village, 1,0);
        //AddMap("GoddesSatute_0", "마을여신상", "VilageGoddessSatute", MapType.GoddessStatue);
        MoveCharacterToStatue("Vilage_0");

        for(int i = 0; i < ArrowParent.transform.childCount; i++)
        {
            mapArrowObjectList.Add(ArrowParent.transform.GetChild(i).GetComponent<ArrowMapIcon>());
        }

        foreach(var arrow in mapArrowObjectList)
        {
            Utils.OnOff(arrow.gameObject, false);
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

                CM.instance.SnapToTarget(num);
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
            Utils.OnOff(icon.gameObject, true); // 아이콘 켜주기
            //리스트에 없으면 추가
            if (!mapIcons.Contains(icon))
            {
                mapIcons.Add(icon);
            }
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
            allMaps[mapID].isVisited = true;
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