using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;

public static class ActionFactory
{
    private static readonly Dictionary<string, Type> _types = new Dictionary<string, Type>();

    #region Answer

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void RegisterAllActions()
    {
        var baseType = typeof(ActionBase);
        var actionTypes = Assembly.GetAssembly(baseType).GetTypes().Where(t => t.IsSubclassOf(baseType) && !t.IsAbstract);
            foreach (var at in actionTypes)
        {
            string value = at.Name;
            _types.TryAdd(value, at);
            Debug.Log($"[ACTION FACTORY] KEY ADDED {value}");
        }
    }
        public static ActionBase CreateAction(string key, GameObject toAttach)
    {
        if (_types.TryGetValue(key, out var type))
        {
            return (ActionBase)toAttach.AddComponent(type);
        }
        
        Debug.LogError($"[ACTION FACTORY] KEY NOT FOUND FOR {key}");
        return null;
    }

    #endregion


    #region Pinseel

    // ATTRIBUTES POUR APPELLER LA FONCTION AVANT LA GENERATION DE LA SCENE A METTRE AVANT CETTE FONCTION
    // [ATTRIBUTES ?]

    /*[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void RegisterAllActionsTest()
    {
        // DECLARER UNE VARIABLE QUI EST LA BASE SUPERCLASS //
        Type TypeToGet = typeof(ActionBase);



        // LE PLUS DUR : RECUPERER TOUT LES SCRIPTS (en liste) DE ACTION BASE PRESENT DANS L'ASSEMBLY (Assembly.GetAssembly), ATTENTION ! NE RECUPERER QUE CEUX QUI EN DERIVENT ! EX : PAS RECUP LA CLASSSE "ACTION BASE" MAIS QUE CEUX QUI EN DERIVENT, ET EVITER CEUX QUI SONT ABSTRAITS //
        var SubclassTypes = Assembly.GetAssembly(TypeToGet).GetTypes().Where(t => t.IsSubclassOf(typeof(ActionBase)) && !t.IsAbstract);

        // FOR EACH, DE LA LISTE DE TOUT LES SCRIPTS DERIVANT DE ACTION BASE // ON AJOUTE AU DICTIONNAIRE _types(); AVEC = <nom du script, script (qui est le type); AJOUTER UN  Debug.Log($"[ACTION FACTORY] KEY ADDED {value}");
        foreach (var SubclassType in SubclassTypes)
        {
            _types.TryAdd(SubclassType.Name, SubclassType);
            Debug.Log($"[ACTION FACTORY] KEY ADDED {SubclassType.Name}");
        }
    }

    public static ActionBase CreateAction(string key, GameObject toAttach)
    {
        // VERIFIER DANS LE DICTIONNAIRE SI LA KEY EXISTE
        if (_types.ContainsKey(key))
        {
            // ET SI OUI ON AJOUTE LE COMPOSANT AU GAMEOBJECT (DONC LE SCRIPT QUI HERITE DE ACTION BASE) ET ON LE RETOURNE //
            ActionBase AttachedComponent = (ActionBase)toAttach.AddComponent(_types[key]);
            return AttachedComponent;
        } else
        {
            // SINON ON FAIT UN Debug.LogError POUR DIRE QUE LA KEY N'EXISTE PAS // Debug.LogError($"[ACTION FACTORY] KEY NOT FOUND FOR {key}");
            Debug.LogError($"[ACTION FACTORY] key {key} not found ");
            return null;
        }

    }*/
    
    #endregion
    
}
