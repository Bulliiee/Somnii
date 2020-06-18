using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//보스룸 클리어 체크
public class BossClearCheck : MonoBehaviour
{
    public GameObject stageClearText;

    private GameObject Enemy;

    // Start is called before the first frame update
    void Start()
    {
        Enemy = GameObject.Find("BossEnemies");
    }

    // Update is called once per frame
    void Update()
    {
        if(Enemy.transform.childCount < 1) {
            stageClearText.SetActive(true);
        }
    }
}
