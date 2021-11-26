using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnitTestExample.Controllers;

namespace UnitTestExample
{
    public partial class Form1 : Form
    {
        private AccountController _controller = new AccountController();

        private bool passwordCheckPassed;
        public bool PasswordCheckPassed
        {
            get { return passwordCheckPassed; }
            set 
            { 
                passwordCheckPassed = value;
                btnRegister.Enabled = passwordCheckPassed;
                if (passwordCheckPassed)
                    txtPasswordConfirm.BackColor = Color.White;
                else
                    txtPasswordConfirm.BackColor = Color.Fuchsia;
            }
        }

        public Form1()
        {
            InitializeComponent();
            PasswordCheckPassed = true;
            dgwAccounts.DataSource = _controller.AccountManager.Accounts;         
        }

        private void OnPasswordTextChanged(object sender, EventArgs e)
        {
            PasswordCheckPassed = txtPassword.Text.Equals(txtPasswordConfirm.Text);
        }

        private void OnRegisterClick(object sender, EventArgs e)
        {
            try
            {
                _controller.Register(
                    txtEmail.Text,
                    txtPassword.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public bool ValidatePassword(string password)
        {
            var LowerCase = new Regex(@"[a-z]+");
            var UpperCase = new Regex(@"[A-Z]+");
            var Number = new Regex(@"[0-9]+");
            var EightLong = new Regex(@".{8,}+");
            return LowerCase.IsMatch(password) && UpperCase.IsMatch(password) && Number.IsMatch(password) && EightLong.IsMatch(password);
        }
    }
}
