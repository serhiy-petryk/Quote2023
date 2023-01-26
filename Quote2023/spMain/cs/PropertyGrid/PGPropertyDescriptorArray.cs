using System;
using System.Reflection;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace spMain.cs {

  public class PGPropertyDescriptorArray : PropertyDescriptor {

    object list;
//    Type componentType;
    object key;
    object value;
    public readonly int itemNo = -1;

    public PGPropertyDescriptorArray(object component, object pItem, string name, Attribute[] attributes)
      : base(name, attributes) {
//      this.componentType = component.GetType();
      this.list = component;
      if (component is IDictionary) {
        this.key = PGUtils.GetValueFromObject(pItem, "Key");
        this.value = PGUtils.GetValueFromObject(pItem, "Value");
      }
      else {
        this.value = pItem;
        itemNo = int.Parse(name.Substring(1));
      }
      if (pItem is QData.UI.UIPane) {
      }
    }

    public bool IsShowNameOfArrayElement {
      get { return this.Attributes.Contains(new cs.PG_IsShowNameOfArrayElement(true)); }
    }

    protected override Attribute[] AttributeArray {
      get { return base.AttributeArray; }
      set { base.AttributeArray = value; }
    }

    public override AttributeCollection Attributes {
      get { return base.Attributes; }
    }

    public override bool CanResetValue(object component) { return true; }

    public override Type ComponentType {
      get { return this.list.GetType(); }
    }

    public override string DisplayName {
      get {
//        return (key == null ? "[" + itemNo.ToString() + "]" : key.ToString()); 
        if (key == null) {
          return "[" + itemNo.ToString() + "]" + (this.IsShowNameOfArrayElement? " " + this.value.ToString(): "");
        }
        else return key.ToString();
      }
    }

    public override object GetEditor(Type editorBaseType) {
      if (this.value == null) return base.GetEditor(editorBaseType);
      return PGUtils.PD_GetEditor(this.value.GetType(), base.GetEditor(editorBaseType));
    }

    public override TypeConverter Converter {
      get {
        Type type = typeof(cs.PGTypeConverter);
        if (base.Converter.GetType() == typeof(System.ComponentModel.TypeConverter)) {// No convertor
          if ((type.IsClass && type != typeof(string)) && type != null && typeof(TypeConverter).IsAssignableFrom(type)) {
            return (TypeConverter)this.CreateInstance(type); // Create instance of converter type
          }
        }
        return base.Converter;
      }
    }

    public override string Description {
      get { return null; }
    }

    public override object GetValue(object component) {
      return this.value;
    }

    public override bool IsReadOnly {
      get {
        ReadOnlyAttribute x = (ReadOnlyAttribute)this.Attributes[typeof(ReadOnlyAttribute)];
        if (x != null) return x.IsReadOnly;
        else return false;
      }
    }

    public override string Name {
      get {
        return base.Name; 
      }
    }

    public override Type PropertyType {
      get { return (this.value==null? null :this.value.GetType()); }
    }

    public override void ResetValue(object component) {
    }

    public override bool ShouldSerializeValue(object component) {
      return true;
    }

    public override void SetValue(object component, object value) {// вызов, когда измен€ютс€ данные
      if (list is IDictionary) {
        IDictionary dict = (IDictionary)list;
        dict[key] = value;
        this.value = value;
        return;
      }
      if (list is IList && this.itemNo >= 0) {
        IList col = (IList)list;
        col[itemNo] = value;
        this.value = value;
        return;
      }
    }

		public override void AddValueChanged(object component, EventHandler handler) {
			base.AddValueChanged(component, handler);
		}

		public override bool SupportsChangeEvents {
			get {				return base.SupportsChangeEvents;			}
		}

    public override void RemoveValueChanged(object component, EventHandler handler) {
      base.RemoveValueChanged(component, handler);
    }

    public void RemoveItem() {
      if (this.list is IDictionary) {
        ((IDictionary)list).Remove(this.key);
      }
      if (list is IList) {
        IList x = (IList)list;
        if (!(x.IsFixedSize || x.IsReadOnly)) x.RemoveAt(this.itemNo);
      }
    }

    public override string Category {
      get { return "Items"; }
    }

  }
}