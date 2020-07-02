using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBeamController : MonoBehaviour
{
    public float moveSpeed = 50.0f;
    MeshRenderer BeamMesh;

    void Start()
    {
        BeamMesh = GetComponent<MeshRenderer>();
    }
    void Update()
    {
        BeamMove();
    }
    void BeamMove()
    {
        transform.position += moveSpeed * Vector3.forward * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case "Enemy":
            case "induceEnemy":
            case "Boss":
                Destroy(gameObject);
                break;
        }
            
    }
    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
