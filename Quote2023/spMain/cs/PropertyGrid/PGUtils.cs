using System;
using System.Reflection;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;

namespace spMain.cs {

  interface IPG_AdjustProperties {
    void AdjustProperties(Dictionary<string, PropertyDescriptor> propertyList, ITypeDescriptorContext context);
  }
  interface IPG_ValidateSupport {
    string GetErrorDescription();// return value: error description
  }

  class PGUtils {

    public enum PropertyType {
      [Description("Primitive object")]
      Primitive,
      [Description("Structure object")]
      Structure,
      [Description("KeyValuePair object")]
      KeyValuePair,
      [Description("Enumaration object")]
      Enumeration,
      [Description("Collection object")]
      Collection,
      [Description("General object")]
      Object,
      [Description("Type object")]
      Type
    };

    static Dictionary<string, Dictionary<string, object>> _cacheEnumList = new Dictionary<string, Dictionary<string, object>>();
//    static ArrayList _appliedConverters = new ArrayList();
    static Dictionary<Type, bool> _appliedConverters = new Dictionary<Type,bool>();

    public static Dictionary<string, object> GetEnumList(Type enumType) {
      if (!_cacheEnumList.ContainsKey(enumType.FullName)) {// No in cache
        Dictionary<string, object> list = new Dictionary<string, object>();
        string[] names = Enum.GetNames(enumType);
        Array values = Enum.GetValues(enumType);
        bool flag = false;
        int cnt = 0;
        foreach (object x in values) {
          FieldInfo fi = enumType.GetField(names[cnt]);
          DescriptionAttribute da = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
          if (da != null) {
            flag = true;
            list.Add(da.Description, x);
          }
          else {
            list.Add(names[cnt], x);
          }
          cnt++;
        }
        if (!flag) list = null;// Enum does not have description attribute
        _cacheEnumList.Add(enumType.FullName, list);
        return list;
      }
      return _cacheEnumList[enumType.FullName];
    }

    public static object GetValueFromObject(object Object, string propertyName) {
      PropertyInfo pi = Object.GetType().GetProperty(propertyName);
      return pi.GetValue(Object, null);
    }


    public static void ApplyTypeConverter(object objectOrType) {//
      if (objectOrType != null) {
        Type type = (objectOrType is Type ? (Type)objectOrType : objectOrType.GetType());
        if (type == typeof(object)) return; // Invalid for nullable bool (?bool), ..
        ApplyTypeConverterSingle(type);
        if (type.GetInterface("IEnumerable") != null) {// Apply converter for Collection item type
          Type itemType = PGUtils.GetCollectionItemType(type);
          PGUtils.ApplyTypeConverter(itemType);
        }

/*        if (!(objectOrType is Type)) {// Recursive apply converter ? Not need
          if (objectOrType is IEnumerable) {
            IEnumerator ienum = ((IEnumerable)objectOrType).GetEnumerator();
            ienum.Reset();
            while (ienum.MoveNext()) {
              ApplyTypeConverter(ienum.Current);
            }
          }
          if (GetPropertyTypeFromValue(objectOrType) == PropertyType.KeyValuePair) {
            ApplyTypeConverter(GetValueFromObject(objectOrType, "Value"));
          }
        }*/
      }
    }

    static void ApplyTypeConverterSingle(Type type) {
      if (! _appliedConverters.ContainsKey(type)) {
        Type converterType = TypeDescriptor.GetConverter(type).GetType();
        bool applyFlag = converterType == typeof(System.ComponentModel.TypeConverter) ||
          converterType == typeof(System.ComponentModel.CollectionConverter) ||
          converterType == typeof(System.ComponentModel.ArrayConverter);
        _appliedConverters.Add(type, applyFlag);
        if (applyFlag) {// No convertor
          TypeDescriptor.AddAttributes(type, new Attribute[] { new TypeConverterAttribute(typeof(cs.PGTypeConverter)) });
        }
      }
    }

