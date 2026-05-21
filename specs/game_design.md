# Zeros & Ones: Game Design Document (GDD)

This document outlines the core architecture, gameplay mechanics, features, and design guidelines for "Zeros & Ones," a 2D puzzle platformer and logic circuit simulation game.

---

## 🔄 Core Gameplay Loop

```
[Main Menu] ──(New Game)──> [Vertical Exploration Map]
                                      │
                                (Enter Portal)
                                      ▼
                        [Logic Circuit Level Solver]
                                      │
                           (Validate Truth Table)
                                 /          \
                         (Success)         (Failure)
                            /                  \
   [Unlock Module / Return to Map]      [Penalty Feedback / Explode]
```

---

## 🏃‍♂️ Player Control & Map Movement

### Vertical Exploration Map
- **Layout**: A vertical map (scrolling up/down) where the player travels vertically.
- **Movement**:
  - Left/Right movement on platforms.
  - Vertically climbing/descending (e.g., ladders, elevator, or vertical platforms).
- **Abilities**:
  - **Jump**: Able to jump to reach elevated platforms.
- **Exploration**:
  - Portals (doors) are placed on the left and right sides of the map.
  - The player walks into a portal to initiate a challenge level.

---

## 🧩 Logic Gate Challenges System

### Level Structure
- Entering a portal loads a **Logic Circuit Solver** workspace.
- Each portal is dedicated to solving exactly **one** logic gate or module (starting from NAND, and building up to an ALU).

### Level Constraints (Challenges)
To earn high ratings or clear specific gates, levels enforce one or more constraints:
1. **Time Challenge (تحدي الوقت)**: The player must design and solve the logic gate within a specified countdown timer.
2. **Component Count Challenge (تحدي عدد القطع)**: The player must solve the puzzle using a strict limit on the number of components (minimizing gates for optimization).
3. **Forbidden Components Challenge (تحدي القطع المحظورة)**: The player is prohibited from using certain components (e.g., "Solve XOR without using OR gates").

---

## ⚠️ Penalties & Feedback System

To make circuit design tactile and arcade-like, the game provides immediate positive and negative feedback:

1. **Completely Wrong Solution (الحل الخاطئ تماماً)**:
   - **Feedback**: The logical module physically explodes.
   - **Visual Effect**: Camera shake (اهتزاز في الكاميرا).
2. **Over-engineered / Inefficient Solution (الحل بقطع زائدة)**:
   - **Feedback**: The player's health (Hearts / القلوب) is decreased.
   - **Audio Effect**: A loud, annoying cooling fan noise (صوت مروحة شديد ومزعج) plays, representing the circuit overheating from inefficiency.

---

## 📚 Learning & Educational Hints

1. **Tutorial Slides (الشرائح التعليمية)**:
   - Interactive slides introduced before levels to explain Boolean logic, logic states, binary numbers, and assembly concepts.
2. **Interactive Hint Panel (دلائل وإرشادات الحل)**:
   - **Concepts**: Simple explanations, such as DFA (Deterministic Finite Automata) state diagrams to guide sequential state machine designs.
   - **Shortcuts**: Display keyboard and workspace shortcuts.
   - **Suggested Gates**: Direct prompts pointing the player to a specific component needed for the puzzle.
3. **Visual Signal Flow (المؤثرات البصرية التعليمية)**:
   - **Wire Flashing / Pulsing (وميض الأسلاك)**: Active logical currents (High/Low or 1/0) flash or glow along the wires to visualize signal propagation and help debug circuit routing.

---

## 🏁 Ultimate Objective

- **End Game (الهدف النهائي)**: Reaching and successfully building the **ALU (Arithmetic Logic Unit)**, representing the integration of arithmetic (addition, subtraction) and logical operations.
