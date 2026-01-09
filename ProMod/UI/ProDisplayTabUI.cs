using System;
using System.Collections.Generic;
using UnityEngine;
using BeatSaberMarkupLanguage.Attributes;
using Zenject;
using System.ComponentModel;
using HMUI;

namespace ProMod.UI;

internal class ProDisplayTabUI : ProUI, IInitializable, IDisposable
{
    public void Initialize()
    {

    }

    public void Dispose()
    {

    }

    #region GameplayEffects

    [UIValue("UIValue_DebrisEnabled")]
    private bool UIValue_DebrisEnabled
    {
        get => Plugin.Config.gameplayEffects.debris;
        set { Plugin.Config.gameplayEffects.debris = value; Plugin.Config.Save(); }
    }

    [UIValue("UIValue_ParticlesEnabled")]
    private bool UIValue_ParticlesEnabled
    {
        get => Plugin.Config.gameplayEffects.particles;
        set { Plugin.Config.gameplayEffects.particles = value; Plugin.Config.Save(); }
    }

    [UIValue("UIValue_VibrationEnabled")]
    private bool UIValue_VibrationEnabled
    {
        get => Plugin.Config.gameplayEffects.vibration;
        set { Plugin.Config.gameplayEffects.vibration = value; Plugin.Config.Save(); }
    }

    [UIValue("UIValue_ShockwavesEnabled")]
    private bool UIValue_ShockwavesEnabled
    {
        get => Plugin.Config.gameplayEffects.shockwaves;
        set { Plugin.Config.gameplayEffects.shockwaves = value; Plugin.Config.Save(); }
    }

    [UIValue("UIValue_BombExplosionsEnabled")]
    private bool UIValue_BombExplosionsEnabled
    {
        get => Plugin.Config.gameplayEffects.bombExplosions;
        set { Plugin.Config.gameplayEffects.bombExplosions = value; Plugin.Config.Save(); }
    }

    #endregion

    #region HMDLayers

