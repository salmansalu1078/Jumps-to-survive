# Jumps to Survive

![License](https://img.shields.io/badge/license-MIT-blue) ![HTML5](https://img.shields.io/badge/HTML5-E34F26?logo=html5&logoColor=white) ![JavaScript](https://img.shields.io/badge/JavaScript-F7DF1E?logo=javascript&logoColor=black) ![Canvas](https://img.shields.io/badge/API-Canvas2D-orange)

A browser-based endless platformer where you keep jumping between platforms to survive. Miss a platform and it's over. The game speeds up as your score increases, testing your reaction time and rhythm. Built with the HTML5 Canvas API and vanilla JavaScript — no frameworks, no game engine.

---

## Table of Contents

- [Gameplay](#gameplay)
- [Features](#features)
- [Controls](#controls)
- [Scoring](#scoring)
- [Difficulty Progression](#difficulty-progression)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [Game Architecture](#game-architecture)
- [Physics Model](#physics-model)
- [Configuration](#configuration)
- [Browser Compatibility](#browser-compatibility)
- [Contributing](#contributing)
- [License](#license)

---

## Gameplay

Platforms scroll toward you from the right. Tap or press jump at the right moment to land on each one. Miss and you fall. The platforms get narrower, the gaps get wider, and the scroll speed increases as you survive longer.

Special platforms add variation — spring platforms bounce you higher, crumbling platforms collapse after one step, and moving platforms drift up and down. Collectibles appear on some platforms for bonus points.

---

## Features

- Endless procedurally generated platform stream
- Physics-based jump with variable height (hold longer = jump higher, up to a cap)
- Double jump — one extra jump before landing
- Platform variety: Standard, Spring, Crumbling, Moving (vertical)
- Collectibles: Coins (points), Stars (temporary slow-motion), Shields (survive one fall)
- Dynamic difficulty — speed and gap width scale with score
- High score tracking via `localStorage` (persists across sessions)
- Parallax scrolling background (3 layers)
- Particle effects on landing and collectible pickup
- Sound effects and background music with mute toggle
- Pause support
- Mobile touch support (tap anywhere to jump)

---

## Controls

### Desktop

| Key | Action |
|---|---|
| Space / Arrow Up / W | Jump (hold for higher jump) |
| Space (mid-air) | Double jump |
| P / Escape | Pause / Resume |
| M | Mute / Unmute |
| R | Restart after game over |

### Mobile

| Gesture | Action |
|---|---|
| Tap anywhere | Jump |
| Tap (mid-air) | Double jump |
| Tap pause button | Pause / Resume |

---

## Scoring

| Action | Points |
|---|---|
| Landing on a standard platform | +10 |
| Landing on a spring platform | +15 |
| Landing on a moving platform | +20 |
| Collecting a coin | +5 |
| Collecting a star (slow-mo) | +25 |
| Surviving 10 platforms in a row | +50 bonus |
| Surviving 50 platforms in a row | +200 bonus |

Score multiplier increases by 0.1x for every 10 consecutive platforms without a power-up assist.

---

## Difficulty Progression

| Score Range | Scroll Speed | Min Gap | Max Gap | Platform Width |
|---|---|---|---|---|
| 0 – 199 | 3 px/frame | 80px | 120px | 120 – 160px |
| 200 – 499 | 4 px/frame | 100px | 150px | 100 – 140px |
| 500 – 999 | 5 px/frame | 120px | 180px | 80 – 120px |
| 1000 – 1999 | 6 px/frame | 140px | 220px | 60 – 100px |
| 2000+ | 8 px/frame | 160px | 280px | 40 – 80px |

Crumbling and moving platforms start appearing at 200 points. Spring platforms appear at 100 points.

---

## Getting Started

### Open directly in browser

```bash
git clone https://github.com/Salmanahmed1078/Jumps-to-survive.git
cd Jumps-to-survive
open index.html
```

No install required. Just open in any modern browser.

### Local server (for audio)

Audio requires a server context — `file://` URLs block Web Audio API in some browsers:

```bash
# Python
python3 -m http.server 8080

# Node.js
npx serve .
```

Then open [http://localhost:8080](http://localhost:8080).

---

## Project Structure

```
Jumps-to-survive/
├── index.html              # Game canvas and UI overlay
├── css/
│   ├── style.css           # HUD, score display, menus
│   └── responsive.css      # Mobile scaling
├── js/
│   ├── main.js             # Entry point and game loop bootstrap
│   ├── game.js             # Game state machine and update loop
│   ├── player.js           # Player physics, jump, double-jump logic
│   ├── platform.js         # Platform types and behavior
│   ├── generator.js        # Procedural platform stream generator
│   ├── collectible.js      # Coin, star, shield logic
│   ├── renderer.js         # Canvas 2D rendering layer
│   ├── particles.js        # Landing and pickup particle system
│   ├── background.js       # Parallax background layers
│   ├── input.js            # Keyboard and touch input handler
│   ├── audio.js            # Sound effects and background music
│   ├── score.js            # Score tracking and localStorage
│   └── config.js           # Game constants and tuning values
├── assets/
│   ├── sprites/            # Player and platform sprite sheets
│   ├── backgrounds/        # Parallax background layers
│   └── sounds/             # .ogg and .mp3 files
└── README.md
```

---

## Game Architecture

### Game loop

```js
function loop(timestamp) {
  const dt = timestamp - lastTimestamp;
  lastTimestamp = timestamp;

  input.update();
  game.update(dt);
  renderer.draw(game.state);

  requestAnimationFrame(loop);
}
```

The update step is fixed-timestep with interpolation — physics does not break at lower frame rates.

### State machine

```
TITLE → PLAYING → PAUSED → PLAYING
                     │
                     └── GAME_OVER → TITLE
```

### Platform spawning

The generator maintains a lookahead buffer of 8 platforms. When the leftmost platform scrolls off screen, a new one is appended to the right. Platform properties are calculated based on the current score:

```js
function spawnPlatform(score) {
  const difficulty = getDifficulty(score);
  return {
    x: CANVAS_WIDTH + SPAWN_OFFSET,
    y: randomY(difficulty),
    width: randomWidth(difficulty),
    type: randomType(score),       // standard | spring | crumbling | moving
    speed: difficulty.scrollSpeed,
  };
}
```

---

## Physics Model

### Jump

```
jumpVelocity = BASE_JUMP_VELOCITY (on tap)
             + HOLD_BONUS × holdDuration (while held, up to MAX_HOLD_DURATION)

velocityY += GRAVITY × dt
positionY += velocityY × dt
```

### Variable jump height

| Tap duration | Approximate jump height |
|---|---|
| Instant tap | ~80px |
| Hold 150ms | ~130px |
| Hold 300ms (max) | ~180px |

### Double jump

Available once per airborne period. Resets on landing. Jump velocity is set to `DOUBLE_JUMP_VELOCITY` (slightly lower than BASE_JUMP_VELOCITY). Cannot hold-boost the double jump.

### Platform collision

Only the bottom of the player bounding box is checked against the top of platform bounding boxes. No side or ceiling collisions — the game is a pure vertical-landing platformer.

---

## Configuration

All tuning values live in `js/config.js`:

```js
const CONFIG = {
  CANVAS_WIDTH: 480,
  CANVAS_HEIGHT: 640,
  GRAVITY: 0.5,                  // px per frame squared
  BASE_JUMP_VELOCITY: -12,       // Negative = upward
  DOUBLE_JUMP_VELOCITY: -10,
  MAX_HOLD_DURATION: 300,        // ms
  HOLD_BONUS: 0.015,             // px/frame per ms held
  INITIAL_SCROLL_SPEED: 3,       // px per frame
  PLATFORM_BUFFER: 8,            // platforms in lookahead
  SOUND_ENABLED: true,
  MUSIC_ENABLED: true,
  SHOW_DEBUG: false,             // Bounding box overlay
};
```

---

## Browser Compatibility

| Browser | Minimum Version | Notes |
|---|---|---|
| Chrome | 60+ | Full support |
| Firefox | 55+ | Full support |
| Safari | 12+ | Full support |
| Edge | 79+ | Full support |
| iOS Safari | 13+ | Touch controls supported |
| Android Chrome | 70+ | Touch controls supported |

Requires HTML5 Canvas 2D, `requestAnimationFrame`, and Web Audio API.

---

## Contributing

1. Fork the repository
2. Make changes in a feature branch (`git checkout -b feat/your-feature`)
3. Test in Chrome and Firefox at minimum
4. Test mobile layout if you changed input or layout code
5. Open a pull request describing what changed and why

No external dependencies — keep it that way.

---

## License

MIT © Salman Ahmed
