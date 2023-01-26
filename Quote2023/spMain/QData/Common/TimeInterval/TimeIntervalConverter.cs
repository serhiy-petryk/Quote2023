using System;
using System.Globalization;
using System.ComponentModel;

namespace spMain.QData.Common {
  public partial class TimeInterval {
    class TimeIntervalConverter : TypeConverter {

      private static StandardValuesCollection values;
      private static string valuesLock = "lock";

      public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
        return ((sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType));
      }

      public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
        return ((destinationType == typeof(System.ComponentModel.Design.Serialization.InstanceDescriptor)) ||
          base.CanConvertTo(context, destinationType));
      }

      public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
        string str = value as string;
        return new TimeInterval(str);
      }

      public override bool GetStandardValuesSupported(ITypeDescriptorContext context) {
        return true;
      }

      public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) {
        if (values == null) {
          lock (valuesLock) {
            if (values == null) {
              values = new TypeConverter.StandardValuesCollection(new object[] {
              new TimeInterval(1),new TimeInterval(2),new TimeInterval(3),new TimeInterval(5),new TimeInterval(10),
              new TimeInterval(15),new TimeInterval(30),
              new TimeInterval(1*60),new TimeInterval(2*60),new TimeInterval(3*60),new TimeInterval(5*60),new TimeInterval(10*60),
              new TimeInterval(15*60),new TimeInterval(30*60),new TimeInterval(60*60),
                new TimeInterval(-1),new TimeInterval(-2),
              new TimeInterval(-3),new TimeInterval(-4)});
            }
          }
        }
        return values;
      }
    }
  }
}
