﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Idevgame.GameState.DialogState;

public class LoadingDialogState:CommonDialogState<LoadingUiController>
{
    public override string DialogName { get { return "LoadingWnd"; } }
    public LoadingDialogState(MainDialogStateManager dialog):base(dialog)
    {

    }
}

public class LoadingUiController : Dialog
{
    [SerializeField]
    Image mProgressBar;
    [SerializeField]
    Image ShowLoading;
    [SerializeField]
    Text mProgessLab;
    [SerializeField]
    Text LoadingNoticeLabel;
    public override void OnDialogStateEnter(BaseDialogState ownerState, BaseDialogState previousDialog, object data)
    {
        base.OnDialogStateEnter(ownerState, previousDialog, data);
        Texture2D loadingTexture = null;
        if (Main.Ins.CombatData.Chapter == null)
        {
            if (Main.Ins.CombatData.GLevelItem != null && !string.IsNullOrEmpty(Main.Ins.CombatData.GLevelItem.BgTexture))
                loadingTexture = GameObject.Instantiate(Resources.Load<Texture2D>(Main.Ins.CombatData.GLevelItem.BgTexture));
        }
        else
        {
            for (int i = 0; i < Main.Ins.CombatData.Chapter.resPath.Count; i++)
            {
                if (Main.Ins.CombatData.Chapter.resPath[i].EndsWith(Main.Ins.CombatData.GLevelItem.BgTexture + ".jpg"))
                {
                    byte[] array = System.IO.File.ReadAllBytes(Main.Ins.CombatData.Chapter.resPath[i]);
                    Texture2D tex = new Texture2D(0, 0);
                    tex.LoadImage(array);
                    loadingTexture = tex;
                    break;
                }
            }
        }
        if (loadingTexture != null) {
            ShowLoading.sprite = Sprite.Create(loadingTexture, new Rect(0, 0, loadingTexture.width, loadingTexture.height), Vector2.zero);
            //把这个图全屏拉大到画布大小
            Utility.Expand(ShowLoading, loadingTexture.width, loadingTexture.height);
            ShowLoading.color = Color.white;
        }
        SetLoadingNoticeLabel();
        mProgressBar.fillAmount = 0f;
    }

    public void UpdateProgress(float progress)
    {
        mProgressBar.fillAmount = progress;
        mProgessLab.text = string.Format("{0:P0}", progress);
    }


    public void SetLoadingNoticeLabel()
    {
        LoadingNoticeLabel.text = "";
    }
}