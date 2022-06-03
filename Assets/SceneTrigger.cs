using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour
{

    public GameObject Player;
    public GameObject PlayerCam;
    public GameObject cutscene;
    public GameObject cutsceneCam;
    public GameObject UI;
    public GameObject CamBars;

    private void OnTriggerEnter(Collider other)
    {
        Player.SetActive(false);
        UI.SetActive(false);
        PlayerCam.SetActive(false);

        cutscene.SetActive(true);
        cutsceneCam.SetActive(true);
        CamBars.SetActive(true);

       Invoke("sceneSpace", 9.0f);
    }

    void sceneSpace()
    {
        SceneManager.LoadScene(4, LoadSceneMode.Single);
    }

}