using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// 캐릭터들의 공통 움직임 컨트롤
public class ControllerScript : MonoBehaviour
{
    public JoyStickSetting joystick;   // JoyStick 스크립트
    public float HP = 100.0f;   // 체력(inspector에서 개별 조정 필요, CharacterSwitch.cs에서 체력바 조절)
    public float MoveSpeed = 4f;

    private Vector3 _moveVector;    // 플레이어 이동벡터
    private Transform _transform;   // 플레이어 트랜스폼
    private SpriteRenderer charRenderer;    // 캐릭터의 스프라이트 렌더러 가져옴
    private Animator animator;  // 애니메이터 가져오기

    public static bool isClear = false; // Portal 스크립트에서 참조
    public static bool hitCheck = false;    // 맞는 모션동안(ture)은 무적, 맞는 모션 끝나면 false
                                            // enemy들의 각 스크립트에서 참조

    void OnEnable()
    {
        // 초기화
        _transform = transform; // 트랜스폼 캐싱
        _moveVector = Vector3.zero; // 플레이어 이동벡터 초기화
        charRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        // 카메라 컨트롤러 스크립트 초기화(플레이어 다시 연결)
        CameraController cameraScript = GameObject.Find("Main Camera").GetComponent<CameraController>();   
        joystick = GameObject.Find("JoyStickPanel").GetComponent<JoyStickSetting>();
        cameraScript.Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        // _transform = transform; // 트랜스폼 캐싱
        // _moveVector = Vector3.zero; // 플레이어 이동벡터 초기화
        // charRenderer = GetComponent<SpriteRenderer>();
        // animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // 터치패드 입력받기
        HandleInput();

        if(isClear) {
            SetPosition();
        }
    }

    void FixedUpdate()
    {
        // 플레이어 이동
        Move();
    }

    public void HandleInput()
    {
        _moveVector = poolInput();
    }

    public Vector3 poolInput()
    {
        float h = joystick.GetHorizontalValue();
        float v = joystick.GetVerticalValue();    // y축 사용시 활성화
        Vector3 moveDir = new Vector3(h, v, 0).normalized;  // y축사용시 인자(h, v, 0)

        return moveDir;
    }

    public void Move()
    {
        
        if(_moveVector.x < 0) { // 이동하는 x축벡터값 음수면
            if(!WeponControl.nowAttack) {   // 공격중이 아니라면
                charRenderer.flipX = true;  // 스프라이트 플립
            }
            // Player의 애니메이션 parameter 불값 변경
            animator.SetBool("isWalking", true);
        }
        else if(_moveVector.x > 0) {    // 양수면
            if(!WeponControl.nowAttack) {   // 공격중이 아니라면
                charRenderer.flipX = false; // 원래대로
            }
            animator.SetBool("isWalking", true);
        }
        else if(_moveVector.x == 0) {   // 안움직이면
            animator.SetBool("isWalking", false);
        }
        
        // 캐릭터 벡터값, 움직이는 힘 줘서 움직이게 하기
        _transform.Translate(_moveVector * MoveSpeed * Time.deltaTime);
    }

    // 맞았을 때 애니메이션 이벤트, 체력 처리
    public void OnHit()
    {
        hitCheck = true;
        animator.SetBool("isHit", true);
    }

    public void EndHit()
    {
        animator.SetBool("isHit", false);
        hitCheck = false;
    }

    // 룸 클리어시 알맞은 바운드 위치로 캐릭터 포지션 이동
    void SetPosition()
    {
        isClear = false;

        GameObject bound = GameObject.Find(ClearCheck.boundName);

        // 바운드에 맞는 포지션으로 캐릭터 이동
        this.transform.position = new Vector3(bound.transform.position.x, bound.transform.position.y, this.transform.position.z);
    }
}
