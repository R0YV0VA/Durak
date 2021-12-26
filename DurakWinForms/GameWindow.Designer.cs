namespace DurakWinForms
{
  partial class GameWindow
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameWindow));
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.gameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ServerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.IamZone = new System.Windows.Forms.GroupBox();
            this.Gamer3Zone = new System.Windows.Forms.GroupBox();
            this.GameField = new System.Windows.Forms.GroupBox();
            this.DeckZone = new System.Windows.Forms.GroupBox();
            this.DeckBack = new System.Windows.Forms.PictureBox();
            this.TrumpCard = new System.Windows.Forms.PictureBox();
            this.TrumpImage = new System.Windows.Forms.PictureBox();
            this.Gamer2Zone = new System.Windows.Forms.GroupBox();
            this.Gamer1Zone = new System.Windows.Forms.GroupBox();
            this.Gamer4Zone = new System.Windows.Forms.GroupBox();
            this.Gamer5Zone = new System.Windows.Forms.GroupBox();
            this.endRoundBtn = new System.Windows.Forms.Button();
            this.takeCardsBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gameStateTb = new System.Windows.Forms.TextBox();
            this.mainMenu.SuspendLayout();
            this.DeckZone.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DeckBack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrumpCard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrumpImage)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gameMenuItem,
            this.ServerMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(1179, 28);
            this.mainMenu.TabIndex = 0;
            this.mainMenu.Text = "menuStrip1";
            // 
            // gameMenuItem
            // 
            this.gameMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newGameToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.gameMenuItem.Name = "gameMenuItem";
            this.gameMenuItem.Size = new System.Drawing.Size(47, 24);
            this.gameMenuItem.Text = "Гра";
            // 
            // newGameToolStripMenuItem
            // 
            this.newGameToolStripMenuItem.Enabled = false;
            this.newGameToolStripMenuItem.Name = "newGameToolStripMenuItem";
            this.newGameToolStripMenuItem.Size = new System.Drawing.Size(155, 26);
            this.newGameToolStripMenuItem.Text = "Нова гра";
            this.newGameToolStripMenuItem.Click += new System.EventHandler(this.newGameToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(155, 26);
            this.exitToolStripMenuItem.Text = "Вихід";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // ServerMenuItem
            // 
            this.ServerMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startServerToolStripMenuItem,
            this.connectToServerToolStripMenuItem});
            this.ServerMenuItem.Name = "ServerMenuItem";
            this.ServerMenuItem.Size = new System.Drawing.Size(74, 24);
            this.ServerMenuItem.Text = "Сервер";
            // 
            // startServerToolStripMenuItem
            // 
            this.startServerToolStripMenuItem.Name = "startServerToolStripMenuItem";
            this.startServerToolStripMenuItem.Size = new System.Drawing.Size(261, 26);
            this.startServerToolStripMenuItem.Text = "Створити сервер";
            this.startServerToolStripMenuItem.Click += new System.EventHandler(this.startServerToolStripMenuItem_Click_1);
            // 
            // connectToServerToolStripMenuItem
            // 
            this.connectToServerToolStripMenuItem.Name = "connectToServerToolStripMenuItem";
            this.connectToServerToolStripMenuItem.Size = new System.Drawing.Size(261, 26);
            this.connectToServerToolStripMenuItem.Text = "Під\'єднатися до серверу";
            this.connectToServerToolStripMenuItem.Click += new System.EventHandler(this.connectToServerToolStripMenuItem_Click);
            // 
            // IamZone
            // 
            this.IamZone.Location = new System.Drawing.Point(172, 528);
            this.IamZone.Margin = new System.Windows.Forms.Padding(4);
            this.IamZone.Name = "IamZone";
            this.IamZone.Padding = new System.Windows.Forms.Padding(4);
            this.IamZone.Size = new System.Drawing.Size(823, 148);
            this.IamZone.TabIndex = 2;
            this.IamZone.TabStop = false;
            this.IamZone.Text = "Я";
            // 
            // Gamer3Zone
            // 
            this.Gamer3Zone.Location = new System.Drawing.Point(172, 33);
            this.Gamer3Zone.Margin = new System.Windows.Forms.Padding(4);
            this.Gamer3Zone.Name = "Gamer3Zone";
            this.Gamer3Zone.Padding = new System.Windows.Forms.Padding(4);
            this.Gamer3Zone.Size = new System.Drawing.Size(823, 148);
            this.Gamer3Zone.TabIndex = 3;
            this.Gamer3Zone.TabStop = false;
            this.Gamer3Zone.Text = "Гравець 3";
            // 
            // GameField
            // 
            this.GameField.Location = new System.Drawing.Point(172, 188);
            this.GameField.Margin = new System.Windows.Forms.Padding(4);
            this.GameField.Name = "GameField";
            this.GameField.Padding = new System.Windows.Forms.Padding(4);
            this.GameField.Size = new System.Drawing.Size(653, 332);
            this.GameField.TabIndex = 4;
            this.GameField.TabStop = false;
            this.GameField.Text = "Стіл";
            // 
            // DeckZone
            // 
            this.DeckZone.Controls.Add(this.DeckBack);
            this.DeckZone.Controls.Add(this.TrumpCard);
            this.DeckZone.Controls.Add(this.TrumpImage);
            this.DeckZone.Location = new System.Drawing.Point(8, 111);
            this.DeckZone.Margin = new System.Windows.Forms.Padding(4);
            this.DeckZone.Name = "DeckZone";
            this.DeckZone.Padding = new System.Windows.Forms.Padding(4);
            this.DeckZone.Size = new System.Drawing.Size(147, 154);
            this.DeckZone.TabIndex = 0;
            this.DeckZone.TabStop = false;
            this.DeckZone.Text = "Колода";
            // 
            // DeckBack
            // 
            this.DeckBack.Location = new System.Drawing.Point(40, 25);
            this.DeckBack.Margin = new System.Windows.Forms.Padding(4);
            this.DeckBack.Name = "DeckBack";
            this.DeckBack.Size = new System.Drawing.Size(107, 123);
            this.DeckBack.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.DeckBack.TabIndex = 2;
            this.DeckBack.TabStop = false;
            this.DeckBack.Visible = false;
            // 
            // TrumpCard
            // 
            this.TrumpCard.Location = new System.Drawing.Point(0, 25);
            this.TrumpCard.Margin = new System.Windows.Forms.Padding(4);
            this.TrumpCard.Name = "TrumpCard";
            this.TrumpCard.Size = new System.Drawing.Size(107, 123);
            this.TrumpCard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.TrumpCard.TabIndex = 1;
            this.TrumpCard.TabStop = false;
            this.TrumpCard.Visible = false;
            // 
            // TrumpImage
            // 
            this.TrumpImage.Location = new System.Drawing.Point(47, 55);
            this.TrumpImage.Margin = new System.Windows.Forms.Padding(4);
            this.TrumpImage.Name = "TrumpImage";
            this.TrumpImage.Size = new System.Drawing.Size(53, 49);
            this.TrumpImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.TrumpImage.TabIndex = 0;
            this.TrumpImage.TabStop = false;
            // 
            // Gamer2Zone
            // 
            this.Gamer2Zone.Location = new System.Drawing.Point(4, 33);
            this.Gamer2Zone.Margin = new System.Windows.Forms.Padding(4);
            this.Gamer2Zone.Name = "Gamer2Zone";
            this.Gamer2Zone.Padding = new System.Windows.Forms.Padding(4);
            this.Gamer2Zone.Size = new System.Drawing.Size(160, 308);
            this.Gamer2Zone.TabIndex = 5;
            this.Gamer2Zone.TabStop = false;
            this.Gamer2Zone.Text = "Гравець 2";
            // 
            // Gamer1Zone
            // 
            this.Gamer1Zone.Location = new System.Drawing.Point(4, 368);
            this.Gamer1Zone.Margin = new System.Windows.Forms.Padding(4);
            this.Gamer1Zone.Name = "Gamer1Zone";
            this.Gamer1Zone.Padding = new System.Windows.Forms.Padding(4);
            this.Gamer1Zone.Size = new System.Drawing.Size(160, 308);
            this.Gamer1Zone.TabIndex = 6;
            this.Gamer1Zone.TabStop = false;
            this.Gamer1Zone.Text = "Гравець 1";
            // 
            // Gamer4Zone
            // 
            this.Gamer4Zone.Location = new System.Drawing.Point(1003, 33);
            this.Gamer4Zone.Margin = new System.Windows.Forms.Padding(4);
            this.Gamer4Zone.Name = "Gamer4Zone";
            this.Gamer4Zone.Padding = new System.Windows.Forms.Padding(4);
            this.Gamer4Zone.Size = new System.Drawing.Size(160, 308);
            this.Gamer4Zone.TabIndex = 6;
            this.Gamer4Zone.TabStop = false;
            this.Gamer4Zone.Text = "Гравець 4";
            // 
            // Gamer5Zone
            // 
            this.Gamer5Zone.Location = new System.Drawing.Point(1003, 368);
            this.Gamer5Zone.Margin = new System.Windows.Forms.Padding(4);
            this.Gamer5Zone.Name = "Gamer5Zone";
            this.Gamer5Zone.Padding = new System.Windows.Forms.Padding(4);
            this.Gamer5Zone.Size = new System.Drawing.Size(160, 308);
            this.Gamer5Zone.TabIndex = 7;
            this.Gamer5Zone.TabStop = false;
            this.Gamer5Zone.Text = "Гравець 5";
            // 
            // endRoundBtn
            // 
            this.endRoundBtn.Location = new System.Drawing.Point(25, 302);
            this.endRoundBtn.Margin = new System.Windows.Forms.Padding(4);
            this.endRoundBtn.Name = "endRoundBtn";
            this.endRoundBtn.Size = new System.Drawing.Size(100, 28);
            this.endRoundBtn.TabIndex = 1;
            this.endRoundBtn.Text = "Відбій";
            this.endRoundBtn.UseVisualStyleBackColor = true;
            this.endRoundBtn.Click += new System.EventHandler(this.endRoundBtn_Click);
            // 
            // takeCardsBtn
            // 
            this.takeCardsBtn.Location = new System.Drawing.Point(25, 271);
            this.takeCardsBtn.Margin = new System.Windows.Forms.Padding(4);
            this.takeCardsBtn.Name = "takeCardsBtn";
            this.takeCardsBtn.Size = new System.Drawing.Size(100, 28);
            this.takeCardsBtn.TabIndex = 2;
            this.takeCardsBtn.Text = "Беру";
            this.takeCardsBtn.UseVisualStyleBackColor = true;
            this.takeCardsBtn.Click += new System.EventHandler(this.takeCardsBtn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gameStateTb);
            this.groupBox1.Controls.Add(this.endRoundBtn);
            this.groupBox1.Controls.Add(this.takeCardsBtn);
            this.groupBox1.Controls.Add(this.DeckZone);
            this.groupBox1.Location = new System.Drawing.Point(833, 188);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(161, 332);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Гра";
            // 
            // gameStateTb
            // 
            this.gameStateTb.Location = new System.Drawing.Point(8, 21);
            this.gameStateTb.Margin = new System.Windows.Forms.Padding(4);
            this.gameStateTb.Multiline = true;
            this.gameStateTb.Name = "gameStateTb";
            this.gameStateTb.ReadOnly = true;
            this.gameStateTb.Size = new System.Drawing.Size(145, 85);
            this.gameStateTb.TabIndex = 3;
            // 
            // GameWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1179, 690);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Gamer5Zone);
            this.Controls.Add(this.Gamer4Zone);
            this.Controls.Add(this.Gamer1Zone);
            this.Controls.Add(this.Gamer2Zone);
            this.Controls.Add(this.GameField);
            this.Controls.Add(this.Gamer3Zone);
            this.Controls.Add(this.IamZone);
            this.Controls.Add(this.mainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenu;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1197, 737);
            this.MinimumSize = new System.Drawing.Size(1197, 737);
            this.Name = "GameWindow";
            this.Text = "Дурень";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.DeckZone.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DeckBack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrumpCard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrumpImage)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.MenuStrip mainMenu;
    private System.Windows.Forms.ToolStripMenuItem ServerMenuItem;
    private System.Windows.Forms.ToolStripMenuItem startServerToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem connectToServerToolStripMenuItem;
    private System.Windows.Forms.GroupBox IamZone;
    private System.Windows.Forms.GroupBox Gamer3Zone;
    private System.Windows.Forms.GroupBox GameField;
    private System.Windows.Forms.GroupBox DeckZone;
    private System.Windows.Forms.PictureBox TrumpImage;
    private System.Windows.Forms.ToolStripMenuItem gameMenuItem;
    private System.Windows.Forms.ToolStripMenuItem newGameToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.GroupBox Gamer2Zone;
    private System.Windows.Forms.GroupBox Gamer1Zone;
    private System.Windows.Forms.GroupBox Gamer4Zone;
    private System.Windows.Forms.GroupBox Gamer5Zone;
    private System.Windows.Forms.Button takeCardsBtn;
    private System.Windows.Forms.Button endRoundBtn;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.PictureBox DeckBack;
    private System.Windows.Forms.PictureBox TrumpCard;
    private System.Windows.Forms.TextBox gameStateTb;
  }
}

