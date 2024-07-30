using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title_Ui : MonoBehaviour
{
    public GameObject LoadingScreen;
    public Text TextLoading;
    public int SceneNum;

    private Image loadingScreenImage; // Reference to the Image component
    private Color textLoadingColor; // Store the original text color

    // Start is called before the first frame update
    void Start()
    {
        // Get the Image component from the LoadingScreen GameObject
        loadingScreenImage = LoadingScreen.GetComponent<Image>();
        // Store the original text color
        textLoadingColor = TextLoading.color;

        // Set LoadingScreen active and alpha to 1
        LoadingScreen.SetActive(true);
        loadingScreenImage.color = new Color(loadingScreenImage.color.r, loadingScreenImage.color.g, loadingScreenImage.color.b, 1f);
        TextLoading.color = new Color(textLoadingColor.r, textLoadingColor.g, textLoadingColor.b, 1f);

        // Start decreasing alpha
        StartCoroutine(DecreaseAlpha(loadingScreenImage));
        StartCoroutine(DecreaseAlpha(TextLoading));
    }

    public void LoadScene0()
    {
        LoadingScreen.SetActive(true);
        // Start increasing the alpha value of the LoadingScreen image
        StartCoroutine(IncreaseAlpha(loadingScreenImage));

        // Start increasing the alpha value of the TextLoading text
        StartCoroutine(IncreaseAlpha(TextLoading));

        //SceneManager.LoadScene(0); // Load Scene 0
    }

    public void LoadScene1()
    {
        LoadingScreen.SetActive(true);
        // Start increasing the alpha value of the LoadingScreen image
        StartCoroutine(DecreaseAlpha(loadingScreenImage));

        // Start increasing the alpha value of the TextLoading text
        StartCoroutine(DecreaseAlpha(TextLoading));

        //SceneManager.LoadScene(0); // Load Scene 0
    }

    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator IncreaseAlpha(Graphic graphic)
    {
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime;
            graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, alpha);
            yield return null;
        }
        SceneManager.LoadScene(SceneNum);
    }

    private IEnumerator DecreaseAlpha(Graphic graphic)
    {
        float alpha = 1f;
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime;
            graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, alpha);
            yield return null;
        }
        LoadingScreen.SetActive(false); // Set LoadingScreen active to false when alpha reaches 0
    }
}