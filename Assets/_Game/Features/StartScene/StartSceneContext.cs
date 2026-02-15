using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneContext : MonoBehaviour
{
    private static StartSceneContext _instance;
    public static StartSceneContext Instance => _instance;
    
    private void Awake()
    {
        _instance ??= this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}