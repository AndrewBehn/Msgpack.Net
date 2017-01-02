namespace Msgpack
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

    public class MsgpackConverterSettings
    {
        public static readonly MsgpackConverterSettings Default = new MsgpackConverterSettings();
        public NullValueHandling NullValueHandling { get; set; } = NullValueHandling.Ignore;
        public NameValueHandling NameValueHandling { get; set; } = NameValueHandling.Standard;
    }
}