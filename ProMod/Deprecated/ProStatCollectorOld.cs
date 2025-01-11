//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Zenject;
//using UnityEngine;
//using static BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData;

//namespace ProMod.Stats
//{
//    public class ProStatCollector : MonoBehaviour
//    {

//        [Inject]
//        private IScoreController _scoreController;

//        [Inject]
//        private ComboController _comboController;


//        [Inject]
//        private IAudioTimeSource _audioTimeSource;

//        [Inject]
//        private ProStatData _statData;

//        [Inject]
//        private IReadonlyBeatmapData _beatmapData;

//        [Inject]
//        private GameEnergyCounter _gameEnergyCounter;
//        [Inject]
//        private PlayerHeadAndObstacleInteraction _playerHeadAndObstacleInteraction;
//        [Inject]
//        private BeatmapObjectManager _beatmapObjectManager;

//        private void Awake()
//        {
//            _statData.songLength = _audioTimeSource.songLength;
//            _statData.songTime = _audioTimeSource.songTime;
//            _lastSongTime = _audioTimeSource.songTime;
//            _statData.maxBeatmapScore = ScoreModel.ComputeMaxMultipliedScoreForBeatmap(_beatmapData);

//            IEnumerable<NoteData> noteDataList = _beatmapData.GetBeatmapDataItems<NoteData>(0);
//            IEnumerable<SliderData> sliderDataList = _beatmapData.GetBeatmapDataItems<SliderData>(0);

//            foreach (NoteData noteData in noteDataList)
//            {
//                if(noteData.scoringType > NoteData.ScoringType.NoScore)
//                {
//                    _statData.beatmapNoteCount++;
//                }
//            }
//            foreach (SliderData sliderData in sliderDataList)
//            {
//                if(sliderData.sliderType == SliderData.Type.Burst)
//                {
//                    _statData.beatmapNoteCount += sliderData.sliceCount - 1;
//                }
//            }


//            _statData.energy = _gameEnergyCounter.energy;

//            if(_gameEnergyCounter.energyType == GameplayModifiers.EnergyType.Battery)
//            {
//                _statData.maxLives = _gameEnergyCounter.batteryLives;
//                _statData.lives = _gameEnergyCounter.batteryEnergy;
//            } else
//            {
//                _statData.maxLives = 0;
//                _statData.lives = 0;
//            }

//            _statData.failed = false;

//            _scoreController.scoringForNoteFinishedEvent += ScoreController_scoringForNoteFinishedEvent;
//            _comboController.comboDidChangeEvent += ComboController_comboDidChangeEvent;

//            _gameEnergyCounter.gameEnergyDidChangeEvent += GameEnergyCounter_gameEnergyDidChangeEvent;
//            _gameEnergyCounter.gameEnergyDidReach0Event += GameEnergyCounter_gameEnergyDidReach0Event;

//            _beatmapObjectManager.noteWasCutEvent += BeatmapObjectManager_noteWasCutEvent;
//            _playerHeadAndObstacleInteraction.headDidEnterObstaclesEvent += PlayerHeadAndObstacleInteraction_headDidEnterObstaclesEvent;

//        }



//        private void OnDestroy()
//        {
//            _scoreController.scoringForNoteFinishedEvent -= ScoreController_scoringForNoteFinishedEvent;
//            _comboController.comboDidChangeEvent -= ComboController_comboDidChangeEvent;

//            _gameEnergyCounter.gameEnergyDidChangeEvent -= GameEnergyCounter_gameEnergyDidChangeEvent;
//            _gameEnergyCounter.gameEnergyDidReach0Event -= GameEnergyCounter_gameEnergyDidReach0Event;


//            _beatmapObjectManager.noteWasCutEvent -= BeatmapObjectManager_noteWasCutEvent;
//            _playerHeadAndObstacleInteraction.headDidEnterObstaclesEvent -= PlayerHeadAndObstacleInteraction_headDidEnterObstaclesEvent;
//        }

//        private float _lastSongTime;
//        private void Update()
//        {
//            _statData.songTime = _audioTimeSource.songTime;
//            if (_statData.songTime - _lastSongTime > 0.1f)
//            {
//                _statData.Changed();
//            }

//        }

//        private void PlayerHeadAndObstacleInteraction_headDidEnterObstaclesEvent()
//        {
//            _statData.wallTouches++;

//            if(_statData.maxCurrentScore > 0)
//            {
//                _statData.comboBreaks++;
//            }


//            _statData.Changed();
//        }

//        private void BeatmapObjectManager_noteWasCutEvent(NoteController noteController, in NoteCutInfo noteCutInfo)
//        {
//            if (noteCutInfo.noteData.gameplayType == NoteData.GameplayType.Bomb)
//            {
//                _statData.bombHits++;
//                if (_statData.maxCurrentScore > 0)
//                {
//                    _statData.comboBreaks++;
//                }
//                _statData.Changed();
//            }
//        }

