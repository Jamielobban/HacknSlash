using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MoreMountains.Feedbacks;
using System;

public class ControllerManager : MonoBehaviour
{
    public Gamepad controller;

    float delayCuadrado;
    float delayTriangulo;
    bool cuadradoHold;
    bool trianguloHold;

    public bool ataqueCuadrado;
    public bool ataqueCuadradoCargado;
    public bool ataqueTriangulo;
    public bool ataqueTrianguloCargado;


    public bool ataqueCuadradoCargadoL2;
    public bool ataqueTrianguloCargadoL2;
    public bool ataqueCuadradoL2;
    public bool ataqueTrianguloL2;


    public bool ataqueCuadradoPress;
    public bool ataqueCuadradoCargadoPress;
    public bool ataqueTrianguloPress;
    public bool ataqueTrianguloCargadoPress;


    public bool ataqueCuadradoCargadoL2Press;
    public bool ataqueTrianguloCargadoL2Press;
    public bool ataqueCuadradoL2Press;
    public bool ataqueTrianguloL2Press;



    bool dejarMantenerCuadrado;
    bool dejarMantenerTriangulo;

    bool dejarMantenerCuadradoL2;
    bool dejarMantenerTrianguloL2;

    bool jump;
    bool canJump;
    public bool dash;

    public bool MouseControl;
    Vector2 leftStick;
    Vector2 rightStick;
    InputAction.CallbackContext X;
    InputAction.CallbackContext O;
    public InputAction.CallbackContext Box;
    public InputAction.CallbackContext Triangle;
    public InputAction.CallbackContext Teleport;

    InputAction.CallbackContext R2;
    InputAction.CallbackContext L2;
    InputAction.CallbackContext Tab;
    InputAction.CallbackContext interact;

    Event squarePress;
    Event squareHold;

    public bool Teleport1 = false;
    public bool Teleport2 = false;
    float time;

    public PlayerCollision Interact;

