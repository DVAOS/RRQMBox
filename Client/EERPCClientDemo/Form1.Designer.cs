//------------------------------------------------------------------------------
//  此代码版权（除特别声明或在RRQMCore.XREF命名空间的代码）归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  Gitee源代码仓库：https://gitee.com/RRQM_Home
//  Github源代码仓库：https://github.com/RRQM
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------

namespace EERPCClientDemo
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button2 = new System.Windows.Forms.Button();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button5 = new System.Windows.Forms.Button();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1103, 89);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(194, 38);
            this.button2.TabIndex = 14;
            this.button2.Text = "获取所有事件";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(721, 89);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(152, 35);
            this.checkBox3.TabIndex = 13;
            this.checkBox3.Text = "Everyone";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(587, 89);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(128, 35);
            this.checkBox2.TabIndex = 12;
            this.checkBox2.Text = "Service";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(447, 89);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(123, 35);
            this.checkBox1.TabIndex = 11;
            this.checkBox1.Text = "Owner";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(30, 89);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(398, 38);
            this.textBox2.TabIndex = 10;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(903, 89);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(150, 38);
            this.button1.TabIndex = 9;
            this.button1.Text = "发布事件";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(447, 487);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(1138, 339);
            this.textBox1.TabIndex = 8;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(30, 29);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(150, 46);
            this.button3.TabIndex = 15;
            this.button3.Text = "连接";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(447, 173);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(150, 38);
            this.button4.TabIndex = 17;
            this.button4.Text = "订阅事件";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(30, 173);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(398, 38);
            this.textBox3.TabIndex = 16;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 31;
            this.listBox1.Location = new System.Drawing.Point(30, 294);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(398, 531);
            this.listBox1.TabIndex = 18;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(1435, 293);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(150, 179);
            this.button5.TabIndex = 20;
            this.button5.Text = "触发事件";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(447, 294);
            this.textBox4.Multiline = true;
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(954, 178);
            this.textBox4.TabIndex = 19;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1595, 837);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox textBox4;
    }
}

