using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;

namespace spMain.cs {

  class PGTypeConverter : TypeConverter {

    cs.PG_ListAttribute _attrList;

    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
      if (context == null || context.PropertyDescriptor == null) return false;// CollectionEditor || PG Root
      if (context.PropertyDescriptor is PGPropertyDescriptorArray) return false;
      Type type = context.PropertyDescriptor.PropertyType;
      if (context.PropertyDescriptor is PGPropertyDescriptor) {
        PGPropertyDescriptor pd = (PGPropertyDescriptor)context.PropertyDescriptor;
        if (pd._attrList != null) return true;
      }
      PGUtils.PropertyType pType = PGUtils.GetPropertyTypeFromType(type);
      if (pType == PGUtils.PropertyType.Collection || pType == PGUtils.PropertyType.Object)
        // Description of Object == can not edit
        return false;
      if (sourceType == typeof(string))
        return true;
      else
        return base.CanConvertFrom(context, sourceType);
    }

    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
      if (context == null) return false; // CollectionEditor
      if (context.PropertyDescriptor is PGPropertyDescriptorArray) return false;
      PGPropertyDescriptor pd = (PGPropertyDescriptor)context.PropertyDescriptor;
      if (pd._attrList != null) return true;
      if (destinationType == typeof(string)) return true;
      else
        return base.CanConvertTo(context, destinationType);
    }

    public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value) {
      if (context == null) return value;// CollectionEditor
      // List 
      if (this._attrList != null) {
        return this._attrList.GetValue(value.ToString());
      }
      // Nullable
      if (context.PropertyDescriptor.PropertyType.Name == "Nullable`1") {
        NullableConverter conv = new NullableConverter(context.PropertyDescriptor.PropertyType);
        return conv.ConvertFrom(context, culture, value);
      }
      // Enumeration
      if (value != null && context.PropertyDescriptor.PropertyType.IsEnum) {
        Type enumType = context.PropertyDescriptor.PropertyType;
        foreach (FieldInfo fi in enumType.GetFields()) {
          DescriptionAttribute dna = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
          if ((dna != null) && ((string)value == dna.Description)) return Enum.Parse(enumType, fi.Name);
        }
        return Enum.Parse(enumType, (string)value);
      }

      try {
        if (value != null && value.ToString() == "" && context.PropertyDescriptor.PropertyType == typeof(string)) return null;
        return Convert.ChangeType(value, context.PropertyDescriptor.PropertyType);
      }
      catch {
        return value; // PropertyGrid Error Message appears
      }
    }

    public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType) {
      if (value == null) return Convert.ChangeType(value, destinationType);

      // List 
      if (this._attrList != null) {
        return this._attrList.GetKeyStringFromValue(value);
      }

      Type type = value.GetType();
      PGUtils.PropertyType myType = PGUtils.GetPropertyTypeFromValue(value);
      // Nullable ??? not need
      try {
        if (destinationType == typeof(string)) {
          switch (myType) {
            case PGUtils.PropertyType.Enumeration:
              Type enumType = value.GetType();
              FieldInfo fi = enumType.GetField(Enum.GetName(enumType, value));
              DescriptionAttribute dna = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
              if (dna != null)
                return dna.Description;
              else
                return value.ToString();
            case PGUtils.PropertyType.Collection:
              if (value is ICollection) {
                ICollection x = (ICollection)value;
                switch (x.Count) {
                  case 0: return "(no items)";
                  case 1: return "(1 item)";
                  default: return "(" + x.Count + " items)";
                }
              }
              return value.ToString();
            case PGUtils.PropertyType.KeyValuePair: 
//              return value.ToString();
              return PGUtils.GetValueFromObject(value, "Key").ToString();
            case PGUtils.PropertyType.Object: //return value.ToString();
            default:
              if (value.GetType().FullName == value.ToString()) return null;// No show the type name
              if (context != null && context.PropertyDescriptor != null && context.PropertyDescriptor is PGPropertyDescriptorArray) {
                if (((PGPropertyDescriptorArray)context.PropertyDescriptor).IsShowNameOfArrayElement) {
                  return null;
                }

              }
              return value.ToString();
          }
        }
        else {
          return Convert.ChangeType(value, destinationType);
        }
      }
      catch {
        return value; // PropertyGrid Error Message appears
      }
    }

    public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes) {
/*      if (context != null && context is GridItem && ((GridItem)context).Tag is string) {// Refresh after exit from Collection Editor
        // Ёлементы массива не отрисовывуютс€ после закрыти€ CollectionEditor, из-за опаздывающей передачи нового массива к родителю. 
        // Ќо после передачи нового массива родителю делаетс€ этот вызов. ќтсюда мы вызываем Refresh, чтобы отрисовать элементы нового массива.
  //¬ышеуказанна€ проблема решаетс€, если убрать PropertyGrid.Refresh() перед закрытием CollectionEditor
        ((GridItem)context).Tag = null;// Clear trigger flag
        PropertyInfo pi= context.GetType().GetProperty("OwnerGrid", BindingFlags.Instance | BindingFlags.Public);
        if (pi != null) {
          PropertyGrid owner = (PropertyGrid)pi.GetValue(context, null);
//          owner.Refresh();
        }
      }*/
      return PGUtils.GetProperties(context, value, attributes);
    }

    public override bool GetPropertiesSupported(ITypeDescriptorContext context) {
      if (context == null) return false; // ????
      if (context.PropertyDescriptor == null) return true;// Class definition
      PGUtils.PropertyType type = PGUtils.GetPropertyTypeFromType(context.PropertyDescriptor.PropertyType);
      return type == PGUtils.PropertyType.Collection || type == PGUtils.PropertyType.Object;
    }

    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) {
      PGPropertyDescriptor pd = (PGPropertyDescriptor)context.PropertyDescriptor;
      if (pd._attrList != null) return new StandardValuesCollection(pd._attrList.GetValueList());
      if (pd.PropertyType.IsEnum) {
        return new StandardValuesCollection(Enum.GetValues(context.PropertyDescriptor.PropertyType));
      }
      return null;
    }

    public override bool GetStandardValuesSupported(ITypeDescriptorContext context) {
      if (context.PropertyDescriptor == null) return false;
      if (!(context.PropertyDescriptor is PGPropertyDescriptor)) return false;
      PGPropertyDescriptor pd = (PGPropertyDescriptor)context.PropertyDescriptor;
      this._attrList = pd._attrList;
      return pd._attrList != null;
    }

    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) {
      return true;
      //      return base.GetStandardValuesExclusive(context);
    }

  }
}
