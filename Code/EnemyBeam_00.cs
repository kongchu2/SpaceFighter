using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeam_00 : MonoBehaviour
{
    private float beamMoveSpeed = 17.5f;
    Vector3 MovePosi;
    private Vector3 targetPos;
    private float distance;
    private Vector3 playerBackPos = 100 * Vector3.back;
    private MeshRenderer beamMesh;
    public GameObject redBall = null;
    static int hexNum;
    private Vector3 direction;
    Vector3 movePosi;
    int i;
    private void Start()
    {
        beamMesh = GetComponent<MeshRenderer>();
        targetPos.z = playerBackPos.z;
        switch (tag)
        {
            case "RandomDirectionBeams":
                targetPos.x = Random.Range(-50f,51f);
                break;
            case "HexBall":
                targetPos.x = Random.Range(-25f, 26f);
                targetPos.z = transform.position.z + Random.Range(-20f, 21f);
                StartCoroutine("HexBoom");
                break;
            case "hexBoomClone":
                StartCoroutine("HexMove");
                break;
            case "InduceBall":
                targetPos = transform.position - GameObject.FindGameObjectWithTag("Player").transform.position;
                distance = targetPos.magnitude;
                direction = targetPos / distance;
                beamMoveSpeed = 30.0f;
                StartCoroutine("DirMove");
                return;
            case "background":
                targetPos.x = transform.position.x;
                beamMoveSpeed = 13.5f;
                break;
            default:
                targetPos.x = transform.position.x;
                break;
        }
        StartCoroutine("Move");
    }
    IEnumerator DirMove()
    {
        while(true)
        {
            transform.position += direction * -beamMoveSpeed * Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator Move()
    {
        while(true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, beamMoveSpeed * Time.deltaTime);
            yield return null;
        }
    }
    IEnumerator HexMove()
    {
        switch (gameObject.name[12])
        {
            case '0':
                movePosi = Vector3.right;
                break;
            case '1':
                movePosi = new Vector3(1f, 0f, -1f);
                break;
            case '2':
                movePosi = new Vector3(1f, 0f, 1f);
                break;
            case '3':
                movePosi = new Vector3(-1f, 0f, 1f);
                break;
            case '4':
                movePosi = new Vector3(-1f, 0f, -1f);
                break;
            default:
                movePosi = Vector3.left;
                break;
        }

        while (true)
        {
            transform.position += movePosi * beamMoveSpeed * Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator HexBoom()
    {
        
        while(!(transform.position == targetPos))
        {               
            yield return new WaitForFixedUpdate();
        }
        
        for (i = 0; i < 6; i++)
        {
            GameObject hexBoomClone = Instantiate(redBall, transform.position, Quaternion.Euler(90f, 0f, 0f));
            hexBoomClone.name = "hexBoomClone" + i;
            hexBoomClone.tag = "hexBoomClone";
        }
        Destroy(this.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }
    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
