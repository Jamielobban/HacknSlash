using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MoreMountains.Feedbacks;

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
    Vector2 dPad;
    InputAction.CallbackContext X;
    InputAction.CallbackContext O;
    public InputAction.CallbackContext Box;
    public InputAction.CallbackContext Triangle;
    public InputAction.CallbackContext Teleport;

    //InputAction.CallbackContext dPad;


    InputAction.CallbackContext R2;
    InputAction.CallbackContext L2;
    InputAction.CallbackContext Tab;
    InputAction.CallbackContext interact;

    public bool Teleport1 = false;
    public bool Teleport2 = false;
    float time;


    // Start is called before the first frame update
    void Start()
    {
        leftStick = new Vector2();
        rightStick = new Vector2();
        dPad = new Vector2();

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
    public void GetDpad(InputAction.CallbackContext context)
    {
        leftStick = context.ReadValue<Vector2>();
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

    public Vector2 GetDpadValue()
    {
        return dPad;
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
    public bool CheckIfPressed()
    {
        if (X.action == null)
        {
            return false;
        }
        return X.action.WasPressedThisFrame();
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

                    }
                }
                else
                {
                    ataqueCuadrado = true;
                    ataqueCuadradoPress = true;

                }
            }

            //if (Box.action.WasReleasedThisFrame() && (Time.time - delayCuadrado) <= 0.35f)
            //{
            //    ResetBotonesAtaques();
            //    if (L2.action != null)
            //    {
            //        if (L2.action.IsPressed())
            //        {
            //            ataqueCuadradoL2 = true;
            //            ataqueCuadradoL2Press = true;

            //        }
            //        else
            //        {
            //            ataqueCuadrado = true;
            //            ataqueCuadradoPress = true;

            //        }
            //    }
            //    else
            //    {
            //        ataqueCuadrado = true;
            //        ataqueCuadradoPress = true;

            //    }
            //}
            //if (Box.action.IsPressed() && (Time.time - delayCuadrado) > 0.35f && cuadradoHold)
            //{
            //    ResetBotonesAtaques();

            //    if (L2.action != null)
            //    {
            //        if(L2.action.IsPressed())
            //        {
            //            ataqueCuadradoCargadoL2 = true;
            //            ataqueCuadradoCargadoL2Press = true;

            //        }
            //        else
            //        {
            //            ataqueCuadradoCargado = true;
            //            ataqueCuadradoCargadoPress = true;

            //        }
            //    }
            //    else
            //    {
            //        ataqueCuadradoCargado = true;
            //        ataqueCuadradoCargadoPress = true;

            //    }
            //    cuadradoHold = false;

            //}
        }
        if (Triangle.action != null)
        {
            if (Triangle.action.WasPressedThisFrame())
            {
                delayTriangulo = Time.time;
                trianguloHold = true;
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

                    }
                }
                else
                {
                    ataqueTriangulo = true;
                    ataqueTrianguloPress = true;

                }
            }
            //if (Triangle.action.WasReleasedThisFrame() && (Time.time - delayTriangulo) <= 0.35f)
            //{
            //    ResetBotonesAtaques();
            //    if (L2.action != null)
            //    {
            //        if (L2.action.IsPressed())
            //        {
            //            ataqueTrianguloL2 = true;
            //            ataqueTrianguloL2Press = true;

            //        }
            //        else
            //        {
            //            ataqueTriangulo = true;
            //            ataqueTrianguloPress = true;

            //        }
            //    }
            //    else
            //    {
            //        ataqueTriangulo = true;
            //        ataqueTrianguloPress = true;

            //    }


            //}

            //if (Triangle.action.IsPressed() && (Time.time - delayTriangulo) > 0.35f && trianguloHold)
            //{
            //    ResetBotonesAtaques();

            //    trianguloHold = false;
            //    if (L2.action != null)
            //    {
            //        if (L2.action.IsPressed())
            //        {
            //            ataqueTrianguloCargadoL2 = true;
            //            ataqueTrianguloCargadoL2Press = true;

            //        }
            //        else
            //        {
            //            ataqueTrianguloCargado = true;
            //            ataqueTrianguloCargadoPress = true;

            //        }
            //    }
            //    else
            //    {
            //        ataqueTrianguloCargado = true;
            //        ataqueTrianguloCargadoPress = true;

            //    }

            //}
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
    // Update is called once per frame
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
