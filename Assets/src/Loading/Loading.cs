using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Loading : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_progressText;
    [SerializeField] private Slider m_slider;
    [SerializeField] private int m_sceneIndex;

    private void Start()
    {
        LoadSceen();
    }

    private void LoadSceen()
    {
        StartCoroutine(LoadSceenAysnc());
    }

    private IEnumerator LoadSceenAysnc()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(m_sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress);

            m_slider.value = progress;
            m_progressText.SetText((progress * 100.0f) + "%");

            yield return null;
        }



    }

}
