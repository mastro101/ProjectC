using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelector : MonoBehaviour
{
    public static SceneSelector instance;

    #region Mono
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    #region API
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    #endregion
}
