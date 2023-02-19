using System;
using System.Collections;
using System.Reflection;

namespace spMain {
  static class csUtilsReflection {
    public static BindingFlags allBFs = BindingFlags.CreateInstance | BindingFlags.DeclaredOnly | BindingFlags.Default |
  BindingFlags.ExactBinding | BindingFlags.FlattenHierarchy | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.IgnoreCase |
  BindingFlags.IgnoreReturn | BindingFlags.IgnoreReturn | BindingFlags.Instance | BindingFlags.InvokeMethod |
  BindingFlags.NonPublic | BindingFlags.OptionalParamBinding | BindingFlags.Public | BindingFlags.PutDispProperty |
  BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.Static | BindingFlags.SuppressChangeType;

    public static object[] GetConstants(Type enumType, Type constantType) {
      MethodAttributes attributes = MethodAttributes.Static | MethodAttributes.Public;
      PropertyInfo[] properties = enumType.GetProperties();
      ArrayList list = new ArrayList();
      for (int i = 0; i < properties.Length; i++) {
        PropertyInfo info = properties[i];
        if (info.PropertyType == constantType) {
          MethodInfo getMethod = info.GetGetMethod();
          if ((getMethod != null) && ((getMethod.Attributes & attributes) == attributes)) {
            object[] index = null;
            list.Add(info.GetValue(null, index));
          }
        }
      }
      return list.ToArray();
    }


    public static object GetFieldRecurs(object obj, string sFieldName) {
      Type type = obj.GetType();
      while (type != null) {
        FieldInfo[] fis = type.GetFields(allBFs);
        for (int i = 0; i < fis.Length; i++) {
          if (fis[i].Name == sFieldName)
            return fis[i].GetValue(obj);
        }
        type = type.BaseType;
      }
      throw new Exception("Can not find field '" + sFieldName + "' in type " + obj.GetType().Name);
    }
    public static object GetPropertyRecurs(object obj, string sPropertyName) {
      Type type = obj.GetType();
      while (type != null) {
        PropertyInfo[] pis = type.GetProperties(allBFs);
        for (int i = 0; i < pis.Length; i++) {
          if (pis[i].Name == sPropertyName)
            return pis[i].GetValue(obj, null);
        }
        type = type.BaseType;
      }
      throw new Exception("Can not find property '" + sPropertyName + "' in type " + obj.GetType().Name);
    }
    public static object InvokeMethodRecurs(object obj, string sMethodName, object[] parameters) {
      Type type = obj.GetType();
      while (type != null) {
        MethodInfo[] mis = type.GetMethods(allBFs);
        for (int i = 0; i < mis.Length; i++) {
          if (mis[i].Name == sMethodName) {
            return mis[i].Invoke(obj, parameters);
            //            return pis[i].GetValue(obj, null);
          }
        }
        type = type.BaseType;
      }
      throw new Exception("Can not find method '" + sMethodName + "' in type " + obj.GetType().Name);
    }
    public static void SetField1(object obj, string sFieldName, object value) {
      FieldInfo[] fis = obj.GetType().GetFields(allBFs);
      for (int i = 0; i < fis.Length; i++) {
        if (fis[i].Name == sFieldName) {
          fis[i].SetValue(obj, value);
          return;
        }
      }
      throw new Exception("Can not find filed '" + sFieldName + "' in type " + obj.GetType().FullName);
    }
    public static void SetFieldUsingSignature(System.Type type, string sFieldName, object target, object newValue) {
      FieldInfo[] fields = type.GetFields(allBFs);
      FieldInfo fi = null;
      for (int i = 0; i < fields.Length; i++) {
        if (fields[i].Name == sFieldName) {
          fi = fields[i];
          break;
        }
      }
      if (fi != null) {
        fi.SetValue(target, newValue);
      }
      else throw new Exception("Can not find '" + sFieldName + "' field in '" + type.FullName + "' type");
    }

    public static void SetField(object obj, string sFieldName, object oValue) {
      obj.GetType().InvokeMember(sFieldName, BindingFlags.SetField, null, obj, new object[] { oValue }, Settings.ciUS);
    }
    public static object GetField(object obj, string sFieldName) {
      return obj.GetType().InvokeMember(sFieldName, BindingFlags.GetField, null, obj, null, Settings.ciUS);
    }
    public static void SetProperty(object obj, string sProperty, object oValue) {
      obj.GetType().InvokeMember(sProperty, BindingFlags.SetProperty | BindingFlags.Instance |
        BindingFlags.Public | BindingFlags.NonPublic, null, obj, new object[] { oValue }, Settings.ciUS);
      //      obj.GetType().InvokeMember(sProperty, BindingFlags.SetProperty, null, obj, new object[] { oValue }, csIni.ciUS);
    }
    public static object GetProperty(object obj, string sProperty) {
      return obj.GetType().InvokeMember(sProperty, BindingFlags.GetProperty | BindingFlags.Instance |
        BindingFlags.Public | BindingFlags.NonPublic, null, obj, null, Settings.ciUS);
      //return obj.GetType().InvokeMember(sProperty, BindingFlags.GetProperty, null, obj, null, csIni.ciUS);
    }
    public static object InvokeMethod(object obj, string sProperty, object[] oParam) {
      return obj.GetType().InvokeMember(sProperty, BindingFlags.InvokeMethod, null, obj, oParam, Settings.ciUS);
    }
    public static object InvokeMethod(object obj, string sProperty, object oValue) {
      object[] oParam = new object[1];
      oParam[0] = oValue;
      return obj.GetType().InvokeMember(sProperty, BindingFlags.InvokeMethod, null, obj, oParam, Settings.ciUS);
    }
  }
}
