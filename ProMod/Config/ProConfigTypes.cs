using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using IPA.Config.Stores;
using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ProMod.Config
{
    public class ProColorSerializable
    {
        public float R = 1.0f;
        public float G = 1.0f;
        public float B = 1.0f;

        public static implicit operator Color(ProColorSerializable proColor)
        {
            return new Color(proColor.R, proColor.G, proColor.B);
        }

        public static implicit operator ProColorSerializable(Color proColor)
        {
            return new ProColorSerializable() { R = proColor.r, G = proColor.g, B = proColor.b };
        }
    }
    public class ProVector2Serializable
    {
        public float X = 1.0f;
        public float Y = 1.0f;

        public static implicit operator Vector2(ProVector2Serializable proVector2)
        {
            return new Vector2(proVector2.X, proVector2.Y);
        }

        public static implicit operator ProVector2Serializable(Vector2 vector2)
        {
            return new ProVector2Serializable() { X = vector2.x, Y = vector2.y };
        }
    }
    public class ProVector3Serializable
    {
        public float X = 1.0f;
        public float Y = 1.0f;
        public float Z = 1.0f;

        public static implicit operator Vector3(ProVector3Serializable proVector)
        {
            return new Vector3(proVector.X, proVector.Y, proVector.Z);
        }

        public static implicit operator ProVector3Serializable(Vector3 vector3)
        {
            return new ProVector3Serializable() { X = vector3.x, Y = vector3.y, Z = vector3.z };
        }
    }
    public class ProCutScoreConfig : IComparable<ProCutScoreConfig>
    {
        [JsonProperty("Score")]
        public int score = 0;
        [JsonProperty("Color")]
        public ProColorSerializable color = new Color(1.0f,1.0f,1.0f);

        [JsonProperty("Size")]
        public int size = 100;
        public ProCutScoreConfig(int _score, Color _color, int _size)
        {
            score = _score;
            color = _color;
            size = _size;
        }
        public ProCutScoreConfig() { }

        public int CompareTo(ProCutScoreConfig other)
        {
            return other.score.CompareTo(score);
        }
    }

    public class ProAccColorConfig : IComparable<ProAccColorConfig>
    {
        [JsonProperty("Score")]
        public int score = 0;
        [JsonProperty("Color")]
        public ProColorSerializable color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        public ProAccColorConfig(int _score, Color _color)
        {
            score = _score;
            color = _color;
        }
        public ProAccColorConfig() { }

        public int CompareTo(ProAccColorConfig other)
        {
            return other.score.CompareTo(score);
        }
    }

    public class ProReactionTimePoint : IComparable<ProReactionTimePoint>
    {

        [JsonProperty("NJS")]
        public float njs = 10.0f;
        [JsonProperty("RT")]
        public float rt = 420.0f;

        public ProReactionTimePoint(float _njs, float _rt)
        {
            njs = _njs;
            rt = _rt;
        }
        public ProReactionTimePoint() { }

        public int CompareTo(ProReactionTimePoint other)
        {
            return njs.CompareTo(other.njs);
        }
    }

    public class ProStatConfig
    {
        [JsonProperty("Name")]
        public string name = "";

        [JsonConverter(typeof(StringEnumConverter)), JsonProperty("Location")]
        public Stats.ProStatLocation location = Stats.ProStatLocation.Custom;

        [JsonProperty("CustomLocation")]
        public Stats.ProStatLocationData customLocation { get; set; } = new Stats.ProStatLocationData();

        public ProStatConfig(string _name, Stats.ProStatLocation _location)
        {
            name = _name;
            location = _location;
        }
        public ProStatConfig(string _name, Stats.ProStatLocation _location, Stats.ProStatLocationData _customLocation)
        {
            name = _name;
            location = _location;
            customLocation = _customLocation;
        }
        public ProStatConfig() { }
    }
}