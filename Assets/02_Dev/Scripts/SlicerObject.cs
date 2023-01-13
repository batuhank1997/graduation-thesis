using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using EzySlice;
using Managers;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class SlicerObject : MonoBehaviour
{
    [SerializeField] private bool canSlice;
    [SerializeField] private float upForce;
    [SerializeField] private float forwardForce;
    [SerializeField] private float torque;
    
    [SerializeField] private Material slicedMaterial;
    private MeshVolumeCalculator meshVolumeCalculator;

    private Vector3 fallForceDir;

    private void Start()
    {
        meshVolumeCalculator = new MeshVolumeCalculator();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out SliceableBase sliceable))
        {
            if (!canSlice)
                return;
            
            if (SlicerObjectController.I.IsSlicerBetweenBalls())
                return;

            canSlice = false;
            // slicedMaterial = sliceable.GetMaterial();

            Slash(sliceable.gameObject);
        }
    }

    void Slash(GameObject slice)
    {
        CreateHulls(slice);

        PlayFXs();
        
        SlicerObjectController.onSlashExecuted?.Invoke();
        Destroy(slice);
    }

    void CreateHulls(GameObject slice)
    {
        SlicedHull hull = Slice(slice, slicedMaterial);
            
        if (hull == null)
            return;
        
        GameObject slicedObjectTop = hull.CreateUpperHull(slice, slicedMaterial);
        GameObject slicedObjectBottom = hull.CreateLowerHull(slice, slicedMaterial);

        var topVolume = meshVolumeCalculator.CalculateMeshVolume(slicedObjectTop.GetComponent<MeshFilter>().mesh);
        var bottomVolume = meshVolumeCalculator.CalculateMeshVolume(slicedObjectBottom.GetComponent<MeshFilter>().mesh);
            
        slicedObjectBottom.transform.SetParent(FindObjectOfType<LevelGenerator>().levelParent);
        slicedObjectTop.transform.SetParent(FindObjectOfType<LevelGenerator>().levelParent);
            
        slicedObjectTop.AddComponent<MeshCollider>();
        slicedObjectBottom.AddComponent<MeshCollider>();
        
        slicedObjectTop.tag = "Hull";
        slicedObjectBottom.tag = "Hull";

        var balls = SlicerObjectController.I.GetBalls();

        //Check which hull shall remain
        
        var colliders = Physics.OverlapSphere(balls[0].transform.position + (Vector3.forward * -0.25f), 0.1f);
        
        colliders.ForEach(collider => 
        {
            if (collider.CompareTag("Hull"))
            {
                if (collider.name == slicedObjectTop.name)
                {
                    Destroy(slicedObjectTop.GetComponent<MeshCollider>());
                    slicedObjectBottom.layer = 7;
                    slicedObjectBottom.GetComponent<MeshCollider>().convex = true;
                    MakeSliceable(slicedObjectTop, topVolume);
                    Fall(slicedObjectBottom);
                }
                else
                {
                    slicedObjectTop.layer = 7;
                    slicedObjectTop.GetComponent<MeshCollider>().convex = true;
                    Destroy(slicedObjectBottom.GetComponent<MeshCollider>());
                    MakeSliceable(slicedObjectBottom, bottomVolume);
                    Fall(slicedObjectTop);
                }
            }
        });
    }

    void PlayFXs()
    {
        iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);
        CameraController.I.SuccessCamShake();
        AudioController.I.PlaySlashFX();
    }

    private SlicedHull Slice(GameObject _obj, Material _mat)
    {
        return _obj.Slice(transform.position, transform.up, _mat);
    }

    void MakeSliceable(GameObject remainingObject, float leftVolume)
    {
        SlicerObjectController.I.SetLeftVolumeUI(leftVolume);
        if (SlicerObjectController.I.CheckLeftVolume(leftVolume))
        {
            GameManager.I.SetState(GameState.Finished);
            UIManager.I.SetState(UIState.Won);
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Success);
            return;
        }

        remainingObject.layer = 7;
        remainingObject.AddComponent<Platform>();
        remainingObject.tag = Keys.TAG_PLATFORM;
        var col = remainingObject.AddComponent<MeshCollider>();
        col.sharedMesh = remainingObject.GetComponent<MeshFilter>().mesh;
        remainingObject.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
    }

    void Fall(GameObject obj)
    {
        var rb = obj.AddComponent<Rigidbody>();
        rb.AddForce(fallForceDir + (Vector3.back * forwardForce) + (Vector3.up * upForce), ForceMode.Impulse);
        rb.AddTorque(torque * -Vector3.right, ForceMode.Impulse);
    }

    public void SetForceDirection(Vector3 dir)
    {
        fallForceDir = dir;
    }
}