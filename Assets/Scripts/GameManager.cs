using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject cam;

    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    GameObject newPlayer = Clicked();
        //    if (newPlayer != null)
        //    {
        //        GameObject player = cam.GetComponent<CameraController>().player;
        //        if (Clicked().GetComponent<PlayerController>() != null && !(newPlayer == player))
        //        //if (newPlayer.CompareTag(Tags.Character.ToString()) && !(newPlayer == player))
        //        {
        //            if (player != null)
        //            {
        //                PlayerController playerScript = player.GetComponent<PlayerController>();
        //                playerScript.isChoosenOne = false;
        //                //playerScript.agent.destination = player.transform.position;
        //            }

        //            cam.GetComponent<CameraController>().player = newPlayer;
        //            newPlayer.GetComponent<PlayerController>().isChoosenOne = true;
        //        }
        //    }
        //}
    }

    GameObject Clicked()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;

        //RaycastHit hit = new RaycastHit();

        //Color color = new Color(0.5f, 0.5f, 1.0f);
        //Debug.DrawLine(Camera.main.transform.position, Input.mousePosition, color);

        //if (Physics.Raycast(ray, out hit, 200))
        //    return hit.collider.gameObject;
        //else
        //    return null;

        return Physics.Raycast(ray, out RaycastHit hit, 200) ? hit.collider.gameObject : null;
    }

    void GameOver()
    {
        SceneManager.LoadScene(SceneTag.Menu.ToString());
    }
}

////Raycast against a specific collider (plane is a gameObject or Transform)
//if(plane.collider.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), hit))