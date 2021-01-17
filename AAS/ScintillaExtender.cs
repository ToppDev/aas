using System;
using ScintillaNET;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using AutocompleteMenuNS;
using System.Collections;

namespace AAS
{
    class ScintillaExtender
    {
        Scintilla scintilla;
        private static Form1 mainWindow_;

        public ScintillaExtender(Scintilla scintilla, Form1 mainWindow)
        {
            mainWindow_ = mainWindow;
            this.scintilla = scintilla;
        }

        public void configure()
        {
            //scintilla.Lexer = Lexer.Cpp;

            // Set listeners
            scintilla.InsertCheck += new EventHandler<InsertCheckEventArgs>(scintilla_InsertCheck);
            scintilla.CharAdded += new EventHandler<CharAddedEventArgs>(scintilla_CharAdded);
            scintilla.TextChanged += new EventHandler(scintilla_TextChanged);
            scintilla.UpdateUI += new EventHandler<UpdateUIEventArgs>(scintilla_UpdateUI);

            // Displaying Line Numbers
            scintilla_TextChanged(null, null);
            scintilla.Margins[1].Width = 0;

            scintilla_AutomaticSyntaxHighlighting();

            scintilla_AutomaticCodeFolding();

            scintilla_BraceMatching();

            BuildAutocompleteMenu();
            //set as autocomplete source
            mainWindow_.autocompleteMenu1.SetAutocompleteItems(new DynamicCollection());
        }

