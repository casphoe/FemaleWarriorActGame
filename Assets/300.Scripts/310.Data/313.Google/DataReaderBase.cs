using UnityEngine;

//스프레드 시트로 읽어올 여러 데이터에 기본이 되는 읽기 형식에 대해 정의한 추상 클래스
// 추상 클래스 : 직접 객체를 만들 수 없는 클래스 공통 기능과 구조를 정의하고 상속 받은 자식 클래스들이 실제 내용을 구현하도록 강제 가능
public class DataReaderBase : ScriptableObject
{
    //시트 주소를 연결
    // /d/ ~ /edit# 범위 내인 '~' 영역을 지정하면 됩니다
    [Header("시트의 주소")][SerializeField] public string associatedSheet = "";
    //시트의 워크시트를 연결
    [Header("스프레드 시트의 시트 이름")][SerializeField] public string associatedWorksheet = "";

    //워크시트에서 읽기 시작할 행 번호와 읽기를 마칠 끝 행 번호를 설정
    [Header("읽기 시작할 행 번호")][SerializeField] public int START_ROW_LENGTH = 2;
    [Header("읽을 마지막 행 번호")][SerializeField] public int END_ROW_LENGTH = -1;
}