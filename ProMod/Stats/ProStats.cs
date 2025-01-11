using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ProMod.Stats
{
    using NotePosition = (NoteLineLayer, int);
    using NoteDirection = NoteCutDirection;
    using NoteType = NoteData.ScoringType;

    public class ProStats : MonoBehaviour, IProStatData
    {

        [Inject]
        private ScoreController _scoreController;

        [Inject]
        private PlayerHeadAndObstacleInteraction _playerHeadAndObstacleInteraction;

        [Inject]
        private BeatmapObjectManager _beatmapObjectManager;

        [Inject]
        private IReadonlyBeatmapData _readonlyBeatmapData;

        public IReadonlyBeatmapData readonlyBeatmapData => _readonlyBeatmapData;

        [Inject]
        private GameEnergyCounter _gameEnergyCounter;

        [Inject]
        private IAudioTimeSource _audioTimeSource;

        [Inject]
        private ColorManager _colorManager;

        public Color ColorForSaber(SaberType saberType)
        {
            return _colorManager.ColorForSaberType(saberType);
        }

        private ProStatNode rootStatNode = new ProStatNode();

        private ProStatDimension<NoteType, ProStatNode> statsByType = new ProStatDimension<NoteType, ProStatNode>();
        private ProStatDimension<SaberType, ProStatNode<NotePosition, ProStatNode<NoteDirection, ProStatNode<NoteType, ProStatNode>,NoteType, ProStatNode>,NoteType, ProStatNode>> statsBySaber = new ProStatDimension<SaberType, ProStatNode<NotePosition, ProStatNode<NoteDirection, ProStatNode<NoteType, ProStatNode>, NoteType, ProStatNode>, NoteType, ProStatNode>>();
        private ProStatDimension<NotePosition, ProStatNode<NoteDirection,ProStatNode<NoteType,ProStatNode>,NoteType, ProStatNode>> statsByPosition = new ProStatDimension<NotePosition, ProStatNode<NoteDirection, ProStatNode<NoteType, ProStatNode>, NoteType, ProStatNode>>();

        public ProStatNode<NotePosition, ProStatNode<NoteDirection, ProStatNode<NoteType, ProStatNode>, NoteType, ProStatNode>, NoteType, ProStatNode> this[SaberType saberType] => statsBySaber[saberType];
        public ProStatNode<NoteDirection, ProStatNode<NoteType, ProStatNode>, NoteType, ProStatNode> this[NotePosition notePosition] => statsByPosition[notePosition];
        public ProStatNode this[NoteType noteType] => statsByType[noteType];

        #region RootNodeExtensions
        public int maxPossibleScore { get; private set; }
        public int currentCombo { get; private set; }
        public int maxPossibleCurrentCombo { get; private set; }
        public int maxPossibleCombo { get; private set; }

        public bool isFullCombo => maxPossibleCombo == currentCombo;

        public int currentMultiplier { get; private set; }
        public int maxPossibleCurrentMultiplier { get; private set; }

        public bool failed { get; private set; }
        public float currentEnergy { get; private set; }
        public int currentLives { get; private set; }
        public int maxLives { get; private set; }
        public int wallTouchCount { get; private set; }

        public float songLength => _audioTimeSource.songLength;
        public float songProgress => _audioTimeSource.songTime;
        public float songEndTime => _audioTimeSource.songEndTime;
        public int errorCount => rootStatNode.errorCount + wallTouchCount;

        public ProIntegerHistogram beforeCutScore => ((IProStatData)rootStatNode).beforeCutScore;
        public ProIntegerHistogram afterCutScore => ((IProStatData)rootStatNode).afterCutScore;
        public ProIntegerHistogram centerDistanceCutScore => ((IProStatData)rootStatNode).centerDistanceCutScore;
        public int centerDistanceScoreDamage => ((IProStatData)rootStatNode).centerDistanceScoreDamage;
        public int beforeCutScoreDamage => ((IProStatData)rootStatNode).beforeCutScoreDamage;
        public int afterCutScoreDamage => ((IProStatData)rootStatNode).afterCutScoreDamage;
        public ProIntegerHistogram cutScore => ((IProStatData)rootStatNode).cutScore;

        public ProFloatStat beforeCutSwingRating => ((IProStatData)rootStatNode).beforeCutSwingRating;
        public ProFloatStat afterCutSwingRating => ((IProStatData)rootStatNode).afterCutSwingRating;
        public ProFloatStat timeDependence => ((IProStatData)rootStatNode).timeDependence;
        public ProFloatStat timeDependenceAngle => ((IProStatData)rootStatNode).timeDependenceAngle;
        public ProFloatStat centerDistance => ((IProStatData)rootStatNode).centerDistance;
        public ProFloatStat timeDeviation => ((IProStatData)rootStatNode).timeDeviation;
        public ProFloatStat timeDeviationMagnitude => ((IProStatData)rootStatNode).timeDeviationMagnitude;
        public ProFloatStat saberSpeed => ((IProStatData)rootStatNode).saberSpeed;

        public ProVectorStat cutPoint => ((IProStatData)rootStatNode).cutPoint;
        public ProVectorStat cutNormal => ((IProStatData)rootStatNode).cutNormal;

        public int currentScore => ((IProStatData)rootStatNode).currentScore;
        public int maxPossibleCurrentScore => ((IProStatData)rootStatNode).maxPossibleCurrentScore;
        public int maxPossibleCurrentGoodCutScore => ((IProStatData)rootStatNode).maxPossibleCurrentGoodCutScore;
        public int currentMaxMultiplierScore => ((IProStatData)rootStatNode).currentMaxMultiplierScore;

        public float currentAccuracy => ((IProStatData)rootStatNode).currentAccuracy;
        public float currentFullComboAccuracy => ((IProStatData)rootStatNode).currentFullComboAccuracy;
        public float estimatedFinalAccuracy => ((IProStatData)rootStatNode).estimatedFinalAccuracy;
        public float currentAccuracyLoss => ((IProStatData)rootStatNode).currentAccuracyLoss;

        public int comboBreakCount => ((IProStatData)rootStatNode).comboBreakCount;
        public int missCount => ((IProStatData)rootStatNode).missCount;
        public int badCutCount => ((IProStatData)rootStatNode).badCutCount;
        public int bombCutCount => ((IProStatData)rootStatNode).bombCutCount;

        #endregion

        public event Action onUpdate;
        public event Action onStatsReady;
        public bool ready { get; private set; } = false;

        public void Awake()
        {
            _beatmapObjectManager.noteWasCutEvent += BeatmapObjectManager_noteWasCutEvent;
            _scoreController.scoringForNoteFinishedEvent += ScoreController_scoringForNoteFinishedEvent;
            _playerHeadAndObstacleInteraction.headDidEnterObstaclesEvent += PlayerHeadAndObstacleInteraction_headDidEnterObstaclesEvent;

            _gameEnergyCounter.gameEnergyDidChangeEvent += GameEnergyCounter_gameEnergyDidChangeEvent;
            _gameEnergyCounter.gameEnergyDidReach0Event += GameEnergyCounter_gameEnergyDidReach0Event;

            rootStatNode.rootStatData = this;
            statsBySaber.rootStatData = this;
            statsByPosition.rootStatData = this;
            statsByType.rootStatData = this;

            maxPossibleScore = ScoreModel.ComputeMaxMultipliedScoreForBeatmap(_readonlyBeatmapData);

            IEnumerable<NoteData> noteDataList = _readonlyBeatmapData.GetBeatmapDataItems<NoteData>(0);
            IEnumerable<SliderData> sliderDataList = _readonlyBeatmapData.GetBeatmapDataItems<SliderData>(0);


            foreach (NoteData noteData in noteDataList)
            {
                if (noteData.scoringType > NoteType.NoScore)
                {
                    maxPossibleCombo++;
                }
            }

            foreach (SliderData sliderData in sliderDataList)
            {
                if (sliderData.sliderType == SliderData.Type.Burst)
                {
                    maxPossibleCombo += sliderData.sliceCount - 1;
                }
            }


            currentEnergy = _gameEnergyCounter.energy;

            if (_gameEnergyCounter.energyType == GameplayModifiers.EnergyType.Battery)
            {
               maxLives = _gameEnergyCounter.batteryLives;
               currentLives = _gameEnergyCounter.batteryEnergy;
            }
            else
            {
                maxLives = 0;
                currentLives = 0;
            }

            ready = true;
            onStatsReady?.Invoke();

        }
        public void OnDestroy()
        {
            _beatmapObjectManager.noteWasCutEvent -= BeatmapObjectManager_noteWasCutEvent;
            _scoreController.scoringForNoteFinishedEvent -= ScoreController_scoringForNoteFinishedEvent;
            _playerHeadAndObstacleInteraction.headDidEnterObstaclesEvent -= PlayerHeadAndObstacleInteraction_headDidEnterObstaclesEvent;

            _gameEnergyCounter.gameEnergyDidChangeEvent -= GameEnergyCounter_gameEnergyDidChangeEvent;
            _gameEnergyCounter.gameEnergyDidReach0Event -= GameEnergyCounter_gameEnergyDidReach0Event;
        }

        private void GameEnergyCounter_gameEnergyDidReach0Event()
        {
            failed = true;
            onUpdate();
        }

        private void GameEnergyCounter_gameEnergyDidChangeEvent(float energy)
        {
            currentEnergy = energy;
            onUpdate();
        }

        private void PlayerHeadAndObstacleInteraction_headDidEnterObstaclesEvent()
        {
            currentCombo = 0;
            wallTouchCount++;
            onUpdate();
        }

        private void ScoreController_scoringForNoteFinishedEvent(ScoringElement scoringElement)
        {
            rootStatNode.ScoreElement(scoringElement);

            statsByType.ScoreElement(scoringElement);
            statsBySaber.ScoreElement(scoringElement);
            statsByPosition.ScoreElement(scoringElement);

            if (scoringElement.noteData.scoringType > NoteType.NoScore)
            {
                maxPossibleCurrentCombo++;
                if (scoringElement is GoodCutScoringElement)
                {
                    currentCombo++;
                } else
                {
                    currentCombo = 0;
                }
            }
            
            onUpdate();

        }

        private void BeatmapObjectManager_noteWasCutEvent(NoteController noteController, in NoteCutInfo noteCutInfo)
        {
            if(noteCutInfo.noteData.gameplayType == NoteData.GameplayType.Bomb)
            {
                currentCombo = 0;

                rootStatNode.CutBomb(noteController, noteCutInfo);
                statsByType.CutBomb(noteController, noteCutInfo);
                statsBySaber.CutBomb(noteController, noteCutInfo);
                statsByPosition.CutBomb(noteController, noteCutInfo);
                onUpdate();
            }

        }


        private float lastUpdate = 0;
        private void Update()
        {
            float time = Time.time;
            if(lastUpdate + 0.1f <= time)
            {
                lastUpdate = time;
                onUpdate();
            }
        }

    }
}
