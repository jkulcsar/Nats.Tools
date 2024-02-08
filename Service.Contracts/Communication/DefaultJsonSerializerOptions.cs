using System.Text.Json;
using System.Text.Json.Serialization;

namespace Service.Contracts.Communication;

public static class DefaultJsonSerializerOptions
{
    public static readonly JsonSerializerOptions Default = GetDefaultJsonSerializerOptions();

    private static JsonSerializerOptions GetDefaultJsonSerializerOptions()
    {
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true
        };

        AddCustomConverters(options);

        return options;
    }

    private static void AddCustomConverters(JsonSerializerOptions options)
    {
        AddInterfaceConverters(options);
        AddMarkingDataConverters(options);

        //options.Converters.Add(new ApiVector3DSerializer());
        options.Converters.Add(new DictionaryConverter());
    }

    private static void AddInterfaceConverters(JsonSerializerOptions options)
    {
        // options.AddInterfaceConverter<IBoundingBox, ApiBoundingBox>();
        // options.AddInterfaceConverter<IPoint3D, ApiPoint3D>();
        // options.AddInterfaceConverter<IVector3D, ApiVector3D>();
        // options.AddInterfaceConverter<ISoftwareVersionInfo, SoftwareVersionInfo>();
        // options.AddInterfaceConverter<IStagingItemInfo, ApiStagingItemInfo>();
        // options.AddInterfaceConverter<IProcessItem, ApiProcessItem>();
        // options.AddInterfaceConverter<IProcessStepItem, ApiProcessStepItem>();
        // options.AddInterfaceConverter<IMarkingProcessState, ApiMarkingProcessState>();
        // options.AddInterfaceConverter<IMarkingGroup, ApiMarkingGroup>();
        // options.AddInterfaceConverter<IRotaryAxisInfo, ApiRotaryAxisInfo>();
        // options.AddInterfaceConverter<ILcpInfo, ApiLcpInfo>();
        // options.AddInterfaceConverter<IMarkingFocusTestInfo, ApiMarkingFocusInfo>();
        // options.AddInterfaceConverter<IApiCollisionInfo, ApiCollisionInfo>();
        // options.AddInterfaceConverter<IApiCollisionCornerInfo, ApiCollisionCornerInfo>();
        // options.AddInterfaceConverter<IConfigurationCategory, ApiConfigurationCategory>();
        // options.AddInterfaceConverter<ISetting, ApiSetting>();
        // options.AddInterfaceConverter<IDisplayInfo, ApiDisplayInfo>();
        // options.AddInterfaceConverter<IUnit, ApiUnit>();
        // options.AddInterfaceConverter<IDictionary<object, string>, Dictionary<object, string>>();
        // options.AddInterfaceConverter<IUpdateSettingInfo, ApiUpdateSettingInfo>();
    }

    private static void AddInterfaceConverter<TInterface, TImplementation>(this JsonSerializerOptions options)
        where TImplementation : class, TInterface
        where TInterface : class
        => options.Converters.Add(new InterfaceConverter<TImplementation, TInterface>());

    private static void AddMarkingDataConverters(JsonSerializerOptions options)
    {
        // options.AddInterfaceConverter<IMarkingContent, MarkingContent>();
        // options.AddInterfaceConverter<IMarkingData, MarkingData>();
        // options.AddInterfaceConverter<IHardwareParameter, HardwareParameter>();
        // options.AddInterfaceConverter<IProcessMetaData, ProcessMetaData>();
        // options.AddInterfaceConverter<IProcessParameter, ProcessParameter>();
        // options.AddInterfaceConverter<ILaserParameter, LaserParameter>();
        // options.AddInterfaceConverter<IMarkingMetaData, MarkingMetaData>();
        // options.AddInterfaceConverter<IMarkingParameter, MarkingParameter>();
        // options.AddInterfaceConverter<ITransformation, Transformation>();
        // options.AddInterfaceConverter<IVector, Vector>();
        // options.AddInterfaceConverter<IArc, Arc>();
        // options.AddInterfaceConverter<IBarcode1D, Barcode1D>();
        // options.AddInterfaceConverter<IBarcodeDataMatrix, BarcodeDataMatrix>();
        // options.AddInterfaceConverter<IBSpline, BSpline>();
        // options.AddInterfaceConverter<ICircle, Circle>();
        // options.AddInterfaceConverter<IEllipse, Ellipse>();
        // options.AddInterfaceConverter<IEllipseArc, EllipseArc>();
        // options.AddInterfaceConverter<ILine, Line>();
        // options.AddInterfaceConverter<IPoint, Point>();
        // options.AddInterfaceConverter<IPolyLine, PolyLine>();
        // options.AddInterfaceConverter<IProgrammableGeometry, ProgrammableGeometry>();
        // options.AddInterfaceConverter<IRectangle, Rectangle>();
        // options.AddInterfaceConverter<IText, Text>();
        // options.AddInterfaceConverter<IBarcodeTextStyle, BarcodeTextStyle>();
        // options.AddInterfaceConverter<ICircularText, CircularText>();
        // options.AddInterfaceConverter<ILineWidth, LineWidth>();
        // options.AddInterfaceConverter<IMultiLayerHatch, MultiLayerHatch>();
        // options.AddInterfaceConverter<ITextStyle, TextStyle>();
    }
}

