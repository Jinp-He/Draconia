//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;
using SimpleJSON;



namespace cfg
{ 

public sealed partial class PlayerInfo :  Bright.Config.BeanBase 
{
    public PlayerInfo(JSONNode _json) 
    {
        { if(!_json["id"].IsNumber) { throw new SerializationException(); }  Id = _json["id"]; }
        { if(!_json["Name"].IsString) { throw new SerializationException(); }  Name = _json["Name"]; }
        { if(!_json["AttackPower"].IsNumber) { throw new SerializationException(); }  AttackPower = _json["AttackPower"]; }
        { if(!_json["AttackPreCD"].IsNumber) { throw new SerializationException(); }  AttackPreCD = _json["AttackPreCD"]; }
        { if(!_json["AttackPostCD"].IsNumber) { throw new SerializationException(); }  AttackPostCD = _json["AttackPostCD"]; }
        { if(!_json["AttackRange"].IsNumber) { throw new SerializationException(); }  AttackRange = _json["AttackRange"]; }
        { if(!_json["AttackRecover"].IsNumber) { throw new SerializationException(); }  AttackRecover = _json["AttackRecover"]; }
        { if(!_json["Speed"].IsNumber) { throw new SerializationException(); }  Speed = _json["Speed"]; }
        { if(!_json["InitialHP"].IsNumber) { throw new SerializationException(); }  InitialHP = _json["InitialHP"]; }
        { if(!_json["CriticalHitRate"].IsNumber) { throw new SerializationException(); }  CriticalHitRate = _json["CriticalHitRate"]; }
        { if(!_json["CriticalDamage"].IsNumber) { throw new SerializationException(); }  CriticalDamage = _json["CriticalDamage"]; }
        { if(!_json["HitRate"].IsNumber) { throw new SerializationException(); }  HitRate = _json["HitRate"]; }
        { if(!_json["DodgeRate"].IsNumber) { throw new SerializationException(); }  DodgeRate = _json["DodgeRate"]; }
        { if(!_json["Armor"].IsNumber) { throw new SerializationException(); }  Armor = _json["Armor"]; }
        { if(!_json["MagicResist"].IsNumber) { throw new SerializationException(); }  MagicResist = _json["MagicResist"]; }
        { var __json0 = _json["InitialCards"]; if(!__json0.IsArray) { throw new SerializationException(); } InitialCards = new System.Collections.Generic.List<int>(__json0.Count); foreach(JSONNode __e0 in __json0.Children) { int __v0;  { if(!__e0.IsNumber) { throw new SerializationException(); }  __v0 = __e0; }  InitialCards.Add(__v0); }   }
        { var __json0 = _json["NormalAttackCard"]; if(!__json0.IsArray) { throw new SerializationException(); } NormalAttackCard = new System.Collections.Generic.List<int>(__json0.Count); foreach(JSONNode __e0 in __json0.Children) { int __v0;  { if(!__e0.IsNumber) { throw new SerializationException(); }  __v0 = __e0; }  NormalAttackCard.Add(__v0); }   }
        { if(!_json["PassiveModifier"].IsNumber) { throw new SerializationException(); }  PassiveModifier = _json["PassiveModifier"]; }
        { if(!_json["HitDecreaseRate"].IsNumber) { throw new SerializationException(); }  HitDecreaseRate = _json["HitDecreaseRate"]; }
        { if(!_json["Alias"].IsString) { throw new SerializationException(); }  Alias = _json["Alias"]; }
        { if(!_json["DrawCardNum"].IsNumber) { throw new SerializationException(); }  DrawCardNum = _json["DrawCardNum"]; }
        { if(!_json["BackNum"].IsNumber) { throw new SerializationException(); }  BackNum = _json["BackNum"]; }
        { if(!_json["Passive"].IsString) { throw new SerializationException(); }  Passive = _json["Passive"]; }
        { if(!_json["PassiveDesc"].IsString) { throw new SerializationException(); }  PassiveDesc = _json["PassiveDesc"]; }
        PostInit();
    }

    public PlayerInfo(int id, string Name, int AttackPower, int AttackPreCD, int AttackPostCD, int AttackRange, int AttackRecover, int Speed, int InitialHP, float CriticalHitRate, float CriticalDamage, float HitRate, float DodgeRate, int Armor, int MagicResist, System.Collections.Generic.List<int> InitialCards, System.Collections.Generic.List<int> NormalAttackCard, int PassiveModifier, float HitDecreaseRate, string Alias, int DrawCardNum, int BackNum, string Passive, string PassiveDesc ) 
    {
        this.Id = id;
        this.Name = Name;
        this.AttackPower = AttackPower;
        this.AttackPreCD = AttackPreCD;
        this.AttackPostCD = AttackPostCD;
        this.AttackRange = AttackRange;
        this.AttackRecover = AttackRecover;
        this.Speed = Speed;
        this.InitialHP = InitialHP;
        this.CriticalHitRate = CriticalHitRate;
        this.CriticalDamage = CriticalDamage;
        this.HitRate = HitRate;
        this.DodgeRate = DodgeRate;
        this.Armor = Armor;
        this.MagicResist = MagicResist;
        this.InitialCards = InitialCards;
        this.NormalAttackCard = NormalAttackCard;
        this.PassiveModifier = PassiveModifier;
        this.HitDecreaseRate = HitDecreaseRate;
        this.Alias = Alias;
        this.DrawCardNum = DrawCardNum;
        this.BackNum = BackNum;
        this.Passive = Passive;
        this.PassiveDesc = PassiveDesc;
        PostInit();
    }