    // ================================  Attributes ==============================
    static public AttributeCollection MergeAttributes(ICollection mainAttrCollection, ICollection secondAttrCollection) {
      List<Attribute> attrs = new List<Attribute>();

      IEnumerator ienum = mainAttrCollection.GetEnumerator();
      ienum.Reset();
      while (ienum.MoveNext()) {
        attrs.Add((Attribute)ienum.Current);
      }

      ienum = secondAttrCollection.GetEnumerator();
      ienum.Reset();
      while (ienum.MoveNext()) {
        Attribute a = (Attribute)ienum.Current;
        bool flag = false;
        for (int i = 0; i < attrs.Count; i++) {
          if (a.GetType() == attrs[i].GetType()) {
            flag = true;
            break;
          }
        }
        if (!flag) attrs.Add(a);
      }
      return new AttributeCollection(attrs.ToArray());
    }

    static public ICollection GetDetailAttributes(object value, PropertyDescriptor pd) {
      PropertyType myType = GetPropertyTypeFromType(pd.PropertyType);
      PropertyInfo pi = value.GetType().GetProperty(pd.Name);
      if (pi == null) return new Attribute[0];
      else return (ICollection)pi.GetCustomAttributes(true);
    }

    static public Attribute[] GetHeaderAttributes(ITypeDescriptorContext context, object value) {
      ICollection baseAttrs = null;
      bool readOnly = false;
      if (context == null || context.PropertyDescriptor == null) {// Root object == Atrributes in TypeDescriptor.GetAttributes(value)
        baseAttrs = value.GetType().GetCustomAttributes(true);
      }
      else {
        //        baseAttrs = context.PropertyDescriptor.Attributes;
        PropertyInfo pi = context.Instance.GetType().GetProperty(context.PropertyDescriptor.Name);
        if (pi == null) {// Arrays
          baseAttrs = new List<Attribute>();
        }
        else {
          baseAttrs = pi.GetCustomAttributes(true);
        }
        if (context.PropertyDescriptor is PGPropertyDescriptor) {
          readOnly = ((PGPropertyDescriptor)context.PropertyDescriptor).IsReadOnly;
        }
      }
      List<Attribute> attrList = new List<Attribute>();
      if (readOnly) attrList.Add(new ReadOnlyAttribute(true));
      foreach (object attr in baseAttrs) {
        if (attr is ReadOnlyAttribute || attr is BrowsableAttribute) {
          attrList.Add((Attribute)attr);
        }
      }
      return attrList.ToArray();
    }

    // ========================  GET Parent (GridItem) ========================
    /*  ??? 2010-05-16 Мешает*/
    public static List<GridItem> GetParents(GridItem gridItem) {
      List<GridItem> ob = new List<GridItem>();
      GridItem curr = gridItem;
      while (curr != null) {
        while (curr.Parent != null) {
          curr = curr.Parent;
          if (curr.GridItemType != GridItemType.Category) {
            ob.Add(curr);
          }
        }
        object x1 = csUtilsReflection.GetPropertyRecurs(curr, "OwnerGrid");
        curr = null;
        if (x1 != null) {// Check parent object for Collection
          PropertyGrid pg = (PropertyGrid)x1;
          object tag = pg.ParentForm.Tag;
          if (tag != null) {
            curr = (GridItem)((cs.PGCollectionEditor)tag).baseContext;
            ob.Add(curr);
          }
        }
      }
      return ob;
    }

    static public PropertyType GetPropertyTypeFromValue(object value) {
      if (value is Type) return PropertyType.Type;
      if (value is string) return PropertyType.Primitive;
      if (value is Enum) return PropertyType.Enumeration;
      return GetPropertyTypeFromType(value.GetType());
    }
    static public PropertyType GetPropertyTypeFromType(Type type) {
      if (type.IsEnum) return PropertyType.Enumeration;
      if (type == typeof(string)) return PropertyType.Primitive;
      if (type.IsValueType) {
        if (type == typeof(DictionaryEntry)) return PropertyType.KeyValuePair;
        if (type.IsGenericType) {
          Type t1 = type.GetGenericTypeDefinition();
          if (t1 != null && t1.FullName == @"System.Collections.Generic.KeyValuePair`2")
            return PropertyType.KeyValuePair;
        }
        /* 2010-05-18       PropertyInfo pi1 = type.GetProperty("Key");
                if (pi1 != null) {
                  PropertyInfo pi2 = type.GetProperty("Value");
                  if (pi2 != null) return PropertyType.KeyValuePair;
                }*/
        return PropertyType.Structure;
      }
      if (type == typeof(System.Type)) return PropertyType.Type;
      if (type.IsClass) {
        if (type.GetInterface("IEnumerable") != null) return PropertyType.Collection;
        return PropertyType.Object;
      }
      throw new Exception("Can not defined property type for " + type.FullName);
    }
    static public bool IsClass(Type type) {
      return type.IsClass && type != typeof(string) && type.FullName != "System.Type";
    }

