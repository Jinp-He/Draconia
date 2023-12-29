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

public sealed partial class Common :  Bright.Config.BeanBase 
{
    public Common(JSONNode _json) 
    {
        { if(!_json["name"].IsString) { throw new SerializationException(); }  Name = _json["name"]; }
        { if(!_json["desc"].IsString) { throw new SerializationException(); }  Desc = _json["desc"]; }
        { if(!_json["type"].IsString) { throw new SerializationException(); }  Type = _json["type"]; }
        { if(!_json["color"].IsString) { throw new SerializationException(); }  Color = _json["color"]; }
        PostInit();
    }

    public Common(string name, string desc, string type, string color ) 
    {
        this.Name = name;
        this.Desc = desc;
        this.Type = type;
        this.Color = color;
        PostInit();
    }

    public static Common DeserializeCommon(JSONNode _json)
    {
        return new Common(_json);
    }

    public string Name { get; private set; }
    public string Desc { get; private set; }
    public string Type { get; private set; }
    public string Color { get; private set; }

    public const int __ID__ = 2024019467;
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
        + "Name:" + Name + ","
        + "Desc:" + Desc + ","
        + "Type:" + Type + ","
        + "Color:" + Color + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}
}