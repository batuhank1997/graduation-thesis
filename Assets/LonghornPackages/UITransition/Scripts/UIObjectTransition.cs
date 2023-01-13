using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public enum TransitionType
{
    Scale,
    Fade,
    Slide,
}

public enum SlideDirection
{
    Up,
    Down,
    Right,
    Left,
    Custom
}

public class UIObjectTransition : MonoBehaviour
{
    [SerializeField] private bool turnOnAtStart;
    [SerializeField] private float turnOnDelayTime = 1f;

    //https://easings.net/ Check the link to examine all ease types.
    [HideIf("useCustomAnimCurve")] [SerializeField]
    private Ease animEase;

    [SerializeField] private bool useCustomAnimCurve;

    [ShowIf("useCustomAnimCurve")] [SerializeField]
    private AnimationCurve customAnimCurve;

    [Header("Turn On")] [Space(5)] [SerializeField]
    private TransitionType turnOnTransition;

    [ShowIf("turnOnTransition", TransitionType.Scale)] [SerializeField]
    private float turnOnScaleValue = 1f;

    [ShowIf("turnOnTransition", TransitionType.Slide)] [SerializeField]
    private SlideDirection slideInFrom;

    [ShowIf("@this.slideInFrom == SlideDirection.Custom && this.turnOnTransition == TransitionType.Slide")]
    [SerializeField]
    private Transform customSlideInPoint;

    [ShowIf("turnOnTransition", TransitionType.Fade)] [SerializeField]
    private float turnOnFadeValue = 1f;

    [SerializeField] private float turnOnDuration = 0.5f;

    [Header("Turn Off")] [SerializeField] private TransitionType turnOffTransition;

    [ShowIf("turnOffTransition", TransitionType.Slide)] [SerializeField]
    private SlideDirection slideOutTo;

    [ShowIf("@this.slideOutTo == SlideDirection.Custom && this.turnOffTransition == TransitionType.Slide")]
    [SerializeField]
    private Transform customSlideOutPoint;

    [SerializeField] private float turnOffDuration = 0.5f;

    private bool isInitPosSet;
    private bool isSlidePosSet;
    private Vector3 initPos;
    private Vector3 afterSlidePos;
    private Vector3 beforeSlidePos;

    private void Awake()
    {
        if (isInitPosSet) return;
        isInitPosSet = true;
        initPos = transform.position;
    }

    private void OnEnable()
    {
        SetStartSettings();

        if (turnOnAtStart)
        {
            TurnOnTransitionWithDelay();
        }
    }

    #region Turn On

    [Button("TURN ON TRANSITION")]
    public void TurnOnTransitionWithDelay()
    {
        Invoke(nameof(TurnOnTransition), turnOnDelayTime);
    }

