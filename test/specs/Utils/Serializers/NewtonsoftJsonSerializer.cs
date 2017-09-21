﻿using System.IO;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace TDL.Test.Specs.Utils.Serializers
{
    internal class NewtonsoftJsonSerializer : ISerializer
    {
        private readonly JsonSerializer serializer;

        public NewtonsoftJsonSerializer(JsonSerializer serializer)
        {
            this.serializer = serializer;
        }

        public string ContentType
        {
            get { return MimeType.Json; }
            set { }
        }

        public string DateFormat { get; set; }

        public string Namespace { get; set; }

        public string RootElement { get; set; }

        public string Serialize(object obj)
        {
            using (var stringWriter = new StringWriter())
            using (var jsonTextWriter = new JsonTextWriter(stringWriter))
            {
                serializer.Serialize(jsonTextWriter, obj);
                return stringWriter.ToString();
            }
        }

        public T Deserialize<T>(IRestResponse response)
        {
            var content = response.Content;

            using (var stringReader = new StringReader(content))
            using (var jsonTextReader = new JsonTextReader(stringReader))
            {
                return serializer.Deserialize<T>(jsonTextReader);
            }
        }

        public static NewtonsoftJsonSerializer Default =>
            new NewtonsoftJsonSerializer(new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore,
            });
    }
}
