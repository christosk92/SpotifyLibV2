﻿namespace SpotifyLibV2.Connect.Contexts
{
    public class GeneralInfiniteContext : AbsSpotifyContext
    {
        public GeneralInfiniteContext(string context) : base(context)
        {
        }

        public override bool IsFinite() => false;
    }
}
