using ProMod.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProMod.HUD;

public abstract class ProHUDCompoundElement : IProHUDElement
{
    private bool enabled = true;
    public bool Enabled => enabled;

    private Vector2Int size = Vector2Int.zero;
    public Vector2Int Size => size;
    public abstract IEnumerable<string> ChildElements { get; }

    RectTransform rectTransform;

    private class ElementDisplayData
    {
        public string name;
        public Vector2Int size;
        public IProHUDElement proElement;
        public RectTransform rectTransform;
        public bool enabled;
        public ProHUD.ElementType elementType;
    }

    private class ElementRowData
    {
        public int height;
        public int width;
        public List<int> elementIndexes;
    }

    private List<ElementDisplayData> elementDisplayData = new List<ElementDisplayData>();

    public void Initialize(RectTransform rectTransform)
    {
        this.rectTransform = rectTransform;

        foreach (string childName in ChildElements)
        {

            if (!ProHUD.ElementExists(childName)) { continue; }

            if (!ProHUD.ElementHasInterface(childName, typeof(IProHUDElement))) { continue; }

            IProHUDElement childElement = ProHUD.CreateElement<IProHUDElement>(childName);

            Vector2Int childSize = childElement.Size;



            RectTransform childTransform = new GameObject("ChildName").AddComponent<RectTransform>();
            childTransform.localScale = new Vector3(1f, 1f, 0f);
            childTransform.sizeDelta = new Vector2((float)childSize.x, (float)childSize.y);
            childTransform.pivot = Vector2.one * 0.5f;
            childTransform.anchorMin = childTransform.pivot;
            childTransform.anchorMax = childTransform.pivot;
            childTransform.SetParent(rectTransform, false);

            //Image debugBoxImage = new GameObject("DebugBox").AddComponent<Image>();
            //debugBoxImage.material = ProUtil.NoGlowNoFogSpriteMaterial;
            //debugBoxImage.type = Image.Type.Simple;

            //Texture2D whiteTexture = Texture2D.whiteTexture;
            //debugBoxImage.sprite = Sprite.Create(whiteTexture, new Rect(0, 0, whiteTexture.width, whiteTexture.height), Vector2.one * 0.5f);
            //debugBoxImage.color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));


            //RectTransform debugBoxTransform = debugBoxImage.rectTransform;
            //debugBoxTransform.SetParent(childCanvas.transform, false);
            //debugBoxTransform.anchorMin = Vector2.one * 0.5f;
            //debugBoxTransform.anchorMax = Vector2.one * 0.5f;
            //debugBoxTransform.pivot = Vector2.one * 0.5f;
            //debugBoxTransform.sizeDelta = rectTransform.sizeDelta;
            //debugBoxTransform.localPosition = Vector3.zero;

            childElement.Initialize(childTransform);

            childTransform.gameObject.SetActive(false);

            elementDisplayData.Add(new ElementDisplayData
            {
                name = childName,
                size = childSize,
                proElement = childElement,
                rectTransform = childTransform,
                enabled = false,
                elementType = ProHUD.GetElementType(childName)
            });
        }

        ProUtil.SetLayerRecursive(rectTransform.gameObject, CameraUtils.Core.VisibilityLayer.UI);
    }

    public void OnStatReady(ProStats proStats)
    {
        foreach(ElementDisplayData elementDisplay in elementDisplayData)
        {
            elementDisplay.proElement.OnStatReady(proStats);
        }
    }