public class InterfaceConverter<TImplementation, TInterface> : JsonConverter<TInterface>
    where TImplementation : class, TInterface
    where TInterface : class
{
    public override TInterface? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        JsonSerializer.Deserialize<TImplementation>(ref reader, options);

    public override void Write(Utf8JsonWriter writer, TInterface value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case null:
                JsonSerializer.Serialize(writer, (TImplementation?)null, options);
                break;
            default:
            {
                JsonSerializer.Serialize(writer, value, typeof(TImplementation), options);
                break;
            }
        }
    }
}

// public class ApiVector3DSerializer : JsonConverter<ApiVector3D>
// {
//     public override ApiVector3D Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//     {
//         if (reader.TokenType != JsonTokenType.StartObject)
//             throw new JsonException(
//                 $"The Json does not provide a 'StartObject' Token! Recognized token was '{reader.TokenType}'.");
//
//         var x = ReadDoubleFromPropertyName(ref reader, nameof(ApiVector3D.X));
//         var y = ReadDoubleFromPropertyName(ref reader, nameof(ApiVector3D.Y));
//         var z = ReadDoubleFromPropertyName(ref reader, nameof(ApiVector3D.Z));
//
//         if (!reader.Read() ||
//             reader.TokenType != JsonTokenType.EndObject)
//         {
//             throw new JsonException(
//                 $"The Json does not provide a 'EndObject' Token! Recognized token was '{reader.TokenType}'.");
//         }
//
//         return new ApiVector3D(x, y, z);
//     }
//
//     public override void Write(Utf8JsonWriter writer, ApiVector3D value, JsonSerializerOptions options)
//     {
//         switch (value)
//         {
//             case null:
//                 JsonSerializer.Serialize(writer, (ApiVector3D?)null, options);
//                 break;
//             default:
//             {
//                 JsonSerializer.Serialize(writer, value, options);
//                 break;
//             }
//         }
//     }
//
//     private static double ReadDoubleFromPropertyName(ref Utf8JsonReader reader, string propertyName)
//     {
//         if (!reader.Read()
//             || reader.TokenType != JsonTokenType.PropertyName
//             || reader.GetString() != propertyName)
//         {
//             throw new JsonException(
//                 $"A Property needs to start with a 'PropertyName' Token! Recognized token was '{reader.TokenType}'.");
//         }
//
//         if (!reader.Read()
//             || reader.TokenType != JsonTokenType.Number)
//         {
//             throw new JsonException(
//                 $"A Property needs to be followed by a 'Number' Token! Recognized token was '{reader.TokenType}'.");
//         }
//
//         return reader.GetDouble();
//     }
// }

public class DictionaryConverter : JsonConverter<Dictionary<object, string>>
{
    public override Dictionary<object, string> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dictionary = JsonSerializer.Deserialize<Dictionary<string, string>?>(ref reader, options) ?? new Dictionary<string, string>();
        return dictionary.ToDictionary(x => (object)x.Key, x => x.Value);
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<object, string> value, JsonSerializerOptions options)
    {
        var dictionary = value.ToDictionary(x => x.Key.ToString() ?? Guid.NewGuid().ToString(), x => x.Value);
        JsonSerializer.Serialize(writer, dictionary, options);
    }
}