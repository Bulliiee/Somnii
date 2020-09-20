using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  // 씬매니저 사용시 선언

// 각종 버튼UI 이벤트들
public class UiEvent : MonoBehaviour
{
    public GameObject Book;                     // 일시정지시 나타날 책, UI 오브젝트
    public GameObject UIBtn;                    // UI버튼 캔버스
    public GameObject PauseBtn;                 // 평상시 pause
    public GameObject ActPauseBtn;              // pause 활성화시
    public GameObject QuitApp;                  // QuitBtn 누를시 나타나는 패널

    public JoyStickSetting joystick;            // 조이스틱 입력받은거 받아오기 위한 스크립트
    public GameObject character;                // 현재 캐릭터 게임오브젝트
    private Vector3 _moveVector;                // 조이스틱으로 입력받은 벡터값 저장 위함

    public static bool portalCheck = false;     // true: 보스룸 포탈 클릭, false: 일반 룸 포탈 클릭
                                                // 포탈컨트롤 스크립트들에서 참조

                                            
    //private bool pauseOn = false;   // true: 일시정지중, false: 아님

    void Start()
    {
        // 0.667
        if(SceneManager.GetActiveScene().name == "Start") { // 스타트씬일때
            Invoke("InvokeStartScene", 0.667f);
        }
        joystick = GameObject.Find("JoyStickPanel").GetComponent<JoyStickSetting>();
        _moveVector = Vector3.zero;
    }

    void InvokeStartScene()
    {
        GameObject.Find("TouchToStart").GetComponent<Button>().enabled = true;
    }

    void Update()
    {
        
    }

    // touch to start
    public void ChangeLobbyScene()
    {
        SceneManager.LoadScene("CharChoice");
    }

    // pause, resume 버튼
    public void ActivePauseBtn()
    {
        //if (pauseOn == false) { // 일시정지 아닐 때 누르면
            Book.SetActive(true); 
            UIBtn.SetActive(false);
            PauseBtn.SetActive(false);
            ActPauseBtn.SetActive(true);
            // 1은 1배속, 0.5는 0.5배속
            //Time.timeScale = 0; // 시간흐름 비율 0으로(정지)
            //pauseOn = true; // 일시정지다
        //}
    }

    // BackGround 터치시에도 Resume될 수 있게 동작(예정)
    public void ResumeBtn()
    {
        //if(pauseOn == true) {  // 일시정지일 때
            Book.SetActive(false);    
            UIBtn.SetActive(true);
            PauseBtn.SetActive(true);
            ActPauseBtn.SetActive(false);
            character = GameObject.FindGameObjectWithTag("Player");
            character.GetComponent<AttackControl>().AttackBtnInit();
            //Time.timeScale = 1.0f;  // 시간흐름 비율 원래대로
            //pauseOn = false;    // 일시정지 해제
        //}
    }

    public void QuitBtn() 
    {
        // ActivePauseBtn의 ResumeBtn, QuitBtn 비활성화
        GameObject.Find("ResumeBtn").SetActive(false);
        GameObject.Find("QuitBtn").SetActive(false);

        QuitApp.SetActive(true);
        Time.timeScale = 0; // 시간흐름 비율 0으로(정지)
    }

    public void QuitApp_Yes() 
    {
        Debug.Log("Application Quit!!!!");
        Application.Quit();
    }

    public void QuitApp_No() {
        // ActivePauseBtn의 ResumeBtn, QuitBtn 활성화
        GameObject.Find("ActivePauseBtn").transform.Find("ResumeBtn").gameObject.SetActive(true);
        GameObject.Find("ActivePauseBtn").transform.Find("QuitBtn").gameObject.SetActive(true);
        QuitApp.SetActive(false);
        Time.timeScale = 1.0f; // 시간흐름 비율 원래대로
    }

    public void PortalActive()
    {
        if(!portalCheck) {    // 일반포탈에 들어갈 때
            GameObject.Find("PortalCanvas(Clone)").GetComponent<PortalControl>().ActivePortal();
        }
        else {  // 보스포탈에 들어갈 때
            GameObject.Find("BossPortalCanvas(Clone)").GetComponent<BossPortalControl>().ActivePortal();
        }
    }

    // 공격버튼
    public void FAttackButtonActive()
    {
        if(ControllerScript.hitCheck) { // 맞는모션중이면 공격x
            return;
        }
        character = GameObject.FindGameObjectWithTag("Player");
        ControllerScript.isAttack = true;
        character.GetComponent<Animator>().Play("FAttack");
        GameObject.Find("UIBtn").transform.Find("FAttackBtn").gameObject.SetActive(false);
    }
    public void SAttackButtonActive()
    {
        character = GameObject.FindGameObjectWithTag("Player");
        character.GetComponent<ControllerScript>().EndHit();
        ControllerScript.isAttack = true;
        character.GetComponent<Animator>().SetBool("FtoS", true);
        //character.GetComponent<Animator>().Play("SAttack");
        GameObject.Find("UIBtn").transform.Find("SAttackBtn").gameObject.SetActive(false);
    }
    public void TAttackButtonActive()
    {
        character = GameObject.FindGameObjectWithTag("Player");
        character.GetComponent<ControllerScript>().EndHit();
        ControllerScript.isAttack = true;
        character.GetComponent<Animator>().SetBool("StoT", true);
        //character.GetComponent<Animator>().Play("TAttack");
        GameObject.Find("UIBtn").transform.Find("TAttackBtn").gameObject.SetActive(false);
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

    public void EvasionButtonActive()
    {
        character = GameObject.FindGameObjectWithTag("Player");
        character.GetComponent<ControllerScript>().isDash = true;
    }

    // void OnApplicationQuit() 
    // {
    //     Application.CancelQuit();
    //     #if !UNITY_EDITOR

    //     System.Diagnostics.Process.GetCurrentProcess().Kill();
    //     #endif
    // }

}
