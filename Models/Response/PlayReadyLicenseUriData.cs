﻿using Newtonsoft.Json;
using SpotifyLibV2.Api;

namespace SpotifyLibV2.Models.Response
{
    /// <summary>
    /// The return body of <see cref="IPlaybackLicense.GetPlayReadyLicense(string)"/>
    /// </summary>
    public class PlayReadyLicenseUriData
    {
        /// <summary>
        /// The Unix time in MS that this license will expire
        /// </summary>
        [JsonProperty("expires")]
        public long ExpiresAt { get; set; }

        /// <summary>
        /// Tne URI to use to get the playback license
        /// </summary>
        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
}