        static List<string[]> referenceMethods;
        static List<string[]> keywords;
        static List<string[]> methods;
        static List<string[]> snippets;
        static List<string[]> declarationSnippets;
        private void BuildAutocompleteMenu()
        {
            #region ReferenceMethods

            // Keyword, ToolTipTitle, ToolTipText
            referenceMethods = new List<string[]>();
            referenceMethods.Add(new string[] { "KeyBoard(\"\");", "void KeyBoard(string text)", "Simulates keyboard input." });
            referenceMethods.Add(new string[] { "LClick();", "void LClick(int x, int y)", "Simulates a left click at the given position." });
            referenceMethods.Add(new string[] { "RClick();", "void RClick(int x, int y)", "Simulates a right click at the given position." });
            referenceMethods.Add(new string[] { "MClick();", "void MClick(int x, int y)", "Simulates a middle click at the given position." });
            referenceMethods.Add(new string[] { "LHold();", "void LHold(int xDown, int yDown, int xUp, int yUp)", "Simulates holding of the left mouse button between the given positions." });
            referenceMethods.Add(new string[] { "RHold();", "void RHold(int xDown, int yDown, int xUp, int yUp)", "Simulates holding of the right mouse button between the given positions." });
            referenceMethods.Add(new string[] { "MHold();", "void MHold(int xDown, int yDown, int xUp, int yUp)", "Simulates holding of the middle mouse button between the given positions." });
            referenceMethods.Add(new string[] { "LDoubleClick();", "void LDoubleClick(int x, int y)", "Simulates a left double click at the given position." });
            referenceMethods.Add(new string[] { "RDoubleClick();", "void RDoubleClick(int x, int y)", "Simulates a right double click at the given position." });
            referenceMethods.Add(new string[] { "MDoubleClick();", "void MDoubleClick(int x, int y)", "Simulates a middle double click at the given position." });
            referenceMethods.Add(new string[] { "Wait();", "void Wait(int milliseconds)", "Let the program wait the given amount of milliseconds." });
            referenceMethods.Add(new string[] { "WaitForColor();", "bool WaitForColor(int r, int g, int b, int x, int y)",
                                                                   "bool WaitForColor(int r, int g, int b, int x, int y, int timeout, int intervall)\n"
                                                                 + "Let the program wait till the given pixel at x,y has the given RGB color.\n"
                                                                 + "If timeout parameter is set, the program will continue after the given milliseconds and in this case the function will return false.\n"
                                                                 + "The intervall parameter sets the time for the checks." });
            referenceMethods.Add(new string[] { "WaitForActiveWindow(\"\");", "bool WaitForActiveWindow(string title)",
                                                    "bool WaitForActiveWindow(string title, int timeout, int intervall)\n"
                                                  + "Let the program wait till a window with the given name get's focused.\n"
                                                  + "If timeout parameter is set, the program will continue after the given milliseconds and in this case the function will return false.\n"
                                                  + "The intervall parameter sets the time for the checks." });
            referenceMethods.Add(new string[] { "WaitForKey(\"\");", "bool WaitForKey(string key)",
                                                    "bool WaitForKey(string key, int timeout, int intervall)\n"
                                                  + "Let the program wait till the user presses the specified key.\n"
                                                  + "If timeout parameter is set, the program will continue after the given milliseconds and in this case the function will return false.\n"
                                                  + "The intervall parameter sets the time for the checks."});
            referenceMethods.Add(new string[] { "GetActiveWindowTitle();", "string GetActiveWindowTitle()", "Returns the title of the active window." });
            referenceMethods.Add(new string[] { "Hook();", "Starts the keyboard hook", "This will automatically be called at program start." });
            referenceMethods.Add(new string[] { "UnHook();", "Stops the keyboard hook", "This will automatically be called at program end." });

            #endregion

            #region Keywords

            // Keyword, ToolTipTitle, ToolTipText
            keywords = new List<string[]>();
            keywords.Add(new string[] { "abstract", "", "" });
            keywords.Add(new string[] { "as", "", "" });
            keywords.Add(new string[] { "base", "", "" });
            keywords.Add(new string[] { "bool", "bool-keyword", "" });
            keywords.Add(new string[] { "break", "", "" });
            keywords.Add(new string[] { "byte", "", "" });
            keywords.Add(new string[] { "case", "", "" });
            keywords.Add(new string[] { "catch", "", "" });
            keywords.Add(new string[] { "char", "", "" });
            keywords.Add(new string[] { "checked", "", "" });
            keywords.Add(new string[] { "class", "", "" });
            keywords.Add(new string[] { "const", "", "" });
            keywords.Add(new string[] { "continue", "", "" });
            keywords.Add(new string[] { "decimal", "", "" });
            keywords.Add(new string[] { "default", "", "" });
            keywords.Add(new string[] { "delegate", "", "" });
            keywords.Add(new string[] { "do", "", "" });
            keywords.Add(new string[] { "double", "", "" });
            keywords.Add(new string[] { "else", "", "" });
            keywords.Add(new string[] { "enum", "", "" });
            keywords.Add(new string[] { "event", "", "" });
            keywords.Add(new string[] { "explicit", "", "" });
            keywords.Add(new string[] { "extern", "", "" });
            keywords.Add(new string[] { "false", "", "" });
            keywords.Add(new string[] { "finally", "", "" });
            keywords.Add(new string[] { "fixed", "", "" });
            keywords.Add(new string[] { "float", "", "" });
            keywords.Add(new string[] { "for", "", "" });
            keywords.Add(new string[] { "foreach", "", "" });
            keywords.Add(new string[] { "goto", "", "" });
            keywords.Add(new string[] { "if", "", "" });
            keywords.Add(new string[] { "implicit", "", "" });
            keywords.Add(new string[] { "in", "", "" });
            keywords.Add(new string[] { "int", "", "" });
            keywords.Add(new string[] { "interface", "", "" });
            keywords.Add(new string[] { "internal", "", "" });
            keywords.Add(new string[] { "is", "", "" });
            keywords.Add(new string[] { "lock", "", "" });
            keywords.Add(new string[] { "long", "", "" });
            keywords.Add(new string[] { "namespace", "", "" });
            keywords.Add(new string[] { "new", "", "" });
            keywords.Add(new string[] { "null", "", "" });
            keywords.Add(new string[] { "object", "", "" });
            keywords.Add(new string[] { "operator", "", "" });
            keywords.Add(new string[] { "out", "", "" });
            keywords.Add(new string[] { "override", "", "" });
            keywords.Add(new string[] { "params", "", "" });
            keywords.Add(new string[] { "private", "", "" });
            keywords.Add(new string[] { "protected", "", "" });
            keywords.Add(new string[] { "public", "", "" });
            keywords.Add(new string[] { "readonly", "", "" });
            keywords.Add(new string[] { "ref", "", "" });
            keywords.Add(new string[] { "return", "", "" });
            keywords.Add(new string[] { "sbyte", "", "" });
            keywords.Add(new string[] { "sealed", "", "" });
            keywords.Add(new string[] { "short", "", "" });
            keywords.Add(new string[] { "sizeof", "", "" });
            keywords.Add(new string[] { "stackalloc", "", "" });
            keywords.Add(new string[] { "static", "", "" });
            keywords.Add(new string[] { "string", "", "" });
            keywords.Add(new string[] { "struct", "", "" });
            keywords.Add(new string[] { "switch", "", "" });
            keywords.Add(new string[] { "this", "", "" });
            keywords.Add(new string[] { "throw", "", "" });
            keywords.Add(new string[] { "true", "", "" });
            keywords.Add(new string[] { "try", "", "" });
            keywords.Add(new string[] { "typeof", "", "" });
            keywords.Add(new string[] { "uint", "", "" });
            keywords.Add(new string[] { "ulong", "", "" });
            keywords.Add(new string[] { "unchecked", "", "" });
            keywords.Add(new string[] { "unsafe", "", "" });
            keywords.Add(new string[] { "ushort", "", "" });
            keywords.Add(new string[] { "using", "", "" });
            keywords.Add(new string[] { "virtual", "", "" });
            keywords.Add(new string[] { "void", "", "" });
            keywords.Add(new string[] { "volatile", "", "" });
            keywords.Add(new string[] { "while", "", "" });
            keywords.Add(new string[] { "add", "", "" });
            keywords.Add(new string[] { "alias", "", "" });
            keywords.Add(new string[] { "ascending", "", "" });
            keywords.Add(new string[] { "descending", "", "" });
            keywords.Add(new string[] { "dynamic", "", "" });
            keywords.Add(new string[] { "from", "", "" });
            keywords.Add(new string[] { "get", "", "" });
            keywords.Add(new string[] { "global", "", "" });
            keywords.Add(new string[] { "group", "", "" });
            keywords.Add(new string[] { "into", "", "" });
            keywords.Add(new string[] { "join", "", "" });
            keywords.Add(new string[] { "let", "", "" });
            keywords.Add(new string[] { "orderby", "", "" });
            keywords.Add(new string[] { "partial", "", "" });
            keywords.Add(new string[] { "remove", "", "" });
            keywords.Add(new string[] { "select", "", "" });
            keywords.Add(new string[] { "set", "", "" });
            keywords.Add(new string[] { "value", "", "" });
            keywords.Add(new string[] { "var", "", "" });
            keywords.Add(new string[] { "where", "", "" });
            keywords.Add(new string[] { "yield", "", "" });

            #endregion

            #region Methods

            // Keyword, ToolTipTitle, ToolTipText
            methods = new List<string[]>();
            methods.Add(new string[] { "Equals()", "", "" });
            methods.Add(new string[] { "GetHashCode()", "", "" });
            methods.Add(new string[] { "GetType()", "", "" });
            methods.Add(new string[] { "ToString()", "", "" });

            #endregion

            #region Snippets

            // Keyword, ToolTipTitle, ToolTipText
            snippets = new List<string[]>();
            snippets.Add(new string[] { "if (^) {}", "", "" });
            snippets.Add(new string[] { "if (^) {} else {}", "", "" });
            snippets.Add(new string[] { "for (^;;) {}", "", "" });
            snippets.Add(new string[] { "while (^) {}", "", "" });
            snippets.Add(new string[] { "do {^} while ();", "", "" });
            snippets.Add(new string[] { "switch (^) {case : break;}", "", "" });

            #endregion

            #region declarationSnippets

            // Keyword, ToolTipTitle, ToolTipText
            declarationSnippets = new List<string[]>();
            declarationSnippets.Add(new string[] { "public class ^{}", "", "" });
            declarationSnippets.Add(new string[] { "private class ^{}", "", "" });
            declarationSnippets.Add(new string[] { "internal class ^{}", "", "" });
            declarationSnippets.Add(new string[] { "public struct ^{}", "", "" });
            declarationSnippets.Add(new string[] { "private struct ^{}", "", "" });
            declarationSnippets.Add(new string[] { "internal struct ^{}", "", "" });
            declarationSnippets.Add(new string[] { "public void ^() {}", "", "" });
            declarationSnippets.Add(new string[] { "private void ^() {}", "", "" });
            declarationSnippets.Add(new string[] { "internal void ^() {}", "", "" });
            declarationSnippets.Add(new string[] { "protected void ^() {}", "", "" });
            declarationSnippets.Add(new string[] { "public ^{ get; set; }", "", "" });
            declarationSnippets.Add(new string[] { "private ^{ get; set; }", "", "" });
            declarationSnippets.Add(new string[] { "internal ^{ get; set; }", "", "" });
            declarationSnippets.Add(new string[] { "protected ^{ get; set; }", "", "" });

            #endregion
        }

