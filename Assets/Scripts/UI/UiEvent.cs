using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // 씬매니저 사용시 선언

// 각종 버튼UI 이벤트들
public class UiEvent : MonoBehaviour
{
    public GameObject Book;   // 일시정지시 나타날 책, UI 오브젝트
    public GameObject UIBtn;    // UI버튼 캔버스
    public GameObject PauseBtn; // 평상시 pause
    public GameObject ActPauseBtn;   // pause 활성화시
    public GameObject QuitApp;  // QuitBtn 누를시 나타나는 패널

    public static bool portalCheck = false; // true: 보스룸 포탈 클릭, false: 일반 룸 포탈 클릭
                                            // 포탈컨트롤 스크립트들에서 참조
                                            
    //private bool pauseOn = false;   // true: 일시정지중, false: 아님

    // touch to start
    public void ChangeLobbyScene()
    {
        SceneManager.LoadScene("CharacterChoice");
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
            Time.timeScale = 0; // 시간흐름 비율 0으로(정지)
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
            Time.timeScale = 1.0f;  // 시간흐름 비율 원래대로
            //pauseOn = false;    // 일시정지 해제
        //}
    }

    public void QuitBtn() 
    {
        // ActivePauseBtn의 ResumeBtn, QuitBtn 비활성화
        GameObject.Find("ResumeBtn").SetActive(false);
        GameObject.Find("QuitBtn").SetActive(false);

        QuitApp.SetActive(true);
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

    // void OnApplicationQuit() 
    // {
    //     Application.CancelQuit();
    //     #if !UNITY_EDITOR

    //     System.Diagnostics.Process.GetCurrentProcess().Kill();
    //     #endif
    // }

}
