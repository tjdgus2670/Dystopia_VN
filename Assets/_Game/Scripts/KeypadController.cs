using System.Collections;
using UnityEngine;
using TMPro;            
using Yarn.Unity;       

public class KeypadController : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject puzzlePanel;      
    public TMP_InputField inputField;   
    
    private string correctPassword;     
    private bool isPuzzleActive = false; 

    void Start()
    {
        var runner = FindObjectOfType<DialogueRunner>();
        if (runner != null)
        {
            runner.AddCommandHandler<string>("start_hack", StartHackRoutine);
        }
        
        // 키보드 입력을 스크립트에서 직접 처리하므로 입력창은 읽기 전용으로 유지
        inputField.readOnly = true; 
    }

    // [추가된 부분] 매 프레임마다 키보드가 눌렸는지 검사합니다.
    void Update()
    {
        // 퍼즐이 꺼져있으면 키보드 입력을 받지 않음
        if (!isPuzzleActive) return;

        // 1. 숫자 키 입력 (0~9) - 윗줄 숫자키 & 우측 숫자패드 모두 지원
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i) || Input.GetKeyDown(KeyCode.Keypad0 + i))
            {
                InputNumber(i.ToString());
            }
        }

        // 2. 지우기 (Backspace)
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            DeleteNumber();
        }

        // 3. 엔터 (Enter) - 입력 제출
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            CheckCode();
        }

        // 4. ESC (취소/닫기) - 원하시면 추가하세요
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePuzzle();
        }
    }

    // --- 기존 기능들 ---
    public void InputNumber(string number)
    {
        if (inputField.text.Length < 4) 
        {
            inputField.text += number;
        }
    }

    public void DeleteNumber()
    {
        if (inputField.text.Length > 0)
        {
            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
        }
    }

    private IEnumerator StartHackRoutine(string password)
    {
        correctPassword = password;
        inputField.text = "";           
        puzzlePanel.SetActive(true);    
        isPuzzleActive = true;

        while (isPuzzleActive)
        {
            yield return null; 
        }

        puzzlePanel.SetActive(false);
    }

    public void CheckCode()
    {
        if (inputField.text == correctPassword)
        {
            Debug.Log("해킹 성공!");
            FindObjectOfType<InMemoryVariableStorage>().SetValue("$hack_success", true);
            isPuzzleActive = false; 
        }
        else
        {
            Debug.Log("불일치!");
            inputField.text = ""; 
        }
    }

    public void ClosePuzzle()
    {
        Debug.Log("해킹 취소");
        FindObjectOfType<InMemoryVariableStorage>().SetValue("$hack_success", false);
        isPuzzleActive = false; 
    }
}