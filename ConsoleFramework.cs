using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CJ {
    #region interface IApplication
    public interface IApplication {
        void Run();
        void Stop();
    }
    #endregion
    #region interface IMenuItem
    public interface IMenuItem {
        string Text { get; }
        void Action();
    }
    #endregion
    #region abstract class MenuItem
    public abstract class MenuItem : IMenuItem {
        #region Protected Fields
        protected string _text;
        #endregion

        #region Public Properties
        public string Text { get { return _text; } }
        #endregion

        #region Abstract Methods
        public abstract void Action();
        #endregion

        #region Protected Constructors
        protected MenuItem() { _text = "Menu Item"; }
        protected MenuItem(string text) { _text = text; }
        #endregion
    }
    #endregion
    #region instance class ContainerMenuItem
    public class ContainerMenuItem : MenuItem {
        #region Protected Fields
        protected List<IMenuItem> _items;
        protected ConsoleColor _backColor, _foreColor, _itemsBackColor, _itemsForeColor, _itemsHiBackColor, _itemsHiForeColor;
        protected string _windowTitle, _itemsListTitle, _instructions;
        protected int _selected;
        #endregion

        #region Public Properties
        public List<IMenuItem> Items { get { return _items; } }
        //This is the background of the window when this menu item is displayed (executed/selected)
        public ConsoleColor BackColor { get { return _backColor; } set { _backColor = value; } }
        //The foreground  of the window when this menu item is displayed (executed/selected)
        public ConsoleColor ForeColor { get { return _foreColor; } set { _foreColor = value; } }
        //The default background color of sub items names
        public ConsoleColor ItemsBackColor { get { return _itemsBackColor; } set { _itemsBackColor = value; } }
        //The default foreground color of sub items names
        public ConsoleColor ItemsForeColor { get { return _itemsForeColor; } set { _itemsForeColor = value; } }
        //The background color of sub items when they are highlighted
        public ConsoleColor ItemsHilightBackColor { get { return _itemsHiBackColor; } set { _itemsHiBackColor = value; } }
        //The foreground color of sub items when they are highlighted
        public ConsoleColor ItemsHilightForeColor { get { return _itemsHiForeColor; } set { _itemsHiForeColor = value; } }
        //This is displayed as the console window title when this menu item is displayed (action executed)
        public string WindowTitle { get { return _windowTitle; } set { _windowTitle = value; } }
        //This is displayed as the title of the page when this menu item is displayed
        public string ItemsListTitle { get { return _itemsListTitle; } set { _itemsListTitle = value; } }
        //This is displayed as the content of the page when the menu item is displayed, right before the menu items list
        public string Instructions { get { return _instructions; } set { _instructions = value; } }
        //Gets the currently selected item (0-based index)
        public int CurrentSelection { get { return _selected; } }
        #endregion

        #region Constructors
        protected ContainerMenuItem() : base() { _items = new List<IMenuItem>(); _init(); }
        public ContainerMenuItem(string text) : base(text) { _items = new List<IMenuItem>(); _init(); }
        public ContainerMenuItem(string text, IEnumerable<IMenuItem> items) : base(text) { _items = new List<IMenuItem>(items); _init(); }
        private void _init() {
            _backColor = ConsoleColor.Black; _foreColor = ConsoleColor.White;
            _itemsBackColor = ConsoleColor.Blue; _itemsForeColor = ConsoleColor.White;
            _itemsHiBackColor = ConsoleColor.Gray; _itemsHiForeColor = ConsoleColor.Black;
            _windowTitle = _itemsListTitle = _text; _selected = 0;
        }
        #endregion

        #region Public Methods
        public override void Action() {
            Console.CursorVisible = false;
            Console.Title = _windowTitle;
            bool again = true;
            while (again) {
                _drawMenu();
                ConsoleKeyInfo k = new ConsoleKeyInfo(); bool loop = true;
                while (loop) {
                    k = Console.ReadKey(true);
                    if (k.Key == ConsoleKey.UpArrow) {
                        _selected = _selected > 0 ? _selected - 1 : _items.Count - 1;
                        _drawMenu();
                    }
                    else if (k.Key == ConsoleKey.DownArrow) {
                        _selected = _selected < _items.Count - 1 ? _selected + 1 : 0;
                        _drawMenu();
                    }
                    else if (k.Key == ConsoleKey.Escape) { loop = false; again = false; }
                    else if (k.Key == ConsoleKey.Enter) { loop = false; }
                }
                if (_items.Count > 0 && k.Key != ConsoleKey.Escape) { _items[_selected].Action(); }
            }
        }
        protected void _drawMenu() {
            Console.BackgroundColor = _backColor; Console.ForegroundColor = _foreColor;
            Console.Clear();
            Console.SetCursorPosition((Console.WindowWidth / 2) - (_itemsListTitle.Length / 2), 1);
            Console.WriteLine(_itemsListTitle);
            Console.WriteLine(string.IsNullOrEmpty(_instructions) ? "" : _instructions);
            Console.WriteLine();
            for (int i = 0; i < _items.Count; i++) {
                Console.Write("{0,4}. ", i + 1);
                Console.BackgroundColor = i == _selected ? _itemsHiBackColor : _itemsBackColor;
                Console.ForegroundColor = i == _selected ? _itemsHiForeColor : _itemsForeColor;
                Console.WriteLine(_items[i].Text); Console.WriteLine();
                Console.BackgroundColor = _backColor; Console.ForegroundColor = _foreColor;
            }
            Console.WriteLine();
            Console.WriteLine("Please select one of the options above ^ and press <Enter> to activate. Press <Esc> to quit. ");
        }
        #endregion
    }
    #endregion
    #region instance class ActionMenuItem
    public class ActionMenuItem : MenuItem {
        #region Public Methods
        public override void Action() {
            OnSelected(EventArgs.Empty);
        }
        #endregion

        #region Constructors
        protected ActionMenuItem() : base() { }
        public ActionMenuItem(string text) : base(text) { }
        #endregion

        #region Events
        public event EventHandler Selected;
        protected virtual void OnSelected(EventArgs e) { if (Selected != null) { Selected(this, e); } }
        #endregion
    }
    #endregion
    #region class MenuApplication
    public class MenuApplication : ContainerMenuItem, IApplication {
        #region Protected Fields
        protected bool _showSplash;
        protected string _splashText;
        #endregion

        #region Public Properties
        public bool ShowSplash { get { return _showSplash; } set { _showSplash = value; } }
        public string SplashScreenText { get { return _splashText; } set { _splashText = value; } }
        #endregion

        #region Constructors
        public MenuApplication() : base() { _showSplash = false; _splashText = ""; }
        public MenuApplication(string text) : base(text) { _showSplash = false; _splashText = ""; }
        public MenuApplication(string text, string title, string windowTitle, string instructions)
            : base(text) {
            _instructions = instructions; _itemsListTitle = title; _windowTitle = windowTitle; _showSplash = false; _splashText = "";
        }
        public MenuApplication(IEnumerable<IMenuItem> items) : base("", items) { _showSplash = false; _splashText = ""; }
        public MenuApplication(string text, IEnumerable<IMenuItem> items) : base(text, items) { _showSplash = false; _splashText = ""; }
        public MenuApplication(string text, string title, string windowTitle, string instructions, IEnumerable<IMenuItem> items)
            : base(text, items) {
            _instructions = instructions; _itemsListTitle = title; _windowTitle = windowTitle; _showSplash = false; _splashText = "";
        }
        #endregion

        #region Public Methods
        public virtual void ShowSplashScreen() {
            if (_showSplash) {
                Console.BackgroundColor = _backColor; Console.ForegroundColor = _foreColor;
                Console.Clear();
                Console.WriteLine(_splashText);
                Thread.Sleep(1500);
            }
        }
        public void Run() { ShowSplashScreen(); Action(); }
        //behavior could change
        public void Stop() { throw new Exception("Application Aborted By User"); }
        #endregion
    }
    #endregion
    #region Callback Delegates
    public delegate void CommandCallback(string argumentsText);
    #endregion
    #region interface ICommand
    public interface ICommand {
        string Text { get; }
        string HelpText { get; }
        string HelpSyntax { get; }
        bool HasArguments { get; }
        bool HasOptions { get; }
        void Execute(string rawArgs);
    }
    #endregion
    #region abstract class CommandBase
    public abstract class CommandBase : ICommand {
        #region Protected Fields
        protected string _text, _helpText, _helpSyntax;
        protected bool _hasArgs, _hasOptions;
        #endregion

        #region Constructors
        protected CommandBase(string text, string helpText, string helpSyntax, bool hasArguments, bool hasOptions) {
            _text = text; _helpText = helpText; _helpSyntax = helpSyntax; _hasArgs = hasArguments; _hasOptions = hasOptions;
        }
        protected CommandBase(string text, string helpText, string helpSyntax) {
            _text = text; _helpText = helpText; _helpSyntax = helpSyntax; _hasArgs = true; _hasOptions = true;
        }
        protected CommandBase(string text, string helpSyntax) {
            _text = text; _helpText = _helpSyntax = helpSyntax; _hasArgs = true; _hasOptions = true;
        }
        protected CommandBase(string text, string helpSyntax, bool hasArguments, bool hasOptions) {
            _text = text; _helpText = _helpSyntax = helpSyntax; _hasArgs = hasArguments; _hasOptions = hasOptions;
        }
        protected CommandBase(string text) {
            _text = text; _helpText = _helpSyntax = ""; _hasArgs = true; _hasOptions = true;
        }
        protected CommandBase(string text, bool hasArguments, bool hasOptions) {
            _text = text; _helpText = _helpSyntax = ""; _hasArgs = hasArguments; _hasOptions = hasOptions;
        }
        protected CommandBase() { _hasArgs = _hasOptions = true; _helpSyntax = _helpText = _text = ""; }
        #endregion

        #region Public Properties
        public string Text { get { return _text; } }
        public string HelpText { get { return _helpText; } }
        public string HelpSyntax { get { return _helpSyntax; } }
        public bool HasArguments { get { return _hasArgs; } }
        public bool HasOptions { get { return _hasOptions; } }
        #endregion

        #region Abstract Methods
        public abstract void Execute(string rawArgs);
        #endregion
    }
    #endregion
    #region instance class Command
    public class Command : CommandBase {
        #region Protected Fields
        protected CommandCallback _callback;
        #endregion

        #region Public Properties
        public CommandCallback Action { get { return _callback; } }
        #endregion

        #region Constructors
        protected Command() : base() { _callback = null; }
        protected Command(string text) : base(text) { _callback = null; }
        protected Command(CommandCallback action) : base() { _callback = action; }
        public Command(string text, CommandCallback action) : base(text) { _callback = action; }
        public Command(string text, CommandCallback action, bool hasArguments, bool hasOptions) : base(text, hasArguments, hasOptions) { _callback = action; }
        public Command(string text, CommandCallback action, string helpSyntax, bool hasArguments, bool hasOptions) : base(text, helpSyntax, hasArguments, hasOptions) { _callback = action; }
        public Command(string text, CommandCallback action, string helpSyntax) : base(text, helpSyntax) { _callback = action; }
        public Command(string text, CommandCallback action, string helpText, string helpSyntax) : base(text, helpText, helpSyntax) { _callback = action; }
        public Command(string text, CommandCallback action, string helpText, string helpSyntax, bool hasArguments, bool hasOptions) : base(text, helpText, helpSyntax, hasArguments, hasOptions) { _callback = action; }
        #endregion

        #region Public Overrides
        public override void Execute(string rawArgs) {
            if (_callback != null) { _callback(rawArgs); }
        }
        #endregion
    }
    #endregion
    #region class CommandPromptBase
    public class CommandPromptBase : Dictionary<string, ICommand>, IApplication {
        #region Protected Fields
        protected string _promptFormat;
        protected string _prompt;
        protected ConsoleColor _backColor, _foreColor;
        protected bool _caseSensitive;
        protected bool _showSplash;
        protected string _splashText;
        #endregion

        #region Public Properties
        public string Prompt { get { return _promptFormat; } set { _promptFormat = value; UpdatePrompt(); } }
        public string ActualPrompt { get { return _prompt; } }
        public ConsoleColor BackColor { get { return _backColor; } set { _backColor = value; } }
        public ConsoleColor ForeColor { get { return _foreColor; } set { _foreColor = value; } }
        public bool IsCaseSensitive { get { return _caseSensitive; } }
        public bool ShowSplash { get { return _showSplash; } set { _showSplash = value; } }
        public string SplashScreenText { get { return _splashText; } set { _splashText = value; } }
        #endregion

        #region Constructors
        public CommandPromptBase() : base() { _init("cmd\\> ", ConsoleColor.Black, ConsoleColor.White, true, false, ""); }
        public CommandPromptBase(int capacity) : base(capacity) { _init("cmd\\> ", ConsoleColor.Black, ConsoleColor.White, true, false, ""); }
        public CommandPromptBase(IEnumerable<ICommand> cmds)
            : base() {
            _init("cmd\\> ", ConsoleColor.Black, ConsoleColor.White, true, false, "");
            Add(cmds);
        }
        public CommandPromptBase(IEnumerable<Command> cmds)
            : base() {
            _init("cmd\\> ", ConsoleColor.Black, ConsoleColor.White, true, false, "");
            Add(cmds);
        }
        public CommandPromptBase(IEnumerable<ICommand> cmds, string promptFormat)
            : base() {
            _init(promptFormat, ConsoleColor.Black, ConsoleColor.White, true, false, "");
            Add(cmds);
        }
        public CommandPromptBase(IEnumerable<Command> cmds, string promptFormat)
            : base() {
            _init(promptFormat, ConsoleColor.Black, ConsoleColor.White, true, false, "");
            Add(cmds);
        }
        public CommandPromptBase(IEnumerable<ICommand> cmds, string promptFormat, bool caseSensetive)
            : base() {
            _init(promptFormat, ConsoleColor.Black, ConsoleColor.White, caseSensetive, false, "");
            Add(cmds);
        }
        public CommandPromptBase(IEnumerable<Command> cmds, string promptFormat, bool caseSensetive)
            : base() {
            _init(promptFormat, ConsoleColor.Black, ConsoleColor.White, caseSensetive, false, "");
            Add(cmds);
        }
        public CommandPromptBase(IEnumerable<ICommand> cmds, string promptFormat, bool caseSensetive, ConsoleColor back, ConsoleColor fore)
            : base() {
            _init(promptFormat, back, fore, caseSensetive, false, "");
            Add(cmds);
        }
        public CommandPromptBase(IEnumerable<Command> cmds, string promptFormat, bool caseSensetive, ConsoleColor back, ConsoleColor fore)
            : base() {
            _init(promptFormat, back, fore, caseSensetive, false, "");
            Add(cmds);
        }
        public CommandPromptBase(IEnumerable<ICommand> cmds, string promptFormat, bool caseSensetive, string splashText)
            : base() {
            _init(promptFormat, ConsoleColor.Black, ConsoleColor.White, caseSensetive, true, splashText);
            Add(cmds);
        }
        public CommandPromptBase(IEnumerable<Command> cmds, string promptFormat, bool caseSensetive, string splashText)
            : base() {
            _init(promptFormat, ConsoleColor.Black, ConsoleColor.White, caseSensetive, true, splashText);
            Add(cmds);
        }
        public CommandPromptBase(IEnumerable<ICommand> cmds, string promptFormat, bool caseSensetive, ConsoleColor back, ConsoleColor fore, string splashText)
            : base() {
            _init(promptFormat, back, fore, caseSensetive, true, splashText);
            Add(cmds);
        }
        public CommandPromptBase(IEnumerable<Command> cmds, string promptFormat, bool caseSensetive, ConsoleColor back, ConsoleColor fore, string splashText)
            : base() {
            _init(promptFormat, back, fore, caseSensetive, true, splashText);
            Add(cmds);
        }

        public CommandPromptBase(string promptFormat) : base() { _init(promptFormat, ConsoleColor.Black, ConsoleColor.White, true, false, ""); }
        public CommandPromptBase(string promptFormat, bool caseSensetive) : base() { _init(promptFormat, ConsoleColor.Black, ConsoleColor.White, caseSensetive, false, ""); }
        public CommandPromptBase(string promptFormat, bool caseSensetive, ConsoleColor back, ConsoleColor fore) : base() { _init(promptFormat, back, fore, caseSensetive, false, ""); }
        public CommandPromptBase(string promptFormat, bool caseSensetive, string splashText) : base() { _init(promptFormat, ConsoleColor.Black, ConsoleColor.White, caseSensetive, true, splashText); }
        public CommandPromptBase(string promptFormat, bool caseSensetive, ConsoleColor back, ConsoleColor fore, string splashText) : base() { _init(promptFormat, back, fore, caseSensetive, true, splashText); }
        private void _init(string promptFormat, ConsoleColor back, ConsoleColor fore, bool isCaseSensitive, bool showSplash, string splashText) {
            _promptFormat = promptFormat; _backColor = back; _foreColor = fore; _caseSensitive = isCaseSensitive; _showSplash = showSplash; _splashText = splashText;
            UpdatePrompt();
        }
        #endregion

        #region Protected Methods
        //Temporarily. Should be changed to parse format and calculate prompt from it
        protected void UpdatePrompt() {
            _prompt = _promptFormat;
        }
        protected void Cls() { Console.Clear(); }
        protected void WritePrompt() { Console.Write(_prompt); }
        protected void ShowCommandNotSupported(string cmd) { Console.WriteLine("The command you entered ({0}) was not found or is not supported.", cmd); }
        #endregion

        #region Public Methods
        //Hiding public dictionary add method to avoid adding incosistent data
        protected new void Add(string key, ICommand cmd) { base.Add(key, cmd); }
        //This Add should be used instead
        public void Add(ICommand cmd) { base.Add(_caseSensitive ? cmd.Text : cmd.Text.ToLower(), cmd); }
        //Overwriting Remove() to allow for case sensitivety
        public new bool Remove(string cmdName) { return _caseSensitive ? base.Remove(cmdName) : base.Remove(cmdName.ToLower()); }
        //Overwriting ContainsKey() to allow for case sensitivety
        public new bool ContainsKey(string cmdName) { return _caseSensitive ? base.ContainsKey(cmdName) : base.ContainsKey(cmdName.ToLower()); }
        //New methods
        public bool CommandSupported(string cmdName) { return _caseSensitive ? base.ContainsKey(cmdName) : base.ContainsKey(cmdName.ToLower()); }
        public void Add(IEnumerable<ICommand> cmds) {
            foreach (ICommand cmd in cmds) { base.Add(_caseSensitive ? cmd.Text : cmd.Text.ToLower(), cmd); }
        }
        public void Add(IEnumerable<Command> cmds) {
            foreach (Command cmd in cmds) { base.Add(_caseSensitive ? cmd.Text : cmd.Text.ToLower(), cmd); }
        }
        public string[] GetCommandsList() {
            string[] res = new string[Count];
            Keys.CopyTo(res, 0);
            return res;
        }
        public virtual void ShowSplashScreen() {
            if (_showSplash) {
                Console.BackgroundColor = _backColor; Console.ForegroundColor = _foreColor;
                Console.Clear();
                Console.WriteLine(_splashText);
                Thread.Sleep(1500);
            }
        }
        #endregion

        #region IApplication Methods
        public virtual void Run() {
            ShowSplashScreen();
            Console.BackgroundColor = _backColor; Console.ForegroundColor = _foreColor;
            Cls();
            bool run = true;
            while (run) {
                WritePrompt();
                string input = Console.ReadLine().Trim();
                if (!string.IsNullOrEmpty(input)) {
                    string command = "", args = "";
                    int space = input.IndexOf(' ');
                    command = space < 0 ? input : input.Substring(0, space);
                    args = space < 0 ? "" : input.Substring(space + 1);
                    if (!_caseSensitive) { command = command.ToLower(); }
                    if (CommandSupported(command)) { this[command].Execute(args); }
                    else {
                        switch (command) {
                            case "exit":
                            case "quit": run = false; break;
                            case "cls": Cls(); break;
                            case "prompt": Prompt = args; break;
                            default: ShowCommandNotSupported(command); break;
                        }
                    }
                }
            }
        }
        public virtual void Stop() { throw new Exception("Application Aborted By User!"); }
        #endregion
    }
    #endregion
}
