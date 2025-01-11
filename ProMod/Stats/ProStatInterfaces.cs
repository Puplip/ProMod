using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProMod.Stats
{

    public interface IProStatTreeMember
    {
        IProStatData rootStatData { set; }
        public void ScoreElement(ScoringElement scoringElement);
        public void CutBomb(NoteController noteController, NoteCutInfo noteCutInfo);
    }

    public interface IProStatData
    {
        ProIntegerHistogram beforeCutScore { get; }
        ProIntegerHistogram afterCutScore { get; }
        ProIntegerHistogram centerDistanceCutScore { get; }
        public int centerDistanceScoreDamage { get; }
        public int beforeCutScoreDamage { get; }
        public int afterCutScoreDamage { get; }
        ProIntegerHistogram cutScore { get; }
        ProFloatStat beforeCutSwingRating { get; }
        ProFloatStat afterCutSwingRating { get; }
        ProFloatStat timeDependence { get; }
        ProFloatStat timeDependenceAngle { get; }
        //ProFloatStat zAxisAngle { get; }
        ProFloatStat centerDistance { get; }
        ProFloatStat timeDeviation { get; }
        ProFloatStat timeDeviationMagnitude { get; }
        ProFloatStat saberSpeed { get; }
        ProVectorStat cutPoint { get; }
        ProVectorStat cutNormal { get; }


        int currentScore { get; }
        int maxPossibleCurrentScore { get; }
        int maxPossibleCurrentGoodCutScore { get; }
        int currentMaxMultiplierScore { get; }
        int maxPossibleScore { get; }

        float currentAccuracy { get; }
        float currentFullComboAccuracy { get; }
        float estimatedFinalAccuracy { get; }
        float currentAccuracyLoss { get; }

        int currentCombo { get; }
        int maxPossibleCurrentCombo { get; }
        int maxPossibleCombo { get; }

        bool isFullCombo { get; }
        int comboBreakCount { get; }
        int missCount { get; }
        int badCutCount { get; }
        int bombCutCount { get; }
        int errorCount { get; }
        int wallTouchCount { get; }

        int currentMultiplier { get; }
        int maxPossibleCurrentMultiplier { get; }

        bool failed { get; }
        float currentEnergy { get; }
        int currentLives { get; }
        int maxLives { get; }

        float songLength { get; }
        float songProgress { get; }
        float songEndTime { get; }
    }
}
