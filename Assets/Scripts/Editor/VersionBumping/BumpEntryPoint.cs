using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build.Reporting;

namespace Editor.VersionBumping
{
    public static class BumpEntryPoint
    {
        private static readonly string Eol = Environment.NewLine;

        public static void Bump()
        {
            var arguments = GetValidatedOptions();
            var version = arguments["-version"];
            switch (version)
            {
                case "major":
                    VersionBumpingUtils.BumpMajorVersion();
                    break;
                case "minor":
                    VersionBumpingUtils.BumpMinorVersion();
                    break;
                case "patch":
                    VersionBumpingUtils.BumpPatchVersion();
                    break;
                default:
                    throw new ArgumentException(
                        $"-version argument should be one of: mayor, minor or patch, it was {version}"
                        );
            }
            
            VersionBumpingUtils.UpdateHashAndTimeStamp();
            
            Console.WriteLine($"{version} version bump succeeded!");
            EditorApplication.Exit(0);
        }

        private static Dictionary<string, string> GetValidatedOptions()
        {
            ParseCommandLineArguments(out Dictionary<string, string> validatedOptions);

            if (!validatedOptions.ContainsKey("version"))
            {
                Console.WriteLine("Missing argument -version");
                EditorApplication.Exit(110);
            }

            return validatedOptions;
        }

        private static void ParseCommandLineArguments(out Dictionary<string, string> providedArguments)
        {
            providedArguments = new Dictionary<string, string>();
            string[] args = Environment.GetCommandLineArgs();

            Console.WriteLine(
                $"{Eol}" +
                $"###########################{Eol}" +
                $"#    Parsing settings     #{Eol}" +
                $"###########################{Eol}" +
                $"{Eol}"
            );

            // Extract flags with optional values
            for (int current = 0, next = 1; current < args.Length; current++, next++)
            {
                // Parse flag
                bool isFlag = args[current].StartsWith("-");
                if (!isFlag) continue;
                string flag = args[current].TrimStart('-');

                // Parse optional value
                bool flagHasValue = next < args.Length && !args[next].StartsWith("-");
                string value = flagHasValue ? args[next].TrimStart('-') : "";
                string displayValue = $"\"{value}\"";

                // Assign
                Console.WriteLine($"Found flag \"{flag}\" with value {displayValue}.");
                providedArguments.Add(flag, value);
            }
        }
    }
}