    [UIValue("UIValue_HMDLayerDefault")]
    private bool UIValue_HMDLayerDefault
    {
        get => Plugin.Config.hmdCameraMask.Default;
        set { Plugin.Config.hmdCameraMask.Default = value; Plugin.Config.Save(); }
    }
    [UIValue("UIValue_HMDLayerTransparentFX")]
    private bool UIValue_HMDLayerTransparentFX
    {
        get => Plugin.Config.hmdCameraMask.TransparentFX;
        set { Plugin.Config.hmdCameraMask.TransparentFX = value; Plugin.Config.Save(); }
    }
    [UIValue("UIValue_HMDLayerIgnoreRaycast")]
    private bool UIValue_HMDLayerIgnoreRaycast
    {
        get => Plugin.Config.hmdCameraMask.IgnoreRaycast;
        set { Plugin.Config.hmdCameraMask.IgnoreRaycast = value; Plugin.Config.Save(); }
    }
    [UIValue("UIValue_HMDLayerThirdPerson")]
    private bool UIValue_HMDLayerThirdPerson
    {
        get => Plugin.Config.hmdCameraMask.ThirdPerson;
        set { Plugin.Config.hmdCameraMask.ThirdPerson = value; Plugin.Config.Save(); }
    }
    [UIValue("UIValue_HMDLayerWater")]
    private bool UIValue_HMDLayerWater
    {
        get => Plugin.Config.hmdCameraMask.Water;
        set { Plugin.Config.hmdCameraMask.Water = value; Plugin.Config.Save(); }
    }
    [UIValue("UIValue_HMDLayerUI")]
    private bool UIValue_HMDLayerUI
    {
        get => Plugin.Config.hmdCameraMask.UI;
        set { Plugin.Config.hmdCameraMask.UI = value; Plugin.Config.Save(); }
    }
    [UIValue("UIValue_HMDLayerFirstPerson")]
    private bool UIValue_HMDLayerFirstPerson
    {
        get => Plugin.Config.hmdCameraMask.FirstPerson;
        set { Plugin.Config.hmdCameraMask.FirstPerson = value; Plugin.Config.Save(); }
    }
    [UIValue("UIValue_HMDLayerScreenDisplacement")]
    private bool UIValue_HMDLayerScreenDisplacement
    {
        get => Plugin.Config.hmdCameraMask.ScreenDisplacement;
        set { Plugin.Config.hmdCameraMask.ScreenDisplacement = value; Plugin.Config.Save(); }
    }
    [UIValue("UIValue_HMDLayerNote")]
    private bool UIValue_HMDLayerNote
    {
        get => Plugin.Config.hmdCameraMask.Note;
        set { Plugin.Config.hmdCameraMask.Note = value; Plugin.Config.Save(); }
    }
    [UIValue("UIValue_HMDLayerNoteDebris")]
    private bool UIValue_HMDLayerNoteDebris
    {
        get => Plugin.Config.hmdCameraMask.NoteDebris;
        set { Plugin.Config.hmdCameraMask.NoteDebris = value; Plugin.Config.Save(); }
    }
    [UIValue("UIValue_HMDLayerAvatar")]
    private bool UIValue_HMDLayerAvatar
    {
        get => Plugin.Config.hmdCameraMask.Avatar;
        set { Plugin.Config.hmdCameraMask.Avatar = value; Plugin.Config.Save(); }
    }
    [UIValue("UIValue_HMDLayerObstacle")]
    private bool UIValue_HMDLayerObstacle
    {
        get => Plugin.Config.hmdCameraMask.Obstacle;
        set { Plugin.Config.hmdCameraMask.Obstacle = value; Plugin.Config.Save(); }
    }
    [UIValue("UIValue_HMDLayerSaber")]
    private bool UIValue_HMDLayerSaber
    {
        get => Plugin.Config.hmdCameraMask.Saber;
        set { Plugin.Config.hmdCameraMask.Saber = value; Plugin.Config.Save(); }
    }
    [UIValue("UIValue_HMDLayerNeonLight")]
    private bool UIValue_HMDLayerNeonLight
    {
        get => Plugin.Config.hmdCameraMask.NeonLight;
        set { Plugin.Config.hmdCameraMask.NeonLight = value; Plugin.Config.Save(); }
    }
    [UIValue("UIValue_HMDLayerEnvironment")]
    private bool UIValue_HMDLayerEnvironment
    {
        get => Plugin.Config.hmdCameraMask.Environment;
        set { Plugin.Config.hmdCameraMask.Environment = value; Plugin.Config.Save(); }
    }
    [UIValue("UIValue_HMDLayerGrabPassTexture1")]
    private bool UIValue_HMDLayerGrabPassTexture1
    {
        get => Plugin.Config.hmdCameraMask.GrabPassTexture1;
        set { Plugin.Config.hmdCameraMask.GrabPassTexture1 = value; Plugin.Config.Save(); }
    }
    [UIValue("UIValue_HMDLayerCutEffectParticles")]
    private bool UIValue_HMDLayerCutEffectParticles
    {
        get => Plugin.Config.hmdCameraMask.CutEffectParticles;
        set { Plugin.Config.hmdCameraMask.CutEffectParticles = value; Plugin.Config.Save(); }
    }
    [UIValue("UIValue_HMDLayerHmdOnly")]
    private bool UIValue_HMDLayerHmdOnly
    {
        get => Plugin.Config.hmdCameraMask.HmdOnly;
        set { Plugin.Config.hmdCameraMask.HmdOnly = value; Plugin.Config.Save(); }
    }
    [UIValue("UIValue_HMDLayerDesktopOnly")]
    private bool UIValue_HMDLayerDesktopOnly
    {
        get => Plugin.Config.hmdCameraMask.DesktopOnly;
        set { Plugin.Config.hmdCameraMask.DesktopOnly = value; Plugin.Config.Save(); }
    }
    [UIValue("UIValue_HMDLayerNonReflectedParticles")]
    private bool UIValue_HMDLayerNonReflectedParticles
    {
        get => Plugin.Config.hmdCameraMask.NonReflectedParticles;
        set { Plugin.Config.hmdCameraMask.NonReflectedParticles = value; Plugin.Config.Save(); }
    }
    [UIValue("UIValue_HMDLayerEnvironmentPhysics")]
    private bool UIValue_HMDLayerEnvironmentPhysics
    {
        get => Plugin.Config.hmdCameraMask.EnvironmentPhysics;
        set { Plugin.Config.hmdCameraMask.EnvironmentPhysics = value; Plugin.Config.Save(); }
    }
    [UIValue("UIValue_HMDLayerAlwaysVisible")]
    private bool UIValue_HMDLayerAlwaysVisible
    {
        get => Plugin.Config.hmdCameraMask.AlwaysVisible;
        set { Plugin.Config.hmdCameraMask.AlwaysVisible = value; Plugin.Config.Save(); }
    }
    [UIValue("UIValue_HMDLayerEvent")]
    private bool UIValue_HMDLayerEvent
    {
        get => Plugin.Config.hmdCameraMask.Event;
        set { Plugin.Config.hmdCameraMask.Event = value; Plugin.Config.Save(); }
    }
    [UIValue("UIValue_HMDLayerDesktopOnlyAndReflected")]
    private bool UIValue_HMDLayerDesktopOnlyAndReflected
    {
        get => Plugin.Config.hmdCameraMask.DesktopOnlyAndReflected;
        set { Plugin.Config.hmdCameraMask.DesktopOnlyAndReflected = value; Plugin.Config.Save(); }
    }
    [UIValue("UIValue_HMDLayerHmdOnlyAndReflected")]
    private bool UIValue_HMDLayerHmdOnlyAndReflected
    {
        get => Plugin.Config.hmdCameraMask.HmdOnlyAndReflected;
        set { Plugin.Config.hmdCameraMask.HmdOnlyAndReflected = value; Plugin.Config.Save(); }
    }
    [UIValue("UIValue_HMDLayerFixMRAlpha")]
    private bool UIValue_HMDLayerFixMRAlpha
    {
        get => Plugin.Config.hmdCameraMask.FixMRAlpha;
        set { Plugin.Config.hmdCameraMask.FixMRAlpha = value; Plugin.Config.Save(); }
    }
    [UIValue("UIValue_HMDLayerAlwaysVisibleAndReflected")]
    private bool UIValue_HMDLayerAlwaysVisibleAndReflected
    {
        get => Plugin.Config.hmdCameraMask.AlwaysVisibleAndReflected;
        set { Plugin.Config.hmdCameraMask.AlwaysVisibleAndReflected = value; Plugin.Config.Save(); }
    }
    [UIValue("UIValue_HMDLayerDontShowInExternalMRCamera")]
    private bool UIValue_HMDLayerDontShowInExternalMRCamera
    {
        get => Plugin.Config.hmdCameraMask.DontShowInExternalMRCamera;
        set { Plugin.Config.hmdCameraMask.DontShowInExternalMRCamera = value; Plugin.Config.Save(); }
    }
    [UIValue("UIValue_HMDLayerPlayersPlace")]
    private bool UIValue_HMDLayerPlayersPlace
    {
        get => Plugin.Config.hmdCameraMask.PlayersPlace;
        set { Plugin.Config.hmdCameraMask.PlayersPlace = value; Plugin.Config.Save(); }
    }
    [UIValue("UIValue_HMDLayerSkybox")]
    private bool UIValue_HMDLayerSkybox
    {
        get => Plugin.Config.hmdCameraMask.Skybox;
        set { Plugin.Config.hmdCameraMask.Skybox = value; Plugin.Config.Save(); }
    }
    [UIValue("UIValue_HMDLayerMRForegroundClipPlane")]
    private bool UIValue_HMDLayerMRForegroundClipPlane
    {
        get => Plugin.Config.hmdCameraMask.MRForegroundClipPlane;
        set { Plugin.Config.hmdCameraMask.MRForegroundClipPlane = value; Plugin.Config.Save(); }
    }
    [UIValue("UIValue_HMDLayerReserved")]
    private bool UIValue_HMDLayerReserved
    {
        get => Plugin.Config.hmdCameraMask.Reserved;
        set { Plugin.Config.hmdCameraMask.Reserved = value; Plugin.Config.Save(); }
    }

    #endregion
}
