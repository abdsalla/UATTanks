using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class AIController 
{
    public Color playerColor;
    public Transform spawnPoint;
    [HideInInspector] public int playerNumber;
    [HideInInspector] public int wins;
    [HideInInspector] public string coloredPlayerText;
    [HideInInspector] public GameObject instance;

    private GameObject canvasGameObject;
    private Tank_Data tank_DataRef;
    private Firing firingRef;


    public void Setup()
    {
        tank_DataRef =instance.GetComponent<Tank_Data>();
        firingRef = instance.GetComponent<Firing>();
        canvasGameObject = instance.GetComponentInChildren<Canvas>().gameObject;

        tank_DataRef.playerNumber = playerNumber;
        firingRef.playerNumber = playerNumber;

        coloredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(playerColor) + ">PLAYER " + playerNumber + "</color>";

        MeshRenderer[] renderers = instance.GetComponentsInChildren<MeshRenderer>();

        for (int i =0; i < renderers.Length; i++)
        {
            renderers[i].material.color = playerColor;
        }

    }

    public void DisableControl()
    {
        tank_DataRef.enabled = false;
        firingRef.enabled = false;

        canvasGameObject.SetActive(false);
    }

    public void EnableControl()
    {
        tank_DataRef.enabled = true;
        firingRef.enabled = true;

        canvasGameObject.SetActive(true);
    }
    public void Reset()
    {
        instance.transform.position = spawnPoint.position;
        instance.transform.rotation = spawnPoint.rotation;

        instance.SetActive(false);
        instance.SetActive(true);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

}
