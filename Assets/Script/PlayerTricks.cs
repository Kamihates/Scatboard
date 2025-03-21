using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTricks : MonoBehaviour
{
    private string[] _inputTrick = new string[]{"W","A","S","D"};
    private bool _isTrick = false;
    private float _timeTricks = 0;
    private bool _randomTimeTricksSet = false;
    private float _randomTimeTricks;
    private int _currentInputTrickIndex = 0;
    public GameObject up;
    public GameObject left;
    public GameObject down;
    public GameObject right;
    private List<string> _chosenInput;

    private void Update()
    {
        if (!_randomTimeTricksSet)
        {    
            _randomTimeTricks = Random.Range(9f, 15f);
            _randomTimeTricksSet = true;
        }
        _timeTricks += Time.deltaTime;
        if (_timeTricks > _randomTimeTricks)
        {
            _timeTricks = 0;
            _randomTimeTricksSet = false;
            TricksStart();
        }
    }
    
    private void TricksInitialize()
    {
        int randomNumber = Random.Range(3, 6);
        _chosenInput = new List<string>();
        for (int i = 0; i < randomNumber; i++)
        {
            int randomIndex = Random.Range(0, _inputTrick.Length);
            _chosenInput.Add(_inputTrick[randomIndex]);
        }
        _currentInputTrickIndex = 0;
        _isTrick = true;
        HideInput();
        ShowInput(_chosenInput, _currentInputTrickIndex);
    }

    private void TricksStart()
    {
        Time.timeScale = 0.25f;
        TricksInitialize();
        StartCoroutine(EndTricks());
    }

    public void Tricks(InputAction.CallbackContext context)
    {
        if (!_isTrick)
            return;

        if (context.performed)
        {
            string inputKey = context.control.name.ToUpper();
            Debug.Log("Touche à cliquer : " + _chosenInput[_currentInputTrickIndex]);
            if (inputKey == _chosenInput[_currentInputTrickIndex])
            {
                _currentInputTrickIndex++;
                if (_currentInputTrickIndex >= _chosenInput.Count)
                {
                    _isTrick = false;
                    Time.timeScale = 1f;
                    HideInput();
                    Debug.Log("Tricks réussi");
                    return;
                }
                HideInput();
                ShowInput(_chosenInput, _currentInputTrickIndex);
            }
        }
    }

    private void ShowInput(List<string> chosenInput, int _currentInputTrickIndex)
    {
        switch(chosenInput[_currentInputTrickIndex])
        {
            case "W":
                up.SetActive(true);
                break;
            case "A":
                left.SetActive(true);
                break;
            case "S":
                down.SetActive(true);
                break;
            case "D":
                right.SetActive(true);
                break;
        }
    }

    private void HideInput()
    {
        up.SetActive(false);
        left.SetActive(false);
        down.SetActive(false);
        right.SetActive(false);
    }

    private IEnumerator EndTricks()
    {
        yield return new WaitForSecondsRealtime(4f);
        Time.timeScale = 1f;
    }

    private IEnumerator Wait2s()
    {
        yield return new WaitForSecondsRealtime(2f);
    }
}
