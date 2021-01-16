﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpotifyLibV2.Models.Response;

namespace SpotifyLibV2.Helpers.Converters
{
    public class PlayableItemConverter : JsonConverter
    {
        public override bool CanWrite { get => false; }

        public override bool CanConvert(Type objectType) => true;

        public override object? ReadJson(JsonReader reader, Type objectType,
            object? existingValue, JsonSerializer serializer)
        {

            var token = JToken.ReadFrom(reader);
            if (token.Type == JTokenType.Null)
            {
                return null;
            }

            var type = token["type"]?.Value<string>();
            if (type == "track")
            {
                var obj = new FullTrack();
                serializer.Populate(token.CreateReader(), obj);
                return obj;
            }
            else if (type == "episode")
            {
                var obj = new FullEpisode();
                serializer.Populate(token.CreateReader(), obj);
                return obj;
            }
            else
            {
                throw new Exception($@"Received unkown playlist element type: {type}.
If you're requesting a subset of available fields via the fields query paramter,
make sure to include at least the type field. Often it's `items(track(type))` or `item(type)`");
            }
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }
    }
}
