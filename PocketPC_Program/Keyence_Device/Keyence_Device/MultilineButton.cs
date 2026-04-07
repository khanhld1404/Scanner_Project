using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Keyence_Device.Class;
namespace Keyence_Device
{
    public class MultilineButton : Control
    {

        private bool _pressed = false;

        public MultilineButton()
        {
            this.BackColor = Color.Lime;
            this.ForeColor = Color.Black;
            // DÙNG font có sẵn trên thiết bị và có tiếng Việt
            this.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            this.Enabled = true;


            // Không cho focus/tab tới control này
            this.TabStop = false;

        }

        private int _borderWidth = 1;
        public int BorderWidth
        {
            get { return _borderWidth; }
            set { _borderWidth = Math.Max(0, value); Invalidate(); }
        }

        private Color _borderColor = Color.Black;
        public Color BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            Rectangle rc = this.ClientRectangle;

            // nền (đổi khi nhấn)
            using (SolidBrush b = new SolidBrush(_pressed ? Darken(this.BackColor, 0.15f) : this.BackColor))
                g.FillRectangle(b, rc);

            // viền
            if (_borderWidth > 0)
            {
                using (Pen p = new Pen(_borderColor, _borderWidth))
                    g.DrawRectangle(p, rc.X, rc.Y, rc.Width - 1, rc.Height - 1);
            }

            // vẽ chữ: tự wrap + hỗ trợ \n
            Rectangle textRect = rc;
            textRect.Inflate(-4, -4);
            DrawMultilineText(g, this.Text, this.Font, this.ForeColor, textRect);
        }


        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!this.Enabled) return;

            // Nếu đang suppress toàn cục → bỏ qua (tránh click-through)
            if (InputGuard.IsSuppressed()) return;

            this.Capture = true;
            _pressed = true;
            Invalidate();

            // (không đổi) RAISE CLICK sớm
            base.OnClick(EventArgs.Empty);

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (!this.Enabled) return;
            _pressed = false;
            Invalidate();
            this.Capture = false;
            base.OnMouseUp(e);
        }



        private static Color Darken(Color c, float amount)
        {
            if (amount < 0f) amount = 0f;
            if (amount > 1f) amount = 1f;
            int r = (int)(c.R * (1f - amount));
            int g = (int)(c.G * (1f - amount));
            int b = (int)(c.B * (1f - amount));
            return Color.FromArgb(r, g, b);
        }

        // ====== Word-wrap thủ công cho CF ======
        private void DrawMultilineText(Graphics g, string text, Font font, Color color, Rectangle bounds)
        {
            if (string.IsNullOrEmpty(text))
                return;

            // Bắt buộc dùng \n (CF có thể không xử lý \r\n)
            text = text.Replace("\r\n", "\n");

            using (SolidBrush brush = new SolidBrush(this.Enabled ? color : SystemColors.GrayText))
            {
                int lineHeight = (int)Math.Ceiling(g.MeasureString("Ag", font).Height);
                int y = bounds.Top;

                string[] logicalLines = text.Split('\n'); // người dùng tự chèn \n
                foreach (string logical in logicalLines)
                {
                    string remaining = logical;
                    while (remaining.Length > 0)
                    {
                        string fitted = FitLine(g, font, remaining, bounds.Width);
                        if (fitted.Length == 0) break;

                        g.DrawString(fitted, font, brush, new Rectangle(bounds.Left, y, bounds.Width, lineHeight));
                        y += lineHeight;

                        if (y + lineHeight > bounds.Bottom) return; // hết chỗ
                        remaining = remaining.Substring(fitted.Length).TrimStart(); // bỏ khoảng trắng đầu dòng
                    }
                }
            }
        }

        // Trả về đoạn con của 'text' vừa khít chiều rộng 'maxWidth'
        private string FitLine(Graphics g, Font font, string text, int maxWidth)
        {
            // Nếu toàn bộ vừa thì trả về ngay
            SizeF full = g.MeasureString(text, font);
            if (full.Width <= maxWidth) return text;

            // Cắt theo từ (space)
            int lastSpace = -1;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == ' ') lastSpace = i;
                string sub = text.Substring(0, i + 1);
                SizeF sz = g.MeasureString(sub, font);
                if (sz.Width > maxWidth)
                {
                    if (lastSpace > 0)
                        return text.Substring(0, lastSpace); // cắt tại space gần nhất
                    else
                        return text.Substring(0, Math.Max(1, i)); // từ quá dài: cắt cứng
                }
            }
            return text; // fallback
        }

    }
}
