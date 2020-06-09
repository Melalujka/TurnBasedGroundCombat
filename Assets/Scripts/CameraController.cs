using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            gameObject.transform.position = GetBehindPosition(player, 30, 30);
            // gameObject.transform.rotation = player.rotation + new Vector3(-30, 30, -30);
            gameObject.transform.LookAt(player.position);
        }
        
    }

    Vector3 GetBehindPosition(Transform target, float distanceBehind, float distanceAbove)
    {
        return target.position - (target.forward * distanceBehind) + (target.up * distanceAbove);
    }
}
