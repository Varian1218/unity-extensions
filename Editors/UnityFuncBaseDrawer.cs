#if UNITY_EDITOR
#if USE_UNITY_FUNC_BASE_DRAWER
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityExtensions.Editors
{
    [CustomPropertyDrawer(typeof(UnityFuncBase), true)]
    public class UnityFuncBaseDrawer : PropertyDrawer
    {
        private static Type[] GetArgTypes(SerializedProperty argsProp)
        {
            return Array.Empty<Type>();
            // Type[] types = new Type[argsProp.arraySize];
            // for (int i = 0; i < argsProp.arraySize; i++)
            // {
            //     SerializedProperty argProp = argsProp.GetArrayElementAtIndex(i);
            //     SerializedProperty typeNameProp = argProp.FindPropertyRelative("_typeName");
            //     if (typeNameProp != null) types[i] = Type.GetType(typeNameProp.stringValue, false);
            //     if (types[i] == null)
            //         types[i] = Arg.RealType((Arg.ArgType)argProp.FindPropertyRelative("argType").enumValueIndex);
            // }
            //
            // return types;
        }

        private static string ToString(Type type)
        {
            return type.Name;
        }

        private static MethodInfo GetMethod(object target, string methodName, Type[] types)
        {
            return target.GetType().GetMethod(
                methodName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static,
                null,
                CallingConventions.Any,
                types,
                null
            );
        }

        private static Object[] GetObjects(Object obj)
        {
            return obj switch
            {
                Component component => component.GetComponents<Component>(),
                GameObject gameObject => gameObject.GetComponents<Component>(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static (List<string>, List<(string, Object, MethodInfo)>) GetMethods(Type[] args, Object[] objects)
        {
            var dynamicMethods = new List<string>();
            var staticMethods = new List<(string, Object, MethodInfo)>();
            foreach (var it in objects)
            {
                var type = it.GetType();
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                foreach (var methodInfo in methods)
                {
                    if (methodInfo.ReturnType != args[^1]) continue;
                    if (methodInfo.IsGenericMethod) continue;
                    var parameters = methodInfo.GetParameters().Select(x => x.ParameterType).ToArray();
                    if (parameters.Length > 0) continue;
                    // if (parameters.Any(x => !Arg.IsSupported(x))) continue;
                    var declaringType = methodInfo.DeclaringType ?? throw new NullReferenceException();
                    staticMethods.Add((
                        $"{type.Name}/{declaringType.Name}",
                        it,
                        methodInfo
                    ));
                }
            }
            return (dynamicMethods, staticMethods);
        }

        // private void MethodSelector(SerializedProperty property)
        // {
        //     Type returnType = null;
        //     var argTypes = new Type[0];
        //     SerializableCallbackBase dummy = GetDummyFunction(property);
        //     Type[] genericTypes = dummy.GetType().BaseType.GetGenericArguments();
        //     // SerializableEventBase is always void return type
        //     if (dummy is SerializableEventBase)
        //     {
        //         returnType = typeof(void);
        //         if (genericTypes.Length > 0)
        //         {
        //             argTypes = new Type[genericTypes.Length];
        //             Array.Copy(genericTypes, argTypes, genericTypes.Length);
        //         }
        //     }
        //     else
        //     {
        //         if (genericTypes != null && genericTypes.Length > 0)
        //         {
        //             // The last generic argument is the return type
        //             returnType = genericTypes[genericTypes.Length - 1];
        //             if (genericTypes.Length > 1)
        //             {
        //                 argTypes = new Type[genericTypes.Length - 1];
        //                 Array.Copy(genericTypes, argTypes, genericTypes.Length - 1);
        //             }
        //         }
        //     }
        //
        //     var targetProp = property.FindPropertyRelative("_target");
        //
        //     var dynamicItems = new List<MenuItem>();
        //     var staticItems = new List<MenuItem>();
        //
        //     var targets = new List<Object> { targetProp.objectReferenceValue };
        //
        //     switch (targets[0])
        //     {
        //         case Component:
        //             targets = (targets[0] as Component).gameObject.GetComponents<Component>().ToList<Object>();
        //             targets.Add((targetProp.objectReferenceValue as Component).gameObject);
        //             break;
        //         case GameObject:
        //             targets = (targets[0] as GameObject).GetComponents<Component>().ToList<Object>();
        //             targets.Add(targetProp.objectReferenceValue as GameObject);
        //             break;
        //     }
        //
        //     foreach (var it in targets)
        //     {
        //         var methods = it.GetType()
        //             .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        //
        //         foreach (var method in methods)
        //         {
        //             if (returnType != null && method.ReturnType != returnType) continue;
        //             if (method.IsGenericMethod) continue;
        //             var parameters = method.GetParameters().Select(x => x.ParameterType).ToArray();
        //             if (parameters.Length > 4) continue;
        //             // if (parameters.Any(x => !Arg.IsSupported(x))) continue;
        //             string methodPrettyName = PrettifyMethod(method);
        //             staticItems.Add(new MenuItem(
        //                 it.GetType().Name + "/" + method.DeclaringType.Name,
        //                 methodPrettyName,
        //                 () => SetMethod(property, t1, method, false)
        //             ));
        //             if (argTypes.Length == 0 || !Enumerable.SequenceEqual(argTypes, parameters)) continue;
        //             dynamicItems.Add(new MenuItem(
        //                 it.GetType().Name + "/" + method.DeclaringType.Name,
        //                 method.Name,
        //                 () => SetMethod(property, method, method, true)
        //             ));
        //         }
        //     }
        //
        //     // Construct and display context menu
        //     var menu = new GenericMenu();
        //     // if (dynamicItems.Count > 0)
        //     // {
        //     //     string[] paths = dynamicItems.GroupBy(x => x.path).Select(x => x.First().path).ToArray();
        //     //     foreach (var path in paths)
        //     //     {
        //     //         menu.AddItem(new GUIContent(path + "/Dynamic " + PrettifyTypes(argTypes)), false, null);
        //     //     }
        //     //
        //     //     for (var i = 0; i < dynamicItems.Count; i++)
        //     //     {
        //     //         menu.AddItem(dynamicItems[i].label, false, dynamicItems[i].action);
        //     //     }
        //     //
        //     //     foreach (var path in paths)
        //     //     {
        //     //         menu.AddItem(new GUIContent(path + "/  "), false, null);
        //     //         menu.AddItem(new GUIContent(path + "/Static parameters"), false, null);
        //     //     }
        //     // }
        //
        //     for (var i = 0; i < staticItems.Count; i++)
        //     {
        //         menu.AddItem(staticItems[i].label, false, staticItems[i].action);
        //     }
        //
        //     if (menu.GetItemCount() == 0)
        //     {
        //         menu.AddDisabledItem(new GUIContent("No methods with return type '" + GetTypeName(returnType) + "'"));   
        //     }
        //     menu.ShowAsContext();
        // }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.serializedObject.ApplyModifiedProperties();
            label.text = $" {label.text}";

#if UNITY_2019_1_OR_NEWER
            GUI.Box(position, "");
#else
		GUI.Box(position, "", (GUIStyle)
			"flow overlay box");
#endif
            position.y += 4;
            property.serializedObject.Update();
            EditorGUI.BeginProperty(position, label, property);
            var pos = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            var targetRect = new Rect(pos.x, pos.y, pos.width, EditorGUIUtility.singleLineHeight);
            var targetProp = property.FindPropertyRelative("target");
            EditorGUI.PropertyField(targetRect, targetProp, GUIContent.none);
            var target = targetProp.objectReferenceValue;
            switch (target)
            {
                case null:
                {
                    var helpBoxRect = new Rect(
                        position.x + 8,
                        targetRect.max.y + EditorGUIUtility.standardVerticalSpacing,
                        position.width - 16, EditorGUIUtility.singleLineHeight
                    );
                    const string msg = "Call not set. Execution will be slower.";
                    EditorGUI.HelpBox(helpBoxRect, msg, MessageType.Warning);
                    break;
                }
                case MonoScript:
                {
                    var helpBoxRect = new Rect(
                        position.x + 8,
                        targetRect.max.y + EditorGUIUtility.standardVerticalSpacing,
                        position.width - 16,
                        EditorGUIUtility.singleLineHeight + EditorGUIUtility.singleLineHeight +
                        EditorGUIUtility.standardVerticalSpacing
                    );
                    const string msg = "Assign a GameObject, Component or a ScriptableObject, not a script.";
                    EditorGUI.HelpBox(helpBoxRect, msg, MessageType.Warning);
                    break;
                }
                default:
                {
                    var indent = EditorGUI.indentLevel;
                    EditorGUI.indentLevel++;
                    var methodProp = property.FindPropertyRelative("methodName");
                    var methodName = methodProp.stringValue;
                    var argProps = property.FindPropertyRelative("args");
                    var argTypes = GetArgTypes(argProps);
                    var dynamicProp = property.FindPropertyRelative("dynamic");
                    var dynamic = dynamicProp.boolValue;
                    var activeMethod = GetMethod(target, methodName, argTypes);
                    var methodLabel = activeMethod != null ? new GUIContent(PrettifyMethod(activeMethod)) :
                        string.IsNullOrEmpty(methodName) ? new GUIContent("n/a") :
                        new GUIContent($"Missing ({methodName}({string.Join(", ", argTypes.Select(ToString))}))");
                    var methodRect = new Rect(
                        position.x,
                        targetRect.max.y + EditorGUIUtility.standardVerticalSpacing,
                        position.width,
                        EditorGUIUtility.singleLineHeight
                    );
                    pos = EditorGUI.PrefixLabel(
                        methodRect,
                        GUIUtility.GetControlID(FocusType.Passive),
                        new GUIContent(dynamic ? "Method (dynamic)" : "Method")
                    );
                    if (EditorGUI.DropdownButton(pos, methodLabel, FocusType.Keyboard))
                    {
                        var args = Type.GetType(property.FindPropertyRelative("typeName").stringValue, true).GetGenericArguments();
                        var (dynamicMethods, staticMethods) = GetMethods(args, GetObjects(target));
                        var menu = new GenericMenu();
                        // if (dynamicMethods.Count > 0)
                        // {
                        //     string[] paths = dynamicMethods.GroupBy(x => x.path).Select(x => x.First().path).ToArray();
                        //     foreach (var path in paths)
                        //     {
                        //         menu.AddItem(new GUIContent(path + "/Dynamic " + PrettifyTypes(argTypes)), false, null);
                        //     }
                        //
                        //     for (var i = 0; i < dynamicItems.Count; i++)
                        //     {
                        //         menu.AddItem(dynamicItems[i].label, false, dynamicItems[i].action);
                        //     }
                        //
                        //     foreach (var path in paths)
                        //     {
                        //         menu.AddItem(new GUIContent(path + "/  "), false, null);
                        //         menu.AddItem(new GUIContent(path + "/Static parameters"), false, null);
                        //     }
                        // }
                    
                        foreach (var (path, t, methodInfo) in staticMethods)
                        {
                            menu.AddItem(
                                new GUIContent($"{path}/{PrettifyMethod(methodInfo)}"),
                                false,
                                () => SetMethod(property, t, methodInfo, false)
                            );
                        }
                    
                        if (menu.GetItemCount() == 0)
                        {
                            menu.AddDisabledItem(new GUIContent("No methods with return type '" + ToString(args[^1]) + "'"));   
                        }
                        menu.ShowAsContext();
                    }

                    // if (activeMethod != null && !dynamic) {
                    // 	// Args
                    // 	ParameterInfo[] activeParameters = activeMethod.GetParameters();
                    // 	Rect argRect = new Rect(position.x, methodRect.max.y + EditorGUIUtility.standardVerticalSpacing, position.width, EditorGUIUtility.singleLineHeight);
                    // 	string[] types = new string[argProps.arraySize];
                    // 	for (int i = 0; i < types.Length; i++) {
                    // 		SerializedProperty argProp = argProps.FindPropertyRelative("Array.data[" + i + "]");
                    // 		GUIContent argLabel = new GUIContent(ObjectNames.NicifyVariableName(activeParameters[i].Name));
                    // 	
                    // 		EditorGUI.BeginChangeCheck();
                    // 		switch ((Arg.ArgType) argProp.FindPropertyRelative("argType").enumValueIndex) {
                    // 			case Arg.ArgType.Bool:
                    // 				EditorGUI.PropertyField(argRect, argProp.FindPropertyRelative("boolValue"), argLabel);
                    // 				break;
                    // 			case Arg.ArgType.Int:
                    // 				EditorGUI.PropertyField(argRect, argProp.FindPropertyRelative("intValue"), argLabel);
                    // 				break;
                    // 			case Arg.ArgType.Float:
                    // 				EditorGUI.PropertyField(argRect, argProp.FindPropertyRelative("floatValue"), argLabel);
                    // 				break;
                    // 			case Arg.ArgType.String:
                    // 				EditorGUI.PropertyField(argRect, argProp.FindPropertyRelative("stringValue"), argLabel);
                    // 				break;
                    // 			case Arg.ArgType.Object:
                    // 				SerializedProperty typeProp = argProp.FindPropertyRelative("_typeName");
                    // 				SerializedProperty objProp = argProp.FindPropertyRelative("objectValue");
                    // 		
                    // 				if (typeProp != null) {
                    // 					Type objType = Type.GetType(typeProp.stringValue, false);
                    // 					EditorGUI.BeginChangeCheck();
                    // 					Object obj = objProp.objectReferenceValue;
                    // 					obj = EditorGUI.ObjectField(argRect, argLabel, obj, objType, true);
                    // 					if (EditorGUI.EndChangeCheck()) {
                    // 						objProp.objectReferenceValue = obj;
                    // 					}
                    // 				} else {
                    // 					EditorGUI.PropertyField(argRect, objProp, argLabel);
                    // 				}
                    // 				break;
                    // 		}
                    // 		if (EditorGUI.EndChangeCheck()) {
                    // 			property.FindPropertyRelative("dirty").boolValue = true;
                    // 		}
                    // 		argRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    // 	}
                    // }
                    EditorGUI.indentLevel = indent;
                    break;
                }
            }
            EditorGUI.EndProperty();
            property.serializedObject.ApplyModifiedProperties();
        }

        private static string PrettifyMethod(MethodInfo methodInfo)
        {
            var returnParameter = methodInfo.ReturnParameter ?? throw new NullReferenceException();
            var parameters = string.Join(", ", methodInfo.GetParameters().Select(it => ToString(it.ParameterType)));
            return $"{ToString(returnParameter.ParameterType)} {methodInfo.Name}({parameters})";
        }

        private string PrettifyTypes(Type[] types) {
            return types == null ? throw new ArgumentNullException() : string.Join(", ", types.Select(ToString).ToArray());
        }
        
        private static void SetMethod(SerializedProperty property, Object target, MethodInfo methodInfo, bool dynamic) {
            var targetProp = property.FindPropertyRelative("target");
            targetProp.objectReferenceValue = target;
            var methodProp = property.FindPropertyRelative("methodName");
            methodProp.stringValue = methodInfo.Name;
            var dynamicProp = property.FindPropertyRelative("dynamic");
            dynamicProp.boolValue = dynamic;
            // var argProp = property.FindPropertyRelative("args");
            // var parameters = methodInfo.GetParameters();
            // argProp.arraySize = parameters.Length;
            // for (var i = 0; i < parameters.Length; i++) {
            //     argProp.FindPropertyRelative("Array.data[" + i + "].argType").enumValueIndex = (int) Arg.FromRealType(parameters[i].ParameterType);
            //     argProp.FindPropertyRelative("Array.data[" + i + "]._typeName").stringValue = parameters[i].ParameterType.AssemblyQualifiedName;
            // }
            property.FindPropertyRelative("dirty").boolValue = true;
            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            var lineHeight = EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight;
            var targetProp = property.FindPropertyRelative("target");
            var argProps = property.FindPropertyRelative("args");
            var dynamicProp = property.FindPropertyRelative("dynamic");
            var height = lineHeight + lineHeight;
            if (targetProp.objectReferenceValue)
            {
                if (targetProp.objectReferenceValue is MonoScript) 
                {
                    height += lineHeight;
                }
                // else if (!dynamicProp.boolValue)
                // {
                //     height += argProps.arraySize * lineHeight;
                // }
            }
            height += 8;
            return height;
        }
    }
}
#endif
#endif