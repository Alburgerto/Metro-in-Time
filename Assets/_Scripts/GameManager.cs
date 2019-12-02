using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum Timeline { Start, Escorial, PuertaAlcalá, Atocha, End}
public enum GameState { Game, Pause }

public class GameManager : MonoBehaviour
{
    public GameObject m_specialSpot;
    public GameObject m_character;
    public GameObject m_pausePanel;
    public GameObject m_textPanel;
    public GameObject m_travelPanel;
    public GameObject m_blackPanel;
    public GameObject[] m_spots;
    public AudioClip[] m_musicClips;
    public AudioClip m_trainClip;
    public TextMeshProUGUI m_TMPText;
    public SceneText[] m_sceneText;
    public Sprite[] m_backgrounds;
    public Image m_backgroundImage;
    public TextMeshProUGUI m_collectedInfoText;
    public float m_fadeTime;

    private AudioSource m_audioSource;
    private Image m_blackPanelImage;
    private int m_collectedInfoCount;
    private int m_audioClip;
    private int m_background;
    private int m_timeLine; // Which SceneText
    private int m_textIndex; // Relative to one SceneText (m_sceneText[m_timeLineIndex])
    private Timeline m_time;

    private void Awake()
    {
        m_time = Timeline.Start;
        m_background = 0;
        m_timeLine   = 0;
        m_textIndex  = 0;
        m_collectedInfoCount = 0;
        m_audioSource = GetComponent<AudioSource>();
        m_blackPanelImage = m_blackPanel.GetComponent<Image>();
    }

    private void Start()
    {
        m_TMPText.text = m_sceneText[0].m_sceneLines[0];
        m_audioSource.clip = m_musicClips[0];
        m_audioSource.Play();

        StartCoroutine(StartGame());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            m_textPanel.SetActive(false);
            m_pausePanel.SetActive(true);
        }
    }

    public void DisplayText(string l_text)
    {
        m_TMPText.text = l_text;
    }

    public void TextClick()
    {
        if (m_textIndex == m_sceneText[m_timeLine].m_sceneLines.Length - 1)
        {
            if (m_timeLine == m_sceneText.Length - 1)
            {
                StartCoroutine(EndGame());
            }
            return;
        }
        m_textIndex++;
        DisplayText(m_sceneText[m_timeLine].m_sceneLines[m_textIndex]);
    }

    public void ShowTravelPanel()
    {
        m_travelPanel.SetActive(true);
    }

    public void TravelPanel(bool l_travel)
    {
        if (l_travel)
        {
            m_background++;
            m_audioClip++;
            m_timeLine++;
            m_textIndex = 0;
            StartCoroutine(NextScene());
        }
        m_travelPanel.SetActive(false);
    }

    private IEnumerator StartGame()
    {
        while (m_textIndex < m_sceneText[m_timeLine].m_sceneLines.Length - 1)
        {
            yield return new WaitForSeconds(2.5f);
            m_textIndex++;
            DisplayText(m_sceneText[m_timeLine].m_sceneLines[m_textIndex]);
        }
        m_background++;
        m_audioClip++;
        m_timeLine++;
        m_textIndex = 0;

        StartCoroutine(NextScene());
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(2.5f);
        Debug.Log("END");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator PlayText()
    {
        while (m_textIndex < m_sceneText[m_timeLine].m_sceneLines.Length - 1)
        {
            yield return new WaitForSeconds(2.5f);
            m_textIndex++;
            DisplayText(m_sceneText[m_timeLine].m_sceneLines[m_textIndex]);
        }

        if (m_timeLine == m_sceneText.Length - 1)
        {
            StartCoroutine(EndGame());
        }
    }

    public IEnumerator NextScene()
    {
        m_blackPanel.SetActive(true);

        float time = 0;
        Color color = new Color(0, 0, 0, 0);

        // fade out
        while (time < m_fadeTime)
        {
            color.a = Mathf.Lerp(0f, 1f, time / m_fadeTime);
            m_blackPanelImage.color = color;

            m_audioSource.volume = Mathf.Lerp(1, 0, time / m_fadeTime);

            time += Time.deltaTime;
            yield return null;
        }

        m_audioSource.volume = 0.4f;
        m_audioSource.clip = m_trainClip;
        m_audioSource.Play();
        yield return new WaitForSeconds(6);

        // When panel is black, change background image
        m_backgroundImage.sprite = m_backgrounds[m_background];

        int spot = 0;
        bool showChar = false;

        if (m_backgroundImage.sprite.name == "escorial")
        {
            spot = 0;
            showChar = true;

        }
        else if (m_backgroundImage.sprite.name == "alcalá")
        {
            spot = 1;
            showChar = true;

        }
        else if (m_backgroundImage.sprite.name == "atocha")
        {
            spot = 2;
            showChar = true;

        }
        else if (m_backgroundImage.sprite.name == "XXI_tren")
        {
            spot = -1;
            showChar = false;
        }
        DisplayPlayable(showChar, spot);

        DisplayText(m_sceneText[m_timeLine].m_sceneLines[m_textIndex]);
        time = 0;

        m_audioSource.volume = 0;
        m_audioSource.clip = m_musicClips[m_audioClip];
        m_audioSource.Play();

        // fade in
        while (time < m_fadeTime)
        {
            color.a = Mathf.Lerp(1f, 0f, time / m_fadeTime);
            m_blackPanelImage.color = color;

            m_audioSource.volume = Mathf.Lerp(0, 1, time / m_fadeTime);

            time += Time.deltaTime;
            yield return null;
        }
        m_blackPanel.SetActive(false);

        StartCoroutine(PlayText());
    }

    // Enables everything that appears when you can click on the screen
    public void DisplayPlayable(bool l_display, int l_level)
    {
        m_character.SetActive(l_display);
        m_specialSpot.SetActive(l_display);

        foreach (var spot in m_spots)
        {
            spot.SetActive(false);
        }
        if (l_level < 0) { return; }
        m_spots[l_level].SetActive(l_display);
    }

}
