# Unity Essentials

This module is part of the Unity Essentials ecosystem and follows the same lightweight, editor-first approach.
Unity Essentials is a lightweight, modular set of editor utilities and helpers that streamline Unity development. It focuses on clean, dependency-free tools that work well together.

All utilities are under the `UnityEssentials` namespace.

```csharp
using UnityEssentials;
```

## Installation

Install the Unity Essentials entry package via Unity's Package Manager, then install modules from the Tools menu.

- Add the entry package (via Git URL)
    - Window → Package Manager
    - "+" → "Add package from git URL…"
    - Paste: `https://github.com/CanTalat-Yakan/UnityEssentials.git`

- Install or update Unity Essentials packages
    - Tools → Install & Update UnityEssentials
    - Install all or select individual modules; run again anytime to update

---

#  ScriptableObject Drawer

> Quick overview: Inline‑edit ScriptableObject references directly in the Inspector. Assign an asset, expand the field, and edit its serialized properties without leaving the current inspector.

A small PropertyDrawer that renders a ScriptableObject reference like normal, and when expanded, draws the referenced asset’s Inspector inline. It hides the `m_Script` field and works with custom editors too.

![screenshot](Documentation/Screenshot.png)

## Features
- Inline Inspector for ScriptableObject references via a foldout
- Works with default and custom editors (uses Unity’s cached editor)
- Hides the `m_Script` field; shows only relevant serialized properties
- Auto‑detects when there are no visible properties and hides the foldout
- Falls back to default behavior for array/list elements
- Editor‑only; zero runtime overhead

## Requirements
- Unity Editor 6000.0+ (Editor‑only; no runtime code)
- No external dependencies

Tip: Create ScriptableObject assets via the Project window (e.g., Create → <Your Type>) and then assign them to the reference field to edit inline.

## Usage
Define a ScriptableObject and reference it from a MonoBehaviour

```csharp
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Game Config")]
public class GameConfig : ScriptableObject
{
    public int maxLives = 3;
    public float difficulty = 1.0f;
}

public class GameController : MonoBehaviour
{
    public GameConfig config; // Assign an asset, then expand to edit inline
}
```

Behavior with arrays/lists

```csharp
public class MultipleConfigs : MonoBehaviour
{
    public GameConfig[] configs; // Drawer uses default per‑element fields (no inline editors in arrays)
}
```

## How It Works
- The drawer targets `ScriptableObject` (and derived types)
- First draws the standard `ObjectField` for the reference
- If the reference is null → no foldout is shown
- If assigned, it creates (or reuses) a cached `Editor` for the referenced asset
- It scans the asset’s serialized properties; if none (besides `m_Script`), the foldout is hidden
- When expanded, it iterates the asset’s serialized properties and draws them (skipping `m_Script`), supporting nested/child properties
- Changes are applied to the referenced asset via `ApplyModifiedProperties`

## Notes and Limitations
- Arrays/lists: Inline editing is disabled for array elements; you’ll see regular object fields per element
- Very large or deeply nested assets can make the host inspector tall; collapse the foldout to save space
- Read‑only/custom logic from custom editors is respected (the drawer uses Unity’s standard editor for the target type)
- Inline editing doesn’t create assets; create them via the Project window or your own tooling

## Files in This Package
- `Editor/ScriptableObjectDrawer.cs` – PropertyDrawer for inline ScriptableObject editing (foldout + cached editor)
- `Editor/UnityEssentials.ScriptableObjectDrawer.Editor.asmdef` – Editor assembly definition

## Tags
unity, unity-editor, propertydrawer, scriptableobject, inline, inspector, editor, ui, tools, workflow
