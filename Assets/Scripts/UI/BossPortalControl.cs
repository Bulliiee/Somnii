using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 보스포탈 처리
public class BossPortalControl : MonoBehaviour
{
    private GameObject bound;
    private GameObject wall;
    private GameObject portalAnimationObj;
    private GameObject portalImageObj;

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
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        //transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        Vector3 portalVector = new Vector3(player.transform.position.x + 3f, player.transform.position.y + 2.6f);
        transform.position = portalVector;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivePortal()
    {
        Debug.Log("Boss Portal Touch!!");

        PortalControl.minimapCheckFlag = true;
        ClearCheck.isClear = true;
        ControllerScript.isClear = true;
        CameraController.isClear = true;
        ClearCheck.bossRoomCheck = true;

        // 바운드, 벽 오브젝트 참조
        bound = GameObject.Find("BossBound");
        wall = GameObject.Find("Wall");
        ClearCheck.boundName = "BossBound";

        // 포탈 누르면 넘어갈 바운드의 Enemies오브젝트 활성화
        bound.transform.Find("BossEnemies").gameObject.SetActive(true);

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
            GameObject.Find("UIBtn").transform.Find("AttackBtn").gameObject.SetActive(false);
            GameObject.Find("UIBtn").transform.Find("CoralActionBtn").gameObject.SetActive(true);
            UiEvent.portalCheck = true;
        }    
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(CharacterSwitch.CharCheck && other.gameObject.tag == "Player") {
            GameObject.Find("UIBtn").transform.Find("AttackBtn").gameObject.SetActive(true);
            GameObject.Find("UIBtn").transform.Find("CoralActionBtn").gameObject.SetActive(false);
        }
    }
}
