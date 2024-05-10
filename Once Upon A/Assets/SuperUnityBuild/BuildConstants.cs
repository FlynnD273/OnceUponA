using System;

// This file is auto-generated. Do not modify or move this file.

namespace SuperUnityBuild.Generated
{
    public enum ReleaseType
    {
        None,
        Release,
    }

    public enum Platform
    {
        None,
        Linux,
        PC,
    }

    public enum ScriptingBackend
    {
        None,
        Mono,
    }

    public enum Architecture
    {
        None,
        Linux_x64,
        Windows_x86,
        Windows_x64,
    }

    public enum Distribution
    {
        None,
    }

    public static class BuildConstants
    {
        public static readonly DateTime buildDate = new DateTime(638508806314264460);
        public const string version = "1.0.0.1";
        public const ReleaseType releaseType = ReleaseType.Release;
        public const Platform platform = Platform.PC;
        public const ScriptingBackend scriptingBackend = ScriptingBackend.Mono;
        public const Architecture architecture = Architecture.Windows_x64;
        public const Distribution distribution = Distribution.None;
    }
}

