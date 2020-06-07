using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDestination : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0)) 
        //    CastRayToWorld();

    }

    void CastRayToWorld()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 point = ray.origin + (ray.direction * 30);
        Debug.Log("World point " + point);
    }
}
