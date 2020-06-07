using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject cam;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject newPlayer = Clicked();
            if (newPlayer.name == "Player" || newPlayer.name == "Character_Elf")
            {
                Transform player = cam.GetComponent<CameraController>().player;
                if (player != null)
                    player.GetComponent<PlayerController>().isChoosenOne = false;

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
}
