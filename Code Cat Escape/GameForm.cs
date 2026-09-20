using Timer = System.Windows.Forms.Timer;

namespace Code_Cat_Escape
{
    public partial class GameForm : Form
    {
        // -----------------------------
        // Game settings
        // -----------------------------
        private const double EscapeDistance = 100;
        private const double QuestionTime = 10;

        // -----------------------------
        // Game data
        // -----------------------------
        private readonly Random _random = new();
        private readonly List<QuizQuestion> _questions;

        private QuizQuestion _currentQuestion = null!;
        private int _lastQuestionIndex = -1;

        private int _score;
        private int _level = 1;
        private int _streak;
        private int _lives = 3;
        private int _answered;

        private double _time;

        private double _catProgress = 10;
        private double _monsterProgress;

        private bool _gameRunning;
        private bool _answerLocked;

        // -----------------------------
        // UI
        // -----------------------------
        private readonly Timer _gameTimer;
        private readonly Timer _nextQuestionTimer;

        private GamePanel _scenePanel = null!;
        private PictureBox _cat = null!;
        private PictureBox _monster = null!;

        private Label _scoreLabel = null!;
        private Label _levelLabel = null!;
        private Label _streakLabel = null!;
        private Label _livesLabel = null!;

        private Label _categoryLabel = null!;
        private Label _questionLabel = null!;
        private Label _codeLabel = null!;
        private Label _timeLabel = null!;
        private Label _feedbackLabel = null!;

        private ProgressBar _timeBar = null!;
        private ProgressBar _escapeBar = null!;
        private Label _escapeLabel = null!;

        private Button[] _answerButtons = null!;

        public GameForm()
        {
            Text = "Code Cat - Escape!";

            ClientSize =  new Size(1180, 760);
            StartPosition =   FormStartPosition.CenterScreen;
            FormBorderStyle =  FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            BackColor =   Color.FromArgb(8, 10, 20);
            Font = new Font("Segoe UI", 10F);
            DoubleBuffered = true;

            // Load questions from Resources.
            _questions = new QuestionRepository().Load();

            BuildGameScreen();

            // Load images from Resources.
            LoadImages();

            _gameTimer = new Timer {Interval = 100};
            _gameTimer.Tick += GameTimer_Tick;

            _nextQuestionTimer = new Timer {Interval = 800};
            _nextQuestionTimer.Tick +=  NextQuestionTimer_Tick;

            StartNewGame();
        }

        // =========================================================
        // BUILD UI
        // =========================================================
        private void BuildGameScreen()
        {
            CreateTopBar();
            CreateScene();
            CreateBottomBar();
        }
        private void CreateTopBar()
        {
            Panel topBar = new()
            {
                Dock = DockStyle.Top,
                Height = 76,
                BackColor = Color.FromArgb(27, 29, 48),
                Padding = new Padding(15, 8, 15, 8)
            };
            TableLayoutPanel hud = new()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1
            };

            for (int i = 0; i < 4; i++)
                hud.ColumnStyles.Add( new ColumnStyle(SizeType.Percent, 25));

            _scoreLabel =   CreateHudLabel("SCORE\n0");
            _levelLabel =  CreateHudLabel("LEVEL\n1");
            _streakLabel =    CreateHudLabel("STREAK\n0");
            _livesLabel =    CreateHudLabel("LIVES\n♥ ♥ ♥");

            hud.Controls.Add(_scoreLabel,0,0);
            hud.Controls.Add(_levelLabel,1,0);
            hud.Controls.Add(_streakLabel,2,0);
            hud.Controls.Add(_livesLabel,3,0);

            topBar.Controls.Add(hud);
            Controls.Add(topBar);
        }
        private static Label CreateHudLabel(string text)
        {
            return new Label
            {
                Dock = DockStyle.Fill,
                Text = text,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.White,
                Font = new Font("Segoe UI",12F,FontStyle.Bold)
            };
        }
        private void CreateScene()
        {
            _scenePanel = new GamePanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(8,10,20),
                BackgroundImage = Properties.Resources.Background,
                BackgroundImageLayout =ImageLayout.Stretch
            };

            CreateQuestionCard();
            CreateAnswerButtons();
            CreateFeedback();
            CreateCharacters();
            CreateEscapeBar();

