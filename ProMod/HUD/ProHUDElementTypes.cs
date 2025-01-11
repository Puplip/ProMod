using BeatSaberMarkupLanguage;
using ProMod.Stats;
using System.Collections.Generic;
using System.Reflection;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using IPA.Config.Data;

namespace ProMod.HUD;


public abstract class ProHUDElementBase : IProHUDElement
{
    ProHUDElementAttribute proHUDElementAttribute;
    public Vector2Int Size { 
        get
        {
            if (proHUDElementAttribute != null || (proHUDElementAttribute = GetType().GetCustomAttribute<ProHUDElementAttribute>()) != null)
            {
                return new Vector2Int(proHUDElementAttribute.width, proHUDElementAttribute.height);
            }
            return Vector2Int.zero;
        }
    }

    public virtual void Initialize(RectTransform rectTransform) { }

    protected bool enabled = true;
    public bool Enabled => enabled;

    public virtual void OnStatUpdate(ProStats proStats) { }
    public virtual void OnStatReady(ProStats proStats) { }
}

public abstract class ProHUDTextElement : ProHUDElementBase
{
    protected Canvas canvas;
    protected TextMeshProUGUI textMeshProUGUI;
    public override void Initialize(RectTransform rectTransform)
    {
        canvas = rectTransform.gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;

        textMeshProUGUI = BeatSaberUI.CreateText<TextMeshProUGUI>(rectTransform, "", Vector2.zero, rectTransform.sizeDelta);
        textMeshProUGUI.fontSize = rectTransform.sizeDelta.y * 1.25f;
        textMeshProUGUI.alignment = TextAlignmentOptions.Midline;
        textMeshProUGUI.autoSizeTextContainer = false;
        textMeshProUGUI.enableAutoSizing = false;
    }

    public virtual bool UpdateEnabled(ProStats proStats)
    {
        return true;
    }
    public abstract string UpdateText(ProStats proStats);

    public override void OnStatUpdate(ProStats proStats)
    {
        if(!(enabled = UpdateEnabled(proStats))) { return; }
        textMeshProUGUI.text = UpdateText(proStats);
    }
}

public abstract class ProHUDTwoColorBar : ProHUDElementBase
{
    protected Canvas canvas;
    protected Image firstBar;
    protected Image secondBar;
    protected Image seekLine;

    public virtual Color FixedFirstColor => Color.white;
    public virtual Color UpdateFirstColor(ProStats proStats)
    {
        return FixedFirstColor;
    }

    public virtual Color FixedSecondColor => Color.grey;
    public virtual Color UpdateSecondColor(ProStats proStats)
    {
        return FixedSecondColor;
    }

    public virtual bool SeekLine => false;

    public abstract float UpdateRatio(ProStats proStats);

    public override void Initialize(RectTransform rectTransform)
    {

        canvas = rectTransform.gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;

        Texture2D whiteTexture = Texture2D.whiteTexture;

        firstBar = new GameObject("FirstBar").AddComponent<Image>();
        firstBar.sprite = Sprite.Create(whiteTexture, new Rect(0, 0, whiteTexture.width, whiteTexture.height), Vector2.one * 0.5f);
        firstBar.material = ProUtil.NoGlowNoFogSpriteMaterial;
        firstBar.type = Image.Type.Filled;
        firstBar.fillMethod = Image.FillMethod.Horizontal;
        firstBar.fillOrigin = (int)Image.OriginHorizontal.Left;

        secondBar = new GameObject("SecondBar").AddComponent<Image>();
        secondBar.sprite = Sprite.Create(whiteTexture, new Rect(0, 0, whiteTexture.width, whiteTexture.height), Vector2.one * 0.5f);
        secondBar.material = ProUtil.NoGlowNoFogSpriteMaterial;
        secondBar.type = Image.Type.Simple;

        //second first because it's in the back
        RectTransform secondBarTransform = secondBar.rectTransform;
        secondBarTransform.SetParent(rectTransform, false);
        secondBarTransform.anchorMin = Vector2.one * 0.5f;
        secondBarTransform.anchorMax = Vector2.one * 0.5f;
        secondBarTransform.pivot = Vector2.one * 0.5f;
        secondBarTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x,rectTransform.sizeDelta.y * 0.5f);
        secondBarTransform.localPosition = Vector3.zero;

