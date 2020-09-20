using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//룸 클리어 체크
public class ClearCheck : MonoBehaviour
{
    private GameObject Room;
    private GameObject Bound;
    private GameObject Enemy;

    public GameObject portal;
    public GameObject bossPortal;
    public GameObject stageClearText;

    public static int[,] roomCheckArr = new int[4,4];       // 클리어 한 룸 체크용 2차원배열, 1이면 클리어한 룸, 0이면반대
    public static bool bossRoomCheck = false;               // 보스룸 입장시 true
    public static bool isClear = false;                     // PortalControl.cs에서 참조

    private int boundFirstNumber;                           // 현재 Bound의 첫 번째 번호
    private int boundSecondNumber;                          // 현재 Bound의 두 번째 번호
    private int stageCount = 0;                             // 클리어한 룸의 갯수, 16이 되면 스테이지 클리어
    private int lineCount = 0;                              // 완성된 라인 수 체크

    private bool updateControl = true;                      // Update() 컨트롤용 //  //  //  //
    private bool isAllRoomsClear = false;                   // 모든 룸을 클리어시 true

    // 첫 스타트 Room00(Bound00)에서 시작
    public static string boundName = "Bound00"; // CameraController.cs, ControllerScript.cs에서 참조
    private string enemiesName = "Enemies00";

    // Start is called before the first frame update
    void Start()
    {
        Room = GameObject.Find("Rooms");

        SetGameObjectName();

        for (int i = 0; i < 4; i++) {
            for(int j = 0; j < 4; j++) {
                roomCheckArr[i, j] = 0; // roomCheckArr 초기화
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 룸의 몬스터 다 잡으면(Enemies**오브젝트의 자식 개수 체크)
        //                                       // // // //
        if (Enemy.transform.childCount < 1 && updateControl && !isAllRoomsClear) {
            stageCount++;
            if (stageCount == 16) {   // 모든 룸이 클리어상태라면
                isAllRoomsClear = true;
                ActivePortal();
                return;
            }

            // roomCheckArr[]의 해당 바운드 번호를 1로 변경
            Debug.Log("Enemies" + boundFirstNumber + boundSecondNumber + "are Empty(All Enemies are destroyed)");
            roomCheckArr[boundFirstNumber, boundSecondNumber] = 1;  // 룸 체크

            // 완성된 라인 수 체크
            Debug.Log("StageLine Checking");
            StageLineCheck();

            // 포탈 생성
            Debug.Log("Bound" + boundFirstNumber + boundSecondNumber + " Portal Active");
            ActivePortal();

            // 클리어된 바운드를 제외한 새로운 바운드를 찾아 연결함
            Debug.Log("New Bound connecting...");
            GetRandomBoundNumber(); // 난수번호 생성
        }

        // 일반 포탈 클릭시 isClear값 true
        if (isClear) {
            isClear = false;
            updateControl = true;
        }

        // 보스룸 포탈 클릭시
        if (bossRoomCheck) {
            enemiesName = "BossEnemies";
            SetGameObjectName();
            bossRoomCheck = false;
            // 스크립트 교체
            gameObject.GetComponent<BossClearCheck>().enabled = true;
            gameObject.GetComponent<ClearCheck>().enabled = false;
        }
    }

    // 바운드 번호 난수생성하기
    void GetRandomBoundNumber()
    {
        updateControl = false;
        int firstNumber;
        int secondNumber;
        string tmp;

        while (true) {
            firstNumber = Random.Range(0, 4);   // 첫번째 난수번호 얻음 
            secondNumber = Random.Range(0, 4);  // 두번째 난수번호 얻음

            tmp = firstNumber.ToString() + secondNumber.ToString();

            if (roomCheckArr[firstNumber, secondNumber] == 0) { // roomCheckArr==1이면 다시 난수 생성
                break;  // roomCheckArr == 0이면 난수 생성 멈춤
            }
        }

        boundName = "Bound" + tmp;   // boundName 설정
        enemiesName = "Enemies" + tmp;   // enemiesName 설정

        // 다음 바운드 켜기
        Room.transform.Find(ClearCheck.boundName).gameObject.SetActive(true);

        SetGameObjectName();
    }

    // Bound와 Enemies 게임오브젝트를 boundName, enemiesName으로 스크립트의 Bound, Enemy에 이어주기
    void SetGameObjectName()
    {
        // Bound와 Enemy 연결
        Bound = GameObject.Find(boundName);
        Enemy = Bound.transform.Find(enemiesName).gameObject;

        // 다음 방이 보스룸이라면 밑의 구문은 실행할 필요 없음
        if (!bossRoomCheck) {
            boundFirstNumber = int.Parse(Bound.name.Substring(5, 1));  // 현재 Bound의 첫번째 번호 얻기
            boundSecondNumber = int.Parse(Bound.name.Substring(6, 1)); // 현재 Bound의 두번째 번호 얻기

            Debug.Log("BoundFirstNumber" + boundFirstNumber);
            Debug.Log("BoundSecondNumber" + boundSecondNumber);
        }

        updateControl = true;
    }

    // 룸 클리어시 해당 룸에 포탈 생성
    void ActivePortal()
    {
        // 일반 포탈 클릭시 현재 스크립트의 isClear=true, updateControl = true 됨
        updateControl = false;

        // 모든 방이 클리어된게 아니라면 밑의 구문 실행, 모든 방 클리어라면 보스포탈만 나오게 됨
        if (!isAllRoomsClear) {
            Instantiate(portal, Bound.transform);
        }

        // 완성된 라인 수가 1개 이상이면 보스포탈도 생성
        if(lineCount >= 1) {
            Instantiate(bossPortal, Bound.transform);
        }
    }

    // roomCheckArr[]의 값 체크
    public void CheckRoomArr()
    {
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 4; j++) {
                Debug.Log("[" + i + ", " + j + "] = " + roomCheckArr[i, j]);

            }
        }
    }

