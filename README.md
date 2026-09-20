# 🐱 Code Cat – Escape!

A fun C# WinForms game that teaches C# through a simple quiz-based escape game.

Help the coding cat escape from the bug monster by answering C# questions correctly.

Every correct answer moves the cat forward.

Every wrong answer, or every missed question, lets the bug get closer.

**Can you save the cat?**

---

## 🎮 About the Game

**Code Cat – Escape!** is a small educational game built with:

- C#
- .NET 10
- Windows Forms
- Visual Studio 2026
- System.Text.Json

The goal of the project is to combine programming fundamentals with a small, fun game.

Instead of answering questions in a traditional quiz application, your answers directly affect the game.

### ✅ Correct Answer

🐱 The cat moves forward.

### ❌ Wrong Answer

👾 The bug moves closer.

### ⏱️ Time Runs Out

The answer counts as a wrong answer and the bug gets closer.

### ❤️ Three Lives

You can survive two catches.

After the third catch, the game is over.

### 🏁 Escape

Reach 100% escape distance and save the cat.

---

# ✨ Features

- C# learning questions
- Three answer choices
- Randomized questions
- Ten-second timer for each question
- Score system
- Streak system
- Bonus points
- Level system
- Three lives
- Cat versus bug chase
- Escape progress bar
- Correct and incorrect answer feedback
- Explanations for questions
- JSON-based question database
- Visual Studio Resources
- AI-generated game graphics
- Double buffering for smoother animation
- Restart functionality

---

# 🧠 What You Can Learn

This project demonstrates several useful C# and WinForms concepts.

## C# Fundamentals

- Variables
- Constants
- Classes
- Objects
- Arrays
- Lists
- Boolean values
- Conditions
- Loops
- Methods
- Random numbers

## Windows Forms

- Forms
- Panels
- Labels
- Buttons
- PictureBox
- ProgressBar
- Timer
- Events
- Event handlers
- TableLayoutPanel
- Control positioning

## JSON

The game stores its questions in a JSON resource.

The JSON data is converted into C# objects using:

```csharp
JsonSerializer.Deserialize<List<QuizQuestion>>();

This keeps the question data separate from the game logic.

.NET Resources

The project uses Visual Studio Resources for:

Questions
Cat image
Monster image
Background image

For example:

Properties.Resources.questions
Properties.Resources.cat
Properties.Resources.monster
Properties.Resources.Background
📁 Project Structure
Code Cat Escape
│
├── Properties
│   ├── Resources.resx
│   └── Resources.Designer.cs
│
├── Resources
│   ├── Background.png
│   ├── cat.png
│   ├── monster.png
│   └── questions.json
│
├── GameForm.cs
├── GamePanel.cs
├── QuestionRepository.cs
├── QuizQuestion.cs
├── Program.cs
│
├── Code Cat Escape.csproj
└── README.md
🏗️ How the Project Works

The main idea is:

questions.json
      ↓
Visual Studio Resource
      ↓
QuestionRepository
      ↓
QuizQuestion objects
      ↓
GameForm
      ↓
Game Logic
      ↓
🐱 Cat vs 👾 Bug

The game itself follows this simple flow:

Show Question
      ↓
Choose A / B / C
      ↓
Check Answer
      ↓
Correct?
   /       \
 YES       NO
  ↓         ↓
Cat +10    Bug moves
  ↓         ↓
Update      Update
Score       Lives
  \         /
   \       /
    Next Question
🧩 Main Classes
QuizQuestion

QuizQuestion represents one C# question.

It contains:

public class QuizQuestion
{
    public string Question { get; set; } = "";
    public string Code { get; set; } = "";
    public string[] Answers { get; set; } = Array.Empty<string>();
    public int CorrectAnswer { get; set; }
    public string Category { get; set; } = "";
    public string Explanation { get; set; } = "";
}

Each question contains:

The question text
Optional code
Three answers
The correct answer index
A category
An explanation
QuestionRepository

QuestionRepository loads the questions from the JSON resource.

The resource is read as bytes and converted into UTF-8 text:

byte[] data =
    Properties.Resources.questions;

string json =
    Encoding.UTF8.GetString(data);

The JSON is then converted into C# objects:

List<QuizQuestion>? questions =
    JsonSerializer.Deserialize<List<QuizQuestion>>(
        json,
        new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

This means the game can work with the questions as normal C# objects.

GamePanel

GamePanel is a small custom Panel used by the game.

It enables double buffering:

public class GamePanel : Panel
{
    public GamePanel()
    {
        DoubleBuffered = true;
        ResizeRedraw = true;

        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer,
            true);

        UpdateStyles();
    }
}

Double buffering helps reduce visible flickering while the cat and monster are moving.

GameForm

GameForm contains the actual game.

It handles:

Game state
User interface
Questions
Timer
Answers
Score
Streaks
Levels
Lives
Cat movement
Monster movement
Win state
Game-over state
🎯 Game Settings

The game uses two simple settings:

private const double EscapeDistance = 100;
private const double QuestionTime = 10;

The cat reaches safety when it reaches:

100%

The player gets:

10 seconds

for each question.

❓ Adding Your Own Questions

One of the easiest ways to customize the game is to edit the question database.

The questions are stored in:

questions.json

Example:

{
  "question": "What keyword checks a condition?",
  "code": "if (score > 100) { }",
  "answers": [
    "for",
    "if",
    "class"
  ],
  "correctAnswer": 1,
  "category": "Conditions",
  "explanation": "The if keyword checks whether the condition is true."
}

The correctAnswer value uses a zero-based index:

0 = first answer
1 = second answer
2 = third answer

For example:

"answers": [
  "for",
  "if",
  "class"
],
"correctAnswer": 1

means:

A) for
B) if      ← correct
C) class
💡 Create More C# Questions

You can easily add questions about:

Variables
Data types
Conditions
Loops
Methods
Classes
Objects
OOP
Collections
Strings
Operators
LINQ
async / await
Exceptions
Interfaces
Inheritance

The game logic does not need to change when you add more questions.

🎨 Game Assets

The project uses three visual resources:

cat.png
monster.png
Background.png

The cat and monster are displayed using PictureBox controls.

The background is assigned directly to the main game panel:

_scenePanel.BackgroundImage =
    Properties.Resources.Background;

_scenePanel.BackgroundImageLayout =
    ImageLayout.Stretch;

This allows the cat and monster to move independently over the background.

🤖 AI-Generated Artwork

The graphics used in this project were created with ChatGPT.

The goal was to create a small, cohesive visual style for the game without requiring a dedicated game-art workflow.

The visual assets include:

Coding cat
Bug monster
Nighttime game background

The artwork can be replaced with your own assets without changing the core game logic.

🚀 Getting Started
Requirements

You need:

Windows
Visual Studio 2026
.NET 10
Windows Forms support
Clone the Repository
git clone https://github.com/AS-Sharpdev/Code-Cat-Escape.git

Then open the project in Visual Studio.

Run the Game
Open the solution in Visual Studio.
Make sure the project builds successfully.
Press F5.

Or choose:

Start

The game should start immediately.

🎮 How to Play

The rules are simple.

Answer correctly

The cat moves forward.

Answer incorrectly

The bug gets closer.

Run out of time

The answer counts as a mistake.

Get caught

You lose one life.

Reach 100%

The cat escapes and you win.

🏆 Score and Streak

Correct answers give points.

The current score also depends on the level.

A streak is increased for consecutive correct answers.

Every five correct answers in a row, the player receives a bonus.

This creates a small reward system and encourages the player to continue answering correctly.

📈 Level System

The game increases the level as more questions are answered.

The current implementation uses:

_level =
    Math.Min(
        10,
        1 + _answered / 5);

The maximum level is:

10
🔧 Ideas for Improvements

This project is intentionally small, which makes it a good starting point for adding your own features.

Possible improvements include:

Sound effects
Background music
Animated cat
Animated monster
More levels
Boss battles
Power-ups
Combo multipliers
High-score saving
Local leaderboard
Difficulty selection
More advanced C# questions
Coding challenges instead of multiple-choice questions
Different environments
Multiple cats
Different monsters
🤖 Future Idea: Local AI

One possible next step is connecting the game to a local AI model such as Ollama.

Instead of loading every question from JSON, the game could ask a local AI model to generate new C# questions.

For example:

Code Cat
    ↓
Ollama
    ↓
Generate C# Question
    ↓
Show Question
    ↓
Player Answers

The AI could potentially generate:

Questions
Answers
Explanations
Difficulty levels
Coding challenges

This would turn Code Cat into a local AI-powered C# learning game.

📺 YouTube Tutorial

This project was created as a fun C# and WinForms project for the A.S. Sharpdev YouTube channel.

The video walks through the project step by step and explains how the different parts of the game work.

The focus is not on building a huge game.

The goal is to show how useful C# concepts can be combined into a small, playable project.

🔗 Links
GitHub

https://github.com/AS-Sharpdev/Code-Cat-Escape

YouTube

https://www.youtube.com/@AS-Sharpdev

📜 License

No specific open-source license has been added to this repository yet.

Please check the repository before redistributing the project or using the assets in other projects.

🐱 Save the Cat!

Learn C#.

Build something fun.

And don't let the bugs catch you.

Happy coding!
