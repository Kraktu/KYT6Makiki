using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StuckChecker : MonoBehaviour
{
    [SerializeField] private BreakingWaveLauncher breakingWaveLauncher = null;
    [SerializeField] private UnityEvent onStuck = null;
    [SerializeField] private UnityEvent onEscaped = null;
    private List<Collider> currentCollisions;



    public void Reset()
    {
        gameObject.SetActive(true);
        currentCollisions.Clear();
    }



    private void Awake()
    {
        currentCollisions = new List<Collider>();
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
            if(currentCollisions.Count == 1)
            {
                onStuck.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == 8 || other.gameObject.layer == 9)
        {
            ExitObstacle(other);
        }
    }

    private void ExitObstacle(Collider other)
    {
        currentCollisions.Remove(other);
        if(currentCollisions.Count == 0)
        {
            onEscaped.Invoke();
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
