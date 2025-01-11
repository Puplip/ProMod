using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using UnityEngine;
using TMPro;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.FloatingScreen;
using BeatSaberMarkupLanguage.ViewControllers;
using BeatSaberMarkupLanguage.Attributes;
using HMUI;
using UnityEngine.PlayerLoop;
using System.Collections;
using ProMod.Stats;

namespace ProMod.HUD;


public class ProHUDController : MonoBehaviour
{

    [Inject]
    private IJumpOffsetYProvider _jumpOffsetYProvider;

    [Inject]
    private ProStats _proStats;

    private ProHUDElementController topElementController;
    private ProHUDElementController bottomElementControlller;
    private ProHUDElementController leftElementControlller;
    private ProHUDElementController rightElementControlller;

    private ProHUDElementController healthElementController;

    private Transform playerHeightTransform;

    private void Awake()
    {

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        playerHeightTransform = new GameObject("ProStatPlayerHeight").transform;
        playerHeightTransform.SetParent(transform, false);
        playerHeightTransform.localPosition = Vector3.up * (_jumpOffsetYProvider.jumpOffsetY + 1.4f);
        playerHeightTransform.localRotation = Quaternion.identity;

        healthElementController = new GameObject("ProHUDHealth").AddComponent<ProHUDElementController>();
        healthElementController.transform.SetParent(transform, false);
        healthElementController.InitElement("HealthBar");

        rightElementControlller = new GameObject("ProHUDRightMain").AddComponent<ProHUDElementController>();
        rightElementControlller.transform.SetParent(playerHeightTransform, false);
        rightElementControlller.transform.localEulerAngles = new Vector3(0f, 15f, 0f);

        leftElementControlller = new GameObject("ProHUDLeftMain").AddComponent<ProHUDElementController>();
        leftElementControlller.transform.SetParent(playerHeightTransform, false);
        leftElementControlller.transform.localEulerAngles = new Vector3(0f, -15f, 0f);

        topElementController = new GameObject("ProHUDTopMain").AddComponent<ProHUDElementController>();
        topElementController.transform.SetParent(playerHeightTransform, false);
        topElementController.transform.localEulerAngles = new Vector3(-25f, 0f, 0f);

        bottomElementControlller = new GameObject("ProHUDBottomMain").AddComponent<ProHUDElementController>();
        bottomElementControlller.transform.SetParent(transform, false);
        bottomElementControlller.transform.localEulerAngles = new Vector3(25f, 0f, 0f);

        switch (Plugin.Config.proHUDConfig.leftRightElementPositions)
        {
            case ProHUDConfig.LeftRightElementPositions.AccuracyLeftComboRight:
                leftElementControlller.InitElement("Accuracy");
                rightElementControlller.InitElement("ComboMain");
                break;
            case ProHUDConfig.LeftRightElementPositions.ComboLeftAccuracyRight:
                leftElementControlller.InitElement("ComboMain");
                rightElementControlller.InitElement("Accuracy");
                break;
        }

        switch (Plugin.Config.proHUDConfig.topBottomElementPositions)
        {
            case ProHUDConfig.TopBottomElementPositions.CutStatsTop:
                topElementController.InitElement(Plugin.Config.proHUDConfig.showCutStats ? "CutStats" : "Empty");
                bottomElementControlller.InitElement(Plugin.Config.proHUDConfig.showScoreDensity ? "ScoreDensity" : "Empty");
                break;
            case ProHUDConfig.TopBottomElementPositions.ScoreDensityTop:
                topElementController.InitElement(Plugin.Config.proHUDConfig.showScoreDensity ? "ScoreDensity" : "Empty");
                bottomElementControlller.InitElement(Plugin.Config.proHUDConfig.showCutStats ? "CutStats" : "Empty");
                break;
            case ProHUDConfig.TopBottomElementPositions.BothTopCutStatsLeft:
            case ProHUDConfig.TopBottomElementPositions.BothTopScoreDensityLeft:
                topElementController.InitElement("BothTopBottomElements");
                bottomElementControlller.InitElement("Empty");
                break;
            case ProHUDConfig.TopBottomElementPositions.BothBottomCutStatsLeft:
            case ProHUDConfig.TopBottomElementPositions.BothBottomScoreDensityLeft:
                topElementController.InitElement("Empty");
                bottomElementControlller.InitElement("BothTopBottomElements");
                break;
        }

        ProHUDPatch_HUDPositionReady();

        ProHUDPatch.HUDPositionReady += ProHUDPatch_HUDPositionReady;

        _proStats.onUpdate += ProStats_onUpdate;
        if (!_proStats.ready)
        {
            _proStats.onStatsReady += ProStats_onStatsReady;
        } else
        {
            ProStats_onStatsReady();
        }
    }

    private void ProStats_onStatsReady()
    {
        healthElementController.OnStatReady(_proStats);
        rightElementControlller.OnStatReady(_proStats);
        leftElementControlller.OnStatReady(_proStats);
        topElementController.OnStatReady(_proStats);
        bottomElementControlller.OnStatReady(_proStats);
    }

    bool needsUpdate = false;
    private void ProStats_onUpdate()
    {
        needsUpdate = true;
    }

    private void OnDestroy()
    {
        ProHUDPatch.HUDPositionReady -= ProHUDPatch_HUDPositionReady;
        _proStats.onUpdate -= ProStats_onUpdate;
    }

    private bool isInitialized = false;
    private void ProHUDPatch_HUDPositionReady()
    {

        Plugin.Log.Info("ProHUDPatch_HUDPositionReady");
        
        healthElementController.transform.localPosition = ProHUDPatch.energyPanelPos + new Vector3(0f, 0f, -0.1f);

        rightElementControlller.transform.localPosition = new Vector3(ProHUDPatch.hudWidth, 0, ProHUDPatch.hudDistance);

        leftElementControlller.transform.localPosition = new Vector3(-ProHUDPatch.hudWidth, 0, ProHUDPatch.hudDistance);

        topElementController.transform.localPosition = new Vector3(0, 2.5f, ProHUDPatch.hudDistance - 1f);
        bottomElementControlller.transform.localPosition = ProHUDPatch.energyPanelPos + new Vector3(0f, -0.75f, -0.75f);

        isInitialized = true;
    }

    private void LateUpdate()
    {
        if (!(isInitialized && needsUpdate)) { return; }

        healthElementController.OnStatUpdate(_proStats);

        rightElementControlller.OnStatUpdate(_proStats);
        leftElementControlller.OnStatUpdate(_proStats);
        topElementController.OnStatUpdate(_proStats);
        bottomElementControlller.OnStatUpdate(_proStats);

        needsUpdate = false;
    }




}