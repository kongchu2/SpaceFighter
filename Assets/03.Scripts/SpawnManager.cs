using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemy_00;
    public GameObject enemy_01;
    public GameObject enemy_02;
    public GameObject boss_00;
    private GameObject[] enemys = null;

    public GameObject backgroundGalaxy1;
    public GameObject backgroundGalaxy2;

    public AudioClip boss_00BGM;
    private AudioClip basicBGM;
    private AudioSource myAudio;

    public GameObject skill1Item;
    public GameObject x2SkillItem;

    private Vector3 spawnPos;


    public static int level = 1;
    private int i;
    private void Start()
    {
        level = 1;
        StartCoroutine("Spawn");
        StartCoroutine("ItemSpawn");
        StartCoroutine("InduceEnemySpawn");
        StartCoroutine("X2ItemSpawn");
        StartCoroutine(BackgroundSpawn());
        myAudio = GetComponent<AudioSource>();
        basicBGM = myAudio.clip;
    }
    private void FixedUpdate()
    {
        if (PlayerController.isDie)
            StopAllCoroutines();
        
    }
    IEnumerator Spawn()
    {
        while (true)
        {

            for (i = 0; i < 3 + level; i++)
            {
                if(i%2==0)
                    spawnPos = new Vector3(Random.Range(-26, 1), 0f, transform.position.z);
                else
                    spawnPos = new Vector3(Random.Range(1, 27), 0f, transform.position.z);
                Instantiate(enemy_00, spawnPos, Quaternion.Euler(-180f, 0f, 0f));
                yield return new WaitForSeconds(Random.Range(0.4f, 1.4f));
            }
            if(true)
                for(i=0; i < 2 ; i++)
                {
                    float spawnXPos = (Random.Range(-1, 1) == -1 ? -1:1) *(Random.Range(-1, 1) == -1 ? 20f:9f);
                    spawnPos = new Vector3(spawnXPos, 0f, transform.position.z);
                    Instantiate(enemy_02, spawnPos, Quaternion.Euler(-180f, 0f, 0f));
                }
            while (GameObject.FindGameObjectWithTag("Enemy") != null)
            {
                yield return new WaitForFixedUpdate();
            }
            if (true)
            {
                PlayerController.bossComing = true;
                enemys = GameObject.FindGameObjectsWithTag("induceEnemy");
                foreach (GameObject enemy in enemys)
                {
                    Destroy(enemy);
                }
                myAudio.Stop();
                myAudio.clip = boss_00BGM;
                myAudio.time = 0;
                myAudio.Play();
                spawnPos = new Vector3(0f, 3f, transform.position.z);
                Instantiate(boss_00, spawnPos, Quaternion.identity);
                while (GameObject.FindGameObjectWithTag("Boss") != null)
                {
                    yield return new WaitForFixedUpdate();
                }
                yield return new WaitForSeconds(2.5f);
                myAudio.Stop();
                myAudio.clip = basicBGM;
                myAudio.time = 0;
                myAudio.Play();
                yield return new WaitForSeconds(5f);
            }
        }
    }
    IEnumerator InduceEnemySpawn()
    {
        while(true)
        {
            yield return new WaitForSeconds(7.0f);
            if (true)
            {
                spawnPos = new Vector3(Random.Range(-24, 25), 0f, transform.position.z);
                Instantiate(enemy_01, spawnPos, Quaternion.Euler(180f, 0f, 0f));
            }
            yield return new WaitForSeconds(2.0f);
            if (true)
            {
                spawnPos = new Vector3(Random.Range(-24, 25), 0f, transform.position.z);
                Instantiate(enemy_01, spawnPos, Quaternion.Euler(180f, 0f, 0f));
            }
            break;
        }
    }
    IEnumerator ItemSpawn()
    {
        while(true)
        {
            yield return new WaitForSeconds(5f);
            if (true)
            {
                spawnPos = new Vector3(Random.Range(-25, 26), 0f, transform.position.z);
                Instantiate(skill1Item, spawnPos, Quaternion.Euler(90f,0f,0f));
            }
            yield return new WaitForSeconds(7f);
        }
    }
    IEnumerator X2ItemSpawn()
    {
        while(true)
        {
            yield return new WaitForSeconds(8f);
            if (true)
            {
                spawnPos = new Vector3(Random.Range(-25, 26), 0f, transform.position.z);
                Instantiate(x2SkillItem, spawnPos, Quaternion.Euler(90f, 0f, 0f));
            }
            yield return new WaitForSeconds(2f);
        }
    }
    IEnumerator BackgroundSpawn()
    {
        while(true)
        {
            
            GameObject backgroundGalaxy;
            spawnPos = new Vector3(Random.Range(-25, 26), -9f, transform.position.z);
            if (Random.Range(0,2) == 0)
                backgroundGalaxy = Instantiate(backgroundGalaxy1, spawnPos, Quaternion.Euler(90f, 0f, 0f));
            else
                backgroundGalaxy = Instantiate(backgroundGalaxy2, spawnPos, Quaternion.Euler(90f, 0f, 0f));
            backgroundGalaxy.transform.localScale = Vector3.one * Random.Range(3f, 7.5f);
            yield return new WaitForSeconds(Random.Range(13.5f, 17.5f));
        }
    }
}
