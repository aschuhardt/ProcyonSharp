using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ProcyonSharp.Attributes;
using ProcyonSharp.Bindings;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ProcyonSharp.Input.Serialization;

public static class InputFileHelper
{
    private const string InputMapFilename = "input.yaml";
    private const string AvailableKeysFilename = "input_keys.txt";

    private static void WriteAvailableInputKeysFile()
    {
        using var output = File.CreateText(AvailableKeysFilename);
        output.WriteLine("Below are a list of all valid key names for use in input.yaml:");
        output.WriteLine();

        foreach (var key in Enum.GetNames(typeof(Key)))
            output.WriteLine(key);
    }

    public static IDictionary<T, GameStateInputFunctionKeyMap> Load<T>()
        where T : Enum
    {
        if (!File.Exists(InputMapFilename))
            SaveDefault<T>();

        var configuredInputStateGroups = DeserializeInputMapFile<T>();
        var stateTypesByStateValue = GetStateTypesByStateValue<T>();

        var inputStates = new Dictionary<T, GameStateInputFunctionKeyMap>();
        foreach (var configuredInputStateGroup in configuredInputStateGroups)
        {
            var stateValue = configuredInputStateGroup.State;

            if (stateValue == null || !stateTypesByStateValue.ContainsKey(stateValue) ||
                configuredInputStateGroup.Functions == null)
                continue;

            var stateType = stateTypesByStateValue[stateValue];
            var inputFunctionsByName = stateType.GetMethods()
                .Where(method => method.GetCustomAttributes<InputAttribute>().Any())
                .ToDictionary(method => method.Name);

            inputStates.Add(stateValue, BuildInputState(configuredInputStateGroup, inputFunctionsByName));
        }

        return inputStates;
    }

    private static GameStateInputFunctionKeyMap BuildInputState<T>(
        SerializedGameStateInput<T> configuredInputSerializedGameState,
        IReadOnlyDictionary<string, MethodInfo> inputFunctionsByName) where T : Enum
    {
        var keyMap = new GameStateInputFunctionKeyMap();

        if (configuredInputSerializedGameState.Functions == null)
            return keyMap;

        foreach (var configuredInputFunction in configuredInputSerializedGameState.Functions)
        {
            if (string.IsNullOrEmpty(configuredInputFunction.FunctionName))
                continue;

            if (!inputFunctionsByName.ContainsKey(configuredInputFunction.FunctionName))
            {
                Console.WriteLine(
                    $"Skipping unknown input function name \"{configuredInputFunction.FunctionName}\" found in {InputMapFilename}");
                continue;
            }

            var inputFunc = inputFunctionsByName[configuredInputFunction.FunctionName];
            configuredInputFunction.FunctionCall =
                inputFunc.CreateDelegate(typeof(Action<>).MakeGenericType(inputFunc.DeclaringType));

            if (!keyMap.Functions.ContainsKey(configuredInputFunction.Key))
                keyMap.Functions.Add(configuredInputFunction.Key, new List<MappedFunctionCall>());

            keyMap.Functions[configuredInputFunction.Key].Add(configuredInputFunction);
        }

        return keyMap;
    }

    private static void SaveDefault<T>() where T : Enum
    {
        var gameStateTypes = GetStateTypesByStateValue<T>();
        var defaultInputConfiguration = new List<SerializedGameStateInput<T>>(gameStateTypes.Count);
        foreach (var stateType in gameStateTypes)
        {
            var mappedFunctions = new List<MappedFunctionCall>();
            foreach (var method in stateType.Value.GetMethods()
                         .Where(m => m.GetCustomAttributes<InputAttribute>().Any()))
            foreach (var inputFuncDesc in method.GetCustomAttributes<InputAttribute>())
            {
                var mod = inputFuncDesc.DefaultModifier;
                mappedFunctions.Add(new MappedFunctionCall
                {
                    FunctionName = method.Name,
                    Key = inputFuncDesc.DefaultKey,
                    Alt = mod.Alt,
                    Ctrl = mod.Control,
                    Shift = mod.Shift
                });
            }

            defaultInputConfiguration.Add(new SerializedGameStateInput<T>
            {
                Functions = mappedFunctions.ToArray(),
                State = stateType.Key
            });
        }

        using var outputFile = File.Create(InputMapFilename);
        using var outputWriter = new StreamWriter(outputFile);
        new SerializerBuilder()
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitDefaults)
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build()
            .Serialize(outputWriter, defaultInputConfiguration.ToArray());

        WriteAvailableInputKeysFile();
    }

    private static IEnumerable<SerializedGameStateInput<T>> DeserializeInputMapFile<T>() where T : Enum
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();

        var modeInputGroups =
            deserializer.Deserialize<SerializedGameStateInput<T>[]>(File.ReadAllText(InputMapFilename));
        return modeInputGroups;
    }

    private static IReadOnlyDictionary<T, Type> GetStateTypesByStateValue<T>() where T : Enum
    {
        var inputClassByStateValue = Assembly.GetEntryAssembly()!
            .GetTypes()
            .Where(t => t.GetCustomAttribute<StateAttribute>() != null)
            .Select(t => new { Type = t, Attr = t.GetCustomAttribute<StateAttribute>() })
            .Where(t => t.Attr?.StateEnumValue is T)
            .Select(t =>
                new { t.Type, t.Attr.StateEnumValue })
            .ToDictionary(t => (T)t.StateEnumValue, t => t.Type);
        return inputClassByStateValue;
    }
}