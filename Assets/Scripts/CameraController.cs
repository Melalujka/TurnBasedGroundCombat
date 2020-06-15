using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private float sense = 10f;
    private Vector3 camRot = Vector3.forward;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            StartCoroutine("RotateCamera");
        //{
        //    var cam = Camera.main.GetComponentInParent<Transform>();

        //    camRot = Quaternion.Euler(0, Input.GetAxis("Mouse X") * 10, 0) * camRot;
        //    Camera.main.transform.Rotate(-Input.GetAxis("Mouse Y") * sense, 0, 0);
            
        //    if (Input.GetMouseButtonDown(0))
        //        Cursor.visible = false;
        //}
        //else
        //    Cursor.visible = true;

        if (player != null)
        {
            //gameObject.transform.position = GetBehindPosition(player.transform, 30, 30);
            gameObject.transform.position = player.transform.position - (camRot - Vector3.up) * 30;
            
            gameObject.transform.LookAt(player.transform.position);
        }
        
    }

    IEnumerator RotateCamera()
    {
        yield return new WaitForSeconds(0.2f);
        Cursor.visible = false;
        while (Input.GetMouseButton(0))
        {
            camRot = Quaternion.Euler(0, Input.GetAxis("Mouse X") * 10, 0) * camRot;
            Camera.main.transform.Rotate(-Input.GetAxis("Mouse Y") * sense, 0, 0);
            yield return null;
        }
        Cursor.visible = true;
        Debug.Log("Coroutine finished");
    }

    Vector3 GetBehindPosition(Transform target, float distanceBehind, float distanceAbove)
    {
        return target.position - (target.forward * distanceBehind) + (target.up * distanceAbove);
    }
}