    public static PlayerInfo DeserializePlayerInfo(JSONNode _json)
    {
        return new PlayerInfo(_json);
    }

    /// <summary>
    /// ID
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// 角色名称
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// 攻击力
    /// </summary>
    public int AttackPower { get; private set; }
    /// <summary>
    /// 攻击前摇
    /// </summary>
    public int AttackPreCD { get; private set; }
    /// <summary>
    /// 攻击后摇
    /// </summary>
    public int AttackPostCD { get; private set; }
    /// <summary>
    /// 攻击范围
    /// </summary>
    public int AttackRange { get; private set; }
    /// <summary>
    /// 攻击硬直
    /// </summary>
    public int AttackRecover { get; private set; }
    /// <summary>
    /// 速度 
    /// </summary>
    public int Speed { get; private set; }
    /// <summary>
    /// 生命
    /// </summary>
    public int InitialHP { get; private set; }
    /// <summary>
    /// 暴击率
    /// </summary>
    public float CriticalHitRate { get; private set; }
    /// <summary>
    /// 暴击伤害
    /// </summary>
    public float CriticalDamage { get; private set; }
    /// <summary>
    /// 击中率
    /// </summary>
    public float HitRate { get; private set; }
    /// <summary>
    /// 闪避率
    /// </summary>
    public float DodgeRate { get; private set; }
    /// <summary>
    /// 护甲
    /// </summary>
    public int Armor { get; private set; }
    /// <summary>
    /// 魔抗
    /// </summary>
    public int MagicResist { get; private set; }
    /// <summary>
    /// 初始卡组
    /// </summary>
    public System.Collections.Generic.List<int> InitialCards { get; private set; }
    public System.Collections.Generic.List<CardInfo> InitialCards_Ref { get; private set; }
    /// <summary>
    /// 普通卡
    /// </summary>
    public System.Collections.Generic.List<int> NormalAttackCard { get; private set; }
    public System.Collections.Generic.List<CardInfo> NormalAttackCard_Ref { get; private set; }
    /// <summary>
    /// 被动数值
    /// </summary>
    public int PassiveModifier { get; private set; }
    /// <summary>
    /// 硬直系数
    /// </summary>
    public float HitDecreaseRate { get; private set; }
    /// <summary>
    /// 角色代号
    /// </summary>
    public string Alias { get; private set; }
    /// <summary>
    /// 每回合抽卡数
    /// </summary>
    public int DrawCardNum { get; private set; }
    /// <summary>
    /// 每回合退回数
    /// </summary>
    public int BackNum { get; private set; }
    /// <summary>
    /// 被动
    /// </summary>
    public string Passive { get; private set; }
    /// <summary>
    /// 被动描述
    /// </summary>
    public string PassiveDesc { get; private set; }

    public const int __ID__ = -205981873;
    public override int GetTypeId() => __ID__;

    public  void Resolve(Dictionary<string, object> _tables)
    {
        { TbCardInfo __table = (TbCardInfo)_tables["TbCardInfo"]; this.InitialCards_Ref = new System.Collections.Generic.List<CardInfo>(); foreach(var __e in InitialCards) { this.InitialCards_Ref.Add(__table.GetOrDefault(__e)); } }
        { TbCardInfo __table = (TbCardInfo)_tables["TbCardInfo"]; this.NormalAttackCard_Ref = new System.Collections.Generic.List<CardInfo>(); foreach(var __e in NormalAttackCard) { this.NormalAttackCard_Ref.Add(__table.GetOrDefault(__e)); } }
        PostResolve();
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "Id:" + Id + ","
        + "Name:" + Name + ","
        + "AttackPower:" + AttackPower + ","
        + "AttackPreCD:" + AttackPreCD + ","
        + "AttackPostCD:" + AttackPostCD + ","
        + "AttackRange:" + AttackRange + ","
        + "AttackRecover:" + AttackRecover + ","
        + "Speed:" + Speed + ","
        + "InitialHP:" + InitialHP + ","
        + "CriticalHitRate:" + CriticalHitRate + ","
        + "CriticalDamage:" + CriticalDamage + ","
        + "HitRate:" + HitRate + ","
        + "DodgeRate:" + DodgeRate + ","
        + "Armor:" + Armor + ","
        + "MagicResist:" + MagicResist + ","
        + "InitialCards:" + Bright.Common.StringUtil.CollectionToString(InitialCards) + ","
        + "NormalAttackCard:" + Bright.Common.StringUtil.CollectionToString(NormalAttackCard) + ","
        + "PassiveModifier:" + PassiveModifier + ","
        + "HitDecreaseRate:" + HitDecreaseRate + ","
        + "Alias:" + Alias + ","
        + "DrawCardNum:" + DrawCardNum + ","
        + "BackNum:" + BackNum + ","
        + "Passive:" + Passive + ","
        + "PassiveDesc:" + PassiveDesc + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}
}