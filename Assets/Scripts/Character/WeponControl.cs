using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// 캐릭터에 따른 무기 움직임 및 근접공격 처리(원거리 공격은 따로 처리)
public class WeponControl : MonoBehaviour
{
    private SpriteRenderer handRenderer;
    private BoxCollider2D attackBound;
    private GameObject player;
    private SpriteRenderer playerRenderer;
    private Animator animator;
    public AttackBtn attackBtn; // AttackBtn 스트립트

    private Vector3 _moveVector;    // 이동벡터

    private bool updateCtrl = true;    // 업데이트 함수 컨트롤용, true일때만 Update()실행
    private bool flipTemp = false;  // flip상태 저장
    public static bool nowAttack = false; // true(attack중)는 캐릭터 플립 제한

    // Start is called before the first frame update
    void Start()
    {
        handRenderer = GetComponent<SpriteRenderer>();
        // player = GameObject.Find("Player");
        player = GameObject.FindGameObjectWithTag("Player");
        playerRenderer = player.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        attackBound = GetComponent<BoxCollider2D>();
        attackBtn = GameObject.Find("UIBtn").transform.Find("AttackBtn").gameObject.GetComponent<AttackBtn>();

        _moveVector = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();  // 터치 입력받음
        if(_moveVector != Vector3.zero && !nowAttack) {   // 입력받는 중이면
            nowAttack = true;
        }
        else if(_moveVector == Vector3.zero) {   // 입력받지 않는 중이면
            nowAttack = false;
        }

        Attack();

        if(playerRenderer.flipX != flipTemp) {
            updateCtrl = true;
            HandControl();
        }

        if(playerRenderer.flipX && updateCtrl) {
            flipTemp = true;
            updateCtrl = false;
        }
        else if(!playerRenderer.flipX && updateCtrl) {
            flipTemp = false;
            updateCtrl = false;
        }
    }

    // Animation Event에서 참조
    void AttackBoundOff()
    {
        attackBound.enabled = false;
    }
    void AttackBoundOn()
    {
        attackBound.enabled = true;   
    }
    
    // 입력받기
    public void HandleInput()
    {
        _moveVector = poolInput();
    }

    // 입력받은 좌표 받기
    public Vector3 poolInput()
    {
        float h = attackBtn.GetHorizontalValue();
        float v = attackBtn.GetVerticalValue();    // y축 사용시 활성화
        Vector3 moveDir = new Vector3(h, v, 0).normalized;  // y축사용시 인자(h, v, 0)

        return moveDir;
    }

    // 어택
    void Attack()
    {
        if(_moveVector.x > 0) { // 좌측 타격
            animator.SetBool("isAttack", true);
            //renderer.flipX = false;
            playerRenderer.flipX = false;
        }
        else if(_moveVector.x < 0) {    // 우측 타격
            animator.SetBool("isAttack", true);
            //renderer.flipX = true;
            playerRenderer.flipX = true;
        }
        else if(_moveVector.x == 0) {   // 공격x
            animator.SetBool("isAttack", false);
        }
    }

    // 움직일 때 손 위치 세팅
    void HandControl() 
    {
        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x * -1, gameObject.transform.localPosition.y);
        attackBound.offset = new Vector2(attackBound.offset.x * -1, attackBound.offset.y);

        if(playerRenderer.flipX) {
            handRenderer.flipX = true;
        }
        else if(!playerRenderer.flipX) {
            handRenderer.flipX = false;
        }
    }

    // 근접무기 적 때리기
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Enemy") {   // 공격 바운드가 적과 충돌시
            EnemyControl enemyCtrl = other.gameObject.GetComponent<EnemyControl>(); // 충돌한 오브젝트의 스크립트 받음
            enemyCtrl.HP -= 25f;
        }
    }
}
