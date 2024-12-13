open System
open System.Windows.Forms
open System.Drawing
open System.IO

type Form2() as this =
    inherit Form()

    // UI Components
    let panelHeader = new Panel()
    let labelTitle = new Label()
    let panelSidebar = new Panel()
    let buttonLoadFile = new Button()
    let buttonAnalyze = new Button()
    let buttonClear = new Button()
    let panelContent = new Panel()
    let textBoxInput = new TextBox()
    let labelInput = new Label()
    let labelOutput = new Label()
    let pictureBoxIcon = new PictureBox()

    
    let createPanelWithLabel text location size color =
        let panel = new Panel(Size = size, Location = location, BackColor = color)
        let label = new Label(Text = text, Font = new Font("Century Gothic", 12.0F), AutoSize = true, Location = Point(5, 16))
        panel.Controls.Add(label)
        panel


    let analyzeText (text: string) =

        let paragraphs = text.Split([|'\n'; '\r'|], StringSplitOptions.RemoveEmptyEntries)
        
        let words = 
            text.Split([|' '; '\n'; '\t'; '.'; ','; ';'; ':'; '!' ;' '|], StringSplitOptions.RemoveEmptyEntries)
            |> Array.filter (fun word -> not (String.IsNullOrWhiteSpace(word))) 

        let sentences = text.Split([|'.'; '!'; '?'|], StringSplitOptions.RemoveEmptyEntries)

        let wordCount = words.Length
        let sentenceCount = sentences.Length
        let paragraphCount = paragraphs.Length

        let avgSentenceLength = if sentenceCount > 0 then wordCount / sentenceCount else 0

        let wordFrequency =
            words
            |> Seq.map (fun word -> word.ToLowerInvariant())
            |> Seq.groupBy id
            |> Seq.map (fun (word, occurrences) -> word, Seq.length occurrences)
            |> Seq.sortByDescending snd
            |> Seq.toList

        wordCount, sentenceCount, paragraphCount, avgSentenceLength, wordFrequency


        buttonAnalyze.Click.Add(fun _ -> 
    let text = textBoxInput.Text
    let wordCount, sentenceCount, paragraphCount, avgSentenceLength, wordFrequency = analyzeText text

    let updateLabel (panel: Panel) newText =
        let label = panel.Controls.[0] :?> Label
        label.Text <- newText

    updateLabel panel1 $"    {avgSentenceLength}"
    updateLabel panel2 $"    {paragraphCount}"
    updateLabel panel3 $"    {wordCount}"
    updateLabel panel5 $"    {sentenceCount}"

    let frequentWordsText =
        wordFrequency
        |> List.truncate 5
        |> List.map (fun (word, count) -> $"    {word}:   {count}")
        |> String.concat "\n\n"

    updateLabel panel6 $"{frequentWordsText}"
)

    let panel1Label = new Label(Text = "Readability Score:", Font = new Font("Century Gothic", 12.0F), AutoSize = true, Location = Point(276, 357))
    let panel2Label = new Label(Text = "Paragraph Count:", Font = new Font("Century Gothic", 12.0F), AutoSize = true, Location = Point(25, 357))
    let panel3Label = new Label(Text = "Word Count:", Font = new Font("Century Gothic", 12.0F), AutoSize = true, Location = Point(25, 234))
    let panel5Label = new Label(Text = "Sentence Count:", Font = new Font("Century Gothic", 12.0F), AutoSize = true, Location = Point(276, 234))
    let panel6Label = new Label(Text = "Most Frequent Words:", Font = new Font("Century Gothic", 12.0F), AutoSize = true, Location = Point(533, 234))

    // Panels for output
    let panel1 = createPanelWithLabel "" (Point(276, 387)) (Size(232, 90)) (Color.FromArgb(230, 230, 255))
    let panel2 = createPanelWithLabel "" (Point(25, 387)) (Size(232, 90)) (ColorTranslator.FromHtml("#eaeded"))
    let panel3 = createPanelWithLabel "" (Point(25, 264)) (Size(232, 90)) (Color.FromArgb(255, 230, 230))
    let panel5 = createPanelWithLabel "" (Point(276, 264)) (Size(232, 90)) (ColorTranslator.FromHtml("#c3eafb"))
    let panel6 = createPanelWithLabel ""  (Point(533, 264)) (Size(355, 210)) (ColorTranslator.FromHtml("#dafada"))

    
    do
        // Header
        panelHeader.BackColor <- ColorTranslator.FromHtml("#367CAF")
        panelHeader.Dock <- DockStyle.Top
        panelHeader.Size <- Size(1100, 80)
        labelTitle.Text <- "Text Analyzer"
        labelTitle.Font <- new Font("Century Gothic", 20.0F)
        labelTitle.ForeColor <- Color.White
        labelTitle.AutoSize <- true
        labelTitle.TextAlign <- ContentAlignment.MiddleRight
        labelTitle.Location <- Point(20, 20)
        panelHeader.Controls.Add(labelTitle)

        // Sidebar
        panelSidebar.BackColor <- ColorTranslator.FromHtml("#ece0d0")
        panelSidebar.Dock <- DockStyle.Left
        panelSidebar.Size <- Size(200, 646)

        buttonLoadFile.Text <- "Load File"
        buttonLoadFile.Font <- new Font("Century Gothic", 14.0F)
        buttonLoadFile.Size <- Size(200, 81)
        buttonLoadFile.FlatStyle <- FlatStyle.Flat
        buttonLoadFile.FlatAppearance.BorderSize <- 0
        panelSidebar.Controls.Add(buttonLoadFile)

        buttonAnalyze.Text <- "Analyze"
        buttonAnalyze.Font <- new Font("Century Gothic", 14.0F)
        buttonAnalyze.Size <- Size(200, 81)
        buttonAnalyze.FlatStyle <- FlatStyle.Flat
        buttonAnalyze.FlatAppearance.BorderSize <- 0
        buttonAnalyze.Location <- Point(0, 87)
        panelSidebar.Controls.Add(buttonAnalyze)

        buttonClear.Text <- "Clear"
        buttonClear.Font <- new Font("Century Gothic", 14.0F)
        buttonClear.Size <- Size(200, 81)
        buttonClear.FlatStyle <- FlatStyle.Flat
        buttonClear.FlatAppearance.BorderSize <- 0
        buttonClear.Location <- Point(0, 174)
        panelSidebar.Controls.Add(buttonClear)

        // Content
        panelContent.BackColor <- ColorTranslator.FromHtml("#fcfcf4")
        panelContent.Dock <- DockStyle.Fill

        labelInput.Text <- "Input Text:"
        labelInput.Font <- new Font("Century Gothic", 12.0F)
        labelInput.Location <- Point(20, 20)

        textBoxInput.Font <- new Font("Century Gothic", 10.0F)
        textBoxInput.Multiline <- true
        textBoxInput.ScrollBars <- ScrollBars.Vertical
        textBoxInput.Size <- Size(660, 137)
        textBoxInput.Location <- Point(20, 50)

        
        panelContent.Controls.AddRange([| panel1Label; panel2Label; panel3Label; panel5Label; panel6Label |])
        panelContent.Controls.AddRange([| panel1; panel2; panel3; panel5; panel6 |])
        panelContent.Controls.Add(labelInput)
        panelContent.Controls.Add(textBoxInput)

        // Icon
        pictureBoxIcon.Location <- Point(730, 50)
        pictureBoxIcon.Size <- Size(140, 137)
        pictureBoxIcon.Image <- Image.FromFile("1.png")
        pictureBoxIcon.SizeMode <- PictureBoxSizeMode.StretchImage
        panelContent.Controls.Add(pictureBoxIcon)

        
        this.Controls.Add(panelContent)
        this.Controls.Add(panelSidebar)
        this.Controls.Add(panelHeader)
        this.ClientSize <- Size(1130, 600)
        this.Text <- "Text Analyzer"
        this.StartPosition <- FormStartPosition.CenterScreen

        // Button event handlers
        buttonLoadFile.Click.Add(fun _ -> 
            use openFileDialog = new OpenFileDialog(Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*", Title = "Open Text File")
            if openFileDialog.ShowDialog() = DialogResult.OK then
                textBoxInput.Text <- File.ReadAllText(openFileDialog.FileName)
        )

        buttonAnalyze.Click.Add(fun _ -> 
          if String.IsNullOrWhiteSpace(textBoxInput.Text) then
                ignore (System.Windows.Forms.MessageBox.Show("Please enter text or load a file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error))
          else    
              let text = textBoxInput.Text
              let wordCount, sentenceCount, paragraphCount, avgSentenceLength, wordFrequency = analyzeText text
            
              let updateLabel (panel: Panel) newText =
                  let label = panel.Controls.[0] :?> Label
                  label.Text <- newText
            
              updateLabel panel1 $"    {avgSentenceLength}"
              updateLabel panel2 $"    {paragraphCount}"
              updateLabel panel3 $"    {wordCount}"
              updateLabel panel5 $"    {sentenceCount}"
            
              let frequentWordsText =
                  wordFrequency
                  |> List.truncate 5
                  |> List.map (fun (word, count) -> $"    {word}:   {count}")
                  |> String.concat "\n\n"
            
              updateLabel panel6 $"{frequentWordsText}"
        )


        buttonClear.Click.Add(fun _ -> 
            textBoxInput.Clear()
            [panel1; panel2; panel3; panel5; panel6]
            |> List.iter (fun panel -> 
                let label = panel.Controls.[0] :?> Label
                label.Text <- ""
            )
        )

[<STAThread>]
[<EntryPoint>]
let main argv =
    Application.EnableVisualStyles()
    Application.SetCompatibleTextRenderingDefault(false)
    Application.Run(new Form2())
    0
