﻿using UnityEngine;
using VRTK;

public class ControllerBehaviour : MonoBehaviour
{
    private GameObject menu;
    private GameObject headset;
    private GameObject radialMenu;

    public bool isRadialMenuController;

    private void Awake()
    {
        headset = GameObject.FindGameObjectWithTag("Headset");
        radialMenu = transform.Find("RadialMenu").gameObject;
    }

    private void Start()
    {
        menu = GlobalVariables.staticMenu;

        //Setup controller event listeners for Menu button
        GetComponent<VRTK_ControllerEvents>().ButtonTwoReleased += new ControllerInteractionEventHandler(DoButtonTwoReleased);
    }

    private void DoButtonTwoReleased(object sender, ControllerInteractionEventArgs e)
    {
        if(menu.activeSelf)
        {
            Debug.Log("Hiding Menu");
            menu.SetActive(false);
        } else
        {
            Debug.Log("Showing Menu");

            // Get the XZ projection of the forward position of the headset camera
            Vector3 lookForwardPositionXZ = Vector3.ProjectOnPlane(headset.transform.forward, new Vector3(0, 1, 0));

            // Gets the position at exactly 2.5 meters in front of the headset, along the XZ plane.
            Vector3 headsetFrontPosition = headset.transform.position + 2.5f * lookForwardPositionXZ.normalized;

            // Sets the menu position to headsetFrontPosition, but at the same height as before.
            menu.transform.position = new Vector3(headsetFrontPosition.x, menu.transform.position.y, headsetFrontPosition.z);

            // Set the rotation so that the menu faces the camera
            menu.transform.rotation = Quaternion.LookRotation(menu.transform.position - headset.transform.position);

            // Show menu
            menu.SetActive(true);
        }
    }

    public void SetControllerMode(bool isRadialMenu)
    {
        radialMenu.SetActive(isRadialMenu);
        isRadialMenuController = isRadialMenu;
    }
}
