using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController_02 : MonoBehaviour
{
    public float moveSpeed = 7.0f;
    int hp = 6;
    public float explosionScale = 2.0f;
    public GameObject induceBall;
    private MeshCollider myCollider;
    public GameObject explosionObject;
    void Start()
    {
        myCollider = GetComponent<MeshCollider>();

        StartCoroutine(Shooting());
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z < -60f)
            Destroy(this.gameObject);
        transform.position += Vector3.back * moveSpeed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case "PlayerBeams":
                hp--;
                if(hp <= 0)
                {
                    myCollider.enabled = false;
                    GameObject explosion = Instantiate(explosionObject, transform.position, Quaternion.identity);

                    explosion.transform.localScale = Vector3.one * explosionScale;
                    Destroy(explosion, 1f);
                    GameController.score += 100;
                    Destroy(gameObject);
                }
                break;
        }
    }
    IEnumerator Shooting()
    {
        yield return new WaitForSeconds(2.5f);
        while(!PlayerController.isDie)
        {
            Vector3 spawnPos = new Vector3(transform.position.x, 0f, transform.position.z + 1);
            Instantiate(induceBall, spawnPos, Quaternion.Euler(90f,0f,0f));
            yield return new WaitForSeconds(4.5f);
        }
    }
}
