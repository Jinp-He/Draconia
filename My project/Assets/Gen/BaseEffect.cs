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

public sealed partial class BaseEffect :  Bright.Config.BeanBase 
{
    public BaseEffect(JSONNode _json) 
    {
        { if(!_json["BaseEffectType"].IsNumber) { throw new SerializationException(); }  BaseEffectType = (BaseEffectType)_json["BaseEffectType"].AsInt; }
        { if(!_json["Param1"].IsNumber) { throw new SerializationException(); }  Param1 = _json["Param1"]; }
        { if(!_json["Param2"].IsNumber) { throw new SerializationException(); }  Param2 = _json["Param2"]; }
        { if(!_json["Param3"].IsNumber) { throw new SerializationException(); }  Param3 = _json["Param3"]; }
        PostInit();
    }

    public BaseEffect(BaseEffectType BaseEffectType, float Param1, int Param2, int Param3 ) 
    {
        this.BaseEffectType = BaseEffectType;
        this.Param1 = Param1;
        this.Param2 = Param2;
        this.Param3 = Param3;
        PostInit();
    }

    public static BaseEffect DeserializeBaseEffect(JSONNode _json)
    {
        return new BaseEffect(_json);
    }

    public BaseEffectType BaseEffectType { get; private set; }
    /// <summary>
    /// 攻击力相关系数
    /// </summary>
    public float Param1 { get; private set; }
    /// <summary>
    /// id相关系数
    /// </summary>
    public int Param2 { get; private set; }
    /// <summary>
    /// 数量相关系数
    /// </summary>
    public int Param3 { get; private set; }

    public const int __ID__ = 761620450;
    public override int GetTypeId() => __ID__;

    public  void Resolve(Dictionary<string, object> _tables)
    {
        PostResolve();
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "BaseEffectType:" + BaseEffectType + ","
        + "Param1:" + Param1 + ","
        + "Param2:" + Param2 + ","
        + "Param3:" + Param3 + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}
}
