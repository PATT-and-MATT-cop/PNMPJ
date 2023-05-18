using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("Title");
    }

    void Update()
    {
        if(Input.anyKeyDown) {
            SceneManager.LoadScene("Game");
        }
    }
}