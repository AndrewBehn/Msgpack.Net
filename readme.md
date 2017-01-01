# Msgpack.Net

Msgpack.Net is a C# / .Net Core implementation of msgpack.  The API is very similar to Json.Net, as well as the implementation.  The project has no dependencies beyond the .Net Core SDK!

## Installation

Though the packages have not been published yet, the intended way to eventually consume this library will be through NuGet.

## Usage

The easiest wayt to use Msgpack.Net is through the MsgpackConvert class.  This class simply provides a static wrapper around a default MsgpackSerializer instance, and also allows the user to pass custom MsgpackSerializerSettings for their operation.  If a user would like more control over how a particular type is serialized, simply write a custom MsgpackConverter, and add it to the Serializer's ConverterCache.  Alternatively, you can decorate a type with a TypeConverter attribute to control its serialization.  Finally, for the lowest level of control, write a custom MgspackReader or MsgpackWriter to control the way that individual components of the binary msgpack representation are read and written. 

## Contributing

If you have found a bug, or an have an idea for an improvement, please fork the repo, make the changes and open a pull-request.  The current github settings are very strict about ensuring that all builds and tests pass before a PR can be merged.  In addition to satisfying these requirements I will require that any ug fixes have regression tests, and any new features have new tests to verify their implementation.

## History

Msgpack.Net was created to provide a powerful yet simple msgpack serializer/deserializer.  The API and source code was heavily inspired by Json.Net, though the code here should be a good deal simpler.  

## Credits

Andrew Behn, Developer and Project Owner

## License

TODO: Write license