        /// <summary>
        /// This item appears when any part of snippet text is typed
        /// </summary>
        class DeclarationSnippet : SnippetAutocompleteItem
        {
            public static string RegexSpecSymbolsPattern = @"[\^\$\[\]\(\)\.\\\*\+\|\?\{\}]";

            public DeclarationSnippet(string snippet)
                : base(snippet)
            {
            }

            public override CompareResult Compare(string fragmentText)
            {
                var pattern = Regex.Replace(fragmentText, RegexSpecSymbolsPattern, "\\$0");
                if (Regex.IsMatch(Text, "\\b" + pattern, RegexOptions.IgnoreCase))
                    return CompareResult.Visible;
                return CompareResult.Hidden;
            }
        }

        /// <summary>
        /// Divides numbers and words: "123AND456" -> "123 AND 456"
        /// Or "i=2" -> "i = 2"
        /// </summary>
        class InsertSpaceSnippet : AutocompleteItem
        {
            string pattern;

            public InsertSpaceSnippet(string pattern)
                : base("")
            {
                this.pattern = pattern;
            }

            public InsertSpaceSnippet()
                : this(@"^(\d+)([a-zA-Z_]+)(\d*)$")
            {
            }

            public override CompareResult Compare(string fragmentText)
            {
                if (Regex.IsMatch(fragmentText, pattern))
                {
                    Text = InsertSpaces(fragmentText);
                    if (Text != fragmentText)
                        return CompareResult.Visible;
                }
                return CompareResult.Hidden;
            }

