using ProMod.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Zenject;

namespace ProMod.HUD.Elements
{
    [ProHUDElement("ScoreDensity", 276, 120)]
    public class ProHUDScoreDensity : ProHUDElementBase
    {
        private Canvas canvas;
        private RectTransform rectTransform;
        private Image fillGraph;
        private Image graphBackground;
        private Image seek;
        public override void Initialize(RectTransform rectTransform)
        {
            this.rectTransform = rectTransform;
        }

        private static float[] scoreDensityFilter = new float[]
        {
0.000022516898408715f,
0.000032222334217595f,
0.000043058680383818f,
0.000055031689684846f,
0.000068137675791901f,
0.000082362941353153f,
0.000097683251428793f,
0.000114063358269142f,
0.000131456583301077f,
0.000149804462016326f,
0.000169036457237560f,
0.000189069745974703f,
0.000209809084775070f,
0.000231146758117530f,
0.000252962614004084f,
0.000275124190463417f,
0.000297486936202001f,
0.000319894528121233f,
0.000342179287866335f,
0.000364162698987015f,
0.000385656025674321f,
0.000406461033395824f,
0.000426370811086013f,
0.000445170693864233f,
0.000462639284552773f,
0.000478549571556982f,
0.000492670139952061f,
0.000504766471901939f,
0.000514602331819092f,
0.000521941230965202f,
0.000526547965495825f,
0.000528190221272897f,
0.000526640238111593f,
0.000521676525497803f,
0.000513085621214052f,
0.000500663883749694f,
0.000484219308850381f,
0.000463573360086384f,
0.000438562802893653f,
0.000409041531169545f,
0.000374882375190502f,
0.000335978879365284f,
0.000292247038147598f,
0.000243626978308968f,
0.000190084575718920f,
0.000131612994796918f,
0.000068234138890717f,
0.000000000000000000f,
-0.000073006103496926f,
-0.000150668412141610f,
-0.000232837744358645f,
-0.000319330659260165f,
-0.000409928772932334f,
-0.000504378237237291f,
-0.000602389389487438f,
-0.000703636580605810f,
-0.000807758188579449f,
-0.000914356823145833f,
-0.001022999726729329f,
-0.001133219375669908f,
-0.001244514284764480f,
-0.001356350017077716f,
-0.001468160399879167f,
-0.001579348946433280f,
-0.001689290482214029f,
-0.001797333010842092f,
-0.001902799602157574f,
-0.002004990812789138f,
-0.002103186952151055f,
-0.002196650706811252f,
-0.002284629897160630f,
-0.002366360395512289f,
-0.002441069194895273f,
-0.002507977616739739f,
-0.002566304644627605f,
-0.002615270370311471f,
-0.002654099537291523f,
-0.002682025166391513f,
-0.002698292246996801f,
-0.002702161476915467f,
-0.002692913033203078f,
-0.002669850355757610f,
-0.002632303925047982f,
-0.002579635014991229f,
-0.002511239401743675f,
-0.002426551009022702f,
-0.002325045470531147f,
-0.002206243590116945f,
-0.002069714680468319f,
-0.001915079761419766f,
-0.001742014599326468f,
-0.001550252569453878f,
-0.001339587323923864f,
-0.001109875248457014f,
-0.000861037691949895f,
-0.000593062953823265f,
-0.000306008015068424f,
0.000000000000000001f,
0.000324762643110706f,
0.000668009253102377f,
0.001029396367952290f,
0.001408507445643251f,
0.001804852900299658f,
0.002217870458525453f,
0.002646925839333593f,
0.003091313759476893f,
0.003550259264381757f,
0.004022919383258217f,
0.004508385105320365f,
0.005005683672410077f,
0.005513781181682327f,
0.006031585490392067f,
0.006557949413229033f,
0.007091674201087589f,
0.007631513288641997f,
0.008176176296632507f,
0.008724333273362818f,
0.009274619158572346f,
0.009825638451586278f,
0.010375970064469013f,
0.010924172339819781f,
0.011468788211859853f,
0.012008350488574477f,
0.012541387231894784f,
0.013066427212241275f,
0.013582005413204430f,
0.014086668561713818f,
0.014578980658747447f,
0.015057528485460551f,
0.015520927059569221f,
0.015967825016909836f,
0.016396909893310312f,
0.016806913282254223f,
0.017196615844290393f,
0.017564852144739480f,
0.017910515296969371f,
0.018232561389353242f,
0.018530013674979414f,
0.018801966504250284f,
0.019047588981679534f,
0.019266128329469293f,
0.019456912941813120f,
0.019619355115321074f,
0.019752953442491523f,
0.019857294856752670f,
0.019932056319255532f,
0.019977006139313514f,
0.019992004922138547f,
0.019977006139313514f,
0.019932056319255532f,
0.019857294856752670f,
0.019752953442491523f,
0.019619355115321074f,
0.019456912941813120f,
0.019266128329469293f,
0.019047588981679534f,
0.018801966504250284f,
0.018530013674979414f,
0.018232561389353242f,
0.017910515296969371f,
0.017564852144739480f,
0.017196615844290393f,
0.016806913282254223f,
0.016396909893310312f,
0.015967825016909836f,
0.015520927059569235f,
0.015057528485460551f,
0.014578980658747447f,
0.014086668561713818f,
0.013582005413204430f,
0.013066427212241275f,
0.012541387231894784f,
0.012008350488574477f,
0.011468788211859853f,
0.010924172339819781f,
0.010375970064469013f,
0.009825638451586278f,
0.009274619158572346f,
0.008724333273362818f,
0.008176176296632507f,
0.007631513288641997f,
0.007091674201087589f,
0.006557949413229033f,
0.006031585490392067f,
0.005513781181682327f,
0.005005683672410077f,
0.004508385105320365f,
0.004022919383258217f,
0.003550259264381757f,
0.003091313759476890f,
0.002646925839333593f,
0.002217870458525453f,
0.001804852900299658f,
0.001408507445643252f,
0.001029396367952290f,
0.000668009253102377f,
0.000324762643110706f,
0.000000000000000001f,
-0.000306008015068424f,
-0.000593062953823265f,
-0.000861037691949895f,
-0.001109875248457014f,
-0.001339587323923864f,
-0.001550252569453878f,
-0.001742014599326468f,
-0.001915079761419766f,
-0.002069714680468319f,
-0.002206243590116945f,
-0.002325045470531147f,
-0.002426551009022702f,
-0.002511239401743675f,
-0.002579635014991229f,
-0.002632303925047982f,
-0.002669850355757610f,
-0.002692913033203076f,
-0.002702161476915467f,
-0.002698292246996799f,
-0.002682025166391512f,
-0.002654099537291522f,
-0.002615270370311471f,
-0.002566304644627604f,
-0.002507977616739739f,
-0.002441069194895273f,
-0.002366360395512289f,
-0.002284629897160631f,
-0.002196650706811252f,
-0.002103186952151055f,
-0.002004990812789138f,
-0.001902799602157574f,
-0.001797333010842092f,
-0.001689290482214029f,
-0.001579348946433280f,
-0.001468160399879167f,
-0.001356350017077716f,
-0.001244514284764480f,
-0.001133219375669908f,
-0.001022999726729329f,
-0.000914356823145833f,
-0.000807758188579449f,
-0.000703636580605810f,
-0.000602389389487438f,
-0.000504378237237291f,
-0.000409928772932334f,
-0.000319330659260165f,
-0.000232837744358645f,
-0.000150668412141610f,
-0.000073006103496926f,
0.000000000000000000f,
0.000068234138890717f,
0.000131612994796918f,
0.000190084575718920f,
0.000243626978308969f,
0.000292247038147598f,
0.000335978879365285f,
0.000374882375190502f,
0.000409041531169545f,
0.000438562802893653f,
0.000463573360086384f,
0.000484219308850381f,
0.000500663883749694f,
0.000513085621214052f,
0.000521676525497803f,
0.000526640238111593f,
0.000528190221272897f,
0.000526547965495825f,
0.000521941230965202f,
0.000514602331819092f,
0.000504766471901939f,
0.000492670139952061f,
0.000478549571556982f,
0.000462639284552773f,
0.000445170693864233f,
0.000426370811086013f,
0.000406461033395824f,
0.000385656025674321f,
0.000364162698987014f,
0.000342179287866334f,
0.000319894528121233f,
0.000297486936202001f,
0.000275124190463417f,
0.000252962614004085f,
0.000231146758117530f,
0.000209809084775070f,
0.000189069745974703f,
0.000169036457237561f,
0.000149804462016326f,
0.000131456583301077f,
0.000114063358269142f,
0.000097683251428793f,
0.000082362941353153f,
0.000068137675791901f,
0.000055031689684846f,
0.000043058680383818f,
0.000032222334217595f,
0.000022516898408715f,
        };
        private float[] filteredScoreDensity = null;
        private int scoreDensityMagnitude = 0;