    public void OnStatUpdate(ProStats proStats)
    {
        List<int> enabledElementsList = new List<int>();
        HashSet<int> enabledElementSet = new HashSet<int>();

        bool changed = false;
        for (int i = 0; i < elementDisplayData.Count; i++)
        {

            ElementDisplayData elementDisplay = elementDisplayData[i];

            elementDisplay.proElement.OnStatUpdate(proStats);




            switch (elementDisplay.elementType)
            {
                case ProHUD.ElementType.Normal:

                    bool newElementEnabled = elementDisplay.proElement.Enabled;
                    Vector2Int newElementSize = elementDisplay.proElement.Size;
                    if (newElementEnabled != elementDisplay.enabled || newElementSize != elementDisplay.size)
                    {
                        changed = true;
                        elementDisplay.enabled = newElementEnabled;
                        elementDisplay.size = newElementSize;
                    }

                    if (newElementEnabled)
                    {
                        enabledElementSet.Add(i);
                        enabledElementsList.Add(i);
                    }

                    break;

                case ProHUD.ElementType.NewLine:

                    //remove hanging horizontal separators
                    if (enabledElementsList.Any() && elementDisplayData[enabledElementsList.Last()].elementType == ProHUD.ElementType.HorizontalSeparator)
                    {
                        int removeIndex = enabledElementsList.Last();
                        enabledElementsList.RemoveAt(enabledElementsList.Count - 1);
                        enabledElementSet.Remove(removeIndex);
                    }

                    //only add after a normal element
                    if (enabledElementsList.Any() && elementDisplayData[enabledElementsList.Last()].elementType == ProHUD.ElementType.Normal)
                    {
                        enabledElementSet.Add(i);
                        enabledElementsList.Add(i);
                    }
                    break;
                case ProHUD.ElementType.HorizontalSeparator:

                    //only add after a normal element
                    if (enabledElementsList.Any() && elementDisplayData[enabledElementsList.Last()].elementType == ProHUD.ElementType.Normal)
                    {
                        enabledElementSet.Add(i);
                        enabledElementsList.Add(i);
                    }
                    break;

                case ProHUD.ElementType.VerticalSeparator:

                    //remove horizontal separators
                    if (enabledElementsList.Any() && elementDisplayData[enabledElementsList.Last()].elementType == ProHUD.ElementType.HorizontalSeparator)
                    {
                        int removeIndex = enabledElementsList.Last();
                        enabledElementsList.RemoveAt(enabledElementsList.Count - 1);
                        enabledElementSet.Remove(removeIndex);
                    }

                    //remove new lines
                    if (enabledElementsList.Any() && elementDisplayData[enabledElementsList.Last()].elementType == ProHUD.ElementType.NewLine)
                    {
                        int removeIndex = enabledElementsList.Last();
                        enabledElementsList.RemoveAt(enabledElementsList.Count - 1);
                        enabledElementSet.Remove(removeIndex);
                    }

                    //only add after a normal element
                    if (enabledElementsList.Any() && elementDisplayData[enabledElementsList.Last()].elementType == ProHUD.ElementType.Normal)
                    {
                        enabledElementSet.Add(i);
                        enabledElementsList.Add(i);
                    }
                    break;
            }

        }

        //if nothing changed, there's no need to reposition everything
        if (!changed) { return; }

        //remove non-normal ending elements
        while (enabledElementsList.Any() && elementDisplayData[enabledElementsList.Last()].elementType != ProHUD.ElementType.Normal)
        {
            int removeIndex = enabledElementsList.Last();
            enabledElementsList.RemoveAt(enabledElementsList.Count - 1);
            enabledElementSet.Remove(removeIndex);
        }

        //update display
        for (int i = 0; i < elementDisplayData.Count; i++)
        {
            ElementDisplayData elementDisplay = elementDisplayData[i];
            elementDisplay.enabled = enabledElementSet.Contains(i);
            elementDisplay.rectTransform.gameObject.SetActive(elementDisplay.enabled);


        }

        //Done if there are no Enabled Elements
        if (!enabledElementsList.Any())
        {
            size = Vector2Int.zero;
            enabled = false;
            return;
        }


        List<ElementRowData> elementRowData = new List<ElementRowData>();
        int boardHeight = 0;
        int boardWidth = 0;
        bool newLine = true;

        ElementRowData lastRowData;

        foreach (int elementIndex in enabledElementsList)
        {
            var elementDisplay = elementDisplayData[elementIndex];
            elementDisplay.rectTransform.sizeDelta = elementDisplay.proElement.Size;

            if (elementDisplay.elementType == ProHUD.ElementType.NewLine || elementDisplay.elementType == ProHUD.ElementType.VerticalSeparator)
            {
                newLine = true;
                if (elementDisplay.elementType == ProHUD.ElementType.NewLine)
                {
                    continue;
                }
            }

            if (newLine)
            {

                if (elementRowData.Count > 0)
                {
                    lastRowData = elementRowData.Last();
                    boardHeight += lastRowData.height;
                    boardWidth = Math.Max(boardWidth, lastRowData.width);
                }

                elementRowData.Add(new ElementRowData
                {
                    width = elementDisplay.size.x,
                    height = elementDisplay.size.y,
                    elementIndexes = new List<int> { elementIndex }
                });

                if (elementDisplay.elementType != ProHUD.ElementType.VerticalSeparator)
                {
                    newLine = false;
                }

            }
            else
            {

                var lastRow = elementRowData.Last();

                lastRow.width += elementDisplay.size.x;
                lastRow.height = Math.Max(lastRow.height, elementDisplay.size.y);
                lastRow.elementIndexes.Add(elementIndex);
            }

        }

        lastRowData = elementRowData.Last();
        boardHeight += lastRowData.height;
        boardWidth = Math.Max(boardWidth, lastRowData.width);

        Vector3 boardOffset = new Vector3(0f, (float)boardHeight / 2f, 0f);

        float rowOffsetY = 0;
        foreach (var elementRow in elementRowData)
        {
            float rowOffsetX = (float)elementRow.width / -2f;
            foreach (var statIndex in elementRow.elementIndexes)
            {
                var statSize = elementDisplayData[statIndex].size;

                elementDisplayData[statIndex].rectTransform.localPosition = boardOffset + new Vector3(rowOffsetX + (float)statSize.x * 0.5f, (float)elementRow.height * -0.5f - rowOffsetY, 0f);

                rowOffsetX += (float)statSize.x;
            }

            rowOffsetY += (float)elementRow.height;
        }

        enabled = true;
        size = new Vector2Int(boardWidth, boardHeight);

    }
}
