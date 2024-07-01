using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FadePanel : MonoBehaviour
{
    public VisualElement BackElement;
    private void Awake()
    {
        BackElement = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("BackFade");
    }
    public void FadeInScene(float duration) 
    {
        DOVirtual.Float(1, 0, duration, value => { BackElement.style.opacity = value; }).SetEase(Ease.InQuad);
    }
    public void FadeOutScene(float duration) 
    {
        DOVirtual.Float(0, 1, duration, value => { BackElement.style.opacity = value; }).SetEase(Ease.InQuad);
    }
}
