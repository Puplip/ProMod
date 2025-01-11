using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProMod.Stats
{
    using NotePosition = (NoteLineLayer, int);
    using NoteDirection = NoteCutDirection;
    using NoteType = NoteData.ScoringType;

    public class ProStatDimension<K, V> : IEnumerable<KeyValuePair<K, V>>, IProStatTreeMember where V : ProStatNode, new()
    {
        public IProStatData rootStatData
        {
            set
            {
                foreach (var keyValue in this)
                {
                    keyValue.Value.rootStatData = value;
                }
            }
        }

        private static List<NoteType> noteTypes = new List<NoteType>()
        {
            NoteType.Normal,
            NoteType.SliderHead,
            NoteType.SliderTail,
            NoteType.BurstSliderHead,
            NoteType.BurstSliderElement
        };
        private static List<SaberType> saberTypes = new List<SaberType>() { SaberType.SaberA, SaberType.SaberB };
        private static List<NoteDirection> noteDirections = new List<NoteDirection>()
        {
            NoteDirection.Up,
            NoteDirection.Down,
            NoteDirection.Left,
            NoteDirection.Right,
            NoteDirection.UpLeft,
            NoteDirection.UpRight,
            NoteDirection.DownLeft,
            NoteDirection.DownRight,
            NoteDirection.Any
        };
        private static List<NotePosition> notePositions = new List<NotePosition>()
        {
            (NoteLineLayer.Base,0),
            (NoteLineLayer.Base,1),
            (NoteLineLayer.Base,2),
            (NoteLineLayer.Base,3),
            (NoteLineLayer.Upper,0),
            (NoteLineLayer.Upper,1),
            (NoteLineLayer.Upper,2),
            (NoteLineLayer.Upper,3),
            (NoteLineLayer.Top,0),
            (NoteLineLayer.Top,1),
            (NoteLineLayer.Top,2),
            (NoteLineLayer.Top,3)
        };

        private static Dictionary<Type, object> keyListByType = new Dictionary<Type, object>()
        {
            {typeof(NoteType), noteTypes },
            {typeof(SaberType), saberTypes },
            {typeof(NoteDirection), noteDirections },
            {typeof(NotePosition), notePositions }
        };

        private Dictionary<K, V> statsByKey = new Dictionary<K, V>();
        public V this[K key] => statsByKey[key];

        public ProStatDimension()
        {
            foreach (K key in keyListByType[typeof(K)] as List<K>)
            {
                statsByKey[key] = Activator.CreateInstance<V>();
            }
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return statsByKey.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void ScoreElement(ScoringElement scoringElement)
        {
            if (typeof(K) == typeof(NoteType))
            {
                K key = (K)(scoringElement.noteData.scoringType as object);
                if (!statsByKey.ContainsKey(key)) { return; }
                this[key].ScoreElement(scoringElement);
            }
            else if (typeof(K) == typeof(SaberType))
            {
                if (scoringElement.noteData.colorType == ColorType.None) { return; }
                K key = (K)(scoringElement.noteData.colorType.ToSaberType() as object);
                if (!statsByKey.ContainsKey(key)) { return; }
                this[key].ScoreElement(scoringElement);
            }
            else if (typeof(K) == typeof(NoteDirection))
            {
                K key = (K)(scoringElement.noteData.cutDirection as object);
                if (!statsByKey.ContainsKey(key)) { return; }
                this[key].ScoreElement(scoringElement);
            }
            else if (typeof(K) == typeof(NotePosition))
            {
                K key = (K)((scoringElement.noteData.noteLineLayer, scoringElement.noteData.lineIndex) as object);
                if (!statsByKey.ContainsKey(key)) { return; }
                this[key].ScoreElement(scoringElement);
            }
        }

        public void CutBomb(NoteController noteController, NoteCutInfo noteCutInfo)
        {
            if (typeof(K) == typeof(SaberType))
            {
                K key = (K)(noteCutInfo.saberType as object);
                if (!statsByKey.ContainsKey(key)) { return; }
                this[key].CutBomb(noteController, noteCutInfo);
            }
            else if (typeof(K) == typeof(NoteDirection))
            {
                foreach (var keyValue in this)
                {
                    keyValue.Value.CutBomb(noteController, noteCutInfo);
                }
            }
            else if (typeof(K) == typeof(NotePosition))
            {
                K key = (K)((noteCutInfo.noteData.noteLineLayer, noteCutInfo.noteData.lineIndex) as object);
                if (!statsByKey.ContainsKey(key)) { return; }
                this[key].CutBomb(noteController, noteCutInfo);
            }
        }
    }

    public class ProStatNode<K, V> : ProStatNode, IEnumerable<KeyValuePair<K, V>> where V : ProStatNode, new()
    {
        ProStatDimension<K, V> statDimension = new ProStatDimension<K, V>();
        public V this[K key] => statDimension[key];
        public override IProStatData rootStatData
        {
            set
            {
                base.rootStatData = value;
                foreach (var keyValue in this)
                {
                    keyValue.Value.rootStatData = value;
                }
            }
        }
        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return statDimension.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override void ScoreElement(ScoringElement scoringElement)
        {
            base.ScoreElement(scoringElement);
            statDimension.ScoreElement(scoringElement);
        }
        public override void CutBomb(NoteController noteController, NoteCutInfo noteCutInfo)
        {
            base.CutBomb(noteController, noteCutInfo);
            statDimension.CutBomb(noteController, noteCutInfo);
        }
    }

    public class ProStatNode<K, V, K2, V2> : ProStatNode where V : ProStatNode, new() where V2 : ProStatNode, new()
    {
        private ProStatDimension<K, V> statDimension = new ProStatDimension<K, V>();
        private ProStatDimension<K2, V2> statDimension2 = new ProStatDimension<K2, V2>();

        public ProStatDimension<K, V> First => statDimension;
        public ProStatDimension<K2, V2> Second => statDimension2;
        public V this[K key] => statDimension[key];
        public V2 this[K2 key] => statDimension2[key];

        public override IProStatData rootStatData
        {
            set
            {
                base.rootStatData = value;
                foreach (var keyValue in statDimension)
                {
                    keyValue.Value.rootStatData = value;
                }
                foreach (var keyValue in statDimension2)
                {
                    keyValue.Value.rootStatData = value;
                }
            }
        }

        public override void ScoreElement(ScoringElement scoringElement)
        {
            base.ScoreElement(scoringElement);
            statDimension.ScoreElement(scoringElement);
            statDimension2.ScoreElement(scoringElement);
        }

        public override void CutBomb(NoteController noteController, NoteCutInfo noteCutInfo)
        {
            base.CutBomb(noteController, noteCutInfo);
            statDimension.CutBomb(noteController, noteCutInfo);
            statDimension2.CutBomb(noteController, noteCutInfo);
        }
    }
}
