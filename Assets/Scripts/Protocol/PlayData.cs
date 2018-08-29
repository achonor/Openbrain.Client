// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: xls2proto/play_data.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
/// <summary>Holder for reflection information generated from xls2proto/play_data.proto</summary>
public static partial class PlayDataReflection {

  #region Descriptor
  /// <summary>File descriptor for xls2proto/play_data.proto</summary>
  public static pbr::FileDescriptor Descriptor {
    get { return descriptor; }
  }
  private static pbr::FileDescriptor descriptor;

  static PlayDataReflection() {
    byte[] descriptorData = global::System.Convert.FromBase64String(
        string.Concat(
          "Chl4bHMycHJvdG8vcGxheV9kYXRhLnByb3RvIv0BCglwbGF5X2RhdGESCgoC",
          "aWQYASABKAUSDAoEbmFtZRgCIAEoCRITCgtwcmVmYWJfcGF0aBgDIAEoCRIM",
          "CgRpY29uGAQgASgJEhIKCmludHJvX2ljb24YBSABKAkSEgoKaW50cm9fdGlt",
          "ZRgGIAEoAhIMCgR0aW1lGAcgASgCEhoKEmV4cGVjdF9ncmFkZV9zY2FsZRgI",
          "IAMoBRIRCglhdHRyaWJ1dGUYCSADKAUSDgoGcGFyYW0xGAogAygFEg4KBnBh",
          "cmFtMhgLIAMoBRIOCgZwYXJhbTMYDCADKAUSDgoGcGFyYW00GA0gAygJEg4K",
          "BnBhcmFtNRgOIAMoBSIsCg9wbGF5X2RhdGFfQVJSQVkSGQoFaXRlbXMYASAD",
          "KAsyCi5wbGF5X2RhdGFiBnByb3RvMw=="));
    descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
        new pbr::FileDescriptor[] { },
        new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
          new pbr::GeneratedClrTypeInfo(typeof(global::play_data), global::play_data.Parser, new[]{ "Id", "Name", "PrefabPath", "Icon", "IntroIcon", "IntroTime", "Time", "ExpectGradeScale", "Attribute", "Param1", "Param2", "Param3", "Param4", "Param5" }, null, null, null),
          new pbr::GeneratedClrTypeInfo(typeof(global::play_data_ARRAY), global::play_data_ARRAY.Parser, new[]{ "Items" }, null, null, null)
        }));
  }
  #endregion

}
#region Messages
public sealed partial class play_data : pb::IMessage<play_data> {
  private static readonly pb::MessageParser<play_data> _parser = new pb::MessageParser<play_data>(() => new play_data());
  private pb::UnknownFieldSet _unknownFields;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public static pb::MessageParser<play_data> Parser { get { return _parser; } }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public static pbr::MessageDescriptor Descriptor {
    get { return global::PlayDataReflection.Descriptor.MessageTypes[0]; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  pbr::MessageDescriptor pb::IMessage.Descriptor {
    get { return Descriptor; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public play_data() {
    OnConstruction();
  }

  partial void OnConstruction();

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public play_data(play_data other) : this() {
    id_ = other.id_;
    name_ = other.name_;
    prefabPath_ = other.prefabPath_;
    icon_ = other.icon_;
    introIcon_ = other.introIcon_;
    introTime_ = other.introTime_;
    time_ = other.time_;
    expectGradeScale_ = other.expectGradeScale_.Clone();
    attribute_ = other.attribute_.Clone();
    param1_ = other.param1_.Clone();
    param2_ = other.param2_.Clone();
    param3_ = other.param3_.Clone();
    param4_ = other.param4_.Clone();
    param5_ = other.param5_.Clone();
    _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public play_data Clone() {
    return new play_data(this);
  }

  /// <summary>Field number for the "id" field.</summary>
  public const int IdFieldNumber = 1;
  private int id_;
  /// <summary>
  ///* ID 
  /// </summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public int Id {
    get { return id_; }
    set {
      id_ = value;
    }
  }

  /// <summary>Field number for the "name" field.</summary>
  public const int NameFieldNumber = 2;
  private string name_ = "";
  /// <summary>
  ///* 姓名 
  /// </summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public string Name {
    get { return name_; }
    set {
      name_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
    }
  }

  /// <summary>Field number for the "prefab_path" field.</summary>
  public const int PrefabPathFieldNumber = 3;
  private string prefabPath_ = "";
  /// <summary>
  ///* 玩法prefab路径 
  /// </summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public string PrefabPath {
    get { return prefabPath_; }
    set {
      prefabPath_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
    }
  }

  /// <summary>Field number for the "icon" field.</summary>
  public const int IconFieldNumber = 4;
  private string icon_ = "";
  /// <summary>
  ///* icon 
  /// </summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public string Icon {
    get { return icon_; }
    set {
      icon_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
    }
  }

  /// <summary>Field number for the "intro_icon" field.</summary>
  public const int IntroIconFieldNumber = 5;
  private string introIcon_ = "";
  /// <summary>
  ///* 介绍icon 
  /// </summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public string IntroIcon {
    get { return introIcon_; }
    set {
      introIcon_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
    }
  }

