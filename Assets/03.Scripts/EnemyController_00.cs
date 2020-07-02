using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController_00 : MonoBehaviour
{
    public float moveSpeed = 15.0f;
    public float rotatespeed = 0.4f;
    public int hp = 5;
    public float moveRange = 5.0f;
    public float shootingDelay = 2.5f;
    public GameObject purpleBeam;
    public GameObject explosion;
    private Transform targetTrans;
    private Quaternion basicEnemyRotation = Quaternion.Euler(-180f,0f,0f);
    private bool isrecall;

    private CapsuleCollider playerCollider;

    Vector3 target = Vector3.zero;
    void Start()
    {
        playerCollider = GetComponent<CapsuleCollider>();
        if (tag == "induceEnemy")
        {
            StartCoroutine("InduceMove");
            moveSpeed = 30.0f;
        }
        else
            StartCoroutine("Move");
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "PlayerBeams":
                hp--;
                if (hp <= 0)
                {
                    playerCollider.enabled = false;
                    GameObject explosionObject = Instantiate(explosion, transform.position, Quaternion.identity);
                    
                    Destroy(explosionObject, 1f);
                    GameController.score += 75;
                    Destroy(gameObject);
                }
                break;
        }
    }
    void PlayerDie()
    {
        if (tag == "induceEnemy")
            basicEnemyRotation = Quaternion.Euler(0f, 180f, 0f);
        StartCoroutine(PlayerDieRotation(basicEnemyRotation));
    }
    IEnumerator PlayerDieRotation(Quaternion Rotation)
    {
        target = new Vector3(transform.position.x, 0f, -80f);
        while(transform.rotation != Rotation)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Rotation, rotatespeed);
            yield return null;
        }
    }
    IEnumerator InduceMove()
    {
        targetTrans = GameObject.FindGameObjectWithTag("Player").transform;
        while(!PlayerController.isDie)
        {
            transform.LookAt(targetTrans.position);
            transform.position = Vector3.MoveTowards(transform.position, targetTrans.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        StartCoroutine("RandomMove");
    }
    IEnumerator RandomMove()
    {
        while(true)
        {
            if (PlayerController.isDie && !isrecall)
            {
                isrecall = true;
                PlayerDie();
            }
            else if(!PlayerController.isDie)
            {
                Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
                if (pos.x < 0.05f)
                    pos.x = 0.05f;

                if (pos.x > 0.90f)
                    pos.x = 0.90f;

                if (pos.y < 0.30f)
                    pos.y = 0.30f;

                if (pos.y > 0.90f)
                    pos.y = 0.90f;

                transform.position = Camera.main.ViewportToWorldPoint(pos);
            }
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
    IEnumerator MovePos()
    {
        while(!PlayerController.isDie)
        {
            
            target.x = transform.position.x + Random.Range(-1, 2) * moveRange;
            target.z = transform.position.z + Random.Range(-1, 2) * moveRange;
            yield return new WaitForSeconds(Random.Range(1.3f, 3.1f));
        }
    }
    IEnumerator Shooting()
    {
        while(!PlayerController.isDie)
        {
            Instantiate(purpleBeam, transform.position, Quaternion.Euler(90f, 0f, 0f));
            yield return new WaitForSeconds(shootingDelay + Random.Range(0.1f,1f));
        }
    }
    IEnumerator Move()
    {
        while (transform.position.z > 25.0f)
        {
            transform.position += Vector3.back * moveSpeed * Time.deltaTime;
            yield return null;
        }
        
        StartCoroutine("MovePos");
        StartCoroutine("RandomMove");
        StartCoroutine("Shooting");
        StopCoroutine("Move");
    }
}
