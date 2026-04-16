# 🧲 Gravity Manipulation Puzzle Game (Sky Beneath Inspired)

A third-person puzzle platformer built in Unity where the player manipulates gravity to traverse surfaces and collect cubes within a time limit.

---

<img width="426" height="240" alt="Video Project 1" src="https://github.com/user-attachments/assets/21babcd6-c194-42ef-a9c5-6520cb268ebb" />

## 🎮 Gameplay Overview

* Control a character in a 3D environment
* Change gravity direction to walk on walls and ceilings
* Use strategic thinking to collect all cubes before time runs out
* Avoid falling into empty space

---

## 🕹️ Controls

| Key        | Action                   |
| ---------- | ------------------------ |
| W A S D    | Move character           |
| Mouse      | Rotate camera            |
| Space      | Jump                     |
| Arrow Keys | Select gravity direction |
| Enter      | Apply gravity            |
| ESC        | Unlock cursor            |

---

## 🚀 Features

### ✅ Character Movement

* Camera-relative movement system
* Smooth rotation and animation blending
* Rigidbody-based physics controller

### 🌍 Gravity Manipulation

* Change gravity in 4 directions (no diagonal)
* Smooth transition between surfaces
* Player reorients correctly after gravity switch

### 👻 Hologram Preview System

* Shows where player will align before applying gravity
* Prevents invalid or confusing transitions

### 🎥 Custom Third-Person Camera

* Mouse-controlled rotation
* Collision detection to avoid clipping
* Fully compatible with dynamic gravity

### 🧠 Ground Detection System

* SphereCast-based detection
* Handles slopes and edges
* Includes coyote time for smooth gameplay

### ⏱️ Timer System

* 2-minute countdown
* Game ends if objectives are not completed

### 💀 Game Over Conditions

* Player falls into empty space
* Timer reaches zero

## ⚙️ Setup Instructions

1. Clone the repository:

```bash
git clone https://github.com/TanmayBV/unity-gravity-puzzle-game.git
```

2. Open the project in Unity (recommended version: 2021 or later)

3. Open the main scene:

```
Assets/Scenes/MainScene.unity
```

4. Press ▶️ Play to run the game

---

## 🧪 Build Instructions

### Windows

* Go to **File → Build Settings**
* Select **Windows**
* Click **Build**

### Mac

* Go to **File → Build Settings**
* Select **macOS**
* Click **Build**

---

## 🧼 Code Quality

* Clean and modular structure
* Separation of concerns (movement, gravity, camera, game state)
* Optimized physics usage
* Well-documented scripts

---

## 🎯 Key Technical Highlights

* Custom gravity system (independent of Unity default gravity)
* Camera-relative input with directional snapping
* Smooth player reorientation using quaternions
* Physics-based movement using Rigidbody
* Advanced ground detection using SphereCast

---

## 📦 Deliverables

* ✅ Complete Unity Project
* ✅ Git repository with commit history
* ✅ Windows build
* ✅ Mac build

---

## 📌 Future Improvements

* Sound effects & background music
* UI polish (menus, transitions)
* More puzzle levels

---

## 👨‍💻 Author

**Tanmay Baravkar**
Unity Game Developer

---

## ⭐ Acknowledgment

Inspired by gravity-based mechanics similar to *Sky Beneath*

---

## 📜 License

This project is for educational and evaluation purposes only.
