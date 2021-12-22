using System;
using System.Windows.Forms;

namespace AppConsultaCep
{
    public partial class FrmFrete : Form
    {
        private void CalcularFreteEPrazo()
        {
            var calculaFrete = new CalculaFrete(cbTipoEnvio.SelectedIndex,
                                                txtCepOrigem.Text, txtCepDestino.Text,
                                                txtPeso.Text,
                                                cbFormato.SelectedIndex + 1,
                                                txtComprimento.Text,
                                                txtAltura.Text,
                                                txtLargura.Text,
                                                txtDiametro.Text,
                                                rbSimEmMaos.Checked,
                                                txtValorDeclarado.Text,
                                                rbSimAviso.Checked
                                                );

            calculaFrete.CalcularFreteEPrazoDeEntrega();

            txtValor.Text = calculaFrete.ValorFrete.ToString();
            txtPrazo.Text = calculaFrete.PrazoEntrega.ToString();

        }
        public FrmFrete()
        {
            InitializeComponent();
        }

        private void FrmFrete_Load(object sender, EventArgs e)
        {
            cbTipoEnvio.SelectedIndex = 0;
            cbFormato.SelectedIndex = 0;
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                CalcularFreteEPrazo();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}