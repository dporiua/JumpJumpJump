using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float camSpeed = 2f;
    public Transform playerPos;
    public float yOffset = 1f;

    
    void Update()
    {
        Vector3 pos = new Vector3(0f, playerPos.position.y + yOffset, -10f);
        transform.position = Vector3.Slerp(transform.position,pos,camSpeed * Time.deltaTime);
    }
}
