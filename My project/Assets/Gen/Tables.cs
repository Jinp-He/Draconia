//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using SimpleJSON;


namespace cfg
{ 
   
public sealed partial class Tables
{
    public TbPlayerInfo TbPlayerInfo {get; }
    public TbCardInfo TbCardInfo {get; }
    public TbStageInfo TbStageInfo {get; }
    public TbEnemyInfo TbEnemyInfo {get; }
    public TbCommon TbCommon {get; }
    public TbBuffInfo TbBuffInfo {get; }

    public Tables(System.Func<string, JSONNode> loader)
    {
        var tables = new System.Collections.Generic.Dictionary<string, object>();
        TbPlayerInfo = new TbPlayerInfo(loader("tbplayerinfo")); 
        tables.Add("TbPlayerInfo", TbPlayerInfo);
        TbCardInfo = new TbCardInfo(loader("tbcardinfo")); 
        tables.Add("TbCardInfo", TbCardInfo);
        TbStageInfo = new TbStageInfo(loader("tbstageinfo")); 
        tables.Add("TbStageInfo", TbStageInfo);
        TbEnemyInfo = new TbEnemyInfo(loader("tbenemyinfo")); 
        tables.Add("TbEnemyInfo", TbEnemyInfo);
        TbCommon = new TbCommon(loader("tbcommon")); 
        tables.Add("TbCommon", TbCommon);
        TbBuffInfo = new TbBuffInfo(loader("tbbuffinfo")); 
        tables.Add("TbBuffInfo", TbBuffInfo);
        PostInit();

        TbPlayerInfo.Resolve(tables); 
        TbCardInfo.Resolve(tables); 
        TbStageInfo.Resolve(tables); 
        TbEnemyInfo.Resolve(tables); 
        TbCommon.Resolve(tables); 
        TbBuffInfo.Resolve(tables); 
        PostResolve();
    }

    public void TranslateText(System.Func<string, string, string> translator)
    {
        TbPlayerInfo.TranslateText(translator); 
        TbCardInfo.TranslateText(translator); 
        TbStageInfo.TranslateText(translator); 
        TbEnemyInfo.TranslateText(translator); 
        TbCommon.TranslateText(translator); 
        TbBuffInfo.TranslateText(translator); 
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}