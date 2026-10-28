using FlatSharp;

using ProtoBuf;



namespace Matrix.BinarySerialization;



public static class SerializationData

{

    public static SimpleModel Simple() =>

        new()

        {

            Id = 42,

            Name = "Ada",

            Active = true

        };



    public static OrderModel Nested() =>

        new()

        {

            Id = 73,

            Customer = new CustomerModel

            {

                Name = "Ada",

                Address = new AddressModel

                {

                    City = "London",

                    PostalCode = "SW1"

                }

            }

        };



    public static CollectionModel Collection() =>

        new()

        {

            Items =

            [

                new SimpleModel { Id = 1, Name = "One", Active = true },

                new SimpleModel { Id = 2, Name = "Two", Active = false },

                new SimpleModel { Id = 3, Name = "Three", Active = true }

            ]

        };



    public static EnumModel Enum() =>

        new()

        {

            Status = ProcessingStatus.Ready

        };



    public static byte[] SimpleBytes() => SerializeBytes(Simple());



    public static byte[] NestedBytes() => SerializeBytes(Nested());



    public static byte[] CollectionBytes() => SerializeBytes(Collection());



    public static SimpleMessage SimpleMessage() =>

        new()

        {

            Id = 42,

            Name = "Ada",

            Active = true

        };



    public static OrderMessage OrderMessage() =>

        new()

        {

            Id = 73,

            Customer = new CustomerMessage

            {

                Name = "Ada",

                Address = new AddressMessage

                {

                    City = "London",

                    PostalCode = "SW1"

                }

            }

        };



    public static CollectionMessage CollectionMessage()

    {

        var message = new CollectionMessage();

        foreach (var item in Collection().Items)

        {

            message.Items.Add(new SimpleMessage

            {

                Id = item.Id,

                Name = item.Name,

                Active = item.Active

            });

        }



        return message;

    }



    public static EnumMessage EnumMessage() =>

        new()

        {

            Status = ProcessingStatusMessage.Ready

        };



    public static SimpleTable SimpleTable() =>

        new()

        {

            Id = 42,

            Name = "Ada",

            Active = true

        };



    public static OrderTable OrderTable() =>

        new()

        {

            Id = 73,

            Customer = new CustomerTable

            {

                Name = "Ada",

                Address = new AddressTable

                {

                    City = "London",

                    PostalCode = "SW1"

                }

            }

        };



    public static SimpleList SimpleList() =>

        new()

        {

            Items =

            [

                new SimpleTable { Id = 1, Name = "One", Active = true },

                new SimpleTable { Id = 2, Name = "Two", Active = false },

                new SimpleTable { Id = 3, Name = "Three", Active = true }

            ]

        };



    public static EnumTable EnumTable() =>

        new()

        {

            Status = (int)ProcessingStatus.Ready

        };



    public static byte[] SimpleTableBytes() =>

        WriteTable(SimpleTable(), Schemas.Flat.SimpleTable.Serializer);



    public static byte[] OrderTableBytes() =>

        WriteTable(OrderTable(), Schemas.Flat.OrderTable.Serializer);



    public static byte[] SimpleListBytes() =>

        WriteTable(SimpleList(), Schemas.Flat.SimpleList.Serializer);



    public static global::Avro.Schema SimpleSchema { get; } = global::Avro.Schema.Parse(

        """

        {"type":"record","name":"SimpleModel","namespace":"Matrix.BinarySerialization","fields":[{"name":"Id","type":"int"},{"name":"Name","type":"string"},{"name":"Active","type":"boolean"}]}

        """);



    public static global::Avro.Schema NestedSchema { get; } = global::Avro.Schema.Parse(

        """

        {"type":"record","name":"OrderModel","namespace":"Matrix.BinarySerialization","fields":[{"name":"Id","type":"int"},{"name":"Customer","type":{"type":"record","name":"CustomerModel","fields":[{"name":"Name","type":"string"},{"name":"Address","type":{"type":"record","name":"AddressModel","fields":[{"name":"City","type":"string"},{"name":"PostalCode","type":"string"}]}}]}}]}

        """);



    public static global::Avro.Schema CollectionSchema { get; } = global::Avro.Schema.Parse(

        """

        {"type":"record","name":"CollectionModel","namespace":"Matrix.BinarySerialization","fields":[{"name":"Items","type":{"type":"array","items":{"type":"record","name":"CollectionItem","fields":[{"name":"Id","type":"int"},{"name":"Name","type":"string"},{"name":"Active","type":"boolean"}]}}}]}

        """);



    public static global::Avro.Schema EnumSchema { get; } = global::Avro.Schema.Parse(

        """

        {"type":"record","name":"EnumModel","namespace":"Matrix.BinarySerialization","fields":[{"name":"Status","type":{"type":"enum","name":"ProcessingStatus","symbols":["Pending","Ready","Failed"]}}]}

        """);



    public static byte[] SimpleAvroBytes() => WriteAvro(Simple(), SimpleSchema);



    public static byte[] NestedAvroBytes() => WriteAvro(Nested(), NestedSchema);



    public static byte[] CollectionAvroBytes() => WriteAvro(Collection(), CollectionSchema);



    private static byte[] SerializeBytes<T>(T value)

    {

        using var stream = new MemoryStream(64);

        Serializer.Serialize(stream, value);

        return stream.ToArray();

    }



    private static byte[] WriteTable<T>(T value, ISerializer<T> serializer)

        where T : class

    {

        var buffer = new byte[serializer.GetMaxSize(value)];

        var written = serializer.Write(buffer, value);

        return buffer.AsSpan(0, written).ToArray();

    }



    private static byte[] WriteAvro<T>(T value, global::Avro.Schema schema)

    {

        using var stream = new MemoryStream(64);

        var writer = new global::Avro.Reflect.ReflectWriter<T>(

            schema,

            new global::Avro.Reflect.ClassCache());

        writer.Write(value, new global::Avro.IO.BinaryEncoder(stream));

        return stream.ToArray();

    }

}

