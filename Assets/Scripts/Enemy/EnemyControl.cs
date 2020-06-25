using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy의 공통 HP컨트롤
public class EnemyControl : MonoBehaviour
{
    public float HP = 100f; // 적의 체력 총량(inspector에서 개별 조정 필요)
    private float currentHP;    //현재 체력

    // Start is called before the first frame update
    void Awake()
    {
        //HP = 100.0f;
        currentHP = HP;
    }

    // Update is called once per frame
    void Update()
    {
        if(HP <= 0) {   // 죽을 때
            Destroy(gameObject, 0.1f);
        }

        if(HP != currentHP) {   // 맞을 때
            currentHP = HP;
            Debug.Log("DEnemy Hit!");
        }
    }
}
