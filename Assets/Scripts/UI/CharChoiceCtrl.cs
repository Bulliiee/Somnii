using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;  // 씬매니저 사용시 선언

// 캐릭터 선택창, CharacterSwitch.cs스크립트로 이름 전달
public class CharChoiceCtrl : MonoBehaviour
{
    public static string[] charName = new string[3];    // CharacterSwitch.cs에서 참조
    
    public GameObject charStatPanel;   // 캐릭터 클릭시 나타나는 스텟 팝업창
    public GameObject skillStatPanel;   // 스킬박스 클릭시 나타나는 스킬 설명 팝업창

    private Vector3 originPosition; // 원래 이미지 좌표, 드래그 끝나면 돌아가기 위함
    private Vector3 touchInput; // 터치한 곳(손가락 위치 따라감) 좌표
    private GameObject CharacterSprite; // 선택된 캐릭터 스프라이트
    private GameObject[] CharBox = new GameObject[3];   // UI의 캐릭터박스
    private GameObject[] SkillBox = new GameObject[3];  // UI의 스킬박스
    

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 3; i++) {
            CharBox[i] = GameObject.Find("CharBox" + i);
            SkillBox[i] = GameObject.Find("SkillBox" + i);
            charName[i] = null;
        }
    }

    // 이미지 누를 때, 전역변수 CharacterSprite체크해서 스탯 동적으로 변환할것!!!
    public void CharImgPointerDown(GameObject charSpr)
    {
        originPosition = charSpr.transform.position;
        Debug.Log("Down");
    }
    // 이미지 드래그할 때
    public void CharImgOnDrag(GameObject charSpr)
    {
        CharacterSprite = charSpr;  // 전역변수에 담기
        charSpr.GetComponent<SpriteRenderer>().enabled = true;
        touchInput = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        charSpr.transform.position = new Vector3(touchInput.x, touchInput.y, charSpr.transform.position.z);
        Debug.Log("OnDrag");
    }
    // 이미지 드래그 끝날 때
    public void CharImgEndDrag(GameObject charSpr)
    {
        charSpr.GetComponent<SpriteRenderer>().enabled = false;
        charSpr.transform.position = originPosition;
        Debug.Log("EndDrag");
        CharacterSprite = null; // 선택한것 초기화
    }
    // 이미지 클릭할 떄
    public void CharImgClick(GameObject charSpr)    // 캐릭터 클릭시 스텟창보여줌
    {
        charStatPanel.SetActive(true);
        Debug.Log("Character Img Click");
    }
    public void CharacterBoxClick(int index)    // 캐릭터박스 클릭시 캐릭터의 스킬 선택가능
    {
        if(CharBox[index].transform.childCount < 1) {   // 아무것도 없는 박스 클릭하면 그냥 리턴
            return;
        }

        for(int i = 0; i < 3; i++) {    // 박스 누를 때마다 스킬박스 안에 바꾸기 위함
            if(SkillBox[i].transform.childCount > 0 && SkillBox[i].transform.GetChild(0).name != "LockSlot") {
                Destroy(SkillBox[i].transform.GetChild(0).gameObject);
            }
        }

        switch(charName[index]) {   // 스킬박스 이미지 생성위함, 나중에 함수로 묶을것!!
            case "Kinies":
                GameObject Kiskill0 = Instantiate(GameObject.Find("KiniesSkill0"));

                Kiskill0.name = "KiniesSkill0";
                Kiskill0.transform.parent = SkillBox[0].transform;
                Kiskill0.transform.localPosition = new Vector3(0f, 0f);
                Debug.Log("Kinies CharBox Click");
                break;
            case "Nasci":
                Debug.Log("Nasci CharBox Click");
                break;
            case "Mechane":
                Debug.Log("Mechane CharBox Click");
                break;
            case "Poier":
                Debug.Log("Poier CharBox Click");
                break;
            case "Magus":
                Debug.Log("Magus CharBox Click");
                break;
            case "Lufu":
                Debug.Log("Lufu CharBox Click");
                break;
            case "Sacrum":
                Debug.Log("Sacrum CharBox Click");
                break;
            case "SPlayer":
                GameObject Spskill0 = Instantiate(GameObject.Find("SPlayerSkill0"));

                Spskill0.name = "SPlayerSkill0";
                Spskill0.transform.parent = SkillBox[0].transform;
                Spskill0.transform.localPosition = new Vector3(0f, 0f);
                Debug.Log("SPlayer CharBox Click");
                break;
            case "DPlayer":
                GameObject Dpskill0 = Instantiate(GameObject.Find("DPlayerSkill0"));

                Dpskill0.name = "DPlayerSkill0";
                Dpskill0.transform.parent = SkillBox[0].transform;
                Dpskill0.transform.localPosition = new Vector3(0f, 0f);
                Debug.Log("DPlayer CharBox Click");
                break;
            default:
                Debug.Log("CharBox Child Error!!");
                break;
        }
    }
    public void SkillBoxImgClick(int index) // 스킬박스 클릭시 스킬 스텟창 보여줌
    {
        if(SkillBox[index].transform.childCount < 1) {  // 아무것도 없는 박스 클릭하면 그냥 리턴
            return;
        }
        if(SkillBox[index].transform.GetChild(0).gameObject.name.Equals("LockSlot")) {  // LockSlot이면 그냥 리턴
            return;
        }

        skillStatPanel.SetActive(true);
        skillStatPanel.transform.GetChild(0).GetComponent<Text>().text = SkillBox[index].transform.GetChild(0).name + " 의 " + index + "번 스킬 설명";
        Debug.Log("Skill Box Click");
    }

    // 이미지 드랍
    public void CharBoxDrop(int index) 
    {  
        if(CharacterSprite == null) {   // 선택한거 없는데 드랍했으면 그냥 리턴
            return;
        }

        for(int i = 0; i < 3; i++) {    // 각 캐릭터박스 체크
            if(CharBox[i].transform.childCount > 0) {   // 캐릭터박스에 들어있는 박스에서
                if(CharBox[i].transform.GetChild(0).gameObject.name == CharacterSprite.name + ("(Clone)")) {    
                    Destroy(CharBox[i].transform.GetChild(0).gameObject);   // 들어있는애랑 선택한 애 이름이 같으면 삭제
                    charName[i] = null;
                }
            }
        }

        GameObject _sprite = Instantiate(CharacterSprite);
        _sprite.transform.parent = CharBox[index].transform;
        _sprite.transform.position = CharBox[index].transform.position;
        _sprite.transform.localScale = new Vector3(108f, 108f, 108f);
        _sprite.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 255f);

        charName[index] = CharacterSprite.name;

        if(CharBox[index].transform.childCount > 1) {   // 한 캐릭터박스에 2개이상이면 먼저있던거 삭제
            Destroy(CharBox[index].transform.GetChild(0).gameObject);
        }

        Debug.Log(CharacterSprite.name + " Drop");
    }

    // 시작버튼
    public void GoToStage()
    {
        // 3명중 한 명이라도 안고르면
        if(charName[0] == null || charName[1] == null || charName[2] == null) {
            GameObject.Find("TopText").GetComponent<Text>().text = "캐릭터를 세 명 모두 선택해야합니다.";
            Invoke("InvokeText", 1.5f);
            return;
        }

        SceneManager.LoadScene("FirstStage");
    }
    // 텍스트 변경
    void InvokeText()
    {
        GameObject.Find("TopText").GetComponent<Text>().text = "캐릭터를 드래그&드랍으로 선택하세요";
    }

    // 스탯패널 off버튼
    public void PanelOFF(GameObject panel)
    {
        panel.SetActive(false);
    }
}
