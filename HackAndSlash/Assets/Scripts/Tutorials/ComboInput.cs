using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Enums;

public abstract class ComboInput
{
    Image _sceneImage;
    public Sprite _inputSprite { get; private set; }
    protected PlayerControl _player;
    public Action OnInputDone; 
    public Action OnInputFailed;
    protected void InputDone() => OnInputDone?.Invoke(); 
    protected void InputFailed() => OnInputFailed?.Invoke();

    public ComboInput(Image sceneImage, Sprite inputSprite)
    {
        _sceneImage = sceneImage;
        _inputSprite = inputSprite;
        _player = GameManager.Instance.Player;
        OnInputDone += CorrectInput;
        OnInputFailed += IncorrectInput;
    }

    public abstract void StartListening(bool firstOfChain); 
    
    public abstract void EndListening(); 

    public void LoadImage()
    {
        _sceneImage.sprite = _inputSprite;
        if (!_sceneImage.enabled)
            _sceneImage.enabled = true;
        if (!_sceneImage.gameObject.activeSelf)
            _sceneImage.gameObject.SetActive(true);        

        DOVirtual.Color(_sceneImage.color, Color.white, 0.6f, (col) =>
        {
            _sceneImage.color = col;
        }).SetEase(Ease.InOutSine);
    }

    private void CorrectInput()
    {
        AudioManager.Instance.PlayDelayFx(Enums.Effects.PositiveClickTuto, 0);
        DOVirtual.Color(_sceneImage.color, Color.green, 1f, (color) =>
        {
            _sceneImage.color = color;
        }).SetEase(Ease.InOutSine);
    }

    private void IncorrectInput()
    {
        AudioManager.Instance.PlayDelayFx(Effects.ErrorButton, 0);
        DOVirtual.Color(_sceneImage.color, Color.red, 1f, (color) =>
        {
            _sceneImage.color = color;
        }).SetEase(Ease.InOutSine);
    }

    ~ComboInput()    
    {
        OnInputDone -= CorrectInput;
        OnInputFailed -= IncorrectInput;
    }
}
