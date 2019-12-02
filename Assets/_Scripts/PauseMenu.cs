using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject m_particles;
    public GameObject m_textPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Resume();
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        m_textPanel.SetActive(true);
        m_particles.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Exit()
    {
        Debug.Log("Exiting...");
        Application.Quit();
    }
}
