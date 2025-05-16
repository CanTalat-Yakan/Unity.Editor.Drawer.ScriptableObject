# Unity Essentials

**Unity Essentials** is a lightweight, modular utility namespace designed to streamline development in Unity. 
It provides a collection of foundational tools, extensions, and helpers to enhance productivity and maintain clean code architecture.

## 📦 This Package

This package is part of the **Unity Essentials** ecosystem.  
It integrates seamlessly with other Unity Essentials modules and follows the same lightweight, dependency-free philosophy.

## 🌐 Namespace

All utilities are under the `UnityEssentials` namespace. This keeps your project clean, consistent, and conflict-free.

```csharp
using UnityEssentials;
```

# ScriptableObjectDrawer  
Custom property drawer for Unity's `ScriptableObject` fields in the editor, enabling foldout display of the object's inspector inline.

## Usage Examples
- Automatically applies to any `ScriptableObject` field in Unity Editor due to `[CustomPropertyDrawer(typeof(ScriptableObject), true)]`.  
- Displays the object reference field with a foldout arrow to expand and show the full inspector of the referenced ScriptableObject inline.  
- Collapses foldout if the reference is null, preventing unnecessary UI clutter.  
- Creates and caches a dedicated editor instance for the referenced ScriptableObject to render its inspector inside the foldout.  
- Supports nested `ScriptableObject` editors inside property drawers, improving editor usability for complex serialized data.