namespace ProMod.Stats
{
    public class ProStat_InstantAcc : ProStatTitleValue
    {
        protected override string Title { get; set; } = "ACC";
        protected override string Value(ProStatData proStatData)
        {
            return RatioColor(proStatData.acc) + RatioValue(proStatData.acc);
        }
    }

    public class ProStat_MaxAcc : ProStatTitleValue
    {
        protected override string Title { get; set; } = "Max ACC";
        protected override string Value(ProStatData proStatData)
        {
            float acc = (float)(proStatData.maxBeatmapScore - proStatData.maxCurrentScore + proStatData.score) / (float)proStatData.maxBeatmapScore;
            return RatioColor(acc) + RatioValue(acc);
        }
    }

    public class ProStat_EstimateAcc : ProStatTitleValue
    {
        protected override string Title { get; set; } = "Est ACC";
        protected override string Value(ProStatData proStatData)
        {
            float acc = ((float)(proStatData.maxBeatmapScore - proStatData.maxCurrentScore) * proStatData.CutStats.CutScore.Average() / 115.0f + (float)proStatData.score) / (float)proStatData.maxBeatmapScore;
            return RatioColor(acc) + RatioValue(acc);
        }
    }

    public class ProStat_Combo : ProStatTitleValue
    {
        protected override string Title { get; set; } = "Combo";
        protected override string Value(ProStatData proStatData)
        {
            return IntValue(proStatData.combo);
        }
    }

    public class ProStat_ComboDamage : ProStatTitleValue
    {
        protected override string Title { get; set; } = "Combo Dmg";
        protected override string Value(ProStatData proStatData)
        {
            return RatioValue((float)proStatData.comboDamage / (float)proStatData.maxBeatmapScore);
        }
    }

    public class ProStat_TimeLeft : ProStatTitleValue
    {
        protected override string Title { get; set; } = "Time Left";
        protected override string Value(ProStatData proStatData)
        {
            return MinuteSecondValue(proStatData.songLength - proStatData.songTime);
        }
    }

    public class ProStat_LeftAcc : ProStatTitleValue
    {
        protected override string Title { get; set; } = "L ACC";
        protected override string Value(ProStatData proStatData)
        {
            float acc = proStatData.CutStatsBySaber[SaberType.SaberA].CutScore.Average() / 115.0f;
            return RatioColor(acc) + RatioValue(acc);
        }
    }

    public class ProStat_RightAcc : ProStatTitleValue
    {
        protected override string Title { get; set; } = "R ACC";
        protected override string Value(ProStatData proStatData)
        {
            float acc = proStatData.CutStatsBySaber[SaberType.SaberB].CutScore.Average() / 115.0f;
            return RatioColor(acc) + RatioValue(acc);
        }
    }

    public class ProStat_LeftSwingScore : ProStatTitleValue
    {
        protected override string Title { get; set; } = "L Swing";
        protected override string Value(ProStatData proStatData)
        {
            float acc = proStatData.CutStatsBySaber[SaberType.SaberA].CutScoreSwing.Average() / 100.0f;
            return RatioValue(acc);
        }
    }

    public class ProStat_RightSwingScore : ProStatTitleValue
    {
        protected override string Title { get; set; } = "R Swing";
        protected override string Value(ProStatData proStatData)
        {
            float acc = proStatData.CutStatsBySaber[SaberType.SaberB].CutScoreSwing.Average() / 100.0f;
            return RatioValue(acc);
        }
    }

    public class ProStat_LeftPreSwingScore : ProStatTitleValue
    {
        protected override string Title { get; set; } = "L PreSwing";
        protected override string Value(ProStatData proStatData)
        {
            float acc = proStatData.CutStatsBySaber[SaberType.SaberA].CutScorePreSwing.Average() / 100.0f;
            return RatioValue(acc);
        }
    }

    public class ProStat_RightPreSwingScore : ProStatTitleValue
    {
        protected override string Title { get; set; } = "R PreSwing";
        protected override string Value(ProStatData proStatData)
        {
            float acc = proStatData.CutStatsBySaber[SaberType.SaberB].CutScorePreSwing.Average() / 100.0f;
            return RatioValue(acc);
        }
    }

    public class ProStat_LeftPostSwingScore : ProStatTitleValue
    {
        protected override string Title { get; set; } = "L PostSwing";
        protected override string Value(ProStatData proStatData)
        {
            float acc = proStatData.CutStatsBySaber[SaberType.SaberA].CutScorePostSwing.Average() / 100.0f;
            return RatioValue(acc);
        }
    }

    public class ProStat_RightPostSwingScore : ProStatTitleValue
    {
        protected override string Title { get; set; } = "R PostSwing";
        protected override string Value(ProStatData proStatData)
        {
            float acc = proStatData.CutStatsBySaber[SaberType.SaberB].CutScorePostSwing.Average() / 100.0f;
            return RatioValue(acc);
        }
    }
    public class ProStat_LeftAimScore : ProStatTitleValue
    {
        protected override string Title { get; set; } = "L Error";
        protected override string Value(ProStatData proStatData)
        {
            return MillimeterValue(proStatData.CutStatsBySaber[SaberType.SaberA].CutScoreAim.Average() / 100.0f);
        }
    }