    // 스테이지의 모든 룸 클리어하는지 체크
    // 현재는 클리어한 룸의 갯수로 체크하지만 roomCheckArr[]의 값으로 체크해도 됨
    // bool StageCheck()
    // {
    //     stageCount++;

    //     // 클리어 한 룸의 수가 16개가 되면
    //     if (stageCount == 16) {
    //         return true;
    //     }
    //     else {
    //         return false;
    //     }

    //     //for(int i = 0; i < 4; i++) {
    //     //    for(int j = 0; j < 4; j++) {
    //     //        if(roomCheckArr[i, j] != 1) {
    //     //            isAllRoomClear = false;
    //     //            break;
    //     //        }
    //     //    }
    //     //    if(isAllRoomClear == false) {
    //     //        break;
    //     //    }
    //     //}
    // }

    // 완성된 라인 수 체크
    void StageLineCheck()
    {
        int roomCount = 0;  // 각 줄에서 클리어 한 룸 체크
        lineCount = 0;

        // 가로줄 체크
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 4; j++) {
                if (roomCheckArr[i, j] == 1) {
                    roomCount++;
                }
                if (roomCount == 4) {
                    lineCount++;
                }
                if (j == 3) {
                    roomCount = 0;
                }
            }
        }
        roomCount = 0;
        // 세로줄 체크
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 4; j++) {
                if (roomCheckArr[j, i] == 1) {
                    roomCount++;
                }
                if (roomCount == 4) {
                    lineCount++;
                }
                if (j == 3) {
                    roomCount = 0;
                }
            }
        }
        roomCount = 0;
        // 대각선 체크_1
        for (int i = 0; i < 4; i++) {
            if (roomCheckArr[i, i] == 1) {
                roomCount++;
            }
            if (roomCount == 4) {
                lineCount++;
            }
        }
        roomCount = 0;
        // 대각선 체크_2
        if (roomCheckArr[0, 3] == 1 && roomCheckArr[1, 2] == 1 &&
            roomCheckArr[2, 1] == 1 && roomCheckArr[3, 0] == 1) {
            lineCount++;
        }

        Debug.Log("lineCount: " + lineCount);
    }
}
