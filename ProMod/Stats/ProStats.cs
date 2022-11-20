using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using UnityEngine;

namespace ProMod.Stats
{

    public class ProVectorStat {
        private double x;
        private double y;
        private double z;

        internal void Add(Vector3 v)
        {
            x += (double)v.x;
            y += (double)v.y;
            z += (double)v.z;
        }

        public Vector3 Average(int count)
        {
            if(count > 0)
            {
                return new Vector3((float)(x / (double)count), (float)(y / (double)count), (float)(z / (double)count));
            }
            return new Vector3();
        }
    }
    public class ProIntStat
    {
        private long value;

        internal void Add(int i)
        {
            value += i;
        }

        public float Average(int count)
        {
            if (count > 0)
            {
                return (float)value / (float)count;
            }
            return 0;
        }

    }

    public class ProRealStat
    {
        private double value;

        internal void Add(float i)
        {
            value += i;
        }

        public float Average(int count)
        {
            if (count > 0)
            {
                return (float)value / (float)count;
            }
            return 0;
        }

    }

    public class ProCutStats
    {
        public int Count { get; private set; }

        public ProIntStat CutScore = new ProIntStat();
        public ProIntStat CutScoreAim = new ProIntStat();
        public ProIntStat CutScoreSwing = new ProIntStat();
        public ProIntStat CutScorePreSwing = new ProIntStat();
        public ProIntStat CutScorePostSwing = new ProIntStat();

        public ProRealStat TimeDependence = new ProRealStat();
        public ProRealStat TimeDeviation = new ProRealStat();
        public ProRealStat CutDirDeviation = new ProRealStat();
        public ProRealStat CutAngle = new ProRealStat();
        public ProRealStat CutPosDeviation = new ProRealStat();

        public ProVectorStat CutPoint = new ProVectorStat();
        internal void AddGoodCut(GoodCutScoringElement goodCut)
        {
            Count++;
            CutScore.Add(goodCut.cutScoreBuffer.cutScore);
            CutScoreAim.Add(goodCut.cutScoreBuffer.centerDistanceCutScore);
            CutScoreSwing.Add(goodCut.cutScoreBuffer.afterCutScore + goodCut.cutScoreBuffer.beforeCutScore);
            CutScorePreSwing.Add(goodCut.cutScoreBuffer.beforeCutScore);
            CutScorePostSwing.Add(goodCut.cutScoreBuffer.afterCutScore);
            TimeDependence.Add(Mathf.Abs(goodCut.cutScoreBuffer.noteCutInfo.cutNormal.z * 100.0f));
            TimeDeviation.Add(goodCut.cutScoreBuffer.noteCutInfo.timeDeviation);
            CutDirDeviation.Add(goodCut.cutScoreBuffer.noteCutInfo.cutDirDeviation);
            
            CutAngle.Add(goodCut.cutScoreBuffer.noteCutInfo.cutAngle);
            CutPosDeviation.Add(goodCut.cutScoreBuffer.noteCutInfo.cutDistanceToCenter);
            
            CutPoint.Add(goodCut.cutScoreBuffer.noteCutInfo.cutPoint);
        }
    }



    public class ProStatData
    {
        public ProCutStats CutStats = new ProCutStats();

        public Dictionary<SaberType, ProCutStats> CutStatsBySaber = new Dictionary<SaberType, ProCutStats>();

        public Dictionary<NoteCutDirection, ProCutStats> CutStatsByDir = new Dictionary<NoteCutDirection, ProCutStats>();
        public Dictionary<Tuple<NoteLineLayer, int>, ProCutStats> CutStatsByPos = new Dictionary<Tuple<NoteLineLayer, int>, ProCutStats>();
        public Dictionary<Tuple<NoteLineLayer, int, NoteCutDirection>, ProCutStats> CutStatsByPosDir = new Dictionary<Tuple<NoteLineLayer, int, NoteCutDirection>, ProCutStats>();

        public Dictionary<Tuple<SaberType,NoteLineLayer,int>, ProCutStats> CutStatsBySaberPos = new Dictionary<Tuple<SaberType, NoteLineLayer, int>, ProCutStats>();
        public Dictionary<Tuple<SaberType, NoteCutDirection>, ProCutStats> CutStatsBySaberDir = new Dictionary<Tuple<SaberType, NoteCutDirection>, ProCutStats>();
        public Dictionary<Tuple<SaberType, NoteLineLayer, int, NoteCutDirection>, ProCutStats> CutStatsBySaberPosDir = new Dictionary<Tuple<SaberType, NoteLineLayer, int, NoteCutDirection>, ProCutStats>();

        public int score;
        public int maxScore;
        public int combo;
        public int comboBreaks;
        public int comboPenalty;
        public int misses;
        public int badCuts;
        public int bombHits;
        public int wallTouches;

        private readonly static SaberType[] saberTypes = { SaberType.SaberA, SaberType.SaberB };
        private readonly static NoteLineLayer[] noteLineLayers = { NoteLineLayer.Base, NoteLineLayer.Upper, NoteLineLayer.Top };
        private readonly static int[] noteRows = { 0, 1, 2, 3 };
        private readonly static NoteCutDirection[] cutDirections = {
            NoteCutDirection.Up,
            NoteCutDirection.Down,
            NoteCutDirection.Left,
            NoteCutDirection.Right,
            NoteCutDirection.UpLeft,
            NoteCutDirection.UpRight,
            NoteCutDirection.DownLeft,
            NoteCutDirection.DownRight,
            NoteCutDirection.Any
        };

        public event Action onChangeEvent;

        internal void Changed()
        {
            onChangeEvent();
        }

        public ProStatData()
        {
            foreach (NoteCutDirection cutDirection in cutDirections)
            {
                CutStatsByDir.Add(cutDirection, new ProCutStats());
            }
            foreach (NoteLineLayer noteLineLayer in noteLineLayers)
            {
                foreach (int noteRow in noteRows)
                {
                    CutStatsByPos.Add(new Tuple<NoteLineLayer, int>(noteLineLayer, noteRow), new ProCutStats());
                    foreach (NoteCutDirection noteCutDirection in cutDirections)
                    {
                        CutStatsByPosDir.Add(new Tuple<NoteLineLayer, int, NoteCutDirection>(noteLineLayer, noteRow, noteCutDirection), new ProCutStats());
                    }
                }
            }
            foreach (SaberType saberType in saberTypes)
            {
                CutStatsBySaber.Add(saberType, new ProCutStats());
                foreach (NoteLineLayer noteLineLayer in noteLineLayers)
                {
                    foreach (int noteRow in noteRows)
                    {
                        CutStatsBySaberPos.Add(new Tuple<SaberType, NoteLineLayer, int>(saberType, noteLineLayer, noteRow),new ProCutStats());
                        foreach (NoteCutDirection noteCutDirection in cutDirections)
                        {
                            CutStatsBySaberPosDir.Add(new Tuple<SaberType,NoteLineLayer, int, NoteCutDirection>(saberType,noteLineLayer, noteRow, noteCutDirection), new ProCutStats());
                        }
                    }
                }
                foreach (NoteCutDirection noteCutDirection in cutDirections)
                {
                    CutStatsBySaberDir.Add(new Tuple<SaberType, NoteCutDirection>(saberType, noteCutDirection), new ProCutStats());
                }
            }
        }
    }
}