            public string InsertSpaces(string fragment)
            {
                var m = Regex.Match(fragment, pattern);
                if (m.Groups[1].Value == "" && m.Groups[3].Value == "")
                    return fragment;
                return (m.Groups[1].Value + " " + m.Groups[2].Value + " " + m.Groups[3].Value).Trim();
            }

            public override string ToolTipTitle
            {
                get
                {
                    return Text;
                }
            }
        }
        
        internal class DynamicCollection : IEnumerable<AutocompleteItem>
        {
            public IEnumerator<AutocompleteItem> GetEnumerator()
            {
                return BuildList().GetEnumerator();
            }

            private IEnumerable<AutocompleteItem> BuildList()
            {
                // find all words of the text
                var words = new Dictionary<string, string>();
                if (mainWindow_.scintilla.Text != null)
                    foreach (Match m in Regex.Matches(mainWindow_.scintilla.Text, @"\b\w+\b"))
                    {
                        bool contains = false;
                        foreach (var item in keywords)
                            if (item[0] == m.Value)
                            {
                                contains = true;
                                break;
                            }
                        if (!contains)
                            foreach (var item in referenceMethods)
                                if (item[0] == m.Value)
                                {
                                    contains = true;
                                    break;
                                }
                        if (!contains)
                            foreach (var item in methods)
                                if (item[0] == m.Value)
                                {
                                    contains = true;
                                    break;
                                }
                        if (!contains)
                            foreach (var item in snippets)
                                if (item[0] == m.Value)
                                {
                                    contains = true;
                                    break;
                                }
                        if (!contains)
                            foreach (var item in declarationSnippets)
                                if (item[0] == m.Value)
                                {
                                    contains = true;
                                    break;
                                }
                        if (!contains)
                            words[m.Value] = m.Value;
                    }

                // static members
                foreach (var item in referenceMethods)
                    yield return new AutocompleteItem(item[0]) { ToolTipTitle = item[1], ToolTipText = item[2] };
                foreach (var item in snippets)
                    yield return new SnippetAutocompleteItem(item[0]) { ImageIndex = 1, ToolTipTitle = item[1], ToolTipText = item[2] };
                foreach (var item in declarationSnippets)
                    yield return new DeclarationSnippet(item[0]) { ImageIndex = 0, ToolTipTitle = item[1], ToolTipText = item[2] };
                foreach (var item in methods)
                    yield return new MethodAutocompleteItem(item[0]) { ImageIndex = 2, ToolTipTitle = item[1], ToolTipText = item[2] };
                foreach (var item in keywords)
                    yield return new AutocompleteItem(item[0]) { ToolTipTitle = item[1], ToolTipText = item[2] };

                yield return new InsertSpaceSnippet();
                yield return new InsertSpaceSnippet(@"^(\w+)([=<>!:]+)(\w+)$");

                // dynamic members
                foreach (var word in words.Keys)
                    yield return new AutocompleteItem(word);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        private void scintilla_AutomaticSyntaxHighlighting()
        {
            // Configuring the default style with properties
            // we have common to every lexer style saves time.
            scintilla.StyleResetDefault();
            scintilla.Styles[Style.Default].Font = "Consolas";
            scintilla.Styles[Style.Default].Size = 10;

            // Configure the CPP (C#) lexer styles
            scintilla.Styles[Style.Cpp.Default].ForeColor = Color.Black;
            scintilla.Styles[Style.Cpp.Comment].ForeColor = Color.FromArgb(0, 128, 0); // Green
            scintilla.Styles[Style.Cpp.CommentLine].ForeColor = Color.FromArgb(0, 128, 0); // Green
            scintilla.Styles[Style.Cpp.CommentLineDoc].ForeColor = Color.FromArgb(128, 128, 128); // Gray
            scintilla.Styles[Style.Cpp.Number].ForeColor = Color.Olive;
            scintilla.Styles[Style.Cpp.Word].ForeColor = Color.Blue;
            scintilla.Styles[Style.Cpp.Word2].ForeColor = Color.Blue;
            scintilla.Styles[Style.Cpp.String].ForeColor = Color.FromArgb(163, 21, 21); // Red
            scintilla.Styles[Style.Cpp.Character].ForeColor = Color.FromArgb(163, 21, 21); // Red
            scintilla.Styles[Style.Cpp.Verbatim].ForeColor = Color.FromArgb(163, 21, 21); // Red
            scintilla.Styles[Style.Cpp.StringEol].BackColor = Color.Pink;
            scintilla.Styles[Style.Cpp.Operator].ForeColor = Color.Purple;
            scintilla.Styles[Style.Cpp.Preprocessor].ForeColor = Color.Maroon;

            // Set the keywords
            scintilla.SetKeywords(0, "abstract as base break case catch checked continue default delegate do else event explicit extern false finally fixed for foreach goto if implicit in interface internal is lock namespace new null object operator out override params private protected public readonly ref return sealed sizeof stackalloc switch this throw true try typeof unchecked unsafe using virtual while");
            scintilla.SetKeywords(1, "bool byte char class const decimal double enum float int long sbyte short static string struct uint ulong ushort void");
        }

        private void scintilla_AutomaticCodeFolding()
        {
            // Instruct the lexer to calculate folding
            scintilla.SetProperty("fold", "1");
            scintilla.SetProperty("fold.compact", "1");

            // Configure a margin to display folding symbols
            scintilla.Margins[2].Type = MarginType.Symbol;
            scintilla.Margins[2].Mask = Marker.MaskFolders;
            scintilla.Margins[2].Sensitive = true;
            scintilla.Margins[2].Width = 20;

            // Set colors for all folding markers
            for (int i = 25; i <= 31; i++)
            {
                scintilla.Markers[i].SetForeColor(SystemColors.ControlLightLight);
                scintilla.Markers[i].SetBackColor(SystemColors.ControlDark);
            }

            // Configure folding markers with respective symbols
            scintilla.Markers[Marker.Folder].Symbol = MarkerSymbol.BoxPlus;
            scintilla.Markers[Marker.FolderOpen].Symbol = MarkerSymbol.BoxMinus;
            scintilla.Markers[Marker.FolderEnd].Symbol = MarkerSymbol.BoxPlusConnected;
            scintilla.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            scintilla.Markers[Marker.FolderOpenMid].Symbol = MarkerSymbol.BoxMinusConnected;
            scintilla.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            scintilla.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            // Enable automatic folding
            scintilla.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);
        }

