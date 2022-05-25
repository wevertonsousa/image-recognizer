using IronOcr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageRecognize.API.Helpers
{
    public class FilterBlocks
    {
        public static string GetName(OcrResult.Block[] blocks)
        {
            try
            {
                var nome = blocks.FirstOrDefault(t => t.Text.ToUpper().Contains("NOME"));
                if (nome != null)
                {
                    if (nome.Text.ToUpper().Split("NOME").Count() > 0)
                    {
                        return RemoveExcept(nome.Text.ToUpper().Split("NOME")[1].Replace("E\n", "").Replace("\n", "").Trim());
                    }
                }

                nome = blocks.FirstOrDefault(t => t.Text.ToUpper().Contains("OME"));
                if (nome != null)
                    return RemoveExcept(nome.Text.ToUpper().Replace("OME", "").Replace("\n", "").Trim());

                for (int i = 0; i < blocks.Count(); i++)
                {
                    if (blocks[i].Text.ToUpper().Contains("IDENTIDADE"))
                    {
                        if (blocks[i - 1].Lines.Count() > 1)
                            return RemoveExcept(blocks[i - 1].Lines[1].Text.ToUpper().Replace("NOME", "").Replace("\n", "").Trim(),true, false,false, false,true,false, false);

                        return RemoveExcept(blocks[i - 1].Text.ToUpper().Replace("NOME", "").Replace("\n", "").Trim());
                    }
                }

                return String.Empty;
            }
            catch
            {
                return String.Empty;
            }
        }

        public static string GetIdentidade(OcrResult.Block[] blocks)
        {
            try
            {
                var identidade = blocks.FirstOrDefault(t => t.Text.ToUpper().Contains("IDENTIDADE"));

                if (identidade != null)
                    return RemoveExcept(identidade.Lines[1].Text.ToUpper().Trim(), false, true);

                return String.Empty;
            }
            catch
            {
                return String.Empty;
            }
        }

        public static string GetCPF(OcrResult.Block[] blocks)
        {
            try
            {

                var identidade = blocks.FirstOrDefault(t => t.Text.ToUpper().Contains("IDENTIDADE"));

                for (int i = 0; i < identidade.Lines.Count(); i++)
                {
                    if (identidade.Lines[i].Text.ToUpper().Contains("CPF"))
                    {
                        var cpf = identidade.Lines[i + 1].Text.Replace("B", "8");
                        cpf = RemoveExcept(cpf, false, true).Substring(0, 11);
                        return cpf;
                    }
                }

                return String.Empty;
            }
            catch
            {
                return String.Empty;
            }
        }
        public static string GetDataNascimento(OcrResult.Block[] blocks)
        {
            try
            {

                var identidade = blocks.FirstOrDefault(t => t.Text.ToUpper().Contains("IDENTIDADE"));

                for (int i = 0; i < identidade.Lines.Count(); i++)
                {
                    if (identidade.Lines[i].Text.ToUpper().Contains("CPF"))
                    {
                        var cpf = identidade.Lines[i + 1].Text.Replace("B", "8");
                        var split = cpf.Split("/");
                        string dataNascimento = split[0].Substring(split[0].Length - 2) + "/" + split[1] + "/" + split[2];
                        return dataNascimento.Replace("'","").Trim();
                    }
                }

                return String.Empty;
            }
            catch
            {
                return String.Empty;
            }
        }

        public static string GetOrgaoEmissor(OcrResult.Block[] blocks)
        {
            try
            {
                var format = blocks.FirstOrDefault(t => t.Text.ToUpper().Contains("IDENTIDADE"));

                if (format != null)
                {
                    string orgaoEmissor = RemoveExcept(format.Lines[1].Text.ToUpper().Trim(), true, false, false, false, false, false, true);
                    var split = orgaoEmissor.Split("/");
                    for (int i = 0; i < split.Count(); i++)
                    {
                        if (split[i].Contains("SSP"))
                            return "SSP";

                        return split[i];
                    }
                }
                return String.Empty;
            }
            catch
            {
                return String.Empty;
            }
        }

        public static string GetUF(OcrResult.Block[] blocks)
        {
            try
            {
                var format = blocks.FirstOrDefault(t => t.Text.ToUpper().Contains("IDENTIDADE"));

                if (format != null)
                {
                    string orgaoEmissor = RemoveExcept(format.Lines[1].Text.ToUpper().Trim(), true, false, false, false, false, false, true);
                    var split = orgaoEmissor.Split("/");
                    for (int i = 0; i < split.Count(); i++)
                    {
                        return split[i+1].Substring(0,2);
                    }
                }
                return String.Empty;
            }
            catch
            {
                return String.Empty;
            }
        }

        public static string GetPai(OcrResult.Block[] blocks)
        {
            try
            {
                for (int i = 0; i < blocks.Count(); i++)
                {
                    var identidade = blocks[i].Text.Contains("FILIA");
                    if (identidade)
                    {
                        string nomePai = "";
                        nomePai = blocks[i].Lines[1].Text;
                        if (blocks[i + 1].Text.Replace("\n","") != string.Empty)
                            if (blocks[i + 1].Text.Replace("\n", "").Length < 15)
                                nomePai += " " + blocks[i + 1].Text.Replace("\n", "");

                        return nomePai;                        
                    }
                }

                
                return String.Empty;
            }
            catch
            {
                return String.Empty;
            }
        }

        public static string GetMae(OcrResult.Block[] blocks)
        {
            try
            {
                for (int i = 0; i < blocks.Count(); i++)
                {
                    var identidade = blocks[i].Text.Contains("FILIA");
                    if (identidade)
                    {
                        string nomeMae = "";
                        if (blocks[i + 1].Text.Replace("\n", "") != string.Empty)
                        {
                            if (blocks[i + 1].Text.Replace("\n", "").Length > 15)
                                nomeMae += " " + blocks[i + 1].Text.Replace("\n", "");
                            else
                            {
                                nomeMae += " " + blocks[i + 2].Text.Replace("\n", "");
                            }
                        }

                        return nomeMae.Trim();
                    }
                }


                return String.Empty;
            }
            catch
            {
                return String.Empty;
            }
        }

        public static string GetValidade(OcrResult.Block[] blocks)
        {
            try
            {
                for (int i = 0; i < blocks.Count(); i++)
                {
                    var validade = blocks[i].Text.Contains("VALIDADE");
                    if (validade)
                    {
                        var words = blocks[i].Words.Where(t => t.Text.Contains("/")).ToList();
                        if (words.Count > 0)
                            return words.FirstOrDefault().Text.Replace("'", "").Trim();
                    }
                }
                return String.Empty;
            }
            catch
            {
                return String.Empty;
            }
        }

        public static string GetPrimeiraHabilitacao(OcrResult.Block[] blocks)
        {
            try
            {
                for (int i = 0; i < blocks.Count(); i++)
                {
                    var validade = blocks[i].Text.Contains("VALIDADE");
                    if (validade)
                    {
                        var words = blocks[i].Words.Where(t => t.Text.Contains("/")).ToList();
                        if (words.Count > 0)
                            return words.LastOrDefault().Text.Replace("'", "").Trim();
                    }
                }
                return String.Empty;
            }
            catch
            {
                return String.Empty;
            }
        }

        public static string GetDataEmissao(OcrResult.Block[] blocks)
        {
            try
            {
                for (int i = 0; i < blocks.Count(); i++)
                {
                    var validade = blocks[i].Text.Contains("EMISSÃO");
                    if (validade)
                    {
                        var words = blocks[i].Words.Where(t => t.Text.Contains("/")).ToList();
                        if (words.Count > 0)
                            return words.FirstOrDefault().Text.Replace("'", "").Trim();
                    }
                }
                return String.Empty;
            }
            catch
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Returns a copy of the original string containing only the set of whitelisted characters.
        /// </summary>
        /// <param name="value">The string that will be copied and scrubbed.</param>
        /// <param name="alphas">If true, all alphabetical characters (a-zA-Z) will be preserved; otherwise, they will be removed.</param>
        /// <param name="numerics">If true, all numeric characters (0-9) will be preserved; otherwise, they will be removed.</param>
        /// <param name="dashes">If true, all dash characters (-) will be preserved; otherwise, they will be removed.</param>
        /// <param name="underlines">If true, all underscore characters (_) will be preserved; otherwise, they will be removed.</param>
        /// <param name="spaces">If true, all whitespace (e.g. spaces, tabs) will be preserved; otherwise, they will be removed.</param>
        /// <param name="periods">If true, all dot characters (".") will be preserved; otherwise, they will be removed.</param>
        public static string RemoveExcept(string value, bool alphas = false, bool numerics = false, bool dashes = false, bool underlines = false, bool spaces = false, bool periods = false, bool bars = false)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;
            if (new[] { alphas, numerics, dashes, underlines, spaces, periods }.All(x => x == false)) return value;

            var whitelistChars = new HashSet<char>(string.Concat(
                alphas ? "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ" : "",
                numerics ? "0123456789" : "",
                dashes ? "-" : "",
                underlines ? "_" : "",
                periods ? "." : "",
                bars ? "/" : "",
                spaces ? " " : ""
            ).ToCharArray());

            var scrubbedValue = value.Aggregate(new StringBuilder(), (sb, @char) =>
            {
                if (whitelistChars.Contains(@char)) sb.Append(@char);
                return sb;
            }).ToString();

            return scrubbedValue;
        }


    }
}
