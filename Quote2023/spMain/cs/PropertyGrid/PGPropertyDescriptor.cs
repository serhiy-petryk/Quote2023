using System;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;

namespace spMain.cs {
  public class PGPropertyDescriptor : PropertyDescriptor {

		public enum ListType { NoList, BlankList, StringList, ObjectList };
		//////////////////////////   Fields ////////////////////////////// 
		private PropertyDescriptor basePropertyDescriptor;

		public BrowsableAttribute _attrBrowsable;
		public ReadOnlyAttribute _attrReadOnly;// Extend from Parent to child
		// Item attributes
//		public cs.PG_IsFixedCollectionAttribute _attrIsFixedCollection; // Set in CollectionEditor
		public cs.PG_ListAttribute _attrList;
		public DescriptionAttribute _attrDescription;
		public DisplayNameAttribute _attrDisplayName;

		private AttributeCollection _parentAttrs;
		private string _dynamicCall;
		private object _dynamicCallArgs;
		private AttributeCollection _cachedAttrs;
		private bool flagConverter;
    private bool flagEditor;
    private Type editorType=null;

		//////////////////////////   Constructor  ////////////////////////////// 
		public PGPropertyDescriptor(PropertyDescriptor basePropertyDescriptor, AttributeCollection parentAttrs)
			: base(basePropertyDescriptor) {
			this.basePropertyDescriptor = basePropertyDescriptor;
			this._parentAttrs = parentAttrs;
		}

		public void ApplyAttributes(object value) {
			this.ResetAttributes();
			this.ApplyAttributes(this._parentAttrs, false);
			this.ApplyAttributes(base.Attributes, true);
			if (this.PropertyType.IsEnum) {
				Dictionary<string, object> x = PGUtils.GetEnumList(this.PropertyType);
				if (x != null) {
					this._attrList = new PG_ListAttribute(x, false);
				}
			}
			if (!String.IsNullOrEmpty(_dynamicCall)) {
				MethodInfo mi = value.GetType().GetMethod(this._dynamicCall);
				mi.Invoke(value, new object[] { this, this._dynamicCallArgs });
			}
		}

		void ResetAttributes() {
			this._attrBrowsable = null;
			this._attrDescription = null;
			this._attrDisplayName = null;
//			this._attrIsFixedCollection = null;
			this._attrList = null;
			this._attrReadOnly = null;
			this._dynamicCall = null;
			this._dynamicCallArgs = null;
			this._cachedAttrs = null;
			this.flagConverter = false;
			this.flagEditor = false;
      this.editorType = null;
		}

		void ApplyAttributes(AttributeCollection attrs, bool baseAttrFlag) {
      foreach (Attribute a in attrs) {
				if (a is BrowsableAttribute) {
					this._attrBrowsable = (BrowsableAttribute)a;
					continue;
				}
        if (a is ReadOnlyAttribute) {
          if (baseAttrFlag) {// type attribute (missing for base attribute set) == if not 'Set' for object than ReadOnly=true = not valid
            continue;
          }
          else {// custom attributes
            this._attrReadOnly = (ReadOnlyAttribute)a;
          }
					continue;
				}
				if (a is PG_ListAttribute) {
					this._attrList = (PG_ListAttribute)a;
					continue;
				}
				if (a is DisplayNameAttribute) {
					this._attrDisplayName = (DisplayNameAttribute)a;
					continue;
				}
				if (a is DescriptionAttribute) {
					this._attrDescription = (DescriptionAttribute)a;
					continue;
				}
				if (a is PG_DynamicCallAttribute) {
					PG_DynamicCallAttribute x = (PG_DynamicCallAttribute)a;
					this._dynamicCall = x.dynamicCallName;
					this._dynamicCallArgs = x.args;
					continue;
				}
				if (a is TypeConverterAttribute) this.flagConverter = true;
        if (a is EditorAttribute) {
          this.flagEditor = true;
          this.editorType = Type.GetType(((EditorAttribute)a).EditorTypeName);

        }
			}

/*      // 3 below lines were added by sp 2010-05-18 == Если не установлено set для array, то 
      // MyCollectionEditor не позволяет добавлять данные для массивов, но их можно изменить
      if (type.IsArray && (this._attrIsFixedCollection == null || !this._attrIsFixedCollection.isFixedCollection) && 
        objectPI !=null && !objectPI.CanWrite) {
        this._attrIsFixedCollection = new PG_IsFixedCollectionAttribute(true);
      }*/
      return;
		}

		public override TypeConverter Converter {
			get {
				if (this._attrList != null) {
					return (TypeConverter)this.CreateInstance(typeof(cs.PGTypeConverter));// List
				}
				Type type = typeof(cs.PGTypeConverter);
				if ((PGUtils.IsClass(this.PropertyType)) && !this.flagConverter &&
					typeof(TypeConverter).IsAssignableFrom(type)) {
					return (TypeConverter)this.CreateInstance(type); // Create instance of converter type
				}
				return base.Converter;
			}
		}