        RectTransform firstBarTransform = firstBar.rectTransform;
        firstBarTransform.SetParent(rectTransform, false);
        firstBarTransform.anchorMin = Vector2.one * 0.5f;
        firstBarTransform.anchorMax = Vector2.one * 0.5f;
        firstBarTransform.pivot = Vector2.one * 0.5f;
        firstBarTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y * 0.5f);
        firstBarTransform.localPosition = Vector3.zero;

        if (SeekLine)
        {
            seekLine = new GameObject("SecondBar").AddComponent<Image>();
            seekLine.sprite = Sprite.Create(whiteTexture, new Rect(0, 0, whiteTexture.width, whiteTexture.height), Vector2.one * 0.5f);
            seekLine.material = ProUtil.NoGlowNoFogSpriteMaterial;
            seekLine.type = Image.Type.Simple;

            RectTransform seekLineTransform = seekLine.rectTransform;
            seekLineTransform.SetParent(rectTransform, false);
            seekLineTransform.anchorMin = Vector2.one * 0.5f;
            seekLineTransform.anchorMax = Vector2.one * 0.5f;
            seekLineTransform.pivot = Vector2.one * 0.5f;
            seekLineTransform.sizeDelta = new Vector2(2f, rectTransform.sizeDelta.y * 0.75f);
            seekLineTransform.localPosition = Vector3.zero;
        }

    }

    public virtual bool UpdateEnabled(ProStats proStats)
    {
        return true;
    }

    public override void OnStatUpdate(ProStats proStats)
    {
        if(!(enabled = UpdateEnabled(proStats))) { return; }

        float ratio = UpdateRatio(proStats);
        firstBar.fillAmount = ratio;
        firstBar.color = UpdateFirstColor(proStats);
        secondBar.color = UpdateSecondColor(proStats);

        if (SeekLine)
        {
            seekLine.rectTransform.anchoredPosition = new Vector2((-0.5f + ratio) * (canvas.transform as RectTransform).sizeDelta.x, 0f);
        }
    }
}

public abstract class ProHUDOutlineBar : ProHUDElementBase
{
    protected Canvas canvas;
    protected Image outlineImage;
    protected Image barImage;

    public abstract float UpdateRatio(ProStats proStats);

    public virtual Color FixedColor => Color.white;
    public virtual Color UpdateColor(ProStats proStats)
    {
        return FixedColor;
    }

    public override void Initialize(RectTransform rectTransform)
    {

        canvas = rectTransform.gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;

        const int outlineScalingFactor = 4;
        const int outlineThickness = 2;
        const int outlineMargin = 2;
        const float barSizeDelta = (float)(outlineThickness * 2 + outlineMargin * 2);



        outlineImage = new GameObject("OutlineImage").AddComponent<Image>();
        outlineImage.material = ProUtil.NoGlowNoFogSpriteMaterial;
        outlineImage.type = Image.Type.Simple;

        Texture2D whiteTexture = Texture2D.whiteTexture;
        barImage = new GameObject("FillBar").AddComponent<Image>();
        barImage.sprite = Sprite.Create(whiteTexture, new Rect(0, 0, whiteTexture.width, whiteTexture.height), Vector2.one * 0.5f);
        barImage.material = ProUtil.NoGlowNoFogSpriteMaterial;
        barImage.type = Image.Type.Filled;
        barImage.fillMethod = Image.FillMethod.Horizontal;
        barImage.fillOrigin = (int)Image.OriginHorizontal.Left;

        RectTransform outlineTransform = outlineImage.rectTransform;
        outlineTransform.SetParent(rectTransform,false);
        outlineTransform.anchorMin = Vector2.one * 0.5f;
        outlineTransform.anchorMax = outlineTransform.anchorMin;
        outlineTransform.pivot = outlineTransform.anchorMin;
        outlineTransform.sizeDelta = rectTransform.sizeDelta;
        outlineTransform.localPosition = Vector3.zero;

        RectTransform barTransform = barImage.rectTransform;
        barTransform.SetParent(rectTransform, false);
        barTransform.anchorMin = Vector2.one * 0.5f;
        barTransform.anchorMax = outlineTransform.anchorMin;
        barTransform.pivot = outlineTransform.anchorMin;
        barTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x - barSizeDelta, rectTransform.sizeDelta.y - barSizeDelta);
        barTransform.localPosition = Vector3.zero;