  /// <summary>Field number for the "intro_time" field.</summary>
  public const int IntroTimeFieldNumber = 6;
  private float introTime_;
  /// <summary>
  ///* 介绍时间（秒） 
  /// </summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public float IntroTime {
    get { return introTime_; }
    set {
      introTime_ = value;
    }
  }

  /// <summary>Field number for the "time" field.</summary>
  public const int TimeFieldNumber = 7;
  private float time_;
  /// <summary>
  ///* 玩法时间（秒） 
  /// </summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public float Time {
    get { return time_; }
    set {
      time_ = value;
    }
  }

  /// <summary>Field number for the "expect_grade_scale" field.</summary>
  public const int ExpectGradeScaleFieldNumber = 8;
  private static readonly pb::FieldCodec<int> _repeated_expectGradeScale_codec
      = pb::FieldCodec.ForInt32(66);
  private readonly pbc::RepeatedField<int> expectGradeScale_ = new pbc::RepeatedField<int>();
  /// <summary>
  ///* 期望分数比例 
  /// </summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public pbc::RepeatedField<int> ExpectGradeScale {
    get { return expectGradeScale_; }
  }

  /// <summary>Field number for the "attribute" field.</summary>
  public const int AttributeFieldNumber = 9;
  private static readonly pb::FieldCodec<int> _repeated_attribute_codec
      = pb::FieldCodec.ForInt32(74);
  private readonly pbc::RepeatedField<int> attribute_ = new pbc::RepeatedField<int>();
  /// <summary>
  ///* 属性转化率(万分比) 
  /// </summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public pbc::RepeatedField<int> Attribute {
    get { return attribute_; }
  }

  /// <summary>Field number for the "param1" field.</summary>
  public const int Param1FieldNumber = 10;
  private static readonly pb::FieldCodec<int> _repeated_param1_codec
      = pb::FieldCodec.ForInt32(82);
  private readonly pbc::RepeatedField<int> param1_ = new pbc::RepeatedField<int>();
  /// <summary>
  ///* 玩法参数 
  /// </summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public pbc::RepeatedField<int> Param1 {
    get { return param1_; }
  }

  /// <summary>Field number for the "param2" field.</summary>
  public const int Param2FieldNumber = 11;
  private static readonly pb::FieldCodec<int> _repeated_param2_codec
      = pb::FieldCodec.ForInt32(90);
  private readonly pbc::RepeatedField<int> param2_ = new pbc::RepeatedField<int>();
  /// <summary>
  ///* 玩法参数 
  /// </summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public pbc::RepeatedField<int> Param2 {
    get { return param2_; }
  }

  /// <summary>Field number for the "param3" field.</summary>
  public const int Param3FieldNumber = 12;
  private static readonly pb::FieldCodec<int> _repeated_param3_codec
      = pb::FieldCodec.ForInt32(98);
  private readonly pbc::RepeatedField<int> param3_ = new pbc::RepeatedField<int>();
  /// <summary>
  ///* 玩法参数 
  /// </summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public pbc::RepeatedField<int> Param3 {
    get { return param3_; }
  }

  /// <summary>Field number for the "param4" field.</summary>
  public const int Param4FieldNumber = 13;
  private static readonly pb::FieldCodec<string> _repeated_param4_codec
      = pb::FieldCodec.ForString(106);
  private readonly pbc::RepeatedField<string> param4_ = new pbc::RepeatedField<string>();
  /// <summary>
  ///* 玩法参数 
  /// </summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public pbc::RepeatedField<string> Param4 {
    get { return param4_; }
  }

  /// <summary>Field number for the "param5" field.</summary>
  public const int Param5FieldNumber = 14;
  private static readonly pb::FieldCodec<int> _repeated_param5_codec
      = pb::FieldCodec.ForInt32(114);
  private readonly pbc::RepeatedField<int> param5_ = new pbc::RepeatedField<int>();
  /// <summary>
  ///* 玩法参数 
  /// </summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public pbc::RepeatedField<int> Param5 {
    get { return param5_; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override bool Equals(object other) {
    return Equals(other as play_data);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public bool Equals(play_data other) {
    if (ReferenceEquals(other, null)) {
      return false;
    }
    if (ReferenceEquals(other, this)) {
      return true;
    }
    if (Id != other.Id) return false;
    if (Name != other.Name) return false;
    if (PrefabPath != other.PrefabPath) return false;
    if (Icon != other.Icon) return false;
    if (IntroIcon != other.IntroIcon) return false;
    if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(IntroTime, other.IntroTime)) return false;
    if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(Time, other.Time)) return false;
    if(!expectGradeScale_.Equals(other.expectGradeScale_)) return false;
    if(!attribute_.Equals(other.attribute_)) return false;
    if(!param1_.Equals(other.param1_)) return false;
    if(!param2_.Equals(other.param2_)) return false;
    if(!param3_.Equals(other.param3_)) return false;
    if(!param4_.Equals(other.param4_)) return false;
    if(!param5_.Equals(other.param5_)) return false;
    return Equals(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override int GetHashCode() {
    int hash = 1;
    if (Id != 0) hash ^= Id.GetHashCode();
    if (Name.Length != 0) hash ^= Name.GetHashCode();
    if (PrefabPath.Length != 0) hash ^= PrefabPath.GetHashCode();
    if (Icon.Length != 0) hash ^= Icon.GetHashCode();
    if (IntroIcon.Length != 0) hash ^= IntroIcon.GetHashCode();
    if (IntroTime != 0F) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(IntroTime);
    if (Time != 0F) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(Time);
    hash ^= expectGradeScale_.GetHashCode();
    hash ^= attribute_.GetHashCode();
    hash ^= param1_.GetHashCode();
    hash ^= param2_.GetHashCode();
    hash ^= param3_.GetHashCode();
    hash ^= param4_.GetHashCode();
    hash ^= param5_.GetHashCode();
    if (_unknownFields != null) {
      hash ^= _unknownFields.GetHashCode();
    }
    return hash;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override string ToString() {
    return pb::JsonFormatter.ToDiagnosticString(this);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public void WriteTo(pb::CodedOutputStream output) {
    if (Id != 0) {
      output.WriteRawTag(8);
      output.WriteInt32(Id);
    }
    if (Name.Length != 0) {
      output.WriteRawTag(18);
      output.WriteString(Name);
    }
    if (PrefabPath.Length != 0) {
      output.WriteRawTag(26);
      output.WriteString(PrefabPath);
    }
    if (Icon.Length != 0) {
      output.WriteRawTag(34);
      output.WriteString(Icon);
    }
    if (IntroIcon.Length != 0) {
      output.WriteRawTag(42);
      output.WriteString(IntroIcon);
    }
    if (IntroTime != 0F) {
      output.WriteRawTag(53);
      output.WriteFloat(IntroTime);
    }
    if (Time != 0F) {
      output.WriteRawTag(61);
      output.WriteFloat(Time);
    }
    expectGradeScale_.WriteTo(output, _repeated_expectGradeScale_codec);
    attribute_.WriteTo(output, _repeated_attribute_codec);
    param1_.WriteTo(output, _repeated_param1_codec);
    param2_.WriteTo(output, _repeated_param2_codec);
    param3_.WriteTo(output, _repeated_param3_codec);
    param4_.WriteTo(output, _repeated_param4_codec);
    param5_.WriteTo(output, _repeated_param5_codec);
    if (_unknownFields != null) {
      _unknownFields.WriteTo(output);
    }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public int CalculateSize() {
    int size = 0;
    if (Id != 0) {
      size += 1 + pb::CodedOutputStream.ComputeInt32Size(Id);
    }
    if (Name.Length != 0) {
      size += 1 + pb::CodedOutputStream.ComputeStringSize(Name);
    }
    if (PrefabPath.Length != 0) {
      size += 1 + pb::CodedOutputStream.ComputeStringSize(PrefabPath);
    }
    if (Icon.Length != 0) {
      size += 1 + pb::CodedOutputStream.ComputeStringSize(Icon);
    }
    if (IntroIcon.Length != 0) {
      size += 1 + pb::CodedOutputStream.ComputeStringSize(IntroIcon);
    }
    if (IntroTime != 0F) {
      size += 1 + 4;
    }
    if (Time != 0F) {
      size += 1 + 4;
    }
    size += expectGradeScale_.CalculateSize(_repeated_expectGradeScale_codec);
    size += attribute_.CalculateSize(_repeated_attribute_codec);
    size += param1_.CalculateSize(_repeated_param1_codec);
    size += param2_.CalculateSize(_repeated_param2_codec);
    size += param3_.CalculateSize(_repeated_param3_codec);
    size += param4_.CalculateSize(_repeated_param4_codec);
    size += param5_.CalculateSize(_repeated_param5_codec);
    if (_unknownFields != null) {
      size += _unknownFields.CalculateSize();
    }
    return size;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public void MergeFrom(play_data other) {
    if (other == null) {
      return;
    }
    if (other.Id != 0) {
      Id = other.Id;
    }
    if (other.Name.Length != 0) {
      Name = other.Name;
    }
    if (other.PrefabPath.Length != 0) {
      PrefabPath = other.PrefabPath;
    }
    if (other.Icon.Length != 0) {
      Icon = other.Icon;
    }
    if (other.IntroIcon.Length != 0) {
      IntroIcon = other.IntroIcon;
    }
    if (other.IntroTime != 0F) {
      IntroTime = other.IntroTime;
    }
    if (other.Time != 0F) {
      Time = other.Time;
    }
    expectGradeScale_.Add(other.expectGradeScale_);
    attribute_.Add(other.attribute_);
    param1_.Add(other.param1_);
    param2_.Add(other.param2_);
    param3_.Add(other.param3_);
    param4_.Add(other.param4_);
    param5_.Add(other.param5_);
    _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public void MergeFrom(pb::CodedInputStream input) {
    uint tag;
    while ((tag = input.ReadTag()) != 0) {
      switch(tag) {
        default:
          _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
          break;
        case 8: {
          Id = input.ReadInt32();
          break;
        }
        case 18: {
          Name = input.ReadString();
          break;
        }
        case 26: {
          PrefabPath = input.ReadString();
          break;
        }
        case 34: {
          Icon = input.ReadString();
          break;
        }
        case 42: {
          IntroIcon = input.ReadString();
          break;
        }
        case 53: {
          IntroTime = input.ReadFloat();
          break;
        }
        case 61: {
          Time = input.ReadFloat();
          break;
        }
        case 66:
        case 64: {
          expectGradeScale_.AddEntriesFrom(input, _repeated_expectGradeScale_codec);
          break;
        }
        case 74:
        case 72: {
          attribute_.AddEntriesFrom(input, _repeated_attribute_codec);
          break;
        }
        case 82:
        case 80: {
          param1_.AddEntriesFrom(input, _repeated_param1_codec);
          break;
        }
        case 90:
        case 88: {
          param2_.AddEntriesFrom(input, _repeated_param2_codec);
          break;
        }
        case 98:
        case 96: {
          param3_.AddEntriesFrom(input, _repeated_param3_codec);
          break;
        }
        case 106: {
          param4_.AddEntriesFrom(input, _repeated_param4_codec);
          break;
        }
        case 114:
        case 112: {
          param5_.AddEntriesFrom(input, _repeated_param5_codec);
          break;
        }
      }
    }
  }

}

public sealed partial class play_data_ARRAY : pb::IMessage<play_data_ARRAY> {
  private static readonly pb::MessageParser<play_data_ARRAY> _parser = new pb::MessageParser<play_data_ARRAY>(() => new play_data_ARRAY());
  private pb::UnknownFieldSet _unknownFields;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public static pb::MessageParser<play_data_ARRAY> Parser { get { return _parser; } }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public static pbr::MessageDescriptor Descriptor {
    get { return global::PlayDataReflection.Descriptor.MessageTypes[1]; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  pbr::MessageDescriptor pb::IMessage.Descriptor {
    get { return Descriptor; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public play_data_ARRAY() {
    OnConstruction();
  }

  partial void OnConstruction();

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public play_data_ARRAY(play_data_ARRAY other) : this() {
    items_ = other.items_.Clone();
    _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public play_data_ARRAY Clone() {
    return new play_data_ARRAY(this);
  }

  /// <summary>Field number for the "items" field.</summary>
  public const int ItemsFieldNumber = 1;
  private static readonly pb::FieldCodec<global::play_data> _repeated_items_codec
      = pb::FieldCodec.ForMessage(10, global::play_data.Parser);
  private readonly pbc::RepeatedField<global::play_data> items_ = new pbc::RepeatedField<global::play_data>();
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public pbc::RepeatedField<global::play_data> Items {
    get { return items_; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override bool Equals(object other) {
    return Equals(other as play_data_ARRAY);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public bool Equals(play_data_ARRAY other) {
    if (ReferenceEquals(other, null)) {
      return false;
    }
    if (ReferenceEquals(other, this)) {
      return true;
    }
    if(!items_.Equals(other.items_)) return false;
    return Equals(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override int GetHashCode() {
    int hash = 1;
    hash ^= items_.GetHashCode();
    if (_unknownFields != null) {
      hash ^= _unknownFields.GetHashCode();
    }
    return hash;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override string ToString() {
    return pb::JsonFormatter.ToDiagnosticString(this);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public void WriteTo(pb::CodedOutputStream output) {
    items_.WriteTo(output, _repeated_items_codec);
    if (_unknownFields != null) {
      _unknownFields.WriteTo(output);
    }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public int CalculateSize() {
    int size = 0;
    size += items_.CalculateSize(_repeated_items_codec);
    if (_unknownFields != null) {
      size += _unknownFields.CalculateSize();
    }
    return size;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public void MergeFrom(play_data_ARRAY other) {
    if (other == null) {
      return;
    }
    items_.Add(other.items_);
    _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public void MergeFrom(pb::CodedInputStream input) {
    uint tag;
    while ((tag = input.ReadTag()) != 0) {
      switch(tag) {
        default:
          _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
          break;
        case 10: {
          items_.AddEntriesFrom(input, _repeated_items_codec);
          break;
        }
      }
    }
  }

}

#endregion


#endregion Designer generated code
