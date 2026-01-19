using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity; 

public class PortraitController : MonoBehaviour
{
    public Image portraitImage; 

    // [수정] Awake 대신 Start를 사용하여 안전하게 등록합니다.
    void Start()
    {
        // 1. 이미지 숨기기 (초기화)
        if (portraitImage != null) portraitImage.color = Color.clear;

        // 2. 명령어 등록
        // FindObjectOfType을 사용하면 이 스크립트가 어디에 붙어있든 DialogueRunner를 찾아냅니다.
        var runner = FindObjectOfType<DialogueRunner>();
        
        if (runner != null)
        {
            runner.AddCommandHandler<string>("show_face", ShowFace);
            runner.AddCommandHandler("hide_face", HideFace);
            Debug.Log("[System] PortraitController 명령어(show_face) 등록 완료");
        }
        else
        {
            Debug.LogError("[Error] DialogueRunner를 찾을 수 없습니다. PortraitController가 작동하지 않습니다.");
        }
    }

    public void ShowFace(string imageName)
    {
        // 대소문자 구분 없이 파일 찾기 (무조건 소문자로 변환)
        Sprite newSprite = Resources.Load<Sprite>("Sprites/" + imageName.ToLower());

        if (newSprite != null)
        {
            portraitImage.sprite = newSprite;
            portraitImage.color = Color.white; 
            Debug.Log($"[System] 이미지 로드 성공: {imageName}");
        }
        else
        {
            Debug.LogError($"[Error] 이미지를 찾을 수 없습니다: Resources/Sprites/{imageName}");
        }
    }
    
    public void HideFace()
    {
        portraitImage.color = Color.clear;
    }
}