namespace Calculator2
{
    partial class CalculatorForm
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
            this.GetAnswer = new System.Windows.Forms.Button();
            this.Expression = new System.Windows.Forms.TextBox();
            this.Answer = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // GetAnswer
            // 
            this.GetAnswer.Location = new System.Drawing.Point(122, 162);
            this.GetAnswer.Name = "GetAnswer";
            this.GetAnswer.Size = new System.Drawing.Size(94, 29);
            this.GetAnswer.TabIndex = 0;
            this.GetAnswer.Text = "=";
            this.GetAnswer.UseVisualStyleBackColor = true;
            this.GetAnswer.Click += new System.EventHandler(this.GetAnswer_Click);
            // 
            // Expression
            // 
            this.Expression.Location = new System.Drawing.Point(12, 21);
            this.Expression.Name = "Expression";
            this.Expression.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Expression.Size = new System.Drawing.Size(311, 27);
            this.Expression.TabIndex = 1;
            this.Expression.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Answer
            // 
            this.Answer.Location = new System.Drawing.Point(12, 63);
            this.Answer.Name = "Answer";
            this.Answer.ReadOnly = true;
            this.Answer.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Answer.Size = new System.Drawing.Size(311, 27);
            this.Answer.TabIndex = 2;
            this.Answer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // CalculatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 273);
            this.Controls.Add(this.Answer);
            this.Controls.Add(this.Expression);
            this.Controls.Add(this.GetAnswer);
            this.Name = "CalculatorForm";
            this.Text = "CalculatorForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button GetAnswer;
        private TextBox Expression;
        private TextBox Answer;
    }
}