        private void scintilla_BraceMatching()
        {
            scintilla.IndentationGuides = IndentView.LookBoth;
            scintilla.Styles[Style.BraceLight].BackColor = Color.LightGray;
            scintilla.Styles[Style.BraceLight].ForeColor = Color.BlueViolet;
            scintilla.Styles[Style.BraceBad].ForeColor = Color.Red;
        }

        int lastCaretPos = 0;
        private static bool IsBrace(int c)
        {
            switch (c)
            {
                case '(':
                case ')':
                case '[':
                case ']':
                case '{':
                case '}':
                case '<':
                case '>':
                    return true;
            }

            return false;
        }
        private void scintilla_UpdateUI(object sender, UpdateUIEventArgs e)
        {
            //mainWindow_.Text = scintilla.CurrentPosition.ToString();

            // Has the caret changed position? (BraceHighlightning)
            var caretPos = scintilla.CurrentPosition;
            if (lastCaretPos != caretPos)
            {
                lastCaretPos = caretPos;
                var bracePos1 = -1;
                var bracePos2 = -1;

                // Is there a brace to the left or right?
                if (caretPos > 0 && IsBrace(scintilla.GetCharAt(caretPos - 1)))
                    bracePos1 = (caretPos - 1);
                else if (IsBrace(scintilla.GetCharAt(caretPos)))
                    bracePos1 = caretPos;

                if (bracePos1 >= 0)
                {
                    // Find the matching brace
                    bracePos2 = scintilla.BraceMatch(bracePos1);
                    if (bracePos2 == Scintilla.InvalidPosition)
                    {
                        scintilla.BraceBadLight(bracePos1);
                        scintilla.HighlightGuide = 0;
                    }
                    else
                    {
                        scintilla.BraceHighlight(bracePos1, bracePos2);
                        scintilla.HighlightGuide = scintilla.GetColumn(bracePos1);
                    }
                }
                else
                {
                    // Turn off brace matching
                    scintilla.BraceHighlight(Scintilla.InvalidPosition, Scintilla.InvalidPosition);
                    scintilla.HighlightGuide = 0;
                }
            }
        }

