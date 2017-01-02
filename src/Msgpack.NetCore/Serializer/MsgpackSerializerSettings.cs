namespace Msgpack.Serializer
{
    public enum NullValueHandling
    {
        Include,
        Ignore
    }

    public enum NameValueHandling
    {
        Lower,
        Standard
    }

    public class MsgpackSerializerSettings
    {
        public static readonly MsgpackSerializerSettings Default = new MsgpackSerializerSettings();
        public NullValueHandling NullValueHandling { get; set; } = NullValueHandling.Ignore;
        public NameValueHandling NameValueHandling { get; set; } = NameValueHandling.Standard;
    }
}