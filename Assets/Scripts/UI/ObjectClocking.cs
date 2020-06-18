using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 배경의 투명도 조절하는 스크립트, 캐릭터가 오브젝트 뒤로 들어갈시 작동
public class ObjectClocking : MonoBehaviour
{
    SpriteRenderer spr;
    private bool isCollide = false; // 플레이어 들어오면 true
    private bool isDone = false;  // 투명도 조절 완료 되면 true

    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    void Update() 
    {
        // 플레이어가 들어오면
        if(isCollide && !isDone) {
            Dark();
        }
        // 플레이어가 나가면
        else if(!isCollide && !isDone) {
            Transparent();
        }
    }

    void Dark() 
    {
            Color color = spr.color;
            color.a -=  0.1f;
            spr.color = color;

            if(spr.color.a <= 0.75f) {
                isDone = true;
            }
    }

    void Transparent()
    {
            Color color = spr.color;
            color.a +=  0.1f;
            spr.color = color;

            if(spr.color.a >= 1f) {
                isDone = true;
            }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Player") {
            isCollide = true;
            isDone = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Player") {
            isCollide = false;
            isDone = false;
        }
    }
}
