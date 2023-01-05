using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using UnityEngine;

namespace ProMod.Stats
{
    public class ProStatCollector : MonoBehaviour
    {

        [Inject]
        private IScoreController _scoreController;

        [Inject]
        private ComboController _comboController;


        [Inject]
        private IAudioTimeSource _audioTimeSource;

        [Inject]
        private ProStatData _statData;

        [Inject]
        private IReadonlyBeatmapData _beatmapData;
        private void Awake()
        {
            _scoreController.scoringForNoteFinishedEvent += ScoreController_scoringForNoteFinishedEvent;
            _comboController.comboDidChangeEvent += ComboController_comboDidChangeEvent;
            _comboController.comboBreakingEventHappenedEvent += ComboController_comboBreakingEventHappenedEvent;
            _statData.songLength = _audioTimeSource.songLength;
            _statData.songTime = _audioTimeSource.songTime;
            _lastSongTime = _audioTimeSource.songTime;
            _statData.maxBeatmapScore = ScoreModel.ComputeMaxMultipliedScoreForBeatmap(_beatmapData);
        }

        private float _lastSongTime;
        private void Update()
        {
            _statData.songTime = _audioTimeSource.songTime;
            if (_statData.songTime - _lastSongTime > 0.1f)
            {
                _statData.Changed();
            }

        }

        private void ScoreController_scoringForNoteFinishedEvent(ScoringElement scoringElement)
        {
            _statData.score = _scoreController.multipliedScore;
            _statData.maxCurrentScore = _scoreController.immediateMaxPossibleMultipliedScore;

            _statData.comboDamage += (scoringElement.maxMultiplier - scoringElement.multiplier) * scoringElement.maxPossibleCutScore;

            if (scoringElement is GoodCutScoringElement)
            {
                GoodCutScoringElement goodCut = scoringElement as GoodCutScoringElement;
                if (goodCut.noteData.scoringType == NoteData.ScoringType.Normal)
                {
                    NoteLineLayer noteLineLayer = goodCut.noteData.noteLineLayer;
                    int noteRow = goodCut.noteData.lineIndex;
                    SaberType saberType = goodCut.cutScoreBuffer.noteCutInfo.saberType;
                    NoteCutDirection cutDirection = goodCut.noteData.cutDirection;

                    _statData.CutStats.AddGoodCut(goodCut);

                    _statData.CutStatsByDir[cutDirection].AddGoodCut(goodCut);
                    _statData.CutStatsBySaber[saberType].AddGoodCut(goodCut);

                    _statData.CutStatsByPos[new Tuple<NoteLineLayer, int>(noteLineLayer, noteRow)].AddGoodCut(goodCut);
                    _statData.CutStatsByPosDir[new Tuple<NoteLineLayer, int,NoteCutDirection>(noteLineLayer, noteRow, cutDirection)].AddGoodCut(goodCut);

                    _statData.CutStatsBySaberPos[new Tuple<SaberType, NoteLineLayer, int>(saberType,noteLineLayer, noteRow)].AddGoodCut(goodCut);
                    _statData.CutStatsBySaberDir[new Tuple<SaberType, NoteCutDirection>(saberType, cutDirection)].AddGoodCut(goodCut);

                    _statData.CutStatsBySaberPosDir[new Tuple<SaberType,NoteLineLayer, int, NoteCutDirection>(saberType,noteLineLayer, noteRow, cutDirection)].AddGoodCut(goodCut);

                }
            }
            else if (scoringElement is BadCutScoringElement)
            {
                BadCutScoringElement badCut = scoringElement as BadCutScoringElement;
                _statData.badCuts++;
            }
            else if (scoringElement is MissScoringElement)
            {
                MissScoringElement miss = scoringElement as MissScoringElement;
                _statData.misses++;
            }
            _statData.Changed();
        }

        private void ComboController_comboDidChangeEvent(int combo)
        {
            _statData.combo = combo;
            _statData.Changed();
        }
        private void ComboController_comboBreakingEventHappenedEvent()
        {
            _statData.comboBreaks++;
            _statData.Changed();
        }

    }
}