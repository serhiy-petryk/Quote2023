using System;
using System.Reflection;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace spMain.QData.Data {
  class DataInputPropertyDescriptor : PropertyDescriptor {

    object _parent;
    DataInput _datainput;

    public DataInputPropertyDescriptor(object parent, DataInput datainput, Attribute[] attrs)
      : base(datainput._id, attrs) {
      this._parent = parent; this._datainput = datainput;
    }

    public override string Name {
      get { return _datainput._prompt; }
    }

    public override object GetValue(object component) {
      return _datainput._value;
    }

    public override Type PropertyType {
      get { return this._datainput._dataType; }
    }

    public override bool IsReadOnly {
      get { return false; }
    }

    public override bool CanResetValue(object component) {
      return false;
    }

    public override Type ComponentType {
      get { return this._parent.GetType(); }
    }

    public override void ResetValue(object component) {
      throw new Exception("The method or operation is not implemented.");
    }

    public override void SetValue(object component, object value) {
      if (value is ICloneable) {
        this._datainput._value = ((ICloneable)value).Clone();
      }
      else if (value is ISerializable) {
        this._datainput._value = csFastSerializer.Utils.GetClone(value);
      }
      else this._datainput._value = value;
    }

    public override bool ShouldSerializeValue(object component) {//???
      return true;
    }

    public override string Description {
      get {
        return this._datainput._description;
      }
    }

    public override string DisplayName {
      get { return this._datainput._prompt; }
    }
    public override string Category {
      get { return "Parameters"; }
    }

    public override TypeConverter Converter {
      get {
        return TypeDescriptor.GetConverter(this._datainput._dataType);
        //        return base.Converter;
      }
    }

    public override object GetEditor(Type editorBaseType) {
      return TypeDescriptor.GetEditor(this._datainput._dataType, editorBaseType);
      //      return base.GetEditor(editorBaseType);
    }
  }
}
