using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 캐릭터 변경 UI 스크립트, 처음 캐릭터 생성(CharChoice.cs참조) 및 캐릭터 변경 기능
public class CharacterSwitch : MonoBehaviour
{
    private GameObject[] SwitchPlayer = new GameObject[3];
    private GameObject[] CharPortrait = new GameObject[3];

    public static bool CharCheck = true;   // 캐릭터가 바뀌는 순간(캐릭터 없을 때 false)
                                            //캐릭터 체크하는기능 off시키기 위함(ture일 때만 체크)
                                            // ex) PortalControl.cs

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 3; i++) {
            SwitchPlayer[i] = Resources.Load<GameObject>("Prefabs/Character/" + CharChoice.charName[i]);
            CharPortrait[i] = Resources.Load<GameObject>("Prefabs/Character/" + CharChoice.charName[i] + "_Portrait");
        }
        
        // Instantiate(SwitchPlayer[0]).transform.position = new Vector3(-6f, -7f);
        PrefabInst();
    }

    private void PrefabInst()
    {
        GameObject[] tempPortrait = new GameObject[3];

        Instantiate(SwitchPlayer[0]).transform.position = new Vector3(-6f, -7f);

        for(int i = 0; i < 2; i++) {    // 원래는 i < 3으로 해야함!!!
            tempPortrait[i] = Instantiate(CharPortrait[i]);
            tempPortrait[i].transform.SetParent(GameObject.Find("CharStat_" + i).transform);
            tempPortrait[i].transform.localPosition = new Vector3(-94f, 0f);
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
        
        currentPlayer.SetActive(false);
        Instantiate(SwitchPlayer[index]).transform.position = currentPlayerVector;
        Destroy(currentPlayer);

        CharCheck = true;
    }
}
