using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundSroll : MonoBehaviour
{
    public float scrollSpeed = 0.15f;
    Material myMaterial;

    private void Start()
    {
        myMaterial = GetComponent<MeshRenderer>().material;
    }
    private void Awake()
    {
        Screen.SetResolution(828, 1792, true);
    }
    private void Update()
    {
        if(!PlayerController.isDie)
        {
            float newOffsetX = myMaterial.mainTextureOffset.x + scrollSpeed * Time.deltaTime;
            Vector2 newOffset = new Vector2(newOffsetX, 0);

            myMaterial.mainTextureOffset = newOffset;
        }
    }
}
