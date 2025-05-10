using System.Drawing;
using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.Controls
{
    partial class ConsoleRepresentationSelector
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            btnChangeConsoleCharacterBackColor = new Button();
            btnChangeConsoleCharacterForeColor = new Button();
            btnChangeConsoleCharacter = new Button();
            lblConsoleRepresentation = new Label();
            SuspendLayout();
            // 
            // btnChangeConsoleCharacterBackColor
            // 
            btnChangeConsoleCharacterBackColor.Location = new Point(73, 58);
            btnChangeConsoleCharacterBackColor.Name = "btnChangeConsoleCharacterBackColor";
            btnChangeConsoleCharacterBackColor.Size = new Size(135, 23);
            btnChangeConsoleCharacterBackColor.TabIndex = 115;
            btnChangeConsoleCharacterBackColor.Text = "Change Background...";
            btnChangeConsoleCharacterBackColor.UseVisualStyleBackColor = true;
            btnChangeConsoleCharacterBackColor.Click += btnChangeConsoleCharacterBackColor_Click;
            // 
            // btnChangeConsoleCharacterForeColor
            // 
            btnChangeConsoleCharacterForeColor.Location = new Point(73, 29);
            btnChangeConsoleCharacterForeColor.Name = "btnChangeConsoleCharacterForeColor";
            btnChangeConsoleCharacterForeColor.Size = new Size(135, 23);
            btnChangeConsoleCharacterForeColor.TabIndex = 114;
            btnChangeConsoleCharacterForeColor.Text = "Change Foreground...";
            btnChangeConsoleCharacterForeColor.UseVisualStyleBackColor = true;
            btnChangeConsoleCharacterForeColor.Click += btnChangeConsoleCharacterForeColor_Click;
            // 
            // btnChangeConsoleCharacter
            // 
            btnChangeConsoleCharacter.Location = new Point(73, 0);
            btnChangeConsoleCharacter.Name = "btnChangeConsoleCharacter";
            btnChangeConsoleCharacter.Size = new Size(135, 23);
            btnChangeConsoleCharacter.TabIndex = 113;
            btnChangeConsoleCharacter.Text = "Change Character...";
            btnChangeConsoleCharacter.UseVisualStyleBackColor = true;
            btnChangeConsoleCharacter.Click += btnChangeConsoleCharacter_Click;
            // 
            // lblConsoleRepresentation
            // 
            lblConsoleRepresentation.Font = new Font("Consolas", 36F);
            lblConsoleRepresentation.Location = new Point(0, 9);
            lblConsoleRepresentation.Name = "lblConsoleRepresentation";
            lblConsoleRepresentation.Size = new Size(64, 64);
            lblConsoleRepresentation.TabIndex = 112;
            lblConsoleRepresentation.TextAlign = ContentAlignment.MiddleCenter;
            lblConsoleRepresentation.UseMnemonic = false;
            // 
            // ConsoleRepresentationSelector
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnChangeConsoleCharacterBackColor);
            Controls.Add(btnChangeConsoleCharacterForeColor);
            Controls.Add(btnChangeConsoleCharacter);
            Controls.Add(lblConsoleRepresentation);
            Name = "ConsoleRepresentationSelector";
            Size = new Size(211, 83);
            ResumeLayout(false);
        }

        #endregion

        private Button btnChangeConsoleCharacterBackColor;
        private Button btnChangeConsoleCharacterForeColor;
        private Button btnChangeConsoleCharacter;
        private Label lblConsoleRepresentation;
    }
}
