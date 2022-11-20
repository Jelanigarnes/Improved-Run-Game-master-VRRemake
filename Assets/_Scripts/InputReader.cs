using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputReader : MonoBehaviour
{
    public PlayerControVR inputAction;

    public static InputReader controller;

    private void OnEnable()
    {
        inputAction.Enable();
    }

    private void OnDisable()
    {
        inputAction.Disable();
    }


    void Awake()
    {
        //instance
        if (controller == null)
        {
            controller = this;
        }

        inputAction = new PlayerControVR();
    }

}
