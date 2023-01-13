using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using Sirenix.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private ParticleSystem ballExplosion;
    [SerializeField] private int ballCount;
    [SerializeField] private float speed;
    [SerializeField] private bool isOnBoarding;
    [Range(-1,1)]
    [SerializeField] private int onBoardingDirection;
    
    private float _speed;
    private Vector3 _direction;
    private Rigidbody rb;
    private Transform levelParent;
    private float bounceTimer = 0.0f;
    private bool canBounce = true;
    
    private void Start()
    {
        GameManager.OnStateChange += OnFinish;
        rb = GetComponent<Rigidbody>();
        
        if (isOnBoarding)
        {
            SetVelocity(new Vector3(onBoardingDirection * 1, 0, 0));
        }
        else
        {
            SetVelocity(new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0));
        }
        
        levelParent = FindObjectOfType<LevelGenerator>().levelParent;
    }

    void SetVelocity(Vector3 newVelocity)
    {
        _direction = newVelocity.normalized;
    }

    public IEnumerator EnterSlowMotion()
    {
        speed /= 4f;
        SetVelocity(rb.velocity);
        yield return new WaitForSeconds(2f);
        speed *= 4f;
        SetVelocity(rb.velocity);
    }
    
    public void Explode()
    {
        StartCoroutine(InstantiateFailBalls());
    }
    
    void OnFinish()
    {
        SetVelocity(Vector3.zero);
    }

    private void Update()
    {
        _speed = speed;
        rb.velocity = _direction.normalized * _speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag(Keys.TAG_EDGE) || collision.transform.CompareTag(Keys.TAG_OBSTACLE))
        {
            //bounce physics
            if (!canBounce)
                return;

            canBounce = false;

            var direction = Vector3.Reflect(rb.velocity.normalized, collision.contacts[0].normal);
            
            _direction = direction;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.transform.CompareTag(Keys.TAG_EDGE) || other.transform.CompareTag(Keys.TAG_OBSTACLE))
        {
            canBounce = true;
        }
    }

    IEnumerator InstantiateFailBalls()
    {
        AudioController.I.PlayFailFX();
        
        CameraController.I.FailCamShake();
        
        for (int i = 0; i < ballCount; i++)
        {
            GameObject newBall = Instantiate(ballPrefab, transform.position, Quaternion.identity);
            newBall.transform.localScale = Vector3.one * 0.5f;
            newBall.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0).normalized * 2.5f;
            newBall.transform.SetParent(levelParent);
        }
        
        GameManager.I.SetState(GameState.Finished);
        transform.GetChild(0).gameObject.SetActive(false);
        
        yield return new WaitForSeconds(2f);
        UIManager.I.SetState(UIState.Lost);
        
    }

    public void PlaySplashFX()
    {
        ballExplosion.transform.SetParent(levelParent);
        ballExplosion.Play();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (SlicerObjectController.I.GetBalls().IsNullOrEmpty())
            return;

        SlicerObjectController.I.GetBalls().Remove(this);
        GameManager.OnStateChange -= OnFinish;
    }
}
