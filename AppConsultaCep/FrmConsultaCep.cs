using Correios;
using System;
using System.Windows.Forms;

namespace AppConsultaCep
{
    public partial class FrmConsultaCep : Form
    {
        public FrmConsultaCep()
        {
            InitializeComponent();
        }

        private void BtnConsultar_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtCep.Text))
            { 
                MessageBox.Show("O CEP não pode ser vazio.");
                return;
            }
            try
            {
                
                CorreiosApi correios = new CorreiosApi();
                var endereco = correios.consultaCEP(txtCep.Text);
                txtEndereco.Text = endereco.end;
                txtComplemento1.Text = endereco.complemento;
                txtComplemento2.Text = endereco.complemento2;
                txtBairro.Text = endereco.bairro;
                txtCidade.Text = endereco.cidade;
                txtUf.Text = endereco.uf;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtCep_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