        private void scintilla_InsertCheck(object sender, InsertCheckEventArgs e)
        {
            if ((e.Text.EndsWith("\r") || e.Text.EndsWith("\n"))) // Code Indention
            {
                int startPos = scintilla.Lines[scintilla.LineFromPosition(scintilla.CurrentPosition)].Position;
                int endPos = e.Position;
                string curLineText = scintilla.GetTextRange(startPos, (endPos - startPos)); //Text until the caret so that the whitespace is always equal in every line.

                Match indent = Regex.Match(curLineText, "^[ \\t]*");
                e.Text = (e.Text + indent.Value);
                if (Regex.IsMatch(curLineText, "{\\s*$"))
                {
                    for (int i = 0; i < scintilla.TabWidth; i++)
                        e.Text += ' '; // Add tab
                    
                }
            }
        }

        private void scintilla_CharAdded(object sender, CharAddedEventArgs e)
        {
            //The '}' char.
            if (e.Char == 125) // Code Indention
            {
                int curLine = scintilla.LineFromPosition(scintilla.CurrentPosition);

                if (scintilla.Lines[curLine].Text.Trim() == "}")
                { //Check whether the bracket is the only thing on the line.. For cases like "if() { }".
                    SetIndent(scintilla, curLine, GetIndent(scintilla, curLine) - 4);
                }
            }
            //else
            //{
            //    // Find the word start
            //    var currentPos = scintilla.CurrentPosition;
            //    var wordStartPos = scintilla.WordStartPosition(currentPos, true);

            //    // Display the autocompletion list
            //    var lenEntered = currentPos - wordStartPos;
            //    if (lenEntered > 0)
            //    {
            //        scintilla.AutoCShow(lenEntered, "abstract|as|base|break|case|catch|checked"
            //            + "|continue|default|delegate|do|else|event|explicit|extern|false|finally"
            //            + "|fixed|for|foreach|goto|if|implicit|in|interface|internal|is|lock|namespace"
            //            + "|new|null|object|operator|out|override|params|private|protected|public|readonly"
            //            + "|ref|return|sealed|sizeof|stackalloc|switch|this|throw|true|try|typeof|unchecked"
            //            + "|unsafe|using|virtual|while");
            //    }
            //}
        }

