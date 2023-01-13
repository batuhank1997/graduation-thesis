using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

public class SlicerPlacer : MonoBehaviour
{
    [SerializeField] private GameObject slicerPrefab;
    [SerializeField] private GameObject edgeCollider;
    [Space]
    [Header("FX")]
    [SerializeField] private GameObject trail;
    [SerializeField] private LineRenderer line;
    [SerializeField] private GameObject obsFailFXPrefab;
    [Space]
    [Header("DEBUG")]
    [SerializeField] private bool debugMode;
    private GameObject b;
    private GameObject i;
    
    private Vector3 firstPos;
    private Vector3 firstPosOldPos;
    private Vector3 lastPos;
    
    private GameObject firstCollider;
    private GameObject lastCollider;

    private bool canSlice = true;
    private bool safeMode = false;

    private void Start()
    {
        if (debugMode)
        {
            b = GameObject.CreatePrimitive(PrimitiveType.Cube);
            i = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        
            b.transform.localScale = Vector3.one * 0.2f;
            Destroy(b.GetComponent<Collider>());
            i.transform.localScale = Vector3.one * 0.2f;
            Destroy(i.GetComponent<Collider>());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out SliceableBase sliceable) && !safeMode)
        {
            safeMode = true;
            print("ENTER");
            firstPos = (transform.position) + Vector3.forward;
            
            if (debugMode)
                b.transform.position = firstPos;
        }
        if (other.TryGetComponent(out Ball ball))
        {

            if (BonusController.I.GetShieldStatus())
            {
                CameraController.I.FailCamShake();
                BonusController.I.RemoveShield();
                return;
            }
            
            ball.Explode();
        }
        if (other.TryGetComponent(out ObstacleEdge obstacleEdge))
        {
            CameraController.I.FailCamShake();
            SetCollider(false, false);
            safeMode = false;
            canSlice = false;
        }
        if (other.TryGetComponent(out Obstacle obstacle))
        {
            CameraController.I.FailCamShake();
            SetCollider(false);
            PlayObstacleFailFX();
            
            if (BonusController.I.GetShieldStatus())
            {
                BonusController.I.RemoveShield();
                return;
            }
            
            GameManager.I.SetState(GameState.Finished);
            UIManager.I.SetState(UIState.Lost);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out SliceableBase sliceable) && safeMode)
        {
            print("EXIT");
            safeMode = false;
            if (Vector3.Distance(firstPos, transform.position) < .2)
                return;

            lastPos = (transform.position) + Vector3.forward;

            if (debugMode)
                i.transform.position = lastPos;

            if (Vector3.Distance(firstPos, lastPos) < .2)
                return;
            
            PlaceSlicer();
        }
    }

    void PlaceSlicer()
    {
        print("CUT");
        var angle = SetPosAndAngle().Item2;
        var InstantiatePos = SetPosAndAngle().Item1 + (Vector3.back);

        if (firstPos == firstPosOldPos)
            return;
        
        firstPosOldPos = firstPos;
        
        var slicer = Instantiate(slicerPrefab, InstantiatePos,
            Quaternion.Euler(new Vector3(0, 0, angle)));

        slicer.GetComponent<SlicerObject>().SetForceDirection(lastPos - firstPos);

        // slicer.transform.localScale = new Vector3(0, 0, 0);
        
        if (SlicerObjectController.I.IsSlicerBetweenBalls())
        {
            CreateLineWarning(firstPos, lastPos);
            Destroy(slicer);
            return;
        }
        
        CreateNewEdgeCollider(InstantiatePos, angle);
        slicer.transform.SetParent(FindObjectOfType<LevelGenerator>().levelParent);
    }
    
    void CreateLineWarning(Vector3 startPoint, Vector3 endPoint)
    {
        LineRenderer _line = new LineRenderer();
        _line = line;
        _line.transform.position = Vector3.back * 0.5f;
        _line.SetPosition(0, startPoint + Vector3.back);
        _line.SetPosition(1, endPoint + Vector3.back);
        CameraController.I.FailCamShake();
        StartCoroutine(LineBlink(_line));
    }

    IEnumerator LineBlink(LineRenderer lr)
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.1f);
            lr.enabled = !lr.enabled;
        }
    }

    void CreateNewEdgeCollider(Vector3 pos, float angle)
    {
        var newEdge = Instantiate(edgeCollider, pos,
            Quaternion.Euler(new Vector3(0, 0, angle)));
        
        newEdge.transform.SetParent(FindObjectOfType<LevelGenerator>().levelParent);
    }

    (Vector3, float) SetPosAndAngle()
    {
        var angle = 0f;
        var InstantiatePos = Vector3.zero;
        
        if (firstPos.y < lastPos.y && firstPos.x < lastPos.x)
        {
            angle = Vector3.Angle((lastPos - firstPos).normalized, Vector3.right);
            InstantiatePos = firstPos;
        }
        else if (firstPos.y < lastPos.y && firstPos.x > lastPos.x)
        {
            angle = Vector3.Angle((lastPos - firstPos).normalized, Vector3.right);
            InstantiatePos = firstPos;
        }
        else if (firstPos.y > lastPos.y && firstPos.x < lastPos.x)
        {
            angle = Vector3.Angle((firstPos - lastPos).normalized, Vector3.right);
            InstantiatePos = lastPos;
        }
        else if (firstPos.y > lastPos.y && firstPos.x > lastPos.x)
        {
            angle = Vector3.Angle((lastPos - firstPos).normalized, Vector3.left);
            InstantiatePos = firstPos;
        }
        
        return (InstantiatePos, angle);
    }
    
    public void SetCollider(bool isTrue, bool mouseDown = true)
    {
        GetComponent<Collider>().enabled = isTrue;
        
        if (mouseDown)
            trail.GetComponent<ParticleSystem>().Play();
        else
        {
            trail.GetComponent<ParticleSystem>().Stop();
            safeMode = false;
        }
    }

    void PlayObstacleFailFX()
    {
        Instantiate(obsFailFXPrefab, transform.position, Quaternion.identity);
        iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Failure);
    }
}
