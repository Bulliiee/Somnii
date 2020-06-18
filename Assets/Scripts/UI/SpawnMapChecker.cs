using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 추가할 것: 현재 있는 룸 보여주기!!
// 구현된 것: 미니맵 체커 위치에 맞게 생성
// Book UI에서 클리어 한 룸들을 보여주는 미니맵
public class SpawnMapChecker : MonoBehaviour
{
    public GameObject currentBoundChecker;  // 현재 바운드 체크 오브젝트(프리팹)
    public GameObject mapChecker;   // 미니맵 체크 오브젝트(프리팹)
    public GameObject miniMap;  // 미니맵 오브젝트(mapChecker들의 부모가 됨)
    private GameObject currentBoundTemp;    // 현재 바운드 오브젝트 임시저장소

    private Vector3 vector;
    private int firstNumber;
    private int secondNumber;    
    private string boundName;
    private bool checkFlag = false;
    private bool isBossRoom = false;   // 보스 룸으로 가면 true

    void Awake()
    {
        
    }

    void Start()
    {
        vector.z = -1f;
        boundName = ClearCheck.boundName;
        firstNumber = int.Parse(boundName.Substring(5, 1)); // 현재 바운드의 번호 찾기
        secondNumber = int.Parse(boundName.Substring(6, 1));
        checkFlag = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!PortalControl.minimapCheckFlag && checkFlag && !isBossRoom) {
            checkFlag = false;

            boundName = ClearCheck.boundName;

            if(boundName == "BossBound") {  // 보스룸이라면
                isBossRoom = true;
                return;
            }

            firstNumber = int.Parse(boundName.Substring(5, 1)); // 현재 바운드의 번호 찾기
            secondNumber = int.Parse(boundName.Substring(6, 1));
                
            PositionSetting();  // 포지션 세팅

            CheckerPositionMove(currentBoundChecker);
        }
        else if(PortalControl.minimapCheckFlag) {   // 포탈 터치시
            PortalControl.minimapCheckFlag = false;

            Destroy(currentBoundTemp);
            CheckerPositionMove(mapChecker);

            // 클리어된 바운드 제거
            GameObject currentBound = GameObject.Find(boundName);
            Destroy(currentBound, 1f);
        
            checkFlag = true;
        }
    }

    // 체커 생성 및 이동
    void CheckerPositionMove(GameObject checker)
    {
        // 체커 생성
        GameObject checkerTemp = Instantiate(checker);
        // 부모 오브젝트 맞추기
        checkerTemp.transform.parent = miniMap.transform;
        // 체커 위치 맞추기
        checkerTemp.transform.localPosition = vector;
        
        if(checkerTemp.name == "MiniMap_CurrentBoundCheck(Clone)") {
            currentBoundTemp = checkerTemp;
        }
    }

    // 바운드 넘버에 맞는 좌표 부여
    void PositionSetting()
    {
        switch(firstNumber) {
            case 0:
                vector.y = 0.938f;
                break;
            case 1:
                vector.y = 0.313f;
                break;
            case 2:
                vector.y = -0.313f;
                break;
            case 3:
                vector.y = -0.938f;
                break;
        }

        switch(secondNumber) {
            case 0:
                vector.x = -0.938f;
                break;
            case 1:
                vector.x = -0.313f;
                break;
            case 2:
                vector.x = 0.313f;
                break;
            case 3:
                vector.x = 0.938f;
                break;
        }
    }

}
