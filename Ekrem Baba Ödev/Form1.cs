using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Linq;

namespace LohusaDestekApp
{
    public partial class Form1 : Form
    {
        // Platformun yerel veritabanı nesnesi ve aktif kullanıcı bilgisi
        private Database db;
        private User currentUser;
        private Post selectedPost;

        // --- Yeni Premium Akademik & Klinik Özellik Değişkenleri ---
        private List<EpdsQuestion> epdsQuestions;
        private int epdsCurrentQuestionIndex = 0;
        private int epdsTotalScore = 0;

        // --- Dinamik Premium Modifikasyonlar ---
        private Button btnDeletePost;
        private Button btnReportPost;
        private TextBox txtPeerIP;
        private Button btnConnectPeer;
        private Button btnShowMyIP;
        private ListBox lstPeerIPs;
        private Label lblActivePeersIndicator;

        public Form1()
        {
            InitializeComponent();
            
            // Bileşenlerin ek kurulumları ve olay bağlamaları (Garanti olması açısından)
            this.Load += new EventHandler(Form1_Load);
            this.Resize += new EventHandler(Form1_Resize);
            
            this.btnLogin.Click += new EventHandler(btnLogin_Click);
            this.btnRegister.Click += new EventHandler(btnRegister_Click);
            this.btnLogout.Click += new EventHandler(btnLogout_Click);
            
            this.btnSharePost.Click += new EventHandler(btnSharePost_Click);
            this.btnShareComment.Click += new EventHandler(btnShareComment_Click);
            
            this.cmbFilterCategory.SelectedIndexChanged += new EventHandler(cmbFilterCategory_SelectedIndexChanged);
            this.lstFeed.SelectedIndexChanged += new EventHandler(lstFeed_SelectedIndexChanged);
            
            this.lstFeed.DrawItem += new DrawItemEventHandler(lstFeed_DrawItem);
            this.lstComments.DrawItem += new DrawItemEventHandler(lstComments_DrawItem);

            // Yeni akıllı özelliklerin olay bağlamaları
            this.txtSearch.TextChanged += new EventHandler(txtSearch_TextChanged);
            this.btnLikePost.Click += new EventHandler(btnLikePost_Click);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Formun boyutunu ve sınırlarını dinamikleştirerek büyütmeyi açalım
            this.ClientSize = new Size(1100, 700);
            this.MinimumSize = new Size(1100, 750);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Resize += new EventHandler(Form1_Resize);

            // Kontrollerin esnek büyümesi için Anchor'ları dinamik atayalım
            txtSelectedPostContent.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            btnLikePost.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            txtCommentContent.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            btnShareComment.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

            // Verileri yerel XML dosyasından yükleyelim (İlk açılışta örnek veriler otomatik eklenir)
            db = DataHelper.LoadData();
            DataHelper.Log("[SİSTEM] Lohusalık Anonim Yardımlaşma Platformu başarıyla başlatıldı.");

            // Yerel Wi-Fi/Ağ dinleyici thread başlat
            StartWifiListener();

            // Formun ve bileşenlerin premium SaaS (Indigo & Slate) renk paleti programatik olarak atanır
            this.BackColor = Color.FromArgb(245, 247, 250); // Slack/Notion tarzı açık gri-mavi arkaplan
            pnlLoginRegister.BackColor = Color.FromArgb(240, 244, 248);
            pnlHeader.BackColor = Color.FromArgb(31, 38, 57); // Asil Koyu Lacivert/Indigo Header
            btnLogout.BackColor = Color.FromArgb(235, 90, 120); // Soft, belirgin pembe/kırmızı çıkış butonu
            
            pnlLeftColumn.BackColor = Color.FromArgb(245, 247, 250);
            pnlRightColumn.BackColor = Color.FromArgb(245, 247, 250);
            pnlDivider.BackColor = Color.FromArgb(228, 231, 239); // Zarif bölücü çizgisi
            
            btnLogin.BackColor = Color.FromArgb(90, 100, 230); // Giriş butonu asil indigo
            btnRegister.FlatAppearance.BorderColor = Color.FromArgb(90, 100, 230);
            btnRegister.ForeColor = Color.FromArgb(90, 100, 230);
            btnSharePost.BackColor = Color.FromArgb(90, 100, 230); // Paylaş butonu indigo
            btnShareComment.BackColor = Color.FromArgb(90, 100, 230);
            btnLikePost.BackColor = Color.FromArgb(235, 240, 255); // Empati butonu asil indigo
            btnLikePost.ForeColor = Color.FromArgb(90, 100, 230);

            // Günün tavsiyesi panelini asil indigo/pastel maviye dönüştür
            pnlDailyTip.BackColor = Color.FromArgb(235, 240, 255);
            lblDailyTip.ForeColor = Color.FromArgb(60, 80, 180);

            // Köşeleri pürüzsüzce yuvarlat
            RoundedControlHelper.MakeRounded(pnlLoginCard, 20);
            RoundedControlHelper.MakeRounded(btnLogin, 8);
            RoundedControlHelper.MakeRounded(btnRegister, 8);
            RoundedControlHelper.MakeRounded(btnLogout, 8);
            RoundedControlHelper.MakeRounded(pnlSelectedPostCard, 12);
            RoundedControlHelper.MakeRounded(pnlNewPost, 12);
            RoundedControlHelper.MakeRounded(btnSharePost, 8);
            RoundedControlHelper.MakeRounded(btnShareComment, 8);
            RoundedControlHelper.MakeRounded(btnLikePost, 8);
            RoundedControlHelper.MakeRounded(pnlDailyTip, 8);

            // Yeni Premium Bileşenlerin Köşe Yuvarlatmaları
            RoundedControlHelper.MakeRounded(pnlEpdsCard, 16);
            RoundedControlHelper.MakeRounded(pnlStatsCard, 16);
            RoundedControlHelper.MakeRounded(pnlSupportDirectoryCard, 16);
            RoundedControlHelper.MakeRounded(btnTabFeed, 8);
            RoundedControlHelper.MakeRounded(btnTabEpds, 8);
            RoundedControlHelper.MakeRounded(btnTabMood, 8);
            RoundedControlHelper.MakeRounded(btnTabExpert, 8);
            RoundedControlHelper.MakeRounded(btnTabStats, 8);
            RoundedControlHelper.MakeRounded(btnEpdsStart, 8);
            RoundedControlHelper.MakeRounded(btnEpdsNext, 8);
            RoundedControlHelper.MakeRounded(btnEpdsRestart, 8);
            RoundedControlHelper.MakeRounded(btnHotline191, 8);
            RoundedControlHelper.MakeRounded(btnHotline182, 8);
            RoundedControlHelper.MakeRounded(btnHotlineKades, 8);
            RoundedControlHelper.MakeRounded(btnShowContract, 8);

            // Phase 2 Rounding
            RoundedControlHelper.MakeRounded(pnlMoodCard, 16);
            RoundedControlHelper.MakeRounded(pnlExpertCard, 16);
            RoundedControlHelper.MakeRounded(btnMoodEmoji1, 12);
            RoundedControlHelper.MakeRounded(btnMoodEmoji2, 12);
            RoundedControlHelper.MakeRounded(btnMoodEmoji3, 12);
            RoundedControlHelper.MakeRounded(btnMoodEmoji4, 12);
            RoundedControlHelper.MakeRounded(btnMoodEmoji5, 12);
            RoundedControlHelper.MakeRounded(pnlFaq1, 12);
            RoundedControlHelper.MakeRounded(pnlFaq2, 12);
            RoundedControlHelper.MakeRounded(pnlFaq3, 12);
            RoundedControlHelper.MakeRounded(pnlFaq4, 12);

            // GDI+ Premium Degrade Giriş Ekranı Boyama Event Bağlantısı
            pnlLoginRegister.Paint += (s, pe) =>
            {
                using (System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                    pnlLoginRegister.ClientRectangle,
                    Color.FromArgb(240, 244, 248), // Premium Çelik Gri
                    Color.FromArgb(220, 225, 245), // Premium Lavanta-Slate
                    45F))
                {
                    pe.Graphics.FillRectangle(brush, pnlLoginRegister.ClientRectangle);
                }
            };

            // ListBox sınırlarını modernleştirmek için çerçeveleri kaldırıp özel renkleri ata
            lstFeed.BorderStyle = BorderStyle.None;
            lstFeed.BackColor = Color.FromArgb(245, 247, 250);
            lstComments.BorderStyle = BorderStyle.None;
            lstComments.BackColor = Color.FromArgb(245, 247, 250);

            InitializeDailyTip();

            // Kategori ComboBox'larını dolduralım
            cmbFilterCategory.Items.Clear();
            cmbFilterCategory.Items.AddRange(new string[] { "Hepsi", "Psikolojik Destek", "Bebek Bakımı", "Beslenme & Sağlık" });
            cmbFilterCategory.SelectedIndex = 0; // "Hepsi" seçili başlar

            cmbPostCategory.Items.Clear();
            cmbPostCategory.Items.AddRange(new string[] { "Psikolojik Destek", "Bebek Bakımı", "Beslenme & Sağlık" });
            cmbPostCategory.SelectedIndex = 0; // İlk kategori seçili başlar

            // Dinamik Eklenen Premium Butonların Köşelerini Yuvarla
            if (btnDeletePost != null) RoundedControlHelper.MakeRounded(btnDeletePost, 8);
            if (btnReportPost != null) RoundedControlHelper.MakeRounded(btnReportPost, 8);
            if (btnConnectPeer != null) RoundedControlHelper.MakeRounded(btnConnectPeer, 6);
            if (btnShowMyIP != null) RoundedControlHelper.MakeRounded(btnShowMyIP, 6);
            if (lstPeerIPs != null) RoundedControlHelper.MakeRounded(lstPeerIPs, 8);

            // Kayıtlı Farklı Ağlardaki Annelerin IP Adreslerini Listeye Doldur
            if (db != null && db.CustomPeers != null && lstPeerIPs != null)
            {
                lstPeerIPs.Items.Clear();
                foreach (string peer in db.CustomPeers)
                {
                    lstPeerIPs.Items.Add(peer);
                }
            }

            // Kullanıcı giriş yapmamış olarak başlat
            ShowLoginPanel();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // Form boyutu değişirse veya yüklendiğinde Giriş Kartını tam ortalayalım
            CenterLoginCard();

            // Edinburgh (EPDS) Kartını merkezleyelim
            if (pnlEpdsCard != null && pnlEpdsTabContent != null)
            {
                int epdsX = Math.Max(15, (pnlEpdsTabContent.Width - pnlEpdsCard.Width) / 2);
                int epdsY = Math.Max(15, (pnlEpdsTabContent.Height - pnlEpdsCard.Height) / 2);
                pnlEpdsCard.Location = new Point(epdsX, epdsY);
            }
            // Duygu Durum (Mood) Kartını merkezleyelim
            if (pnlMoodCard != null && pnlMoodTabContent != null)
            {
                int moodX = Math.Max(15, (pnlMoodTabContent.Width - pnlMoodCard.Width) / 2);
                int moodY = Math.Max(15, (pnlMoodTabContent.Height - pnlMoodCard.Height) / 2);
                pnlMoodCard.Location = new Point(moodX, moodY);
            }
            // Uzman FAQ Kartını merkezleyelim
            if (pnlExpertCard != null && pnlExpertTabContent != null)
            {
                int expertX = Math.Max(15, (pnlExpertTabContent.Width - pnlExpertCard.Width) / 2);
                int expertY = Math.Max(15, (pnlExpertTabContent.Height - pnlExpertCard.Height) / 2);
                pnlExpertCard.Location = new Point(expertX, expertY);
            }
        }

        private void CenterLoginCard()
        {
            if (pnlLoginRegister != null && pnlLoginCard != null)
            {
                pnlLoginCard.Left = (pnlLoginRegister.Width - pnlLoginCard.Width) / 2;
                pnlLoginCard.Top = (pnlLoginRegister.Height - pnlLoginCard.Height) / 2;
            }
        }

        // --- Panel Gösterim Kontrolleri ---
        private void ShowLoginPanel()
        {
            currentUser = null;
            selectedPost = null;
            
            txtNickname.Clear();
            txtPassword.Clear();
            
            pnlLoginRegister.Visible = true;
            pnlDashboard.Visible = false;
            pnlLoginRegister.BringToFront();
            
            CenterLoginCard();
        }

        private void ShowDashboardPanel()
        {
            pnlLoginRegister.Visible = false;
            pnlDashboard.Visible = true;
            pnlDashboard.BringToFront();

            // Aktif kullanıcı bilgisini header'da gösterelim (Kare karakter hatasını önlemek için ikonsuz temiz metin)
            lblActiveUser.Text = "Aktif Anne: " + currentUser.Nickname;
            
            // Günün tavsiyesini her girişte yenileyelim
            InitializeDailyTip();

            // Akışı yenileyelim
            RefreshFeed();
        }

        // --- Giriş & Kayıt Sistemi İşlemleri ---
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string nickname = txtNickname.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Lütfen rumuz ve şifre alanlarını boş bırakmayınız.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kullanıcıyı veritabanında ara
            User user = db.Users.Find(u => u.Nickname.Equals(nickname, StringComparison.OrdinalIgnoreCase));

