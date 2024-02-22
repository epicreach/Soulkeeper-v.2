using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GoToGameplayScene : MonoBehaviour
{
    public void goToBoss(){
        SceneManager.LoadSceneAsync(2);
    }
}