        Texture2D outlineTexture = new Texture2D(Mathf.RoundToInt(rectTransform.sizeDelta.x * (float)outlineScalingFactor), Mathf.RoundToInt(rectTransform.sizeDelta.y * (float)outlineScalingFactor));

        int farXMin = outlineTexture.width - (outlineScalingFactor * outlineThickness);
        int farYMin = outlineTexture.height - (outlineScalingFactor * outlineThickness);

        const int closeMax = outlineScalingFactor * outlineThickness;

        for (int x = 0; x < outlineTexture.width; x++)
        {
            for(int y = 0; y < outlineTexture.height; y++)
            {
                if(x < closeMax || y < closeMax || x >= farXMin || y >= farYMin)
                {
                    outlineTexture.SetPixel(x, y, Color.white);
                } else
                {
                    outlineTexture.SetPixel(x, y, Color.clear);
                }
            }
        }

        outlineTexture.Apply();
        outlineImage.sprite = Sprite.Create(outlineTexture, new Rect(0, 0, outlineTexture.width, outlineTexture.height), Vector2.one * 0.5f);

    }

    public virtual bool UpdateEnabled(ProStats proStats)
    {
        return true;
    }

    public override void OnStatUpdate(ProStats proStats)
    {
        if(!(enabled = UpdateEnabled(proStats))) { return; }
        barImage.fillAmount = UpdateRatio(proStats);
        barImage.color = UpdateColor(proStats);
    }
}

public abstract class ProHUDSaberHistogram : ProHUDElementBase
{
    protected Canvas canvas;
    protected Image xAxis;
    protected Image yAxis;
    protected Image[,] saberBarImages = null;

    public abstract int HistogramBucketCount { get; }

    public abstract ProIntegerHistogram GetIntegerHistogram(ProStats proStats, SaberType saberType);

    public override void Initialize(RectTransform rectTransform)
    {

        canvas = rectTransform.gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        Texture2D barTexture = Texture2D.whiteTexture;


        xAxis = new GameObject("HistogramXAxis").AddComponent<Image>();
        xAxis.material = ProUtil.NoGlowNoFogSpriteMaterial;
        xAxis.type = Image.Type.Simple;
        xAxis.sprite = Sprite.Create(barTexture, new Rect(0, 0, barTexture.width, barTexture.height), Vector2.one * 0.5f);


        yAxis = new GameObject("HistogramYAxis").AddComponent<Image>();
        yAxis.material = ProUtil.NoGlowNoFogSpriteMaterial;
        yAxis.type = Image.Type.Simple;
        yAxis.sprite = Sprite.Create(barTexture, new Rect(0, 0, barTexture.width, barTexture.height), Vector2.one * 0.5f);

        const float axisThickness = 2f;

        RectTransform xAxisTransform = xAxis.rectTransform;
        xAxisTransform.SetParent(rectTransform, false);
        xAxisTransform.anchorMin = Vector2.one * 0.5f;
        xAxisTransform.anchorMax = xAxisTransform.anchorMin;
        xAxisTransform.pivot = xAxisTransform.anchorMin;
        xAxisTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x,axisThickness);
        xAxisTransform.localPosition = new Vector3(0f, (axisThickness - rectTransform.sizeDelta.y) * 0.5f, 0f);

        RectTransform yAxisTransform = yAxis.rectTransform;
        yAxisTransform.SetParent(rectTransform, false);
        yAxisTransform.anchorMin = Vector2.one * 0.5f;
        yAxisTransform.anchorMax = yAxisTransform.anchorMin;
        yAxisTransform.pivot = yAxisTransform.anchorMin;
        yAxisTransform.sizeDelta = new Vector2(axisThickness, rectTransform.sizeDelta.y);
        yAxisTransform.localPosition = new Vector3((axisThickness - rectTransform.sizeDelta.x) * 0.5f, 0f, 0f);

