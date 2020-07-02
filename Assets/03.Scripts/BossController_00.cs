using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController_00 : MonoBehaviour
{
    private Vector3 targetPos = new Vector3(0f, 3f, 40f);
    private Vector3 originalPos;
    public static int bossHp;
    private float bossMoveSpeed = 3.0f;
    private float bossMoveRange = 10.5f;   
    public static bool isDie;
    public GameObject bossBeam;
    public GameObject hexball;
    public GameObject ultCaution;
    public GameObject ultBeam;
    public GameObject ultCharging;
    private GameObject shottingPos;
    private GameObject shottingMinusPos;
    public GameObject boomParticle;

    CapsuleCollider bossCollider;
    void Start()
    {
        bossHp = SpawnManager.level * 100 + 250;
        isDie = false;
        bossCollider = GetComponent<CapsuleCollider>();
        StartCoroutine("Move");
        originalPos = targetPos;
        shottingPos = transform.GetChild(1).gameObject;
        shottingMinusPos = transform.GetChild(2).gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case "PlayerBeams":
                bossHp--;
                if (bossHp <= 0 && !isDie && !PlayerController.isDie)
                {
                    isDie = true;
                    SpawnManager.level++;
                    if(PlayerController.hp < 3)
                    {
                        GameObject.FindGameObjectWithTag("HpBar").transform.GetChild(PlayerController.hp).gameObject.SetActive(true);
                        PlayerController.hp++;
                        
                    }
                    StopAllCoroutines();
                    StartCoroutine("Boom");
                    Destroy(this.gameObject,2f);
                }
                break;
        }
    }
    IEnumerator Move()
    {
        while(transform.position.z >= originalPos.z)
        {
            transform.position += (bossMoveSpeed + 5f) * Vector3.back * Time.deltaTime;
            yield return null;
        }
        PlayerController.bossComing = false;

        StartCoroutine("AttackManager");
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, bossMoveSpeed * Time.deltaTime);
            yield return null;
        }
    }
    IEnumerator AttackManager()
    {
        while(true)
        {
            StartCoroutine("Shotting");
            yield return new WaitForSeconds(15f);
            StopCoroutine("Shotting");
            StartCoroutine("Ult");
            yield return new WaitForSeconds(15f);
            StopCoroutine("Ult");
            StopCoroutine("UltShotting");
            ultBeam.SetActive(false);
            StartCoroutine("HexBall");
            yield return new WaitForSeconds(6 + SpawnManager.level * 2);
        }
    }
    IEnumerator Shotting()
    {
        while(true)
        {
            Instantiate(bossBeam, shottingPos.transform.position, Quaternion.Euler(90, 0, 0));
            Instantiate(bossBeam, shottingMinusPos.transform.position, Quaternion.Euler(90, 0, 0));
            yield return new WaitForSeconds(0.35f - SpawnManager.level * 0.01f);
        }
    }
    IEnumerator HexBall()
    {
        int i;
        for (i = 0; i < 3 + SpawnManager.level; i++)
        {
            for(int j = 0; j < 1 + SpawnManager.level; j++)
            {
                Instantiate(hexball, shottingPos.transform.position, Quaternion.Euler(90f, 0f, 0f));
                Instantiate(hexball, shottingMinusPos.transform.position, Quaternion.Euler(90f, 0f, 0f));
            }
            yield return new WaitForSeconds(2.0f);
        }
    }
    IEnumerator Ult()
    {
        targetPos = originalPos;
        yield return new WaitForSeconds(3f);
        ultCharging.SetActive(true);
        StartCoroutine(UltCaution());
        yield return new WaitForSeconds(2f);
        ultCharging.SetActive(false);
        StartCoroutine("UltMove");
        StartCoroutine("UltShotting");
        ultBeam.SetActive(true);
    }
    IEnumerator UltCaution()
    {
        ultCaution.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        ultCaution.SetActive(false);
        yield return new WaitForSeconds(0.4f);
        ultCaution.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        ultCaution.SetActive(false);
        yield return new WaitForSeconds(0.4f);
        ultCaution.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        ultCaution.SetActive(false);
    }
    IEnumerator UltMove()
    {
        while(true)
        {
            if(transform.position == targetPos)
            {
                targetPos.x = Random.Range(-1, 2) * bossMoveRange;
                targetPos.z = Random.Range(20f, originalPos.z);
            }
            yield return null;
        }
    }
    IEnumerator UltShotting()
    {
        while(true)
        {

            Instantiate(bossBeam, shottingPos.transform.position, Quaternion.Euler(90f, 0f, 0f));
            Instantiate(bossBeam, shottingMinusPos.transform.position, Quaternion.Euler(90f, 0f, 0f));
            yield return new WaitForSeconds(0.45f - SpawnManager.level * 0.02f);
        }
    }

    IEnumerator Boom()
    {
        while (true)
        {
            Vector3 particleSpawnPos = new Vector3(Random.Range(-8, 9) + transform.position.x,
                Random.Range(-4, 4),
                transform.position.z + Random.Range(-25, 14));
            GameObject particleClone = Instantiate(boomParticle, particleSpawnPos, Quaternion.identity);
            Destroy(particleClone, 1f);
            yield return new WaitForSeconds(0.08f);
        }
    }
}
