# SyncColor
# 🎮 Color Sync Runner

This is my practice project developed in **Unity 6** to practice advanced Unity features:

- ✅ Zenject (Dependency Injection)
- ✅ UniTask (Async)
- ✅ Unity UI Toolkit
- ✅ New Input System
- ✅ Addressables
- ✅ Unity Playables
- ✅ HLSL Shader Showcase
- ✅ Mobile Swipe/Tap Controls

---

### 🎮 Game Overview

Player auto-runs forward. Tap & swipe controls allow:
- **Swipe Up**: Jump
- **Tap & Move**: Change horizontal lane
- **Color Buttons**: Tap to switch character color
- Stops movement if no touch is detected.

---

### 📂 Tech Stack

| Feature         | Details                                |
|----------------|----------------------------------------|
| Platform        | Unity 6 (Mobile Android/iOS)          |
| Architecture    | Modular (Zenject DI + Clean Scripts)  |
| Input           | New Input System + Touch Gestures     |
| UI              | UI Toolkit + Custom UXML/USS          |
| Optimization    | Addressables + LFS Support(GIT)            |
| Animation       | Unity Playables (custom timeline)     |
| Effects         | Custom HLSL Shaders                   |

---

### 🚀 Setup

1. Clone the repo
2. Open with **Unity 6**
3. Install dependencies via Package Manager
4. Hit Play & test on mobile device

---

### 📁 Project Structure

```plaintext
Assets/
├── _Project/
│   ├── Core/              # GameInstaller, core DI
│   ├── Input/             # MobileInputService, swipes
│   ├── Player/            # Controller, Color handling
│   ├── UI/                # UI Toolkit UXML + Logic
│   └── Installers/
├── Scenes/
│   └── MainScene.unity