//        private void GameEnergyCounter_gameEnergyDidReach0Event()
//        {
//            _statData.failed = true;
//            _statData.Changed();
//        }
//        private void GameEnergyCounter_gameEnergyDidChangeEvent(float energy)
//        {
//            _statData.energy = energy;
//            if (_statData.maxLives > 0)
//            {
//                _statData.lives = _gameEnergyCounter.batteryEnergy;
//            }
//            _statData.Changed();
//        }

//        private void ComboController_comboDidChangeEvent(int combo)
//        {

//            _statData.maxCombo = Math.Max(combo, _statData.maxCombo);
//            _statData.combo = combo;
//            _statData.Changed();
//        }

//        private void ScoreController_scoringForNoteFinishedEvent(ScoringElement scoringElement)
//        {
//            _statData.score = _scoreController.multipliedScore;
//            _statData.maxCurrentScore = _scoreController.immediateMaxPossibleMultipliedScore;

//            _statData.comboDamage += (scoringElement.maxMultiplier - scoringElement.multiplier) * scoringElement.cutScore;

//            if (scoringElement.noteData.colorType != ColorType.None)
//            {
//                SaberType scoringElementSaberType = scoringElement.noteData.colorType.ToSaberType();
//                _statData.MaxCurrentScoreBySaber[scoringElementSaberType] += scoringElement.maxMultiplier * scoringElement.maxPossibleCutScore;
//                _statData.CurrentScoreBySaber[scoringElementSaberType] += scoringElement.multiplier * scoringElement.cutScore;
//                _statData.ComboDamageBySaber[scoringElementSaberType] += (scoringElement.maxMultiplier - scoringElement.multiplier) * scoringElement.cutScore;
//            }

//            if (scoringElement.noteData.scoringType > NoteData.ScoringType.NoScore)
//            {
//                _statData.scoredNoteCount++;
//            }

//            if (scoringElement is GoodCutScoringElement)
//            {
//                GoodCutScoringElement goodCut = scoringElement as GoodCutScoringElement;


//                if (scoringElement.noteData.colorType != ColorType.None)
//                {
//                    _statData.maxGoodCutScore += scoringElement.maxMultiplier * scoringElement.maxPossibleCutScore;
//                    _statData.MaxGoodCutScoreBySaber[scoringElement.noteData.colorType.ToSaberType()] += scoringElement.maxMultiplier * scoringElement.maxPossibleCutScore;
//                }

//                if (goodCut.noteData.scoringType == NoteData.ScoringType.Normal)
//                {
//                    NoteLineLayer noteLineLayer = goodCut.noteData.noteLineLayer;
//                    int noteRow = goodCut.noteData.lineIndex;
//                    SaberType saberType = goodCut.cutScoreBuffer.noteCutInfo.saberType;
//                    NoteCutDirection cutDirection = goodCut.noteData.cutDirection;

//                    _statData.CutStats.AddGoodCut(goodCut);

//                    _statData.CutStatsByDir[cutDirection].AddGoodCut(goodCut);
//                    _statData.CutStatsBySaber[saberType].AddGoodCut(goodCut);

//                    _statData.CutStatsByPos[new Tuple<NoteLineLayer, int>(noteLineLayer, noteRow)].AddGoodCut(goodCut);
//                    _statData.CutStatsByPosDir[new Tuple<NoteLineLayer, int, NoteCutDirection>(noteLineLayer, noteRow, cutDirection)].AddGoodCut(goodCut);

//                    _statData.CutStatsBySaberPos[new Tuple<SaberType, NoteLineLayer, int>(saberType, noteLineLayer, noteRow)].AddGoodCut(goodCut);
//                    _statData.CutStatsBySaberDir[new Tuple<SaberType, NoteCutDirection>(saberType, cutDirection)].AddGoodCut(goodCut);

//                    _statData.CutStatsBySaberPosDir[new Tuple<SaberType, NoteLineLayer, int, NoteCutDirection>(saberType, noteLineLayer, noteRow, cutDirection)].AddGoodCut(goodCut);

//                }
//            }
//            else if (scoringElement is BadCutScoringElement)
//            {
//                BadCutScoringElement badCut = scoringElement as BadCutScoringElement;
//                _statData.badCuts++;
//                _statData.comboBreaks++;

//                if (scoringElement.noteData.colorType != ColorType.None)
//                {
//                    int damage = scoringElement.maxPossibleCutScore * scoringElement.maxMultiplier;

//                    _statData.badCutDamage += damage;
//                    _statData.BadCutDamageBySaber[scoringElement.noteData.colorType.ToSaberType()] += damage;
//                }
//            }
//            else if (scoringElement is MissScoringElement)
//            {
//                MissScoringElement miss = scoringElement as MissScoringElement;
//                _statData.misses++;
//                _statData.comboBreaks++;

//                if (scoringElement.noteData.colorType != ColorType.None)
//                {
//                    int damage = scoringElement.maxPossibleCutScore * scoringElement.maxMultiplier;

//                    _statData.missDamage += damage;
//                    _statData.MissDamageBySaber[scoringElement.noteData.colorType.ToSaberType()] += damage;
//                }
//            }
//            _statData.Changed();
//        }
//    }
//}