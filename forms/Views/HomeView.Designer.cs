﻿namespace forms
{
    partial class HomeView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HomeView));
            job_label = new Label();
            txtbox_job = new TextBox();
            applyButton = new Button();
            title_label = new Label();
            label1 = new Label();
            amount_jobs = new TextBox();
            amout_jobs_label = new Label();
            label_choose_by = new Label();
            location_label = new Label();
            job_type_label = new Label();
            comboBox_choose_by = new ComboBox();
            experience_level_label = new Label();
            checkedListBox_type_job = new CheckedListBox();
            checkedListBox_remote = new CheckedListBox();
            groupBox_job = new GroupBox();
            groupBox_filters = new GroupBox();
            comboBox_annoucement_date = new ComboBox();
            label_annoucement_date = new Label();
            checkedListBox_experience_level = new CheckedListBox();
            button_config = new Button();
            groupBox_job.SuspendLayout();
            groupBox_filters.SuspendLayout();
            SuspendLayout();
            // 
            // job_label
            // 
            job_label.AutoSize = true;
            job_label.Location = new Point(24, 49);
            job_label.Name = "job_label";
            job_label.Size = new Size(131, 15);
            job_label.TabIndex = 0;
            job_label.Text = "Cargo ou competência:";
            // 
            // txtbox_job
            // 
            txtbox_job.Location = new Point(24, 67);
            txtbox_job.Name = "txtbox_job";
            txtbox_job.Size = new Size(200, 23);
            txtbox_job.TabIndex = 1;
            // 
            // applyButton
            // 
            applyButton.Font = new Font("Segoe UI", 12F);
            applyButton.Location = new Point(297, 675);
            applyButton.Name = "applyButton";
            applyButton.Size = new Size(102, 44);
            applyButton.TabIndex = 2;
            applyButton.Text = "Aplicar";
            applyButton.UseVisualStyleBackColor = true;
            applyButton.Click += ApplyButton_Click;
            // 
            // title_label
            // 
            title_label.AutoSize = true;
            title_label.FlatStyle = FlatStyle.Flat;
            title_label.Font = new Font("Segoe UI", 14F);
            title_label.ForeColor = SystemColors.ActiveCaptionText;
            title_label.Location = new Point(240, 42);
            title_label.Name = "title_label";
            title_label.Size = new Size(217, 25);
            title_label.TabIndex = 7;
            title_label.Text = "LINKEDIN AUTOMATION";
            // 
            // label1
            // 
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(100, 23);
            label1.TabIndex = 0;
            // 
            // amount_jobs
            // 
            amount_jobs.Location = new Point(24, 120);
            amount_jobs.Name = "amount_jobs";
            amount_jobs.Size = new Size(44, 23);
            amount_jobs.TabIndex = 10;
            // 
            // amout_jobs_label
            // 
            amout_jobs_label.AutoSize = true;
            amout_jobs_label.Location = new Point(24, 102);
            amout_jobs_label.Name = "amout_jobs_label";
            amout_jobs_label.Size = new Size(73, 15);
            amout_jobs_label.TabIndex = 11;
            amout_jobs_label.Text = "N° de vagas:";
            // 
            // label_choose_by
            // 
            label_choose_by.AutoSize = true;
            label_choose_by.Location = new Point(38, 22);
            label_choose_by.Name = "label_choose_by";
            label_choose_by.Size = new Size(84, 15);
            label_choose_by.TabIndex = 26;
            label_choose_by.Text = "Classificar por:";
            // 
            // location_label
            // 
            location_label.AutoSize = true;
            location_label.Location = new Point(38, 450);
            location_label.Name = "location_label";
            location_label.Size = new Size(52, 15);
            location_label.TabIndex = 25;
            location_label.Text = "Remoto:";
            // 
            // job_type_label
            // 
            job_type_label.AutoSize = true;
            job_type_label.Location = new Point(38, 288);
            job_type_label.Name = "job_type_label";
            job_type_label.Size = new Size(77, 15);
            job_type_label.TabIndex = 24;
            job_type_label.Text = "Tipo de vaga:";
            // 
            // comboBox_choose_by
            // 
            comboBox_choose_by.AutoCompleteCustomSource.AddRange(new string[] { "A qualquer momento", "Último mês", "Última semana", "Últimas 24 horas" });
            comboBox_choose_by.FormattingEnabled = true;
            comboBox_choose_by.Items.AddRange(new object[] { "Mais recentes", "Mais relevantes" });
            comboBox_choose_by.Location = new Point(38, 40);
            comboBox_choose_by.Name = "comboBox_choose_by";
            comboBox_choose_by.Size = new Size(121, 23);
            comboBox_choose_by.TabIndex = 23;
            // 
            // experience_level_label
            // 
            experience_level_label.AutoSize = true;
            experience_level_label.Location = new Point(38, 142);
            experience_level_label.Name = "experience_level_label";
            experience_level_label.Size = new Size(113, 15);
            experience_level_label.TabIndex = 19;
            experience_level_label.Text = "Nível de experiência";
            // 
            // checkedListBox_type_job
            // 
            checkedListBox_type_job.FormattingEnabled = true;
            checkedListBox_type_job.Items.AddRange(new object[] { "Tempo integral", "Meio período", "Contrato", "Temporário", "Voluntário", "Estágio", "Outro" });
            checkedListBox_type_job.Location = new Point(38, 306);
            checkedListBox_type_job.Name = "checkedListBox_type_job";
            checkedListBox_type_job.Size = new Size(120, 130);
            checkedListBox_type_job.TabIndex = 28;
            // 
            // checkedListBox_remote
            // 
            checkedListBox_remote.FormattingEnabled = true;
            checkedListBox_remote.Items.AddRange(new object[] { "Remoto", "Híbrido", "Presencial" });
            checkedListBox_remote.Location = new Point(38, 468);
            checkedListBox_remote.Name = "checkedListBox_remote";
            checkedListBox_remote.Size = new Size(120, 58);
            checkedListBox_remote.TabIndex = 29;
            // 
            // groupBox_job
            // 
            groupBox_job.Controls.Add(txtbox_job);
            groupBox_job.Controls.Add(amout_jobs_label);
            groupBox_job.Controls.Add(amount_jobs);
            groupBox_job.Controls.Add(job_label);
            groupBox_job.Location = new Point(40, 95);
            groupBox_job.Name = "groupBox_job";
            groupBox_job.Size = new Size(280, 164);
            groupBox_job.TabIndex = 34;
            groupBox_job.TabStop = false;
            groupBox_job.Text = "Vaga";
            // 
            // groupBox_filters
            // 
            groupBox_filters.Controls.Add(comboBox_annoucement_date);
            groupBox_filters.Controls.Add(label_annoucement_date);
            groupBox_filters.Controls.Add(checkedListBox_experience_level);
            groupBox_filters.Controls.Add(comboBox_choose_by);
            groupBox_filters.Controls.Add(label_choose_by);
            groupBox_filters.Controls.Add(location_label);
            groupBox_filters.Controls.Add(checkedListBox_remote);
            groupBox_filters.Controls.Add(checkedListBox_type_job);
            groupBox_filters.Controls.Add(experience_level_label);
            groupBox_filters.Controls.Add(job_type_label);
            groupBox_filters.Location = new Point(384, 95);
            groupBox_filters.Name = "groupBox_filters";
            groupBox_filters.Size = new Size(240, 549);
            groupBox_filters.TabIndex = 35;
            groupBox_filters.TabStop = false;
            groupBox_filters.Text = "Filtros";
            // 
            // comboBox_annoucement_date
            // 
            comboBox_annoucement_date.FormattingEnabled = true;
            comboBox_annoucement_date.Items.AddRange(new object[] { "A qualquer momento", "Último mês", "Última semana", "Últimas 24 horas" });
            comboBox_annoucement_date.Location = new Point(38, 102);
            comboBox_annoucement_date.Name = "comboBox_annoucement_date";
            comboBox_annoucement_date.Size = new Size(121, 23);
            comboBox_annoucement_date.TabIndex = 32;
            // 
            // label_annoucement_date
            // 
            label_annoucement_date.AutoSize = true;
            label_annoucement_date.Location = new Point(38, 84);
            label_annoucement_date.Name = "label_annoucement_date";
            label_annoucement_date.Size = new Size(97, 15);
            label_annoucement_date.TabIndex = 31;
            label_annoucement_date.Text = "Data do anúncio:";
            // 
            // checkedListBox_experience_level
            // 
            checkedListBox_experience_level.FormattingEnabled = true;
            checkedListBox_experience_level.Items.AddRange(new object[] { "Estágio", "Júnior", "Assistente", "Pleno-sênior", "Diretor", "Executivo" });
            checkedListBox_experience_level.Location = new Point(38, 160);
            checkedListBox_experience_level.Name = "checkedListBox_experience_level";
            checkedListBox_experience_level.Size = new Size(120, 112);
            checkedListBox_experience_level.TabIndex = 30;
            // 
            // button_config
            // 
            button_config.AllowDrop = true;
            button_config.BackgroundImage = (Image)resources.GetObject("button_config.BackgroundImage");
            button_config.BackgroundImageLayout = ImageLayout.Zoom;
            button_config.Font = new Font("Segoe UI", 1F);
            button_config.Location = new Point(594, 42);
            button_config.Name = "button_config";
            button_config.Size = new Size(30, 28);
            button_config.TabIndex = 14;
            button_config.UseVisualStyleBackColor = true;
            button_config.Click += button_config_Click;
            // 
            // HomeView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImageLayout = ImageLayout.Center;
            ClientSize = new Size(688, 731);
            Controls.Add(button_config);
            Controls.Add(groupBox_filters);
            Controls.Add(groupBox_job);
            Controls.Add(label1);
            Controls.Add(title_label);
            Controls.Add(applyButton);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "HomeView";
            Text = "Linkedin automation";
            groupBox_job.ResumeLayout(false);
            groupBox_job.PerformLayout();
            groupBox_filters.ResumeLayout(false);
            groupBox_filters.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label job_label;
        private TextBox txtbox_job;
        private Button send;
        private Button applyButton;
        private Label password_Label;
        private Label title_label;
        private Label label1;
        private TextBox amount_jobs;
        private Label amout_jobs_label;
        private Label label_choose_by;
        private Label location_label;
        private Label job_type_label;
        private ComboBox comboBox_choose_by;
        private ComboBox comboBox_experience_level;
        private Label experience_level_label;
        private CheckedListBox checkedListBox_type_job;
        private CheckedListBox checkedListBox_remote;
        private GroupBox groupBox_job;
        private GroupBox groupBox_filters;
        private CheckedListBox checkedListBox_experience_level;
        private ComboBox comboBox_annoucement_date;
        private Label label_annoucement_date;
        private Button button_config;
    }
}