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

public sealed partial class MapEvent :  Bright.Config.BeanBase 
{
    public MapEvent(JSONNode _json) 
    {
        { if(!_json["StartingRow"].IsNumber) { throw new SerializationException(); }  StartingRow = _json["StartingRow"]; }
        { if(!_json["EndingRow"].IsNumber) { throw new SerializationException(); }  EndingRow = _json["EndingRow"]; }
        { if(!_json["StartingCol"].IsNumber) { throw new SerializationException(); }  StartingCol = _json["StartingCol"]; }
        { if(!_json["EndingCol"].IsNumber) { throw new SerializationException(); }  EndingCol = _json["EndingCol"]; }
        { if(!_json["EventType"].IsNumber) { throw new SerializationException(); }  EventType = (EventTypeEnum)_json["EventType"].AsInt; }
        { if(!_json["Name"].IsString) { throw new SerializationException(); }  Name = _json["Name"]; }
        { if(!_json["CorrespondentId"].IsNumber) { throw new SerializationException(); }  CorrespondentId = _json["CorrespondentId"]; }
        PostInit();
    }

    public MapEvent(int StartingRow, int EndingRow, int StartingCol, int EndingCol, EventTypeEnum EventType, string Name, int CorrespondentId ) 
    {
        this.StartingRow = StartingRow;
        this.EndingRow = EndingRow;
        this.StartingCol = StartingCol;
        this.EndingCol = EndingCol;
        this.EventType = EventType;
        this.Name = Name;
        this.CorrespondentId = CorrespondentId;
        PostInit();
    }

    public static MapEvent DeserializeMapEvent(JSONNode _json)
    {
        return new MapEvent(_json);
    }

    /// <summary>
    /// 行
    /// </summary>
    public int StartingRow { get; private set; }
    /// <summary>
    /// 结束行
    /// </summary>
    public int EndingRow { get; private set; }
    /// <summary>
    /// 列
    /// </summary>
    public int StartingCol { get; private set; }
    /// <summary>
    /// 结束列
    /// </summary>
    public int EndingCol { get; private set; }
    /// <summary>
    /// 事件属性
    /// </summary>
    public EventTypeEnum EventType { get; private set; }
    /// <summary>
    /// 地图上对应名字/事件名字
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// 事件对应怪物/对话Id
    /// </summary>
    public int CorrespondentId { get; private set; }

    public const int __ID__ = 219757246;
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
        + "StartingRow:" + StartingRow + ","
        + "EndingRow:" + EndingRow + ","
        + "StartingCol:" + StartingCol + ","
        + "EndingCol:" + EndingCol + ","
        + "EventType:" + EventType + ","
        + "Name:" + Name + ","
        + "CorrespondentId:" + CorrespondentId + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}
}