        private int maxLineNumberCharLength;
        private void scintilla_TextChanged(object sender, EventArgs e)
        {
            mainWindow_.resetCompiler();

            // Did the number of characters in the line number display change?
            // i.e. nnn VS nn, or nnnn VS nn, etc...
            var maxLineNumberCharLength = scintilla.Lines.Count.ToString().Length;
            if (maxLineNumberCharLength == this.maxLineNumberCharLength)
                return;

            // Calculate the width required to display the last line number
            // and include some padding for good measure.
            const int padding = -1;
            scintilla.Margins[0].Width = scintilla.TextWidth(Style.LineNumber, new string('9', maxLineNumberCharLength + 1)) + padding;
            this.maxLineNumberCharLength = maxLineNumberCharLength;
        }

        //Codes for the handling the Indention of the lines.
        //They are manually added here until they get officially added to the Scintilla control.
        #region "CodeIndent Handlers"
        // https://github.com/jacobslusser/ScintillaNET/issues/35
        // https://gist.github.com/JohnyMac/f2910192987a73a52ab4

        const int SCI_SETLINEINDENTATION = 2126;
        const int SCI_GETLINEINDENTATION = 2127;
        private void SetIndent(ScintillaNET.Scintilla scin, int line, int indent)
        {
            scin.DirectMessage(SCI_SETLINEINDENTATION, new IntPtr(line), new IntPtr(indent));
        }
        private int GetIndent(ScintillaNET.Scintilla scin, int line)
        {
            return (scin.DirectMessage(SCI_GETLINEINDENTATION, new IntPtr(line), (IntPtr)null).ToInt32());
        }
        #endregion
    }
}
