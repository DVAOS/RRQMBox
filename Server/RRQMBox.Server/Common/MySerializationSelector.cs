using RRQMCore.Helper;
using RRQMCore.Serialization;
using RRQMCore.XREF.Newtonsoft.Json;
using RRQMSocket.RPC;
using RRQMSocket.RPC.RRQMRPC;
using System;
using System.Text;

namespace RRQMBox.Server.Common
{
    public class MySerializationSelector : SerializationSelector
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="serializationType"></param>
        /// <param name="parameterBytes"></param>
        /// <param name="parameterType"></param>
        /// <returns></returns>
        public override object DeserializeParameter(SerializationType serializationType, byte[] parameterBytes, Type parameterType)
        {
            if (parameterBytes == null)
            {
                return parameterType.GetDefault();
            }
            switch (serializationType)
            {
                case SerializationType.RRQMBinary:
                    {
                        return SerializeConvert.RRQMBinaryDeserialize(parameterBytes, 0, parameterType);
                    }
                case SerializationType.SystemBinary:
                    {
                        return SerializeConvert.BinaryDeserialize(parameterBytes, 0, parameterBytes.Length);
                    }
                case SerializationType.Json:
                    {
                        return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(parameterBytes), parameterType);
                    }
                case SerializationType.Xml:
                    {
                        return SerializeConvert.XmlDeserializeFromBytes(parameterBytes, parameterType);
                    }
                case (SerializationType)4:
                    {
                        //此处可自行实现
                        return default;
                    }
                default:
                    throw new RRQMRPCException("未指定的反序列化方式");
            }
        }

        /// <summary>
        /// 序列化参数
        /// </summary>
        /// <param name="serializationType"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override byte[] SerializeParameter(SerializationType serializationType, object parameter)
        {
            if (parameter == null)
            {
                return null;
            }
            switch (serializationType)
            {
                case SerializationType.RRQMBinary:
                    {
                        return SerializeConvert.RRQMBinarySerialize(parameter, true);
                    }
                case SerializationType.SystemBinary:
                    {
                        return SerializeConvert.BinarySerialize(parameter);
                    }
                case SerializationType.Json:
                    {
                        return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(parameter));
                    }
                case SerializationType.Xml:
                    {
                        return SerializeConvert.XmlSerializeToBytes(parameter);
                    }
                case (SerializationType)4:
                    {
                        //此处可自行实现
                        return default;
                    }
                default:
                    throw new RRQMRPCException("未指定的序列化方式");
            }
        }
    }
}