            Controls.Add(_scenePanel);
        }
        private void CreateQuestionCard()
        {
            Panel card = new()
            {
                Location = new Point(260, 20),
                Size = new Size(830, 250),
                BackColor = Color.FromArgb(20, 24, 43),
                BorderStyle = BorderStyle.FixedSingle
            };

            _categoryLabel = new Label
            {
                Location = new Point(25, 15),
                Size = new Size(780, 22),
                ForeColor = Color.Cyan,
                Font = new Font("Consolas", 10F, FontStyle.Bold)
            };

            _questionLabel = new Label
            {
                Location = new Point(25, 42),
                Size = new Size(780, 60),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 17F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            _codeLabel = new Label
            {
                Location = new Point(65, 110),
                Size = new Size(700, 48),
                ForeColor = Color.Gainsboro,
                BackColor = Color.FromArgb(8, 10, 20),
                Font = new Font("Consolas", 13F),
                TextAlign = ContentAlignment.MiddleCenter,
                BorderStyle = BorderStyle.FixedSingle
            };

            _timeBar = new ProgressBar
            {
                Location = new Point(65, 170),
                Size = new Size(700, 14),
                Minimum = 0,
                Maximum = 100,
                Value = 100
            };

            _timeLabel = new Label
            {
                Location = new Point(65, 186),
                Size = new Size(700, 28),
                ForeColor = Color.White,
                Font = new Font("Consolas", 10F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            card.Controls.Add(_categoryLabel);
            card.Controls.Add(_questionLabel);
            card.Controls.Add(_codeLabel);
            card.Controls.Add(_timeBar);
            card.Controls.Add(_timeLabel);

            _scenePanel.Controls.Add(card);

            card.BringToFront();
        }
        private void CreateAnswerButtons()
        {
            TableLayoutPanel answerPanel = new()
            {
                Location = new Point(260, 285),
                Size = new Size(830, 92),
                ColumnCount = 3,
                RowCount = 1,
                BackColor = Color.Transparent,
                Padding = new Padding(0)
            };

            for (int i = 0; i < 3; i++)
            {
                answerPanel.ColumnStyles.Add(
                    new ColumnStyle(
                        SizeType.Percent,
                        33.33F));
            }

            _answerButtons = new Button[3];

            for (int i = 0; i < 3; i++)
            {
                Button button = new()
                {
                    Dock = DockStyle.Fill,
                    Margin = new Padding(5),
                    Tag = i,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(22, 28, 50),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    TabStop = false
                };

                button.FlatAppearance.BorderSize = 1;

                button.Click += AnswerButton_Click;

                answerPanel.Controls.Add(
                    button,
                    i,
                    0);

                _answerButtons[i] = button;
            }

            _scenePanel.Controls.Add(answerPanel);

            answerPanel.BringToFront();
        }
        private void CreateFeedback()
        {
            _feedbackLabel = new Label
            {
                Location = new Point(260, 385),
                Size = new Size(830, 50),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            _scenePanel.Controls.Add(_feedbackLabel);

            _feedbackLabel.BringToFront();
        }
        private void CreateCharacters()
        {
            _monster = CreateCharacterBox("MONSTER");
            _cat =     CreateCharacterBox("CAT");

            _scenePanel.Controls.Add(_monster);
            _scenePanel.Controls.Add(_cat);

            _monster.BringToFront();
            _cat.BringToFront();
        }
        private static PictureBox CreateCharacterBox(string Charater)
        {
            if (Charater == "CAT")
            {
                return new PictureBox
                {
                    Size = new Size(60, 90),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.Transparent,
                    TabStop = false
                };
            }
            else if (Charater == "MONSTER")
            {
                return new PictureBox
                {
                    Size = new Size(150, 120),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.Transparent,
                    TabStop = false
                };
            }
            else
            {
                return new PictureBox
                {
                    Size = new Size(125, 100),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.Transparent
                };
            }
        }
        private void CreateEscapeBar()
        {
            _escapeBar = new ProgressBar
            {
                Location = new Point(330, 545),
                Size = new Size(690, 18),
                Minimum = 0,
                Maximum = 100,
                Value = 10
            };

            _escapeLabel = new Label
            {
                Location = new Point(330, 568),
                Size = new Size(690, 28),
                ForeColor = Color.White,
                Font = new Font("Consolas", 10F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            _scenePanel.Controls.Add(_escapeBar);
            _scenePanel.Controls.Add(_escapeLabel);

            _escapeBar.BringToFront();
            _escapeLabel.BringToFront();
        }
        private void CreateBottomBar()
        {
            Panel bottomBar = new()
            {
                Dock = DockStyle.Bottom,
                Height = 66,
                BackColor = Color.FromArgb(27, 29, 48),
                Padding = new Padding(15, 10, 15, 10)
            };
            Label tip = new()
            {
                Dock = DockStyle.Fill,
                Text = "Learn C#  •  Choose an answer  •  Save the Cat!",
                ForeColor = Color.LightGray,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };
            Button restart = new()
            {
                Dock = DockStyle.Right,
                Width = 125,
                Text = "RESTART",
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(36, 48, 78),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            restart.FlatAppearance.BorderSize = 1;
            restart.Click += (_, _) => StartNewGame();
            bottomBar.Controls.Add(tip);
            bottomBar.Controls.Add(restart);
            Controls.Add(bottomBar);
        }

        // =========================================================
        // ASSETS
        // =========================================================
        private void LoadImages()
        {
            _cat.Image =  Properties.Resources.cat;
            _monster.Image = Properties.Resources.monster;
        }

        // =========================================================
        // GAME START
        // =========================================================
        private void StartNewGame()
        {
            _score = 0;
            _level = 1;
            _streak = 0;
            _lives = 3;
            _answered = 0;

            _time = 0;
            _catProgress = 10;
            _monsterProgress = 0;

            _lastQuestionIndex = -1;

            _gameRunning = true;
            _answerLocked = false;

            UpdateHud();
            UpdateCharacters();
            ShowNextQuestion();

            _gameTimer.Start();
        }
        // =========================================================
        // QUESTIONS
        // =========================================================
        private void ShowNextQuestion()
        {
            int index = GetRandomQuestionIndex();

            _currentQuestion =    _questions[index];
            _lastQuestionIndex =   index;

            _time = 0;
            _answerLocked = false;

            _categoryLabel.Text =   _currentQuestion.Category.ToUpperInvariant();
            _questionLabel.Text =   _currentQuestion.Question;

            _codeLabel.Text =    _currentQuestion.Code.Replace("\\n",Environment.NewLine);

            _codeLabel.Visible = !string.IsNullOrWhiteSpace(_currentQuestion.Code);

            for (int i = 0;i < _answerButtons.Length; i++)
            {
                _answerButtons[i].Text =
                    $"{(char)('A' + i)})  " +
                    $"{_currentQuestion.Answers[i]}";

                _answerButtons[i].Enabled = true;
                _answerButtons[i].BackColor = Color.FromArgb(22,28,50);
            }

            _feedbackLabel.Text = "";
            UpdateTimer();
        }
        private int GetRandomQuestionIndex()
        {
            if (_questions.Count == 1)
                return 0;

            int index;

            do
            {
                index = _random.Next(_questions.Count);
            }
            while ( index == _lastQuestionIndex);

            return index;
        }

        // =========================================================
        // TIMER
        // =========================================================
        private void GameTimer_Tick(object? sender, EventArgs e)
        {
            if (!_gameRunning || _answerLocked)
            {
                return;
            }

            _time += 0.1;

            UpdateTimer();

            if (_time >= QuestionTime)
            {
                ResolveAnswer(-1);
            }
        }
        private void UpdateTimer()
        {
            double remaining = Math.Max(0, QuestionTime - _time);
            _timeLabel.Text = $"TIME: {remaining:0.0}s";
            int value =  (int)(  remaining / QuestionTime *   100);
            _timeBar.Value =Math.Clamp(value,0,100);
        }

        // =========================================================
        // ANSWER
        // =========================================================
        private void AnswerButton_Click(object? sender, EventArgs e)
        {
            if (!_gameRunning || _answerLocked)
            {
                return;
            }

            if (sender is Button button && button.Tag is int answerIndex)
            {
                ResolveAnswer(answerIndex);
            }
        }
        private void ResolveAnswer( int answerIndex)
        {
            if (_answerLocked)
                return;

            _answerLocked = true;
            _answered++;

            foreach (Button button     in _answerButtons)
            {
                button.Enabled = false;
            }

            bool correct =answerIndex == _currentQuestion.CorrectAnswer;
            if (answerIndex >= 0)
            {
                _answerButtons[answerIndex].BackColor =  correct ? Color.FromArgb(25,120,75) : Color.FromArgb(135,45,55);
            }

            if (correct)
            {
                HandleCorrectAnswer();
            }
            else
            {
                HandleWrongAnswer(
                    answerIndex < 0);
            }

            UpdateLevel();
            UpdateHud();
            UpdateCharacters();

            if (_catProgress >=  EscapeDistance)
            {
                WinGame();
                return;
            }

            if (_monsterProgress >=  _catProgress)
            {
                _lives--;
                _monsterProgress =Math.Max(0, _catProgress - 25);
                _feedbackLabel.Text =  $"💥 The bug caught you!  " +  $"{Math.Max(0, _lives)} lives left.";

                UpdateHud();
                UpdateCharacters();

                if (_lives <= 0)
                {
                    LoseGame();
                    return;
                }
            }

            _nextQuestionTimer.Start();
        }
        private void HandleCorrectAnswer()
        {
            _score += 10 + _level * 2;

            _streak++;
            _catProgress += 10;

            _monsterProgress = Math.Max(0,_monsterProgress - 2);
            _feedbackLabel.Text = $"✓ Correct!  " +  $"{_currentQuestion.Explanation}";
            _feedbackLabel.ForeColor =   Color.LimeGreen;

            if (_streak % 5 == 0)
            {
                _score += 50;
                _catProgress += 5;
                _feedbackLabel.Text =   $"🔥 {_streak} STREAK! " +  "+50 BONUS";
            }
        }
        private void HandleWrongAnswer(  bool timeIsUp)
        {
            _streak = 0;
            _monsterProgress +=  12 + _level;

            _catProgress = Math.Max(5, _catProgress - 3);

            string reason = timeIsUp ? "Time's up!" : "Not quite!";

            _feedbackLabel.Text =$"✗ {reason}  " +   $"{_currentQuestion.Explanation}";
            _feedbackLabel.ForeColor =   Color.OrangeRed;
        }
        private void NextQuestionTimer_Tick( object? sender,  EventArgs e)
        {
            _nextQuestionTimer.Stop();

            if (_gameRunning)
            {
                ShowNextQuestion();
            }
        }
        private void UpdateLevel()
        {
            _level =  Math.Min(10,1 + _answered / 5);
        }

        // =========================================================
        // GAME RESULTS
        // =========================================================
        private void WinGame()
        {
            _gameRunning = false;

            _gameTimer.Stop();
            _nextQuestionTimer.Stop();

            MessageBox.Show(
                $"You saved the cat!\n\n" +
                $"Score: {_score}\n" +
                $"Level: {_level}\n" +
                $"Streak: {_streak}",

                "YOU ESCAPED! 🐱",

                MessageBoxButtons.OK,

                MessageBoxIcon.Information);
        }
        private void LoseGame()
        {
            _gameRunning = false;

            _gameTimer.Stop();
            _nextQuestionTimer.Stop();

            MessageBox.Show(
                $"The bug caught the cat.\n\n" +
                $"Score: {_score}\n" +
                $"Level: {_level}\n" +
                $"Correct answers: {_answered}",

                "GAME OVER!",

                MessageBoxButtons.OK,

                MessageBoxIcon.Warning);
        }

        // =========================================================
        // HUD + CHARACTERS
        // =========================================================
        private void UpdateHud()
        {
            _scoreLabel.Text = $"SCORE\n{_score}";
            _levelLabel.Text =  $"LEVEL\n{_level}";
            _streakLabel.Text = $"STREAK\n{_streak}";

            string hearts =   string.Join(" ", Enumerable.Repeat("♥", _lives));
            _livesLabel.Text =$"LIVES\n{hearts}";
        }
        private void UpdateCharacters()
        {
            int sceneWidth = _scenePanel.ClientSize.Width;
            int sceneHeight = _scenePanel.ClientSize.Height;
            if (sceneWidth <= 0 || sceneHeight <= 0)
            {
                return;
            }
            // The track is only the lower part of the screen.
            int left = 40;
            int right = sceneWidth - 40;
            int width = Math.Max(1, right - left);
            double catX = left + (_catProgress / EscapeDistance) * width;
            double monsterX = left + (_monsterProgress / EscapeDistance) * width;
            int y = sceneHeight - 270;
            int catLeft = (int)catX - _cat.Width / 2;
            int monsterLeft = (int)monsterX - _monster.Width / 2;
            catLeft = Math.Clamp(catLeft, 10, sceneWidth - _cat.Width - 10);
            // Keep a visible gap between monster and cat.
            // The logical game distance can still be very small,
            // but the two PictureBoxes will never overlap.
            const int gap = 8;
            int minimumCatLeft = 10 + _monster.Width + gap;
            catLeft = Math.Max(catLeft, minimumCatLeft);
            monsterLeft = Math.Min(monsterLeft, catLeft - _monster.Width - gap);
            monsterLeft = Math.Clamp(  monsterLeft,    10,   sceneWidth - _monster.Width - 10);

            _cat.Location = new Point(catLeft, y + 30);
            _monster.Location = new Point(monsterLeft, y);
            _escapeBar.Value = Math.Clamp((int)_catProgress, 0, 100);
            _escapeLabel.Text = $"ESCAPE DISTANCE  {Math.Clamp(_catProgress, 0, 100):0}%";

            _monster.BringToFront();
            _cat.BringToFront();
        }

        protected override void OnFormClosing( FormClosingEventArgs e)
        {
            _gameTimer.Stop();
            _nextQuestionTimer.Stop();

            base.OnFormClosing(e);
        }
    }
}
