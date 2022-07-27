using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public string toOpen;
   public void onPress()
    {
        SceneManager.LoadScene(toOpen, LoadSceneMode.Single);
    }

}
