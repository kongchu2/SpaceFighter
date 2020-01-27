using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Transform playerTransform;
    Transform beamSpawnTrans;
    CapsuleCollider playerCollider;
    GameObject playerPrefab;

    GameObject[] enemyBeams;
    private GameObject engineTrails;
    private GameObject defaultBeam;
    public GameObject yellowBeam;
    public GameObject blueBeam;
    public GameObject hpBar;
    public GameObject explosionParticle;
    public GameObject skill1Particle;
    public GameObject skill1Image;
    public GameObject skill1;
    public GameObject gameOverUI;
    public Text yourScoreUI;

    private Quaternion newRotation;
    private Vector3 movePosi;

    private float horizontalValue;
    private float verticalValue;

    private bool doubleShooting;
    public static bool bossComing;
    public static bool isDie;

    private float shootingDelayTime = 0.15f;

    private float dragMoveSpeed = 2.2f;
    private float moveSpeed = 40.0f;
    private float rotateSpeed = 0.2f;
   
    public static int hp = 3;
    private int skill1cnt = 2;

    void Start()
    {
        playerTransform = GetComponent<Transform>();
        playerCollider = GetComponent<CapsuleCollider>();
        playerPrefab = playerTransform.GetChild(0).gameObject;

        hp = 3;
        isDie = false;
        defaultBeam = yellowBeam;
        engineTrails = playerTransform.GetChild(0).gameObject.transform.GetChild(1).gameObject;

        InvokeRepeating("Shooting", 0f, shootingDelayTime);
        StartCoroutine("Skill1");
    }

    void Update()
    {
        PlayerMoveKeyboard();
    }
    private void FixedUpdate()
    {
        SideWarp();
    }
    void PlayerMoveKeyboard()
    {
        //입력 가로/세로
        horizontalValue = Input.GetAxisRaw("Horizontal");
        verticalValue = Input.GetAxisRaw("Vertical");

        //가로 회전
        if (Mathf.Abs(horizontalValue) > 0)
        {
            newRotation = Quaternion.Euler(0f, 0f, horizontalValue > 0 ? -20f : 20f);

            playerTransform.rotation = Quaternion.Slerp(
                playerTransform.rotation, newRotation, rotateSpeed);
        }
        else
            playerTransform.rotation = Quaternion.Slerp(
                playerTransform.rotation, Quaternion.identity, rotateSpeed);

        //engineTrail on/off
        if (verticalValue > 0)
            engineTrails.SetActive(true);
        else
            engineTrails.SetActive(false);

        movePosi = new Vector3(horizontalValue, 0f, verticalValue);
        playerTransform.position += moveSpeed * movePosi * Time.deltaTime;
    }
    void Shooting()
    {
        if (bossComing)
            return;
        if(!doubleShooting)
            Instantiate(defaultBeam, playerTransform.position, Quaternion.Euler(90f,0f,0f));
        else
        {
            Vector3 doubleShootingPos = new Vector3(playerTransform.position.x + 1.25f,0f,playerTransform.position.z);
            Instantiate(defaultBeam, doubleShootingPos, Quaternion.Euler(90f, 0f, 0f));
            doubleShootingPos.x -= 2.5f;
            Instantiate(defaultBeam, doubleShootingPos, Quaternion.Euler(90f, 0f, 0f));
        }
    }
    void SideWarp()
    {
        
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        if (pos.x < 0f)
            pos.x = 1f;

        if (pos.x > 1f)
            pos.x = 0f;

        if (pos.y < 0.01f)
            pos.y = 0.01f;

        if (pos.y > 0.99f)
            pos.y = 0.99f;

        playerTransform.position = Camera.main.ViewportToWorldPoint(pos);
    }
    IEnumerator Skill1()
    {
        while(true)
        {
            if ((Input.touchCount > 1 || Input.GetKeyDown(KeyCode.Q)) && skill1cnt > 0)
            {
                enemyBeams = GameObject.FindGameObjectsWithTag("EnemyBeams");
                foreach (GameObject enemyBeam in enemyBeams)
                {
                    Destroy(enemyBeam);
                }
                enemyBeams = GameObject.FindGameObjectsWithTag("HexBall");
                foreach (GameObject enemyBeam in enemyBeams)
                {
                    Destroy(enemyBeam);
                }
                enemyBeams = GameObject.FindGameObjectsWithTag("hexBoomClone");
                foreach (GameObject enemyBeam in enemyBeams)
                {
                    Destroy(enemyBeam);
                }
                enemyBeams = GameObject.FindGameObjectsWithTag("RandomDirectionBeams");
                foreach (GameObject enemyBeam in enemyBeams)
                {
                    Destroy(enemyBeam);
                }
                enemyBeams = GameObject.FindGameObjectsWithTag("InduceBall");
                foreach(GameObject enemyBeam in enemyBeams)
                {
                    Destroy(enemyBeam);
                }
                skill1cnt--;
                Destroy(skill1.transform.GetChild(skill1cnt).gameObject);
                GameObject skill1Part = Instantiate(skill1Particle, playerTransform.position, Quaternion.identity);
                Destroy(skill1Part, 1f);
                yield return new WaitForSeconds(1f);
            }
            yield return null;
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        switch(col.tag)
        {
            case "Enemy":
            case "Boss":
            case "InduceEnemy":
			case "InduceBall":
            case "EnemyBeams":
            case "RandomDirectionBeams":
            case "hexBoomClone":
            case "FinalAttack":
                hp--;
                hpBar.transform.GetChild(hp).gameObject.SetActive(false);
                Die();
                StartCoroutine("Hitting");
                break;
            case "Skill1":
                skill1cnt++;
                GameObject skill1Clone = Instantiate(skill1Image, skill1.transform);
                skill1Clone.transform.localPosition = new Vector3((-130f * (skill1cnt-1)+50f), 0f, 0f);
                break;
            case "2x":
                if (doubleShooting)
                    StopCoroutine("DoubleShooting");
                StartCoroutine("DoubleShooting");
                break;
        }
    }
    void Die()
    {
        if (hp <= 0)
        {
            isDie = true;
            gameOverUI.SetActive(true);
            yourScoreUI.text = "Your Score : " + GameController.score +
                            "\n High Score : " + GameController.highScore;
            GameObject explosionClone = Instantiate(explosionParticle, transform.position, Quaternion.identity);
            Destroy(explosionClone, 1f);
            Destroy(this.gameObject);
        }
    }
    IEnumerator DoubleShooting()
    {
        defaultBeam = blueBeam;
        doubleShooting = true;
        yield return new WaitForSeconds(5f);
        doubleShooting = false;
        defaultBeam = yellowBeam;
    }
    IEnumerator Hitting()
    {
        int i;
        playerCollider.enabled = false;
        for(i=0;i<7;i++)
        {
            playerPrefab.SetActive(!playerPrefab.activeSelf);
            yield return new WaitForSeconds(0.3f);
        }
        playerCollider.enabled = true;
        playerPrefab.SetActive(true);
    }
}