    public Action OnSquarePress;
    public Action OnSquareHold;
    public Action OnTrianglePress;
    public Action OnTriangleHold;
    public void SquarePress() => OnSquarePress?.Invoke();
    public void SquareHold() => OnSquareHold?.Invoke();
    public void TrianglePress() => OnTrianglePress?.Invoke();
    public void TriangleHold() => OnTriangleHold?.Invoke();
    void Start()
    {
        leftStick = new Vector2();
        rightStick = new Vector2();

        dash = false;
        ataqueCuadrado = false;
        ataqueTriangulo = false;
        ataqueCuadradoCargado = false;
        ataqueTrianguloCargado = false;
        ataqueTrianguloCargadoL2 = false;
        ataqueCuadradoCargadoL2 = false;
        jump = false;
        canJump = true;
        dejarMantenerCuadrado = false;
        dejarMantenerTriangulo = false;
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            controller = Gamepad.all[i];
        }
    }

    public bool GetController()
    {
        return true;
    }
    public InputAction.CallbackContext GetTabButton()
    {
        return Tab;
    }
    public InputAction.CallbackContext GetInteractButton()
    {
        return interact;
    }
    public void GetLeftJoystick(InputAction.CallbackContext context)
    {
        leftStick = context.ReadValue<Vector2>();
    }
    public void GetTeleport(InputAction.CallbackContext context)
    {
        Teleport = context;
    }
    public void GetRightJoystick(InputAction.CallbackContext context)
    {
        rightStick = context.ReadValue<Vector2>();
    }
    public void GetR2(InputAction.CallbackContext context)
    {
        R2 = context;
    }
    public void GetInteract(InputAction.CallbackContext context)
    {
        interact = context;
    }

    public void GetTab(InputAction.CallbackContext context)
    {
        Tab = context;
    }
    public void GetL2(InputAction.CallbackContext context)
    {
        L2 = context;
    }
    public void GetX(InputAction.CallbackContext context)
    {
        X = context;
    }
    public void GetO(InputAction.CallbackContext context)
    {
        O = context;
    }
    public void GetBox(InputAction.CallbackContext context)
    {
        Box = context;
    }
    public void GetTriangle(InputAction.CallbackContext context)
    {
        Triangle = context;
    }
    public bool StartMove()
    {

        return leftStick.magnitude > 0.3f;



    }
    public Vector2 LeftStickValue()
    {

        return leftStick;



    }
    public Vector2 RightStickValue()
    {

        return rightStick;
    }
    public bool CheckIfPressed()
    {
        if (X.action == null)
        {
            return false;
        }
        return X.action.WasPressedThisFrame();
    }
    public bool RightTriggerPressed()
    {
        if (R2.action == null)
            return false;
        return R2.action.IsPressed();
    }
    public void ResetBotonesAtaques()
    {
        ataqueCuadrado = false;
        ataqueTriangulo = false;
        ataqueCuadradoCargado = false;
        ataqueTrianguloCargado = false;
        ataqueTrianguloCargadoL2 = false;
        ataqueCuadradoCargadoL2 = false;
        ataqueTrianguloL2 = false;
        ataqueCuadradoL2 = false;
        Teleport1 = false;
        Teleport2 = false;

        dash = false;
    }

    public bool CheckIfJump()
    {
        if (X.action == null)
            return false;
        if (X.action.WasPressedThisFrame() && canJump)
        {
            Interact.InteractPerformed();
            jump = true;
            canJump = false;
        }


        if (jump)
        {

            jump = false;
            canJump = true;

            return true;
        }
        return false;
    }

    public void ControlesAtaques()
    {
        //ataqueCuadrado = false;
        //ataqueTriangulo = false;
        //ataqueCuadradoCargado = false;
        //ataqueTrianguloCargado = false;

        if (Box.action != null)
        {
            if (Box.action.WasPressedThisFrame())
            {
                delayCuadrado = Time.time;
                cuadradoHold = true;


            }

            if (Box.action.WasReleasedThisFrame() && (Time.time - delayCuadrado) <= 0.25f)
            {
                ResetBotonesAtaques();
                if (L2.action != null)
                {
                    if (L2.action.IsPressed())
                    {
                        ataqueCuadradoL2 = true;
                        ataqueCuadradoL2Press = true;

                    }
                    else
                    {
                        ataqueCuadrado = true;
                        ataqueCuadradoPress = true;
                        SquarePress();
                    }
                }
                else
                {
                    ataqueCuadrado = true;
                    ataqueCuadradoPress = true;
                    SquarePress();

                }
            }
            if (Box.action.IsPressed() && (Time.time - delayCuadrado) > 0.25f && cuadradoHold)
            {
                ResetBotonesAtaques();

                if (L2.action != null)
                {
                    if (L2.action.IsPressed())
                    {
                        ataqueCuadradoCargadoL2 = true;
                        ataqueCuadradoCargadoL2Press = true;

                    }
                    else
                    {
                        ataqueCuadradoCargado = true;
                        ataqueCuadradoCargadoPress = true;
                        SquareHold();
                    }
                }
                else
                {
                    ataqueCuadradoCargado = true;
                    ataqueCuadradoCargadoPress = true;
                    SquareHold();
                }
                cuadradoHold = false;

            }
        }
        if (Triangle.action != null)
        {
            if (Triangle.action.WasPressedThisFrame())
            {
                delayTriangulo = Time.time;
                trianguloHold = true;

            }
            if (Triangle.action.WasReleasedThisFrame() && (Time.time - delayTriangulo) <= 0.25f)
            {
                ResetBotonesAtaques();
                if (L2.action != null)
                {
                    if (L2.action.IsPressed())
                    {
                        ataqueTrianguloL2 = true;
                        ataqueTrianguloL2Press = true;

                    }
                    else
                    {
                        ataqueTriangulo = true;
                        ataqueTrianguloPress = true;
                        TrianglePress();
                    }
                }
                else
                {
                    ataqueTriangulo = true;
                    ataqueTrianguloPress = true;
                    TrianglePress();
                }


            }

            if (Triangle.action.IsPressed() && (Time.time - delayTriangulo) > 0.25f && trianguloHold)
            {
                ResetBotonesAtaques();

                trianguloHold = false;
                if (L2.action != null)
                {
                    if (L2.action.IsPressed())
                    {
                        ataqueTrianguloCargadoL2 = true;
                        ataqueTrianguloCargadoL2Press = true;

                    }
                    else
                    {
                        ataqueTrianguloCargado = true;
                        ataqueTrianguloCargadoPress = true;
                        TriangleHold();
                    }
                }
                else
                {
                    ataqueTrianguloCargado = true;
                    ataqueTrianguloCargadoPress = true;
                    TriangleHold();
                }

            }
        }

        if (O.action != null)
        {
            if (O.action.WasPressedThisFrame())
            {
                ResetBotonesAtaques();

                dash = true;
            }
        }



    }

    bool holdTeleport = false;
    void Update()
    {
        Teleport2 = false;
        Teleport1 = false;

        ataqueCuadradoPress = false;
        ataqueTrianguloPress = false;
        ataqueCuadradoCargadoPress = false;
        ataqueTrianguloCargadoPress = false;
        ataqueTrianguloCargadoL2Press = false;
        ataqueCuadradoCargadoL2Press = false;
        ataqueTrianguloL2Press = false;
        ataqueCuadradoL2Press = false;

        if (X.action != null)
        {
            if (X.action.WasReleasedThisFrame() && !canJump)
            {
                jump = false;
                canJump = true;
            }
        }

        if (O.action != null)
        {
            if (L2.action != null)
            {
                if (L2.action.IsPressed() && O.action.IsPressed() && !holdTeleport)
                {
                    holdTeleport = true;
                    time = Time.time;
                }

                if ((Time.time - time) >= 0.25f && L2.action.IsPressed() && O.action.IsPressed())
                {
                    holdTeleport = false;

                    Teleport2 = true;
                }
                if ((Time.time - time) < 0.25f && (L2.action.IsPressed() && O.action.WasReleasedThisFrame()))
                {
                    holdTeleport = false;

                    Teleport1 = true;
                }
            }

        }


        ControlesAtaques();
    }



    public bool GetDash()
    {
        if (O.action == null)
            return false;
        if (L2.action == null)
        {
            return O.action.WasPressedThisFrame();
        }
        else
        {
            return O.action.WasPressedThisFrame() && !L2.action.IsPressed();

        }

    }




}
