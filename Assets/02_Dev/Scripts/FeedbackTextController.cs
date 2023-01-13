using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class FeedbackTextController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private string[] texts;
    
    void Start()
    {
        SlicerObjectController.onSlashExecuted += ShowFeedback;
    }

    void ShowFeedback()
    {
        feedbackText.fontSize = 0;
        feedbackText.text = texts[Random.Range(0, texts.Length)];
        
        if (DOTween.IsTweening(feedbackText))
            DOTween.Kill(feedbackText);
        
        DOTween.To(()=> feedbackText.fontSize, x=> feedbackText.fontSize = x, 90, 0.5f).OnComplete(() =>
        {
            feedbackText.transform.DOShakeRotation(0.75f,new Vector3(0,0,90),10, 1, true).OnComplete(() =>
            {
                DOTween.To(() => feedbackText.fontSize, x => feedbackText.fontSize = x, 0, 0.15f);
            });
        });
    }
}
