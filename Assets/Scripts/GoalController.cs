using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{

    [SerializeField] private MeshRenderer flagPoleMesh;

    private GolfBallController _golfBallController;

    private void Start()
    {
        _golfBallController = FindObjectOfType<GolfBallController>();
    }

    private void Update()
    {
        if (_golfBallController)
        {
            float dist = Vector3.Distance(transform.position, _golfBallController.transform.position);
            float distVal = (dist - 1) / 3;
            float distToCam = Vector3.Distance(transform.position, Camera.main.transform.position);
            float distToCamVal = (distToCam - 2) / 3;

            flagPoleMesh.material.SetColor("_MainColor", new Color(1, 1, 1, (distVal < distToCamVal) ? distVal : distToCamVal));
        }
    }
}