    // ===========================   Collections ========================
    static public object CreateCollectionItem(Type collectionType) {
      if (collectionType.IsArray) return CreateInstance(collectionType.GetElementType());
      if (collectionType.IsGenericType) {//Generic.KeyValuePair
        Type[] types = collectionType.GetGenericArguments();
        switch (types.Length) {
          case 1: return CreateInstance(types[0]);
          case 2:
            object key = CreateInstance(types[0]);
            object value = CreateInstance(types[1]);
            return Activator.CreateInstance(GetCollectionItemType(collectionType), new object[] { key, value });
        }
        throw new Exception("Can not define collection item type for " + collectionType.GetType().FullName);
      }
      //      if (collectionType == typeof(StringDictionary)) return new DictionaryEntry("", "");
      Type itemType = null;
      if (collectionType.GetInterface("IDictionary") != null) itemType = typeof(DictionaryEntry);
      else {
        PropertyInfo[] properties = collectionType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        for (int i = 0; i < properties.Length; i++) {
          if (properties[i].Name.Equals("Item") || properties[i].Name.Equals("Items")) {
            itemType = properties[i].PropertyType;
            break;
          }
        }
      }
      if (itemType != null) return CreateInstance(itemType);
      return null;
    }

		class KeyValuePairWrapper<TKey, TValue> {
			TKey _key;
			TValue _value;

			public KeyValuePairWrapper(TKey key, TValue value) {
				this._key = key; this._value = value;
			}

			public TKey Key {
				get { return this._key; }
				set { this._key = value; }
			}
			public TValue Value {
				get { return this._value; }
				set { this._value = value; }
			}
		}

    static public object UICreateCollectionItem(Type collectionType, object defaultItem, ArrayList dictKeys, object owner) {
      object item = CreateCollectionItem(collectionType);
      if (item == null) {
        item = defaultItem;
      }
      if (item != null) {
        PGUtils.PropertyType myType = PGUtils.GetPropertyTypeFromValue(item);
        if (myType == PGUtils.PropertyType.KeyValuePair) {
          object key = PGUtils.GetValueFromObject(item, "Key");
          object value = PGUtils.GetValueFromObject(item, "Value");
          if (key != null && value != null) {
//						KeyValuePairWrapper<object, object> _data = new KeyValuePairWrapper<object, object>(key, value);
/*						KeyValuePairWrapper<string, object>[] _data=new KeyValuePairWrapper<string,object>[2];
												_data[0] = new KeyValuePairWrapper<string, object>("Key", key);
												_data[1] = new KeyValuePairWrapper<string, object>("Value", value);
												KeyValuePair<string, object>[] _data=new KeyValuePair<string,object>[2];
						_data[0] = new KeyValuePair<string, object>("Key", key);
						_data[1] = new KeyValuePair<string, object>("Value", value);*/
						Dictionary<string, object> _data = new Dictionary<string, object>(2);
						_data.Add("Key", key);
						_data.Add("Value", value);
						ApplyTypeConverter(key);
            ApplyTypeConverter(value);
            ApplyTypeConverter(_data);
//						_data.to
            while (true) {// Cycle while value does not enter
              object o = PGfrmObjectEditor._GetObject(_data, null, true);
              if (o == null) return null;// Cancel
//							key = _data.Key;
	//						value = _data.Value;
/*							key = _data[0].Value;
							value = _data[1].Value;*/
							key = _data["Key"];
							value = _data["Value"];
							if (dictKeys != null && dictKeys.Contains(key)) {
                MessageBox.Show("'" + key.ToString() + "' key already exists in collection");// start to enter value from begining
              }
              else {
                item= Activator.CreateInstance(item.GetType(), new object[] { key, value });
                break;
              }
            }
          }
        }
      }
/*      if (item is IPG_OwnerSupport) {
        ((IPG_OwnerSupport)item).SetOwner(owner);
      }*/
      return item;
    }

    public static object CreateInstance(Type instanceType) {
      if (instanceType == typeof(string)) return "";
      try {
        return Activator.CreateInstance(instanceType);
      }
      catch { return null; }
    }

