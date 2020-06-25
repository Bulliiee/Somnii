using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 캐릭터 변경 UI 스크립트, 처음 캐릭터 생성(CharChoice.cs참조) 및 캐릭터 변경 기능, 캐릭터들의 현재 체력, 마나관리
public class CharacterSwitch : MonoBehaviour
{
    private GameObject[] SwitchPlayer = new GameObject[3];  // 플레이 할 캐릭터 프리팹3개
    private GameObject[] CharPortrait = new GameObject[3];  // 플레이 할 캐릭터의 초상화 프리팹 3개
    public Image[] HPUIImg = new Image[3];
    public Image[] HPBookImg = new Image[3];

    public float[] currentCharHp = new float[3];   // 캐릭터들의 HP 저장
    private float[] maxCharHP = new float[3];
    private int charIndex = 0;  // 캐릭터 인덱스

    public static bool CharCheck = true;   // 캐릭터가 바뀌는 순간(캐릭터 없을 때 false)
                                            //캐릭터 체크하는기능 off시키기 위함(ture일 때만 체크)
                                            // ex) PortalControl.cs

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 3; i++) {    // 캐릭터와 초상화 프리팹 잇기
            SwitchPlayer[i] = Resources.Load<GameObject>("Prefabs/Character/" + CharChoice.charName[i]);
            CharPortrait[i] = Resources.Load<GameObject>("Prefabs/Character/" + CharChoice.charName[i] + "_Portrait");
            currentCharHp[i] = SwitchPlayer[i].GetComponent<ControllerScript>().HP; // 각 캐릭터의 HP저장
            maxCharHP[i] = currentCharHp[i];
        }
        
        PrefabInst();
    }
    
    // 맞을 때
    public void CharHit(float damage)
    {
        currentCharHp[charIndex] -= damage;
        HPUIImg[charIndex].fillAmount = currentCharHp[charIndex] / maxCharHP[charIndex];
        HPBookImg[charIndex].fillAmount = currentCharHp[charIndex] / maxCharHP[charIndex];
    }

    private void PrefabInst()
    {
        GameObject[] tempPortrait = new GameObject[3];

        Instantiate(SwitchPlayer[0]).transform.position = new Vector3(-6f, -7f);

        for(int i = 0; i < 3; i++) {    
            tempPortrait[i] = Instantiate(CharPortrait[i]); //초상화 생성
            tempPortrait[i].transform.SetParent(GameObject.Find("CharStat_" + i).transform);    //부모설정
            tempPortrait[i].transform.localPosition = new Vector3(-94.5f, 0f);  // 위치 조정
        }
    }

    public void CharacterSwitchButton(int index)
    {
        GameObject currentPlayer = GameObject.FindGameObjectWithTag("Player");
        Vector3 currentPlayerVector = currentPlayer.transform.localPosition;

        Debug.Log("CurrentPlayer name: " + currentPlayer.name);
        Debug.Log("SwitchPlayer name: " + SwitchPlayer[index].name);

        if(currentPlayer.name == SwitchPlayer[index].name) {
            Debug.Log("Can't Change Player!!");
            return;
        }

        CharCheck = false;
        charIndex = index;
        
        currentPlayer.SetActive(false);
        Instantiate(SwitchPlayer[index]).transform.position = currentPlayerVector;
        Destroy(currentPlayer);

        CharCheck = true;
    }
}