		public override AttributeCollection Attributes {
			get {
				if (this._cachedAttrs == null) {
					List<Attribute> x = new List<Attribute>();
					foreach (Attribute a in base.Attributes) {
						if (a is ReadOnlyAttribute || a is BrowsableAttribute || a is PG_DynamicCallAttribute ||
//							a is PG_IsFixedCollectionAttribute || 
              a is PG_ListAttribute || a is DescriptionAttribute ||
							a is DisplayNameAttribute || a is EditorAttribute) {
						}
						else {
							x.Add(a);
						}
					}
					if (this._attrReadOnly != null && this._attrReadOnly.IsReadOnly) x.Add(this._attrReadOnly);
					if (this._attrBrowsable != null && !this._attrBrowsable.Browsable) x.Add(this._attrBrowsable);
					if (this._attrDescription != null && !String.IsNullOrEmpty(this._attrDescription.Description))
						x.Add(this._attrDescription);
					if (this._attrDisplayName != null && !String.IsNullOrEmpty(this._attrDisplayName.DisplayName))
						x.Add(this._attrDisplayName);
/*					if (this._attrIsFixedCollection != null && this._attrIsFixedCollection.isFixedCollection) 
						x.Add(this._attrIsFixedCollection);*/
					if (this._attrList != null) x.Add(this._attrList);
					this._cachedAttrs = new AttributeCollection(x.ToArray());
				}
				return this._cachedAttrs;
			}
		}

		public override object GetEditor(Type editorBaseType) {
      if (this.PropertyType.Name.ToLower() == "indicatorcolor") {
      }
      object x;
      if (this.editorType != null) x = Activator.CreateInstance(this.editorType);
      else x = base.GetEditor(editorBaseType);
      if (x == null && this.PropertyType == typeof(object)) {// Object Editor
        return new TestPropertyGrid.cs.Support.UIObjectEditor();
      }
      if (x != null && x.GetType() == typeof(PGCollectionEditor)) {
      }
      if (x != null && x.GetType() == typeof(System.ComponentModel.Design.CollectionEditor))
        return new PGCollectionEditor(this.PropertyType);
      if (x != null && x.GetType() == typeof(System.ComponentModel.Design.ArrayEditor))
        return new PGCollectionEditor(this.PropertyType);
      return x;
//      if (this._attrList == null) return x;
	//		return null; last 3 lines changed at 2010-07-25
		}
		public override bool IsReadOnly {
			get {
				ReadOnlyAttribute x = (ReadOnlyAttribute)this.Attributes[typeof(ReadOnlyAttribute)];
				if (x != null) return x.IsReadOnly;
				else return false;
			}
		}

		public override string Description {
			get {
				if (this._attrDescription == null) return basePropertyDescriptor.Description;
				else return this._attrDescription.Description;
			}
		}
		public override string DisplayName {
			get {
				if (this._attrDisplayName == null) return basePropertyDescriptor.DisplayName;
				else return this._attrDisplayName.DisplayName;
			}
		}

		//////////////////////////   Base  ////////////////////////////// 
		public override Type ComponentType { // Get type of object (component)
			get {
				return basePropertyDescriptor.ComponentType;
			}
		}
		public override Type PropertyType {// Get type of element
			get {
				return this.basePropertyDescriptor.PropertyType;
			}
		}
		public override bool CanResetValue(object component) {
			return this.basePropertyDescriptor.CanResetValue(component);
		}
		public override object GetValue(object component) {// GetValue == get element of this type from object(component)
//  added by sp 2010-05-17    
  if (component != null) {
        PGUtils.PropertyType myType = PGUtils.GetPropertyTypeFromValue(component);
        if (myType == PGUtils.PropertyType.KeyValuePair) {
          return PGUtils.GetValueFromObject(component, "Value");
        }
      }
      if (this._attrList != null && this._attrList._dataByValue!=null) {// && this._attrList is IDictionary) {
        if (this._attrList._dataByValue.Count > 1) {
        }

      }
			return this.basePropertyDescriptor.GetValue(component);
		}
		public override void ResetValue(object component) {
			this.basePropertyDescriptor.ResetValue(component);
		}
		public override void SetValue(object component, object value) {
/*      if (this._attrList != null && this._attrList._dataByValue is IDictionary) {// changed (clear comment at 2010-07-28
        this.basePropertyDescriptor.SetValue(component,((IDictionary)this._attrList._dataByValue)[value]);
      }*/
      this.basePropertyDescriptor.SetValue(component, value);
    }
		public override bool ShouldSerializeValue(object component) {
			return this.basePropertyDescriptor.ShouldSerializeValue(component);
		}
/*		public override string Name {
			get {
				return basePropertyDescriptor.Name;
			}
		}
/*		public override bool SupportsChangeEvents {
			get {
				return base.SupportsChangeEvents;
			}
		}
		public override void AddValueChanged(object component, EventHandler handler) {
			base.AddValueChanged(component, handler);
		}
		protected override void OnValueChanged(object component, EventArgs e) {
			base.OnValueChanged(component, e);
		}*/

	}
}
