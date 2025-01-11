using IPA.Utilities;
using ProMod.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProMod.Stats
{

    public class ProStatNode : IProStatTreeMember, IProStatData
    {
        private IProStatData _rootStatData;
        public virtual IProStatData rootStatData { set => _rootStatData = value; }

        #region CutStats

        public ProIntegerHistogram beforeCutScore { get; private set; } = new ProIntegerHistogram(0, 70);
        public ProIntegerHistogram afterCutScore { get; private set; } = new ProIntegerHistogram(0, 30);
        public ProIntegerHistogram centerDistanceCutScore { get; private set; } = new ProIntegerHistogram(0, 15);
        public int centerDistanceScoreDamage { get; private set; } = 0;
        public int beforeCutScoreDamage { get; private set; } = 0;
        public int afterCutScoreDamage { get; private set; } = 0;
        public ProIntegerHistogram cutScore { get; private set; } = new ProIntegerHistogram(0, 115);

        public ProFloatStat beforeCutSwingRating { get; private set; } = new ProFloatStat();
        public ProFloatStat afterCutSwingRating { get; private set; } = new ProFloatStat();

        public ProFloatStat timeDependence { get; private set; } = new ProFloatStat();
        public ProFloatStat timeDependenceAngle { get; private set; } = new ProFloatStat();
        //public ProFloatStat zAxisAngle { get; private set; } = new ProFloatStat();
        public ProFloatStat centerDistance { get; private set; } = new ProFloatStat();

        public ProFloatStat timeDeviation { get; private set; } = new ProFloatStat();
        public ProFloatStat timeDeviationMagnitude { get; private set; } = new ProFloatStat();
        public ProFloatStat saberSpeed { get; private set; } = new ProFloatStat();

        public ProVectorStat cutPoint { get; private set; } = new ProVectorStat();
        public ProVectorStat cutNormal { get; private set; } = new ProVectorStat();

        #endregion

        #region Score
        public int currentScore { get; private set; } = 0;
        public int maxPossibleCurrentScore { get; private set; } = 0;

        public int maxPossibleCurrentGoodCutScore { get; private set; } = 0;

        public int currentMaxMultiplierScore { get; private set; } = 0;

        #endregion

        #region Accuracy

        public float currentAccuracy => maxPossibleCurrentScore > 0 ? (float)((double)currentScore / (double)maxPossibleCurrentScore) : 1f;
        public float currentFullComboAccuracy => maxPossibleCurrentGoodCutScore > 0 ? (float)((double)currentMaxMultiplierScore / (double)maxPossibleCurrentGoodCutScore) : 1f;

        public float estimatedFinalAccuracy
        {
            get
            {
                if (_rootStatData.maxPossibleScore <= 0)
                {
                    return 1f;
                }

                if (maxPossibleCurrentScore > 0)
                {
                    if (maxPossibleCurrentGoodCutScore > 0)
                    {
                        return (float)(((double)(_rootStatData.maxPossibleScore - maxPossibleCurrentScore) * (double)currentMaxMultiplierScore / (double)maxPossibleCurrentGoodCutScore + (double)currentScore) / (double)_rootStatData.maxPossibleScore);
                    }
                    else
                    {
                        return 0f;
                    }
                }
                else
                {
                    return 1f;
                }
            }
        }
        public float currentAccuracyLoss => _rootStatData.maxPossibleScore > 0 ? (float)((double)(maxPossibleCurrentScore - currentScore) / (double)_rootStatData.maxPossibleScore) : 0f;

        #endregion

        #region Errors

        public int comboBreakCount { get; private set; } = 0;
        public int missCount { get; private set; } = 0;
        public int badCutCount { get; private set; } = 0;
        public int bombCutCount { get; private set; } = 0;
        public virtual int errorCount => missCount + badCutCount + bombCutCount;

        public int maxPossibleScore => _rootStatData.maxPossibleScore;

        public int currentCombo => _rootStatData.currentCombo;

        public int maxPossibleCurrentCombo => _rootStatData.maxPossibleCurrentCombo;

        public int maxPossibleCombo => _rootStatData.maxPossibleCombo;

        public bool isFullCombo => _rootStatData.isFullCombo;

        public int wallTouchCount => _rootStatData.wallTouchCount;

        public int currentMultiplier => _rootStatData.currentMultiplier;

        public int maxPossibleCurrentMultiplier => _rootStatData.maxPossibleCurrentMultiplier;

        public bool failed => _rootStatData.failed;

        public float currentEnergy => _rootStatData.currentEnergy;

        public int currentLives => _rootStatData.currentLives;

        public int maxLives => _rootStatData.maxLives;

        public float songLength => _rootStatData.songLength;

        public float songProgress => _rootStatData.songProgress;

        public float songEndTime => _rootStatData.songEndTime;



        #endregion

        public virtual void ScoreElement(ScoringElement scoringElement)
        {
            if (scoringElement is GoodCutScoringElement)
            {
                GoodCutScoringElement goodCut = scoringElement as GoodCutScoringElement;
                ScoreModel.NoteScoreDefinition noteScoreDefinition = goodCut.cutScoreBuffer.noteScoreDefinition;
                if (noteScoreDefinition.maxCutScore <= 0)
                {
                    return;
                }

                cutScore.Add(goodCut.cutScore);

                bool rateBeforeCut = noteScoreDefinition.maxBeforeCutScore > 0 && noteScoreDefinition.minBeforeCutScore != noteScoreDefinition.maxBeforeCutScore;
                bool rateAfterCut = noteScoreDefinition.maxAfterCutScore > 0 && noteScoreDefinition.minAfterCutScore != noteScoreDefinition.maxAfterCutScore;
                bool rateCenterDistance = noteScoreDefinition.maxCenterDistanceCutScore > 0;

                if (rateBeforeCut || rateAfterCut)
                {

                    SaberSwingRatingCounter cutSaberSwingRatingCounter = goodCut.GetField<CutScoreBuffer, GoodCutScoringElement>("_cutScoreBuffer").GetField<SaberSwingRatingCounter, CutScoreBuffer>("_saberSwingRatingCounter");
                    if (ProSwingRatingPatch.swingRatingCache.ContainsKey(cutSaberSwingRatingCounter))
                    {
                        ProSwingRating proSwingRating = ProSwingRatingPatch.swingRatingCache[cutSaberSwingRatingCounter];

                        if (rateBeforeCut)
                        {
                            beforeCutScore.Add(goodCut.cutScoreBuffer.beforeCutScore);
                            beforeCutSwingRating.Add(proSwingRating.beforeCutSwingRating);
                            beforeCutScoreDamage += (noteScoreDefinition.maxBeforeCutScore - goodCut.cutScoreBuffer.beforeCutScore) * goodCut.maxMultiplier;
                        }

                        if (rateAfterCut)
                        {
                            afterCutScore.Add(goodCut.cutScoreBuffer.afterCutScore);
                            afterCutSwingRating.Add(proSwingRating.afterCutSwingRating);
                            afterCutScoreDamage += (noteScoreDefinition.maxAfterCutScore - goodCut.cutScoreBuffer.afterCutScore) * goodCut.maxMultiplier;
                        }
                    }
                }

                if (rateCenterDistance)
                {
                    centerDistanceCutScore.Add(goodCut.cutScoreBuffer.centerDistanceCutScore);
                    centerDistanceScoreDamage += (noteScoreDefinition.maxCenterDistanceCutScore - goodCut.cutScoreBuffer.centerDistanceCutScore) * goodCut.maxMultiplier;
                }

                centerDistance.Add(goodCut.cutScoreBuffer.noteCutInfo.cutDistanceToCenter);
                cutPoint.Add(goodCut.cutScoreBuffer.noteCutInfo.cutPoint);
                cutNormal.Add(goodCut.cutScoreBuffer.noteCutInfo.cutNormal.normalized);
                timeDependence.Add(Mathf.Abs(Vector3.Dot(goodCut.cutScoreBuffer.noteCutInfo.cutNormal.normalized, Vector3.forward)));


                timeDependenceAngle.Add(Mathf.Abs(90f - Vector3.Angle(goodCut.cutScoreBuffer.noteCutInfo.cutNormal, Vector3.forward)));
                //zAxisAngle.Add(Vector3.Angle(goodCut.cutScoreBuffer.noteCutInfo.cutNormal, Vector3.forward));

                timeDeviation.Add(goodCut.cutScoreBuffer.noteCutInfo.timeDeviation);
                timeDeviationMagnitude.Add(Mathf.Abs(goodCut.cutScoreBuffer.noteCutInfo.timeDeviation));

                saberSpeed.Add(goodCut.cutScoreBuffer.noteCutInfo.saberSpeed);

                currentScore += goodCut.cutScore * goodCut.multiplier;
                maxPossibleCurrentScore += goodCut.maxPossibleCutScore * goodCut.maxMultiplier;

                currentMaxMultiplierScore += goodCut.cutScore * goodCut.maxMultiplier;
                maxPossibleCurrentGoodCutScore += goodCut.maxPossibleCutScore * goodCut.maxMultiplier;

            }
            else if (scoringElement is BadCutScoringElement)
            {
                BadCutScoringElement badCut = scoringElement as BadCutScoringElement;

                if (badCut.maxPossibleCutScore > 0)
                {
                    badCutCount++;
                    comboBreakCount++;
                    maxPossibleCurrentScore += badCut.maxPossibleCutScore * badCut.maxMultiplier;
                }

            }
            else if (scoringElement is MissScoringElement)
            {
                MissScoringElement miss = scoringElement as MissScoringElement;

                if (miss.maxPossibleCutScore > 0)
                {
                    missCount++;
                    comboBreakCount++;
                    maxPossibleCurrentScore += miss.maxPossibleCutScore * miss.maxMultiplier;
                }


            }
            else
            {
                Plugin.Log.Error("Got bad scoring Element");
            }
        }

        public virtual void CutBomb(NoteController noteController, NoteCutInfo noteCutInfo)
        {
            if (noteCutInfo.noteData.gameplayType == NoteData.GameplayType.Bomb)
            {
                bombCutCount++;
                if(maxPossibleCombo > 0)
                {
                    comboBreakCount++;
                }
            }
        }

        public ProStatNode() { }
    }

}