            if (user != null && user.Password == password)
            {
                currentUser = user;
                DataHelper.Log("[GİRİŞ] Anonim kullanıcı giriş yaptı: " + currentUser.Nickname);
                ShowDashboardPanel();
            }
            else
            {
                MessageBox.Show("Girdiğiniz rumuz veya şifre hatalı! Lütfen kontrol ediniz.", "Hatalı Giriş", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string nickname = txtNickname.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Hesap oluşturmak için lütfen bir rumuz ve şifre belirleyiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (nickname.Length < 3)
            {
                MessageBox.Show("Rumuz en az 3 karakter olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Aynı rumuzla kayıtlı kullanıcı var mı kontrol et
            bool exists = db.Users.Exists(u => u.Nickname.Equals(nickname, StringComparison.OrdinalIgnoreCase));
            if (exists)
            {
                MessageBox.Show("Bu rumuz (nickname) zaten başka bir anne tarafından alınmış. Lütfen kendinize özgün başka bir rumuz belirleyin.", "Rumuz Dolu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Yeni kullanıcıyı kaydet
            User newUser = new User(nickname, password);
            db.Users.Add(newUser);
            DataHelper.SaveData(db);
            DataHelper.Log("[KAYIT] Yeni anonim kullanıcı kayıt oldu: " + nickname);

            MessageBox.Show("'" + nickname + "' rumuzu ile anonim hesabınız başarıyla oluşturuldu! Şimdi bu bilgilerle giriş yapabilirsiniz.", "Kayıt Başarılı 🌸", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Hesabınızdan çıkış yapmak istediğinize emin misiniz?", "Çıkış Yap", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                DataHelper.Log("[ÇIKIŞ] Anonim kullanıcı çıkış yaptı: " + (currentUser != null ? currentUser.Nickname : ""));
                ShowLoginPanel();
            }
        }

        // --- Gönderi (Post) Akışı Yönetimi ---
        private void RefreshFeed()
        {
            lstFeed.BeginUpdate();
            lstFeed.Items.Clear();

            string selectedFilter = "Hepsi";
            if (cmbFilterCategory.SelectedItem != null)
            {
                selectedFilter = cmbFilterCategory.SelectedItem.ToString();
            }

            string searchText = txtSearch.Text.Trim().ToLowerInvariant();

            // Gönderileri en yeni en üstte olacak şekilde ters sırada ekleyelim
            for (int i = db.Posts.Count - 1; i >= 0; i--)
            {
                Post post = db.Posts[i];
                bool matchesCategory = (selectedFilter == "Hepsi" || post.Category == selectedFilter);
                
                // Kelime Arama kontrolü (İçerik veya Rumuz içinde)
                bool matchesSearch = string.IsNullOrEmpty(searchText) || 
                                     post.Content.ToLowerInvariant().Contains(searchText) || 
                                     post.Nickname.ToLowerInvariant().Contains(searchText);

                if (matchesCategory && matchesSearch)
                {
                    lstFeed.Items.Add(post);
                }
            }

            lstFeed.EndUpdate();

            // Eğer akışta gönderi varsa ilkini otomatik seç, yoksa sağ paneli temizle
            if (lstFeed.Items.Count > 0)
            {
                lstFeed.SelectedIndex = 0;
            }
            else
            {
                ClearSelectedPostDetails();
            }
        }

        private void cmbFilterCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshFeed();
        }

        private void lstFeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            Post post = lstFeed.SelectedItem as Post;
            if (post != null)
            {
                selectedPost = post;
                LoadSelectedPostDetails(post);
            }
            else
            {
                ClearSelectedPostDetails();
            }
        }

        private void ClearSelectedPostDetails()
        {
            selectedPost = null;
            lblSelectedPostMeta.Text = "Seçili Gönderi Yok";
            txtSelectedPostContent.Text = "Lütfen sol taraftaki akıştan okumak veya yorum yapmak istediğiniz bir paylaşımı seçin.";
            lstComments.Items.Clear();
            
            // Yorum paneli girişlerini pasif yap
            txtCommentContent.Clear();
            txtCommentContent.Enabled = false;
            btnShareComment.Enabled = false;

            // Beğeni butonunu pasif yap
            btnLikePost.Enabled = false;
            btnLikePost.Text = "Ben de Yaşıyorum (0) ♡";

            // Dinamik modifikasyonları gizle
            if (btnDeletePost != null) btnDeletePost.Visible = false;
            if (btnReportPost != null) btnReportPost.Visible = false;
        }

        private void LoadSelectedPostDetails(Post post)
        {
            // Kare karakter hatasını önlemek için emoji yerine temiz metin etiketleri
            lblSelectedPostMeta.Text = "Yazar: " + post.Nickname + "   •   Tarih: " + post.CreatedAt.ToString("dd.MM.yyyy HH:mm") + "   •   Kategori: " + post.Category;
            txtSelectedPostContent.Text = post.Content;

            // Yorumları yükle
            lstComments.BeginUpdate();
            lstComments.Items.Clear();
            foreach (var comment in post.Comments)
            {
                lstComments.Items.Add(comment);
            }
            lstComments.EndUpdate();

            // Yorum girişlerini aktif yap
            txtCommentContent.Enabled = true;
            btnShareComment.Enabled = true;

            // Beğeni butonunu aktif et ve sayısını güncelle
            btnLikePost.Enabled = true;
            btnLikePost.Text = "Ben de Yaşıyorum (" + post.LikeCount + ") ♥";

            // Gönderi sahipliğine göre buton durumlarını güncelle
            if (btnDeletePost != null && btnReportPost != null)
            {
                if (currentUser == null)
                {
                    btnDeletePost.Visible = false;
                    btnReportPost.Visible = false;
                }
                else if (post.Nickname == currentUser.Nickname)
                {
                    btnDeletePost.Visible = true;
                    btnReportPost.Visible = false;
                }
                else
                {
                    btnDeletePost.Visible = false;
                    btnReportPost.Visible = true;
                    
                    // Eğer kullanıcı bu gönderiyi zaten şikayet ettiyse şikayet butonunu pasifleştir
                    if (post.ReportedBy != null && post.ReportedBy.Contains(currentUser.Nickname))
                    {
                        btnReportPost.Enabled = false;
                        btnReportPost.Text = "🚩 Şikayet Edildi";
                        btnReportPost.BackColor = Color.FromArgb(240, 240, 240);
                    }
                    else
                    {
                        btnReportPost.Enabled = true;
                        btnReportPost.Text = "🚩 Şikayet Et";
                        btnReportPost.BackColor = Color.FromArgb(255, 243, 230);
                    }
                }
            }
        }

        // --- Yeni Gönderi ve Yorum Ekleme ---
        private void btnSharePost_Click(object sender, EventArgs e)
        {
            if (currentUser == null) return;
            string content = txtPostContent.Text.Trim();
            string category = null;
            if (cmbPostCategory.SelectedItem != null)
            {
                category = cmbPostCategory.SelectedItem.ToString();
            }

            if (string.IsNullOrEmpty(content))
            {
                MessageBox.Show("Lütfen içinizi dökmek veya soru sormak için bir şeyler yazın.", "Boş Gönderi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(category))
            {
                MessageBox.Show("Lütfen gönderiniz için bir kategori seçin.", "Kategori Seçilmedi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Küfür & Hakaret Otomatik Sansürleme ve Temizleme Filtresi
            content = CleanInappropriateLanguage(content);

            // Yeni gönderi oluştur ve kaydet
            Post newPost = new Post(currentUser.Nickname, content, category);
            db.Posts.Add(newPost);
            DataHelper.SaveData(db);
            DataHelper.Log("[GÖNDERİ] Anonim gönderi paylaşıldı (Kat: " + category + "): " + content);
            BroadcastPacket(new NetworkPacket("NEW_POST", DataHelper.SerializeToXml(newPost)));

            // Girdiyi sıfırla
            txtPostContent.Clear();
            txtSearch.Clear(); // Yeni paylaşılan gönderiyi anında görmek için aramayı temizle

            // Akışı güncelle ve yeni gönderiyi otomatik seçmek için yenile
            RefreshFeed();
            
            MessageBox.Show("Gönderiniz tamamen anonim olarak başarıyla paylaşıldı!", "Paylaşıldı 🌸", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnShareComment_Click(object sender, EventArgs e)
        {
            if (currentUser == null) return;
            if (selectedPost == null)
            {
                MessageBox.Show("Lütfen önce yorum yapmak istediğiniz gönderiyi seçin.", "Gönderi Seçilmedi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string content = txtCommentContent.Text.Trim();

            if (string.IsNullOrEmpty(content))
            {
                MessageBox.Show("Lütfen paylaşmak istediğiniz yorumu veya destek mesajını yazın.", "Boş Yorum", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Küfür & Hakaret Otomatik Sansürleme ve Temizleme Filtresi
            content = CleanInappropriateLanguage(content);

            // Yeni yorumu oluştur
            Comment newComment = new Comment(currentUser.Nickname, content);
            
            // Ana veritabanındaki asıl gönderiyi bularak yorumu oraya ekleyelim
            Post originalPost = db.Posts.Find(p => p.Id == selectedPost.Id);
            if (originalPost != null)
            {
                originalPost.Comments.Add(newComment);
                DataHelper.SaveData(db);
                DataHelper.Log("[YORUM] Yeni yorum yazıldı: " + content);
                BroadcastPacket(new NetworkPacket("NEW_COMMENT", DataHelper.SerializeToXml(newComment), originalPost.Id));
                
                // Girdiyi sıfırla
                txtCommentContent.Clear();

                // Detayları yeniden yükle
                LoadSelectedPostDetails(originalPost);
            }
        }

        // --- Yeni Entegre Edilen Akıllı Metotlar ---
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            RefreshFeed();
        }

        private void btnLikePost_Click(object sender, EventArgs e)
        {
            if (selectedPost != null)
            {
                // XML ve Bellekteki ana nesneyi güncelle
                Post originalPost = db.Posts.Find(p => p.Id == selectedPost.Id);
                if (originalPost != null)
                {
                    originalPost.LikeCount++;
                    DataHelper.SaveData(db);
                    DataHelper.Log("[EMPATİ] Gönderi beğenildi: " + originalPost.Id);
                    BroadcastPacket(new NetworkPacket("LIKE_POST", originalPost.LikeCount.ToString(), originalPost.Id));
                    
                    selectedPost.LikeCount = originalPost.LikeCount;
                    btnLikePost.Text = "Ben de Yaşıyorum (" + selectedPost.LikeCount + ") ♥";
                    
                    // Akış ListBox'ını anlık güncelle
                    RefreshFeed();
                }
            }
        }

        // --- YEREL WI-FI / LAN P2P SENKRONIZASYON SOKET SISTEMI ---
        private System.Net.Sockets.UdpClient udpListener;
        private System.Threading.Thread wifiListenerThread;
        private bool isListening = false;
        private const int WifiPort = 19000;

        private void StartWifiListener()
        {
            try
            {
                udpListener = new System.Net.Sockets.UdpClient(WifiPort);
                udpListener.EnableBroadcast = true;
                isListening = true;

                wifiListenerThread = new System.Threading.Thread(ListenForWifiPackets);
                wifiListenerThread.IsBackground = true;
                wifiListenerThread.Start();

                DataHelper.Log("[SİSTEM] Yerel Wi-Fi/Ağ dinleyici soketi başarıyla başlatıldı. Port: " + WifiPort);
            }
            catch (Exception ex)
            {
                DataHelper.Log("[HATA] Wi-Fi dinleyicisi başlatılamadı (Büyük ihtimalle başka bir uygulama veya sekme bu portu kullanıyor): " + ex.Message);
            }
        }

        private void ListenForWifiPackets()
        {
            System.Net.IPEndPoint remoteEP = new System.Net.IPEndPoint(System.Net.IPAddress.Any, 0);
            while (isListening)
            {
                try
                {
                    byte[] bytes = udpListener.Receive(ref remoteEP);
                    string xmlData = System.Text.Encoding.UTF8.GetString(bytes);

                    NetworkPacket packet = DataHelper.DeserializeFromXml<NetworkPacket>(xmlData);
                    if (packet != null)
                    {
                        this.Invoke(new Action(() => ProcessIncomingNetworkPacket(packet)));
                    }
                }
                catch
                {
                    // Dinleyici sonlandırıldığında oluşacak hatayı yutar
                }
            }
        }

        private void ProcessIncomingNetworkPacket(NetworkPacket packet)
        {
            try
            {
                if (packet.Type == "NEW_POST")
                {
                    Post post = DataHelper.DeserializeFromXml<Post>(packet.Payload);
                    if (post != null && !db.Posts.Any(p => p.Id == post.Id))
                    {
                        db.Posts.Add(post);
                        DataHelper.SaveData(db);
                        DataHelper.Log("[AĞ SENKRON] Wi-Fi üzerinden yeni anonim gönderi alındı ve veritabanına işlendi (Rumuz: " + post.Nickname + ")");
                        RefreshFeed();
                    }
                }
                else if (packet.Type == "NEW_COMMENT")
                {
                    Comment comment = DataHelper.DeserializeFromXml<Comment>(packet.Payload);
                    string postId = packet.Extra;
                    Post targetPost = db.Posts.FirstOrDefault(p => p.Id == postId);
                    if (targetPost != null && comment != null)
                    {
                        if (!targetPost.Comments.Any(c => c.Nickname == comment.Nickname && c.Content == comment.Content && c.CreatedAt == comment.CreatedAt))
                        {
                            targetPost.Comments.Add(comment);
                            DataHelper.SaveData(db);
                            DataHelper.Log("[AĞ SENKRON] Wi-Fi üzerinden yeni anonim yorum alındı ve veritabanına işlendi (Rumuz: " + comment.Nickname + ")");

                            if (selectedPost != null && selectedPost.Id == postId)
                            {
                                LoadSelectedPostDetails(targetPost);
                            }
                            else
                            {
                                RefreshFeed();
                            }
                        }
                    }
                }
                else if (packet.Type == "LIKE_POST")
                {
                    string postId = packet.Extra;
                    int likeCount = int.Parse(packet.Payload);
                    Post targetPost = db.Posts.FirstOrDefault(p => p.Id == postId);
                    if (targetPost != null && targetPost.LikeCount < likeCount)
                    {
                        targetPost.LikeCount = likeCount;
                        DataHelper.SaveData(db);
                        DataHelper.Log("[AĞ SENKRON] Wi-Fi üzerinden beğeni eşleşmesi alındı. Gönderi ID: " + postId + ", Beğeni: " + likeCount);

                        if (selectedPost != null && selectedPost.Id == postId)
                        {
                            selectedPost.LikeCount = likeCount;
                            btnLikePost.Text = "Ben de Yaşıyorum (" + likeCount + ") ♥";
                        }
                        RefreshFeed();
                    }
                }
                else if (packet.Type == "DELETE_POST")
                {
                    string postId = packet.Payload;
                    if (db.Posts.Any(p => p.Id == postId))
                    {
                        db.Posts.RemoveAll(p => p.Id == postId);
                        DataHelper.SaveData(db);
                        DataHelper.Log("[AĞ SENKRON] Wi-Fi üzerinden silme komutu alındı ve gönderi silindi. ID: " + postId);
                        
                        if (selectedPost != null && selectedPost.Id == postId)
                        {
                            ClearSelectedPostDetails();
                        }
                        RefreshFeed();
                    }
                }
                else if (packet.Type == "REPORT_POST")
                {
                    string postId = packet.Extra;
                    string reporter = packet.Payload;
                    Post targetPost = db.Posts.FirstOrDefault(p => p.Id == postId);
                    if (targetPost != null)
                    {
                        if (targetPost.ReportedBy == null) targetPost.ReportedBy = new List<string>();
                        if (!targetPost.ReportedBy.Contains(reporter))
                        {
                            targetPost.ReportedBy.Add(reporter);
                            if (targetPost.ReportedBy.Count >= 3)
                            {
                                targetPost.Content = "⚠️ Bu gönderi topluluk kuralları ihlali şikayetleri nedeniyle inceleme altındadır ve içeriği geçici olarak gizlenmiştir.";
                            }
                            DataHelper.SaveData(db);
                            DataHelper.Log("[AĞ SENKRON] Şikayet eşleşmesi alındı. Gönderi ID: " + postId + ", Bildiren: " + reporter);

                            if (selectedPost != null && selectedPost.Id == postId)
                            {
                                LoadSelectedPostDetails(targetPost);
                            }
                            RefreshFeed();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DataHelper.Log("[WIFI HATA] Gelen veri paketi işlenirken hata oluştu: " + ex.Message);
            }
        }

        private void BroadcastPacket(NetworkPacket packet)
        {
            try
            {
                using (System.Net.Sockets.UdpClient sender = new System.Net.Sockets.UdpClient())
                {
                    sender.EnableBroadcast = true;
                    string xml = DataHelper.SerializeToXml(packet);
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(xml);
                    
                    // 1. Yerel Ağ (Subnet Broadcast)
                    sender.Send(bytes, bytes.Length, new System.Net.IPEndPoint(System.Net.IPAddress.Broadcast, WifiPort));
                    
                    // 2. Farklı Ağlardaki Tanımlı Anneler (Direct Peer Unicast)
                    if (db != null && db.CustomPeers != null)
                    {
                        foreach (string peerIp in db.CustomPeers)
                        {
                            try
                            {
                                sender.Send(bytes, bytes.Length, new System.Net.IPEndPoint(System.Net.IPAddress.Parse(peerIp), WifiPort));
                            }
                            catch { }
                        }
                    }
                }
                DataHelper.Log("[AĞ YAYIN] Gönderi paketi yerel ağa (Wi-Fi) ve harici eşlere yayınlandı. Tip: " + packet.Type);
            }
            catch (Exception ex)
            {
                DataHelper.Log("[WIFI HATA] Paket ağa yayınlanırken hata oluştu: " + ex.Message);
            }
        }

        // --- Dinamik Premium Modifikasyonlar Click Event Handlers ---
        private void btnDeletePost_Click(object sender, EventArgs e)
        {
            if (selectedPost == null || currentUser == null) return;

            DialogResult res = MessageBox.Show("Bu gönderiyi silmek istediğinize emin misiniz? Bu işlem geri alınamaz.", "Gönderiyi Sil", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res == DialogResult.Yes)
            {
                string deletedPostId = selectedPost.Id;
                
                // Veritabanından sil
                db.Posts.RemoveAll(p => p.Id == deletedPostId);
                DataHelper.SaveData(db);
                DataHelper.Log("[SİLME] Anne kendi gönderisini sildi. ID: " + deletedPostId);

                // Ağa silme paketini yayınla
                BroadcastPacket(new NetworkPacket("DELETE_POST", deletedPostId));

                // Temizle ve yenile
                ClearSelectedPostDetails();
                RefreshFeed();
                MessageBox.Show("Gönderiniz başarıyla silindi ve tüm ağdaki cihazlarla senkronize edildi.", "Gönderi Silindi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnReportPost_Click(object sender, EventArgs e)
        {
            if (selectedPost == null || currentUser == null) return;

            Post originalPost = db.Posts.Find(p => p.Id == selectedPost.Id);
            if (originalPost != null)
            {
                if (originalPost.ReportedBy == null) originalPost.ReportedBy = new List<string>();

                if (originalPost.ReportedBy.Contains(currentUser.Nickname))
                {
                    MessageBox.Show("Bu gönderiyi zaten şikayet ettiniz.", "Şikayet Alındı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                originalPost.ReportedBy.Add(currentUser.Nickname);
                
                // Şikayet sayısı 3 veya daha fazla ise içeriği otomatik olarak sansürle/gizle!
                if (originalPost.ReportedBy.Count >= 3)
                {
                    originalPost.Content = "⚠️ Bu gönderi topluluk kuralları ihlali şikayetleri nedeniyle inceleme altındadır ve içeriği geçici olarak gizlenmiştir.";
                    DataHelper.Log("[MODERASYON] Gönderi 3 şikayete ulaştığı için otomatik gizlendi. ID: " + originalPost.Id);
                }

                DataHelper.SaveData(db);
                DataHelper.Log("[ŞİKAYET] Gönderi bildirildi. ID: " + originalPost.Id + ", Bildiren: " + currentUser.Nickname);

                // Ağa bildir
                BroadcastPacket(new NetworkPacket("REPORT_POST", currentUser.Nickname, originalPost.Id));

                // Arayüzü yenile
                LoadSelectedPostDetails(originalPost);
                RefreshFeed();

                MessageBox.Show("Gönderi şikayetiniz başarıyla alındı. Platformumuzun huzuru için gösterdiğiniz hassasiyet için teşekkür ederiz! 🌸", "Bildirim Alındı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnConnectPeer_Click(object sender, EventArgs e)
        {
            string ipText = txtPeerIP.Text.Trim();
            if (string.IsNullOrEmpty(ipText))
            {
                MessageBox.Show("Lütfen geçerli bir IP adresi girin.", "Geçersiz IP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            System.Net.IPAddress tempIp;
            if (!System.Net.IPAddress.TryParse(ipText, out tempIp))
            {
                MessageBox.Show("Girilen değer standart bir IP adresi formatında değil. Örn: 192.168.1.50", "Format Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (db.CustomPeers == null) db.CustomPeers = new List<string>();

            if (db.CustomPeers.Contains(ipText))
            {
                MessageBox.Show("Bu IP adresi zaten ağ listesinde kayıtlı.", "Mükerrer IP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ekle, kaydet ve logla
            db.CustomPeers.Add(ipText);
            DataHelper.SaveData(db);
            DataHelper.Log("[AĞ BAĞLANTISI] Yeni harici anne IP adresi eklendi: " + ipText);

            // ListBox'a ekle
            if (lstPeerIPs != null)
            {
                lstPeerIPs.Items.Add(ipText);
            }

            txtPeerIP.Clear();
            UpdateActivePeersIndicator();
            MessageBox.Show("Harici IP adresi başarıyla eklendi! Artık farklı ağda (subnet) olsanız dahi paylaşımlarınız bu IP ile gerçek zamanlı senkronize edilecektir.", "IP Eklendi 🌐", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnShowMyIP_Click(object sender, EventArgs e)
        {
            try
            {
                var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                string myIps = "";
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        myIps += "• IP: " + ip.ToString() + Environment.NewLine;
                    }
                }
                if (string.IsNullOrEmpty(myIps)) myIps = "Bulunamadı (Ağa bağlı olduğunuzdan emin olun).";
                
                MessageBox.Show("Diğer annelerin sizi listelerine ekleyebilmesi için onlara vermeniz gereken IPv4 adresleriniz:" + Environment.NewLine + Environment.NewLine + myIps, "Yerel IP Adresleriniz 🔎", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("IP adresleriniz taranırken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateActivePeersIndicator()
        {
            if (lblActivePeersIndicator != null)
            {
                int count = (db != null && db.CustomPeers != null) ? db.CustomPeers.Count : 0;
                if (count == 0)
                {
                    lblActivePeersIndicator.Text = "🟢 Canlı Ağ Durumu: Sadece yerel ağdaki (Wi-Fi) anneler taranıyor";
                    lblActivePeersIndicator.ForeColor = Color.FromArgb(30, 130, 70);
                }
                else
                {
                    lblActivePeersIndicator.Text = "🔵 Canlı Ağ Durumu: Yerel Wi-Fi + " + count + " Harici Ağ Eşleşmesi Aktif";
                    lblActivePeersIndicator.ForeColor = Color.FromArgb(90, 100, 230);
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            isListening = false;
            if (udpListener != null)
            {
                udpListener.Close();
            }
            base.OnFormClosing(e);
        }

        private void InitializeDailyTip()
        {
            string[] tips = new string[]
            {
                "💡 Uzman Tavsiyesi: Lohusalıkta sıvı tüketimi hayati önem taşır. Günde en az 3 litre su içmek süt salınımını destekler.",
                "💡 Pedagog Tavsiyesi: Bebeğiniz uyuduğunda siz de uyuyun. Dinlenmek, sütünüzün miktarını ve kalitesini doğrudan artırır.",
                "💡 Doktor Tavsiyesi: Lohusalık hüznü geçici ve son derece doğaldır. Kendinize karşı nazik olun, yalnız değilsiniz.",
                "💡 Ebe Tavsiyesi: Günde en az 15 dakikayı sadece kendinize ayırın. Kısa bir ılık duş veya derin nefes egzersizi harikalar yaratır.",
                "💡 Beslenme Uzmanı: Yulaf ezmesi, tahin, dereotu ve rezene çayı anne sütünü destekleyen harika dost besinlerdir.",
                "💡 Uzman Tavsiyesi: Çevrenizden gelen yardım tekliflerini kabul edin. Her şeyi tek başınıza yapmak zorunda değilsiniz.",
                "💡 Psikolog Tavsiyesi: Duygularınızı içinize atmayın. Bu platformda veya bir yakınınızla anonim olarak paylaşmak zihni rahatlatır.",
                "💡 Uzman Tavsiyesi: Bebek bakımında mükemmeliyetçi olmayın. En önemli şey bebeğinizle kurduğunuz o sevgi dolu bağdır."
            };

            Random rng = new Random();
            int index = rng.Next(tips.Length);
            lblDailyTip.Text = tips[index];
        }

        private string CleanInappropriateLanguage(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            // Kapsamlı Türkçe Küfür, Hakaret ve Argo filtre listesi
            string[] badWords = new string[]
            {
                "aptal", "salak", "beceriksiz", "bencil", "embesil", "gerizekalı", "manyak",
                "amcık", "şerefsiz", "piç", "orospu", "göt", "yavşak", "pezevenk", "siktir", 
                "sik", "amına", "götü", "orospunun", "amık", "gavat", "ibne", "meme", "taşşak",
                "yarrak", "sokayım", "sokam", "götveren", "kaltak", "fahişe", "amk", "aq", "sikerim"
            };

            string cleaned = text;
            foreach (var word in badWords)
            {
                // Kelime sınırlarına duyarlı sansürleme (harf duyarsız)
                string pattern = @"\b" + System.Text.RegularExpressions.Regex.Escape(word) + @"\b";
                cleaned = System.Text.RegularExpressions.Regex.Replace(
                    cleaned,
                    pattern,
                    new string('*', word.Length),
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase
                );

                // Türkçe karakter setlerinde substring kontrolü
                if (cleaned.ToLower().Contains(word))
                {
                    cleaned = System.Text.RegularExpressions.Regex.Replace(
                        cleaned,
                        System.Text.RegularExpressions.Regex.Escape(word),
                        new string('*', word.Length),
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase
                    );
                }
            }
            return cleaned;
        }

        // --- Gelişmiş Owner Draw (Özel Çizim) ListBox Çizimleri ---
        private void lstFeed_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            Post post = (Post)lstFeed.Items[e.Index];
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Kart geometrisi (Kenarları yuvarlatılmış hissi veren 4 piksel içeriden çizim)
            Rectangle cardBounds = new Rectangle(e.Bounds.X + 6, e.Bounds.Y + 4, e.Bounds.Width - 12, e.Bounds.Height - 8);

            // Seçim durumuna göre arka plan rengi (SaaS Indigo/Lavanta)
            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            Color cardColor = isSelected ? Color.FromArgb(235, 240, 255) : Color.White;
            Color borderColor = isSelected ? Color.FromArgb(170, 180, 240) : Color.FromArgb(230, 232, 240);

            // Kart Arka Planını Çiz
            using (SolidBrush cardBrush = new SolidBrush(cardColor))
            {
                g.FillRectangle(cardBrush, cardBounds);
            }

            // Kart Çerçevesini Çiz
            using (Pen borderPen = new Pen(borderColor, 1.2f))
            {
                g.DrawRectangle(borderPen, cardBounds);
            }

            // Kategoriye göre renk belirleyelim (Pastel tonlar)
            Color categoryAccentColor = Color.FromArgb(90, 100, 230);
            Color categoryTextCol = Color.FromArgb(90, 100, 230);
            if (post.Category == "Psikolojik Destek")
            {
                categoryAccentColor = Color.FromArgb(238, 230, 255); // Pastel Lavanta
                categoryTextCol = Color.FromArgb(110, 60, 200);
            }
            else if (post.Category == "Bebek Bakımı")
            {
                categoryAccentColor = Color.FromArgb(220, 240, 255); // Pastel Mavi
                categoryTextCol = Color.FromArgb(40, 110, 190);
            }
            else if (post.Category == "Beslenme & Sağlık")
            {
                categoryAccentColor = Color.FromArgb(225, 250, 235); // Pastel Yeşil
                categoryTextCol = Color.FromArgb(30, 130, 70);
            }

            // Sol taraftaki şık kategori şeridi
            Rectangle accentBar = new Rectangle(cardBounds.X, cardBounds.Y, 5, cardBounds.Height);
            using (SolidBrush accentBrush = new SolidBrush(categoryTextCol))
            {
                g.FillRectangle(accentBrush, accentBar);
            }

            // Kategori Kapsülü Çizimi (Sağ alt tarafta - Çakışmaları tamamen önlemek için)
            Font tagFont = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            int tagWidth = 110;
            int tagHeight = 18;
            int tagX = cardBounds.Right - tagWidth - 8;
            int tagY = cardBounds.Bottom - tagHeight - 8;

            using (SolidBrush tagBgBrush = new SolidBrush(categoryAccentColor))
            {
                g.FillRectangle(tagBgBrush, tagX, tagY, tagWidth, tagHeight);
            }

            using (SolidBrush tagTextBrush = new SolidBrush(categoryTextCol))
            {
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(post.Category, tagFont, tagTextBrush, new RectangleF(tagX, tagY, tagWidth, tagHeight), sf);
            }

            // Üst Bilgi Satırı: Rumuz, Tarih ve Beğeni (Kare hatasız temiz çizim)
            string badge = GetUserBadge(post.Nickname);
            string metaText = "Anne: " + post.Nickname + badge + "  |  Tarih: " + post.CreatedAt.ToString("dd.MM.yyyy HH:mm") + "  |  Destek: " + post.LikeCount;
            Font metaFont = new Font("Segoe UI", 8F, FontStyle.Bold);
            using (SolidBrush metaBrush = new SolidBrush(Color.FromArgb(120, 120, 120)))
            {
                g.DrawString(metaText, metaFont, metaBrush, cardBounds.X + 12, cardBounds.Y + 8);
            }

            // Gönderi Önizleme Metni
            Font contentFont = new Font("Segoe UI", 9F, FontStyle.Regular);
            string contentPreview = post.Content;
            if (contentPreview.Length > 52)
            {
                contentPreview = contentPreview.Substring(0, 49) + "...";
            }
            using (SolidBrush contentBrush = new SolidBrush(Color.FromArgb(60, 60, 60)))
            {
                g.DrawString(contentPreview, contentFont, contentBrush, cardBounds.X + 12, cardBounds.Y + 28);
            }
        }

        private void lstComments_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            Comment comment = (Comment)lstComments.Items[e.Index];

            // Seçim durumu renkleri (SaaS Indigo & Lavanta tonları)
            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            Color backColor = isSelected ? Color.FromArgb(235, 240, 255) : Color.FromArgb(253, 253, 253);

            // Arka planı çiz
            using (SolidBrush backBrush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(backBrush, e.Bounds);
            }

            // Bölücü çizgi
            using (Pen dividerPen = new Pen(Color.FromArgb(235, 238, 245)))
            {
                e.Graphics.DrawLine(dividerPen, e.Bounds.X, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);
            }

            // Destekçi Anne ve Zamanı
            string badge = GetUserBadge(comment.Nickname);
            string metaText = "Destekçi Anne: " + comment.Nickname + badge + "  |  Tarih: " + comment.CreatedAt.ToString("dd.MM.yyyy HH:mm");
            Font metaFont = new Font("Segoe UI", 8F, FontStyle.Bold);
            using (SolidBrush metaBrush = new SolidBrush(Color.FromArgb(90, 100, 230)))
            {
                e.Graphics.DrawString(metaText, metaFont, metaBrush, e.Bounds.X + 10, e.Bounds.Y + 6);
            }

            // Yorum Metni
            Font contentFont = new Font("Segoe UI", 9F, FontStyle.Regular);
            using (SolidBrush contentBrush = new SolidBrush(Color.FromArgb(50, 50, 50)))
            {
                e.Graphics.DrawString(comment.Content, contentFont, contentBrush, e.Bounds.X + 10, e.Bounds.Y + 22);
            }
        }

        private string GetUserBadge(string nickname)
        {
            if (db == null) return "";
            
            // Kullanıcının toplam gönderi + yorum sayısını sayarak dinamik unvan belirleyelim
            int activityCount = 0;
            foreach (var post in db.Posts)
            {
                if (post.Nickname == nickname) activityCount++;
                foreach (var comm in post.Comments)
                {
                    if (comm.Nickname == nickname) activityCount++;
                }
            }

            if (activityCount >= 3) return " [Deneyimli Anne]";
            if (activityCount >= 1) return " [Yeni Anne]";
            return " [Destekçi]";
        }

        // ========================================================
        // GEÇİŞ SEKME SİSTEMİ & PREMİUM ÖZELLİKLERİN KURULUMU
        // ========================================================
        private void InitializePremiumFeatures()
        {
            // --- SEKME NAVİGASYON BARI (SUB-HEADER) ---
            pnlSubHeader = new Panel();
            pnlSubHeader.Dock = DockStyle.Top;
            pnlSubHeader.Height = 45;
            pnlSubHeader.BackColor = Color.FromArgb(238, 240, 245);
            pnlDashboard.Controls.Add(pnlSubHeader);

            btnTabFeed = new Button();
            btnTabFeed.Text = "💬 Sohbet Akışı";
            btnTabFeed.Size = new Size(185, 34);
            btnTabFeed.Location = new Point(15, 5);
            btnTabFeed.FlatStyle = FlatStyle.Flat;
            btnTabFeed.FlatAppearance.BorderSize = 0;
            btnTabFeed.Font = new Font("Segoe UI", 9.25F, FontStyle.Bold);
            btnTabFeed.BackColor = Color.FromArgb(90, 100, 230); // Seçili başlar (Indigo)
            btnTabFeed.ForeColor = Color.White;
            btnTabFeed.Click += new EventHandler(btnTabFeed_Click);
            pnlSubHeader.Controls.Add(btnTabFeed);

            btnTabEpds = new Button();
            btnTabEpds.Text = "📋 Ruh Sağlığı Testi (EPDS)";
            btnTabEpds.Size = new Size(205, 34);
            btnTabEpds.Location = new Point(210, 5);
            btnTabEpds.FlatStyle = FlatStyle.Flat;
            btnTabEpds.FlatAppearance.BorderSize = 0;
            btnTabEpds.Font = new Font("Segoe UI", 9.25F, FontStyle.Bold);
            btnTabEpds.BackColor = Color.FromArgb(228, 231, 239);
            btnTabEpds.ForeColor = Color.FromArgb(80, 90, 110);
            btnTabEpds.Click += new EventHandler(btnTabEpds_Click);
            pnlSubHeader.Controls.Add(btnTabEpds);

            btnTabMood = new Button();
            btnTabMood.Text = "🎭 Günlük Duygu Durumu";
            btnTabMood.Size = new Size(205, 34);
            btnTabMood.Location = new Point(425, 5);
            btnTabMood.FlatStyle = FlatStyle.Flat;
            btnTabMood.FlatAppearance.BorderSize = 0;
            btnTabMood.Font = new Font("Segoe UI", 9.25F, FontStyle.Bold);
            btnTabMood.BackColor = Color.FromArgb(228, 231, 239);
            btnTabMood.ForeColor = Color.FromArgb(80, 90, 110);
            btnTabMood.Click += new EventHandler(btnTabMood_Click);
            pnlSubHeader.Controls.Add(btnTabMood);

            btnTabExpert = new Button();
            btnTabExpert.Text = "🩺 Uzman Köşesi & SSS";
            btnTabExpert.Size = new Size(205, 34);
            btnTabExpert.Location = new Point(640, 5);
            btnTabExpert.FlatStyle = FlatStyle.Flat;
            btnTabExpert.FlatAppearance.BorderSize = 0;
            btnTabExpert.Font = new Font("Segoe UI", 9.25F, FontStyle.Bold);
            btnTabExpert.BackColor = Color.FromArgb(228, 231, 239);
            btnTabExpert.ForeColor = Color.FromArgb(80, 90, 110);
            btnTabExpert.Click += new EventHandler(btnTabExpert_Click);
            pnlSubHeader.Controls.Add(btnTabExpert);

            btnTabStats = new Button();
            btnTabStats.Text = "📊 Canlı Topluluk Raporu";
            btnTabStats.Size = new Size(205, 34);
            btnTabStats.Location = new Point(855, 5);
            btnTabStats.FlatStyle = FlatStyle.Flat;
            btnTabStats.FlatAppearance.BorderSize = 0;
            btnTabStats.Font = new Font("Segoe UI", 9.25F, FontStyle.Bold);
            btnTabStats.BackColor = Color.FromArgb(228, 231, 239);
            btnTabStats.ForeColor = Color.FromArgb(80, 90, 110);
            btnTabStats.Click += new EventHandler(btnTabStats_Click);
            pnlSubHeader.Controls.Add(btnTabStats);

            // --- EPDS TAB CONTENT ---
            pnlEpdsTabContent = new Panel();
            pnlEpdsTabContent.Dock = DockStyle.Fill;
            pnlEpdsTabContent.BackColor = Color.FromArgb(255, 246, 247);
            pnlEpdsTabContent.Visible = false;
            pnlDashboard.Controls.Add(pnlEpdsTabContent);

            pnlEpdsCard = new Panel();
            pnlEpdsCard.Size = new Size(800, 480);
            pnlEpdsCard.Location = new Point(150, 15);
            pnlEpdsCard.BackColor = Color.White;
            pnlEpdsTabContent.Controls.Add(pnlEpdsCard);

            lblEpdsTitle = new Label();
            lblEpdsTitle.Text = "Edinburgh Lohusalık Depresyon Ölçeği (EPDS)";
            lblEpdsTitle.Font = new Font("Segoe UI", 13.5F, FontStyle.Bold);
            lblEpdsTitle.ForeColor = Color.FromArgb(190, 80, 110);
            lblEpdsTitle.Location = new Point(25, 20);
            lblEpdsTitle.Size = new Size(750, 30);
            pnlEpdsCard.Controls.Add(lblEpdsTitle);

            lblEpdsSubtitle = new Label();
            lblEpdsSubtitle.Text = "Lohusalıkta ruh sağlığınızı taramak için lütfen son 7 güne ait hislerinizi en uygun seçenekle belirtiniz. Bu test klinik bir kılavuzdur, teşhis koymaz.";
            lblEpdsSubtitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            lblEpdsSubtitle.ForeColor = Color.DimGray;
            lblEpdsSubtitle.Location = new Point(25, 50);
            lblEpdsSubtitle.Size = new Size(750, 40);
            pnlEpdsCard.Controls.Add(lblEpdsSubtitle);

            // Welcome
            pnlEpdsWelcome = new Panel();
            pnlEpdsWelcome.Size = new Size(750, 350);
            pnlEpdsWelcome.Location = new Point(25, 100);
            pnlEpdsCard.Controls.Add(pnlEpdsWelcome);

            lblEpdsWelcome = new Label();
            lblEpdsWelcome.Text = "Lohusalık dönemi, hormonal dalgalanmaların, fiziksel iyileşmenin ve uykusuzluğun yoğun yaşandığı, yeni bir hayata geçiş sürecidir. Bu süreçte zaman zaman üzgün, kaygılı veya yetersiz hissetmeniz son derece doğaldır.\n\nEdinburgh Postnatal Depresyon Ölçeği (EPDS), annelerin bu hassas dönemdeki duygu durumlarını anlamalarına ve farkındalık kazanmalarına yardımcı olan bilimsel olarak kanıtlanmış bir tarama testidir.\n\nToplam 10 sorudan oluşan bu kısa testi tamamen anonim olarak yanıtlayarak ruh halinizin durumunu görebilirsiniz. Hiçbir cevabınız kaydedilmez.";
            lblEpdsWelcome.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            lblEpdsWelcome.ForeColor = Color.FromArgb(80, 80, 80);
            lblEpdsWelcome.Location = new Point(15, 15);
            lblEpdsWelcome.Size = new Size(720, 200);
            pnlEpdsWelcome.Controls.Add(lblEpdsWelcome);

            btnEpdsStart = new Button();
            btnEpdsStart.Text = "Değerlendirmeyi Başlat ➔";
            btnEpdsStart.Size = new Size(240, 42);
            btnEpdsStart.Location = new Point(255, 230);
            btnEpdsStart.FlatStyle = FlatStyle.Flat;
            btnEpdsStart.FlatAppearance.BorderSize = 0;
            btnEpdsStart.BackColor = Color.FromArgb(220, 110, 130);
            btnEpdsStart.ForeColor = Color.White;
            btnEpdsStart.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnEpdsStart.Click += new EventHandler(btnEpdsStart_Click);
            pnlEpdsWelcome.Controls.Add(btnEpdsStart);

            // Quiz Setup
            pnlEpdsQuiz = new Panel();
            pnlEpdsQuiz.Size = new Size(750, 350);
            pnlEpdsQuiz.Location = new Point(25, 100);
            pnlEpdsQuiz.Visible = false;
            pnlEpdsCard.Controls.Add(pnlEpdsQuiz);

            lblEpdsQNo = new Label();
            lblEpdsQNo.Text = "Soru 1 / 10";
            lblEpdsQNo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblEpdsQNo.ForeColor = Color.FromArgb(190, 80, 110);
            lblEpdsQNo.Location = new Point(15, 10);
            lblEpdsQNo.Size = new Size(720, 20);
            pnlEpdsQuiz.Controls.Add(lblEpdsQNo);

            lblEpdsQuestion = new Label();
            lblEpdsQuestion.Text = "Soru metni yükleniyor...";
            lblEpdsQuestion.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblEpdsQuestion.ForeColor = Color.FromArgb(50, 50, 50);
            lblEpdsQuestion.Location = new Point(15, 35);
            lblEpdsQuestion.Size = new Size(720, 50);
            pnlEpdsQuiz.Controls.Add(lblEpdsQuestion);

            rbEpdsOpt1 = new RadioButton();
            rbEpdsOpt1.Font = new Font("Segoe UI", 9.5F);
            rbEpdsOpt1.Location = new Point(35, 95);
            rbEpdsOpt1.Size = new Size(680, 28);
            rbEpdsOpt1.ForeColor = Color.FromArgb(70, 70, 70);
            pnlEpdsQuiz.Controls.Add(rbEpdsOpt1);

            rbEpdsOpt2 = new RadioButton();
            rbEpdsOpt2.Font = new Font("Segoe UI", 9.5F);
            rbEpdsOpt2.Location = new Point(35, 135);
            rbEpdsOpt2.Size = new Size(680, 28);
            rbEpdsOpt2.ForeColor = Color.FromArgb(70, 70, 70);
            pnlEpdsQuiz.Controls.Add(rbEpdsOpt2);

            rbEpdsOpt3 = new RadioButton();
            rbEpdsOpt3.Font = new Font("Segoe UI", 9.5F);
            rbEpdsOpt3.Location = new Point(35, 175);
            rbEpdsOpt3.Size = new Size(680, 28);
            rbEpdsOpt3.ForeColor = Color.FromArgb(70, 70, 70);
            pnlEpdsQuiz.Controls.Add(rbEpdsOpt3);

            rbEpdsOpt4 = new RadioButton();
            rbEpdsOpt4.Font = new Font("Segoe UI", 9.5F);
            rbEpdsOpt4.Location = new Point(35, 215);
            rbEpdsOpt4.Size = new Size(680, 28);
            rbEpdsOpt4.ForeColor = Color.FromArgb(70, 70, 70);
            pnlEpdsQuiz.Controls.Add(rbEpdsOpt4);

            btnEpdsNext = new Button();
            btnEpdsNext.Text = "Sonraki Soru ➔";
            btnEpdsNext.Size = new Size(160, 38);
            btnEpdsNext.Location = new Point(555, 270);
            btnEpdsNext.FlatStyle = FlatStyle.Flat;
            btnEpdsNext.FlatAppearance.BorderSize = 0;
            btnEpdsNext.BackColor = Color.FromArgb(90, 100, 230);
            btnEpdsNext.ForeColor = Color.White;
            btnEpdsNext.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnEpdsNext.Click += new EventHandler(btnEpdsNext_Click);
            pnlEpdsQuiz.Controls.Add(btnEpdsNext);

            // Result
            pnlEpdsResult = new Panel();
            pnlEpdsResult.Size = new Size(750, 350);
            pnlEpdsResult.Location = new Point(25, 100);
            pnlEpdsResult.Visible = false;
            pnlEpdsCard.Controls.Add(pnlEpdsResult);

            lblEpdsResultScore = new Label();
            lblEpdsResultScore.Text = "Toplam Puan: 0 / 30";
            lblEpdsResultScore.Font = new Font("Segoe UI", 14.5F, FontStyle.Bold);
            lblEpdsResultScore.ForeColor = Color.FromArgb(90, 100, 230);
            lblEpdsResultScore.Location = new Point(15, 10);
            lblEpdsResultScore.Size = new Size(720, 30);
            pnlEpdsResult.Controls.Add(lblEpdsResultScore);

            lblEpdsResultTitle = new Label();
            lblEpdsResultTitle.Text = "Durum Analizi: ...";
            lblEpdsResultTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblEpdsResultTitle.ForeColor = Color.FromArgb(50, 50, 50);
            lblEpdsResultTitle.Location = new Point(15, 45);
            lblEpdsResultTitle.Size = new Size(720, 25);
            pnlEpdsResult.Controls.Add(lblEpdsResultTitle);

            lblEpdsResultDesc = new Label();
            lblEpdsResultDesc.Text = "Detaylı açıklama...";
            lblEpdsResultDesc.Font = new Font("Segoe UI", 9.25F);
            lblEpdsResultDesc.ForeColor = Color.FromArgb(80, 80, 80);
            lblEpdsResultDesc.Location = new Point(15, 80);
            lblEpdsResultDesc.Size = new Size(720, 160);
            pnlEpdsResult.Controls.Add(lblEpdsResultDesc);

            btnEpdsRestart = new Button();
            btnEpdsRestart.Text = "Değerlendirmeyi Yeniden Çöz ↺";
            btnEpdsRestart.Size = new Size(240, 38);
            btnEpdsRestart.Location = new Point(255, 270);
            btnEpdsRestart.FlatStyle = FlatStyle.Flat;
            btnEpdsRestart.FlatAppearance.BorderSize = 1;
            btnEpdsRestart.FlatAppearance.BorderColor = Color.FromArgb(90, 100, 230);
            btnEpdsRestart.ForeColor = Color.FromArgb(90, 100, 230);
            btnEpdsRestart.BackColor = Color.White;
            btnEpdsRestart.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnEpdsRestart.Click += new EventHandler(btnEpdsRestart_Click);
            pnlEpdsResult.Controls.Add(btnEpdsRestart);

            // --- STATISTICS & REHBER TAB CONTENT ---
            pnlStatsTabContent = new Panel();
            pnlStatsTabContent.Dock = DockStyle.Fill;
            pnlStatsTabContent.BackColor = Color.FromArgb(245, 247, 250);
            pnlStatsTabContent.Visible = false;
            pnlDashboard.Controls.Add(pnlStatsTabContent);

            // Left Panel (Stats Card)
            pnlStatsLeft = new Panel();
            pnlStatsLeft.Dock = DockStyle.Left;
            pnlStatsLeft.Width = 530;
            pnlStatsLeft.Padding = new Padding(15, 15, 10, 15);
            pnlStatsTabContent.Controls.Add(pnlStatsLeft);

            pnlStatsCard = new Panel();
            pnlStatsCard.Dock = DockStyle.Fill;
            pnlStatsCard.BackColor = Color.White;
            pnlStatsLeft.Controls.Add(pnlStatsCard);

            lblStatsTitle = new Label();
            lblStatsTitle.Text = "Topluluk Nabzı ve Canlı Etkileşim İstatistikleri";
            lblStatsTitle.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            lblStatsTitle.ForeColor = Color.FromArgb(31, 38, 57);
            lblStatsTitle.Location = new Point(20, 15);
            lblStatsTitle.Size = new Size(460, 25);
            pnlStatsCard.Controls.Add(lblStatsTitle);

            // Stat Cards inside Left Panel
            lblStatsTotalPosts = new Label();
            lblStatsTotalPosts.BackColor = Color.FromArgb(235, 240, 255);
            lblStatsTotalPosts.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblStatsTotalPosts.ForeColor = Color.FromArgb(90, 100, 230);
            lblStatsTotalPosts.TextAlign = ContentAlignment.MiddleCenter;
            lblStatsTotalPosts.Location = new Point(20, 50);
            lblStatsTotalPosts.Size = new Size(140, 65);
            lblStatsTotalPosts.Text = "0\nPaylaşılan Soru";
            pnlStatsCard.Controls.Add(lblStatsTotalPosts);

            lblStatsTotalComments = new Label();
            lblStatsTotalComments.BackColor = Color.FromArgb(243, 238, 255);
            lblStatsTotalComments.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblStatsTotalComments.ForeColor = Color.FromArgb(120, 80, 220);
            lblStatsTotalComments.TextAlign = ContentAlignment.MiddleCenter;
            lblStatsTotalComments.Location = new Point(175, 50);
            lblStatsTotalComments.Size = new Size(140, 65);
            lblStatsTotalComments.Text = "0\nDestek Yorumu";
            pnlStatsCard.Controls.Add(lblStatsTotalComments);

            lblStatsTotalLikes = new Label();
            lblStatsTotalLikes.BackColor = Color.FromArgb(225, 245, 235);
            lblStatsTotalLikes.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblStatsTotalLikes.ForeColor = Color.FromArgb(30, 130, 70);
            lblStatsTotalLikes.TextAlign = ContentAlignment.MiddleCenter;
            lblStatsTotalLikes.Location = new Point(330, 50);
            lblStatsTotalLikes.Size = new Size(140, 65);
            lblStatsTotalLikes.Text = "0\nEmpati / Beğeni";
            pnlStatsCard.Controls.Add(lblStatsTotalLikes);

            lblStatsBreakdownTitle = new Label();
            lblStatsBreakdownTitle.Text = "Destek Taleplerinin Kategori Dağılımı";
            lblStatsBreakdownTitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblStatsBreakdownTitle.ForeColor = Color.FromArgb(80, 90, 110);
            lblStatsBreakdownTitle.Location = new Point(20, 135);
            lblStatsBreakdownTitle.Size = new Size(460, 20);
            pnlStatsCard.Controls.Add(lblStatsBreakdownTitle);

            // Category progress bars
            lblBreakdownPsikolojik = new Label();
            lblBreakdownPsikolojik.Text = "Psikolojik Destek (%0)";
            lblBreakdownPsikolojik.Font = new Font("Segoe UI", 8.5F);
            lblBreakdownPsikolojik.ForeColor = Color.DimGray;
            lblBreakdownPsikolojik.Location = new Point(20, 165);
            lblBreakdownPsikolojik.Size = new Size(460, 18);
            pnlStatsCard.Controls.Add(lblBreakdownPsikolojik);

            pbarPsikolojik = new Panel();
            pbarPsikolojik.BackColor = Color.FromArgb(240, 240, 240);
            pbarPsikolojik.Location = new Point(20, 185);
            pbarPsikolojik.Size = new Size(450, 10);
            pnlStatsCard.Controls.Add(pbarPsikolojik);

            pbarPsikolojikFill = new Panel();
            pbarPsikolojikFill.BackColor = Color.FromArgb(120, 80, 220);
            pbarPsikolojikFill.Location = new Point(0, 0);
            pbarPsikolojikFill.Size = new Size(0, 10);
            pbarPsikolojik.Controls.Add(pbarPsikolojikFill);

            lblBreakdownBebek = new Label();
            lblBreakdownBebek.Text = "Bebek Bakımı (%0)";
            lblBreakdownBebek.Font = new Font("Segoe UI", 8.5F);
            lblBreakdownBebek.ForeColor = Color.DimGray;
            lblBreakdownBebek.Location = new Point(20, 210);
            lblBreakdownBebek.Size = new Size(460, 18);
            pnlStatsCard.Controls.Add(lblBreakdownBebek);

            pbarBebek = new Panel();
            pbarBebek.BackColor = Color.FromArgb(240, 240, 240);
            pbarBebek.Location = new Point(20, 230);
            pbarBebek.Size = new Size(450, 10);
            pnlStatsCard.Controls.Add(pbarBebek);

            pbarBebekFill = new Panel();
            pbarBebekFill.BackColor = Color.FromArgb(90, 100, 230);
            pbarBebekFill.Location = new Point(0, 0);
            pbarBebekFill.Size = new Size(0, 10);
            pbarBebek.Controls.Add(pbarBebekFill);

            lblBreakdownBeslenme = new Label();
            lblBreakdownBeslenme.Text = "Beslenme & Sağlık (%0)";
            lblBreakdownBeslenme.Font = new Font("Segoe UI", 8.5F);
            lblBreakdownBeslenme.ForeColor = Color.DimGray;
            lblBreakdownBeslenme.Location = new Point(20, 255);
            lblBreakdownBeslenme.Size = new Size(460, 18);
            pnlStatsCard.Controls.Add(lblBreakdownBeslenme);

            pbarBeslenme = new Panel();
            pbarBeslenme.BackColor = Color.FromArgb(240, 240, 240);
            pbarBeslenme.Location = new Point(20, 275);
            pbarBeslenme.Size = new Size(450, 10);
            pnlStatsCard.Controls.Add(pbarBeslenme);

            pbarBeslenmeFill = new Panel();
            pbarBeslenmeFill.BackColor = Color.FromArgb(30, 130, 70);
            pbarBeslenmeFill.Location = new Point(0, 0);
            pbarBeslenmeFill.Size = new Size(0, 10);
            pbarBeslenme.Controls.Add(pbarBeslenmeFill);

            // Right Panel (Directory Card)
            pnlStatsRight = new Panel();
            pnlStatsRight.Dock = DockStyle.Fill;
            pnlStatsRight.Padding = new Padding(5, 15, 15, 15);
            pnlStatsTabContent.Controls.Add(pnlStatsRight);

            pnlSupportDirectoryCard = new Panel();
            pnlSupportDirectoryCard.Dock = DockStyle.Fill;
            pnlSupportDirectoryCard.BackColor = Color.White;
            pnlStatsRight.Controls.Add(pnlSupportDirectoryCard);

            lblDirectoryTitle = new Label();
            lblDirectoryTitle.Text = "Sağlık Bakanlığı & Resmi Destek Hatları";
            lblDirectoryTitle.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            lblDirectoryTitle.ForeColor = Color.FromArgb(31, 38, 57);
            lblDirectoryTitle.Location = new Point(20, 15);
            lblDirectoryTitle.Size = new Size(460, 25);
            pnlSupportDirectoryCard.Controls.Add(lblDirectoryTitle);

            lblDirectorySubtitle = new Label();
            lblDirectorySubtitle.Text = "Kendinizi yalnız, aşırı yorgun veya lohusa sendromunda hissettiğinizde bu ücretsiz kurumsal hatlardan her an destek alabilirsiniz:";
            lblDirectorySubtitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            lblDirectorySubtitle.ForeColor = Color.DimGray;
            lblDirectorySubtitle.Location = new Point(20, 45);
            lblDirectorySubtitle.Size = new Size(450, 45);
            pnlSupportDirectoryCard.Controls.Add(lblDirectorySubtitle);

            btnHotline191 = new Button();
            btnHotline191.Text = "📞 T.C. Sağlık Bakanlığı ALO 191 Ruh Sağlığı Destek";
            btnHotline191.Size = new Size(450, 40);
            btnHotline191.Location = new Point(20, 100);
            btnHotline191.FlatStyle = FlatStyle.Flat;
            btnHotline191.FlatAppearance.BorderSize = 0;
            btnHotline191.BackColor = Color.FromArgb(235, 240, 255);
            btnHotline191.ForeColor = Color.FromArgb(90, 100, 230);
            btnHotline191.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnHotline191.TextAlign = ContentAlignment.MiddleLeft;
            btnHotline191.Padding = new Padding(10, 0, 0, 0);
            btnHotline191.Click += new EventHandler(btnHotline191_Click);
            pnlSupportDirectoryCard.Controls.Add(btnHotline191);

            btnHotline182 = new Button();
            btnHotline182.Text = "📞 ALO 182 & MHRS Psikolojik Destek Birimleri";
            btnHotline182.Size = new Size(450, 40);
            btnHotline182.Location = new Point(20, 150);
            btnHotline182.FlatStyle = FlatStyle.Flat;
            btnHotline182.FlatAppearance.BorderSize = 0;
            btnHotline182.BackColor = Color.FromArgb(243, 238, 255);
            btnHotline182.ForeColor = Color.FromArgb(120, 80, 220);
            btnHotline182.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnHotline182.TextAlign = ContentAlignment.MiddleLeft;
            btnHotline182.Padding = new Padding(10, 0, 0, 0);
            btnHotline182.Click += new EventHandler(btnHotline182_Click);
            pnlSupportDirectoryCard.Controls.Add(btnHotline182);

            btnHotlineKades = new Button();
            btnHotlineKades.Text = "🌸 KADES Mobil Kadın Destek Resmi Bilgilendirme";
            btnHotlineKades.Size = new Size(450, 40);
            btnHotlineKades.Location = new Point(20, 200);
            btnHotlineKades.FlatStyle = FlatStyle.Flat;
            btnHotlineKades.FlatAppearance.BorderSize = 0;
            btnHotlineKades.BackColor = Color.FromArgb(225, 245, 235);
            btnHotlineKades.ForeColor = Color.FromArgb(30, 130, 70);
            btnHotlineKades.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnHotlineKades.TextAlign = ContentAlignment.MiddleLeft;
            btnHotlineKades.Padding = new Padding(10, 0, 0, 0);
            btnHotlineKades.Click += new EventHandler(btnHotlineKades_Click);
            pnlSupportDirectoryCard.Controls.Add(btnHotlineKades);

            btnShowContract = new Button();
            btnShowContract.Text = "📜 Güvenli Topluluk & Empati Sözleşmesi";
            btnShowContract.Size = new Size(450, 40);
            btnShowContract.Location = new Point(20, 250);
            btnShowContract.FlatStyle = FlatStyle.Flat;
            btnShowContract.FlatAppearance.BorderSize = 1;
            btnShowContract.FlatAppearance.BorderColor = Color.FromArgb(90, 100, 230);
            btnShowContract.ForeColor = Color.FromArgb(90, 100, 230);
            btnShowContract.BackColor = Color.White;
            btnShowContract.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnShowContract.TextAlign = ContentAlignment.MiddleLeft;
            btnShowContract.Padding = new Padding(10, 0, 0, 0);
            btnShowContract.Click += new EventHandler(btnShowContract_Click);
            pnlSupportDirectoryCard.Controls.Add(btnShowContract);

            // --- MOOD TAB CONTENT ---
            pnlMoodTabContent = new Panel();
            pnlMoodTabContent.Dock = DockStyle.Fill;
            pnlMoodTabContent.BackColor = Color.FromArgb(255, 246, 247);
            pnlMoodTabContent.Visible = false;
            pnlDashboard.Controls.Add(pnlMoodTabContent);

            pnlMoodCard = new Panel();
            pnlMoodCard.Size = new Size(800, 480);
            pnlMoodCard.Location = new Point(150, 15);
            pnlMoodCard.BackColor = Color.White;
            pnlMoodTabContent.Controls.Add(pnlMoodCard);

            lblMoodTitle = new Label();
            lblMoodTitle.Text = "Günlük Duygu Durumu Günlüğü ve Takipçisi 🎭";
            lblMoodTitle.Font = new Font("Segoe UI", 13.5F, FontStyle.Bold);
            lblMoodTitle.ForeColor = Color.FromArgb(190, 80, 110);
            lblMoodTitle.Location = new Point(25, 20);
            lblMoodTitle.Size = new Size(750, 30);
            pnlMoodCard.Controls.Add(lblMoodTitle);

            lblMoodSubtitle = new Label();
            lblMoodSubtitle.Text = "Lohusalık döneminde duygu değişimleriniz son derece doğaldır. Her gün ruh halinizi seçerek kendiniz için bir farkındalık günlüğü oluşturun.";
            lblMoodSubtitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            lblMoodSubtitle.ForeColor = Color.DimGray;
            lblMoodSubtitle.Location = new Point(25, 50);
            lblMoodSubtitle.Size = new Size(750, 25);
            pnlMoodCard.Controls.Add(lblMoodSubtitle);

            lblMoodPrompt = new Label();
            lblMoodPrompt.Text = "Bugün kendinizi nasıl hissediyorsunuz? Lütfen aşağıdaki ruh hallerinden birine tıklayın:";
            lblMoodPrompt.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblMoodPrompt.ForeColor = Color.FromArgb(80, 80, 80);
            lblMoodPrompt.Location = new Point(25, 90);
            lblMoodPrompt.Size = new Size(750, 20);
            pnlMoodCard.Controls.Add(lblMoodPrompt);

            // Emoji buttons (😊, 😐, 😴, 😟, 😢)
            btnMoodEmoji1 = new Button();
            btnMoodEmoji1.Text = "😊\nMutlu / Huzurlu";
            btnMoodEmoji1.Size = new Size(130, 80);
            btnMoodEmoji1.Location = new Point(25, 120);
            btnMoodEmoji1.FlatStyle = FlatStyle.Flat;
            btnMoodEmoji1.FlatAppearance.BorderSize = 0;
            btnMoodEmoji1.BackColor = Color.FromArgb(240, 250, 240);
            btnMoodEmoji1.ForeColor = Color.FromArgb(40, 100, 40);
            btnMoodEmoji1.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnMoodEmoji1.Click += (s, e) => RegisterMood("😊", "Mutlu ve Huzurlu");
            pnlMoodCard.Controls.Add(btnMoodEmoji1);

            btnMoodEmoji2 = new Button();
            btnMoodEmoji2.Text = "😐\nNötr / Sakin";
            btnMoodEmoji2.Size = new Size(130, 80);
            btnMoodEmoji2.Location = new Point(175, 120);
            btnMoodEmoji2.FlatStyle = FlatStyle.Flat;
            btnMoodEmoji2.FlatAppearance.BorderSize = 0;
            btnMoodEmoji2.BackColor = Color.FromArgb(245, 245, 245);
            btnMoodEmoji2.ForeColor = Color.FromArgb(80, 80, 80);
            btnMoodEmoji2.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnMoodEmoji2.Click += (s, e) => RegisterMood("😐", "Nötr / Sakin");
            pnlMoodCard.Controls.Add(btnMoodEmoji2);

            btnMoodEmoji3 = new Button();
            btnMoodEmoji3.Text = "😴\nAşırı Yorgun";
            btnMoodEmoji3.Size = new Size(130, 80);
            btnMoodEmoji3.Location = new Point(325, 120);
            btnMoodEmoji3.FlatStyle = FlatStyle.Flat;
            btnMoodEmoji3.FlatAppearance.BorderSize = 0;
            btnMoodEmoji3.BackColor = Color.FromArgb(254, 248, 230);
            btnMoodEmoji3.ForeColor = Color.FromArgb(160, 100, 10);
            btnMoodEmoji3.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnMoodEmoji3.Click += (s, e) => RegisterMood("😴", "Aşırı Yorgun");
            pnlMoodCard.Controls.Add(btnMoodEmoji3);

            btnMoodEmoji4 = new Button();
            btnMoodEmoji4.Text = "😟\nKaygılı / Endişeli";
            btnMoodEmoji4.Size = new Size(130, 80);
            btnMoodEmoji4.Location = new Point(475, 120);
            btnMoodEmoji4.FlatStyle = FlatStyle.Flat;
            btnMoodEmoji4.FlatAppearance.BorderSize = 0;
            btnMoodEmoji4.BackColor = Color.FromArgb(254, 240, 242);
            btnMoodEmoji4.ForeColor = Color.FromArgb(190, 80, 110);
            btnMoodEmoji4.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnMoodEmoji4.Click += (s, e) => RegisterMood("😟", "Kaygılı / Endişeli");
            pnlMoodCard.Controls.Add(btnMoodEmoji4);

            btnMoodEmoji5 = new Button();
            btnMoodEmoji5.Text = "😢\nÜzgün / Yalnız";
            btnMoodEmoji5.Size = new Size(130, 80);
            btnMoodEmoji5.Location = new Point(625, 120);
            btnMoodEmoji5.FlatStyle = FlatStyle.Flat;
            btnMoodEmoji5.FlatAppearance.BorderSize = 0;
            btnMoodEmoji5.BackColor = Color.FromArgb(240, 245, 255);
            btnMoodEmoji5.ForeColor = Color.FromArgb(30, 70, 160);
            btnMoodEmoji5.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnMoodEmoji5.Click += (s, e) => RegisterMood("😢", "Üzgün / Yalnız");
            pnlMoodCard.Controls.Add(btnMoodEmoji5);

            lblMoodHistoryHeader = new Label();
            lblMoodHistoryHeader.Text = "Duygu Durum Günlüğü Geçmişiniz (XML Veritabanı):";
            lblMoodHistoryHeader.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblMoodHistoryHeader.ForeColor = Color.FromArgb(100, 100, 100);
            lblMoodHistoryHeader.Location = new Point(25, 220);
            lblMoodHistoryHeader.Size = new Size(750, 20);
            pnlMoodCard.Controls.Add(lblMoodHistoryHeader);

            lstMoodHistory = new ListBox();
            lstMoodHistory.Location = new Point(25, 250);
            lstMoodHistory.Size = new Size(730, 200);
            lstMoodHistory.Font = new Font("Segoe UI", 9.5F);
            lstMoodHistory.DrawMode = DrawMode.OwnerDrawVariable;
            lstMoodHistory.MeasureItem += new MeasureItemEventHandler(lstMoodHistory_MeasureItem);
            lstMoodHistory.DrawItem += new DrawItemEventHandler(lstMoodHistory_DrawItem);
            pnlMoodCard.Controls.Add(lstMoodHistory);

            // --- EXPERT FAQ TAB CONTENT ---
            pnlExpertTabContent = new Panel();
            pnlExpertTabContent.Dock = DockStyle.Fill;
            pnlExpertTabContent.BackColor = Color.FromArgb(255, 246, 247);
            pnlExpertTabContent.Visible = false;
            pnlDashboard.Controls.Add(pnlExpertTabContent);

            pnlExpertCard = new Panel();
            pnlExpertCard.Size = new Size(800, 480);
            pnlExpertCard.Location = new Point(150, 15);
            pnlExpertCard.BackColor = Color.White;
            pnlExpertTabContent.Controls.Add(pnlExpertCard);

            lblExpertTitle = new Label();
            lblExpertTitle.Text = "🩺 Uzman Köşesi ve İnteraktif SSS Kılavuzu";
            lblExpertTitle.Font = new Font("Segoe UI", 13.5F, FontStyle.Bold);
            lblExpertTitle.ForeColor = Color.FromArgb(190, 80, 110);
            lblExpertTitle.Location = new Point(25, 20);
            lblExpertTitle.Size = new Size(750, 30);
            pnlExpertCard.Controls.Add(lblExpertTitle);

            lblExpertSubtitle = new Label();
            lblExpertSubtitle.Text = "Çocuk doktorlarımız ve psikologlarımız tarafından hazırlanan kılavuz konulara tıklayarak yanıtları görebilirsiniz:";
            lblExpertSubtitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            lblExpertSubtitle.ForeColor = Color.DimGray;
            lblExpertSubtitle.Location = new Point(25, 50);
            lblExpertSubtitle.Size = new Size(750, 25);
            pnlExpertCard.Controls.Add(lblExpertSubtitle);

            // FAQ 1 Accordion
            pnlFaq1 = new Panel();
            pnlFaq1.Size = new Size(740, 42);
            pnlFaq1.Location = new Point(25, 90);
            pnlFaq1.BackColor = Color.FromArgb(255, 240, 242);
            pnlFaq1.Cursor = Cursors.Hand;
            pnlFaq1.Click += (s, e) => ToggleFaq(1);
            pnlExpertCard.Controls.Add(pnlFaq1);

            lblFaqQ1 = new Label();
            lblFaqQ1.Text = "❓ Lohusalık Hüznü ile Klinik Depresyon Arasındaki Fark Nedir?";
            lblFaqQ1.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblFaqQ1.ForeColor = Color.FromArgb(190, 80, 110);
            lblFaqQ1.Location = new Point(10, 10);
            lblFaqQ1.Size = new Size(720, 20);
            lblFaqQ1.Click += (s, e) => ToggleFaq(1);
            pnlFaq1.Controls.Add(lblFaqQ1);

            lblFaqA1 = new Label();
            lblFaqA1.Text = "Lohusalık hüznü (Baby Blues), doğumdan sonraki ilk 10 günde hormonal düşüşten ötürü ağlama hissi ve hafif yorgunlukla seyreder ve kendiliğinden geçer. Klinik depresyon ise 2 haftadan uzun sürer, hayattan zevk alamama ve suçluluk hissiyle devam eder. Bu durumda profesyonel destek alınması önerilir.";
            lblFaqA1.Font = new Font("Segoe UI", 8.75F);
            lblFaqA1.ForeColor = Color.FromArgb(70, 70, 70);
            lblFaqA1.Location = new Point(10, 32);
            lblFaqA1.Size = new Size(720, 40);
            lblFaqA1.Click += (s, e) => ToggleFaq(1);
            pnlFaq1.Controls.Add(lblFaqA1);

            // FAQ 2 Accordion
            pnlFaq2 = new Panel();
            pnlFaq2.Size = new Size(740, 42);
            pnlFaq2.Location = new Point(25, 175);
            pnlFaq2.BackColor = Color.FromArgb(243, 230, 255);
            pnlFaq2.Cursor = Cursors.Hand;
            pnlFaq2.Click += (s, e) => ToggleFaq(2);
            pnlExpertCard.Controls.Add(pnlFaq2);

            lblFaqQ2 = new Label();
            lblFaqQ2.Text = "❓ Bebeğimin Güvenli Uyku Düzeni Nasıl Olmalıdır?";
            lblFaqQ2.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblFaqQ2.ForeColor = Color.FromArgb(120, 30, 160);
            lblFaqQ2.Location = new Point(10, 10);
            lblFaqQ2.Size = new Size(720, 20);
            lblFaqQ2.Click += (s, e) => ToggleFaq(2);
            pnlFaq2.Controls.Add(lblFaqQ2);

            lblFaqA2 = new Label();
            lblFaqA2.Text = "Bebekler mutlaka düz ve sert bir zeminde, sırtüstü (supine) pozisyonda yatırılmalıdır. Yatakta yastık, yorgan, pelüş oyuncak gibi nefes engelleyici nesneler olmamalıdır. Kiraz çekirdeği kemerleri veya hafif kundaklar zıbın üzerinden kaymayacak şekilde sabitlenmelidir.";
            lblFaqA2.Font = new Font("Segoe UI", 8.75F);
            lblFaqA2.ForeColor = Color.FromArgb(70, 70, 70);
            lblFaqA2.Location = new Point(10, 32);
            lblFaqA2.Size = new Size(720, 40);
            lblFaqA2.Click += (s, e) => ToggleFaq(2);
            pnlFaq2.Controls.Add(lblFaqA2);

            // FAQ 3 Accordion
            pnlFaq3 = new Panel();
            pnlFaq3.Size = new Size(740, 42);
            pnlFaq3.Location = new Point(25, 260);
            pnlFaq3.BackColor = Color.FromArgb(225, 242, 254);
            pnlFaq3.Cursor = Cursors.Hand;
            pnlFaq3.Click += (s, e) => ToggleFaq(3);
            pnlExpertCard.Controls.Add(pnlFaq3);

            lblFaqQ3 = new Label();
            lblFaqQ3.Text = "❓ Anne Sütünü Artırmak İçin Hangi Besinler Önerilir?";
            lblFaqQ3.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblFaqQ3.ForeColor = Color.FromArgb(10, 100, 190);
            lblFaqQ3.Location = new Point(10, 10);
            lblFaqQ3.Size = new Size(720, 20);
            lblFaqQ3.Click += (s, e) => ToggleFaq(3);
            pnlFaq3.Controls.Add(lblFaqQ3);

            lblFaqA3 = new Label();
            lblFaqA3.Text = "En önemli süt artırıcı faktör günde en az 3 litre su içmektir. Bunun yanı sıra yulaf ezmesi, dereotu, taze yeşillikler, rezene çayı ve arpa/malt ürünleri laktasyonu (süt üretimini) doğal olarak tetikler. Ancak en büyük süt artırıcı dinlenmiş bir zihindir.";
            lblFaqA3.Font = new Font("Segoe UI", 8.75F);
            lblFaqA3.ForeColor = Color.FromArgb(70, 70, 70);
            lblFaqA3.Location = new Point(10, 32);
            lblFaqA3.Size = new Size(720, 40);
            lblFaqA3.Click += (s, e) => ToggleFaq(3);
            pnlFaq3.Controls.Add(lblFaqA3);

            // FAQ 4 Accordion
            pnlFaq4 = new Panel();
            pnlFaq4.Size = new Size(740, 42);
            pnlFaq4.Location = new Point(25, 345);
            pnlFaq4.BackColor = Color.FromArgb(235, 250, 240);
            pnlFaq4.Cursor = Cursors.Hand;
            pnlFaq4.Click += (s, e) => ToggleFaq(4);
            pnlExpertCard.Controls.Add(pnlFaq4);

            lblFaqQ4 = new Label();
            lblFaqQ4.Text = "❓ Eşlerin Lohusalık Dönemindeki Rolü ve Görevleri Nelerdir?";
            lblFaqQ4.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblFaqQ4.ForeColor = Color.FromArgb(20, 110, 50);
            lblFaqQ4.Location = new Point(10, 10);
            lblFaqQ4.Size = new Size(720, 20);
            lblFaqQ4.Click += (s, e) => ToggleFaq(4);
            pnlFaq4.Controls.Add(lblFaqQ4);

            lblFaqA4 = new Label();
            lblFaqA4.Text = "Eşler, ev işlerini ve yemek sorumluluğunu tamamen üstlenerek anneyi bebek dışındaki yüklerden arındırmalıdır. Geceleri en azından bir besleme/alt değiştirme periyodunu devralıp annenin kesintisiz uyumasını sağlamak ve ona duygusal şefkat göstermek lohusalık depresyonunu neredeyse tamamen engeller.";
            lblFaqA4.Font = new Font("Segoe UI", 8.75F);
            lblFaqA4.ForeColor = Color.FromArgb(70, 70, 70);
            lblFaqA4.Location = new Point(10, 32);
            lblFaqA4.Size = new Size(720, 40);
            lblFaqA4.Click += (s, e) => ToggleFaq(4);
            pnlFaq4.Controls.Add(lblFaqA4);

            // Z-Order layout control to keep it perfectly structured and beautifully docked
            pnlRightColumn.SendToBack();
            pnlDivider.SendToBack();
            pnlLeftColumn.SendToBack();
            pnlDailyTip.SendToBack();
            pnlSubHeader.SendToBack();
            pnlHeader.SendToBack();

            // --- Dinamik Delete ve Report Butonlarının Kurulumu ---
            btnDeletePost = new Button();
            btnDeletePost.Text = "🗑️ Gönderiyi Sil";
            btnDeletePost.FlatStyle = FlatStyle.Flat;
            btnDeletePost.FlatAppearance.BorderSize = 0;
            btnDeletePost.BackColor = Color.FromArgb(255, 230, 235);
            btnDeletePost.ForeColor = Color.FromArgb(235, 90, 120);
            btnDeletePost.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            btnDeletePost.Cursor = Cursors.Hand;
            btnDeletePost.Location = new Point(10, 133);
            btnDeletePost.Size = new Size(130, 30);
            btnDeletePost.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnDeletePost.Visible = false; // Başlangıçta görünmez
            btnDeletePost.Click += new EventHandler(btnDeletePost_Click);
            pnlSelectedPostCard.Controls.Add(btnDeletePost);

            btnReportPost = new Button();
            btnReportPost.Text = "🚩 Şikayet Et";
            btnReportPost.FlatStyle = FlatStyle.Flat;
            btnReportPost.FlatAppearance.BorderSize = 0;
            btnReportPost.BackColor = Color.FromArgb(255, 243, 230);
            btnReportPost.ForeColor = Color.FromArgb(235, 140, 40);
            btnReportPost.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            btnReportPost.Cursor = Cursors.Hand;
            btnReportPost.Location = new Point(150, 133);
            btnReportPost.Size = new Size(110, 30);
            btnReportPost.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnReportPost.Visible = false; // Başlangıçta görünmez
            btnReportPost.Click += new EventHandler(btnReportPost_Click);
            pnlSelectedPostCard.Controls.Add(btnReportPost);

            // --- Farklı Ağlardaki Annelerle Senkronizasyon Arayüzü ---
            Label lblPeerSectionTitle = new Label();
            lblPeerSectionTitle.Text = "🌐 Farklı Ağlardaki Annelerle P2P Senkronizasyonu";
            lblPeerSectionTitle.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblPeerSectionTitle.ForeColor = Color.FromArgb(31, 38, 57);
            lblPeerSectionTitle.Location = new Point(20, 260);
            lblPeerSectionTitle.Size = new Size(450, 22);
            pnlSupportDirectoryCard.Controls.Add(lblPeerSectionTitle);

            Label lblPeerSectionDesc = new Label();
            lblPeerSectionDesc.Text = "Uygulama yerel Wi-Fi dışındaki ağlarda da çalışsın istiyorsanız, diğer annelerin IP adresini buraya ekleyerek sunucusuz P2P ağı kurabilirsiniz:";
            lblPeerSectionDesc.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            lblPeerSectionDesc.ForeColor = Color.DimGray;
            lblPeerSectionDesc.Location = new Point(20, 285);
            lblPeerSectionDesc.Size = new Size(450, 32);
            pnlSupportDirectoryCard.Controls.Add(lblPeerSectionDesc);

            // TextBox for entering Peer IP
            txtPeerIP = new TextBox();
            txtPeerIP.Location = new Point(20, 323);
            txtPeerIP.Size = new Size(160, 25);
            txtPeerIP.Font = new Font("Segoe UI", 9.5F);
            pnlSupportDirectoryCard.Controls.Add(txtPeerIP);

            // Button to add Peer
            btnConnectPeer = new Button();
            btnConnectPeer.Text = "🔗 Anne IP Ekle";
            btnConnectPeer.Location = new Point(190, 321);
            btnConnectPeer.Size = new Size(120, 27);
            btnConnectPeer.FlatStyle = FlatStyle.Flat;
            btnConnectPeer.FlatAppearance.BorderSize = 1;
            btnConnectPeer.FlatAppearance.BorderColor = Color.FromArgb(90, 100, 230);
            btnConnectPeer.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnConnectPeer.ForeColor = Color.FromArgb(90, 100, 230);
            btnConnectPeer.Click += new EventHandler(btnConnectPeer_Click);
            pnlSupportDirectoryCard.Controls.Add(btnConnectPeer);

            // Button to get my IP
            btnShowMyIP = new Button();
            btnShowMyIP.Text = "🔎 IP Adresim Nedir?";
            btnShowMyIP.Location = new Point(320, 321);
            btnShowMyIP.Size = new Size(130, 27);
            btnShowMyIP.FlatStyle = FlatStyle.Flat;
            btnShowMyIP.FlatAppearance.BorderSize = 1;
            btnShowMyIP.FlatAppearance.BorderColor = Color.FromArgb(120, 80, 220);
            btnShowMyIP.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnShowMyIP.ForeColor = Color.FromArgb(120, 80, 220);
            btnShowMyIP.Click += new EventHandler(btnShowMyIP_Click);
            pnlSupportDirectoryCard.Controls.Add(btnShowMyIP);

            // Label for list of active peers
            Label lblPeerListTitle = new Label();
            lblPeerListTitle.Text = "Ağda Kayıtlı Diğer Anneler / Eşler:";
            lblPeerListTitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblPeerListTitle.ForeColor = Color.FromArgb(80, 90, 110);
            lblPeerListTitle.Location = new Point(20, 360);
            lblPeerListTitle.Size = new Size(450, 18);
            pnlSupportDirectoryCard.Controls.Add(lblPeerListTitle);

            // ListBox for entered Peer IPs
            lstPeerIPs = new ListBox();
            lstPeerIPs.Location = new Point(20, 380);
            lstPeerIPs.Size = new Size(430, 80);
            lstPeerIPs.Font = new Font("Segoe UI", 9F);
            pnlSupportDirectoryCard.Controls.Add(lstPeerIPs);

            // Label to show total count of active peers (ping list indicator)
            lblActivePeersIndicator = new Label();
            lblActivePeersIndicator.Text = "🟢 Canlı Ağ Durumu: Sadece yerel ağdaki anneler taranıyor";
            lblActivePeersIndicator.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblActivePeersIndicator.ForeColor = Color.FromArgb(30, 130, 70);
            lblActivePeersIndicator.Location = new Point(20, 470);
            lblActivePeersIndicator.Size = new Size(430, 20);
            pnlSupportDirectoryCard.Controls.Add(lblActivePeersIndicator);

            // EPDS Sorularını Yükle
            InitializeEpdsQuestions();
        }

        private void InitializeEpdsQuestions()
        {
            epdsQuestions = new List<EpdsQuestion>();

            epdsQuestions.Add(new EpdsQuestion(
                "1. Gülebildim ve olayların eğlenceli tarafını görebildim:",
                new string[] { "Her zaman olduğu kadar", "Şimdilerde pek o kadar değil", "Açıkça oldukça az", "Hiçbir zaman" },
                new int[] { 0, 1, 2, 3 }
            ));

            epdsQuestions.Add(new EpdsQuestion(
                "2. Geleceğe hevesle baktım:",
                new string[] { "Her zaman olduğu kadar", "Şimdilerde her zamankinden biraz daha az", "Açıkça her zamankinden daha az", "Neredeyse hiç" },
                new int[] { 0, 1, 2, 3 }
            ));

            epdsQuestions.Add(new EpdsQuestion(
                "3. Bir şeyler kötü gittiğinde gereksiz yere kendimi suçladım:",
                new string[] { "Evet, çoğu zaman", "Evet, bazen", "Sıklıkla değil", "Hayır, hiçbir zaman" },
                new int[] { 3, 2, 1, 0 }
            ));

            epdsQuestions.Add(new EpdsQuestion(
                "4. Nedensiz yere kendimi sıkıntılı ve kaygılı hissettim:",
                new string[] { "Hayır, hiçbir zaman", "Pek sıklıkla değil", "Evet, bazen", "Evet, sıklıkla" },
                new int[] { 0, 1, 2, 3 }
            ));

            epdsQuestions.Add(new EpdsQuestion(
                "5. Nedensiz yere korktum ya da panikledim:",
                new string[] { "Evet, sıklıkla", "Evet, bazen", "Hayır, sıklıkla değil", "Hayır, hiçbir zaman" },
                new int[] { 3, 2, 1, 0 }
            ));

            epdsQuestions.Add(new EpdsQuestion(
                "6. Her şeyin üstesinden gelmeyecek kadar zorlaştığını hissettim:",
                new string[] { "Evet, çoğu zaman hiç baş edemedim", "Evet, bazen eskisi gibi baş edemedim", "Hayır, çoğu zaman baş edebildim", "Hayır, her zamanki gibi iyi baş edebildim" },
                new int[] { 3, 2, 1, 0 }
            ));

            epdsQuestions.Add(new EpdsQuestion(
                "7. Çok mutsuz olduğum için uyumakta zorlandım:",
                new string[] { "Evet, çoğu zaman", "Evet, sıklıkla", "Sıklıkla değil", "Hayır, hiçbir zaman" },
                new int[] { 3, 2, 1, 0 }
            ));

            epdsQuestions.Add(new EpdsQuestion(
                "8. Kendimi üzgün ya da kederli hissettim:",
                new string[] { "Evet, çoğu zaman", "Evet, sıklıkla", "Sıklıkla değil", "Hayır, hiçbir zaman" },
                new int[] { 3, 2, 1, 0 }
            ));

            epdsQuestions.Add(new EpdsQuestion(
                "9. Çok mutsuz olduğum için ağladım:",
                new string[] { "Evet, çoğu zaman", "Evet, sıklıkla", "Sadece bazen", "Hayır, hiçbir zaman" },
                new int[] { 3, 2, 1, 0 }
            ));

            epdsQuestions.Add(new EpdsQuestion(
                "10. Kendime zarar vermeyi düşündüm:",
                new string[] { "Evet, sıklıkla", "Bazen", "Neredeyse hiçbir zaman", "Hiçbir zaman" },
                new int[] { 3, 2, 1, 0 }
            ));
        }

        // --- SEKME GEÇİŞ EVENT HANDLER METOTLARI ---
        private void SetTabsInactive()
        {
            btnTabFeed.BackColor = Color.FromArgb(228, 231, 239);
            btnTabFeed.ForeColor = Color.FromArgb(80, 90, 110);
            btnTabEpds.BackColor = Color.FromArgb(228, 231, 239);
            btnTabEpds.ForeColor = Color.FromArgb(80, 90, 110);
            btnTabMood.BackColor = Color.FromArgb(228, 231, 239);
            btnTabMood.ForeColor = Color.FromArgb(80, 90, 110);
            btnTabExpert.BackColor = Color.FromArgb(228, 231, 239);
            btnTabExpert.ForeColor = Color.FromArgb(80, 90, 110);
            btnTabStats.BackColor = Color.FromArgb(228, 231, 239);
            btnTabStats.ForeColor = Color.FromArgb(80, 90, 110);
        }

        private void btnTabFeed_Click(object sender, EventArgs e)
        {
            SetTabsInactive();
            btnTabFeed.BackColor = Color.FromArgb(90, 100, 230);
            btnTabFeed.ForeColor = Color.White;

            pnlLeftColumn.Visible = true;
            pnlDivider.Visible = true;
            pnlRightColumn.Visible = true;
            pnlEpdsTabContent.Visible = false;
            pnlStatsTabContent.Visible = false;
            pnlMoodTabContent.Visible = false;
            pnlExpertTabContent.Visible = false;
        }

        private void btnTabEpds_Click(object sender, EventArgs e)
        {
            SetTabsInactive();
            btnTabEpds.BackColor = Color.FromArgb(90, 100, 230);
            btnTabEpds.ForeColor = Color.White;

            pnlLeftColumn.Visible = false;
            pnlDivider.Visible = false;
            pnlRightColumn.Visible = false;
            pnlEpdsTabContent.Visible = true;
            pnlStatsTabContent.Visible = false;
            pnlMoodTabContent.Visible = false;
            pnlExpertTabContent.Visible = false;

            // Testi ilk konumuna sıfırla
            ResetEpdsQuiz();
        }

        private void btnTabMood_Click(object sender, EventArgs e)
        {
            SetTabsInactive();
            btnTabMood.BackColor = Color.FromArgb(90, 100, 230);
            btnTabMood.ForeColor = Color.White;

            pnlLeftColumn.Visible = false;
            pnlDivider.Visible = false;
            pnlRightColumn.Visible = false;
            pnlEpdsTabContent.Visible = false;
            pnlStatsTabContent.Visible = false;
            pnlMoodTabContent.Visible = true;
            pnlExpertTabContent.Visible = false;

            RefreshMoodHistory();
        }

        private void btnTabExpert_Click(object sender, EventArgs e)
        {
            SetTabsInactive();
            btnTabExpert.BackColor = Color.FromArgb(90, 100, 230);
            btnTabExpert.ForeColor = Color.White;

            pnlLeftColumn.Visible = false;
            pnlDivider.Visible = false;
            pnlRightColumn.Visible = false;
            pnlEpdsTabContent.Visible = false;
            pnlStatsTabContent.Visible = false;
            pnlMoodTabContent.Visible = false;
            pnlExpertTabContent.Visible = true;
        }

        private void btnTabStats_Click(object sender, EventArgs e)
        {
            SetTabsInactive();
            btnTabStats.BackColor = Color.FromArgb(90, 100, 230);
            btnTabStats.ForeColor = Color.White;

            pnlLeftColumn.Visible = false;
            pnlDivider.Visible = false;
            pnlRightColumn.Visible = false;
            pnlEpdsTabContent.Visible = false;
            pnlStatsTabContent.Visible = true;
            pnlMoodTabContent.Visible = false;
            pnlExpertTabContent.Visible = false;

            // Canlı istatistikleri güncelle
            UpdateLiveStatistics();
        }

        // --- EPDS DEĞERLENDİRME KLİNİK TEST MANTIĞI ---
        private void btnEpdsStart_Click(object sender, EventArgs e)
        {
            pnlEpdsWelcome.Visible = false;
            pnlEpdsQuiz.Visible = true;
            pnlEpdsResult.Visible = false;
            
            epdsCurrentQuestionIndex = 0;
            epdsTotalScore = 0;
            LoadEpdsQuestion(0);
        }

        private void LoadEpdsQuestion(int index)
        {
            if (index < 0 || index >= epdsQuestions.Count) return;

            EpdsQuestion q = epdsQuestions[index];
            lblEpdsQNo.Text = "Tarama Değerlendirmesi: Soru " + (index + 1) + " / 10";
            lblEpdsQuestion.Text = q.Text;

            rbEpdsOpt1.Text = q.Options[0];
            rbEpdsOpt2.Text = q.Options[1];
            rbEpdsOpt3.Text = q.Options[2];
            rbEpdsOpt4.Text = q.Options[3];

            // Seçimleri temizle
            rbEpdsOpt1.Checked = false;
            rbEpdsOpt2.Checked = false;
            rbEpdsOpt3.Checked = false;
            rbEpdsOpt4.Checked = false;

            if (index == epdsQuestions.Count - 1)
            {
                btnEpdsNext.Text = "Testi Tamamla ➔";
            }
            else
            {
                btnEpdsNext.Text = "Sonraki Soru ➔";
            }
        }

        private void btnEpdsNext_Click(object sender, EventArgs e)
        {
            int selectedIndex = -1;
            if (rbEpdsOpt1.Checked) selectedIndex = 0;
            else if (rbEpdsOpt2.Checked) selectedIndex = 1;
            else if (rbEpdsOpt3.Checked) selectedIndex = 2;
            else if (rbEpdsOpt4.Checked) selectedIndex = 3;

            if (selectedIndex == -1)
            {
                MessageBox.Show("Lütfen kendinize en yakın hissi seçip işaretleyiniz.", "Seçim Yapılmadı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Puanı ekle
            epdsTotalScore += epdsQuestions[epdsCurrentQuestionIndex].OptionScores[selectedIndex];

            // Sonraki soruya geç veya testi bitir
            epdsCurrentQuestionIndex++;
            if (epdsCurrentQuestionIndex < epdsQuestions.Count)
            {
                LoadEpdsQuestion(epdsCurrentQuestionIndex);
            }
            else
            {
                ShowEpdsResult();
            }
        }

        private void ShowEpdsResult()
        {
            pnlEpdsQuiz.Visible = false;
            pnlEpdsResult.Visible = true;

            lblEpdsResultScore.Text = "Edinburgh Değerlendirme Skorunuz: " + epdsTotalScore + " / 30 Puan";
            DataHelper.Log("[EPDS TESTİ] EPDS testi tamamlandı. Skor: " + epdsTotalScore + " / 30");

            if (epdsTotalScore < 10)
            {
                lblEpdsResultTitle.Text = "Ruh Sağlığı Raporu: Klinik Risk Düşük (Güvendesiniz) 🟢";
                lblEpdsResultDesc.Text = "Test sonucunuza göre şu an için belirgin bir lohusalık depresyonu riski taşımıyorsunuz. Lohusalık sürecindeki hafif yorgunluk, uykusuzluk ve duygusal dalgalanmalar normaldir. Dinlenmeye, dengeli beslenmeye ve kendinize şefkatli davranmaya özen gösterin. Topluluğumuzda diğer annelere destek yorumları yazarak şefkat köprüleri kurabilirsiniz. Her zaman yalnız olmadığınızı unutmayın! 🌸";
            }
            else if (epdsTotalScore <= 12)
            {
                lblEpdsResultTitle.Text = "Ruh Sağlığı Raporu: Hafif Risk & Lohusalık Hüznü (Baby Blues) 🟡";
                lblEpdsResultDesc.Text = "Sonuçlarınız hafif düzeyde bir duygusal zorlanma ve kaygı yaşadığınızı göstermektedir. Bu durum lohusalıkta ilk haftalarda son derece doğal karşılanan 'Lohusalık Hüznü' sürecidir. Ailenizden ve eşinizden ev işleri ve bebek bakımında mutlaka yardım talep edin. Günlük en az 2 saat kesintisiz dinlenmeye çalışın. Akış üzerinden diğer annelerle dertleşmek zihninizi rahatlatacaktır. Süreç geçicidir. 🌸";
            }
            else
            {
                lblEpdsResultTitle.Text = "Ruh Sağlığı Raporu: Yüksek Klinik Risk (Destek Önerilir) 🔴";
                lblEpdsResultDesc.Text = "Test skorunuz, lohusalık depresyonu riski taşıyor olabileceğinizi göstermektedir. Lütfen kendinizi asla yetersiz hissetmeyin veya suçlamayın; lohusalık depresyonu tamamen hormonal ve fizyolojik kaynaklı, tıbbi tedavisi son derece kolay olan yaygın bir durumdur. Atacağınız en sağlıklı adım, bir psikolog veya psikiyatristten profesyonel destek almaktır. Yan sekmedeki 'Resmi Destek Hatları' alanından ALO 191'i ücretsiz arayarak hemen uzmanlarımızla konuşabilirsiniz. 🌸";
            }
        }

        private void btnEpdsRestart_Click(object sender, EventArgs e)
        {
            ResetEpdsQuiz();
        }

        private void ResetEpdsQuiz()
        {
            pnlEpdsWelcome.Visible = true;
            pnlEpdsQuiz.Visible = false;
            pnlEpdsResult.Visible = false;
            epdsCurrentQuestionIndex = 0;
            epdsTotalScore = 0;
        }

        // --- CANLI İSTATİSTİK VE ANALİTİK RAPORLAMA ---
        private void UpdateLiveStatistics()
        {
            int totalPosts = db.Posts.Count;
            int totalComments = 0;
            int totalLikes = 0;

            int psikolojikCount = 0;
            int bebekCount = 0;
            int beslenmeCount = 0;

            foreach (var post in db.Posts)
            {
                totalLikes += post.LikeCount;
                totalComments += post.Comments.Count;

                if (post.Category == "Psikolojik Destek") psikolojikCount++;
                else if (post.Category == "Bebek Bakımı") bebekCount++;
                else if (post.Category == "Beslenme & Sağlık") beslenmeCount++;
            }

            lblStatsTotalPosts.Text = totalPosts + "\nPaylaşılan Soru";
            lblStatsTotalComments.Text = totalComments + "\nDestek Yorumu";
            lblStatsTotalLikes.Text = totalLikes + "\nEmpati / Beğeni";

            if (totalPosts > 0)
            {
                int pctPsikolojik = (psikolojikCount * 100) / totalPosts;
                int pctBebek = (bebekCount * 100) / totalPosts;
                int pctBeslenme = (beslenmeCount * 100) / totalPosts;

                lblBreakdownPsikolojik.Text = "Psikolojik Destek (%" + pctPsikolojik + " - " + psikolojikCount + " Gönderi)";
                lblBreakdownBebek.Text = "Bebek Bakımı (%" + pctBebek + " - " + bebekCount + " Gönderi)";
                lblBreakdownBeslenme.Text = "Beslenme & Sağlık (%" + pctBeslenme + " - " + beslenmeCount + " Gönderi)";

                pbarPsikolojikFill.Width = (int)(pbarPsikolojik.Width * (psikolojikCount / (double)totalPosts));
                pbarBebekFill.Width = (int)(pbarBebek.Width * (bebekCount / (double)totalPosts));
                pbarBeslenmeFill.Width = (int)(pbarBeslenme.Width * (beslenmeCount / (double)totalPosts));
            }
            else
            {
                lblBreakdownPsikolojik.Text = "Psikolojik Destek (%0)";
                lblBreakdownBebek.Text = "Bebek Bakımı (%0)";
                lblBreakdownBeslenme.Text = "Beslenme & Sağlık (%0)";

                pbarPsikolojikFill.Width = 0;
                pbarBebekFill.Width = 0;
                pbarBeslenmeFill.Width = 0;
            }
        }

        // --- DESTEK HATLARI BUTON TIKLAMA HAREKETLERİ ---
        private void btnHotline191_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Sağlık Bakanlığı ALO 191 Danışma Hattı;\n\nLohusalık sendromu, kaygı, depresyon veya yoğun yalnızlık çeken tüm annelerimize 7/24 tamamen ücretsiz ve gizli psikolojik danışmanlık ve uzman desteği sunmaktadır. Çekinmeden arayabilirsiniz.",
                "T.C. Sağlık Bakanlığı ALO 191",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void btnHotline182_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "ALO 182 Hastane Randevu Sistemi;\n\nDevlet hastanelerinden uzman bir psikolog veya psikiyatrist randevusu oluşturmak, profesyonel klinik destek almak için 182 numaralı hattı arayabilir ya da MHRS internet portalını kullanabilirsiniz.",
                "MHRS Uzman Randevu Bilgi",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void btnHotlineKades_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "KADES Kadın Destek Uygulaması;\n\nEşler veya çevre kaynaklı olası aile içi şiddet, tehdit ve güvenlik riski anlarında tek dokunuşla emniyet güçlerini çağırmanızı sağlayan resmi uygulamadır. Akıllı telefonunuza mutlaka indiriniz.",
                "KADES Acil Güvenlik Bilgisi",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void btnShowContract_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Topluluk Yardımlaşma ve Empati İlkelerimiz:\n\n" +
                "1. Karşılıksız Destek: Her yeni annenin zor zamanlar yaşayabileceğini unutmayın ve empati gösterin.\n" +
                "2. Nazik ve Sevgi Dolu Dil: Küfürlü, hakaret içerikli veya yargılayıcı paylaşımlara asla geçit verilmez.\n" +
                "3. Anonimlik İlkesi: Güvenliğiniz için gerçek isim, adres veya telefon numarası paylaşmayın.\n" +
                "4. Şefkat Köprüsü: Burası klinik teşhis yeri değildir; kalpten kalbe dertleşen, şefkatli bir yardımlaşma yuvasıdır. 🌸",
                "Topluluk Yardımlaşma Sözleşmesi",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        // --- MOOD TRACKER & FAQ ACCORDION METHODLARI (PHASE 2) ---
        private void RegisterMood(string emoji, string desc)
        {
            if (currentUser == null)
            {
                MessageBox.Show("Duygu kaydetmek için lütfen önce giriş yapın.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Bugüne ait eski kaydı varsa silelim ki mükerrer olmasın
            db.Moods.RemoveAll(m => m.Nickname == currentUser.Nickname && m.CreatedAt.Date == DateTime.Today);

            // Yeni kaydı ekle
            db.Moods.Add(new MoodEntry(currentUser.Nickname, emoji, desc));
            DataHelper.SaveData(db);

            MessageBox.Show("Bugünkü ruh haliniz '" + emoji + " (" + desc + ")' başarıyla günlüğünüze kaydedildi! 🌸", "Ruh Hali Güncellendi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            RefreshMoodHistory();
        }

        private void RefreshMoodHistory()
        {
            lstMoodHistory.Items.Clear();
            if (currentUser == null) return;

            // Giriş yapmış kullanıcının duygu durum geçmişini tersten listele (en son ilk başta)
            var userMoods = db.Moods
                .Where(m => m.Nickname == currentUser.Nickname)
                .OrderByDescending(m => m.CreatedAt)
                .ToList();

            foreach (var mood in userMoods)
            {
                lstMoodHistory.Items.Add(mood);
            }
        }

        private void lstMoodHistory_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 48; // Her bir duygu geçmişi satırının yüksekliği
        }

        private void lstMoodHistory_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= lstMoodHistory.Items.Count) return;

            MoodEntry mood = (MoodEntry)lstMoodHistory.Items[e.Index];
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Arka planı temizle
            Color cardColor = Color.FromArgb(255, 250, 251);
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                cardColor = Color.FromArgb(255, 235, 238);
            }

            using (SolidBrush bgBrush = new SolidBrush(cardColor))
            {
                g.FillRectangle(bgBrush, e.Bounds);
            }

            // Kart çerçevesi çizimi
            using (Pen borderPen = new Pen(Color.FromArgb(245, 220, 225), 1))
            {
                g.DrawRectangle(borderPen, e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
            }

            // İçerik Çizimi
            Font emojiFont = new Font("Segoe UI", 16F);
            Font textFont = new Font("Segoe UI", 9.25F, FontStyle.Bold);
            Font dateFont = new Font("Segoe UI", 8.25F, FontStyle.Italic);

            using (SolidBrush emojiBrush = new SolidBrush(Color.Black))
            using (SolidBrush textBrush = new SolidBrush(Color.FromArgb(190, 80, 110)))
            using (SolidBrush dateBrush = new SolidBrush(Color.Gray))
            {
                g.DrawString(mood.MoodName, emojiFont, emojiBrush, e.Bounds.X + 15, e.Bounds.Y + 6);
                g.DrawString("Duygu Durumu: " + mood.MoodDescription, textFont, textBrush, e.Bounds.X + 55, e.Bounds.Y + 8);
                g.DrawString("Kayıt Tarihi: " + mood.CreatedAt.ToString("dd.MM.yyyy HH:mm"), dateFont, dateBrush, e.Bounds.X + 55, e.Bounds.Y + 26);
            }
        }

        private void ToggleFaq(int faqIndex)
        {
            Panel faqPanel = null;
            if (faqIndex == 1) faqPanel = pnlFaq1;
            else if (faqIndex == 2) faqPanel = pnlFaq2;
            else if (faqIndex == 3) faqPanel = pnlFaq3;
            else if (faqIndex == 4) faqPanel = pnlFaq4;

            if (faqPanel == null) return;

            // Eğer panelin yüksekliği 42 (yani kapalıysa) açalım (yükseklik 75)
            // Eğer açık ise kapatalım (yükseklik 42)
            if (faqPanel.Height < 50)
            {
                faqPanel.Height = 75;
                faqPanel.BackColor = Color.White;
                faqPanel.BorderStyle = BorderStyle.FixedSingle;
            }
            else
            {
                faqPanel.Height = 42;
                faqPanel.BorderStyle = BorderStyle.None;
                if (faqIndex == 1) faqPanel.BackColor = Color.FromArgb(255, 240, 242);
                else if (faqIndex == 2) faqPanel.BackColor = Color.FromArgb(243, 230, 255);
                else if (faqIndex == 3) faqPanel.BackColor = Color.FromArgb(225, 242, 254);
                else if (faqIndex == 4) faqPanel.BackColor = Color.FromArgb(235, 250, 240);
            }
        }
    }

    // ==========================================
    // KÖŞELERİ YUVARLATMA YARDIMCI HELPER SINIFI
    // ==========================================
    public static class RoundedControlHelper
    {
        public static void MakeRounded(Control control, int radius)
        {
            control.SizeChanged += (s, e) => ApplyRegion(control, radius);
            ApplyRegion(control, radius);
        }

        private static void ApplyRegion(Control control, int radius)
        {
            if (control.Width <= radius || control.Height <= radius) return;
            
            using (System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                int d = radius * 2;
                path.StartFigure();
                path.AddArc(new Rectangle(0, 0, d, d), 180, 90);
                path.AddArc(new Rectangle(control.Width - d, 0, d, d), 270, 90);
                path.AddArc(new Rectangle(control.Width - d, control.Height - d, d, d), 0, 90);
                path.AddArc(new Rectangle(0, control.Height - d, d, d), 90, 90);
                path.CloseFigure();
                control.Region = new Region(path);
            }
        }
    }

    // ==========================================
    // VERİ MODELLERİ VE YEREL VERİ HELPER SINIFI
    // ==========================================

    public class User
    {
        public string Nickname { get; set; }
        public string Password { get; set; }

        // XML serialization için parametresiz kurucu metot zorunludur
        public User() { }

        public User(string nickname, string password)
        {
            Nickname = nickname;
            Password = password;
        }
    }

    public class Comment
    {
        public string Nickname { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public Comment() { }

        public Comment(string nickname, string content)
        {
            Nickname = nickname;
            Content = content;
            CreatedAt = DateTime.Now;
        }
    }

    public class Post
    {
        public string Id { get; set; }
        public string Nickname { get; set; }
        public string Content { get; set; }
        public string Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Comment> Comments { get; set; }
        public int LikeCount { get; set; }
        public List<string> ReportedBy { get; set; }

        public Post()
        {
            Comments = new List<Comment>();
            ReportedBy = new List<string>();
        }

        public Post(string nickname, string content, string category)
        {
            Id = Guid.NewGuid().ToString();
            Nickname = nickname;
            Content = content;
            Category = category;
            CreatedAt = DateTime.Now;
            Comments = new List<Comment>();
            LikeCount = 0;
            ReportedBy = new List<string>();
        }
    }

    public class MoodEntry
    {
        public string Nickname { get; set; }
        public string MoodName { get; set; }
        public string MoodDescription { get; set; }
        public DateTime CreatedAt { get; set; }

        public MoodEntry() { }

        public MoodEntry(string nickname, string moodName, string moodDescription)
        {
            Nickname = nickname;
            MoodName = moodName;
            MoodDescription = moodDescription;
            CreatedAt = DateTime.Now;
        }
    }

    public class Database
    {
        public List<User> Users { get; set; }
        public List<Post> Posts { get; set; }
        public List<MoodEntry> Moods { get; set; }
        public List<string> CustomPeers { get; set; }

        public Database()
        {
            Users = new List<User>();
            Posts = new List<Post>();
            Moods = new List<MoodEntry>();
            CustomPeers = new List<string>();
        }
    }

    public static class DataHelper
    {
        private static readonly string DbDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Veritabani");
        private static readonly string FilePath = Path.Combine(DbDirectory, "veriler.xml");
        private static readonly string LogFilePath = Path.Combine(DbDirectory, "Uygulama_Loglari.txt");

        public static Database LoadData()
        {
            try
            {
                if (!Directory.Exists(DbDirectory))
                {
                    Directory.CreateDirectory(DbDirectory);
                }

                if (!File.Exists(FilePath))
                {
                    // Yedek dosya var mı kontrol et, varsa oradan yükle
                    string backupPath = FilePath + ".bak";
                    if (File.Exists(backupPath))
                    {
                        try
                        {
                            XmlSerializer serializerBak = new XmlSerializer(typeof(Database));
                            using (FileStream fs = new FileStream(backupPath, FileMode.Open))
                            {
                                Database dbBak = (Database)serializerBak.Deserialize(fs);
                                File.Copy(backupPath, FilePath, true); // Asıl dosyayı yedekten kurtar
                                return dbBak;
                            }
                        }
                        catch { }
                    }

                    Database newDb = new Database();
                    SeedSampleData(newDb); // İlk kez açıldığında boş görünmemesi için örnek veriler eklenir
                    SaveData(newDb);
                    return newDb;
                }

                XmlSerializer serializer = new XmlSerializer(typeof(Database));
                using (FileStream fs = new FileStream(FilePath, FileMode.Open))
                {
                    return (Database)serializer.Deserialize(fs);
                }
            }
            catch
            {
                // Hata durumunda da yedek dosyayı kurtarmayı dene
                string backupPath = FilePath + ".bak";
                if (File.Exists(backupPath))
                {
                    try
                    {
                        XmlSerializer serializerBak = new XmlSerializer(typeof(Database));
                        using (FileStream fs = new FileStream(backupPath, FileMode.Open))
                        {
                            Database dbBak = (Database)serializerBak.Deserialize(fs);
                            File.Copy(backupPath, FilePath, true);
                            return dbBak;
                        }
                    }
                    catch { }
                }

                // Herhangi bir dosya hatasında sıfırdan veritabanı döner
                Database db = new Database();
                SeedSampleData(db);
                return db;
            }
        }

        public static void SaveData(Database db)
        {
            try
            {
                if (!Directory.Exists(DbDirectory))
                {
                    Directory.CreateDirectory(DbDirectory);
                }

                // EŞSİZ GÜVENLİK ÖZELLİĞİ: Her kayıtta bir yedek dosya oluştur (veriler.xml.bak)
                if (File.Exists(FilePath))
                {
                    try
                    {
                        string backupPath = FilePath + ".bak";
                        File.Copy(FilePath, backupPath, true);
                    }
                    catch { }
                }

                XmlSerializer serializer = new XmlSerializer(typeof(Database));
                using (FileStream fs = new FileStream(FilePath, FileMode.Create))
                {
                    serializer.Serialize(fs, db);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veriler yerel dosyaya kaydedilirken hata oluştu: " + ex.Message, "Kayıt Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void Log(string message)
        {
            try
            {
                if (!Directory.Exists(DbDirectory))
                {
                    Directory.CreateDirectory(DbDirectory);
                }
                string logLine = "[" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "] " + message + Environment.NewLine;
                File.AppendAllText(LogFilePath, logLine);
            }
            catch { }
        }

        public static string SerializeToXml<T>(T obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, obj);
                return writer.ToString();
            }
        }

        public static T DeserializeFromXml<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(xml))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        private static void SeedSampleData(Database db)
        {
            // Örnek Kullanıcılar
            db.Users.Add(new User("DeneyimliAnne", "123456"));
            db.Users.Add(new User("UykusuzBuket", "123456"));
            db.Users.Add(new User("YeniAnneEzgi", "123456"));

            // Örnek Gönderi 1
            Post p1 = new Post("YeniAnneEzgi", "Lohusalığın 2. haftasındayım. Bebeğimi çok sevmeme rağmen içimde sürekli anlamsız bir korku ve ağlama hissi var. Eşime de yansıtmamaya çalışıyorum ama çok zorlanıyorum. Benimle aynı şeyleri yaşayan var mı?", "Psikolojik Destek");
            p1.LikeCount = 12;
            p1.Comments.Add(new Comment("DeneyimliAnne", "Çok normal canım benim, hormon dalgalanması yaşıyorsun. 40'ın çıkana kadar kendine izin ver, bol bol uyu ve ağlamaktan çekinme. Yalnız değilsin. ❤️"));
            p1.Comments.Add(new Comment("UykusuzBuket", "Birebir aynı durumdayım Ezgi Hanım. Akşam olunca özellikle üstüme duvarlar geliyor gibi oluyor. Sadece bunun geçici bir süreç olduğunu unutmayın, birbirimize destek olacağız."));

            // Örnek Gönderi 2
            Post p2 = new Post("UykusuzBuket", "Bebeğimin gaz sancısı yüzünden 3 gündür kesintisiz uyku uyumadık. Hangi damlayı veya masajı önerirsiniz? Kiraz çekirdeği kemeri aldık ama tam çözemedik.", "Bebek Bakımı");
            p2.LikeCount = 8;
            p2.Comments.Add(new Comment("DeneyimliAnne", "Kiraz çekirdeğini fırında çok hafif ısıtıp zıbın üzerinden koyun. Ayaklarına da ılık yağ ile masaj yapıp karnına doğru bacaklarını ittirin (bisiklet hareketi). Çok rahatlatır!"));

            // Örnek Gönderi 3
            Post p3 = new Post("DeneyimliAnne", "Sütümü artırmak için bol su içmek dışında gerçekten işe yarayan besinler nelerdir? Malt içeceği öneren var ama emin olamadım.", "Beslenme & Sağlık");
            p3.LikeCount = 15;
            p3.Comments.Add(new Comment("YeniAnneEzgi", "Bana doktorum yulaf ezmesi ve dereotu önermişti, ben her gün tüketiyorum ve çok faydasını gördüm. Bir de tabii ki en büyük formül stresten uzak kalıp dinlenebilmek."));

            db.Posts.Add(p1);
            db.Posts.Add(p2);
            db.Posts.Add(p3);

            // Örnek Duygu Kayıtları
            db.Moods.Add(new MoodEntry("DeneyimliAnne", "😊", "Mutlu ve Huzurlu"));
            db.Moods.Add(new MoodEntry("YeniAnneEzgi", "😴", "Aşırı Yorgun"));
            db.Moods.Add(new MoodEntry("UykusuzBuket", "😟", "Kaygılı / Endişeli"));
        }
    }

    // ==========================================
    // ACIL DESTEK TARAMA TESTİ MODELİ
    // ==========================================
    public class EpdsQuestion
    {
        public string Text { get; set; }
        public string[] Options { get; set; }
        public int[] OptionScores { get; set; }

        public EpdsQuestion(string text, string[] options, int[] scores)
        {
            Text = text;
            Options = options;
            OptionScores = scores;
        }
    }

    public class NetworkPacket
    {
        public string Type { get; set; } // "NEW_POST", "NEW_COMMENT", "LIKE_POST"
        public string Payload { get; set; } // XML formatında serialized nesne
        public string Extra { get; set; } // Comment için PostId veya Like için PostId

        public NetworkPacket() { }
        public NetworkPacket(string type, string payload, string extra = "")
        {
            Type = type;
            Payload = payload;
            Extra = extra;
        }
    }
}