        float histogramBarWidth = (rectTransform.sizeDelta.x - axisThickness) / (float)(3 * HistogramBucketCount + 1);
        float histogramBarHeight = rectTransform.sizeDelta.y - axisThickness;
        float histogramBarY = axisThickness * 0.5f;

        float histogramBarX = rectTransform.sizeDelta.x * -0.5f + axisThickness + 1.5f * histogramBarWidth;
        saberBarImages = new Image[2, HistogramBucketCount];

        //saberBarImages[SaberType.SaberA] = new Image[HistogramBucketCount];
        //saberBarImages[SaberType.SaberB] = new Image[HistogramBucketCount];


        for(int i = 0; i < HistogramBucketCount; i++)
        {
            foreach (SaberType saberType in new SaberType[] { SaberType.SaberA, SaberType.SaberB })
            {
                Image barImage = new GameObject("HistogramBar").AddComponent<Image>();
                barImage.type = Image.Type.Filled;
                barImage.fillMethod = Image.FillMethod.Vertical;
                barImage.fillOrigin = (int)Image.OriginVertical.Bottom;
                barImage.material = ProUtil.NoGlowNoFogSpriteMaterial;
                barImage.sprite = Sprite.Create(barTexture, new Rect(0, 0, barTexture.width, barTexture.height), Vector2.one * 0.5f);
                barImage.fillAmount = 0f;

                RectTransform barTransform = barImage.rectTransform;
                barTransform.SetParent(rectTransform, false);
                barTransform.anchorMin = Vector2.one * 0.5f;
                barTransform.anchorMax = barTransform.anchorMin;
                barTransform.pivot = barTransform.anchorMin;
                barTransform.sizeDelta = new Vector2(histogramBarWidth, histogramBarHeight);
                barTransform.localPosition = new Vector3(histogramBarX, histogramBarY, 0f);


                saberBarImages[(int)saberType,i] = barImage;

                histogramBarX += histogramBarWidth;
            }
            histogramBarX += histogramBarWidth;

        }

    }

    private bool setColor = false;
    public override void OnStatUpdate(ProStats proStats)
    {

        if (!setColor)
        {
            foreach (SaberType saberType in new SaberType[] { SaberType.SaberA, SaberType.SaberB })
            {
                Color saberColor = proStats.ColorForSaber(saberType);
                for (int i = 0; i < HistogramBucketCount; i++)
                {
                    saberBarImages[(int)saberType,i].color = saberColor;
                }
            }
            setColor = true;
        }

        float maxRatio = -1;
        ProIntegerHistogram[] saberHistograms = new ProIntegerHistogram[]
        {
            GetIntegerHistogram(proStats, SaberType.SaberA),
            GetIntegerHistogram(proStats, SaberType.SaberB)
        };

        int bucketCount = saberHistograms[0].Max - saberHistograms[0].Min + 1;
        float[,] ratioBuckets = new float[2, bucketCount];

        foreach (SaberType saberType in new SaberType[] { SaberType.SaberA, SaberType.SaberB })
        {
            ProIntegerHistogram saberHistogram = GetIntegerHistogram(proStats, saberType);

            if(saberHistogram.Count() <= 0) { continue; }

            int max = Math.Min(HistogramBucketCount, saberHistogram.Max - saberHistogram.Min + 1);

            for (int i = 0; i < max; i++)
            {
                float ratio = saberHistogram.RatioAtValue(saberHistogram.Max - i);
                maxRatio = Mathf.Max(maxRatio, ratio);
                ratioBuckets[(int)saberType, i] = ratio;
            }
        }

        if (maxRatio <= 0f) { return; }

        foreach (SaberType saberType in new SaberType[] { SaberType.SaberA, SaberType.SaberB })
        {

            for (int i = 0; i < bucketCount; i++)
            {
                saberBarImages[(int)saberType,i].fillAmount = ratioBuckets[(int)saberType,i] / maxRatio;
            }
        }


    }
}