        private float seekStartX = 0f;
        private float seekLengthX = 0f;
        private float seekStartY = 0f;
        private float seekLengthY = 0f;

        private float fillStart = 0f;
        private float fillLength = 1f;

        public override void OnStatReady(ProStats proStats)
        {
            canvas = rectTransform.gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;

            const int graphBorder = 4;
            const int lineRadius = 8;
            const int lineAliasing = 2;
            const int graphOffset = graphBorder + lineRadius + lineAliasing;

            const int textureWidth = 2048;
            const int scoreDensitySampleCount = textureWidth - graphOffset * 2;
            seekStartX = rectTransform.sizeDelta.x * ((float)graphOffset / (float)scoreDensitySampleCount - 0.5f);
            seekLengthX = rectTransform.sizeDelta.x * (float)scoreDensitySampleCount / (float)textureWidth;
            scoreDensityMagnitude = Mathf.RoundToInt((float)scoreDensitySampleCount / rectTransform.sizeDelta.x * rectTransform.sizeDelta.y) / 2;
            int textureHeight = scoreDensityMagnitude + graphOffset * 2;

            seekStartY = rectTransform.sizeDelta.y * ((float)graphOffset / (float)textureHeight - 0.5f);
            seekLengthY = rectTransform.sizeDelta.y * (float)scoreDensityMagnitude / (float)textureHeight;

            float[] scoreDensity = new float[scoreDensitySampleCount];
            filteredScoreDensity = new float[scoreDensitySampleCount];
            float maxFilteredScoreDensity = 0;

            IEnumerable<NoteData> noteDataList = proStats.readonlyBeatmapData.GetBeatmapDataItems<NoteData>(0);
            IEnumerable<SliderData> sliderDataList = proStats.readonlyBeatmapData.GetBeatmapDataItems<SliderData>(0);
            float scoreDensitySampleWidth = proStats.songLength / (float)scoreDensitySampleCount;
            int noteScoreWindow = Mathf.CeilToInt(1.5f / scoreDensitySampleWidth);

            foreach (NoteData noteData in noteDataList) {

                //skip zero score
                if(noteData.scoringType < NoteData.ScoringType.Normal) { continue; }

                int index = Mathf.FloorToInt(noteData.time / scoreDensitySampleWidth);
                float noteScore = ScoreModel.GetNoteScoreDefinition(noteData.scoringType).maxCutScore;
                //scoreDensity[index] += noteScore;
                for (int i = 0; i < noteScoreWindow; i++)
                {
                    if (index >= scoreDensity.Length)
                    {
                        index--;
                        continue;
                    }
                    else if (index < 0)
                    {
                        break;
                    }

                    //add to density
                    scoreDensity[index--] += noteScore;
                }
            }

            int burstSliderElementScore = ScoreModel.GetNoteScoreDefinition(NoteData.ScoringType.BurstSliderElement).maxCutScore;

            foreach(SliderData sliderData in sliderDataList)
            {
                //only want burst sliders
                if(sliderData.sliderType != SliderData.Type.Burst) { continue; }

                float sliderSliceTimeDelta = (sliderData.tailTime - sliderData.time) / (float)sliderData.sliceCount;
                

                //skip the head of the slider
                for(int i = 1; i < sliderData.sliceCount; i++)
                {

                    int index = Mathf.FloorToInt((sliderData.time + sliderSliceTimeDelta * (float)i) / scoreDensitySampleWidth);
                    
                    for (int j = 0; j < noteScoreWindow; j++)
                    {
                        if (index >= scoreDensity.Length)
                        {
                            index--;
                            continue;
                        }
                        else if (index < 0)
                        {
                            break;
                        }

                        //add to density
                        scoreDensity[index--] += burstSliderElementScore;
                    }
                }

            }

            int filterPeak = scoreDensityFilter.Length / 2 + 1;
            for (int i = 0; i < scoreDensitySampleCount; i++)
            {
                float x = 0;
                for (int j = 0; j < scoreDensityFilter.Length; j++)
                {
                    int index = i - filterPeak + j;
                    if(index < 0) { continue; }
                    if(index >= scoreDensitySampleCount) { break; }
                    x += scoreDensity[index] * scoreDensityFilter[j];
                }
                filteredScoreDensity[i] = (x = Mathf.Max(x, 0f));
                maxFilteredScoreDensity = Mathf.Max(maxFilteredScoreDensity, x);
            }

            if (maxFilteredScoreDensity <= 0) { return; }

            float filteredScoreDensityScale = (float)scoreDensityMagnitude / maxFilteredScoreDensity;
            for (int i = 0; i < scoreDensitySampleCount; i++)
            {
                filteredScoreDensity[i] *= filteredScoreDensityScale;
            }

            Texture2D densityTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
            
            const int lineAlphaTableDimension = lineRadius + lineAliasing;
            float[,] lineAlphaTable = new float[lineAlphaTableDimension, lineAlphaTableDimension];
            for(int x = 0; x < lineAlphaTableDimension; x++)
            {
                for(int y = 0; y <= x; y++)
                {
                    float r = Mathf.Sqrt((float)(x * x) + (float)(y * y));
                    float alpha = Mathf.Clamp(1f - (r - (float)lineRadius) / (float)lineAliasing, 0f, 1f);
                    lineAlphaTable[x, y] = alpha;
                    lineAlphaTable[y, x] = alpha;
                }
            }

            for (int x = 0; x < textureWidth; x++)
            {

                for(int y = 0; y < textureHeight; y++)
                {

                    float maxAlpha = 0f;
                    float densityHeight = (float)(y - graphOffset);

                    for(int dx = 1-lineAlphaTableDimension; dx < lineAlphaTableDimension; dx++)
                    {
                        int xIndex = x + dx - graphOffset;
                        if(xIndex < 0) { continue; }
                        if(xIndex >= scoreDensitySampleCount) { break; }
                        int dy = Mathf.RoundToInt(Mathf.Abs(densityHeight - filteredScoreDensity[xIndex]));
                        if(dy < lineAlphaTableDimension)
                        {
                            maxAlpha = Mathf.Max(maxAlpha, lineAlphaTable[Mathf.Abs(dx),dy]);
                            if(maxAlpha >= 0.995f)
                            {
                                break;
                            }
                        }
                    }

                    densityTexture.SetPixel(x, y, new Color(1f, 1f, 1f, maxAlpha));
                }
            }
            densityTexture.Apply();


            graphBackground = new GameObject("DensityGraphBackground").AddComponent<Image>();
            graphBackground.material = ProUtil.NoGlowNoFogSpriteMaterial;
            graphBackground.type = Image.Type.Simple;
            graphBackground.sprite = Sprite.Create(densityTexture, new Rect(0, 0, densityTexture.width, densityTexture.height), Vector2.one * 0.5f);
            graphBackground.color = Color.grey;

            RectTransform graphBackgroundTransform = graphBackground.rectTransform;
            graphBackgroundTransform.SetParent(rectTransform, false);
            graphBackgroundTransform.anchorMin = Vector2.one * 0.5f;
            graphBackgroundTransform.anchorMax = graphBackgroundTransform.anchorMin;
            graphBackgroundTransform.pivot = graphBackgroundTransform.anchorMin;
            graphBackgroundTransform.sizeDelta = rectTransform.sizeDelta;
            graphBackgroundTransform.localPosition = Vector3.zero;

            fillGraph = new GameObject("DensityGraph").AddComponent<Image>();
            fillGraph.material = ProUtil.NoGlowNoFogSpriteMaterial;
            fillGraph.type = Image.Type.Filled;
            fillGraph.fillMethod = Image.FillMethod.Horizontal;
            fillGraph.fillOrigin = (int)Image.OriginHorizontal.Left;
            fillGraph.sprite = Sprite.Create(densityTexture, new Rect(0, 0, densityTexture.width, densityTexture.height), Vector2.one * 0.5f);

            fillStart = (float)graphOffset / (float)textureWidth;
            fillLength = (float)scoreDensitySampleCount / (float)textureWidth;

            RectTransform fillGraphTransform = fillGraph.rectTransform;
            fillGraphTransform.SetParent(rectTransform, false);
            fillGraphTransform.anchorMin = Vector2.one * 0.5f;
            fillGraphTransform.anchorMax = fillGraphTransform.anchorMin;
            fillGraphTransform.pivot = fillGraphTransform.anchorMin;
            fillGraphTransform.sizeDelta = rectTransform.sizeDelta;
            fillGraphTransform.localPosition = Vector3.zero;

            const int seekTextureSize = 64;
            const int seekTextureCenter = seekTextureSize / 2;
            Texture2D seekTexture = new Texture2D(seekTextureSize, seekTextureSize, TextureFormat.RGBA32, false);
            for(int x = 0; x < seekTextureCenter; x++)
            {
                for(int y = 0; y < seekTextureCenter; y++)
                {

                    float r = Mathf.Sqrt((float)(x * x + y * y));
                    Color color = r < (float)(seekTextureCenter - 1) ? Color.white : Color.clear;

                    seekTexture.SetPixel((seekTextureCenter - 1) - x, (seekTextureCenter - 1) - y, color);
                    seekTexture.SetPixel(seekTextureCenter + x, (seekTextureCenter - 1) - y, color);
                    seekTexture.SetPixel((seekTextureCenter - 1) - x, seekTextureCenter + y, color);
                    seekTexture.SetPixel(seekTextureCenter + x, seekTextureCenter + y, color);
                }
            }
            seekTexture.Apply();

            seek = new GameObject("DensityLine").AddComponent<Image>();
            seek.type = Image.Type.Simple;
            seek.sprite = Sprite.Create(seekTexture, new Rect(0, 0, seekTexture.width, seekTexture.height), Vector2.one * 0.5f);

            RectTransform seekTransform = seek.rectTransform;
            seekTransform.SetParent(rectTransform, false);
            seekTransform.anchorMin = Vector2.one * 0.5f;
            seekTransform.anchorMax = seekTransform.anchorMin;
            seekTransform.pivot = seekTransform.anchorMin;

            float seekBaseRadius = (float)((lineAliasing + lineRadius) * 2) / (float)textureHeight * rectTransform.sizeDelta.y;
            seekTransform.sizeDelta = new Vector2(seekBaseRadius, seekBaseRadius * 2);
            seekTransform.localPosition = new Vector3(rectTransform.sizeDelta.x / -2f,0f,0f);
        }

        public override void OnStatUpdate(ProStats proStats)
        {
            float progressRatio = proStats.songProgress / proStats.songLength;
            int progressSampleIndex = Math.Min(Math.Max(Mathf.FloorToInt(progressRatio * (float)filteredScoreDensity.Length),0),filteredScoreDensity.Length-1);
            seek.rectTransform.anchoredPosition = new Vector2(seekStartX + progressRatio * seekLengthX, seekStartY + seekLengthY * filteredScoreDensity[progressSampleIndex] / (float)scoreDensityMagnitude);
            fillGraph.fillAmount = fillStart + progressRatio * fillLength;
        }
    }
}
