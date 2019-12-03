using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : SingletonMonoBehavoiur<SceneLoader>
{
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void ReloadScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }
}
