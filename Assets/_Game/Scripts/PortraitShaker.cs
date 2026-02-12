using System.Collections;
using UnityEngine;
using Yarn.Unity; // Yarn Spinner 기능을 가져옵니다.

public class PortraitShaker : MonoBehaviour
{
    // 원래 위치를 기억할 변수
    private Vector3 originalPos;

    void Start()
    {
        // 게임 시작할 때 내 원래 위치를 저장해둠
        originalPos = transform.localPosition;
    }

    // Yarn 스크립트에서 <<shake>>라고 쓰면 이 함수가 발동됨!
    [YarnCommand("shake")]
    public void ShakePortrait()
    {
        StartCoroutine(ShakeCo());
    }

    // 0.5초 동안 덜덜 떠는 기능
    IEnumerator ShakeCo()
    {
        float elapsed = 0.0f;
        float duration = 0.5f; // 떨리는 시간
        float magnitude = 10.0f; // 떨리는 강도 (숫자가 클수록 심하게 떨림)

        // 원래 위치 갱신 (혹시 움직였을까봐)
        originalPos = transform.localPosition;

        while (elapsed < duration)
        {
            // 랜덤한 위치로 순간이동 반복
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 다 떨고 나면 원래 위치로 복귀
        transform.localPosition = originalPos;
    }
}