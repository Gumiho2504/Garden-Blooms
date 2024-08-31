using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
public class MenuController : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;
    public GameObject settingPanel, infoPanel,loadingPanel;
    public Text high_score_text;
    


     IEnumerator Start()
    {
        fadeImage.gameObject.SetActive(true);
        FadeIn();
        LeanTween.scale(loadingPanel.transform.GetChild(2).gameObject, Vector3.one * 1.1f, 0.3f).setEase(LeanTweenType.easeInOutQuad).setLoopPingPong();
        yield return new WaitForSeconds(2f);
        LeanTween.alpha(fadeImage.rectTransform, 1f, fadeDuration).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
        {
            loadingPanel.SetActive(false);
            fadeImage.gameObject.SetActive(true);
            FadeIn();
        });
        high_score_text.text = $"highscore : {PlayerPrefs.GetInt("high", 0)}";
        
        
    }

    public void GoToGame(GameObject game)
    {

        AnimateButtonPress(game);
        AudioController.Instance.PlaySFX("click");
        FadeOutAndLoadScene("game");
    }

    public void QuitButton(GameObject game)
    {
        AnimateButtonPress(game);
        AudioController.Instance.PlaySFX("click");
        Application.Quit();
    }



    public void FadeIn()
    {
        LeanTween.alpha(fadeImage.rectTransform, 0f, fadeDuration).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
        {
            fadeImage.gameObject.SetActive(false);
        });
    }

    public void FadeOutAndLoadScene(string sceneName)
    {
        fadeImage.gameObject.SetActive(true);

        LeanTween.alpha(fadeImage.rectTransform, 1f, fadeDuration).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
        {
            SceneManager.LoadScene(sceneName);
        });
    }

    public void onClickInfor(int i)
    {
        AudioController.Instance.PlaySFX("click");
        GameObject g = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        AnimateButtonPress(g);
        if (i == 1) LeanTween.scale(infoPanel, Vector3.one, 0.6f).setEaseLinear();
        else LeanTween.scale(infoPanel, Vector3.zero, 0.6f).setEaseLinear();

    }

    public void onClickSetting(int i)
    {
        AudioController.Instance.PlaySFX("click");
        GameObject g = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        AnimateButtonPress(g);
        if (i == 1) LeanTween.scale(settingPanel, Vector3.one, 0.6f).setEaseLinear();
        else LeanTween.scale(settingPanel, Vector3.zero, 0.6f).setEaseLinear();

    }

    public void AnimateButtonPress(GameObject button)
    {
        LeanTween.scale(button, Vector3.one * 0.9f, 0.1f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
        {
            LeanTween.scale(button, Vector3.one, 0.1f).setEase(LeanTweenType.easeInOutQuad);
        });
    }
}