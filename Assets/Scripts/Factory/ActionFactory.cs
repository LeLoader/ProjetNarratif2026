using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class ActionFactory
{
    private static readonly Dictionary<string, Type> _types = new Dictionary<string, Type>();

    #region Answer
    
    // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    // private static void RegisterAllActions()
    // {
    //     var baseType = typeof(ActionBase);
    //     var actionTypes = Assembly.GetAssembly(baseType).GetTypes().Where(t => t.IsSubclassOf(baseType) && !t.IsAbstract);
    //
    //     foreach (var at in actionTypes)
    //     {
    //         string value = at.Name;
    //         _types.TryAdd(value, at);
    //         Debug.Log($"[ACTION FACTORY] KEY ADDED {value}");
    //     }
    // }
    //
    // public static ActionBase RegisterAction(string key, GameObject toAttach)
    // {
    //     if (_types.TryGetValue(key, out var type))
    //     {
    //         return (ActionBase)toAttach.AddComponent(type);
    //     }
    //     
    //     Debug.LogError($"[ACTION FACTORY] KEY NOT FOUND FOR {key}");
    //     return null;
    // }
    
    #endregion

    
    #region Pinseel
    
   // ATTRIBUTES POUR APPELLER LA FONCTION AVANT LA GENERATION DE LA SCENE A METTRE AVANT CETTE FONCTION
   // [ATTRIBUTES ?]
   
    private static void RegisterAllActionsTest()
    {
        // DECLARER UNE VARIABLE QUI EST LA BASE SUPERCLASS //
        
        // LE PLUS DUR : RECUPERER TOUT LES SCRIPTS (en liste) DE ACTION BASE PRESENT DANS L'ASSEMBLY (Assembly.GetAssembly), ATTENTION ! NE RECUPERER QUE CEUX QUI EN DERIVENT ! EX : PAS RECUP LA CLASSSE "ACTION BASE" MAIS QUE CEUX QUI EN DERIVENT, ET EVITER CEUX QUI SONT ABSTRAITS //

        // FOR EACH, DE LA LISTE DE TOUT LES SCIRPTS DERIVENAT DE ACITON BASE // ON AJOUTE AU DICTIONNAIRE _types(); AVEC = <nom du script, script (qui est le type); AJOUTER UN  Debug.Log($"[ACTION FACTORY] KEY ADDED {value}");
    }

    public static ActionBase RegisterAction(string key, GameObject toAttach)
    {
        // VERIFIER DANS LE DICTIONNAIRE SI LA KEY EXISTE // ET SI OUI ON AJOUTE LE COMPOSANT AU GAMEOBJECT (DONC LE SCRIPT QUI HERITE DE ACTION BASE) ET ON LE RETOURNE //
        
        // SINON ON FAIT UN Debug.LogError POUR DIRE QUE LA KEY N'EXISTE PAS // Debug.LogError($"[ACTION FACTORY] KEY NOT FOUND FOR {key}");
        return null;
    }
    
    #endregion
    
}
