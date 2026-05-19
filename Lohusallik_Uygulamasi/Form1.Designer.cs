namespace LohusaDestekApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlMainContainer = new System.Windows.Forms.Panel();
            this.pnlLoginRegister = new System.Windows.Forms.Panel();
            this.pnlLoginCard = new System.Windows.Forms.Panel();
            this.lblLogo = new System.Windows.Forms.Label();
            this.lblLoginTitle = new System.Windows.Forms.Label();
            this.lblLoginSubtitle = new System.Windows.Forms.Label();
            this.lblNickname = new System.Windows.Forms.Label();
            this.txtNickname = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnRegister = new System.Windows.Forms.Button();
            this.lblWarning = new System.Windows.Forms.Label();
            this.pnlDashboard = new System.Windows.Forms.Panel();
            this.pnlRightColumn = new System.Windows.Forms.Panel();
            this.pnlCommentsSection = new System.Windows.Forms.Panel();
            this.lstComments = new System.Windows.Forms.ListBox();
            this.pnlNewComment = new System.Windows.Forms.Panel();
            this.txtCommentContent = new System.Windows.Forms.TextBox();
            this.btnShareComment = new System.Windows.Forms.Button();
            this.lblCommentsHeader = new System.Windows.Forms.Label();
            this.pnlRightSpacer = new System.Windows.Forms.Panel();
            this.pnlSelectedPostCard = new System.Windows.Forms.Panel();
            this.txtSelectedPostContent = new System.Windows.Forms.TextBox();
            this.lblSelectedPostMeta = new System.Windows.Forms.Label();
            this.pnlDivider = new System.Windows.Forms.Panel();
            this.pnlLeftColumn = new System.Windows.Forms.Panel();
            this.lstFeed = new System.Windows.Forms.ListBox();
            this.pnlNewPost = new System.Windows.Forms.Panel();
            this.btnSharePost = new System.Windows.Forms.Button();
            this.cmbPostCategory = new System.Windows.Forms.ComboBox();
            this.lblPostCategoryLabel = new System.Windows.Forms.Label();
            this.txtPostContent = new System.Windows.Forms.TextBox();
            this.lblNewPostTitle = new System.Windows.Forms.Label();
            this.pnlFilter = new System.Windows.Forms.Panel();
            this.cmbFilterCategory = new System.Windows.Forms.ComboBox();
            this.lblFilter = new System.Windows.Forms.Label();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnLogout = new System.Windows.Forms.Button();
            this.lblActiveUser = new System.Windows.Forms.Label();
            this.lblHeaderTitle = new System.Windows.Forms.Label();
            this.pnlDailyTip = new System.Windows.Forms.Panel();
            this.lblDailyTip = new System.Windows.Forms.Label();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnLikePost = new System.Windows.Forms.Button();
            this.pnlMainContainer.SuspendLayout();
            this.pnlLoginRegister.SuspendLayout();
            this.pnlLoginCard.SuspendLayout();
            this.pnlDashboard.SuspendLayout();
            this.pnlRightColumn.SuspendLayout();
            this.pnlCommentsSection.SuspendLayout();
            this.pnlNewComment.SuspendLayout();
            this.pnlSelectedPostCard.SuspendLayout();
            this.pnlLeftColumn.SuspendLayout();
            this.pnlNewPost.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.pnlDailyTip.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMainContainer
            // 
            this.pnlMainContainer.Controls.Add(this.pnlLoginRegister);
            this.pnlMainContainer.Controls.Add(this.pnlDashboard);
            this.pnlMainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlMainContainer.Name = "pnlMainContainer";
            this.pnlMainContainer.Size = new System.Drawing.Size(1100, 700);
            this.pnlMainContainer.TabIndex = 0;
            // 
            // pnlLoginRegister
            // 
            this.pnlLoginRegister.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(242)))), ((int)(((byte)(244)))));
            this.pnlLoginRegister.Controls.Add(this.pnlLoginCard);
            this.pnlLoginRegister.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLoginRegister.Location = new System.Drawing.Point(0, 0);
            this.pnlLoginRegister.Name = "pnlLoginRegister";
            this.pnlLoginRegister.Size = new System.Drawing.Size(1100, 700);
            this.pnlLoginRegister.TabIndex = 0;
            // 
            // pnlLoginCard
            // 
            this.pnlLoginCard.BackColor = System.Drawing.Color.White;
            this.pnlLoginCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLoginCard.Controls.Add(this.lblLogo);
            this.pnlLoginCard.Controls.Add(this.lblLoginTitle);
            this.pnlLoginCard.Controls.Add(this.lblLoginSubtitle);
            this.pnlLoginCard.Controls.Add(this.lblNickname);
            this.pnlLoginCard.Controls.Add(this.txtNickname);
            this.pnlLoginCard.Controls.Add(this.lblPassword);
            this.pnlLoginCard.Controls.Add(this.txtPassword);
            this.pnlLoginCard.Controls.Add(this.btnLogin);
            this.pnlLoginCard.Controls.Add(this.btnRegister);
            this.pnlLoginCard.Controls.Add(this.lblWarning);
            this.pnlLoginCard.Location = new System.Drawing.Point(350, 110);
            this.pnlLoginCard.Name = "pnlLoginCard";
            this.pnlLoginCard.Size = new System.Drawing.Size(400, 480);
            this.pnlLoginCard.TabIndex = 0;
            // 
            // lblLogo
            // 
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogo.Location = new System.Drawing.Point(30, 15);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(340, 70);
            this.lblLogo.TabIndex = 0;
            this.lblLogo.Text = "🌸";
            this.lblLogo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLoginTitle
            // 
            this.lblLoginTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblLoginTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.lblLoginTitle.Location = new System.Drawing.Point(30, 85);
            this.lblLoginTitle.Name = "lblLoginTitle";
            this.lblLoginTitle.Size = new System.Drawing.Size(340, 35);
            this.lblLoginTitle.TabIndex = 1;
            this.lblLoginTitle.Text = "Lohusa Destek Platformu";
            this.lblLoginTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLoginSubtitle
            // 
            this.lblLoginSubtitle.Font = new System.Drawing.Font("Segoe UI", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblLoginSubtitle.ForeColor = System.Drawing.Color.DimGray;
            this.lblLoginSubtitle.Location = new System.Drawing.Point(30, 120);
            this.lblLoginSubtitle.Name = "lblLoginSubtitle";
            this.lblLoginSubtitle.Size = new System.Drawing.Size(340, 35);
            this.lblLoginSubtitle.TabIndex = 2;
            this.lblLoginSubtitle.Text = "Yeni anneler için tamamen anonim yardımlaşma alanı.";
            this.lblLoginSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNickname
            // 
            this.lblNickname.AutoSize = true;
            this.lblNickname.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblNickname.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.lblNickname.Location = new System.Drawing.Point(40, 172);
            this.lblNickname.Name = "lblNickname";
            this.lblNickname.Size = new System.Drawing.Size(123, 17);
            this.lblNickname.TabIndex = 3;
            this.lblNickname.Text = "Rumuz (Nickname):";
            // 
            // txtNickname
            // 
            this.txtNickname.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtNickname.Location = new System.Drawing.Point(40, 193);
            this.txtNickname.Name = "txtNickname";
            this.txtNickname.Size = new System.Drawing.Size(320, 26);
            this.txtNickname.TabIndex = 4;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.lblPassword.Location = new System.Drawing.Point(40, 233);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(37, 17);
            this.lblPassword.TabIndex = 5;
            this.lblPassword.Text = "Şifre:";
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtPassword.Location = new System.Drawing.Point(40, 254);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.Size = new System.Drawing.Size(320, 26);
            this.txtPassword.TabIndex = 6;
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(40, 305);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(320, 38);
            this.btnLogin.TabIndex = 7;
            this.btnLogin.Text = "Giriş Yap";
            this.btnLogin.UseVisualStyleBackColor = false;
            // 
            // btnRegister
            // 
            this.btnRegister.BackColor = System.Drawing.Color.White;
            this.btnRegister.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRegister.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.btnRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegister.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnRegister.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.btnRegister.Location = new System.Drawing.Point(40, 355);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(320, 38);
            this.btnRegister.TabIndex = 8;
            this.btnRegister.Text = "Anonim Kayıt Ol";
            this.btnRegister.UseVisualStyleBackColor = false;
            // 
            // lblWarning
            // 
            this.lblWarning.Font = new System.Drawing.Font("Segoe UI", 7.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblWarning.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblWarning.Location = new System.Drawing.Point(30, 410);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(340, 45);
            this.lblWarning.TabIndex = 9;
            this.lblWarning.Text = "⚠️ Güvenliğiniz için gerçek isim, soyisim veya e-posta adresi gibi kişisel bilgilerinizi KULLANMAYINIZ. Sadece rumuz yeterlidir.";
            this.lblWarning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlDashboard
            // 
            this.pnlDashboard.Controls.Add(this.pnlRightColumn);
            this.pnlDashboard.Controls.Add(this.pnlDivider);
            this.pnlDashboard.Controls.Add(this.pnlLeftColumn);
            this.pnlDashboard.Controls.Add(this.pnlDailyTip);
            this.pnlDashboard.Controls.Add(this.pnlHeader);
            this.pnlDashboard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDashboard.Location = new System.Drawing.Point(0, 0);
            this.pnlDashboard.Name = "pnlDashboard";
            this.pnlDashboard.Size = new System.Drawing.Size(1100, 700);
            this.pnlDashboard.TabIndex = 1;
            this.pnlDashboard.Visible = false;
            // 
            // pnlDailyTip
            // 
            this.pnlDailyTip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(235)))), ((int)(((byte)(238)))));
            this.pnlDailyTip.Controls.Add(this.lblDailyTip);
            this.pnlDailyTip.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDailyTip.Location = new System.Drawing.Point(0, 65);
            this.pnlDailyTip.Name = "pnlDailyTip";
            this.pnlDailyTip.Size = new System.Drawing.Size(1100, 40);
            this.pnlDailyTip.TabIndex = 4;
            // 
            // lblDailyTip
            // 
            this.lblDailyTip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDailyTip.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblDailyTip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(50)))), ((int)(((byte)(70)))));
            this.lblDailyTip.Location = new System.Drawing.Point(0, 0);
            this.lblDailyTip.Name = "lblDailyTip";
            this.lblDailyTip.Size = new System.Drawing.Size(1100, 40);
            this.lblDailyTip.TabIndex = 0;
            this.lblDailyTip.Text = "💡 Günün Tavsiyesi: Kendinize gün içinde 15 dakika sadece nefes alıp rahatlayabileceğiniz özel bir zaman ayırın.";
            this.lblDailyTip.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlRightColumn
            // 
            this.pnlRightColumn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.pnlRightColumn.Controls.Add(this.pnlCommentsSection);
            this.pnlRightColumn.Controls.Add(this.pnlRightSpacer);
            this.pnlRightColumn.Controls.Add(this.pnlSelectedPostCard);
            this.pnlRightColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRightColumn.Location = new System.Drawing.Point(540, 65);
            this.pnlRightColumn.Name = "pnlRightColumn";
            this.pnlRightColumn.Padding = new System.Windows.Forms.Padding(10);
            this.pnlRightColumn.Size = new System.Drawing.Size(560, 635);
            this.pnlRightColumn.TabIndex = 3;
            // 
            // pnlCommentsSection
            // 
            this.pnlCommentsSection.Controls.Add(this.lstComments);
            this.pnlCommentsSection.Controls.Add(this.pnlNewComment);
            this.pnlCommentsSection.Controls.Add(this.lblCommentsHeader);
            this.pnlCommentsSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCommentsSection.Location = new System.Drawing.Point(10, 195);
            this.pnlCommentsSection.Name = "pnlCommentsSection";
            this.pnlCommentsSection.Size = new System.Drawing.Size(540, 430);
            this.pnlCommentsSection.TabIndex = 2;
            // 
            // lstComments
            // 
            this.lstComments.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstComments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstComments.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstComments.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lstComments.FormattingEnabled = true;
            this.lstComments.ItemHeight = 50;
            this.lstComments.Location = new System.Drawing.Point(0, 25);
            this.lstComments.Name = "lstComments";
            this.lstComments.Size = new System.Drawing.Size(540, 325);
            this.lstComments.TabIndex = 1;
            // 
            // pnlNewComment
            // 
            this.pnlNewComment.BackColor = System.Drawing.Color.Transparent;
            this.pnlNewComment.Controls.Add(this.txtCommentContent);
            this.pnlNewComment.Controls.Add(this.btnShareComment);
            this.pnlNewComment.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlNewComment.Location = new System.Drawing.Point(0, 350);
            this.pnlNewComment.Name = "pnlNewComment";
            this.pnlNewComment.Size = new System.Drawing.Size(540, 80);
            this.pnlNewComment.TabIndex = 2;
            // 
            // txtCommentContent
            // 
            this.txtCommentContent.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtCommentContent.Location = new System.Drawing.Point(0, 10);
            this.txtCommentContent.Multiline = true;
            this.txtCommentContent.Name = "txtCommentContent";
            this.txtCommentContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCommentContent.Size = new System.Drawing.Size(400, 60);
            this.txtCommentContent.TabIndex = 0;
            // 
            // btnShareComment
            // 
            this.btnShareComment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.btnShareComment.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnShareComment.FlatAppearance.BorderSize = 0;
            this.btnShareComment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShareComment.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnShareComment.ForeColor = System.Drawing.Color.White;
            this.btnShareComment.Location = new System.Drawing.Point(410, 10);
            this.btnShareComment.Name = "btnShareComment";
            this.btnShareComment.Size = new System.Drawing.Size(130, 60);
            this.btnShareComment.TabIndex = 1;
            this.btnShareComment.Text = "Destek Ver\r\n💬";
            this.btnShareComment.UseVisualStyleBackColor = false;
            // 
            // lblCommentsHeader
            // 
            this.lblCommentsHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCommentsHeader.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblCommentsHeader.ForeColor = System.Drawing.Color.DimGray;
            this.lblCommentsHeader.Location = new System.Drawing.Point(0, 0);
            this.lblCommentsHeader.Name = "lblCommentsHeader";
            this.lblCommentsHeader.Size = new System.Drawing.Size(540, 25);
            this.lblCommentsHeader.TabIndex = 0;
            this.lblCommentsHeader.Text = "Destek ve Tavsiye Yorumları (Gelen Cevaplar):";
            this.lblCommentsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlRightSpacer
            // 
            this.pnlRightSpacer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlRightSpacer.Location = new System.Drawing.Point(10, 185);
            this.pnlRightSpacer.Name = "pnlRightSpacer";
            this.pnlRightSpacer.Size = new System.Drawing.Size(540, 10);
            this.pnlRightSpacer.TabIndex = 1;
            // 
            // pnlSelectedPostCard
            // 
            this.pnlSelectedPostCard.BackColor = System.Drawing.Color.White;
            this.pnlSelectedPostCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSelectedPostCard.Controls.Add(this.btnLikePost);
            this.pnlSelectedPostCard.Controls.Add(this.txtSelectedPostContent);
            this.pnlSelectedPostCard.Controls.Add(this.lblSelectedPostMeta);
            this.pnlSelectedPostCard.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSelectedPostCard.Location = new System.Drawing.Point(10, 10);
            this.pnlSelectedPostCard.Name = "pnlSelectedPostCard";
            this.pnlSelectedPostCard.Padding = new System.Windows.Forms.Padding(10);
            this.pnlSelectedPostCard.Size = new System.Drawing.Size(540, 175);
            this.pnlSelectedPostCard.TabIndex = 0;
            // 
            // txtSelectedPostContent
            // 
            this.txtSelectedPostContent.BackColor = System.Drawing.Color.White;
            this.txtSelectedPostContent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSelectedPostContent.Font = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtSelectedPostContent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtSelectedPostContent.Location = new System.Drawing.Point(10, 35);
            this.txtSelectedPostContent.Multiline = true;
            this.txtSelectedPostContent.Name = "txtSelectedPostContent";
            this.txtSelectedPostContent.ReadOnly = true;
            this.txtSelectedPostContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSelectedPostContent.Size = new System.Drawing.Size(518, 92);
            this.txtSelectedPostContent.TabIndex = 1;
            this.txtSelectedPostContent.Text = "Lütfen soldaki akıştan okumak istediğiniz bir gönderi seçin.";
            // 
            // btnLikePost
            // 
            this.btnLikePost.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(220)))), ((int)(((byte)(225)))));
            this.btnLikePost.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLikePost.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLikePost.FlatAppearance.BorderSize = 0;
            this.btnLikePost.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnLikePost.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.btnLikePost.Location = new System.Drawing.Point(370, 133);
            this.btnLikePost.Name = "btnLikePost";
            this.btnLikePost.Size = new System.Drawing.Size(158, 30);
            this.btnLikePost.TabIndex = 2;
            this.btnLikePost.Text = "Ben de Yaşıyorum (0) ♡";
            this.btnLikePost.UseVisualStyleBackColor = false;
            // 
            // lblSelectedPostMeta
            // 
            this.lblSelectedPostMeta.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblSelectedPostMeta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.lblSelectedPostMeta.Location = new System.Drawing.Point(10, 10);
            this.lblSelectedPostMeta.Name = "lblSelectedPostMeta";
            this.lblSelectedPostMeta.Size = new System.Drawing.Size(518, 20);
            this.lblSelectedPostMeta.TabIndex = 0;
            this.lblSelectedPostMeta.Text = "Seçili Gönderi Yok";
            this.lblSelectedPostMeta.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlDivider
            // 
            this.pnlDivider.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.pnlDivider.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlDivider.Location = new System.Drawing.Point(530, 65);
            this.pnlDivider.Name = "pnlDivider";
            this.pnlDivider.Size = new System.Drawing.Size(10, 635);
            this.pnlDivider.TabIndex = 2;
            // 
            // pnlLeftColumn
            // 
            this.pnlLeftColumn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.pnlLeftColumn.Controls.Add(this.lstFeed);
            this.pnlLeftColumn.Controls.Add(this.pnlNewPost);
            this.pnlLeftColumn.Controls.Add(this.pnlFilter);
            this.pnlLeftColumn.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeftColumn.Location = new System.Drawing.Point(0, 65);
            this.pnlLeftColumn.Name = "pnlLeftColumn";
            this.pnlLeftColumn.Padding = new System.Windows.Forms.Padding(10);
            this.pnlLeftColumn.Size = new System.Drawing.Size(530, 635);
            this.pnlLeftColumn.TabIndex = 1;
            // 
            // lstFeed
            // 
            this.lstFeed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstFeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstFeed.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstFeed.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lstFeed.FormattingEnabled = true;
            this.lstFeed.ItemHeight = 65;
            this.lstFeed.Location = new System.Drawing.Point(10, 55);
            this.lstFeed.Name = "lstFeed";
            this.lstFeed.Size = new System.Drawing.Size(510, 395);
            this.lstFeed.TabIndex = 1;
            // 
            // pnlNewPost
            // 
            this.pnlNewPost.BackColor = System.Drawing.Color.White;
            this.pnlNewPost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlNewPost.Controls.Add(this.btnSharePost);
            this.pnlNewPost.Controls.Add(this.cmbPostCategory);
            this.pnlNewPost.Controls.Add(this.lblPostCategoryLabel);
            this.pnlNewPost.Controls.Add(this.txtPostContent);
            this.pnlNewPost.Controls.Add(this.lblNewPostTitle);
            this.pnlNewPost.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlNewPost.Location = new System.Drawing.Point(10, 450);
            this.pnlNewPost.Name = "pnlNewPost";
            this.pnlNewPost.Size = new System.Drawing.Size(510, 175);
            this.pnlNewPost.TabIndex = 2;
            // 
            // btnSharePost
            // 
            this.btnSharePost.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.btnSharePost.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSharePost.FlatAppearance.BorderSize = 0;
            this.btnSharePost.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSharePost.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSharePost.ForeColor = System.Drawing.Color.White;
            this.btnSharePost.Location = new System.Drawing.Point(280, 126);
            this.btnSharePost.Name = "btnSharePost";
            this.btnSharePost.Size = new System.Drawing.Size(218, 32);
            this.btnSharePost.TabIndex = 4;
            this.btnSharePost.Text = "Anonim Paylaş 🌸";
            this.btnSharePost.UseVisualStyleBackColor = false;
            // 
            // cmbPostCategory
            // 
            this.cmbPostCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPostCategory.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.cmbPostCategory.FormattingEnabled = true;
            this.cmbPostCategory.Location = new System.Drawing.Point(70, 131);
            this.cmbPostCategory.Name = "cmbPostCategory";
            this.cmbPostCategory.Size = new System.Drawing.Size(195, 23);
            this.cmbPostCategory.TabIndex = 3;
            // 
            // lblPostCategoryLabel
            // 
            this.lblPostCategoryLabel.AutoSize = true;
            this.lblPostCategoryLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblPostCategoryLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblPostCategoryLabel.Location = new System.Drawing.Point(10, 134);
            this.lblPostCategoryLabel.Name = "lblPostCategoryLabel";
            this.lblPostCategoryLabel.Size = new System.Drawing.Size(54, 15);
            this.lblPostCategoryLabel.TabIndex = 2;
            this.lblPostCategoryLabel.Text = "Kategori:";
            // 
            // txtPostContent
            // 
            this.txtPostContent.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtPostContent.Location = new System.Drawing.Point(10, 32);
            this.txtPostContent.Multiline = true;
            this.txtPostContent.Name = "txtPostContent";
            this.txtPostContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPostContent.Size = new System.Drawing.Size(488, 85);
            this.txtPostContent.TabIndex = 1;
            // 
            // lblNewPostTitle
            // 
            this.lblNewPostTitle.AutoSize = true;
            this.lblNewPostTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblNewPostTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.lblNewPostTitle.Location = new System.Drawing.Point(10, 10);
            this.lblNewPostTitle.Name = "lblNewPostTitle";
            this.lblNewPostTitle.Size = new System.Drawing.Size(183, 17);
            this.lblNewPostTitle.TabIndex = 0;
            this.lblNewPostTitle.Text = "Yeni Anonim Gönderi Paylaş";
            // 
            // pnlFilter
            // 
            this.pnlFilter.BackColor = System.Drawing.Color.Transparent;
            this.pnlFilter.Controls.Add(this.txtSearch);
            this.pnlFilter.Controls.Add(this.lblSearch);
            this.pnlFilter.Controls.Add(this.cmbFilterCategory);
            this.pnlFilter.Controls.Add(this.lblFilter);
            this.pnlFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilter.Location = new System.Drawing.Point(10, 10);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(510, 45);
            this.pnlFilter.TabIndex = 0;
            // 
            // cmbFilterCategory
            // 
            this.cmbFilterCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterCategory.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.cmbFilterCategory.FormattingEnabled = true;
            this.cmbFilterCategory.Location = new System.Drawing.Point(70, 8);
            this.cmbFilterCategory.Name = "cmbFilterCategory";
            this.cmbFilterCategory.Size = new System.Drawing.Size(160, 23);
            this.cmbFilterCategory.TabIndex = 1;
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblFilter.Location = new System.Drawing.Point(5, 11);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(54, 15);
            this.lblFilter.TabIndex = 0;
            this.lblFilter.Text = "Kategori:";
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblSearch.Location = new System.Drawing.Point(240, 11);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(71, 15);
            this.lblSearch.TabIndex = 2;
            this.lblSearch.Text = "Kelime Ara:";
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtSearch.Location = new System.Drawing.Point(320, 8);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(180, 23);
            this.txtSearch.TabIndex = 3;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.pnlHeader.Controls.Add(this.btnLogout);
            this.pnlHeader.Controls.Add(this.lblActiveUser);
            this.pnlHeader.Controls.Add(this.lblHeaderTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1100, 65);
            this.pnlHeader.TabIndex = 0;
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(90)))), ((int)(((byte)(120)))));
            this.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(970, 18);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(110, 30);
            this.btnLogout.TabIndex = 2;
            this.btnLogout.Text = "Güvenli Çıkış 🚪";
            this.btnLogout.UseVisualStyleBackColor = false;
            // 
            // lblActiveUser
            // 
            this.lblActiveUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblActiveUser.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblActiveUser.ForeColor = System.Drawing.Color.White;
            this.lblActiveUser.Location = new System.Drawing.Point(620, 24);
            this.lblActiveUser.Name = "lblActiveUser";
            this.lblActiveUser.Size = new System.Drawing.Size(340, 20);
            this.lblActiveUser.TabIndex = 1;
            this.lblActiveUser.Text = "Aktif Rumuz: -";
            this.lblActiveUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHeaderTitle
            // 
            this.lblHeaderTitle.AutoSize = true;
            this.lblHeaderTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblHeaderTitle.ForeColor = System.Drawing.Color.White;
            this.lblHeaderTitle.Location = new System.Drawing.Point(15, 20);
            this.lblHeaderTitle.Name = "lblHeaderTitle";
            this.lblHeaderTitle.Size = new System.Drawing.Size(437, 25);
            this.lblHeaderTitle.TabIndex = 0;
            this.lblHeaderTitle.Text = "🌸 Lohusalık Dönemi Anonim Yardımlaşma Platformu";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1100, 700);
            this.Controls.Add(this.pnlMainContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Name = "Form1";
            this.Text = "Lohusalık Dönemi Anonim Destek Platformu";
            this.pnlMainContainer.ResumeLayout(false);
            this.pnlLoginRegister.ResumeLayout(false);
            this.pnlLoginCard.ResumeLayout(false);
            this.pnlLoginCard.PerformLayout();
            this.pnlDashboard.ResumeLayout(false);
            this.pnlRightColumn.ResumeLayout(false);
            this.pnlCommentsSection.ResumeLayout(false);
            this.pnlNewComment.ResumeLayout(false);
            this.pnlNewComment.PerformLayout();
            this.pnlSelectedPostCard.ResumeLayout(false);
            this.pnlSelectedPostCard.PerformLayout();
            this.pnlLeftColumn.ResumeLayout(false);
            this.pnlNewPost.ResumeLayout(false);
            this.pnlNewPost.PerformLayout();
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.ResumeLayout(false);
            
            // Premium Gelişmiş Özellikler Kurulumu
            this.InitializePremiumFeatures();
        }

        #endregion

        private System.Windows.Forms.Panel pnlMainContainer;
        private System.Windows.Forms.Panel pnlLoginRegister;
        private System.Windows.Forms.Panel pnlLoginCard;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Label lblLoginTitle;
        private System.Windows.Forms.Label lblLoginSubtitle;
        private System.Windows.Forms.Label lblNickname;
        private System.Windows.Forms.TextBox txtNickname;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.Panel pnlDashboard;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.Label lblActiveUser;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Panel pnlLeftColumn;
        private System.Windows.Forms.Panel pnlFilter;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.ComboBox cmbFilterCategory;
        private System.Windows.Forms.ListBox lstFeed;
        private System.Windows.Forms.Panel pnlNewPost;
        private System.Windows.Forms.Label lblNewPostTitle;
        private System.Windows.Forms.TextBox txtPostContent;
        private System.Windows.Forms.Label lblPostCategoryLabel;
        private System.Windows.Forms.ComboBox cmbPostCategory;
        private System.Windows.Forms.Button btnSharePost;
        private System.Windows.Forms.Panel pnlDivider;
        private System.Windows.Forms.Panel pnlRightColumn;
        private System.Windows.Forms.Panel pnlSelectedPostCard;
        private System.Windows.Forms.Label lblSelectedPostMeta;
        private System.Windows.Forms.TextBox txtSelectedPostContent;
        private System.Windows.Forms.Panel pnlRightSpacer;
        private System.Windows.Forms.Panel pnlCommentsSection;
        private System.Windows.Forms.Label lblCommentsHeader;
        private System.Windows.Forms.ListBox lstComments;
        private System.Windows.Forms.Panel pnlNewComment;
        private System.Windows.Forms.TextBox txtCommentContent;
        private System.Windows.Forms.Button btnShareComment;
        private System.Windows.Forms.Panel pnlDailyTip;
        private System.Windows.Forms.Label lblDailyTip;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnLikePost;
        
        // --- Yeni Premium Akademik & Klinik Özellikler Sınıf Elemanları ---
        private System.Windows.Forms.Panel pnlSubHeader;
        private System.Windows.Forms.Button btnTabFeed;
        private System.Windows.Forms.Button btnTabEpds;
        private System.Windows.Forms.Button btnTabStats;
        private System.Windows.Forms.Panel pnlEpdsTabContent;
        private System.Windows.Forms.Panel pnlStatsTabContent;
        private System.Windows.Forms.Panel pnlEpdsCard;
        private System.Windows.Forms.Label lblEpdsTitle;
        private System.Windows.Forms.Label lblEpdsSubtitle;
        private System.Windows.Forms.Panel pnlEpdsWelcome;
        private System.Windows.Forms.Label lblEpdsWelcome;
        private System.Windows.Forms.Button btnEpdsStart;
        private System.Windows.Forms.Panel pnlEpdsQuiz;
        private System.Windows.Forms.Label lblEpdsQNo;
        private System.Windows.Forms.Label lblEpdsQuestion;
        private System.Windows.Forms.RadioButton rbEpdsOpt1;
        private System.Windows.Forms.RadioButton rbEpdsOpt2;
        private System.Windows.Forms.RadioButton rbEpdsOpt3;
        private System.Windows.Forms.RadioButton rbEpdsOpt4;
        private System.Windows.Forms.Button btnEpdsNext;
        private System.Windows.Forms.Panel pnlEpdsResult;
        private System.Windows.Forms.Label lblEpdsResultScore;
        private System.Windows.Forms.Label lblEpdsResultTitle;
        private System.Windows.Forms.Label lblEpdsResultDesc;
        private System.Windows.Forms.Button btnEpdsRestart;
        private System.Windows.Forms.Panel pnlStatsLeft;
        private System.Windows.Forms.Panel pnlStatsCard;
        private System.Windows.Forms.Label lblStatsTitle;
        private System.Windows.Forms.Label lblStatsTotalPosts;
        private System.Windows.Forms.Label lblStatsTotalLikes;
        private System.Windows.Forms.Label lblStatsTotalComments;
        private System.Windows.Forms.Label lblStatsBreakdownTitle;
        private System.Windows.Forms.Label lblBreakdownPsikolojik;
        private System.Windows.Forms.Panel pbarPsikolojik;
        private System.Windows.Forms.Panel pbarPsikolojikFill;
        private System.Windows.Forms.Label lblBreakdownBebek;
        private System.Windows.Forms.Panel pbarBebek;
        private System.Windows.Forms.Panel pbarBebekFill;
        private System.Windows.Forms.Label lblBreakdownBeslenme;
        private System.Windows.Forms.Panel pbarBeslenme;
        private System.Windows.Forms.Panel pbarBeslenmeFill;
        private System.Windows.Forms.Panel pnlStatsRight;
        private System.Windows.Forms.Panel pnlSupportDirectoryCard;
        private System.Windows.Forms.Label lblDirectoryTitle;
        private System.Windows.Forms.Label lblDirectorySubtitle;
        private System.Windows.Forms.Button btnHotline191;
        private System.Windows.Forms.Button btnHotline182;
        private System.Windows.Forms.Button btnHotlineKades;
        private System.Windows.Forms.Button btnShowContract;

        // --- Phase 2: Mood Tracker & Expert FAQ UI Controls ---
        private System.Windows.Forms.Button btnTabMood;
        private System.Windows.Forms.Button btnTabExpert;
        private System.Windows.Forms.Panel pnlMoodTabContent;
        private System.Windows.Forms.Panel pnlMoodCard;
        private System.Windows.Forms.Label lblMoodTitle;
        private System.Windows.Forms.Label lblMoodSubtitle;
        private System.Windows.Forms.Label lblMoodPrompt;
        private System.Windows.Forms.Button btnMoodEmoji1;
        private System.Windows.Forms.Button btnMoodEmoji2;
        private System.Windows.Forms.Button btnMoodEmoji3;
        private System.Windows.Forms.Button btnMoodEmoji4;
        private System.Windows.Forms.Button btnMoodEmoji5;
        private System.Windows.Forms.Label lblMoodHistoryHeader;
        private System.Windows.Forms.ListBox lstMoodHistory;
        
        private System.Windows.Forms.Panel pnlExpertTabContent;
        private System.Windows.Forms.Panel pnlExpertCard;
        private System.Windows.Forms.Label lblExpertTitle;
        private System.Windows.Forms.Label lblExpertSubtitle;
        private System.Windows.Forms.Panel pnlFaq1;
        private System.Windows.Forms.Label lblFaqQ1;
        private System.Windows.Forms.Label lblFaqA1;
        private System.Windows.Forms.Panel pnlFaq2;
        private System.Windows.Forms.Label lblFaqQ2;
        private System.Windows.Forms.Label lblFaqA2;
        private System.Windows.Forms.Panel pnlFaq3;
        private System.Windows.Forms.Label lblFaqQ3;
        private System.Windows.Forms.Label lblFaqA3;
        private System.Windows.Forms.Panel pnlFaq4;
        private System.Windows.Forms.Label lblFaqQ4;
        private System.Windows.Forms.Label lblFaqA4;
    }
}
