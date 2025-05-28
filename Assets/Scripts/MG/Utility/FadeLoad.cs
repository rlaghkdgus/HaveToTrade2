using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeLoad : MonoBehaviour
{
    public GameObject fadeImagePrefab;
    public string SceneName;

    private Image fadeImage;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void StartFadeLoad()
    {
        StartCoroutine(FadeOutAndLoadScene(SceneName));
    }

    IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        GameObject fadeImageObject = Instantiate(fadeImagePrefab, Vector2.zero, Quaternion.identity);
        fadeImage = fadeImageObject.GetComponentInChildren<Image>();

        DontDestroyOnLoad(fadeImageObject);

        yield return StartCoroutine(FadeTo(1f, 1f));

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
        SoundManager.Instance.BGMplay(true, BGMtype.Meat);
        yield return StartCoroutine(FadeTo(0f, 1f));

        yield return new WaitForSeconds(1f);
        Destroy(fadeImageObject);
        Destroy(gameObject);
    }

    IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float startAlpha = fadeImage.color.a;
        float time = 0;

        while (time < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
            time += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, targetAlpha);
    }
}