    static public Type GetCollectionItemType(Type aType) {
      if (aType.IsArray) return aType.GetElementType();
      else {
        if (aType.IsGenericType) {
          Type[] types = aType.GetGenericArguments();
          Type collType = null;
          switch (types.Length) {
            case 1: collType = types[0]; break;
            case 2: collType = Type.GetType("System.Collections.Generic.KeyValuePair`2[[" + types[0] + "],[" + types[1] + "]]"); break;
          }
          if (collType != null) {
            return (collType == aType ? null : collType);
          }
          throw new Exception("Can not define collection item type for " + aType.GetType().FullName);
        }
        if (aType.GetInterface("IDictionary") != null) return typeof(DictionaryEntry);
        PropertyInfo[] properties = aType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        for (int i = 0; i < properties.Length; i++) {
          if (properties[i].Name.Equals("Item") || properties[i].Name.Equals("Items")) {
            return properties[i].PropertyType;
          }
        }
      }
      return null;
    }

    // =================================  Properties =======================================
    public static PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes) {

      PGUtils.PropertyType myType = PGUtils.GetPropertyTypeFromValue(value);
      PropertyDescriptorCollection baseProps = new PropertyDescriptorCollection(null);

      if (value != null && value.GetType().Name == "Color") {
      }


      bool isReadOnly = false;
      List<GridItem> parents = null;
      if (context != null && context is GridItem) {
        parents = PGUtils.GetParents((GridItem)context);
      }

      AttributeCollection tmpAttrs = new AttributeCollection(GetHeaderAttributes(context, value));
      if (tmpAttrs.Contains(ReadOnlyAttribute.Yes)) isReadOnly = true;
      if (!isReadOnly && parents != null) {
        for (int i = 0; i < parents.Count; i++) {
          if (parents[i].PropertyDescriptor != null && parents[i].PropertyDescriptor.Attributes.Contains(ReadOnlyAttribute.Yes)) {
            isReadOnly = true;
            break;
          }
        }
      }

//context== null
//      value.GetType().GetCustomAttributes(true)
//      AttributeCollection contextAttrs = new AttributeCollection(GetHeaderAttributes(context, value));
      AttributeCollection contextAttrs = null;
      bool bbb = false;
      if (value != null) {
        if (TypeDescriptor.GetAttributes(value).Contains(new cs.PG_IsShowNameOfArrayElement(true))) {
          bbb = true;
        }
      }
      if (context != null && context.PropertyDescriptor != null) {
        bbb = context.PropertyDescriptor.Attributes.Matches(new cs.PG_IsShowNameOfArrayElement(true));
      }
      if (isReadOnly)
        contextAttrs = new AttributeCollection(new Attribute[] { ReadOnlyAttribute.Yes , new cs.PG_IsShowNameOfArrayElement(bbb) });
      else
        contextAttrs = new AttributeCollection(new Attribute[] { ReadOnlyAttribute.No, new cs.PG_IsShowNameOfArrayElement(bbb) });

      switch (myType) {
        case PGUtils.PropertyType.Collection: // arrays, collections, dictionaries, etc..

          Attribute[] arrayAttrs =new Attribute[contextAttrs.Count];
          contextAttrs.CopyTo(arrayAttrs, 0);
          IEnumerable ienum = (IEnumerable)value;
          int cnt = 0;
          foreach (object item in ienum) {
            PGPropertyDescriptorArray pda = new PGPropertyDescriptorArray(ienum, item, "#" + cnt++, arrayAttrs);
            baseProps.Add(pda);
          }
          // Если создан класс на базе IEnumerable, то дополнительные свойства,которые помечены атрибутом 
          // Browsable должны попасть в список свойств
          // Нижележащий код отсекает попадание в список свойств, свойств системных IEnumerable объектов (напр., Count/Capacity/KeyCollection etc.)
          PropertyDescriptorCollection pdc_temp = TypeDescriptor.GetProperties(value, attributes);
          for (int i = 0; i < pdc_temp.Count; i++) {
            string s = pdc_temp[i].Name;
            if (pdc_temp[i].Attributes.Matches(BrowsableAttribute.Yes)) {
              baseProps.Add(pdc_temp[i]);
            }
          }
          break;
        //         ??? 2010-05-16 Не могу активировать этот кусок кода. Он мешает через вызов PGUtils.GetParents 
        case PGUtils.PropertyType.KeyValuePair:// Only for PropertyGrid ( Not TreeView)

          Attribute[] arrayAttrs1 = new Attribute[contextAttrs.Count];
          contextAttrs.CopyTo(arrayAttrs1, 0);
          object key = PGUtils.GetValueFromObject(value, "Key");
          GridItem parent = null;
          for (int cnt2 = 0; cnt2 < parents.Count; cnt2++) {
            if (parents[cnt2].Value is IDictionary) {
              parent = parents[cnt2];
              break;
            }
          }
          IEnumerable ienum1 = (IEnumerable)parent.Value;
          IEnumerator en = ienum1.GetEnumerator();
          en.Reset();
          int cnt1 = 0;
          while (en.MoveNext()) {
            object xx = en.Current;
            object key1 = PGUtils.GetValueFromObject(xx, "Key");
            if (object.Equals(key, key1)) {// Поиск нашего элемента
              object val = GetValueFromObject(value, "Value");
              PGPropertyDescriptorArray pda = new PGPropertyDescriptorArray(parent.Value, en.Current, "#" + cnt1, arrayAttrs1);
              baseProps.Add(pda);
              break;
            }
            cnt1++;
          }
          break;
        default: // class
          if (myType == PGUtils.PropertyType.KeyValuePair) {
          }
          baseProps = TypeDescriptor.GetProperties(value, attributes);
          break;
      }
      return cs.PGUtils.RebuildPropertyDescriptors(baseProps, context, value, contextAttrs);
    }

