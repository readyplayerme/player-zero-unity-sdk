# 🧩 Player Zero Unity SDK

Integrate Player Zero avatars and analytics into your Unity game with just a few lines of code. This SDK lets you load and customize avatars, transfer meshes onto existing characters, and track in-game events seamlessly.

📚 **Full Docs:** [Unity Integration Documentation →](https://app.gitbook.com/o/-MUPNxqiv9WwarP92bMF/s/gzrFZv4JI6y2EfhkJwzN/integrations/unity-integration)

> ⚠️ **Note:** Unity builds with Player Zero require **stripping level set to 'Low'**. 'Medium' and 'High' are not supported yet.

## 🎥 Quick Start Video

Prefer watching? Here's a [short video walkthrough](https://www.youtube.com/watch?v=04-SCWpN3K0&embeds_referring_euri=https%3A%2F%2Fcdn.iframe.ly%2F&source_ve_path=MjM4NTE) to get started in under 5 minutes.

## 🚀 Prerequisites

- You've connected with a Player Zero team member.
- Your developer account is set up, and credentials are provided.
- If using a custom rig, your base FBX should have been shared and registered.

## 📦 Installation

1. Visit the [Unity SDK GitHub Releases](https://github.com/readyplayerme/player-zero-unity-sdk/releases) page.
2. Download the latest `.unitypackage` or add it via the Unity Package Manager.

## 🔐 Login

- Go to `Tools → Player Zero` in Unity.
- Use the credentials provided by your account manager to log in.

## 🧬 Load Avatars

### Basic Usage

```csharp
var avatar = await PlayerZeroSdk.InstantiateAvatarAsync(new CharacterRequestConfig {
    AvatarId = "AVATAR_ID_HERE"
});
```

### Load With Blueprint

```csharp
var avatar = await PlayerZeroSdk.InstantiateAvatarAsync(new CharacterRequestConfig {
    AvatarId = "AVATAR_ID_HERE",
    BlueprintId = "BLUEPRINT_ID_HERE"
});
```

### Hot Load Example

```csharp
var avatarId = PlayerZeroSdk.GetHotLoadedAvatarId();
var avatar = await PlayerZeroSdk.InstantiateAvatarAsync(new CharacterRequestConfig {
    AvatarId = avatarId,
    BlueprintId = "BLUEPRINT_ID_HERE"
});
```

## 🎭 Mesh Transfer

Seamlessly apply a Player Zero avatar's visuals to an existing prefab:

```csharp
var avatar = await PlayerZeroSdk.InstantiateAvatarAsync(new CharacterRequestConfig {
    AvatarId = "AVATAR_ID_HERE",
    BlueprintId = "BLUEPRINT_ID_HERE"
});
new MeshTransfer().Transfer(avatar, existingCharacter);
```

## 📊 Analytics

1. Add the `PlayerZeroAnalytics` component to an empty GameObject in your first scene.
2. Set your **Game ID** in `Tools → Player Zero`.

### Fire a Custom Event

```csharp
PlayerZeroSdk.StartEventSession<GameSessionStartedEvent, GameSessionStartedProperties>(
    new GameSessionStartedEvent {
        Properties = new GameSessionStartedProperties {
            AvatarId = PlayerZeroSdk.GetHotLoadedAvatarId()
        }
    }
);
```

## 🔗 Deeplinking

Want to link into specific avatars or features? [Check out the docs on Deeplinking.](https://app.gitbook.com/o/-MUPNxqiv9WwarP92bMF/s/gzrFZv4JI6y2EfhkJwzN/integrations/unity-integration/deeplinking)
