using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingWaveController : MonoBehaviour
{
    public bool CanLaunch { get => canLaunch; set => canLaunch = value; }

    [SerializeField] private BreakingWaveLauncher breakingWaveLauncher = null;
    private List<GameObject> brokenObstacles;
    private bool canLaunch;



    public void LaunchBreakingWave()
    {
        if(breakingWaveLauncher.gameObject.activeInHierarchy)
        {
            return;
        }

        if(!canLaunch)
        {
            return;
        }

        breakingWaveLauncher.gameObject.SetActive(true);
        breakingWaveLauncher.Launch();
    }

    public void MemorizeBrokenObstacle(Collider brokenObstacle)
    {
        brokenObstacles.Add(brokenObstacle.gameObject);
    }

    public void ResetBrokenObstacles()
    {
        foreach(GameObject brokenObstacle in brokenObstacles)
        {
            brokenObstacle.SetActive(true);
        }
        brokenObstacles.Clear();
    }



    private void Awake()
    {
        brokenObstacles = new List<GameObject>();
        canLaunch = true;
    }

    private void Start()
    {
        breakingWaveLauncher.OnObstacleBroken.AddListener(MemorizeBrokenObstacle);
    }
}
