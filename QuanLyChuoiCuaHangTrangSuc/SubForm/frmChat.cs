using Guna.UI2.WinForms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace QuanLyChuoiCuaHangTrangSuc.SubForm.ChatForm
{
    public partial class frmChat : Form
    {
        public frmChat()
        {
            InitializeComponent();
            

        }

        private void frmChat_Load(object sender, EventArgs e)
        {
            txtMessage.Focus(); // Đặt con trỏ vào ô nhập tin nhắn

            var apiKey = "AIzaSyDHbhQT-lJ2kafZAdmAiWRDHVmz7reB1GQ";
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={apiKey}";
            var today = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            var requestData = new
            {
                contents = new[]
                {


                    new
                    {
                        parts = new[]
                        {

                            new { text = "Bạn là một trợ lý AI thân thiện và thông minh, trả lời ngắn gọn, chính xác, không dài dòng. Khi có ai đó hỏi những câu hỏi như bạn là ai,... hãy trả lời rằng mình là Trợ lý ảo của TeeNStyle" +
                            "Hãy luôn bám sát mạch câu chuyện và thông tin đã được nêu trước đó trong cuộc trò chuyện. Không suy diễn hoặc mở rộng quá mức trừ khi được yêu cầu. Trả lời bằng tiếng Việt, không vượt quá 500 từ. " +
                            $"Bối cảnh: Các câu hỏi đến từ Việt Nam, và ngày hiện tại là {today}. " +
                            "Quan trọng: Hãy luôn nhớ nội dung các tin nhắn trước để trả lời hợp lý theo mạch. Chỉ reset khi được yêu cầu rõ ràng như bắt đầu câu chuyện mới hoặc reload. " +
                            "Không cần trả lời lại lời nhắc này. Hãy trả lời từ lời nhắc tiếp theo." }
                        }
                    }
                }
            };

            AddChatBubble("Chào bạn, tôi là trợ lý của TeeNStyle. Tôi có thể giúp gì cho bạn?", false);
            txtMessage.Focus(); // Đặt con trỏ vào ô nhập tin nhắn
        }



        private void AddChatBubble(string message, bool isUser)
        {
            // Panel bao ngoài (để chứa cả dòng tin nhắn)
            Panel outerPanel = new Panel();
            outerPanel.Dock = DockStyle.Top;
            outerPanel.AutoSize = true;
            outerPanel.BackColor = Color.Transparent;
            outerPanel.Padding = new Padding(5); // giảm khoảng cách giữa các tin nhắn

            // Tạo một FlowLayoutPanel để canh trái/phải tùy người gửi
            FlowLayoutPanel flow = new FlowLayoutPanel();
            flow.FlowDirection = isUser ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            flow.AutoSize = true;
            flow.WrapContents = false;
            flow.BackColor = Color.Transparent;
            flow.Dock = DockStyle.Fill;
            flow.Margin = new Padding(5);

            // Nếu là AI thì thêm avatar
            if (!isUser)
            {
                Guna2CirclePictureBox avatar = new Guna2CirclePictureBox();
                avatar.Size = new Size(36, 36);
                avatar.SizeMode = PictureBoxSizeMode.Zoom;
                avatar.Image = Properties.Resources.avtAI;
                avatar.Margin = new Padding(0, 5, 5, 0);
                flow.Controls.Add(avatar);
            }

            // Panel bubble chứa tin nhắn
            Guna2Panel bubble = new Guna2Panel();
            bubble.BorderRadius = 10;
            bubble.Padding = new Padding(10, 5, 10, 5); // padding đều, vừa phải để căn giữa nội dung theo chiều dọc
            bubble.FillColor = isUser ? Color.FromArgb(30, 45, 70) : Color.LightGray;
            bubble.MaximumSize = new Size(chatPanel.Width - 100, 0);
            bubble.AutoSize = true;

            // Label nội dung tin nhắn
            Guna2HtmlLabel lblMessage = new Guna2HtmlLabel();
            lblMessage.BackColor = Color.Transparent;
            lblMessage.ForeColor = isUser ? Color.White : Color.Black;
            lblMessage.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblMessage.Text = FormatMessage(message);
            lblMessage.MaximumSize = new Size(chatPanel.Width - 130, 0);
            lblMessage.AutoSize = true;
            lblMessage.Padding = new Padding(5, 0, 0, 0); // Cách trái 5px
            lblMessage.TextAlignment = ContentAlignment.MiddleLeft;

            // Gói label trong một container để giúp căn giữa dọc chính xác hơn nếu muốn mở rộng
            Panel messageContainer = new Panel();
            messageContainer.AutoSize = true;
            messageContainer.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            messageContainer.BackColor = Color.Transparent;
            messageContainer.Controls.Add(lblMessage);
            messageContainer.Dock = DockStyle.Fill;

            // Add vào bubble và flow
            bubble.Controls.Add(messageContainer);
            flow.Controls.Add(bubble);
            outerPanel.Controls.Add(flow);

            // Thêm vào chatPanel
            chatPanel.Controls.Add(outerPanel);
            chatPanel.Controls.SetChildIndex(outerPanel, 0); // tin nhắn mới ở dưới cùng
            chatPanel.ScrollControlIntoView(outerPanel);
        }

        private string FormatMessage(string input)
        {
            return input.Replace("*", "");
          
        }




        private async Task<string> GetGeminiResponse(string userInput)
        {
            var apiKey = "AIzaSyDHbhQT-lJ2kafZAdmAiWRDHVmz7reB1GQ"; 
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={apiKey}";
            var today = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            var requestData = new
            {
                contents = new[]
                {


                    new
                    {
                        parts = new[]
                        {

                            new { text = userInput }
                        }
                    }
                }
            };

            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();

                dynamic responseJson = JsonConvert.DeserializeObject(responseString);

                // Trích nội dung phản hồi đầu tiên
                return responseJson.candidates[0].content.parts[0].text.ToString();
            }
        }



        private async void txtMessage_IconRightClick(object sender, EventArgs e)
        {
            var userMessage = txtMessage.Text.Trim();
            if (!string.IsNullOrEmpty(userMessage))
            {
                AddChatBubble(userMessage, true); // true = user
                txtMessage.Clear();

                var aiResponse =await GetGeminiResponse(userMessage); // hoặc GetGeminiResponse
                AddChatBubble(aiResponse, false); // false = AI
            }

        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            this.Controls.Clear(); // Xóa controls hiện tại
            this.InitializeComponent(); // Tạo lại controls
            this.frmChat_Load(null, null); // Gọi lại hàm load
        }

        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtMessage_IconRightClick(sender, e);
                e.SuppressKeyPress = true; // Chặn tiếng beep hoặc hành động mặc định
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            txtMessage_IconRightClick(sender, e); // Gọi hàm gửi tin nhắn
        }

        private void btnSupport_Click(object sender, EventArgs e)
        {
            txtMessage_IconRightClick(sender, e); // Gọi hàm gửi tin nhắn
        }
    }
}
