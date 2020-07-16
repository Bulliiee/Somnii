using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 일반 포탈 컨트롤
public class PortalControl : MonoBehaviour
{
    private GameObject bound;
    private GameObject wall;
    private GameObject portalAnimationObj;
    private GameObject portalImageObj;
    public static bool minimapCheckFlag = false;    // SpawnMapChecker.cs, BossPortalControl.cs에서 참조

    void Awake()
    {
        portalAnimationObj = gameObject.transform.GetChild(0).gameObject;
        portalImageObj = gameObject.transform.GetChild(1).gameObject;
        Invoke("PortalImageSetting", 1.25f);
        
    }

    void PortalImageSetting()
    {
        portalImageObj.SetActive(true);
        Destroy(portalAnimationObj);
    }

    // Start is called before the first frame update
    void Start()
    {
        // 플레이어 기준의 위치로 이동
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 portalVector = new Vector3(player.transform.position.x, player.transform.position.y + 3.5f);
        transform.position = portalVector;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivePortal()
    {
        Debug.Log("Portal Touch!!");

        minimapCheckFlag = true;
        ClearCheck.isClear = true;
        ControllerScript.isClear = true;
        CameraController.isClear = true;

        // 바운드, 벽 오브젝트 참조
        bound = GameObject.Find(ClearCheck.boundName);
        wall = GameObject.Find("Wall");

        // 포탈 누르면 넘어갈 바운드의 Enemies오브젝트 활성화
        string enemyName = "Enemies" + bound.name.Substring(5);
        bound.transform.Find(enemyName).gameObject.SetActive(true);
        
        // 벽들을 활성화된 바운드 위치로 이동
        wall.transform.position = new Vector3(bound.transform.position.x, bound.transform.position.y, wall.transform.position.z);

        // 클릭 후처리
        //Invoke("AfterClick", 1f);
    }

    // void AfterClick()
    // {
    //     // 포탈 Active 후 Destroy
    //     GameObject Portal = GameObject.FindGameObjectWithTag("Portal");
    //     Destroy(Portal);
    // }

    private void OnTriggerEnter2D(Collider2D other) 
    {   
        if(CharacterSwitch.CharCheck && other.gameObject.tag == "Player") {
            GameObject.Find("UIBtn").transform.Find("FAttackBtn").gameObject.SetActive(false);
            GameObject.Find("UIBtn").transform.Find("SAttackBtn").gameObject.SetActive(false);
            GameObject.Find("UIBtn").transform.Find("TAttackBtn").gameObject.SetActive(false);
            GameObject.Find("UIBtn").transform.Find("AttackBtnImage").gameObject.SetActive(false);
            GameObject.Find("UIBtn").transform.Find("CoralActionBtn").gameObject.SetActive(true);
            UiEvent.portalCheck = false;
        }    
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(CharacterSwitch.CharCheck && other.gameObject.tag == "Player") {
            GameObject.Find("UIBtn").transform.Find("FAttackBtn").gameObject.SetActive(true);
            GameObject.Find("UIBtn").transform.Find("SAttackBtn").gameObject.SetActive(false);
            GameObject.Find("UIBtn").transform.Find("TAttackBtn").gameObject.SetActive(false);
            GameObject.Find("UIBtn").transform.Find("AttackBtnImage").gameObject.SetActive(true);
            GameObject.Find("UIBtn").transform.Find("CoralActionBtn").gameObject.SetActive(false);
        }
    }

}
