using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject cam;

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject newPlayer = Clicked();
            if (newPlayer.CompareTag(Tags.Player.ToString()))
            {
                PlayerController player = cam.GetComponent<CameraController>().player.GetComponent<PlayerController>();
                if (player != null)
                {
                    //PlayerController playerScript = player.GetComponent<PlayerController>();
                    player.isChoosenOne = false;
                    player.agent.destination = player.transform.position;
                }

                cam.GetComponent<CameraController>().player = newPlayer.transform;
                newPlayer.GetComponent<PlayerController>().isChoosenOne = true;
            }
        }
    }

    GameObject Clicked()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, 1000))
            return hit.collider.gameObject;
        else
            return null;
    }

    void GameOver()
    {
        SceneManager.LoadScene(SceneTag.Menu.ToString());
    }
}