    static public PropertyDescriptorCollection RebuildPropertyDescriptors(
  PropertyDescriptorCollection baseProps, ITypeDescriptorContext context, object value, AttributeCollection headerAttrs) {

      // Prepare temporary list
      Dictionary<string, PropertyDescriptor> list = new Dictionary<string, PropertyDescriptor>();
      foreach (PropertyDescriptor oldPD in baseProps) {
        PGUtils.ApplyTypeConverter(oldPD.PropertyType);
        if (oldPD is PGPropertyDescriptorArray) {// Array property descriptions do not change
          list.Add(oldPD.Name, oldPD);
        }
        else {
          ICollection detailAttrs = GetDetailAttributes(value, oldPD);
          AttributeCollection attrs = MergeAttributes(detailAttrs, headerAttrs);
          PGPropertyDescriptor pd1 = new PGPropertyDescriptor(oldPD, attrs);
          pd1.ApplyAttributes(value);
          list.Add(oldPD.Name, pd1);
        }
      }
      // User can change property list
      if (value is cs.IPG_AdjustProperties) {
        ((IPG_AdjustProperties)value).AdjustProperties(list, context);
      }
      // Prepare out property list
      List<PropertyDescriptor> outList = new List<PropertyDescriptor>();
      foreach (KeyValuePair<string, PropertyDescriptor> kvp in list) {
        if (kvp.Value.IsBrowsable) outList.Add(kvp.Value);
      }
      return new PropertyDescriptorCollection(outList.ToArray());
    }


    //=================================   Property Descriptor ============================
    public static bool PD_IsReadOnly(PropertyDescriptor pd) {
      ReadOnlyAttribute x = (ReadOnlyAttribute)pd.Attributes[typeof(ReadOnlyAttribute)];
      if (x != null) return x.IsReadOnly;
      else return false;
    }

    public static bool PD_CanWrite(PropertyDescriptor pd) {
      PropertyInfo pi = pd.ComponentType.GetProperty(pd.Name);
      return pi.CanWrite;
    }

    public static object PD_GetEditor(Type objectType, object defaultEditor) {
      if (defaultEditor == null && objectType == typeof(object)) {// Object Editor
        return new TestPropertyGrid.cs.Support.UIObjectEditor();
      }
      if (defaultEditor != null && defaultEditor.GetType() == typeof(PGCollectionEditor)) {
      }
      if (defaultEditor != null && defaultEditor.GetType() == typeof(System.ComponentModel.Design.CollectionEditor))
        return new PGCollectionEditor(objectType);
      if (defaultEditor != null && defaultEditor.GetType() == typeof(System.ComponentModel.Design.ArrayEditor))
        return new PGCollectionEditor(objectType);
      return defaultEditor;
      //      if (this._attrList == null) return defaultEditor;
      //    return null;
    }

  }
}