    public class ProStat_RightAimScore : ProStatTitleValue
    {
        protected override string Title { get; set; } = "R Error";
        protected override string Value(ProStatData proStatData)
        {
            return MillimeterValue(proStatData.CutStatsBySaber[SaberType.SaberB].CutScoreAim.Average() / 100.0f);
        }
    }

    public class ProStat_LeftAimDistance : ProStatTitleValue
    {
        protected override string Title { get; set; } = "L Error";
        protected override string Value(ProStatData proStatData)
        {
            return MillimeterValue(proStatData.CutStatsBySaber[SaberType.SaberA].CutPosDeviation.Average() * 1000.0f);
        }
    }

    public class ProStat_RightAimDistance : ProStatTitleValue
    {
        protected override string Title { get; set; } = "R Error";
        protected override string Value(ProStatData proStatData)
        {
            return MillimeterValue(proStatData.CutStatsBySaber[SaberType.SaberB].CutPosDeviation.Average() * 1000.0f);
        }
    }

    public class ProStat_LeftTimeDependence : ProStatTitleValue
    {
        protected override string Title { get; set; } = "L TD";
        protected override string Value(ProStatData proStatData)
        {
            return AngleValue(proStatData.CutStatsBySaber[SaberType.SaberA].TimeDependence.Average());
        }
    }

    public class ProStat_RightTimeDependence : ProStatTitleValue
    {
        protected override string Title { get; set; } = "R TD";
        protected override string Value(ProStatData proStatData)
        {
            return AngleValue(proStatData.CutStatsBySaber[SaberType.SaberB].TimeDependence.Average());
        }
    }

    public class ProStat_LeftSwingDamage : ProStatTitleValue
    {
        protected override string Title { get; set; } = "L Swing Dmg";
        protected override string Value(ProStatData proStatData)
        {
            ProCutStats cutStats = proStatData.CutStatsBySaber[SaberType.SaberA];
            return RatioValue((float)(cutStats.PreSwingDamage.Value() + cutStats.PostSwingDamage.Value()) / (float)proStatData.maxBeatmapScore);
        }
    }

    public class ProStat_RightSwingDamage : ProStatTitleValue
    {
        protected override string Title { get; set; } = "R Swing Dmg";
        protected override string Value(ProStatData proStatData)
        {
            ProCutStats cutStats = proStatData.CutStatsBySaber[SaberType.SaberB];
            return RatioValue((float)(cutStats.PreSwingDamage.Value() + cutStats.PostSwingDamage.Value()) / (float)proStatData.maxBeatmapScore);
        }
    }
    public class ProStat_LeftPreSwingDamage : ProStatTitleValue
    {
        protected override string Title { get; set; } = "L PreSwing Dmg";
        protected override string Value(ProStatData proStatData)
        {
            ProCutStats cutStats = proStatData.CutStatsBySaber[SaberType.SaberA];
            return RatioValue((float)cutStats.PreSwingDamage.Value() / (float)proStatData.maxBeatmapScore);
        }
    }

    public class ProStat_RightPreSwingDamage : ProStatTitleValue
    {
        protected override string Title { get; set; } = "R PreSwing Dmg";
        protected override string Value(ProStatData proStatData)
        {
            ProCutStats cutStats = proStatData.CutStatsBySaber[SaberType.SaberB];
            return RatioValue((float)cutStats.PreSwingDamage.Value() / (float)proStatData.maxBeatmapScore);
        }
    }
    public class ProStat_LeftPostSwingDamage : ProStatTitleValue
    {
        protected override string Title { get; set; } = "L PostSwing Dmg";
        protected override string Value(ProStatData proStatData)
        {
            ProCutStats cutStats = proStatData.CutStatsBySaber[SaberType.SaberA];
            return RatioValue((float)cutStats.PostSwingDamage.Value() / (float)proStatData.maxBeatmapScore);
        }
    }

    public class ProStat_RightPostSwingDamage : ProStatTitleValue
    {
        protected override string Title { get; set; } = "R PostSwing Dmg";
        protected override string Value(ProStatData proStatData)
        {
            ProCutStats cutStats = proStatData.CutStatsBySaber[SaberType.SaberB];
            return RatioValue((float)cutStats.PostSwingDamage.Value() / (float)proStatData.maxBeatmapScore);
        }
    }

    public class ProStat_LeftAimDamage : ProStatTitleValue
    {
        protected override string Title { get; set; } = "L Aim Dmg";
        protected override string Value(ProStatData proStatData)
        {
            ProCutStats cutStats = proStatData.CutStatsBySaber[SaberType.SaberA];
            return RatioValue((float)cutStats.AimDamage.Value() / (float)proStatData.maxBeatmapScore);
        }
    }

    public class ProStat_RightAimDamage : ProStatTitleValue
    {
        protected override string Title { get; set; } = "R Aim Dmg";
        protected override string Value(ProStatData proStatData)
        {
            ProCutStats cutStats = proStatData.CutStatsBySaber[SaberType.SaberB];
            return RatioValue((float)cutStats.AimDamage.Value() / (float)proStatData.maxBeatmapScore);
        }
    }
}