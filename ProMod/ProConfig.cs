
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using IPA.Config.Stores;
using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using UnityEngine;


namespace ProMod
{
    public class ProConfig
    {
        public virtual bool HitScoreColorsEnabled { get; set; } = true;

        public virtual bool ShowHeightGuide { get; set; } = false;

        public class ProColorConfig
        {
            public virtual float r { get; set; } = 0.0f;
            public virtual float g { get; set; } = 0.0f;
            public virtual float b { get; set; } = 0.0f;

            public ProColorConfig(float _r, float _g, float _b) {
                r = _r;
                g = _g;
                b = _b;
            }

            public Color UnityGetUnityColor()
            {
                return new Color(r, g, b);
            }
        }

        [UseConverter]
        public virtual Dictionary<int, ProColorConfig> HitScoreColors { get; set; } = new Dictionary<int, ProColorConfig>();

        public virtual bool HitScoreSizesEnabled { get; set; } = true;

        [UseConverter]
        public virtual Dictionary<int, float> HitScoreSizes { get; set; } = new Dictionary<int, float>();

        public virtual bool ReactionTimeEnabled { get; set; } = true;

        [UseConverter]
        public virtual Dictionary<float, float> JumpDistanceCurve { get; set; } = new Dictionary<float, float>();

        /// <summary>
        /// This is called whenever BSIPA reads the config from disk (including when file changes are detected).
        /// </summary>
        public virtual void OnReload()
        {

            if (HitScoreColors.Count == 0) {
                HitScoreColors.Add(114, new ProColorConfig(1.0f, 0.85f, 0.0f));
                HitScoreColors.Add(112, new ProColorConfig(0.0f, 0.50f, 1.0f));
                HitScoreColors.Add(108, new ProColorConfig(0.95f, 0.95f, 0.95f));
                HitScoreColors.Add(100, new ProColorConfig(1.0f, 0.0f, 0.25f));
                HitScoreColors.Add(0, new ProColorConfig(0.75f, 0.0f, 0.0f));
            }

            if (HitScoreSizes.Count == 0)
            {
                HitScoreSizes.Add(112, 1.3f);
                HitScoreSizes.Add(108, 1.15f);
                HitScoreSizes.Add(0, 1.0f);
            }
        }

        /// <summary>
        /// Call this to force BSIPA to update the config file. This is also called by BSIPA if it detects the file was modified.
        /// </summary>
        public virtual void Changed()
        {
            // Do stuff when the config is changed.
        }

        /// <summary>
        /// Call this to have BSIPA copy the values from <paramref name="other"/> into this config.
        /// </summary>
        public virtual void CopyFrom(ProConfig other)
        {
            // This instance's members populated from other
        }
    }
}
