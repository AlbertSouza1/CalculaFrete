using EnumsNET;
using System;
using System.Text;
using System.Xml;

namespace AppConsultaCep
{
    internal class CalculaFrete
    {
        public ETipoServico Servico { get; set; }
        public string CepOrigem { get; set; }
        public string CepDestino { get; set; }
        public string Peso { get; set; }
        public EFormato Formato { get; set; }
        public decimal Comprimento { get; set; }
        public decimal Altura { get; set; }
        public decimal Largura { get; set; }
        public decimal Diametro { get; set; }
        public string MaoPropria { get; set; }
        public decimal ValorDeclarado { get; set; }
        public string AvisoRecebimento { get; set; }
        public string Retorno { get => "xml"; }
        public string IndicaCalculo { get => "3";}
        public decimal ValorFrete { get; set; }
        public int PrazoEntrega { get; set; }

        public CalculaFrete(int servico, string cepOrigem, string cepDestino, string peso, int formato, string comprimento, string altura, string largura, string diametro, bool maoPropria, string valorDeclarado, bool avisoRecebimento)
        {
            if (string.IsNullOrEmpty(cepOrigem))
            {
                throw new ArgumentException($"CEP origem não pode ser nulo nem vazio.", nameof(cepOrigem));
            }

            if (string.IsNullOrEmpty(cepDestino))
            {
                throw new ArgumentException($"CEP destino não pode ser nulo nem vazio.", nameof(cepDestino));
            }

            if (string.IsNullOrEmpty(peso) || !decimal.TryParse(peso, out _) || decimal.Parse(peso) < 1)
            {
                throw new ArgumentException($"O peso mínimo é de 1Kg.", nameof(peso));
            }

            if (string.IsNullOrEmpty(comprimento) || !decimal.TryParse(comprimento, out _) || decimal.Parse(comprimento) < 15)
            {
                throw new ArgumentException($"O comprimento mínimo é de 15cm.", nameof(comprimento));
            }

            if (string.IsNullOrEmpty(altura) || !decimal.TryParse(altura, out _) || decimal.Parse(altura) < 1)
            {
                throw new ArgumentException($"O comprimento mínimo é de 15cm.", nameof(altura));
            }

            if (string.IsNullOrEmpty(largura) || !decimal.TryParse(largura, out _) || decimal.Parse(largura) < 10)
            {
                throw new ArgumentException($"O comprimento mínimo é de 15cm.", nameof(largura));
            }

            if (string.IsNullOrEmpty(diametro) || !decimal.TryParse(diametro, out _) || decimal.Parse(diametro) < 5)
            {
                throw new ArgumentException($"O comprimento mínimo é de 15cm.", nameof(diametro));
            }

            if (diametro.Length > 0 && !decimal.TryParse(diametro, out _))
            {
                throw new ArgumentException($"O valor declarado deve ser indicado como 0 caso não haja.", nameof(diametro));
            }

            Servico = (ETipoServico)servico;
            CepOrigem = cepOrigem;
            CepDestino = cepDestino;
            Peso = peso;
            Formato = (EFormato)formato;
            Comprimento = decimal.Parse(comprimento);
            Altura = decimal.Parse(altura);
            Largura = decimal.Parse(largura);
            Diametro = decimal.Parse(diametro);
            MaoPropria = maoPropria ? "s" : "n";
            ValorDeclarado = string.IsNullOrEmpty(valorDeclarado) ? 0 : decimal.Parse(valorDeclarado);
            AvisoRecebimento = avisoRecebimento ? "s" : "n";
        }

        public void CalcularFreteEPrazoDeEntrega()
        {
            string url = GerarUrl();

            XmlDocument doc = new XmlDocument();
            doc.Load(url);

            XmlNodeList frete = doc.GetElementsByTagName("Valor");
            XmlNodeList prazoEntrega = doc.GetElementsByTagName("PrazoEntrega");

            ValorFrete = decimal.Parse(frete.Item(0).InnerText);
            PrazoEntrega = int.Parse(prazoEntrega.Item(0).InnerText);          
        }

        public string GerarUrl()
        {
            var strCaminho = new StringBuilder();

            strCaminho.Append(@"http://ws.correios.com.br/calculador/CalcPrecoPrazo.aspx?");
            strCaminho.Append("nCdEmpresa=");
            strCaminho.Append("&sDsSenha=");
            strCaminho.Append($"&nCdServico="+ Servico.AsString(EnumFormat.Description));
            strCaminho.Append("&sCepOrigem=" + CepOrigem);
            strCaminho.Append("&sCepDestino=" + CepDestino);
            strCaminho.Append("&nVlPeso=" + Peso);
            strCaminho.Append($"&nCdFormato="+(int)Formato);
            strCaminho.Append("&nVlComprimento=" + Comprimento);
            strCaminho.Append("&nVlAltura=" + Altura);
            strCaminho.Append("&nVlLargura=" + Largura);
            strCaminho.Append("&nVlDiametro=" + Diametro);
            strCaminho.Append($"&sCdMaoPropria=" + MaoPropria);
            strCaminho.Append("&nVlValorDeclarado=" + ValorDeclarado);
            strCaminho.Append($"&sCdAvisoRecebimento=" + AvisoRecebimento);
            strCaminho.Append($"&StrRetorno="+Retorno);
            strCaminho.Append($"&nIndicaCalculo="+IndicaCalculo);

            return strCaminho.ToString();
        }
    }
}
