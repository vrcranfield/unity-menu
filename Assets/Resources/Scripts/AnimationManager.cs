﻿using UnityEngine;


public class AnimationManager : MonoBehaviour
{
    private bool isPlaying = false;

    private GameObject obj;
    private int currentFrame = 0;
	private int delay = 0;

	const int DELAY_COUNT = 2;

    void Awake()
    {
        Globals.ParaviewObjectLoadedCallbacks += new Globals.CallbackEventHandler(OnParaviewObjectLoaded);
        Globals.animation = this;
    }

    void Start()
    {
        ShowNextFrame();
    }

    void Update()
    {
        if (isPlaying)
        {
            if (delay >= DELAY_COUNT)
            {
                delay = 0;
                ShowNextFrame();
            }
            else
            {
                delay++;
            }
        }
    }

    public void OnParaviewObjectLoaded(GameObject paraviewObj)
    {
        this.obj = Globals.paraviewObj.transform.FindDeepChild("FramedObject").gameObject;
    }

    public void Play()
    {
        if(isObjectLoaded())
            isPlaying = true;
    }

    public void Pause()
    {
        if(isObjectLoaded())
            isPlaying = false;
    }

    private void ShowNextFrame()
    {
        if(isObjectLoaded())
        {
            foreach (Transform child in obj.transform)
            {
                child.gameObject.SetActive(false);
            }
            obj.transform.GetChild(currentFrame).gameObject.SetActive(true);
            currentFrame = (currentFrame + 1) % obj.transform.childCount;
        }
    }

    public bool isAnimation()
    {
        return isObjectLoaded() && obj.transform.childCount > 1;
    }

    public bool isObjectLoaded()
    {
        return obj != null;
    }

    public bool isAnimationPlaying()
    {
        return isPlaying;
    }
}
