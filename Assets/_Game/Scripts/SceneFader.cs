using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections; // 코루틴 사용을 위해 필수!

public class SceneFader : MonoBehaviour
{
    public Image fadeImage; // 검은 막
    public float fadeSpeed = 1.5f; // 암전 속도

    // 외부에서 부를 함수 (버튼에 연결할 것)
    public void FadeTo(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    // 서서히 어두워지는 기능 (코루틴)
    IEnumerator FadeOut(string sceneName)
    {
        // 1. 마우스 입력 차단 (버튼 중복 클릭 방지)
        if (fadeImage != null) fadeImage.raycastTarget = true;

        // 2. 투명도(Alpha)를 0에서 1로 서서히 올림
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            if (fadeImage != null)
            {
                Color c = fadeImage.color;
                c.a = t;
                fadeImage.color = c;
            }
            yield return null; // 한 프레임 대기
        }

        // 3. 다 어두워지면 씬 이동!
        SceneManager.LoadScene(sceneName);
    }
}