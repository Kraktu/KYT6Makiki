using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoofChecker : MonoBehaviour
{
    public bool IsRoofed => isRoofed;

    [SerializeField] private BreakingWaveLauncher breakingWaveLauncher = null;
    [SerializeField] private UnityEvent onRoofExited = null;
    private List<Collider> currentCollisions;
    private bool isRoofed;



    public void Reset()
    {
        currentCollisions.Clear();
    }



    private void Awake()
    {
        currentCollisions = new List<Collider>();
        isRoofed = false;
    }

    private void Start()
    {
        breakingWaveLauncher.OnObstacleBroken.AddListener(CheckBrokenObstacle);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8 || other.gameObject.layer == 9)
        {
            currentCollisions.Add(other);
            isRoofed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == 8 || other.gameObject.layer == 9)
        {
            ExitObstacle(other);
        }
    }

    private void ExitObstacle(Collider obstacle)
    {
        currentCollisions.Remove(obstacle);
        if(currentCollisions.Count == 0)
        {
            isRoofed = false;
            onRoofExited.Invoke();
        }
    }

    private void CheckBrokenObstacle(Collider brokenObstacle)
    {
        if(currentCollisions.Contains(brokenObstacle))
        {
            ExitObstacle(brokenObstacle);
        }
    }
}
