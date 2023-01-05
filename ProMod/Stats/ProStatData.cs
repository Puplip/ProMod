using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using UnityEngine;

namespace ProMod.Stats
{

    public class ProCount
    {
        public int Count { get; protected set; } = 0;
    }
    public class ProVectorStat {
        private double x;
        private double y;
        private double z;
        private ProCount proCount;
        public ProVectorStat(ProCount _proCount)
        {
            proCount = _proCount;
        }

        internal void Add(Vector3 v)
        {
            x += (double)v.x;
            y += (double)v.y;
            z += (double)v.z;
        }

        public Vector3 Average()
        {
            int count = proCount.Count;
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
        private ProCount proCount;
        public ProIntStat(ProCount _proCount)
        {
            proCount = _proCount;
        }

        internal void Add(int i)
        {
            value += i;
        }

        public float Average()
        {
            int count = proCount.Count;
            if (count > 0)
            {
                return (float)value / (float)count;
            }
            return 0;
        }

        public long Value()
        {
            return value;
        }

    }

    public class ProRealStat
    {
        private double value;
        private ProCount proCount;
        public ProRealStat(ProCount _proCount)
        {
            proCount = _proCount;
        }

        internal void Add(float i)
        {
            value += i;
        }

        public float Average()
        {
            int count = proCount.Count;
            if (count > 0)
            {
                return (float)value / (float)count;
            }
            return 0;
        }

    }

    public class ProIntHistogram
    {
        private int[] data;

        private long sum;
        
        internal void Increment(int i)
        {
            if( i >= data.Length || i < 0) { return; }

            data[i]++;
            sum++;
        }

        internal void Add(int i,int v)
        {
            if (i >= data.Length || i < 0) { return; }

            data[i] += v;
            sum += v;
        }

        public float GetFrequency(int i)
        {
            return (float)((double)data[i] / (double)sum);
        }
        public float GetFrequency(int min,int max)
        {
            if(min < 0 || max < 0 || min >= data.Length || max >= data.Length || min > max)
            {
                return -1.0f;
            }

            long acc = 0;

            for(int i = 0; i < data.Length; i++)
            {
                int v = data[i];
                if(i >= min && i <= max)
                {
                    acc += v;
                }
            }
            return (float)((double)acc / (double)sum);
        }

        public ProIntHistogram(int count)
        {
            data = new int[count];
            for(int i = 0; i < count; i++)
            {
                data[i] = 0;
            }
        }
    }

    public class ProCutStats : ProCount
    {

        public ProIntStat CutScore;
        public ProIntStat CutScoreAim;
        public ProIntStat CutScoreSwing;
        public ProIntStat CutScorePreSwing;
        public ProIntStat CutScorePostSwing;
        public ProRealStat TimeDependence;
        public ProRealStat TimeDeviation;
        public ProRealStat CutDirDeviation;
        public ProRealStat CutAngle;
        public ProRealStat CutPosDeviation;
        public ProVectorStat CutPoint;
        public ProIntStat PreSwingDamage;
        public ProIntStat PostSwingDamage;
        public ProIntStat AimDamage;

        public ProIntHistogram CutScoreHistogram = new ProIntHistogram(115);

        public ProCutStats()
        {
            CutScore = new ProIntStat(this as ProCount);
            CutScoreAim = new ProIntStat(this as ProCount);
            CutScoreSwing = new ProIntStat(this as ProCount);
            CutScorePreSwing = new ProIntStat(this as ProCount);
            CutScorePostSwing = new ProIntStat(this as ProCount);
            TimeDependence = new ProRealStat(this as ProCount);
            TimeDeviation = new ProRealStat(this as ProCount);
            CutDirDeviation = new ProRealStat(this as ProCount);
            CutAngle = new ProRealStat(this as ProCount);
            CutPosDeviation = new ProRealStat(this as ProCount);
            CutPoint = new ProVectorStat(this as ProCount);

            PreSwingDamage = new ProIntStat(this as ProCount);
            PostSwingDamage = new ProIntStat(this as ProCount);
            AimDamage = new ProIntStat(this as ProCount);
        }

        internal void AddGoodCut(GoodCutScoringElement goodCut)
        {
            Count++;
            CutScore.Add(goodCut.cutScoreBuffer.cutScore);
            CutScoreHistogram.Increment(goodCut.cutScoreBuffer.cutScore);
            CutScoreAim.Add(goodCut.cutScoreBuffer.centerDistanceCutScore);
            CutScoreSwing.Add(goodCut.cutScoreBuffer.afterCutScore + goodCut.cutScoreBuffer.beforeCutScore);
            CutScorePreSwing.Add(goodCut.cutScoreBuffer.beforeCutScore);
            CutScorePostSwing.Add(goodCut.cutScoreBuffer.afterCutScore);
            TimeDependence.Add(Mathf.Abs(Mathf.Asin(goodCut.cutScoreBuffer.noteCutInfo.cutNormal.z) / Mathf.PI * 180.0f));
            TimeDeviation.Add(goodCut.cutScoreBuffer.noteCutInfo.timeDeviation * 1000.0f);
            CutDirDeviation.Add(goodCut.cutScoreBuffer.noteCutInfo.cutDirDeviation);
            CutAngle.Add(goodCut.cutScoreBuffer.noteCutInfo.cutAngle);
            CutPosDeviation.Add(goodCut.cutScoreBuffer.noteCutInfo.cutDistanceToCenter);
            CutPoint.Add(goodCut.cutScoreBuffer.noteCutInfo.cutPoint);

            PreSwingDamage.Add((70 - goodCut.cutScoreBuffer.beforeCutScore) * goodCut.multiplier);
            PostSwingDamage.Add((30 - goodCut.cutScoreBuffer.afterCutScore) * goodCut.multiplier);
            PostSwingDamage.Add((15 - goodCut.cutScoreBuffer.centerDistanceCutScore) * goodCut.multiplier);
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
        public int maxCurrentScore;
        public int maxBeatmapScore;
        public int combo;
        public int comboBreaks;
        public int comboDamage;
        public int misses;
        public int badCuts;
        public int bombHits;
        public int wallTouches;
        public float songTime;
        public float songLength;

        public float acc
        {
            get => (maxCurrentScore > 0) ? (float)score / (float)maxCurrentScore : 1.0f; 
        }

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