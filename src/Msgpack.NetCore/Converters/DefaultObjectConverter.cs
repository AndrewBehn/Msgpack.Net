using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Msgpack.Extensions;
using Msgpack.Serializer;
using Msgpack.Attrributes;

namespace Msgpack.Converters
{
    public class DefaultObjectConverter : MsgpackConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return !PrimitiveConverter.TypeCodeMap.ContainsKey(objectType);
        }

        public override void WriteMsgpack(MsgpackWriter writer, object value, Type objectType,
            MsgpackConverterSettings settings)
        {
            var converterAttribute = objectType.GetTypeInfo().GetCustomAttribute<MsgpackTypeConverterAttribute>();
            if (converterAttribute != null)
            {
                var converterType = converterAttribute.ConverterType;
                var converter = (MsgpackConverter)Activator.CreateInstance(converterType);
                converter.WriteMsgpack(writer, value, objectType, settings);
            }
            else
            {
                WriteDefaultMsgpack(writer, value, objectType, settings);
            }
        }

        private void WriteDefaultMsgpack(MsgpackWriter writer, object value, Type objectType, MsgpackConverterSettings settings)
        {
            if (value == null)
            {
                writer.WriteNil();
                return;
            }

            var dataContractType = objectType.GetTypeInfo().GetCustomAttribute<DataContractAttribute>() != null;
            Func<MemberInfo, bool> shouldSerialize = mi =>
            {
                if (dataContractType)
                    return mi.GetCustomAttribute<DataMemberAttribute>() != null;

                return true;
            };

            Func<MemberInfo, string> getName = mi =>
            {
                if (dataContractType)
                {
                    var dcname = mi.GetCustomAttribute<DataMemberAttribute>().Name;
                    if (!string.IsNullOrEmpty(dcname))
                        return dcname;
                }
                var infoName = mi.Name;
                if (settings.NameValueHandling == NameValueHandling.Lower)
                    infoName = infoName.ToLower();

                return infoName;
            };

            var members = objectType.GetPropertiesAndFields();
            var memberInfos = members as MemberInfo[] ?? members.ToArray();

            var writeActions = new List<Action>();

            foreach (var m in memberInfos)
                if (shouldSerialize(m))
                {
                    var infoName = getName(m);

                    object propValue = null;
                    Type memberType = null;

                    if (m is PropertyInfo)
                    {
                        var info = m as PropertyInfo;
                        propValue = info.GetValue(value);
                        memberType = info.PropertyType;
                    }
                    else if (m is FieldInfo)
                    {
                        var info = m as FieldInfo;
                        propValue = info.GetValue(value);
                        memberType = info.FieldType;
                    }


                    if (!((propValue == null) && (settings.NullValueHandling == NullValueHandling.Ignore)))
                        writeActions.Add(() =>
                        {
                            var stringConverter = ConverterCache.GetConverter(typeof(string));
                            stringConverter.WriteMsgpack(writer, infoName, typeof(string), settings);

                            var valueConverter = ConverterCache.GetConverter(memberType);
                            valueConverter.WriteMsgpack(writer, propValue, memberType, settings);
                        });
                }

            writer.WriteObjectSize(writeActions.Count);
            writeActions.ForEach(a => a());
        }

        public override object ReadMsgpack(MsgpackReader reader, Type objectType)
        {
            var dataContractType = objectType.GetTypeInfo().GetCustomAttribute<DataContractAttribute>() != null;

            Func<string, MemberInfo> getMemberInfo = name =>
            {
                if (dataContractType)
                    // find property with data contract attribute name
                    return
                        objectType.GetProperties()
                            .FirstOrDefault(
                                p =>
                                    string.Equals(p.GetCustomAttribute<DataMemberAttribute>()?.Name, name,
                                        StringComparison.CurrentCultureIgnoreCase));

                return
                    objectType.GetSettablePropertiesAndFields()
                        .FirstOrDefault(m => string.Equals(m.Name, name, StringComparison.CurrentCultureIgnoreCase));
            };

            var instance = Activator.CreateInstance(objectType);




            var size = reader.ReadObjectSize();
            for (var i = 0; i < size; i++)
            {
                // use converter to string name

                var entryNameConverter = ConverterCache.GetConverter(typeof(string));
                var entryName = entryNameConverter.ReadMsgpack<string>(reader); //reader.ReadString();
                var memberInfo = getMemberInfo(entryName);
                if (memberInfo != null)
                {
                    Type memberType = null;
                    if (memberInfo is FieldInfo)
                    {
                        var fieldInfo = memberInfo as FieldInfo;
                        memberType = fieldInfo.FieldType;
                        var converter = ConverterCache.GetConverter(memberType);
                        var propertyValue = converter.ReadMsgpack(reader, memberType);
                        if (propertyValue != null)
                            fieldInfo.SetValue(instance, propertyValue);
                    }
                    else if (memberInfo is PropertyInfo)
                    {
                        var propertyInfo = memberInfo as PropertyInfo;
                        memberType = propertyInfo.PropertyType;
                        var converter = ConverterCache.GetConverter(memberType);
                        var propertyValue = converter.ReadMsgpack(reader, memberType);
                        if (propertyValue != null)
                            propertyInfo.SetValue(instance, propertyValue);
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
                else
                {
                    // read out value to skip over it
                    reader.ReadNext();
                }
            }

            return instance;
        }
    }
}