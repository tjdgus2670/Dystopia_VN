using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity; 

public class BackgroundController : MonoBehaviour
{
    public Image backgroundImage; 

    void Start()
    {
        // 1. 초기화 (배경은 처음에 안 보일 수도 있고, 기본 배경을 둘 수도 있음)
        // 여기서는 일단 투명하게 시작하지 않고, 검은색 등으로 채워둘 수도 있습니다.
        if (backgroundImage != null) backgroundImage.color = Color.black;

        // 2. 명령어 등록: <<set_bg 파일이름>>
        var runner = FindObjectOfType<DialogueRunner>();
        if (runner != null)
        {
            runner.AddCommandHandler<string>("set_bg", SetBackground);
            Debug.Log("[System] 배경 제어 모듈(set_bg) 활성화 완료");
        }
    }

    public void SetBackground(string imageName)
    {
        // Resources/Backgrounds 폴더에서 찾습니다!
        Sprite newBg = Resources.Load<Sprite>("Backgrounds/" + imageName.ToLower());

        if (newBg != null)
        {
            backgroundImage.sprite = newBg;
            backgroundImage.color = Color.white; // 보이게 설정
            Debug.Log($"[System] 배경 전환: {imageName}");
        }
        else
        {
            Debug.LogError($"[Error] 배경을 찾을 수 없습니다: Resources/Backgrounds/{imageName}");
            // 못 찾으면 그냥 검은 화면으로 (에러 처리)
            backgroundImage.color = Color.black; 
        }
    }
}