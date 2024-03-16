using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class ComboInput
{
    Image _sceneImage;
    public Sprite _inputSprite { get; private set; }
    protected PlayerControl _player;
    public Action OnInputDone; //Aqui es ficara la funció de color a verd
    public Action OnInputFailed; //Aqui es ficara la funció de color a vermell
    protected void InputDone() => OnInputDone?.Invoke(); //Aqui es ficara la funció de color a verd
    protected void InputFailed() => OnInputFailed?.Invoke();//Aqui es ficara la funció de color a vermell

    public ComboInput(Image sceneImage, Sprite inputSprite)
    {
        _sceneImage = sceneImage;
        _inputSprite = inputSprite;
        _player = GameManager.Instance.Player;
        OnInputDone += CorrectInput;
        OnInputFailed += IncorrectInput;
    }

    public abstract void StartListening(); //Action de ControllerManager += InputDone;
    
    public abstract void EndListening(); //Action de ControllerManager -= InputDone;

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
        DOVirtual.Color(_sceneImage.color, Color.green, 1f, (color) =>
        {
            _sceneImage.color = color;
        }).SetEase(Ease.InOutSine);
    }

    private void IncorrectInput()
    {
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
