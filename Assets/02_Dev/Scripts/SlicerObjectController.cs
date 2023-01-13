using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Longhorn.Core;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class SlicerObjectController : Singleton<SlicerObjectController>
{
    [SerializeField] private SlicerPlacer slicerPositionObject;
    [SerializeField] private List<Ball> balls;
    [SerializeField] private LayerMask mask;
    [SerializeField] private MeshFilter msh;

    public static Action onSlashExecuted;
    private float levelFinishPercentage;

    private Camera cam;
    MeshVolumeCalculator meshVolumeCalculator = new MeshVolumeCalculator();
    private float totalMeshVolume;
    
    private void Start()
    {
        cam = Camera.main;
        LevelManager.OnLevelLoaded += OnNewPlatformLoaded;
        OnNewPlatformLoaded();
    }

    public void OnNewPlatformLoaded()
    {
        levelFinishPercentage = FindObjectOfType<LevelData>().GetLevelFinishPercentage();
        UIManager.I.SetLimitImagePosition(levelFinishPercentage);
        msh = FindObjectOfType<Platform>().GetComponent<MeshFilter>();
        totalMeshVolume = meshVolumeCalculator.CalculateMeshVolume(msh.mesh);
        UIManager.I.ResetProgressBar();
    }

    public void SetBalls()
    {
        balls.Clear();
        balls = FindObjectsOfType<Ball>().ToList();
    }

    public List<Ball> GetBalls()
    {
        return balls;
    }

    public bool IsSlicerBetweenBalls()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            Ray ray = new Ray(balls[0].transform.position, balls[i].transform.position - balls[0].transform.position);
            RaycastHit hit;
        
            if (Physics.Raycast(ray, out hit, Vector3.Distance(balls[0].transform.position, balls[i].transform.position - balls[0].transform.position), mask))
            {
                if (hit.collider.CompareTag(Keys.TAG_SLICER))
                    return true;
            }
        }
        

        return false;
    }

    public bool CheckLeftVolume(float leftVolume)
    {
        if (leftVolume < ((totalMeshVolume / 100) * levelFinishPercentage))
            return true;

        return false;
    }
    
    public void SetLeftVolumeUI(float leftVolume)
    {
        UIManager.I.SetLeftVolumeImage(leftVolume, totalMeshVolume);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GameManager.I.GetState() == GameState.Finished)
                return;
            
            slicerPositionObject.transform.position = cam.ScreenToWorldPoint(Input.mousePosition + (Vector3.forward * 10));
            slicerPositionObject.SetCollider(true, true);
        
        }
        else if (Input.GetMouseButton(0))
        {
            if (GameManager.I.GetState() == GameState.Finished)
                return;

            slicerPositionObject.transform.position = cam.ScreenToWorldPoint(Input.mousePosition + (Vector3.forward * 10));

        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (GameManager.I.GetState() == GameState.Finished)
                return;
        
            slicerPositionObject.SetCollider(false, false);
        }
    }

    private void OnDestroy()
    {
        LevelManager.OnLevelLoaded -= OnNewPlatformLoaded;
    }
}