    public void TurnOnTransition()
    {
        gameObject.SetActive(true);
        switch (turnOnTransition)
        {
            case TransitionType.Scale:
                ScaleUpAnim();
                break;
            case TransitionType.Fade:
                FadeAnim(true);
                break;
            case TransitionType.Slide:
                SlideInAnim();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ScaleUpAnim()
    {
        transform.localScale = Vector3.zero;

        if (useCustomAnimCurve)
        {
            transform.DOScale(turnOnScaleValue, turnOnDuration).SetEase(customAnimCurve);
        }
        else
        {
            transform.DOScale(turnOnScaleValue, turnOnDuration).SetEase(animEase);
        }
    }

    private void SetPosBeforeSlide()
    {
        Vector3 direction = slideInFrom switch
        {
            SlideDirection.Down => Vector3.down,
            SlideDirection.Left => Vector3.left,
            SlideDirection.Up => Vector3.up,
            SlideDirection.Right => Vector3.right,
            _ => Vector3.zero
        };

        Vector3 startOffset;

        if (slideInFrom == SlideDirection.Left || slideInFrom == SlideDirection.Right)
        {
            startOffset = (GetComponent<RectTransform>().rect.width + Screen.width) * direction;
        }
        else
        {
            startOffset = (GetComponent<RectTransform>().rect.height + Screen.height) * direction;
        }

        Vector3 targetPos = initPos;

        if (turnOnTransition != TransitionType.Slide) beforeSlidePos = initPos;

        if (slideInFrom == SlideDirection.Custom)
        {
            if (customSlideInPoint == null)
            {
                Debug.LogWarning("Assign a 'customSlideInPoint' to use custom slide mode !");
                beforeSlidePos = targetPos + startOffset;
            }
            else
            {
                beforeSlidePos = customSlideInPoint.position;
            }
        }
        else
        {
            beforeSlidePos = targetPos + startOffset;
        }
    }

    private void SetPosAfterSlide()
    {
        Vector3 direction = slideOutTo switch
        {
            SlideDirection.Down => Vector3.down,
            SlideDirection.Left => Vector3.left,
            SlideDirection.Up => Vector3.up,
            SlideDirection.Right => Vector3.right,
            _ => Vector3.zero
        };

        Vector3 startOffset;

        if (slideOutTo == SlideDirection.Left || slideOutTo == SlideDirection.Right)
        {
            startOffset = (GetComponent<RectTransform>().rect.width + Screen.width) * direction;
        }
        else
        {
            startOffset = (GetComponent<RectTransform>().rect.height + Screen.height) * direction;
        }

        Vector3 targetPos = initPos;

        if (turnOnTransition != TransitionType.Slide) afterSlidePos = initPos;

        if (slideOutTo == SlideDirection.Custom)
        {
            if (customSlideOutPoint == null)
            {
                Debug.LogWarning("Assign a 'customSlideInPoint' to use custom slide mode !");
                afterSlidePos = targetPos + startOffset;
            }
            else
            {
                afterSlidePos = customSlideOutPoint.position;
            }
        }
        else
        {
            afterSlidePos = targetPos + startOffset;
        }
    }


    private void SlideInAnim()
    {
        if (useCustomAnimCurve)
        {
            transform.DOMove(initPos, turnOnDuration).SetEase(customAnimCurve);
        }
        else
        {
            transform.DOMove(initPos, turnOnDuration).SetEase(animEase);
        }
    }

    #endregion

    #region Turn Off

    [Button("TURN OFF TRANSITION")]
    public void TurnOffTransition()
    {
        switch (turnOffTransition)
        {
            case TransitionType.Scale:
                ScaleDownAnim();
                break;
            case TransitionType.Fade:
                FadeAnim(false);
                break;
            case TransitionType.Slide:
                SlideOutAnim();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    private void ScaleDownAnim()
    {
        if (useCustomAnimCurve)
        {
            transform.DOScale(Vector3.zero, turnOnDuration).SetEase(customAnimCurve);
        }
        else
        {
            transform.DOScale(Vector3.zero, turnOnDuration).SetEase(animEase);
        }
    }

    private void SlideOutAnim()
    {
        Vector3 targetPos = afterSlidePos;

        if (useCustomAnimCurve)
        {
            transform.DOMove(targetPos, turnOnDuration).SetEase(customAnimCurve)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    SetPosition(beforeSlidePos);
                });
        }
        else
        {
            transform.DOMove(targetPos, turnOnDuration).SetEase(animEase)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    SetPosition(beforeSlidePos);
                });
        }
    }

    #endregion

    private void FadeAnim(bool isFadeIn)
    {
        if (!TryGetComponent(out Image img))
        {
            Debug.LogWarning("If you want to fade this object, you need to add image component on it!");
            return;
        }

        float targetFadeVal = isFadeIn ? turnOnFadeValue : 0;
        img.DOFade(targetFadeVal, isFadeIn ? turnOnDuration : turnOffDuration);
    }

    private void SetStartSettings()
    {
        DOTween.Kill(transform);
        
        if (!isSlidePosSet)
        {
            isSlidePosSet = true;
            SetPosBeforeSlide();
            SetPosAfterSlide();
        }
        
        switch (turnOnTransition)
        {
            case TransitionType.Scale:
                transform.localScale = Vector3.zero;
                break;
            case TransitionType.Fade:
                transform.localScale = Vector3.one;

                if (TryGetComponent(out Image img))
                {
                    DOTween.Kill(img);
                    Color color = img.color;
                    color.a = 0;
                    img.color = color;
                }
                break;
            case TransitionType.Slide:
                SetPosition(beforeSlidePos);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}