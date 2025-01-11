using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using ProMod.Stats;

namespace ProMod.HUD;

public interface IProHUDElement
{

    public void Initialize(RectTransform rectTransform);

    public void OnStatUpdate(ProStats proStats);

    public void OnStatReady(ProStats proStats);
    public bool Enabled { get; }

    public Vector2Int Size { get; }
}

public class ProHUD
{

    public enum ElementType
    {
        Normal,
        NewLine,
        VerticalSeparator,
        HorizontalSeparator,
        Invalid
    }

    private class ElementData
    {
        public ProHUDElementAttribute attribute;
        public Type type;
    }

    private static Dictionary<string, ElementData> registeredElements = new Dictionary<string, ElementData>();

    public static bool ElementExists(string elementName)
    {
        return registeredElements.ContainsKey(elementName);
    }

    public static bool ElementHasInterface(string elementName, Type interfaceType)
    {
        if(!registeredElements.ContainsKey(elementName)) {  return false; }
        return registeredElements[elementName].type.GetInterface(interfaceType.Name) != null;

    }

    internal static T CreateElement<T>(string elementName) where T : class
    {
        if (registeredElements.ContainsKey(elementName))
        {

            Plugin.Log.Info($"Creating HUD Element: {elementName}");
            return (T)Activator.CreateInstance(registeredElements[elementName].type);
        } else
        {
            return null;
        }
    }

    public static ElementType GetElementType(string elementName)
    {
        if (!registeredElements.ContainsKey(elementName)) { return ElementType.Invalid; }
        return registeredElements[elementName].attribute.elementType;

    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void RegisterElements()
    {
        foreach(Type type in Assembly.GetCallingAssembly().GetTypes())
        {
            ProHUDElementAttribute proHUDElement = type.GetCustomAttribute<ProHUDElementAttribute>();

            if (proHUDElement == null) { continue; }

            if (type.GetInterface(nameof(IProHUDElement)) == null)
            {
                Plugin.Log.Error($"Error: HUD Element with name [{proHUDElement.name}] doesn't implement IProHUDElement.");
                continue;
            }

            if (proHUDElement.name == "")
            {
                proHUDElement.name = type.Name;
            }

            if (registeredElements.ContainsKey(proHUDElement.name))
            {
                Plugin.Log.Error($"Error: HUD Element with name [{proHUDElement.name}] already exists.");
                continue;
            }

            registeredElements[proHUDElement.name] = new ElementData {
                attribute = proHUDElement,
                type = type
            };

            //Plugin.Log.Info($"Registered HUD Element with name [{proHUDElement.name}] to type [{type.FullName}].");
        }
    }
}



[AttributeUsage(AttributeTargets.Class)]
public class ProHUDElementAttribute : Attribute
{
    
    public string name;
    public int width;
    public int height;
    public ProHUD.ElementType elementType;

    public ProHUDElementAttribute(string name, int width = 0, int height = 20, ProHUD.ElementType elementType = ProHUD.ElementType.Normal)
    {
        this.name = name;
        this.width = width;
        this.height = height;
        this.elementType = elementType;
    }

    public ProHUDElementAttribute(string name, ProHUD.ElementType elementType)
    {
        this.name = name;
        this.width = 0;
        this.height = 0;
        this.elementType = elementType;
    }
}
