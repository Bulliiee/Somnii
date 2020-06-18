using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//바운드에 따른 카메라의 움직임 범위 및 룸 클리어시 카메라의 포지션 변경
public class CameraController : MonoBehaviour
{
    private GameObject player;  // 플레이어 게임오브젝트
    private GameObject gameObjectBound; // 바운드
    private BoxCollider2D bound;    // 바운드 콜라이더
    private Transform playerT;  // 플레이어 트랜스폼
    private Camera theCamera;
    private Vector3 minBound;
    private Vector3 maxBound;
    private Vector2 velocity; 

    private float halfWidth;
    private float halfHeight;
    private float smoothTimeX = 0.1f, smoothTimeY = 0.05f;  // 카메라 부드럽게 따라오게 하는 정도

    public static bool isClear = false; // Portal 스크립트에서 참조

    void Awake()
    {
        // player = GameObject.FindGameObjectWithTag("Player");
        gameObjectBound = GameObject.Find(ClearCheck.boundName);
        bound = gameObjectBound.GetComponent<BoxCollider2D>();
        // playerT = player.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        theCamera = GetComponent<Camera>();
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
        halfHeight = theCamera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;  // 해상도
        Invoke("Init", 0.1f);   // 캐릭터 생성 후(CharSwitch.cs) 플레이어 가져오기 위함
    }

    // 초기화 함수(캐릭터 연결)
    public void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerT = player.transform;
    }

    void Update()
    {
        if(isClear) {
            setBound();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float posX = Mathf.SmoothDamp(this.transform.position.x, playerT.position.x, ref velocity.x, smoothTimeX);
        float posY = Mathf.SmoothDamp(this.transform.position.y, playerT.position.y, ref velocity.y, smoothTimeY);

        this.transform.position = new Vector3(posX, posY, this.transform.position.z);

        float clampedX = Mathf.Clamp(this.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);  //Clamp('값', '최솟값', '최댓값') 값이 무조건 최솟값 최댓값 사이만 나옴
        float clampedY = Mathf.Clamp(this.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);

        this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);
    }

    // 룸 클리어시 알맞은 바운드 포지션으로 카메라 세팅
    void setBound()
    {
        isClear = false;

        gameObjectBound = GameObject.Find(ClearCheck.boundName);
        bound = gameObjectBound.GetComponent<BoxCollider2D>();

        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
        halfHeight = theCamera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;  // 

        // 카메라포지션 바운드에 맞게 변경
        this.transform.position = new Vector3(bound.transform.position.x, bound.transform.position.y, this.transform.position.z);
    }

}
