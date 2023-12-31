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

public sealed partial class CardBuffInfo :  Bright.Config.BeanBase 
{
    public CardBuffInfo(JSONNode _json) 
    {
        { if(!_json["id"].IsNumber) { throw new SerializationException(); }  Id = _json["id"]; }
        { if(!_json["BuffName"].IsString) { throw new SerializationException(); }  BuffName = _json["BuffName"]; }
        { if(!_json["Description"].IsString) { throw new SerializationException(); }  Description = _json["Description"]; }
        { if(!_json["Properties"].IsNumber) { throw new SerializationException(); }  Properties = (EnumCardProperty)_json["Properties"].AsInt; }
        PostInit();
    }

    public CardBuffInfo(int id, string BuffName, string Description, EnumCardProperty Properties ) 
    {
        this.Id = id;
        this.BuffName = BuffName;
        this.Description = Description;
        this.Properties = Properties;
        PostInit();
    }

    public static CardBuffInfo DeserializeCardBuffInfo(JSONNode _json)
    {
        return new CardBuffInfo(_json);
    }

    /// <summary>
    /// ID
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// Buff名
    /// </summary>
    public string BuffName { get; private set; }
    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; private set; }
    /// <summary>
    /// 对应的buff
    /// </summary>
    public EnumCardProperty Properties { get; private set; }

    public const int __ID__ = -894768399;
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
        + "Id:" + Id + ","
        + "BuffName:" + BuffName + ","
        + "Description:" + Description + ","
        + "Properties:" + Properties + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}
}
