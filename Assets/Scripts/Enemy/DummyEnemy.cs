using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DummyEnemy의 움직임
public class DummyEnemy : MonoBehaviour
{
    private float damage = 20f;
    
    // Start is called before the first frame update
    void Awake()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        // 플레이어와 충돌시, 현재 맞는 모션이 아니라면 해당 오브젝트의 OnHit()실행
        if(other.gameObject.tag == "Player" && !ControllerScript.hitCheck) {  
            ControllerScript playerCtrlScript = other.gameObject.GetComponent<ControllerScript>();

            if(!ControllerScript.isAttack) {    // 공격중이 아니라면
                playerCtrlScript.OnHit(gameObject);   // 맞는모션

            }
            GameObject.Find("GameManager").GetComponent<CharacterSwitch>().CharHit(damage); // 데미지주기
        }
    }
}
