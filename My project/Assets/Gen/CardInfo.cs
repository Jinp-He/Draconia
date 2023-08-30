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

public sealed partial class CardInfo :  Bright.Config.BeanBase 
{
    public CardInfo(JSONNode _json) 
    {
        { if(!_json["id"].IsNumber) { throw new SerializationException(); }  Id = _json["id"]; }
        { if(!_json["Name"].IsString) { throw new SerializationException(); }  Name = _json["Name"]; }
        { if(!_json["Desc"].IsString) { throw new SerializationException(); }  Desc = _json["Desc"]; }
        { if(!_json["Cost"].IsNumber) { throw new SerializationException(); }  Cost = _json["Cost"]; }
        { if(!_json["AttackPreCD"].IsNumber) { throw new SerializationException(); }  AttackPreCD = _json["AttackPreCD"]; }
        { if(!_json["AttackPostCD"].IsNumber) { throw new SerializationException(); }  AttackPostCD = _json["AttackPostCD"]; }
        { if(!_json["AttackRange"].IsNumber) { throw new SerializationException(); }  AttackRange = _json["AttackRange"]; }
        { if(!_json["AttackRecover"].IsNumber) { throw new SerializationException(); }  AttackRecover = _json["AttackRecover"]; }
        { var __json0 = _json["Effect"]; if(!__json0.IsArray) { throw new SerializationException(); } Effect = new System.Collections.Generic.List<BaseEffect>(__json0.Count); foreach(JSONNode __e0 in __json0.Children) { BaseEffect __v0;  { if(!__e0.IsObject) { throw new SerializationException(); }  __v0 = BaseEffect.DeserializeBaseEffect(__e0);  }  Effect.Add(__v0); }   }
        { if(!_json["SkillTargetType"].IsNumber) { throw new SerializationException(); }  SkillTargetType = (SkillTarget)_json["SkillTargetType"].AsInt; }
        { if(!_json["BelongedCharacter"].IsNumber) { throw new SerializationException(); }  BelongedCharacter = _json["BelongedCharacter"]; }
        { var __json0 = _json["Properties"]; if(!__json0.IsArray) { throw new SerializationException(); } Properties = new System.Collections.Generic.List<EnumCardProperty>(__json0.Count); foreach(JSONNode __e0 in __json0.Children) { EnumCardProperty __v0;  { if(!__e0.IsNumber) { throw new SerializationException(); }  __v0 = (EnumCardProperty)__e0.AsInt; }  Properties.Add(__v0); }   }
        PostInit();
    }

    public CardInfo(int id, string Name, string Desc, int Cost, int AttackPreCD, int AttackPostCD, int AttackRange, int AttackRecover, System.Collections.Generic.List<BaseEffect> Effect, SkillTarget SkillTargetType, int BelongedCharacter, System.Collections.Generic.List<EnumCardProperty> Properties ) 
    {
        this.Id = id;
        this.Name = Name;
        this.Desc = Desc;
        this.Cost = Cost;
        this.AttackPreCD = AttackPreCD;
        this.AttackPostCD = AttackPostCD;
        this.AttackRange = AttackRange;
        this.AttackRecover = AttackRecover;
        this.Effect = Effect;
        this.SkillTargetType = SkillTargetType;
        this.BelongedCharacter = BelongedCharacter;
        this.Properties = Properties;
        PostInit();
    }

    public static CardInfo DeserializeCardInfo(JSONNode _json)
    {
        return new CardInfo(_json);
    }

    /// <summary>
    /// ID
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// 卡牌名
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// 描述
    /// </summary>
    public string Desc { get; private set; }
    /// <summary>
    /// 费用
    /// </summary>
    public int Cost { get; private set; }
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
    /// 技能效果
    /// </summary>
    public System.Collections.Generic.List<BaseEffect> Effect { get; private set; }
    /// <summary>
    /// 技能对象
    /// </summary>
    public SkillTarget SkillTargetType { get; private set; }
    /// <summary>
    /// 从属英雄
    /// </summary>
    public int BelongedCharacter { get; private set; }
    public PlayerInfo BelongedCharacter_Ref { get; private set; }
    /// <summary>
    /// 特性
    /// </summary>
    public System.Collections.Generic.List<EnumCardProperty> Properties { get; private set; }

    public const int __ID__ = 56078334;
    public override int GetTypeId() => __ID__;

    public  void Resolve(Dictionary<string, object> _tables)
    {
        foreach(var _e in Effect) { _e?.Resolve(_tables); }
        this.BelongedCharacter_Ref = (_tables["TbPlayerInfo"] as TbPlayerInfo).GetOrDefault(BelongedCharacter);
        PostResolve();
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
        foreach(var _e in Effect) { _e?.TranslateText(translator); }
    }

    public override string ToString()
    {
        return "{ "
        + "Id:" + Id + ","
        + "Name:" + Name + ","
        + "Desc:" + Desc + ","
        + "Cost:" + Cost + ","
        + "AttackPreCD:" + AttackPreCD + ","
        + "AttackPostCD:" + AttackPostCD + ","
        + "AttackRange:" + AttackRange + ","
        + "AttackRecover:" + AttackRecover + ","
        + "Effect:" + Bright.Common.StringUtil.CollectionToString(Effect) + ","
        + "SkillTargetType:" + SkillTargetType + ","
        + "BelongedCharacter:" + BelongedCharacter + ","
        + "Properties:" + Bright.Common.StringUtil.CollectionToString(Properties) + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}
}
