using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 캐릭터의 공격이펙트(캐릭터의 애니메이션 이벤트로 실행)
    public void FirstAttackEffect()
    {
        gameObject.transform.GetChild(1).gameObject.GetComponent<Animator>().Play("Effect_FAttack");
    }
    public void SecondAttackEffect()
    {
        gameObject.transform.GetChild(1).gameObject.GetComponent<Animator>().Play("Effect_SAttack");
    }
    public void ThirdAttackEffect()
    {
        gameObject.transform.GetChild(1).gameObject.GetComponent<Animator>().Play("Effect_TAttack");
    }

    // 캐릭터의 무기 공격모션(캐릭터의 애니메이션 이벤트로 실행)
    public void FirstWeponAttack()
    {
        gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>().Play("Wepon_FAttack");
    }
    public void SecondWeponAttack()
    {
        gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>().Play("Wepon_SAttack");
    }
    public void ThirdWeponAttack()
    {
        gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>().Play("Wepon_TAttack");
    }

    // 캐릭터의 무기 공격이펙트(캐릭터의 애니메이션 이벤트로 실행)
    public void FirstWeponAttackEffect()
    {
        gameObject.transform.GetChild(2).gameObject.GetComponent<Animator>().Play("Wepon_FAttack_Effect");
    }
    public void SecondWeponAttackEffect()
    {
        gameObject.transform.GetChild(2).gameObject.GetComponent<Animator>().Play("Wepon_SAttack_Effect");
    }
    public void ThirdWeponAttackEffect()
    {
        gameObject.transform.GetChild(2).gameObject.GetComponent<Animator>().Play("Wepon_TAttack_Effect");
    }

    // 공격버튼 활성화 및 비활성화
    public void FirstAttackBtnActive()
    {
        GameObject.Find("UIBtn").transform.Find("FAttackBtn").gameObject.SetActive(true);
    }
    public void SecondAttackBtnActive()
    {
        GameObject.Find("UIBtn").transform.Find("SAttackBtn").gameObject.SetActive(true);
    }
    public void ThirdAttackBtnActive()
    {
        GameObject.Find("UIBtn").transform.Find("TAttackBtn").gameObject.SetActive(true);
    }
    public void AttackBtnInit()
    {
        GetComponent<Animator>().SetBool("FtoS", false);
        GetComponent<Animator>().SetBool("StoT", false);
        GameObject.Find("UIBtn").transform.Find("FAttackBtn").gameObject.SetActive(true);
        GameObject.Find("UIBtn").transform.Find("SAttackBtn").gameObject.SetActive(false);
        GameObject.Find("UIBtn").transform.Find("TAttackBtn").gameObject.SetActive(false);
        ControllerScript.isAttack = false;
    }

    // 어택 바운드 활성화 및 비활성화
    public void OnFAttackBound()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void OnSAttackBound()
    {
        gameObject.transform.GetChild(1).gameObject.SetActive(true);
    }
    public void OnTAttackBound()
    {
        gameObject.transform.GetChild(2).gameObject.SetActive(true);
    }
    public void OffFAttackBound()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
    public void OffSAttackBound()
    {
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }
    public void OffTAttackBound()
    {
        gameObject.transform.GetChild(2).gameObject.SetActive(false);
    }

    public void GameObjectOff()
    {
        gameObject.SetActive(false);
    }

    // 공격범위 및 판정 함수
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Enemy") {
            ControllerScript _player = GameObject.FindWithTag("Player").GetComponent<ControllerScript>();
            EnemyControl enemyCtrl = other.gameObject.GetComponent<EnemyControl>(); // 충돌한 오브젝트의 스크립트 받음
            float damage = 0;

            if(gameObject.name == "FAttackBound") {
                damage = _player.attackDamage[0];
            }
            else if(gameObject.name == "SAttackBound") {
                damage = _player.attackDamage[1];
            }
            else if(gameObject.name == "TAttackBound") {
                damage = _player.attackDamage[2];
            }
            else {
                damage = 0f;
            }

            enemyCtrl.HP -= damage;
        }
    }
}
