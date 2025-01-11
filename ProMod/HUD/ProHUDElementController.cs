using UnityEngine;
using ProMod.Stats;
using CameraUtils.Core;

namespace ProMod.HUD;

public class ProHUDElementController : MonoBehaviour
{

    private IProHUDElement proHUDElement;
    public void InitElement(string elementName)
    {

        if (!ProHUD.ElementExists(elementName) || !ProHUD.ElementHasInterface(elementName, typeof(IProHUDElement))) { return; }

        proHUDElement = ProHUD.CreateElement<IProHUDElement>(elementName);

        RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
        rectTransform.localScale = new Vector3(0.01f, 0.01f, 0f);
        rectTransform.sizeDelta = proHUDElement.Size;
        rectTransform.pivot = Vector2.one * 0.5f;
        rectTransform.anchorMin = rectTransform.pivot;
        rectTransform.anchorMin = rectTransform.pivot;

        proHUDElement.Initialize(rectTransform);
        gameObject.SetActive(false);

        ProUtil.SetLayerRecursive(gameObject, VisibilityLayer.UI);
    }

    public void OnStatUpdate(ProStats proStats)
    {
        proHUDElement.OnStatUpdate(proStats);

        (transform as RectTransform).sizeDelta = proHUDElement.Size;

        gameObject.SetActive(proHUDElement.Enabled);
    }

    public void OnStatReady(ProStats proStats)
    {
        proHUDElement.OnStatReady(proStats);

        (transform as RectTransform).sizeDelta = proHUDElement.Size;

        gameObject.SetActive(proHUDElement.Enabled);
    }

}
