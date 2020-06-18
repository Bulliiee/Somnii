using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy의 HP컨트롤
public class EnemyControl : MonoBehaviour
{
    public float HP = 100f;
    // public static float HP = 100.0f;
    private float currentHP;

    // Start is called before the first frame update
    void Awake()
    {
        HP = 100.0f;
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
