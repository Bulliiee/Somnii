using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  // 씬매니저 사용시 선언

// 캐릭터 선택창, CharacterSwitch.cs스크립트로 이름 전달, 지금은 안씀
public class CharChoice : MonoBehaviour
{
    public static string[] charName = new string[3];

    public Text currentChoiceCharText;
    private int index = 0;
    private bool setTextPrint = true;   // true: 텍스트수정, false: 텍스트 수정x

    // Start is called before the first frame update
    void Start()
    {
        index = 0;

        for(int i = 0; i < 3; i++) {
            charName[i] = "null";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(setTextPrint) {
            setTextPrint = false;
            PrintText();
        }
    }

    private void PrintText()
    {
        currentChoiceCharText.text = "index: " + index + "\n" +
                                    "firstChar: " + charName[0] + "\n" +
                                    "secondChar: " +charName[1] + "\n" +
                                    "thirdChar: " + charName[2];
    }

    // 캐릭터 중복선택시 선택취소
    private void OverlapCheck(string name)
    {
        for(int i = 0; i < 3; i++) {
            if(charName[i] == name) {
                charName[i] = "null";
                setTextPrint = true;
                return;
            }
        }
        CharSelect(name);
    }

    private void CharSelect(string name)
    {
        charName[index] = name;
        setTextPrint = true;
    }

    public void Setindex(int number)
    {
        index = number;
        setTextPrint = true;
    }

    public void Choice_Char(int index)
    {
        string nameTemp = "null";

        switch(index) { // 프리팹 이름 그대로 적으면 됨
            case 0:
                nameTemp = "DPlayer";
                break;
            case 1:
                nameTemp = "DPlayer2";
                break;
            case 2:
                nameTemp = "SPlayer";
                break;
            case 3:
                nameTemp = "Kinies";
                break;
            case 4:

                break;
            case 5:

                break;
            case 6:

                break;
            default:
                break;
        }
        
        OverlapCheck(nameTemp);
    }

    public void GoToStage()
    {
        // 3명중 한 명이라도 안고르면
        if(charName[0] == "null" || charName[1] == "null" || charName[2] == "null") {
            currentChoiceCharText.text = "캐릭터 선택 해야합니다.";
            return;
        }

        SceneManager.LoadScene("FirstStage");
    }
}
