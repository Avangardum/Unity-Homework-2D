using UnityEngine;

public class PauseController : SingletonMonoBehavoiur<PauseController>
{
    public bool IsPaused { get; private set; } = false;

    public void Pause()
    {
        IsPaused = true;
        Time.timeScale = 0;
    }

    public void Unpause()
    {
        IsPaused = false;
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            SwitchPause();
    }

    public void SwitchPause()
    {
        if (IsPaused)
            Unpause();
        else
            Pause();
    }
}
