using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesLoader : MonoBehaviour
{
	void Start ()
    {
        SceneManager.LoadScene("Env_Forest", LoadSceneMode.Additive);
        //SceneManager.LoadSceneAsync("Env_Dungeon", LoadSceneMode.Additive);
    }
}

